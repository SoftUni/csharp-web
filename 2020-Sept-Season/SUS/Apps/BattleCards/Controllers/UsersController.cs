using BattleCards.Services;
using BattleCards.ViewModels.Users;
using SUS.HTTP;
using SUS.MvcFramework;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BattleCards.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        // GET /users/login
        public HttpResponse Login()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            var userId = this.usersService.GetUserId(username, password);
            if (userId == null)
            {
                return this.Error("Invalid username or password");
            }

            this.SignIn(userId);
            return this.Redirect("/Cards/All");
        }

        // GET /users/register
        public HttpResponse Register()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterInputModel input)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            if (input.Username == null || input.Username.Length < 5 || input.Username.Length > 20)
            {
                return this.Error("Invalid username. The username should be between 5 and 20 characters.");
            }

            if (!Regex.IsMatch(input.Username, @"^[a-zA-Z0-9\.]+$"))
            {
                return this.Error("Invalid username. Only alphanumeric characters are allowed.");
            }

            if (string.IsNullOrWhiteSpace(input.Email) || !new EmailAddressAttribute().IsValid(input.Email))
            {
                return this.Error("Invalid email.");
            }

            if (input.Password == null || input.Password.Length < 6 || input.Password.Length > 20)
            {
                return this.Error("Invalid password. The password should be between 6 and 20 characters.");
            }

            if (input.Password != input.ConfirmPassword)
            {
                return this.Error("Passwords should be the same.");
            }

            if (!this.usersService.IsUsernameAvailable(input.Username))
            {
                return this.Error("Username already taken.");
            }

            if (!this.usersService.IsEmailAvailable(input.Email))
            {
                return this.Error("Email already taken.");
            }

            this.usersService.CreateUser(input.Username, input.Email, input.Password);
            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Error("Only logged-in users can logout.");
            }

            this.SignOut();
            return this.Redirect("/");
        }
    }
}
