using Microsoft.AspNetCore.Mvc;
using MyFirstMvcApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstMvcApp.ViewComponents
{
    public class LatestUserViewComponent : ViewComponent
    {
        private readonly IUsersService usersService;

        public LatestUserViewComponent(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IViewComponentResult Invoke(string text)
        {
            return this.View(new LatestUserViewComponentViewModel
            {
                Text = text,
                Username = usersService.LatestUsername()
            });
        }
    }

    public class LatestUserViewComponentViewModel
    {
        public string Text { get; set; }

        public string Username { get; set; }  
    }
}
