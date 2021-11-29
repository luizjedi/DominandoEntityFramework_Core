using System;
using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Curso.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        //Configura a string de conexão
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data source=(localdb)\\mssqllocaldb; Initial Catalog=C002;Integrated Security:true;polling:true";
            optionsBuilder
            .UseSqlServer(strConnection)
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}