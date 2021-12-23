using System;
using Interceptacao.Domain;
using Interceptacao.Interceptadores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Interceptacao.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Funcao> Funcoes { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }


        //Configura a string de conexão
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=GUIDE-LUIZJEDI;Initial Catalog=Jedi_Interceptacao;Integrated Security=true;pooling=true";
            optionsBuilder
                 .UseSqlServer(strConnection)
                 .EnableSensitiveDataLogging()
                 .AddInterceptors(new InterceptadorDeComandos())
                 .AddInterceptors(new InterceptadorDeConexao())
                 .AddInterceptors(new InterceptadorDePersistencia())
                 .LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}