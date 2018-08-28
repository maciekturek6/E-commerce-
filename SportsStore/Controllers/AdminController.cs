using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace SportsStore.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductRepository repository;

        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }
        //metodzie View przekazywany jest zbiór produktów w bazie danych
        public ViewResult Index() => View(repository.Products);

        [HttpPost]
        public IActionResult SeedDatabase()
        {
            SeedData.EnsurePopulated(HttpContext.RequestServices);
            return RedirectToAction(nameof(Index));
        }


        //wyświetlanie strony pozwalającej administratotowi na zmianę wartości właściwości produktu,
        //dodanie metody akcji umożliwiającej przetwarzanie tych zmian po przesłaniu danych
        public ViewResult Edit(int productId) => // metoda wyszukuje produkt z identyfikatorem odpowiadającym wartości parametru productId
            View(repository.Products.FirstOrDefault(p => p.ProductID == productId));

        //wywołanie w momencie naciśnięcia przycisku
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid) //odczyt właściwości upewniamy się,że proces dołączniania modelu ma możliwość kontroli poprawności danych przesłanych przez użytkownika
            {
                repository.SaveProduct(product); // jeżeli jest ok zapisujemy dane do repozytorium
                TempData["message"] = $"Zapisano {product.Name}."; //kontener TempData - dane takie są ograniczone do jednej sesji użytkownika i są przechowywane do momentu ich odczytania. Odczytamy te dane w widoku gnerowanym przez metodę kacji do której przekierowujemy użytkownika
                return RedirectToAction("Index");//a następnie wywołujemy akcje index
            }
            else
            {
                //Błąd w wartościach danych
                return View(product); //jeżeli jest błąd ponownie generujemy widok Edit
            }            
        }

        public ViewResult Create() => View("Edit", new Product());

        [HttpPost]
        public IActionResult Delete(int productId)
        {
            Product deletedProduct = repository.DeleteProduct(productId);
            if (deletedProduct != null)
            {
                TempData["message"] = $"Usunięto {deletedProduct.Name}.";
            }

            return RedirectToAction("Index");
        }
    }
}
