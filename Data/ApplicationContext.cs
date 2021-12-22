using System;
using EF_Functions.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EF_Functions.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Funcao> Funcoes { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }


        //Configura a string de conexão
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=GUIDE-LUIZJEDI;Initial Catalog=Jedi_EF_Functions;Integrated Security=true;pooling=true";
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
        }
    }
}