using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RolesAndClaims.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolesAndClaims.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        static readonly string[] roles = { "Administrator", "Publisher", "Subscriber" };

        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        RoleManager<UserRole> _roleManager;
        DatabaseContext context;

        public UserController(UserManager<User> _userManager, SignInManager<User> _signInManager, RoleManager<UserRole> _roleManager, DatabaseContext context)
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._roleManager = _roleManager;
            this.context = context;
        }

        internal async Task<IActionResult> AddRolesToIdentity()
        {
            foreach (var role in roles)
            {
                var roleExists = await _roleManager.RoleExistsAsync(role);

                if (!roleExists)
                {
                    var newRole = new UserRole();
                    newRole.Name = role;
                    await _roleManager.CreateAsync(newRole);
                }
            }
            return Ok();
        }

        [HttpGet]
        [Route("add")]
        public async Task<IActionResult> ClearDbAndRetrieveStaticUsers()
        {
            //await AddRolesToIdentity();
            context.RemoveRange(_userManager.Users);

            var users = new List<StaticUser>
            {
                new StaticUser { FirstName = "Adam", Email = "adam@gmail.com", Role = "Administrator" },
                new StaticUser { FirstName = "Peter", Email = "peter@gmail.com", Role = "Publisher" },
                new StaticUser { FirstName = "Susan", Email = "susan@gmail.com", Role = "Subscriber", Age = 48 },
                new StaticUser { FirstName = "Viktor", Email = "viktor@gmail.com", Role = "Subscriber", Age = 15 },
                new StaticUser { FirstName = "Xerxes", Email = "xerxes@gmail.com", Role = null},
            };

            foreach (var user in users)
            {
                var newUser = new User
                {
                    UserName = user.Email,
                    Email = user.Email,
                    Age = user.Age,
                    FirstName = user.FirstName,
                };

                var result = await _userManager.CreateAsync(newUser);

                if (result.Succeeded)
                {
                    if (user.Role != null)
                        await _userManager.AddToRoleAsync(newUser, user.Role);
                }
            }

            return Ok(_userManager.Users);
        }

    }
}
