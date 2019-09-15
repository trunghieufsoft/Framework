using System;
using System.Text;
using System.Threading;
using DataAccess.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BusinessAccess.Service.Interface
{
    public interface IUserService
    {
        Task<List<User>> ToListAsync(CancellationToken cancellationToken = default);
        Task<User> FindAsync(Guid Id, CancellationToken cancellationToken = default);
        Task<Guid> AddAsync(User userEntity, CancellationToken cancellationToken = default);
        Task<Guid> UpdateAsync(User userEntity, CancellationToken cancellationToken = default);
        Task DeleteAsync(User userEntity, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default);
    }
}
