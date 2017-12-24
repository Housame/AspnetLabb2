using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RolesAndClaims.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        [Route("getusers")]
        public IActionResult GetAllUsers()
        {
            var users = userMngr.Users.OrderBy(c => c.FirstName);
            return Ok(users);
        }
        [HttpGet]
        [Route("reset")]
        public async Task<IActionResult> ResetUsers()
        {
            //await AddRolesToIdentity();

            context.RemoveRange(userMngr.Users);
            context.SaveChanges();

            var users = new List<StaticUser>
            {
                new StaticUser { FirstName = "Adam", Email = "adam@gmail.com", Role = "Administrator",Age=null},
                new StaticUser { FirstName = "Peter", Email = "peter@gmail.com", Role = "Publisher",Age=null},
                new StaticUser { FirstName = "Susan", Email = "susan@gmail.com", Role = "Subscriber", Age = 48 },
                new StaticUser { FirstName = "Viktor", Email = "viktor@gmail.com", Role = "Subscriber", Age = 15 },
                new StaticUser { FirstName = "Xerxes", Email = "xerxes@gmail.com", Age=null, Role = null}
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
                    switch (user.Role)
                    {
                        case "Administrator":
                            await userMngr.AddClaimAsync(newUser, new Claim("News", "Admin", "PublishSport", "PublishEconomy", "AllPublisher"));
                            break;
                        case "Subscriber":
                            if (user.Age >= 20 || user.Age == null)
                            {
                                await userMngr.AddClaimAsync(newUser, new Claim("News", "Adult"));
                            }
                            break;
                        case "Publisher":
                            switch (user.Email)
                            {
                                case "peter@gmail.com":
                                    await userMngr.AddClaimAsync(newUser, new Claim("News", "PublishSport"));
                                    await userMngr.AddClaimAsync(newUser, new Claim("News", "PublishEconomy"));
                                    break;
                            }
                            await userMngr.AddClaimAsync(newUser, new Claim("News", "AllPublisher"));
                            break;
                    }
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
            return Ok(user);
        }
        [HttpGet, Route("usersnclaims")]
        public async Task<IActionResult> GetAllUsersWithClaims()
        {
            var listOfUserWithClaims = new List<UserVm>();
            foreach (var user in userMngr.Users.ToList())
            {
                listOfUserWithClaims.Add(new UserVm
                {
                    User = user,
                    Claims = await userMngr.GetClaimsAsync(user)
                });
            }
            return Ok(listOfUserWithClaims);
        }
        [HttpGet, Route("open")]
        public IActionResult Open()
        {
            return Ok("Open");
        }

        [Authorize(Policy = "HiddenNews")]
        [HttpGet, Route("hidden")]
        public IActionResult HiddenNews()
        {
            return Ok("Hidden");
        }

        [Authorize(Policy = "AgeRequirement")]
        [Authorize(Policy = "HiddenNews")]
        [HttpGet, Route("age")]
        public IActionResult Age()
        {
            return Ok("Age");
        }

        [Authorize(Policy = "SportsRequirement")]
        [HttpGet, Route("sport")]
        public IActionResult Sport()
        {
            return Ok("Sport");
        }

        [Authorize(Policy = "CultureRequirement")]
        [HttpGet, Route("culture")]
        public IActionResult Culture()
        {
            return Ok("Culture");
        }

    }
}
