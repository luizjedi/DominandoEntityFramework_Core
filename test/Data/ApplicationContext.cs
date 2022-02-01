using EFCore.Testes.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Testes.Data
{
    class ApplicationContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
    }
}
