using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PROD_VS.Models;
using System;
using System.Linq;
namespace PROD_VS.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Users.Any(u => u.UserName == "Aliaksandr"))
                {
                    return; // База данных уже инициализирована
                }

                var developer = new User
                {
                    UserName = "Aliaksandr",
                    Email = "malashkevich.aliaksandr@gmail.com",
                    AccessLevel = "Full",
                    IsDeveloper = true,
                    LastName = "Malashkevich"
                };

                var passwordHasher = new PasswordHasher<User>();
                developer.PasswordHash = passwordHasher.HashPassword(developer, "LGE2922061");

                context.Users.Add(developer);
                context.SaveChanges();
            }
        }
    }
}
