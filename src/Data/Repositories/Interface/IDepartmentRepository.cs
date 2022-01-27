using EFCore.UoWRepository.Domain;
using System.Threading.Tasks;

namespace EFCore.UoWRepository.Data.Repositories
{
    public interface IDepartmentRepository
    {
        Task<Department> GetbyIdAsunc(int id);
        void Add(Department department);
        //bool Save();
    }
}
