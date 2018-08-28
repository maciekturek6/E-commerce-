using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models
{
    //implementacja intefejsu - przechowywanie i pobieranie zbioru obiektów Order dla utworzonych bądź zmodyfikowanych zamówień
    public class EFOrderRepository : IOrderRepository
    {
        private ApplicationDbContext context;

        public EFOrderRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        //podczas odczytu obiektu Order z bazy danych kolekcja podana za pomocą właściwości Lines powinna być wczytana wraz z każdym obiektem
        //Product powiązanym z obiektem kolekcji
        public IQueryable<Order> Orders => context.Orders
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product);

        public void SaveOrder(Order order)
        {  //EF próbuje zapisać już przechowywane obiekty co prowadzi do błędu, dlatego należy poinfromaować EFC o istnieniu tych obiektów
            context.AttachRange(order.Lines.Select(l => l.Product));
            if (order.OrderID == 0)
            {
                context.Orders.Add(order);
            }

            context.SaveChanges();
        }
    
    }
}
