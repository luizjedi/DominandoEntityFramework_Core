using acessandoB_D.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace acessandoB_D.Data
{
    class ApplicationContext : DbContext
    {
        public DbSet<Pessoa> Pessoas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // const string strConnectionPG = "Host=localhost;Database=Jedi_AcessandoOutrosB_D;Username=postgres;Password=123";
            // const string strConnectionSqlite = "Data source=Jedi_AcessandoOutrosB_D.db"; // Basta informar o nome do arquivo a ser gerado com a extensão .db

            optionsBuilder
                    // .UseNpgsql(strConnectionPG) // Comando de conexão do PostgresSql
                    // .UseSqlite(strConnectionSqlite) // Comando de conexão do SQLite
                    // .UseInMemoryDatabase(databaseName: "Jedi_B_D") // Banco remendado somente para testes, pois não persiste os dados após o término da aplicação.
                    .UseCosmos(
                        "https://localhost:8081", // Porta de conexão (URI)
                        "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", // Primary Key
                        "Jedi_CosmosDB") // Database Name
                    .EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pessoa>(conf =>
            {
                conf.HasKey(p => p.Id);
                conf.ToContainer("Pessoas");
                // conf.Property(p => p.Nome).HasMaxLength(60).IsUnicode(false);
            });
        }
    }
}
