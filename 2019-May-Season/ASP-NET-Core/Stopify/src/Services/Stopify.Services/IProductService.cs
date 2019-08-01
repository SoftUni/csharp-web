using Stopify.Services.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Stopify.Services
{
    public interface IProductService
    {
        IQueryable<ProductTypeServiceModel> GetAllProductTypes();

        IQueryable<ProductServiceModel> GetAllProducts(string criteria = null);

        Task<ProductServiceModel> GetById(string id);

        Task<bool> Create(ProductServiceModel productServiceModel);

        Task<bool> Edit(string id, ProductServiceModel productServiceModel);

        Task<bool> Delete(string id);

        Task<bool> CreateProductType(ProductTypeServiceModel productTypeServiceModel);
    }
}
