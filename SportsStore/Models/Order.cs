using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; //atrybut kontroli poprawności z przestrzeni nazw
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SportsStore.Models
{
    public class Order
    {
        [BindNever] //uniemożliwia użytkownikowi podanie w żądaniu HTTP wartości dla oznaczonych nim właściwości
        public int OrderID { get; set; }
        [BindNever]
        public ICollection<CartLine> Lines { get; set; }

        [BindNever] //oznaczanie zrealizowanych zamówień
        public bool Shipped { get; set; }

        [Required(ErrorMessage = "Proszę podać imię i nazwisko.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Proszę podać pierwszy wiersz adresu.")]
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę miasta.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę województwa.")]
        public string State { get; set; }

        public string Zip { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę kraju.")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; }
    }
}
