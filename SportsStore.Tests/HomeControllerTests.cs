using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
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
            var result = controller.Index(null).ViewData.Model as ProductsListViewModel;

            // Assert
            var productArray = result?.Products.ToArray();
            Assert.True(productArray?.Length == 2);
            Assert.Equal("P1", productArray[0].Name);
            Assert.Equal("P2", productArray[1].Name);
        }

        [Fact]
        public void Can_Paginate()
        {
            // Arrange
            var mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
                new Product {ProductId = 4, Name = "P4"},
                new Product {ProductId = 5, Name = "P5"},
            }).AsQueryable<Product>());

            var controller = new HomeController(mock.Object);
            controller.PageSize = 3;

            // Act
            var result = controller.Index(null, 2).ViewData.Model as ProductsListViewModel;

            // Assert
            var productArray = result?.Products.ToArray();
            Assert.True(productArray?.Length == 2);
            Assert.Equal("P4", productArray[0].Name);
            Assert.Equal("P5", productArray[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange
            var mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
                new Product {ProductId = 4, Name = "P4"},
                new Product {ProductId = 5, Name = "P5"}
            }).AsQueryable<Product>());
            // Arrange
            var controller = new HomeController(mock.Object) {PageSize = 3};
            // Act
            var result = controller.Index(null, 2).ViewData.Model as ProductsListViewModel;
            // Assert
            var pageInfo = result?.PagingInfo;
            Assert.Equal(2, pageInfo?.CurrentPage);
            Assert.Equal(3, pageInfo?.ItemPerPage);
            Assert.Equal(5, pageInfo?.TotalItem);
            Assert.Equal(2, pageInfo?.TotalPages);
        }

        [Fact]
        public void Can_Filter_Products()
        {
            // Arrange
            var mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductId = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductId = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductId = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductId = 5, Name = "P5", Category = "Cat3"},
            }).AsQueryable<Product>());

            var controller = new HomeController(mock.Object);
            controller.PageSize = 3;

            // Act
            var result =
                (controller.Index("Cat2", 1).ViewData.Model as ProductsListViewModel)?
                .Products.ToArray();

            //Assert
            Assert.Equal(2, result?.Length);
            Assert.True(result?[0].Name == "P2" && result?[0].Category == "Cat2");
            Assert.True(result?[1].Name == "P4" && result?[1].Category == "Cat2");
        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            // Arrange
            var mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductId = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductId = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductId = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductId = 5, Name = "P5", Category = "Cat3"}
            }).AsQueryable<Product>());

            var target = new HomeController(mock.Object);
            target.PageSize = 3;

            Func<ViewResult, ProductsListViewModel> GetModel = result =>
                result?.ViewData?.Model as ProductsListViewModel;

            // Action
            int? res1 = GetModel(target.Index("Cat1")).PagingInfo.TotalItem;
            int? res2 = GetModel(target.Index("Cat2")).PagingInfo.TotalItem;
            int? res3 = GetModel(target.Index("Cat3")).PagingInfo.TotalItem;
            int? resAll = GetModel(target.Index(null))?.PagingInfo.TotalItem;

            // Assert
            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }
    }
}