using Microsoft.AspNetCore.Mvc;
using MyFirstAspNetCoreApplication.Data;
using MyFirstAspNetCoreApplication.Service;
using MyFirstAspNetCoreApplication.ViewModels.ViewComponents;
using System.Linq;

namespace MyFirstAspNetCoreApplication.ViewComponents
{
    public class RegisteredUsersViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext dbContext;

        public RegisteredUsersViewComponent(
            ApplicationDbContext dbContext,
            IInstanceCounter instanceCounter)
        {
            this.dbContext = dbContext;
        }

        public IViewComponentResult Invoke(string title)
        {
            var users = this.dbContext.Users.Count();
            var viewModel = new RegisteredUsersViewModel
            {
                Title = title,
                Users = users,
            };
            
            return this.View(viewModel);
        }
    }
}
