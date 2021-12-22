using System;
using System.Collections.Generic;
using Atributos.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atributos.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Atributo> Atributos { get; set; }

        public DbSet<Dictionary<string, object>> Configuracoes => Set<Dictionary<string, object>>("Conficurações");

        public DbSet<Aeroporto> Aeroportos { get; set; }
        //Configura a string de conexão
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=GUIDE-LUIZJEDI;Initial Catalog=Jedi_Atributos;Integrated Security=true;pooling=true";
            optionsBuilder
                 .UseSqlServer(strConnection)
                 .EnableSensitiveDataLogging()
                 .LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

            // Sacola de Propriedade (Property Bags)
            modelBuilder.SharedTypeEntity<Dictionary<string, object>>("Configurações", b =>
            {
                b.Property<int>("Id");

                b.Property<string>("Chave")
                        .HasColumnType("VARCHAR(40)")
                        .IsRequired();

                b.Property<string>("Valor")
                        .HasColumnType("VARCHAR(255)")
                        .IsRequired(); ;
            });
        }
    }
}