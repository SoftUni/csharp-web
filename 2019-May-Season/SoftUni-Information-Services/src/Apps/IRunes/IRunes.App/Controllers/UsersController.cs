using System.Security.Cryptography;
using System.Text;
using IRunes.Models;
using IRunes.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Action;
using SIS.MvcFramework.Result;

namespace IRunes.App.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [NonAction]
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return Encoding.UTF8.GetString(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        public ActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            User userFromDb = this.userService.GetUserByUsernameAndPassword(username, this.HashPassword(password));

            if (userFromDb == null)
            {
                return this.Redirect("/Users/Login");
            }

            this.SignIn(userFromDb.Id, userFromDb.Username, userFromDb.Email);

            return this.Redirect("/");
        }

        public ActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Register(string username, string password, string confirmPassword, string email)
        {
            if (password != confirmPassword)
            {
                return this.Redirect("/Users/Register");
            }

            User user = new User
            {
                Username = username,
                Password = this.HashPassword(password),
                Email = email
            };

            this.userService.CreateUser(user);

            return this.Redirect("/Users/Login");
        }

        public ActionResult Logout()
        {
            this.SignOut();

            return this.Redirect("/");
        }
    }
}
