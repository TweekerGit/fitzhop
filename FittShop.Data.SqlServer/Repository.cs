using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FittShop.Data.Abstracts;
using FittShop.Data.Entities.Core;
using Microsoft.EntityFrameworkCore;

namespace FittShop.Data.SqlServer
{
    public class Repository<TKey, TEntity> : IRepository<TKey, TEntity>
        where TEntity : class, IEntity<TKey>, new()
    {
        protected DbSet<TEntity> db;
        private AppDbContext context;

        public Repository(AppDbContext context)
        {
            this.context = context;
            this.db = (DbSet<TEntity>) context.DataSets[typeof(TEntity)];
        }

        public async Task<TEntity> GetByIdAsync(TKey id) => await this.db.FirstOrDefaultAsync(e => e.Id.Equals(id));
        public async Task<IEnumerable<TEntity>> GetAllAsync() => await this.db.ToListAsync();
        public async Task<IEnumerable<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> filter) => await this.db.Where(filter).ToListAsync();
        
        public async Task<int> CountAsync() => await this.db.CountAsync();
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter) => await this.db.CountAsync(filter);

        public void Create(TEntity entity) => this.db.Add(entity);
        public void Update(TEntity entity) => this.db.Update(entity);
        public void Delete(TEntity entity) => this.db.Remove(entity);
        public void Delete(TKey id) => this.db.Remove(new TEntity {Id = id});

        public async Task SaveAsync() => await this.context.SaveChangesAsync();
    }
    
    public class Repository<TEntity> : Repository<int, TEntity>
        where TEntity : class, IEntity<int>, new()
    {
        public Repository(AppDbContext context) : base(context) { }
    }
}