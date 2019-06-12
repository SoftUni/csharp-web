using Musaca.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Result;
using SIS.MvcFramework.Mapping;
using Musaca.App.ViewModels.Products;
using Musaca.App.BindingModels.Products;
using Musaca.Models;
using SIS.MvcFramework.Attributes.Security;
using System.Linq;

namespace Musaca.App.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService productService;

        private readonly IOrderService orderService;

        public ProductsController(IProductService productService, IOrderService orderService)
        {
            this.productService = productService;
            this.orderService = orderService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult All()
        {
            return this.View(this.productService.GetAll().To<ProductAllViewModel>().ToList());
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(ProductCreateBindingModel productCreateBindingModel)
        {
            if(!this.ModelState.IsValid)
            {
                // TODO: SAVE FORM RESULT
                return this.View();
            }

            this.productService.CreateProduct(productCreateBindingModel.To<Product>());

            return this.Redirect("All");
        }

        [HttpPost]
        public IActionResult Order(ProductOrderBindingModel productOrderBindingModel)
        {
            Product productToOrder = this.productService.GetByName(productOrderBindingModel.Product);

            this.orderService.AddProductToCurrentActiveOrder(productToOrder.Id, this.User.Id);

            return this.Redirect("/");
        }
    }
}
