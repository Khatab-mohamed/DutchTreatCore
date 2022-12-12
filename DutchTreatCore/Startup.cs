using System.Text;
using AutoMapper;
using DutchTreatCore.Data;
using DutchTreatCore.Data.Entities;
using DutchTreatCore.Repositories;
using DutchTreatCore.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace DutchTreatCore
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;

        public Startup(IConfiguration config, IHostingEnvironment env)
        {
            _config = config;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //  Authentication
            services.AddIdentity<StoreUser, IdentityRole>(cfg=>
            {
                cfg.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<DutchContext>();

            // cookie auth and token auth
            services.AddAuthentication().AddCookie().AddJwtBearer(
                cfg =>
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = _config["Tokens:Issuer"],
                        ValidAudience = _config["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
                    }
            );

            services.AddTransient<IMailService, NullMailService>();
            services.AddTransient<DutchSeeder>();
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<IOrdersRepository, OrdersRepository>();
            
            // Support for real mail service
            services.AddMvc(opt =>
                {
                    if (_env.IsProduction())
                    {
                        opt.Filters.Add(new RequireHttpsAttribute());
                    }
                })
                .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling =  ReferenceLoopHandling.Ignore);
            services.AddDbContext<DutchContext>(cfg =>
                cfg.UseSqlServer(_config.GetConnectionString("DefaultConnectionString")));

            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/error");
 
            app.UseAuthentication();

            app.UseMvc(config =>
                config.MapRoute("Default",
                    "/{controller}/{action}/{id?}",
                    new {controller = "App", Action = "Index"}));
            if (env.IsDevelopment())
            {
                // Seed the DataBase
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var seeder =  scope.ServiceProvider.GetService<DutchSeeder>();
                    seeder.Seed().Wait();
                }
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}