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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EFCore.UoWRepository", Version = "v1" });
            });

            services.AddDbContext<ApplicationContext>(prov =>
            prov.UseSqlServer("Data Source=GUIDE-LUIZJEDI;Initial Catalog=Jedi_UoWRepository;Integrated Security=true;pooling=true"));
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
                db.Departamentos.AddRange(Enumerable.Range(1, 10)
                    .Select(d => new Departamento
                    {
                        Descricao = $"Departamento {d}",
                        Colaboradores = Enumerable.Range(1, 10)
                            .Select(c => new Colaborador
                            {
                                Nome = $"Colaborador {c}/{d}"
                            }).ToList()
                    }));

                db.SaveChanges();
            }
        }
    }
}
