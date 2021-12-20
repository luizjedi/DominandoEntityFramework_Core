using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Modelo_de_Dados.Configurations;
using Modelo_de_Dados.Domain;
using static Modelo_de_Dados.Enums._Versao;

namespace Modelo_de_Dados.Data
{
    public class ApplicationContextIndice : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Conversor> Conversores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Ator> Atores { get; set; }
        public DbSet<Filme> Filmes { get; set; }


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
            // modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContextIndice).Assembly);

            // modelBuilder.Entity<Cliente>(p =>
            // {
            //     p.OwnsOne(x => x.Endereco, end =>
            //     {
            //         end.Property(x => x.Bairro).HasColumnName("Bairro"); // Personaliza o nome a ser criado na tabela do banco de dados
            //         end.Property(x => x.Logradouro).HasColumnName("Logradouro");
            //         end.Property(x => x.Cidade).HasColumnName("Cidade");
            //         end.Property(x => x.Estado).HasColumnName("Estado");

            //         end.ToTable("Endereco");
            //     });
            // });
            // modelBuilder
            //     .Entity<Departamento>()
            //     .HasIndex(x => new { x.Descricao, x.Ativo })
            //     .HasDatabaseName("idx_meu_indice_composto")
            //     .HasFilter("Descricao IS NOT NULL")
            //     .HasFillFactor(80)
            //     .IsUnique();
            // modelBuilder.Entity<Estado>().HasData(new[]
            // {
            //     new Estado { Id = 1, Nome = "Piauí"},
            //     new Estado { Id = 2, Nome = "Sergipe"},
            // });
            // modelBuilder.HasDefaultSchema("cadastros");
            // modelBuilder.Entity<Conversor>().ToTable("Estados", "Segundo esquema");

            // var conversao = new ValueConverter<Versao, string>(x => x.ToString(), x => (Versao)Enum.Parse(typeof(Versao), x));
            // Microsoft.EntityFrameworkCore.Storage.ValueConversion.

            // var conversao1 = new EnumToStringConverter<Versao>();

            // modelBuilder.Entity<Conversor>()
            //         .Property(p => p.Versao)
            //         .HasConversion(conversao1);
            // .HasConversion(conversao);
            // .HasConversion(x => x.ToString(), x => (Versao)Enum.Parse(typeof(Versao), x));
            // .HasConversion<string>();

            // modelBuilder.Entity<Conversor>()
            //         .Property(p => p.Status)
            //         .HasConversion(new Modelo_de_Dados.Conversores.ConversorCustomizado());

            // modelBuilder.Entity<Departamento>().Property<DateTime>("UltimaAtualizacao"); // Configura uma Shadow Propertie
        }
    }
}