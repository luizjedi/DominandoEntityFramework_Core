using EFCore.UoWRepository.Data;
using EFCore.UoWRepository.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Linq;

namespace EFCore.UoWRepository
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        // Ignora referências cíclicas no formato Json
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EFCore.UoWRepository", Version = "v1" });
            });

            services.AddDbContext<ApplicationContext>(prov =>
                 prov.UseSqlServer("Data Source=GUIDE-LUIZJEDI;Initial Catalog=Jedi_UoWRepository;Integrated Security=true;pooling=true"));

            #region "Injection Dependency"
            var injectionContainer = new InjectionContainer(services);
            injectionContainer.InjectionDependency();
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EFCore.UoWRepository v1"));
            }

            this.StartDataBase(app);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void StartDataBase(IApplicationBuilder app)
        {
            using var db = app
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<ApplicationContext>();
            if (db.Database.EnsureCreated())
            {
                db.Departments.AddRange(Enumerable.Range(1, 10)
                    .Select(d => new Department
                    {
                        Description = $"Departamento {d}",
                        Colaborators = Enumerable.Range(1, 10)
                            .Select(c => new Colaborator
                            {
                                Name = $"Colaborador {c}/{d}"
                            }).ToList()
                    }));

                db.SaveChanges();
            }
        }
    }
}
