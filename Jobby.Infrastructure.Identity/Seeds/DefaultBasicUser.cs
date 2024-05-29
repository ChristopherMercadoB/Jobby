using Jobby.Core.Application.Enums;
using Jobby.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Jobby.Infrastructure.Identity.Seeds
{
    public static class DefaultBasicUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser defaultUser = new()
            {
                FirstName = "Christopher",
                LastName = "Mercado",
                UserName = "Chris",
                Email = "basicuser@email.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(e=> e.Id != defaultUser.Id))
            {
                var result = await userManager.FindByEmailAsync(defaultUser.Email);
                if (result == null)
                {
                    await userManager.CreateAsync(defaultUser, "Pa$$word123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Enterprise.ToString());
                }
            }
        }
    }
}
