using System.Collections.Generic;
using System.Linq;
using Musaca.App.ViewModels.Products;

namespace Musaca.App.ViewModels.Orders
{
    public class OrderHomeViewModel
    {
        public OrderHomeViewModel()
        {
            this.Products = new List<ProductHomeViewModel>();
        }

        public List<ProductHomeViewModel> Products { get; set; }

        public decimal Price => this.Products.Sum(product => product.Price);
    }
}
