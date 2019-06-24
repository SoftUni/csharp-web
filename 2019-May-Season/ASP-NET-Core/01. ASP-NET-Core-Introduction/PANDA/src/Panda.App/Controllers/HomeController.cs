using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panda.App.Models.Package;
using Panda.Data;
using Panda.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Panda.App.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
        private readonly PandaDbContext context;

        public HomeController(PandaDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            if(this.User.Identity.IsAuthenticated)
            {
                List<PackageHomeViewModel> userPackages = this.context.Packages
                    .Where(package => package.Recipient.UserName == this.User.Identity.Name)
                    .Include(Package => Package.Status)
                    .Select(package => new PackageHomeViewModel
                    {
                        Id = package.Id,
                        Description = package.Description,
                        Status = package.Status.Name
                    })
                    .ToList();

                return this.View(userPackages);
            }

            return this.View();
        }
    }
}
