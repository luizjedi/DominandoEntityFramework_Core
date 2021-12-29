using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using migrations.Domain;
using System;

namespace migrations.Data
{
    class ApplicationContext : DbContext
    {
        public DbSet<Pessoa> Pessoas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=GUIDE-LUIZJEDI;Initial Catalog=Jedi_Migrations;Integrated Security=true;pooling=true";

            optionsBuilder
                    .UseSqlServer(strConnection)
                    .EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pessoa>(conf =>
                {
                    conf.HasKey(p => p.Id);
                    conf.Property(p => p.Nome).HasMaxLength(60).IsUnicode(false);
                });

        }
    }
}
