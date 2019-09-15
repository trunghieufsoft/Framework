using System;
using System.Linq;
using DataAccess.Entity.Base;
using System.Linq.Expressions;
using Asset.Common.Extensions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DataDbContext = DataAccess.DbContext.DataDbContext;
using BusinessAccess.UnitOfWork;
using System.Threading.Tasks;
using System.Threading;
using Serilog;

namespace BusinessAccess.Repository
{
    public class Repository<TEntity, TPrimary> : IRepository<TEntity, TPrimary> where TEntity : BaseEntity
    {
        protected readonly DataDbContext context;
        protected readonly DbSet<TEntity> entities;
        protected readonly IUnitOfWork _unitOfWork;

        public Repository(IDesignTimeDbContextFactory<DataDbContext> dbContextFactory, IUnitOfWork unitOfWork)
        {
            this.context = dbContextFactory.CreateDbContext(new string[0]);
            entities = context.Set<TEntity>();
            _unitOfWork = unitOfWork;
        }

        public virtual Task<IQueryable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(entities.AsQueryable());
        }

        public virtual Task<TEntity> GetAsync(TPrimary id, bool isDelete = false, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(entities.AsNoTracking().FirstOrDefault(s => s.Id.ToString() == id.ToString() && (!s.IsDeleted || isDelete)));
        }

        public virtual Task<IQueryable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(entities.AsNoTracking().Where(predicate));
        }

        public virtual Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool isDelete = false, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(entities.AsNoTracking().FirstOrDefault(predicate));
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity, bool saveChange = true, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.Entry(entity).State = EntityState.Added;
            await SaveAsync(saveChange, cancellationToken);

            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, bool saveChange = true, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            await SaveAsync(saveChange, cancellationToken);

            return entity;
        }

        public virtual async Task<TEntity> DeleteAsync(TEntity entity, bool saveChange = true, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.Entry(entity).State = EntityState.Deleted;
            await SaveAsync(saveChange, cancellationToken);

            return entity;
        }

        public virtual async Task<List<TEntity>> DeleteManyAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
        {
            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    IEnumerable<TEntity> objects = entities.Where(where).AsEnumerable();
                    foreach(TEntity entity in objects)
                    {
                        await DeleteAsync(entity, false, cancellationToken);
                    }
                    await context.SaveChangesAsync(cancellationToken);
                    transaction.Commit();

                    return objects.ToList();
                }
                catch (DbUpdateConcurrencyException)
                {
                    transaction.Rollback();
                    throw;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new DbUpdateException("There is an error when commit transaction.", e);
                }
            }
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(entities.Any(predicate));
        }

        public virtual async Task AddOrUpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            entities.AddOrUpdate(entity);
            await SaveAsync(cancellationToken: cancellationToken);
        }

        public virtual async Task AddOrUpdateAsync(TEntity entity, Expression<Func<TEntity, object>> expression, CancellationToken cancellationToken = default)
        {
            entities.AddOrUpdate(expression, entity);
            await SaveAsync(cancellationToken: cancellationToken);
        }

        public virtual async Task SaveAsync(bool saveChange = true, CancellationToken cancellationToken = default)
        {
            if (saveChange)
                await _unitOfWork.CommitAsync(context, cancellationToken);
        }
    }
}
