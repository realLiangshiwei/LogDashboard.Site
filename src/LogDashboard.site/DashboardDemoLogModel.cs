using LogDashboard.Model;

namespace LogDashboard.site
{
    public class DashboardDemoLogModel : LogModel
    {
        public string Machinename { get; set; }

        public string Callsite { get; set; }

        public string Stacktrace { get; set; }

        public string AspnetRequestIp { get; set; }
    }
}
