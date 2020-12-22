using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SocialNetwork.Domain.Core;

namespace SocialNetwork
{
    public static class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager,
                                                 RoleManager<IdentityRole> roleManager,
                                                 IConfiguration configuration)
        {
            string adminEmail = configuration["adminEmail"];
            string password = configuration["adminEmailPassword"];
            string name = "Босс";
            string surname = "ЭтойкОчалки";

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("moderator") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("moderator"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    Name = name,
                    Surname = surname,
                    Gender = Gender.Male
                };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                    await userManager.AddToRoleAsync(admin, "moderator");
                }
            }
        }
    }
}