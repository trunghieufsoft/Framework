using System;
using Serilog;
using System.Linq;
using System.Threading;
using DataAccess.Entity;
using Asset.Common.Services;
using Asset.Common.ViewModel;
using System.Threading.Tasks;
using Asset.Common.Exceptions;
using System.Linq.Expressions;
using Asset.Common.Enumerations;
using BusinessAccess.Repository;
using System.Collections.Generic;
using DataAccess.Entity.EnumType;
using BusinessAccess.Service.Interface;
using Asset.Common.Timing;

namespace BusinessAccess.Service
{
    public class UserService : BaseService, IUserService
    {
        private readonly IRepository<User> _userRepo;
        private readonly ISystemConfigService _sysConfig;
        public UserService(IRepository<User> userRepo, ISystemConfigService sysConfig)
        {
            _userRepo = userRepo;
            _sysConfig = sysConfig;
        }
        
        public async Task<UserInfoModel> WebLoginAsync(LoginModel model, CancellationToken cancellationToken = default)
        {
            Log.Error("Web login: {Username}/{Password}", model.Username, model.Password);
            User user = await _userRepo.GetAsync(x => x.Username.Equals(model.Username), cancellationToken: cancellationToken);
            return await LoginAsync(user, model.Password, cancellationToken);
        }

        public async Task<Guid> AddAsync(User userEntity, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _userRepo.InsertAsync(userEntity, cancellationToken: cancellationToken);
                return user.Id;
            }
            catch (Exception e)
            {
                Log.Error(e, $"Can't insert data into DB, error: { e.Message }");
                throw;
            }
        }

        public async Task<bool> AnyAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default)
            => await _userRepo.AnyAsync(predicate, cancellationToken);

        public async Task DeleteAsync(User userEntity, CancellationToken cancellationToken = default)
        {
            try
            {
                await _userRepo.DeleteAsync(userEntity, cancellationToken: cancellationToken);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Can't remove data { userEntity.Code }, error: { e.Message}");
                throw;
            }
        }

        public async Task<User> FindAsync(Guid Id, CancellationToken cancellationToken = default)
            => await _userRepo.GetAsync(x => x.Id.Equals(Id), cancellationToken: cancellationToken);

        public async Task<List<User>> ToListAsync(CancellationToken cancellationToken = default)
        {
            var Users = await _userRepo.GetAllAsync(cancellationToken);
            return Users.ToList();
        }

        public async Task<Guid> UpdateAsync(User userEntity, CancellationToken cancellationToken = default)
        {
            try
            {
                var code = string.Join("", userEntity.Code.Split("-"));
                userEntity = await GetModifiedAsync(code, userEntity, cancellationToken: cancellationToken);
                var user = await _userRepo.UpdateAsync(userEntity, cancellationToken: cancellationToken);
                return user.Id;
            }
            catch (Exception e)
            {
                Log.Error(e, $"Can't modified data, error: { e.Message }");
                throw new ExceptionError(Asset.Common.Enumerations.ErrorCodeEnum.FailData, e.Message);
            }
        }

        public async Task UpdateTokenAsync(Guid Id, string subcriseToken, string token, CancellationToken cancellationToken = default)
        {
            User user = await _userRepo.GetAsync(Id, cancellationToken: cancellationToken);

            if (user != null)
            {
                user.Token = token;
                user.SubcriseToken = subcriseToken;
                if (string.IsNullOrEmpty(token))
                    user.LoginTime = null;
                else
                    user.LoginTime = DateTime.Now;
                //await _userRepo.UpdateAsync(user, cancellationToken: cancellationToken);
            }
        }

        #region Mothod
        private async Task<UserInfoModel> LoginAsync(User user, string password, CancellationToken cancellationToken = default)
        {
            if (user != null)
            {
                string passEncrypt = EncryptService.Encrypt(password);
                if (user.Password.Equals(passEncrypt))
                {
                    if (user.Status != StatusEnum.Inactive && (user.LoginFailedNumber == null || (user.LoginFailedNumber != null && user.LoginFailedNumber.Value < _maxLogin)))
                    {
                        user.LoginFailedNumber = 0;
                        var model = UserInfoModel.Instance;
                        model.Fullname = user.FullName;
                        model.Id = user.Id;
                        model.UserType = user.UserTypeStr;
                        model.Username = user.Username;
                        model.PasswordLastUpdate = user.PasswordLastUdt;
                        model = await CheckExpiredPass(model);
                        await _userRepo.UpdateAsync(user);
                        // _logService.Synchronization(user.Username);
                        return model;
                    }
                    else
                    {
                        throw new ExceptionError(ErrorCodeEnum.UserInactive);
                    }
                }
                else
                {
                    if (user.LoginFailedNumber != null && user.LoginFailedNumber.Value >= (_maxLogin - 1) && user.Status != StatusEnum.Inactive)
                    {
                        user.Status = StatusEnum.Inactive;
                        user.LoginFailedNumber = 0;
                        await _userRepo.UpdateAsync(user);
                        throw new ExceptionError(ErrorCodeEnum.LoginFailed3Time);
                    }
                    if (user.Status == StatusEnum.Inactive)
                    {
                        throw new ExceptionError(ErrorCodeEnum.UserInactive);
                    }
                    user.LoginFailedNumber = user.LoginFailedNumber == null ? 1 : user.LoginFailedNumber.Value + 1;
                    await _userRepo.UpdateAsync(user);
                    throw new ExceptionError(ErrorCodeEnum.LoginFailed);
                }
            }
            else
            {
                Log.Information("User does not existed!", ErrorCodeEnum.IncorrectUser);
                throw new ExceptionError(ErrorCodeEnum.IncorrectUser);
            }
        }

        private async Task<User> GetModifiedAsync(string code, User userEntity, CancellationToken cancellationToken = default)
        {
            if (!await _userRepo.AnyAsync(x => x.Code == code))
                throw new ExceptionError(Asset.Common.Enumerations.ErrorCodeEnum.UserModifiedNotExits);

            var temp = await _userRepo.GetAsync(x => x.Code == code, cancellationToken: cancellationToken);
            userEntity.Id = temp.Id;
            userEntity.Code = temp.Code;
            userEntity.Username = temp.Username;
            return userEntity;
        }

        private async Task<UserInfoModel> CheckExpiredPass(UserInfoModel model)
        {
            var key = model.UserType == UserTypeEnum.Staff.ToString() ? SystemConfigEnum.AppPassExpDate.ToString() : SystemConfigEnum.WebPassExpDate.ToString();
            var config = await _sysConfig.GetSystemConfigAsync(key);
            if (config == null || (config.LastModifiedTime.HasValue && config.LastModifiedTime.Value.Date == Clock.Now.Date))
            {
                model.ExpiredPassword = "30";
                return model;
            }
            if (!model.PasswordLastUpdate.HasValue)
            {
                var account = await _userRepo.GetAsync(x => x.Id.Equals(model.Id));
                if (account != null)
                {
                    account.PasswordLastUdt = model.PasswordLastUpdate = Clock.Now;
                    await _userRepo.UpdateAsync(account);
                }
            }
            var expiredDate = model.PasswordLastUpdate.Value.AddDays((CaculateDayOfConfig(config)));
            var now = Clock.Now;
            if (expiredDate.Date < now.Date && model.UserType == UserTypeEnum.Staff.ToString())
            {
                throw new ExceptionError(ErrorCodeEnum.PasswordExpired, model.Username);
            }
            else
            {
                model.ExpiredPassword = Math.Round((expiredDate - now).TotalDays, 0).ToString();
            }

            return model;
        }
        #endregion
    }
}
