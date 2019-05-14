using System;
using AspNetCoreTodo.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreTodo
{
    public static class SeedData
    {
        public static async Task InitializeAsync(
            IServiceProvider services)
        {
            var roleManager = services.
                GetRequiredService<RoleManager<IdentityRole>>();
            await EnsureRolesAsync(roleManager);

            var userManager = services.
                GetRequiredService<UserManager<IdentityUser>>();

            await EnsureTestAdminAsync(userManager);
        }


        private static async Task EnsureRolesAsync(
            RoleManager<IdentityRole> roleManager)
        {
            var alreadyExists = await roleManager
                .RoleExistsAsync(Constants.AdministratorRole);

            if (alreadyExists) return;

            await roleManager.CreateAsync(
                new IdentityRole(Constants.AdministratorRole));
        }

        private static async Task EnsureTestAdminAsync(
            UserManager<IdentityUser> userManager)
        {
            bool flag = false;
            await userManager.Users.ForEachAsync(x =>
            {
                if (x.UserName == "admin.todo.local")
                {
                    flag = true;
                }
            });
            if (flag) return;
            
            var testAdmin = new IdentityUser
            {
                UserName = "admin@todo.local",
                Email = "admin@todo.local"
            };
            await userManager.CreateAsync(testAdmin, "Password7!");
            await userManager.AddToRoleAsync(testAdmin, Constants.AdministratorRole);

        }
    }
}
