using EFCore.DicasETruques.Domain;
using EFCore.DicasETruques.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace EFCore.DicasETruques.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Colaborator> Colaborators { get; set; }
        public DbSet<UserFunction> UserFunctions { get; set; }
        public DbSet<DepartmentReport> DepartmentReports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=GUIDE-LUIZJEDI;Initial Catalog=Jedi_Dicas;Integrated Security=true;pooling=true";

            optionsBuilder
                    .UseSqlServer(strConnection)
                    .EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DepartmentReport>(e =>
            {
                e.HasNoKey();
                e.ToView("vw_department_report");
                e.Property(p => p.Department).HasColumnName("Description");
            });

            // Forçando o uso do VARCHAR
            var properties = modelBuilder.Model.GetEntityTypes()
                    .SelectMany(s => s.GetProperties())
                    .Where(p => p.ClrType == typeof(string) && p.GetColumnType() == null);
            
            foreach (var property in properties)
            {
               property.SetIsUnicode(false);
            }

            modelBuilder.ToSnakeCaseNames();
        }
    }
}
