using System.Threading.Tasks;
using LibTads.Configuration.Dto;

namespace LibTads.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
