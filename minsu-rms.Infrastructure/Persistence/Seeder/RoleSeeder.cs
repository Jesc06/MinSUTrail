using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordManagementSystem.Infrastructure.Persistence.Data;
using Microsoft.Extensions.Configuration;

namespace RecordManagementSystem.Infrastructure.Persistence.Seeder
{
    public class RoleSeeder
    {
        public static async Task Roles(RoleManager<IdentityRole> roleManager, UserManager<UserIdentity> userManager, IConfiguration configuration)
        {
            string[] roles = { "Admin", "Student" };

            foreach(var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var AppSettingsAdminSeeded = configuration.GetSection("AdminSeededAccount");
            var adminEmail = AppSettingsAdminSeeded["Email"];
            var adminPassword = AppSettingsAdminSeeded["Password"];

            var findUser = await userManager.FindByEmailAsync(adminEmail);
            if(findUser == null)
            {
                var createUser = new UserIdentity
                {
                    FirstName = AppSettingsAdminSeeded["FirstName"],
                    MiddleName = AppSettingsAdminSeeded["MiddleName"],
                    LastName = AppSettingsAdminSeeded["LastName"],
                    UserName = adminEmail,
                    Email = adminEmail,
                };

                var registerAccount = await userManager.CreateAsync(createUser,adminPassword);
                if (registerAccount.Succeeded)
                {
                    await userManager.AddToRoleAsync(createUser, "Admin");
                }

            }

        }

    }
}
