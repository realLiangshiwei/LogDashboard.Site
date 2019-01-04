using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using LogDashboard.Site.Localization.Site;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace LogDashboard.Site.Pages
{
    public abstract class SitePageBase : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<SiteResource> L { get; set; }
    }
}
