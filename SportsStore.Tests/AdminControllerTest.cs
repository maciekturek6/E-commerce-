using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class AdminControllerTest
    {
        [Fact]
        public void Index_Contains_All_Products()
        {
            //tworzenie imitacji repozytorium
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            }.AsQueryable<Product>());
            //Utworzenie kontrolera
            AdminController target = new AdminController(mock.Object);

            //Działanie
            Product[] result = GetViewModel<IEnumerable<Product>>(target.Index())?.ToArray();
            //Asercje
            Assert.Equal(3, result.Length);
            Assert.Equal("P1", result[0].Name);
            Assert.Equal("P2", result[1].Name);
            Assert.Equal("P3", result[2].Name);
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }

        [Fact]
        public void Can_Edit_Product()
        {
            //tworzenie imitacji repozytorium
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            }.AsQueryable<Product>());
            //Utworzenie kontrolera
            AdminController target = new AdminController(mock.Object);

            //Działanie
            Product p1 = GetViewModel<Product>(target.Edit(1));
            Product p2 = GetViewModel<Product>(target.Edit(2));
            Product p3 = GetViewModel<Product>(target.Edit(3));
            //Asercje
            Assert.Equal(1, p1.ProductID);
            Assert.Equal(2, p2.ProductID);
            Assert.Equal(3, p3.ProductID);
        }
        [Fact]
        public void Can_Edit_Nonexistent_Product()
        {
            //tworzenie imitacji repozytorium
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            }.AsQueryable<Product>());
            //Utworzenie kontrolera
            AdminController target = new AdminController(mock.Object);

            //Działanie
            Product result = GetViewModel<Product>(target.Edit(4));
            //Asercje
            Assert.Null(result);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            //tworzenie imitacji repozytorium
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //tworzenie imitacji kontenera TempData
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();
            
            //Utworzenie kontrolera
            AdminController target = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };
            Product product = new Product{Name = "Test"};
            //Próba zapisania produktu
            IActionResult result = target.Edit(product);
            //sprawdzenie czy zostało wywołane repozytorium
            mock.Verify(m=>m.SaveProduct(product));
            //sprawdzenie typu zwracanego z metody
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index",(result as RedirectToActionResult).ActionName);
        }

        [Fact]
        public void Can_Save_Invalid_Changes()
        {
            //tworzenie imitacji repozytorium
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            //Utworzenie kontrolera
            AdminController target = new AdminController(mock.Object);
           
            Product product = new Product { Name = "Test" };
            target.ModelState.AddModelError("error","error");
            //Próba zapisania produktu
            IActionResult result = target.Edit(product);
            //sprawdzenie czy zostało wywołane repozytorium
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()),Times.Never());
            //sprawdzenie typu zwracanego z metody
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Delete_Valid_Products()
        {
            //tworzenie produktu
            Product prod = new Product {ProductID = 2, Name = "Test"};

            //tworzenie imitacji repozytorium
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                prod,
                new Product {ProductID = 3, Name = "P3"},
            }.AsQueryable<Product>());
            //Utworzenie kontrolera
            AdminController target = new AdminController(mock.Object);
            //usunięcie produktu
            target.Delete(prod.ProductID);
            //
            mock.Verify(m=>m.DeleteProduct(prod.ProductID));
           
        }
    }
}
