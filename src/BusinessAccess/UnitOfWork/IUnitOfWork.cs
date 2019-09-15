using System;
using System.Threading;
using DataAccess.DbContext;
using System.Threading.Tasks;

namespace BusinessAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        ///     Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>

        Task<int> CommitAsync(DataDbContext context, CancellationToken cancellationToken = default);
    }
}