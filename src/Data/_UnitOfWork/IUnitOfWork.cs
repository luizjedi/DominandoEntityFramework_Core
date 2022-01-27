using EFCore.UoWRepository.Data.Repositories;
using System;

namespace EFCore.UoWRepository.Data._UnitOfWork
{
   public interface IUnitOfWork : IDisposable
    {
        bool Coommit();
        IDepartmentRepository Department { get; }
    }
}
