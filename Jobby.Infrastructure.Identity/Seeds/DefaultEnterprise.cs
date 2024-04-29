﻿using Jobby.Core.Application.Enums;
using Jobby.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Jobby.Infrastructure.Identity.Seeds
{
    public static class DefaultEnterprise
    {
        public static async void SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser defaultUser = new()
            {
                FirstName = "Encima de todo",
                LastName = "",
                UserName = "EDT",
                Email = "edt@email.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.Any(e=> e.Id == defaultUser.Id))
            {
                if (userManager.FindByEmailAsync(defaultUser.Email) == null)
                {
                    await userManager.CreateAsync(defaultUser);
                    await userManager.AddPasswordAsync(defaultUser, "Pa$$word123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Enterprise.ToString());
                }
            }
        }
    }
}