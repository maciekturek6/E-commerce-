using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SportsStore.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IServiceProvider services) //argument który jest klasą używaną w metodzie Configure() klasy StartUp
                                                                    //do zarejestrowania klas oprogramowania pośredniczącego w celu obsługi żądań HTTP(ma to zagwarantować że baza danych ma zawartość)
        {
            ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>(); //metoda pobiera pobiera obiekt appdbcon
            // i wywołuje migrate w celi zagwarantowania przeprowadzenia migracji (utworzenie bazy danych i jej przygotowanie do przechowywania produktów

            // context.Database.Migrate();
            if (!context.Products.Any())
            {
                //jezeli nie ma zadnych obiektów wówczas baza danych zostanie wypełniona kolekcja produktów Product/metoda addRange i savechanges
                context.Products.AddRange(
                    new Product
                    {
                        Name = "Kajak",
                        Descriprtion = "Łódka przeznaczona dla jednej osoby",
                        Category = "Sporty wodne",
                        Price = 275
                    },
                    new Product
                    {
                        Name = "Kamizelka ratunkowa",
                        Descriprtion = "Chroni i udaje uroku",
                        Category = "Sporty wodne",
                        Price = 48.95m
                    },
                    new Product
                    {
                        Name = "Piłka",
                        Descriprtion = "Zatwierdzona przez FIFA rozmiar i waga",
                        Category = "Piłka nożna",
                        Price = 19.50m
                    },
                    new Product
                    {
                        Name = "Flagi narożne",
                        Descriprtion = "Nadadzą twojemu boisku profesjonalny wygląd",
                        Category = "Piłka nożna",
                        Price = 34.95m
                    },
                    new Product
                    {
                        Name = "Stadion",
                        Descriprtion = "Składany stadion na 35000 osób",
                        Category = "Piłka nożna",
                        Price = 79500
                    },
                    new Product
                    {
                        Name = "Czapka",
                        Descriprtion = "Zwiększa efektywność mózgu o 75%",
                        Category = "Szachy",
                        Price = 16
                    },
                    new Product
                    {
                        Name = "Niestabilne krzesło",
                        Descriprtion = "Zmniejsza szanse przeciwnika",
                        Category = "Szachy",
                        Price = 29.95m
                    },
                    new Product
                    {
                        Name = "Ludzka szachownica",
                        Descriprtion = "Przyjemna gra dla całej rodziny!",
                        Category = "Szachy",
                        Price = 75
                    },
                    new Product
                    {
                        Name = "Błyszczący król",
                        Descriprtion = "Figura pokryta złotem i wysadzana diamentami",
                        Category = "Szachy",
                        Price = 1200
                    }
                );
                context.SaveChanges();

            }
        }
    }
}
