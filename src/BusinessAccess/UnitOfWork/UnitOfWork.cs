using System;
using System.Threading;
using DataAccess.DbContext;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BusinessAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the UnitOfWork class.
        /// </summary>
        /// <param name="contextFactory">The object context</param>
        public UnitOfWork(IDesignTimeDbContextFactory<DataDbContext> contextFactory)
        {
            _dbContext = contextFactory.CreateDbContext(new string[0]);
        }

        /// <summary>
        /// Disposes the current object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            if (disposing && _dbContext != null)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }
        }

        public async Task<int> CommitAsync(DataDbContext context, CancellationToken cancellationToken = default)
        {
            int result = 0;

            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    result = await context.SaveChangesAsync(cancellationToken);
                    transaction.Commit();
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
            return result;
        }
    }
}