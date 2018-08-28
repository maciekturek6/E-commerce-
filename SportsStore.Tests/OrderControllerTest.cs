using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class OrderControllerTest  //STR 292 OPIS TESTU  
    {
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            //Utworzenie imitacji repozytorium
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            //Utworzenie pustego koszyka
            Cart cart = new Cart();
            //Utworzenie zamówienia 
            Order order = new Order();
            //Egzemplarz kontrolera
            OrderController target = new OrderController(mock.Object, cart);

            //Działanie
            ViewResult result = target.CheckOut(order) as ViewResult;

            //Asercje
            //sprawdzenie czy zamowienie zostało umieszczone w repozytorium
            mock.Verify(m=>m.SaveOrder(It.IsAny<Order>()), Times.Never);
            //czy metoda zwraca domyślny widok
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            //czy przekazujemy prawidłowy model do widoku
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            //Utworzenie imitacji repozytorium
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            //Utworzenie pustego koszyka
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
           
            //Egzemplarz kontrolera
            OrderController target = new OrderController(mock.Object, cart);
            //dodanie błędu do modelu
            target.ModelState.AddModelError("error","error");

            //Działanie
            ViewResult result = target.CheckOut(new Order()) as ViewResult;

            //Asercje
            //sprawdzenie czy zamowienie zostało umieszczone w repozytorium
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            //czy metoda zwraca domyślny widok
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            //czy przekazujemy prawidłowy model do widoku
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_And_Submit_Order()
        {
            //Utworzenie imitacji repozytorium
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            //Utworzenie pustego koszyka
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            
            //Egzemplarz kontrolera
            OrderController target = new OrderController(mock.Object, cart);

            //Działanie - próba zakończenia zamówienia
            RedirectToActionResult result = target.CheckOut(new Order()) as RedirectToActionResult;
            

            //Asercje
            //sprawdzenie czy zamowienie zostało umieszczone w repozytorium
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            //czy metoda przekierowuje do metody akcji Completed()
            Assert.Equal("Completed", result.ActionName);
        }
    }
}
