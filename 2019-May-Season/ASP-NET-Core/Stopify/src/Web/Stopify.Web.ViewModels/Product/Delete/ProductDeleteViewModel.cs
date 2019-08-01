using Stopify.Services.Mapping;
using Stopify.Services.Models;
using System;

namespace Stopify.Web.ViewModels.Product.Delete
{
    public class ProductDeleteViewModel : IMapFrom<ProductServiceModel>
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime ManufacturedOn { get; set; }

        public string Picture { get; set; }

        public ProductDeleteProductTypeViewModel ProductType { get; set; }
    }
}
