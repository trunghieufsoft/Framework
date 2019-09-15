using System;
using Serilog;
using System.Linq;
using System.Threading;
using DataAccess.Entity;
using System.Threading.Tasks;
using System.Linq.Expressions;
using BusinessAccess.Repository;
using System.Collections.Generic;
using BusinessAccess.Service.Interface;
using Asset.Common.Exceptions;

namespace BusinessAccess.Service
{
    public class UserService : BaseService, IUserService
    {
        private readonly IRepository<User> _userRepo;

        public UserService(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
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
                userEntity = await GetModifiedAsync(code, userEntity);
                var user = await _userRepo.UpdateAsync(userEntity, cancellationToken: cancellationToken);
                return user.Id;
            }
            catch (Exception e)
            {
                Log.Error(e, $"Can't modified data, error: { e.Message }");
                throw new ExceptionError(Asset.Common.Enumerations.ErrorCodeEnum.FailData, e.Message);
            }
        }

        private async Task<User> GetModifiedAsync(string code, User userEntity)
        {
            if (!await _userRepo.AnyAsync(x => x.Code == code))
                throw new ExceptionError(Asset.Common.Enumerations.ErrorCodeEnum.UserModifiedNotExits);

            var temp = await _userRepo.GetAsync(x => x.Code == code);
            userEntity.Id = temp.Id;
            userEntity.Code = temp.Code;
            userEntity.Username = temp.Username;
            return userEntity;
        }
    }
}
