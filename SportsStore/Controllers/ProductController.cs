using System;
//using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;

        //stronicowanie - na stronie 4 produkty
        public int PageSize = 4;

        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }

        //stronicowanie
        public ViewResult List(string category, int productPage = 1) //parametr opcjonalny, dzięki czemu wywołanie metody bez parametry będzie traktowane jakbym 
                                      //podał wartość określoną w definicji (wyświetla się pierwsza strona
            => View(new ProductListViewModel //pobieramy obiekty product
            {
                Products = repository.Products
                    .Where(p=> category == null || p.Category ==category) //jeżeli wartość category jest różna od null wybierane są obiekty Product
                    .OrderBy(p => p.ProductID) //układamy w kolejności klucza
                    .Skip((productPage - 1) *
                          PageSize) //pomijamy produkty znajdujące się przed naszą stroną i odczytujemy
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category ==null ?
                        repository.Products.Count() :
                        repository.Products.Where(e=>e.Category == category).Count()
                },
                CurrentCategory = category //ustawienie właściwości
            });
    }
}
