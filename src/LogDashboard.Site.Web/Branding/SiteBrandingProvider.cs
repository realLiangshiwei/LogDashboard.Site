using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;

namespace LogDashboard.Site.Branding
{
    [Dependency(ReplaceServices = true)]
    public class SiteBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Site";
    }
}
