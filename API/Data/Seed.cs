using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var roles = new List<AppRole>
            {
                new AppRole{Name = "Student"},
                new AppRole{Name = "Teacher"},
                new AppRole{Name = "Admin"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var admin = new AppUser
            {
                UserName = "admin",
                GenderId = 1
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin" });

            var teacher = new AppUser
            {
                UserName = "LongNG",
                GenderId = 1
            };

            await userManager.CreateAsync(teacher, "Pa$$w0rd");
            await userManager.AddToRolesAsync(teacher, new[] { "Teacher" });

            var student = new AppUser
            {
                UserName = "202008-00002",
                GenderId = 1
            };

            await userManager.CreateAsync(student, "Pa$$w0rd");
            await userManager.AddToRolesAsync(student, new[] { "Student" });
        }

        public static async Task SeedGender(DataContext context)
        {
            if (await context.Genders.AnyAsync()) return;

            var genderData = await File.ReadAllTextAsync("Data/DatabaseDataSeed/GenderSeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var genders = JsonSerializer.Deserialize<List<Gender>>(genderData);

            foreach (var gender in genders)
            {
                context.Genders.Add(gender);
            }

            await context.SaveChangesAsync();
        }
    }
}
