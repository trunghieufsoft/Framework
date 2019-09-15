using System.Threading;
using Asset.Common.Provider;
using System.Threading.Tasks;

namespace BusinessAccess.Service.Interface
{
    public interface IEmailProvider
    {
        Task SendMail(MailInfo info, CancellationToken cancellationToken = default);
    }
}