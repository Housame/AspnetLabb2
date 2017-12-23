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

        UserManager<User> userMngr;
        SignInManager<User> signInMngr;
        RoleManager<UserRole> roleMngr;
        DatabaseContext context;

        public UserController(UserManager<User> userMngr, SignInManager<User> signInMngr, RoleManager<UserRole> roleMngr, DatabaseContext context)
        {
            this.userMngr = userMngr;
            this.signInMngr = signInMngr;
            this.roleMngr = roleMngr;
            this.context = context;
        }

        internal async Task<IActionResult> AddRolesToIdentity()
        {
            foreach (var role in roles)
            {
                var roleExists = await roleMngr.RoleExistsAsync(role);

                if (!roleExists)
                {
                    var newRole = new UserRole();
                    newRole.Name = role;
                    await roleMngr.CreateAsync(newRole);
                }
            }
            return Ok();
        }

        [HttpGet]
        [Route("add")]
        public async Task<IActionResult> ClearDbAndRetrieveStaticUsers()
        {
            //await AddRolesToIdentity();
            context.RemoveRange(userMngr.Users);

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

                var result = await userMngr.CreateAsync(newUser);

                if (result.Succeeded)
                {
                    if (user.Role != null)
                        await userMngr.AddToRoleAsync(newUser, user.Role);
                }
            }

            return Ok(userMngr.Users);
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(string email)
        {
            var user = await userMngr.FindByNameAsync(email);
            await signInMngr.SignInAsync(user, false);
            return Ok();
        }

    }
}
