using Jobby.Core.Application.Enums;
using Jobby.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobby.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Enterprise.ToString()));
        }
    }
}
