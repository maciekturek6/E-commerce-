using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SportsStore.Infrastructure;

namespace SportsStore.Models
{
    public class SessionCart : Cart //sessionCart tworzy podklasę klasy Cart, nadpisując metody aby wywoływały implementacje bazowe, a następnie
    //przechowywały uaktualnione informacje o stanie za pomocą metod rozszerzenia interfejsu ISession
    {
        public static Cart GetCart(IServiceProvider services) //metoda GetCart jest fabryką przeznaczoną do tworzenia obiektów typu SessionCart
        // i dostarczającą im obiektu ISession, umożliwiając tym samym przechowywanie obiektów SessionCart
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            SessionCart cart = session?.GetJson<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }
        [JsonIgnore]
        public ISession Session { get; set; }

        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            Session.SetJson("Cart", this);
        }

        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Session.SetJson("Cart",this);
        }

        public override void Clear()
        {
            base.Clear();
            Session.Remove("Cart");
        }
    }
}
