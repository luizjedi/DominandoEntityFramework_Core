using EFCore.DicasETruques.Data;
using EFCore.DicasETruques.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;

namespace EFCore.DicasETruques
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EFCore.DicasETruques", Version = "v1" });
            });

            services.AddDbContext<ApplicationContext>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EFCore.DicasETruques v1"));
            }

            ContadorDeEventos(app);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // Modelos de utilização de métodos padrões

        static void ToQueryString(IApplicationBuilder app)
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureCreated();

                var query = db.Departments.Where(x => x.Id > 2);

                var sql = query.ToQueryString();

                Console.WriteLine(sql);
            }
        }
        static void DebugView(IApplicationBuilder app)
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureCreated();

                db.Departments.Add(new Department { Description = "Teste DebugView" });

                var query = db.Departments.Where(x => x.Id > 2);
            }
        }
        static void Clear(IApplicationBuilder app)
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureCreated();

                db.Departments.Add(new Department { Description = "Teste Clear" });

                db.ChangeTracker.Clear();
            }
        }
        static void ConsultaFiltrada(IApplicationBuilder app)
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureCreated();

                var sql = db.Departments
                    .Include(p => p.Colaborators
                    .Where(c => c.Name
                    .Contains("Teste")))
                    .ToQueryString();

                Console.WriteLine(sql);
            }
        }
        static void SingleOrDefaultVsFirstOrDefault(IApplicationBuilder app)
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureCreated();

                Console.WriteLine("SingleOrDefault");

                _ = db.Departments.SingleOrDefault(p => p.Id > 2);

                Console.WriteLine("FirstOrDefault");

                _ = db.Departments.FirstOrDefault(p => p.Id > 2);
            }
        }
        static void SemChavePrimaria(IApplicationBuilder app)
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var userFunctions = db.UserFunctions.Where(p => p.UserId == Guid.NewGuid()).ToArray();
            }
        }
        static void ToView(IApplicationBuilder app)
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.Database.ExecuteSqlRaw(
                    @"CREATE VIEW vw_department_report AS
                SELECT
                    d.Description, count(c.Id) as Colaborators
                FROM Departments d 
                LEFT JOIN Colaborators c ON c.DepartmentId=d.Id
                GROUP BY d.Description");

                var departments = Enumerable.Range(1, 10)
                    .Select(p => new Department
                    {
                        Description = $"Departamento {p}",
                        Colaborators = Enumerable.Range(1, p)
                            .Select(c => new Colaborator
                            {
                                Name = $"Colaborador {p} - {c}",
                            }).ToList()
                    });

                var department = new Department { Description = "Departamento Sem Colaborador" };

                db.Departments.Add(department);
                db.Departments.AddRange(departments);
                db.SaveChanges();

                var report = db.DepartmentReports
                    .Where(p => p.Colaborators < 20)
                    .OrderBy(p => p.Department)
                    .ToList();

                foreach (var dep in report)
                {
                    Console.WriteLine($"{dep.Department} [ Colaboradores: {dep.Colaborators}]");
                }
            }
        }
        static void NaoUnicode(IApplicationBuilder app)
        {
            using (var db = new ApplicationContext())
            {
                var sql = db.Database.GenerateCreateScript();
                Console.WriteLine(sql);
            }
        }
        static void OperadoresDeAgregacao(IApplicationBuilder app)
        {
            using (var db = new ApplicationContext())
            {
                var sql = db.Departments
                    .GroupBy(g => g.Description)
                    .Select(p =>
                    new
                    {
                        Description = p.Key,
                        Count = p.Count(),
                        Average = p.Average(p => p.Id),
                        Max = p.Max(p => p.Id),
                        Sum = p.Sum(p => p.Id)
                    }).ToQueryString();

                    Console.WriteLine(sql);
            }
        }
        static void OperadoresDeAgregacaoNoAgrupamento(IApplicationBuilder app)
        {
            using (var db = new ApplicationContext())
            {
                var sql = db.Departments
                    .GroupBy(g => g.Description)
                    .Where(p => p.Count() > 1)
                    .Select(p =>
                    new
                    {
                        Description = p.Key,
                        Count = p.Count(),
                    }).ToQueryString();

                Console.WriteLine(sql);
            }
        }
        static void ContadorDeEventos(IApplicationBuilder app)
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                Console.WriteLine($" PID: {System.Diagnostics.Process.GetCurrentProcess().Id}");

                while(Console.ReadKey().Key != ConsoleKey.Escape)
                {
                    var department = new Department
                    {
                        Description = $"Departamento Sem Colaborador"
                    };

                    db.Departments.Add(department);
                    db.SaveChanges();

                    _ = db.Departments.Find(1);
                    _ = db.Departments.AsNoTracking().FirstOrDefault();
                }
            }
        }
    }
}
