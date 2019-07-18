using Stopify.Data.Models;
using Stopify.Services.Mapping;
using System;

namespace Stopify.Services.Models
{
    public class ProductServiceModel : IMapFrom<Product>, IMapTo<Product>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int ProductTypeId { get; set; }

        public ProductTypeServiceModel ProductType { get; set; }

        public decimal Price { get; set; }

        public string Picture { get; set; }

        public DateTime ManufacturedOn { get; set; }
    }
}
