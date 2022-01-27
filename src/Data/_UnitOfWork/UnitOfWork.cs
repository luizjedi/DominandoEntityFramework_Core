using EFCore.UoWRepository.Data.Repositories;
using EFCore.UoWRepository.Data.Repositories.Repository;

namespace EFCore.UoWRepository.Data._UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        private IDepartmentRepository _department;

        #region "GET"
        // Se não for nulo recebe _department, mas se for nulo instância _department com _context
        // Esta implementação permite que novas instâncias sejam criadas apenas quando necessário o que ocupa menos espaço na memória
        public IDepartmentRepository Department { get => _department ?? (_department = new DepartmentRepository(_context)); }
        #endregion

        public UnitOfWork(ApplicationContext context)
        {
            this._context = context;
        }

        public bool Coommit()
        {
            return this._context.SaveChanges() > 0;
        }

        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}
