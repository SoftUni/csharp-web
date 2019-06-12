using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Musaca.App.ViewModels.Orders;
using Musaca.Models;
using Musaca.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Action;
using SIS.MvcFramework.Mapping;
using SIS.MvcFramework.Result;

namespace Musaca.App.Controllers
{
    using BindingModels.Users;

    public class UsersController : Controller
    {
        private readonly IUserService userService;

        private readonly IOrderService orderService;

        public UsersController(IUserService userService, IOrderService orderService)
        {
            this.userService = userService;
            this.orderService = orderService;
        }

        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginBindingModel model)
        {
            User userFromDb = this.userService.GetUserByUsernameAndPassword(model.Username, this.HashPassword(model.Password));

            if (userFromDb == null)
            {
                return this.Redirect("/Users/Login");
            }

            this.SignIn(userFromDb.Id, userFromDb.Username, userFromDb.Email);

            return this.Redirect("/");
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Users/Register");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.Redirect("/Users/Register");
            }

            User user = new User
            {
                Username = model.Username,
                Password = this.HashPassword(model.Password),
                Email = model.Email
            };

            this.userService.CreateUser(user);
            this.orderService.CreateOrder(new Order {CashierId = user.Id});

            return this.Redirect("/Users/Login");
        }

        public IActionResult Profile()
        {
            UserProfileViewModel userProfileViewModel = new UserProfileViewModel();
            List<Order> ordersFromDb = this.orderService.GetAllCompletedOrdersByCashierId(this.User.Id);

            userProfileViewModel.Orders = ordersFromDb 
                .To<OrderProfileViewModel>()
                .ToList();

            foreach (var order in userProfileViewModel.Orders)
            {
                order.CashierName = this.User.Username;
                order.Total = ordersFromDb.Where(orderF => orderF.Id == order.Id)
                    .SelectMany(orderF => orderF.Products)
                    .Sum(pr => pr.Product.Price).ToString();
                order.IssuedOnDate = ordersFromDb.SingleOrDefault(orderF => orderF.Id == order.Id).IssuedOn
                    .ToString("dd/MM/yyyy");
            }

            return this.View(userProfileViewModel);
        }

        public IActionResult Logout()
        {
            this.SignOut();

            return this.Redirect("/");
        }

        [NonAction]
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return Encoding.UTF8.GetString(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }
    }
}
