using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFCore.UoWRepository.Data.Repositories.Base
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationContext context)
        {
            this._dbSet = context.Set<T>();
        }

        #region "CUD"
        public void Add(T entity)
        {
            this._dbSet.Add(entity);
        }
        public void Remove(T entity)
        {
            this._dbSet.Remove(entity);
        }

        public void Update(T entity)
        {
            this._dbSet.Update(entity);

        }
        #endregion

        public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
        {
            return await this._dbSet.CountAsync(expression);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await this._dbSet.FirstOrDefaultAsync(expression);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await this._dbSet.FindAsync(id);
        }

        public async Task<List<T>> GetDataAsync(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>,
            IIncludableQueryable<T, object>> include = null, int? skip = null, int? take = null)
        {
            var query = this._dbSet.AsQueryable();

            if (expression != null)
                query = query.Where(expression);

            // Se include não for nulo ele invoca a query
            include?.Invoke(query);

            if (skip != null && skip.HasValue)
                query.Skip(skip.Value);

            if (take != null && take.HasValue)
                query.Take(take.Value).ToArray();

            return await query.ToListAsync();
        }
    }
}
