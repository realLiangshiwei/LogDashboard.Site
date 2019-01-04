using System;

namespace LogDashboard.Site.Permissions
{
    public static class SitePermissions
    {
        public const string GroupName = "Site";

        //Add your own permission names. Example:
        //public const string MyPermission1 = GroupName + ".MyPermission1";

        public static string[] GetAll()
        {
            //Return an array of all permissions
            return Array.Empty<string>();
        }
    }
}