using Stopify.Services.Mapping;
using Stopify.Services.Models;
using System;

namespace Stopify.Web.ViewModels
{
    public class ProductDetailsViewModel : IMapFrom<ProductServiceModel>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ProductTypeName { get; set; }

        public DateTime ManufacturedOn { get; set; }

        public string Picture { get; set; }
    }
}
