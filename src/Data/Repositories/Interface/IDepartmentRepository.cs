using EFCore.UoWRepository.Data.Repositories.Base;
using EFCore.UoWRepository.Domain;

namespace EFCore.UoWRepository.Data.Repositories
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        //Task<Department> GetbyIdAsunc(int id);
        //void Add(Department department);
        //bool Save();
    }
}
