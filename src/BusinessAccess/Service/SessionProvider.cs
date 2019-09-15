using System;
using System.Threading;
using DataAccess.Entity;
using Asset.Common.Timing;
using System.Threading.Tasks;
using Asset.Common.Exceptions;
using BusinessAccess.UnitOfWork;
using Asset.Common.Enumerations;
using BusinessAccess.Repository;
using DataAccess.Entity.EnumType;
using Asset.Common.Linq.Extensions;
using BusinessAccess.Service.Interface;

namespace BusinessAccess.Service
{
    public class SessionProvider : BaseService, ISessionProvider
    {
        private readonly IRepository<User> _userRepo;
        private readonly ISystemConfigService _configService;

        public SessionProvider(IRepository<User> userRepo,
            ISystemConfigService configService)
        {
            _userRepo = userRepo;
            _configService = configService;
        }

        public async Task CheckSessionAsync(string token, string username, CancellationToken cancellationToken = default)
        {
            User user = await GetUserAsync(username, cancellationToken: cancellationToken);
            if (user == default(User))
            {
                throw new ExceptionError(ErrorCodeEnum.UserDeletedOrNotExisted);
            }

            //Comment for test multi browser
            //if (!user.Token.Equals(token))
            //{
            //    throw new SelfDefinedException(ErrorCodeEnum.YouHaveLogged);
            //}

            var config = await _configService.GetSystemConfigAsync(SystemConfigEnum.WebSessExpDate.ToString());
            var expiredDate = user.LoginTime.Value.AddMinutes((CaculateMinutesOfConfig(config)));

            if (Clock.Now < expiredDate)
            {
                user.LoginTime = Clock.Now;
                await _userRepo.UpdateAsync(user);
            }
            else
            {
                throw new ExceptionError(ErrorCodeEnum.SessionExpired);
            }
        }

        #region GetUser
        public async Task<User> GetUserAsync(string username, string email = null, CancellationToken cancellationToken = default)
        {
            var users = await _userRepo.GetAllAsync(cancellationToken);
            return users.WhereIf(email != null, x => x.Email.Equals(email))
                        .FindField(x => x.Username.Equals(username));
        }
        #endregion
    }
}