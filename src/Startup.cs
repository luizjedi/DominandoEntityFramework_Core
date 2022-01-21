using EFCore.MultiTenant.Data;
using EFCore.MultiTenant.Data.Interceptors;
using EFCore.MultiTenant.Middlewares;
using EFCore.MultiTenant.Provider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;

namespace EFCore.MultiTenant
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<TenantData>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EFCore.MultiTenant", Version = "v1" });
            });

            services.AddScoped<StrategySchemaInterceptor>();

            services.AddDbContext<ApplicationContext>((provider, options) =>
            {
                const string strConnection = "Data Source=GUIDE-LUIZJEDI;Initial Catalog=Jedi_MultiTenant;Integrated Security=true;pooling=true";
                options
                    .UseSqlServer(strConnection)
                    .EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Information);

                var interceptor = provider.GetRequiredService<StrategySchemaInterceptor>();

                options.AddInterceptors(interceptor);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EFCore.MultiTenant v1"));
            }

            //this.DataBaseInitialize(app);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<TenantMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        //private void DataBaseInitialize(IApplicationBuilder app)
        //{
        //    using var db = app.ApplicationServices
        //        .CreateScope()
        //        .ServiceProvider
        //        .GetRequiredService<ApplicationContext>();

        //    db.Database.EnsureDeleted();
        //    db.Database.EnsureCreated();

        //    for (var i = 1; i <= 5; i++)
        //    {
        //        db.Persons.Add(new Person { Name = $"Person {i}" });
        //        db.Products.Add(new Product { Description = $"Product {i}" });
        //    }

        //    db.SaveChanges();
        //}
    }
}
