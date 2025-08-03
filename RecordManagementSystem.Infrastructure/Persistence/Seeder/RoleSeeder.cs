using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordManagementSystem.Infrastructure.Persistence.Data;

namespace RecordManagementSystem.Infrastructure.Persistence.Seeder
{
    public class RoleSeeder
    {
        public static async Task Roles(RoleManager<IdentityRole> roleManager, UserManager<UserIdentity> userManager)
        {
            string[] roles = { "Admin", "Student" };

            foreach(var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var adminEmail = "admin@gmail.com";
            var adminPassword = "Admin@Torech_a&93";

            var findUser = await userManager.FindByEmailAsync(adminEmail);
            if(findUser == null)
            {
                var createUser = new UserIdentity
                {
                    FirstName = "Admin",
                    MiddleName = "Faculty",
                    LastName = "Minsu",
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
