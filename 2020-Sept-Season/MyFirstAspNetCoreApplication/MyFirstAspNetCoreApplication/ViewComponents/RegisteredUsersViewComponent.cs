using Microsoft.AspNetCore.Mvc;
using MyFirstAspNetCoreApplication.Data;
using MyFirstAspNetCoreApplication.ViewModels.ViewComponents;
using System.Linq;

namespace MyFirstAspNetCoreApplication.ViewComponents
{
    public class RegisteredUsersViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext dbContext;

        public RegisteredUsersViewComponent(ApplicationDbContext dbContext)
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
