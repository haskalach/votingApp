using System.Collections.Generic;
using System.Linq;
using Application.API.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Application.API.Data {
    public class Seed {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public Seed (UserManager<User> userManager, RoleManager<Role> roleManager) {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void SeedUsers () {
            if (!_userManager.Users.Any ()) {
                var userData = System.IO.File.ReadAllText ("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>> (userData);

                var roles = new List<Role> {

                    new Role { Name = "Admin" },
                    new Role { Name = "OrganizationAdmin" },
                    new Role { Name = "LaptopUser" },
                    new Role { Name = "MobileUser" },
                    new Role { Name = "Reference" }
                };

                foreach (var role in roles) {
                    _roleManager.CreateAsync (role).Wait ();
                }
                foreach (var user in users) {

                    _userManager.CreateAsync (user, "password").Wait ();
                    _userManager.AddToRoleAsync (user, "LaptopUser").Wait ();
                }

                var adminUser = new User {
                    UserName = "Admin",
                    Name = "Admin"
                };

                IdentityResult result = _userManager.CreateAsync (adminUser, "password").Result;
                if (result.Succeeded) {
                    var admin = _userManager.FindByNameAsync ("Admin").Result;
                    _userManager.AddToRolesAsync (admin, new [] { "Admin" }).Wait ();
                }
            }

        }

    }
}