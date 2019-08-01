using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stopify.Services;
using Stopify.Web.ViewModels.Home.Index;

namespace Stopify.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService productService;

        public HomeController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Index([FromQuery]string criteria = null)
        {
            if(this.User.Identity.IsAuthenticated)
            {
                List<ProductHomeViewModel> products = await this.productService.GetAllProducts(criteria)
                    .Select(product => new ProductHomeViewModel
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Picture = product.Picture
                    })
                    .ToListAsync();

                this.ViewData["criteria"] = criteria;

                return this.View(products);
            }

            return View();
        }
    }
}
