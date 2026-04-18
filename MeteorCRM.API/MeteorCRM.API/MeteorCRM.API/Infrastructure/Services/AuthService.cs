using Google.Apis.Auth;
using MeteorCRM.API.Core.DTOs;
using MeteorCRM.API.Core.Entities;
using MeteorCRM.API.Core.Interfaces;
using MeteorCRM.API.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MeteorCRM.API.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly MeteorDbContext _context;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            MeteorDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                throw new Exception("Passwords do not match.");

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("Email already exists.");

            var user = new ApplicationUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            // Assign Role
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            await _userManager.AddToRoleAsync(user, "User");

            return await GenerateAuthResponse(user);
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !user.IsActive)
                throw new Exception("Invalid credentials.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordValid)
                throw new Exception("Invalid credentials.");

            var isLockedOut = await _userManager.IsLockedOutAsync(user);
            if (isLockedOut)
                throw new Exception("Account is locked. Try again later.");

            await _userManager.ResetAccessFailedCountAsync(user);

            return await GenerateAuthResponse(user);
        }

        public async Task<AuthResponseDTO> GoogleLoginAsync(GoogleAuthDTO dto)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _configuration["GoogleAuth:ClientId"] }
            };

            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken, settings);
            }
            catch
            {
                throw new Exception("Invalid Google token.");
            }

            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    FirstName = payload.GivenName ?? "",
                    LastName = payload.FamilyName ?? "",
                    Email = payload.Email,
                    UserName = payload.Email,
                    ProfilePicture = payload.Picture,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                    throw new Exception("Failed to create user.");

                if (!await _roleManager.RoleExistsAsync("User"))
                    await _roleManager.CreateAsync(new IdentityRole("User"));

                await _userManager.AddToRoleAsync(user, "User");
            }

            return await GenerateAuthResponse(user);
        }

        public async Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenDTO dto)
        {
            var refreshToken = await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == dto.RefreshToken && !r.IsRevoked);

            if (refreshToken == null || refreshToken.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Invalid or expired refresh token.");

            // Revoke old token
            refreshToken.IsRevoked = true;

            var newResponse = await GenerateAuthResponse(refreshToken.User);

            // Update replaced by
            refreshToken.ReplacedByToken = newResponse.RefreshToken;
            await _context.SaveChangesAsync();

            return newResponse;
        }

        public async Task RevokeTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(r => r.Token == token);

            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                await _context.SaveChangesAsync();
            }
        }

        private async Task<AuthResponseDTO> GenerateAuthResponse(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";

            var token = GenerateJwtToken(user, role);
            var refreshToken = GenerateRefreshToken();

            // Save refresh token
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(
                    int.Parse(_configuration["JwtSettings:RefreshTokenExpiryInDays"]!))
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            var expiryMinutes = int.Parse(_configuration["JwtSettings:ExpiryInMinutes"]!);

            return new AuthResponseDTO
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                Role = role
            };
        }

        private string GenerateJwtToken(ApplicationUser user, string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(jwtSettings["ExpiryInMinutes"]!)),
                signingCredentials: new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}