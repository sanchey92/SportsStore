using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Can_Use_Repository()
        {
            // Arrange
            var mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"}
            }).AsQueryable<Product>());
            var controller = new HomeController(mock.Object);
            
            // Act
            var result = (controller.Index() as ViewResult)?.ViewData.Model as IEnumerable<Product>;
            
            // Assert
            var productArray = result?.ToArray();
            Assert.True(productArray?.Length == 2);
            Assert.Equal("P1", productArray[0].Name);
            Assert.Equal("P2", productArray[1].Name);
        }
    }
}