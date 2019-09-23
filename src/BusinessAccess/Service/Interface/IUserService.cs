using System;
using System.Text;
using System.Threading;
using DataAccess.Entity;
using Asset.Common.ViewModel;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace BusinessAccess.Service.Interface
{
    public interface IUserService
    {
        Task<UserInfoModel> WebLoginAsync(LoginModel model, CancellationToken cancellationToken = default);
        Task<List<User>> ToListAsync(CancellationToken cancellationToken = default);
        Task<User> FindAsync(Guid Id, CancellationToken cancellationToken = default);
        Task<Guid> AddAsync(User userEntity, CancellationToken cancellationToken = default);
        Task<Guid> UpdateAsync(User userEntity, CancellationToken cancellationToken = default);
        Task DeleteAsync(User userEntity, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default);
        Task UpdateTokenAsync(Guid Id, string subcriseToken, string token, CancellationToken cancellationToken = default);
    }
}
