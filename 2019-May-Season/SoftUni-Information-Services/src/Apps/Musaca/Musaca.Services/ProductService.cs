using System.Collections.Generic;
using System.Linq;
using Musaca.Data;
using Musaca.Models;

namespace Musaca.Services
{
    public class ProductService : IProductService
    {
        private readonly MusacaDbContext context;

        public ProductService(MusacaDbContext musacaDbContext)
        {
            this.context = musacaDbContext;
        }

        public Product CreateProduct(Product product)
        {
            this.context.Add(product);
            this.context.SaveChanges();

            return product;
        }

        public Product GetByName(string name)
            => this.context.Products.SingleOrDefault(product => product.Name == name);

        public List<Product> GetAll()
            => this.context.Products.ToList();
    }
}
