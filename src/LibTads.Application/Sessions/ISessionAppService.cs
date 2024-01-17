using System.Threading.Tasks;
using Abp.Application.Services;
using LibTads.Sessions.Dto;

namespace LibTads.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
