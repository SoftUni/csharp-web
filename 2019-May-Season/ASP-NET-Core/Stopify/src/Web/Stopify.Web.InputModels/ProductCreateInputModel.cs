using Microsoft.AspNetCore.Http;
using System;

namespace Stopify.Web.InputModels
{
    public class ProductCreateInputModel
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime ManufacturedOn { get; set; }

        public IFormFile Picture { get; set; }

        public string ProductType { get; set; }
    }
}
