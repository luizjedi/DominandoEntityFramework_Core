using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using performance.Domain;
using System;

namespace performance.Data
{
    class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=GUIDE-LUIZJEDI;Initial Catalog=Jedi_Performance;Integrated Security=true;pooling=true";

            optionsBuilder
                .UseSqlServer(strConnection)
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}
