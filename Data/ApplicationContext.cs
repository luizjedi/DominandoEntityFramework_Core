using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UDFs.Domain;

namespace UDFs.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Livro> Livros { get; set; }

        //Configura a string de conexão
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=GUIDE-LUIZJEDI;Initial Catalog=Jedi_UDFs;Integrated Security=true;pooling=true";
            optionsBuilder
                 .UseSqlServer(strConnection)
                 .EnableSensitiveDataLogging()
                 .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}