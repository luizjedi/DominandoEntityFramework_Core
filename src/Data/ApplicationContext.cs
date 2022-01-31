using EFCore.DicasETruques.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCore.DicasETruques.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Colaborator> Colaborators { get; set; }


    }
}
