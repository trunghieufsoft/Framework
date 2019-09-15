using System;
using DataAccess.Entity;
using System.Threading.Tasks;
using Asset.Common.Exceptions;
using BusinessAccess.Repository;
using BusinessAccess.Service.Interface;

namespace BusinessAccess.Service
{
    public class SystemConfigService : BaseService, ISystemConfigService
    {
        private readonly IRepository<SystemConfiguration> _systemConfigRepo;

        public SystemConfigService(IRepository<SystemConfiguration> systemConfigRepo)
        {
            _systemConfigRepo = systemConfigRepo;
        }

        public async Task<SystemConfiguration> GetSystemConfigAsync(string key)
        {
            try
            {
                return await _systemConfigRepo.GetAsync(x => x.KeyStr.Equals(key));
            }
            catch (Exception e)
            {
                throw new NotFound("Cannot get system configuration {e}", e);
            }
        }
    }
}