using DutchTreatCore.Data;
using DutchTreatCore.Repositories;
using DutchTreatCore.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace DutchTreatCore
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMailService, NullMailService>();
            services.AddTransient<DutchSeeder>();
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<IOrdersRepository, OrdersRepository>();
            
            // Support for real mail service
            services.AddMvc().AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling =  ReferenceLoopHandling.Ignore);
            services.AddDbContext<DutchContext>(cfg =>
                cfg.UseSqlServer(_config.GetConnectionString("DefaultConnectionString")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/error");

            app.UseMvc(config =>
                config.MapRoute("Default",
                    "/{controller}/{action}/{id?}",
                    new {controller = "App", Action = "Index"}));
            if (env.IsDevelopment())
            {
                // Seed the DataBase
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
                    seeder.Seed();
                }
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}