using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstAspNetCoreApplication.Controllers;
using MyFirstAspNetCoreApplication.Data;
using MyFirstAspNetCoreApplication.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MyApp.Tests
{
    public class ProductsControllerTests
    {


        [Fact]
        public void GetShouldReturnTheProductIfFound()
        {
            var product = new Product
            {
                Id = 2,
                Name = "product test",
                Price = 100,
            };

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            var controller = new ProductsController(dbContext);

            var result = controller.Get(2);
            Assert.NotNull(result);
            Assert.Equal("product test", result.Value.Name);
        }

        [Fact]
        public void GetShouldReturnNotFoundIfProductDoesNotExist()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            var controller = new ProductsController(dbContext);

            // Act
            var result = controller.Get(3);

            // Assert
            Assert.Null(result.Value);
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
