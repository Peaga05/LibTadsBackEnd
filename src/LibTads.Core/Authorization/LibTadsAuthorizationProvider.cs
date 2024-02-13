using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace LibTads.Authorization
{
    public class LibTadsAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Autores, L("Autores"));
            context.CreatePermission(PermissionNames.Pages_Generos, L("Generos"));
            context.CreatePermission(PermissionNames.Pages_Livros, L("Livros"));
            context.CreatePermission(PermissionNames.Pages_Emprestimos, L("Emprestimos"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, LibTadsConsts.LocalizationSourceName);
        }
    }
}
