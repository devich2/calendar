using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Calendar.DAL.Abstract.IRepository.Base
{
    public interface IGenericKeyRepository<in TKey, TEntity>
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(TKey id);
        Task<List<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> GetCountAsync();
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> IsExisting(TKey id);
    }
}