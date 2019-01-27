using LogDashboard.Models;

namespace LogDashboard.site
{
    public class DashboardDemoLogModel : RequestTraceLogModel
    {
        public string Machinename { get; set; }

        public string Callsite { get; set; }

        public string Stacktrace { get; set; }

        public string AspnetRequestIp { get; set; }
    }
}
