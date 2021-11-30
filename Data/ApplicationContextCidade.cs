using System;
using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Curso.Data
{
    public class ApplicationContextCidade : DbContext
    {
        public DbSet<Cidade> Cidades { get; set; }

        //Configura a string de conexão
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=GUIDE-LUIZJEDI;Initial Catalog=C002;Integrated Security=true;database=JEDI";
            // const string strConnection = "Data Source=rds-glb-mysql.chhnnamfif1j.us-east-1.rds.amazonaws.com;database=GEOMATICA;Pooling=true;User ID=admin;Password=pSD9Uubno8s8An4jYECK";
            optionsBuilder
            .UseSqlServer(strConnection)
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}