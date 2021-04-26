using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FittShop.Data.Entities;
using FittShop.Data.Entities.Core;

namespace FittShop.Data.Abstracts
{
    public interface IRepository<in TKey, TEntity> where TEntity : IEntity<TKey>
    {
        Task<TEntity> GetByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes);

        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> filter);

        void Create(TEntity entity);
        void Update(TEntity entity);

        void Delete(TEntity entity);
        void Delete(TKey id);

        Task SaveAsync();
    }
    
    public interface IRepository<TEntity> where TEntity : IEntity<int>
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> filter);

        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> filter);

        void Create(TEntity entity);
        void Update(TEntity entity);

        void Delete(TEntity entity);
        void Delete(int id);

        Task SaveAsync();
    }
}