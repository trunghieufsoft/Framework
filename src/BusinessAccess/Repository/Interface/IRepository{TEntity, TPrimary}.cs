using System;
using System.Linq;
using System.Threading;
using DataAccess.Entity.Base;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace BusinessAccess.Repository
{
    public interface IRepository<TEntity, in TPrimary> where TEntity : BaseEntity
    {
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task AddOrUpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task AddOrUpdateAsync(TEntity entity, Expression<Func<TEntity, object>> expression, CancellationToken cancellationToken = default);
        Task<TEntity> DeleteAsync(TEntity entity, bool saveChange = true, CancellationToken cancellationToken = default);
        Task<List<TEntity>> DeleteManyAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default);
        Task<IQueryable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<IQueryable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TEntity> GetAsync(TPrimary id, bool isDelete = false, CancellationToken cancellationToken = default);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool isDelete = false, CancellationToken cancellationToken = default);
        Task<TEntity> InsertAsync(TEntity entity, bool saveChange = true, CancellationToken cancellationToken = default);
        Task<TEntity> UpdateAsync(TEntity entity, bool saveChange = true, CancellationToken cancellationToken = default);
    }
}