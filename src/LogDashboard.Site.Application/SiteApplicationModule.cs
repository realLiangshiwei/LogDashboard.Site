using LogDashboard.Site.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace LogDashboard.Site
{
    [DependsOn(
        typeof(SiteDomainModule),
        typeof(AbpIdentityApplicationModule))]
    public class SiteApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add<SitePermissionDefinitionProvider>();
            });

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<SiteApplicationAutoMapperProfile>();
            });
        }
    }
}
