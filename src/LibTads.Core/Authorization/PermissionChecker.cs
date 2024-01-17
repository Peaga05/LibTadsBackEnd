using Abp.Authorization;
using LibTads.Authorization.Roles;
using LibTads.Authorization.Users;

namespace LibTads.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
