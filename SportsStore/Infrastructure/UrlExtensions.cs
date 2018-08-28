using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SportsStore.Infrastructure
{

    //definicja metody rozszerzenia PathAndQuerry
    public static class UrlExtensions
    {
        //klasa httpRequest używana jest do opisania żądania HTTP, metoda generuje adres URL, który jest przekazany przeglądarce WWW po 
        //uaktualnieniu koszyka na zakupy
        public static string PathAndQuerry(this HttpRequest request) =>
            request.QueryString.HasValue
                ? $"{request.Path}{request.QueryString}"
                : request.Path.ToString();
    }
}
