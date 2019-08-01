using Microsoft.EntityFrameworkCore;
using Stopify.Data;
using Stopify.Data.Models;
using Stopify.Services.Mapping;
using Stopify.Services.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Stopify.Services
{
    public class ProductService : IProductService
    {
        private const string PriceLowestToHighestProductOrderCriteria = "price-lowest-to-highest";

        private const string PriceHighestToLowestProductOrderCriteria = "price-highest-to-lowest";

        private const string ManufacturedOnOldestToNewestProductOrderCriteria = "date-oldest-to-newest";

        private const string ManufacturedOnNewestToOldestProductOrderCriteria = "date-newest-to-oldest";

        private readonly StopifyDbContext context;

        public ProductService(StopifyDbContext context)
        {
            this.context = context;
        }

        private IQueryable<Product> GetAllProductsByPriceAscending()
        {
            return this.context.Products.OrderBy(product => product.Price);
        }

        private IQueryable<Product> GetAllProductsByPriceDescending()
        {
            return this.context.Products.OrderByDescending(product => product.Price);
        }

        private IQueryable<Product> GetAllProductsByManufacturedOnAscending()
        {
            return this.context.Products.OrderBy(product => product.ManufacturedOn);
        }

        private IQueryable<Product> GetAllProductsByManufacturedOnDescending()
        {
            return this.context.Products.OrderByDescending(product => product.ManufacturedOn);
        }

        public async Task<bool> Create(ProductServiceModel productServiceModel)
        {
            ProductType productTypeFromDb =
                context.ProductTypes
                .SingleOrDefault(productType => productType.Name == productServiceModel.ProductType.Name);

            if(productTypeFromDb == null)
            {
                throw new ArgumentNullException(nameof(productTypeFromDb));
            }

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

        public IQueryable<ProductServiceModel> GetAllProducts(string criteria = null)
        {
            switch (criteria)
            {
                case PriceLowestToHighestProductOrderCriteria: return this.GetAllProductsByPriceAscending().To<ProductServiceModel>();
                case PriceHighestToLowestProductOrderCriteria: return this.GetAllProductsByPriceDescending().To<ProductServiceModel>();
                case ManufacturedOnOldestToNewestProductOrderCriteria: return this.GetAllProductsByManufacturedOnAscending().To<ProductServiceModel>();
                case ManufacturedOnNewestToOldestProductOrderCriteria: return this.GetAllProductsByManufacturedOnDescending().To<ProductServiceModel>();
            }

            return this.context.Products.To<ProductServiceModel>();
        }

        public async Task<ProductServiceModel> GetById(string id)
        {
            return await this.context.Products
                .To<ProductServiceModel>()
                .SingleOrDefaultAsync(product => product.Id == id);
        }

        public async Task<bool> Edit(string id, ProductServiceModel productServiceModel)
        {
            ProductType productTypeFromDb =
                context.ProductTypes
                .SingleOrDefault(productType => productType.Name == productServiceModel.ProductType.Name);

            if (productTypeFromDb == null)
            {
                throw new ArgumentNullException(nameof(productTypeFromDb));
            }

            Product productFromDb = await this.context.Products.SingleOrDefaultAsync(product => product.Id == id);

            if(productFromDb == null)
            {
                throw new ArgumentNullException(nameof(productFromDb));
            }

            productFromDb.Name = productServiceModel.Name;
            productFromDb.Price = productServiceModel.Price;
            productFromDb.ManufacturedOn = productServiceModel.ManufacturedOn;
            productFromDb.Picture = productServiceModel.Picture;

            productFromDb.ProductType = productTypeFromDb;

            this.context.Products.Update(productFromDb);
            int result = await context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> Delete(string id)
        {
            Product productFromDb = await this.context.Products.SingleOrDefaultAsync(product => product.Id == id);

            if (productFromDb == null)
            {
                throw new ArgumentNullException(nameof(productFromDb));
            }

            this.context.Products.Remove(productFromDb);

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }
    }
}
