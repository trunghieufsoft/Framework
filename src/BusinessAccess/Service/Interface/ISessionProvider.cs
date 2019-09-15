using DataAccess.Entity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessAccess.Service.Interface
{
    public interface ISessionProvider
    {
        Task CheckSessionAsync(string token, string username, CancellationToken cancellationToken = default);
        Task<User> GetUserAsync(string username, string email = null, CancellationToken cancellationToken = default);
    }
}