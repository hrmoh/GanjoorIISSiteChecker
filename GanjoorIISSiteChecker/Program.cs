using Microsoft.Web.Administration;

class Program
{
    static void Main()
    {
        using (ServerManager serverManager = new ServerManager())
        {
            CheckSites(serverManager, "api.ganjoor.net", "api.ganjoor.net");
            CheckSites(serverManager, "ganjoor", "ganjoor");
        }
    }

    static void CheckSites(ServerManager serverManager, string siteName, string appPoolName)
    {
        Site site = serverManager.Sites[siteName];
        ApplicationPool appPool = serverManager.ApplicationPools[appPoolName];

        if (site != null && site.State == ObjectState.Stopped)
        {
            site.Start();
            Console.WriteLine($"Website '{siteName}' has been started.");
        }
        else
        {
            Console.WriteLine($"Website '{siteName}' is already running.");
        }

        if (appPool != null && appPool.State == ObjectState.Stopped)
        {
            appPool.Start();
            Console.WriteLine($"Application pool '{appPoolName}' has been started.");
        }
        else
        {
            Console.WriteLine($"Application pool '{appPoolName}' is already running.");
        }
    }
}
