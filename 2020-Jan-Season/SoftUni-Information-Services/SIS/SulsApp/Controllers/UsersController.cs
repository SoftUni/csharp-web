using SIS.HTTP;
using SIS.HTTP.Logging;
using SIS.HTTP.Response;
using SIS.MvcFramework;
using SulsApp.Models;
using SulsApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;

namespace SulsApp.Controllers
{
    public class UsersController : Controller
    {
        private IUsersService usersService;
        private ILogger logger;

        public UsersController(IUsersService usersService, ILogger logger)
        {
            this.usersService = usersService;
            this.logger = logger;
        }

        public HttpResponse Login()
        {
            return this.View();
        }

        [HttpPost("/Users/Login")]
        public HttpResponse DoLogin()
        {
            string username = this.Request.FormData["username"];
            string password = this.Request.FormData["password"];

            var userId = this.usersService.GetUserId(username, password);
            if (userId == null)
            {
                return this.Redirect("/Users/Login");
            }

            this.SignIn(userId);
            this.logger.Log("User logged in: " + username);
            return this.Redirect("/");
        }

        public HttpResponse Register()
        {
            return this.View();
        }

        [HttpPost("/Users/Register")]
        public HttpResponse DoRegister()
        {
            var username = this.Request.FormData["username"];
            var email = this.Request.FormData["email"];
            var password = this.Request.FormData["password"];
            var confirmPassword = this.Request.FormData["confirmPassword"];

            if (password != confirmPassword)
            {
                return this.Error("Passwords should be the same!");
            }

            if (username?.Length < 5 || username?.Length > 20)
            {
                return this.Error("Username should be between 5 and 20 characters .");
            }

            if (password?.Length < 6 || password?.Length > 20)
            {
                return this.Error("Password should be between 6 and 20 characters.");
            }

            if (!IsValid(email))
            {
                return this.Error("Invalid email!");
            }

            this.usersService.CreateUser(username, email, password);
            this.logger.Log("New user: " + username);
            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            this.SignOut();
            return this.Redirect("/");
        }

        private bool IsValid(string emailaddress)
        {
            try
            {
                new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
