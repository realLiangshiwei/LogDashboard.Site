using LogDashboard.Site.Localization.Site;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LogDashboard.Site.Permissions
{
    public class SitePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(SitePermissions.GroupName);

            //Define your own permissions here. Examaple:
            //myGroup.AddPermission(SitePermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<SiteResource>(name);
        }
    }
}
