using DataAccess.Entity;
using System.Threading.Tasks;

namespace BusinessAccess.Service.Interface
{
    public interface ISystemConfigService
    {
        Task<SystemConfiguration> GetSystemConfigAsync(string key);
    }
}