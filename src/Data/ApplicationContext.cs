using EFCore.MultiTenant.Domain;
using EFCore.MultiTenant.Provider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace EFCore.MultiTenant.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Product> Products { get; set; }

        public readonly TenantData _tenant;

        public ApplicationContext(DbContextOptions<ApplicationContext> options, TenantData tenant) : base(options)
        {
            this._tenant = tenant;
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    const string strConnection = "Data Source=GUIDE-LUIZJEDI;Initial Catalog=Jedi_MultiTenant;Integrated Security=true;pooling=true";

        //    optionsBuilder
        //        .UseSqlServer(strConnection)
        //        .EnableSensitiveDataLogging()
        //        .LogTo(Console.WriteLine, LogLevel.Information);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_tenant.TenantId);

            modelBuilder.Entity<Person>().HasData(new Person { Id = 1, Name = "Person 1", TenantId = "tenant-1" });
            for (var i = 2; i <= 3; i++)
                modelBuilder.Entity<Person>().HasData(new Person { Id = i, Name = $"Person {i}", TenantId = "tenant-2" });

            modelBuilder.Entity<Product>().HasData(new Product { Id = 1, Description = "Product 1", TenantId = "tenant-1" });
            for (var i = 2; i <= 3; i++)
                modelBuilder.Entity<Product>().HasData(new Product { Id = i, Description = $"Product {i}", TenantId = "tenant-2" });

            // Filtros de consulta pelo Id da rota
            //modelBuilder.Entity<Person>().HasQueryFilter(p => p.TenantId == _tenant.TenantId);
            //modelBuilder.Entity<Product>().HasQueryFilter(p => p.TenantId == _tenant.TenantId);
        }
    }
}
