using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using LibTads.Configuration.Dto;

namespace LibTads.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : LibTadsAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
