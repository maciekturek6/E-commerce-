using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Castle.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using  Microsoft.AspNetCore.Identity;
//using IConfiguration = Castle.Core.Configuration.IConfiguration;

//using IConfiguration = Castle.Core.Configuration.IConfiguration;

namespace SportsStore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup (IConfiguration configuration) => 
            Configuration = configuration;
            
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration ["Data:SportStoreProducts:ConnectionString"] ));

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(
                    Configuration["Data:SportStoreIdentity:ConnectionString"]));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IProductRepository, EFProductRepository>(); //kiedy kontroler wymaga implementacji interfejsu IProdRepos, wówczas powinien
            //otrzymać egzemplarz klasy FakeProdRepos
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp)); //obiekt Cart wymagany przez komponenty obslugujące to samo żądanie  HTTP będzie otrzymywał dokładnie ten sam obiekt
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //usługa określająca ze zawsze powinien być uzywany ten sam obiekt
            services.AddTransient<IOrderRepository, EFOrderRepository>(); //zarejestrowanie repozytorium zamówień w charakterze usługi
            services.AddMvc();
            services.AddMemoryCache(); //skonfigurowanie magazynu danych w pamięci
            services.AddSession(); //rejestruje usługi używane w celu uzyskania dostępu do danych sesji
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //IHostingEnv - ma na celu dostarczenie informacji o środowisku, w którym została uruchomiona aplikajca,
        //na przykład: programistyczne lub produkcyjne;
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            
            app.UseStaticFiles();
            app.UseSession(); //pozwala sesji systemowej na automatyczne powiązanie żądań z sesjami po otrzymaniu tych żądań od klienta
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Error",
                    template: "Error",
                    defaults: new { Controller = "Error", action = "Error" }

                );

                routes.MapRoute(
                    name: null,
                    template: "{category}/Strona{productPage:int}",
                    defaults: new { Controller = "Product", action = "List" }

                );
                routes.MapRoute(
                    name: null,
                    template: "Strona{productPage:int}",
                    defaults: new { Controller = "Product", action = "List", productPage =1 }

                );
                routes.MapRoute(
                    name: null,
                    template: "{category}",
                    defaults: new { Controller = "Product", action = "List", productPage = 1 }

                );
                routes.MapRoute(
                    name: null,
                    template: "",
                    defaults: new { Controller = "Product", action = "List", productPage = 1 });
                //nowa trasa musi być dodana przed trasą domyślną (default)

                routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
            });
            //SeedData.EnsurePopulated(app);//umieszczenie danych początkowych w bazie danych 
            //IdentitySeedData.EnsurePopulated(app);
        }
    }
}
