using Stopify.Data;
using Stopify.Data.Models;
using Stopify.Services.Mapping;
using Stopify.Services.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Stopify.Services
{
    public class ProductService : IProductService
    {
        private readonly StopifyDbContext context;

        public ProductService(StopifyDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Create(ProductServiceModel productServiceModel)
        {
            ProductType productTypeFromDb = 
                context.ProductTypes
                .SingleOrDefault(productType => productType.Name == productServiceModel.ProductType.Name);

            Product product = AutoMapper.Mapper.Map<Product>(productServiceModel);
            product.ProductType = productTypeFromDb;

            context.Products.Add(product);
            int result = await context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CreateProductType(ProductTypeServiceModel productTypeServiceModel)
        {
            ProductType productType = new ProductType
            {
                Name = productTypeServiceModel.Name
            };

            context.ProductTypes.Add(productType);
            int result = await context.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<ProductTypeServiceModel> GetAllProductTypes()
        {
            return this.context.ProductTypes.To<ProductTypeServiceModel>();
        }

        public IQueryable<ProductServiceModel> GetAllProducts()
        {
            return this.context.Products.To<ProductServiceModel>();
        }
    }
}
