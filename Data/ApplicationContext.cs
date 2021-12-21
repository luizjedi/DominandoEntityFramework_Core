using System;
using Atributos.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atributos.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

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
            // Aplica ao banco de dados com a configuração abaixo.
            // LUIZ => luiz , com essa collation o EF_Core consegue realizar as buscas ignorando caixa alta e acentuação.
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");

            // Aplica a tabela de Departamento a configuração abaixo.
            // LUIZ => luiz , com essa collation o EF_Core irá diferenciar as letras maiúsculas e os acentos no momento da busca.
            modelBuilder.Entity<Departamento>().Property(p => p.Descricao).UseCollation("SQL_Latin1_General_CP1_CS_AS");

            // Criando Sequências customizadas
            modelBuilder.HasSequence<int>("MinhaSequencia", "sequencias")
                     .StartsAt(1) // Inicia a sequência a partir do valor desejado
                     .IncrementsBy(2) // Define quanto deverá ser incrementado a cada sequência
                     .HasMin(1) // Define um valor mínimo para a sequência
                     .HasMax(10) // Define um valor máximo para a sequência 
                     .IsCyclic(); // Ao atingir o valor máximo esse método reinicia a contagem das sequências a partir do valor mínimo
            modelBuilder.Entity<Departamento>().Property(p => p.Id).HasDefaultValueSql("NEXT VALUE FOR sequencias.MinhaSequencia");

        }
    }
}