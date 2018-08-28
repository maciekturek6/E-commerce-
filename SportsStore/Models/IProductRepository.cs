using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
      //mechanizm pozwalający na pobieranie obiektów Product z bazy danych
    //zdefinijemt dla niego interface
    
        public interface IProductRepository
        {
            IQueryable<Product> Products { get; } //pozwala na pozyskanie sekwencji obiektów Product bez konieczności określania sposobu przechowywania i pobierania danych

            //dodanie możliwośći aktualizowania repozytorium produktów
            void SaveProduct(Product product);

            //nowa metoda obsługi usuwania elementów jest dosyć proste
            Product DeleteProduct(int productID);
        }
    
}
