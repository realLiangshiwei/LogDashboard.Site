using Volo.Abp;

namespace LogDashboard.Site
{
    public abstract class SiteApplicationTestBase : AbpIntegratedTest<SiteApplicationTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
