using Abp.Application.Services;
using LibTads.MultiTenancy.Dto;

namespace LibTads.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

