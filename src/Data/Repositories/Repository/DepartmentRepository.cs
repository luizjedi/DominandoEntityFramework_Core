using EFCore.UoWRepository.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EFCore.UoWRepository.Data.Repositories.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<Department> _dbSet;

        public DepartmentRepository(ApplicationContext context)
        {
            this._context = context;
            this._dbSet = _context.Set<Department>();
        }

        public void Add(Department department)
        {
            _dbSet.Add(department);
        }

        public async Task<Department> GetbyIdAsunc(int id)
        {
            return await _dbSet
                .Include(x => x.Colaborators)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        //public bool Save()
        //{
        //    return this._context.SaveChanges() > 0;
        //}
    }
}
