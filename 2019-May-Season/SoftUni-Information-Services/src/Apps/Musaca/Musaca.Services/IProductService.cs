using System.Collections.Generic;
using Musaca.Models;

namespace Musaca.Services
{
    public interface IProductService
    {
        Product CreateProduct(Product product);

        Product GetByName(string name);

        List<Product> GetAll();
    }
}
