using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        public OrderController(IOrderRepository repoService, Cart cartService)
        {
            repository = repoService;
            cart = cartService;
        }

        //metoda List pobiera z repozytorium wszystkie obiekty Order, których właściwość Shipped ma wartość fasle
        // a następnie przekazuje je do widoku domyślnego(metoda uzywana w celu wyświetlenia administratorowi listy niezrealizowanych zamówień
        [Authorize]
        public ViewResult List() => View(repository.Orders.Where(o => !o.Shipped));

        //wskazuje identyfikator zamówienia, który jest następnie używany do odszukania odpowiedniego obiektu Order w repozytorium, aby jego właściwość można było przypisać wartości true
        [HttpPost]
        [Authorize] //atrybut używany w celu ograniczenia dostępu do metody akcji
        public IActionResult MarkShipped(int orderID)
        {
            Order order = repository.Orders.FirstOrDefault(o => o.OrderID == orderID);
            if (order != null)
            {
                order.Shipped = true;
                repository.SaveOrder(order);
            }

            return RedirectToAction(nameof(List));
        }

        public ViewResult Checkout() => View(new Order());

        [HttpPost] // metoda wykonana w momencie przesłania formularza przez użytkownika
        public IActionResult CheckOut(Order order)
        {
            if (cart.Lines.Count() == 0) // w przypadku braku produktów
            {
                ModelState.AddModelError("", "Koszyk jest pusty!");
            }

            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }

        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }
    }
}
