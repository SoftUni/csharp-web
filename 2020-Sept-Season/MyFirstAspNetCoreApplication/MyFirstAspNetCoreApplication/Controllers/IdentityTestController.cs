using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyFirstAspNetCoreApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyFirstAspNetCoreApplication.Controllers
{
    public class IdentityTestController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public IdentityTestController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Create()
        {
            var user = new ApplicationUser
            {
                Email = "pesho@abv.bg",
                UserName = "pesho@abv.bg",
                EmailConfirmed = true,
                PhoneNumber = "987654321",
            };

            var result = await this.userManager.CreateAsync(user, "123456");
            return this.Json(result);
        }

        public async Task<IActionResult> Login()
        {
            var result = await this.signInManager.PasswordSignInAsync(
                "pesho@abv.bg", "123456", true, true);
            return this.Json(result);
        }

        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return this.Redirect("/");
        }

        public async Task<IActionResult> AddUserToAdmin()
        {
            if (!await this.roleManager.RoleExistsAsync("Admin"))
            {
                await this.roleManager.CreateAsync(new IdentityRole
                {
                    Name = "Admin",
                });
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var result = await this.userManager.AddToRoleAsync(user, "Admin");
            return this.Json(result);
        }

        [Authorize(Roles = "Admin, Teacher, Student")]
        public async Task<IActionResult> WhoAmI()
        {
            if (this.User.IsInRole("Admin"))
            {
                // ...
            }

            var user = await this.userManager.GetUserAsync(this.User);
            return this.Json(user);
        }

        public async Task<IActionResult> AddClaim()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var result = await this.userManager.AddClaimAsync(user, new Claim("state", "GA"));
            return this.Json(result);
        }

        [Authorize(Policy = "AdminUserWithState")]
        public IActionResult GetClaim()
        {
            var claim = this.User.FindFirst("state");
            return this.Json(claim.Value);
        }
    }
}
