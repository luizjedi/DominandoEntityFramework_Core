using EFCore.MultiTenant.Data;
using EFCore.MultiTenant.Data.Interceptors;
using EFCore.MultiTenant.Data.ModelFactory;
using EFCore.MultiTenant.Extensions;
using EFCore.MultiTenant.Middlewares;
using EFCore.MultiTenant.Provider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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

            // Estrategia 1 - Identificador na Tabela

            //services.AddScoped<StrategySchemaInterceptor>();

            //services.AddDbContext<ApplicationContext>((provider, options) =>
            //{
            //    const string strConnection = "Data Source=GUIDE-LUIZJEDI;Initial Catalog=Jedi_MultiTenant;Integrated Security=true;pooling=true";
            //    options
            //        .UseSqlServer(strConnection)
            //        .EnableSensitiveDataLogging()
            //        .LogTo(Console.WriteLine, LogLevel.Information);

            // Estrategia 2 - Schema

            //services.AddDbContext<ApplicationContext>((provider, options) =>
            //{
            //    const string strConnection = "Data Source=GUIDE-LUIZJEDI;Initial Catalog=Jedi_MultiTenant;Integrated Security=true;pooling=true";
            //    options
            //        .UseSqlServer(strConnection)
            //        .EnableSensitiveDataLogging()
            //        .ReplaceService<IModelCacheKeyFactory, StrategySchemaModelCacheKey>()
            //        .LogTo(Console.WriteLine, LogLevel.Information);

            //var interceptor = provider.GetRequiredService<StrategySchemaInterceptor>();
            //options.AddInterceptors(interceptor);
            //});

            // Estrategia 3 - Banco de Dados

            services.AddHttpContextAccessor();

            services.AddScoped<ApplicationContext>(provider =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();

                var httpContext = provider.GetService<IHttpContextAccessor>()?.HttpContext;

                var tenantId = httpContext?.GetTenantId();

                //var connectionString = Configuration.GetConnectionString(tenantId);
                var connectionString = Configuration.GetConnectionString("custom").Replace("Jedi_DATABASE", tenantId);

                optionsBuilder
                    .UseSqlServer(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging();

                return new ApplicationContext(optionsBuilder.Options);
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

            //app.UseMiddleware<TenantMiddleware>();

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
