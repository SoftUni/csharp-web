using Microsoft.EntityFrameworkCore;
using Stopify.Data;
using Stopify.Data.Models;
using Stopify.Services;
using Stopify.Services.Mapping;
using Stopify.Services.Models;
using Stopify.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Stopify.Tests.Service
{
    public class ProductServiceTests
    {
        private IProductService productService;

        private List<Product> GetDummyData()
        {
            return new List<Product>()
            {
                new Product
                {
                    Name = "Melissa AirConditioner XSQ-500",
                    Price = 5000.59M,
                    ManufacturedOn = DateTime.UtcNow.AddDays(-15),
                    Picture = "src/pics/somethingfunny/airconditioner",
                    ProductType = new ProductType
                    {
                        Name = "Air Conditioner"
                    }
                },
                new Product
                {
                    Name = "Samsung STY Plasma",
                    Price = 25000.00M,
                    ManufacturedOn = DateTime.UtcNow.AddDays(-45),
                    Picture = "src/pics/somethingfunny/tv",
                    ProductType = new ProductType
                    {
                        Name = "TV"
                    }
                }
            };
        }

        private async Task SeedData(StopifyDbContext context)
        {
            context.AddRange(GetDummyData());
            await context.SaveChangesAsync();
        }

        public ProductServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task GetAllProducts_WithDummyData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "ProductService GetAllProducts() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.productService = new ProductService(context);

            List<ProductServiceModel> actualResults = await this.productService.GetAllProducts().ToListAsync();
            List<ProductServiceModel> expectedResults = GetDummyData().To<ProductServiceModel>().ToList();

            for(int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, errorMessagePrefix + " " + "Name is not returned properly.");
                Assert.True(expectedEntry.Price == actualEntry.Price, errorMessagePrefix + " " + "Price is not returned properly.");
                Assert.True(expectedEntry.Picture == actualEntry.Picture, errorMessagePrefix + " " + "Picture is not returned properly.");
                Assert.True(expectedEntry.ProductType.Name == actualEntry.ProductType.Name, errorMessagePrefix + " " + "Product Type is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllProducts_WithDummyDataOrderedByPriceAscending_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "ProductService GetAllProducts() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.productService = new ProductService(context);

            List<ProductServiceModel> actualResults = await this.productService.GetAllProducts("price-lowest-to-highest").ToListAsync();
            List<ProductServiceModel> expectedResults = GetDummyData()
                .OrderBy(product => product.Price)
                .To<ProductServiceModel>().ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, errorMessagePrefix + " " + "Name is not returned properly.");
                Assert.True(expectedEntry.Price == actualEntry.Price, errorMessagePrefix + " " + "Price is not returned properly.");
                Assert.True(expectedEntry.Picture == actualEntry.Picture, errorMessagePrefix + " " + "Picture is not returned properly.");
                Assert.True(expectedEntry.ProductType.Name == actualEntry.ProductType.Name, errorMessagePrefix + " " + "Product Type is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllProducts_WithDummyDataOrderedByPriceDescending_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "ProductService GetAllProducts() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.productService = new ProductService(context);

            List<ProductServiceModel> actualResults = await this.productService.GetAllProducts("price-highest-to-lowest").ToListAsync();
            List<ProductServiceModel> expectedResults = GetDummyData()
                .OrderByDescending(product => product.Price)
                .To<ProductServiceModel>().ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, errorMessagePrefix + " " + "Name is not returned properly.");
                Assert.True(expectedEntry.Price == actualEntry.Price, errorMessagePrefix + " " + "Price is not returned properly.");
                Assert.True(expectedEntry.Picture == actualEntry.Picture, errorMessagePrefix + " " + "Picture is not returned properly.");
                Assert.True(expectedEntry.ProductType.Name == actualEntry.ProductType.Name, errorMessagePrefix + " " + "Product Type is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllProducts_WithDummyDataOrderedByManufacturedOnAscending_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "ProductService GetAllProducts() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.productService = new ProductService(context);

            List<ProductServiceModel> actualResults = await this.productService.GetAllProducts("date-oldest-to-newest").ToListAsync();
            List<ProductServiceModel> expectedResults = GetDummyData()
                .OrderBy(product => product.ManufacturedOn)
                .To<ProductServiceModel>().ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, errorMessagePrefix + " " + "Name is not returned properly.");
                Assert.True(expectedEntry.Price == actualEntry.Price, errorMessagePrefix + " " + "Price is not returned properly.");
                Assert.True(expectedEntry.Picture == actualEntry.Picture, errorMessagePrefix + " " + "Picture is not returned properly.");
                Assert.True(expectedEntry.ProductType.Name == actualEntry.ProductType.Name, errorMessagePrefix + " " + "Product Type is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllProducts_WithDummyDataOrderedByManufacturedOnDescending_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "ProductService GetAllProducts() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.productService = new ProductService(context);

            List<ProductServiceModel> actualResults = await this.productService.GetAllProducts("date-newest-to-oldest").ToListAsync();
            List<ProductServiceModel> expectedResults = GetDummyData()
                .OrderByDescending(product => product.ManufacturedOn)
                .To<ProductServiceModel>().ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, errorMessagePrefix + " " + "Name is not returned properly.");
                Assert.True(expectedEntry.Price == actualEntry.Price, errorMessagePrefix + " " + "Price is not returned properly.");
                Assert.True(expectedEntry.Picture == actualEntry.Picture, errorMessagePrefix + " " + "Picture is not returned properly.");
                Assert.True(expectedEntry.ProductType.Name == actualEntry.ProductType.Name, errorMessagePrefix + " " + "Product Type is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllProducts_WithZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "ProductService GetAllProducts() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            this.productService = new ProductService(context);

            List<ProductServiceModel> actualResults = await this.productService.GetAllProducts().ToListAsync();

            Assert.True(actualResults.Count == 0, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAllProductTypes_WithDummyData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "ProductService GetAllProductTypes() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.productService = new ProductService(context);

            List<ProductTypeServiceModel> actualResults = await this.productService.GetAllProductTypes().ToListAsync();
            List<ProductTypeServiceModel> expectedResults = GetDummyData().Select(product => product.ProductType).To<ProductTypeServiceModel>().ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, errorMessagePrefix + " " + "Name is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllProductTypes_WithZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "ProductService GetAllProductTypess() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            this.productService = new ProductService(context);

            List<ProductTypeServiceModel> actualResults = await this.productService.GetAllProductTypes().ToListAsync();

            Assert.True(actualResults.Count == 0, errorMessagePrefix);
        }

        [Fact]
        public async Task GetById_WithExistentId_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "ProductService GetById() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.productService = new ProductService(context);

            ProductServiceModel expectedData = context.Products.First().To<ProductServiceModel>();
            ProductServiceModel actualData = await this.productService.GetById(expectedData.Id);

            Assert.True(expectedData.Id == actualData.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(expectedData.Name == actualData.Name, errorMessagePrefix + " " + "Name is not returned properly.");
            Assert.True(expectedData.Price == actualData.Price, errorMessagePrefix + " " + "Price is not returned properly.");
            Assert.True(expectedData.Picture == actualData.Picture, errorMessagePrefix + " " + "Picture is not returned properly.");
            Assert.True(expectedData.ProductType.Name == actualData.ProductType.Name, errorMessagePrefix + " " + "Product Type is not returned properly.");
        }

        [Fact]
        public async Task GetById_WithNonExistentId_ShouldReturnNull()
        {
            string errorMessagePrefix = "ProductService GetById() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.productService = new ProductService(context);

            ProductServiceModel actualData = await this.productService.GetById("prakash");

            Assert.True(actualData == null, errorMessagePrefix);
        }

        [Fact]
        public async Task Create_WithCorrectData_ShouldSuccessfullyCreate()
        {
            string errorMessagePrefix = "ProductService Create() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.productService = new ProductService(context);

            ProductServiceModel testProduct = new ProductServiceModel
            {
                Name = "Pesho",
                Price = 5,
                ManufacturedOn = DateTime.UtcNow,
                Picture = "src/res/default.png",
                ProductType = new ProductTypeServiceModel
                {
                    Name = "TV"
                }
            };

            bool actualResult = await this.productService.Create(testProduct);
            Assert.True(actualResult, errorMessagePrefix);
        }

        [Fact]
        public async Task Create_WithNonExistentProductType_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "ProductService Create() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.productService = new ProductService(context);

            ProductServiceModel testProduct = new ProductServiceModel
            {
                Name = "Pesho",
                Price = 5,
                ManufacturedOn = DateTime.UtcNow,
                Picture = "src/res/default.png",
                ProductType = new ProductTypeServiceModel
                {
                    Name = "asdasdasd"
                }
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => this.productService.Create(testProduct));
        }

        [Fact]
        public async Task CreateProductType_WithCorrectData_ShouldSuccessfullyCreate()
        {
            string errorMessagePrefix = "ProductService CreateProductType() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            this.productService = new ProductService(context);

            ProductTypeServiceModel testProductType = new ProductTypeServiceModel
            {
                Name = "Pesho"
            };

            bool actualResult = await this.productService.CreateProductType(testProductType);
            Assert.True(actualResult, errorMessagePrefix);
        }

        [Fact]
        public async Task Edit_WithCorrectData_ShouldPassSuccessfully()
        {
            string errorMessagePrefix = "ProductService Edit() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.productService = new ProductService(context);

            ProductServiceModel expectedData = context.Products.First().To<ProductServiceModel>();

            bool actualData = await this.productService.Edit(expectedData.Id, expectedData);

            Assert.True(actualData, errorMessagePrefix);
        }

        [Fact]
        public async Task Edit_WithCorrectData_ShouldEditProductCorrectly()
        {
            string errorMessagePrefix = "ProductService Edit() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.productService = new ProductService(context);

            ProductServiceModel expectedData = context.Products.First().To<ProductServiceModel>();

            expectedData.Name = "Editted_Name";
            expectedData.Price = 0.01M;
            expectedData.ManufacturedOn = DateTime.UtcNow;
            expectedData.Picture = "Editted_Picture";
            expectedData.ProductType = context.ProductTypes.Last().To<ProductTypeServiceModel>();

            await this.productService.Edit(expectedData.Id, expectedData);

            ProductServiceModel actualData = context.Products.First().To<ProductServiceModel>();

            Assert.True(actualData.Name == expectedData.Name, errorMessagePrefix + " " + "Name not editted properly.");
            Assert.True(actualData.Price == expectedData.Price, errorMessagePrefix + " " + "Price not editted properly.");
            Assert.True(actualData.ManufacturedOn == expectedData.ManufacturedOn, errorMessagePrefix + " " + "Manufactured On not editted properly.");
            Assert.True(actualData.Picture == expectedData.Picture, errorMessagePrefix + " " + "Picture not editted properly.");
            Assert.True(actualData.ProductType.Name == expectedData.ProductType.Name, errorMessagePrefix + " " + "Product Type not editted properly.");

        }

        [Fact]
        public async Task Edit_WithNonExistentProductType_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "ProductService Edit() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.productService = new ProductService(context);

            ProductServiceModel expectedData = context.Products.First().To<ProductServiceModel>();

            expectedData.Name = "Editted_Name";
            expectedData.Price = 0.01M;
            expectedData.ManufacturedOn = DateTime.UtcNow;
            expectedData.Picture = "Editted_Picture";
            expectedData.ProductType = new ProductTypeServiceModel
            {
                Name = "Non-Existent"
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => this.productService.Edit(expectedData.Id, expectedData));
        }

        [Fact]
        public async Task Edit_WithNonExistentProductId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "ProductService Edit() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.productService = new ProductService(context);

            ProductServiceModel expectedData = context.Products.First().To<ProductServiceModel>();

            expectedData.Name = "Editted_Name";
            expectedData.Price = 0.01M;
            expectedData.ManufacturedOn = DateTime.UtcNow;
            expectedData.Picture = "Editted_Picture";
            expectedData.ProductType = context.ProductTypes.Last().To<ProductTypeServiceModel>();

            await Assert.ThrowsAsync<ArgumentNullException>(() => this.productService.Edit("Non-Existent", expectedData));
        }

        [Fact]
        public async Task Delete_WithCorrectData_ShouldPassSuccessfully()
        {
            string errorMessagePrefix = "ProductService Delete() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.productService = new ProductService(context);

            string testId = context.Products.First().To<ProductServiceModel>().Id;

            bool actualData = await this.productService.Delete(testId);

            Assert.True(actualData, errorMessagePrefix);
        }

        [Fact]
        public async Task Delete_WithCorrectData_ShouldDeleteSuccessfully()
        {
            string errorMessagePrefix = "ProductService Delete() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.productService = new ProductService(context);

            string testId = context.Products.First().To<ProductServiceModel>().Id;

            await this.productService.Delete(testId);

            int expectedCount = 1;
            int actualCount = context.Products.Count();

            Assert.True(expectedCount == actualCount, errorMessagePrefix);
        }

        [Fact]
        public async Task Delete_WithNonExistentProductId_ShouldThrowArgumentNullException()
        {
            string errorMessagePrefix = "ProductService Delete() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.productService = new ProductService(context);

            await Assert.ThrowsAsync<ArgumentNullException>(() => this.productService.Delete("Non-Existent"));
        }
    }
}
