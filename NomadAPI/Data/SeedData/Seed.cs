﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NomadAPI.Entities;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NomadAPI.Data.SeedData
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync())
                return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/SeedData/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            if (users == null)
                return;

            var roles = new List<AppRole>
            {
                new AppRole{Name="Nomad"},
                new AppRole{Name="Admin"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.Email = user.Email.ToLower();
                user.UserName = user.Email.ToLower();

                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Nomad");
            }

            var admin = new AppUser
            {
                Email = "admin@gmail.com",
                UserName = "admin@gmail.com",
                FirstName = "admin",
                LastName = "admin"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");

            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Nomad" });
        }
    }
}
