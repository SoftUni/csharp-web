using Stopify.Services.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Stopify.Services
{
    public interface IProductService
    {
        Task<IQueryable<ProductTypeServiceModel>> GetAllProductTypes();

        Task<bool> Create(ProductServiceModel productServiceModel);

        Task<bool> CreateProductType(ProductTypeServiceModel productTypeServiceModel);
    }
}
