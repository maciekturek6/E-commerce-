using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class Cart
    {


        private List<CartLine> lineCollection = new List<CartLine>();

        //metoda pozwalająca na dodanie elementu do koszyka
        public virtual void AddItem(Product product, int quantity)
        {
            CartLine line = lineCollection
                .Where(p => p.Product.ProductID == product.ProductID)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        //Usunięcie poprzednio dodanego elementu z koszyka
        public virtual void RemoveLine(Product product) =>
            lineCollection.RemoveAll(l => l.Product.ProductID == product.ProductID);
        //obliczenie całkowitej wartości towarów w koszyku
        public virtual decimal ComputeTotalValue() => lineCollection.Sum(e => e.Product.Price * e.Quantity);
        //wyzerowanie przez usunięcie wszystkich towarów
        public virtual void Clear() => lineCollection.Clear();
        //właściwość dodającą dostęp do zawartości koszyka 
        public virtual IEnumerable<CartLine> Lines => lineCollection;

    }
    //Klasa reprezentująca produkty wybrane przez klienta oraz ilość tego produktu
    public class CartLine
    {
        public int CartLineID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

}

