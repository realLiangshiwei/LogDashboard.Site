using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using LogDashboard.Site.Localization.Site;
using Volo.Abp.UI.Navigation;

namespace LogDashboard.Site.Menus
{
    public class SiteMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<SiteResource>>();

            context.Menu.Items.Insert(0, new ApplicationMenuItem("Site.Home", l["Menu:Home"], "/"));
        }
    }
}
