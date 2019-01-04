using LogDashboard.Site.Localization.Site;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace LogDashboard.Site.Pages
{
    public abstract class SitePageModelBase : AbpPageModel
    {
        protected SitePageModelBase()
        {
            LocalizationResourceType = typeof(SiteResource);
        }
    }
}