using MeteorCRM.API.Core.DTOs;

namespace MeteorCRM.API.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto);
        Task<AuthResponseDTO> LoginAsync(LoginDTO dto);
        Task<AuthResponseDTO> GoogleLoginAsync(GoogleAuthDTO dto);
        Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenDTO dto);
        Task RevokeTokenAsync(string token);
    }
}