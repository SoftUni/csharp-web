using Stopify.Services.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Stopify.Services
{
    public interface IProductService
    {
        IQueryable<ProductTypeServiceModel> GetAllProductTypes();

        IQueryable<ProductServiceModel> GetAllProducts();

        Task<bool> Create(ProductServiceModel productServiceModel);

        Task<bool> CreateProductType(ProductTypeServiceModel productTypeServiceModel);
    }
}
