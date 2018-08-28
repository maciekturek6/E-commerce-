using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    //imitacja repozytorium 
    public class FakeProductRepository /*: IProductRepository*/
    {
        //klasa implementuje interfejs i jako wartość właściwośći Products zwraca stałej wielkości  kolejkcję obiektów Product
        public IQueryable<Product> Products => new List<Product>
        {
            new Product { Name = "Piłka nożna", Price = 25},
            new Product { Name = "Deska surfingowa", Price = 179},
            new Product { Name = "Buty do biegania", Price = 95}
        }.AsQueryable<Product>(); //metoda jest używana w celu przeprowadzenia konwersji kolekcji obiektów na postać typu IQueryalbe <T>, wymaganą
        //do zaimplementowania interfejsu IProductRepository
    }

    
}
