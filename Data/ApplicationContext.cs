using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stored_Procedures.Domain;

namespace Stored_Procedures.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        //Configura a string de conexão
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=GUIDE-LUIZJEDI;Initial Catalog=Jedi_StoredProcedures;Integrated Security=true;pooling=true";
            optionsBuilder
                 .UseSqlServer(strConnection)
                 .EnableSensitiveDataLogging()
                 .LogTo(Console.WriteLine, LogLevel.Information);
        }

        // Filtra as consultas de forma global, de acordo a lógica solicitada
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Departamento>().HasQueryFilter(x => !x.Excluido);
        }
    }
}