using Volo.Abp.Settings;

namespace LogDashboard.Site.Settings
{
    public class SiteSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(SiteSettings.MySetting1));
        }
    }
}
