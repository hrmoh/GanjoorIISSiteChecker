class Program
{
    static void Main()
    {
        string siteName = "api.ganjoor.net"; // Replace with your actual website name
        string appPoolName = "api.ganjoor.net"; // Replace with your actual application pool name

        if (IsSiteStopped(siteName))
        {
            StartSite(siteName);
            Console.WriteLine($"Website '{siteName}' has been started.");
        }
        else
        {
            Console.WriteLine($"Website '{siteName}' is already running.");
        }

        if (IsAppPoolStopped(appPoolName))
        {
            StartAppPool(appPoolName);
            Console.WriteLine($"Application pool '{appPoolName}' has been started.");
        }
        else
        {
            Console.WriteLine($"Application pool '{appPoolName}' is already running.");
        }

        Console.ReadLine();
    }

    static bool IsSiteStopped(string siteName)
    {
        using (System.DirectoryServices.DirectoryEntry site = new($"IIS://localhost/W3SVC/{siteName}"))
        {
            int siteState = (int)site.Properties["ServerState"].Value;
            return siteState != 2; // 2 corresponds to "ServerStateStopped"
        }
    }

    static void StartSite(string siteName)
    {
        using (System.DirectoryServices.DirectoryEntry site = new($"IIS://localhost/W3SVC/{siteName}"))
        {
            _ = site.Invoke("Start", null);
            site.CommitChanges();
        }
    }

    static bool IsAppPoolStopped(string appPoolName)
    {
        using (System.DirectoryServices.DirectoryEntry appPool = new($"IIS://localhost/W3SVC/AppPools/{appPoolName}"))
        {
            int appPoolState = (int)appPool.Properties["AppPoolState"].Value;
            return appPoolState != 2; // 2 corresponds to "AppPoolStateStopped"
        }
    }

    static void StartAppPool(string appPoolName)
    {
        using (System.DirectoryServices.DirectoryEntry appPool = new($"IIS://localhost/W3SVC/AppPools/{appPoolName}"))
        {
            appPool.Invoke("Start", null);
            appPool.CommitChanges();
        }
    }
}
