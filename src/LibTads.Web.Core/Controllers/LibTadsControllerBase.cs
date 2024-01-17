using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace LibTads.Controllers
{
    public abstract class LibTadsControllerBase: AbpController
    {
        protected LibTadsControllerBase()
        {
            LocalizationSourceName = LibTadsConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
