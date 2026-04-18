using MeteorCRM.API.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace MeteorCRM.API.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAndAdminAsync(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            // Seed Roles
            string[] roles = { "Admin", "Manager", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Seed Admin User
            var adminEmail = "admin@meteorcrm.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    FirstName = "Meteor",
                    LastName = "Admin",
                    Email = adminEmail,
                    UserName = adminEmail,
                    IsActive = true,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@Meteor123");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}