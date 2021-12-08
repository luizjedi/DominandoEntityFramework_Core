using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modelo_de_Dados.Domain;

namespace Modelo_de_Dados.Data
{
    public class ApplicationContextIndice : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Estado> Estados { get; set; }

        //Configura a string de conexão
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=GUIDE-LUIZJEDI;Initial Catalog=Jedi_Modelo_de_Dados;Integrated Security=true;pooling=true";
            optionsBuilder
                 .UseSqlServer(strConnection)
                 .EnableSensitiveDataLogging()
                 .LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder
            //     .Entity<Departamento>()
            //     .HasIndex(x => new { x.Descricao, x.Ativo })
            //     .HasDatabaseName("idx_meu_indice_composto")
            //     .HasFilter("Descricao IS NOT NULL")
            //     .HasFillFactor(80)
            //     .IsUnique();
            modelBuilder.Entity<Estado>().HasData(new[]
            {
                new Estado { Id = 1, Nome = "Piauí"},
                new Estado { Id = 2, Nome = "Sergipe"},
            });
        }
    }
}