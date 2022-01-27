using EFCore.UoWRepository.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCore.UoWRepository.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Colaborator> Colaborators { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
    }
}
