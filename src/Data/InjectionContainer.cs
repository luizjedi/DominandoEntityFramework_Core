using EFCore.UoWRepository.Data._UnitOfWork;
using EFCore.UoWRepository.Data.Repositories;
using EFCore.UoWRepository.Data.Repositories.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.UoWRepository.Data
{
    public class InjectionContainer
    {
        private readonly IServiceCollection _services;

        public InjectionContainer(IServiceCollection services)
        {
            this._services = services;
        }

        public void InjectionDependency()
        {
            this._services.AddScoped<IUnitOfWork, UnitOfWork>();

            #region "Interfaces X Entity"
            this._services.AddScoped<IDepartmentRepository, DepartmentRepository>();
#endregion
        }
    }
}
