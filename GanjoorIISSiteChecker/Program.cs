using Microsoft.Extensions.Configuration;
using Microsoft.Web.Administration;

// Load configuration from appsettings.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

string[] sites = configuration.GetSection("Sites").Get<string[]>();
string[] pools = configuration.GetSection("AppPools").Get<string[]>();

using (ServerManager serverManager = new ServerManager())
{
    foreach (var site in sites)
    {
        CheckSite(serverManager, site);
    }

    foreach (var pool in pools)
    {
        CheckPool(serverManager, pool);
    }

}

string[] posts = configuration.GetSection("PostCalls").Get<string[]>();
string[] gets = configuration.GetSection("GetCalls").Get<string[]>();

using (HttpClient client = new HttpClient())
{
    foreach (var post in posts)
    {
        Console.WriteLine($"Checking {post}");
        await client.PostAsync(post, null);
    }
    foreach (var get in gets)
    {
        Console.WriteLine($"Pinging {get}");
        await client.GetAsync(get);

    }
}
Console.WriteLine($"OK!");


static void CheckSite(ServerManager serverManager, string siteName)
{
    Site site = serverManager.Sites[siteName];

    if (site != null && site.State == ObjectState.Stopped)
    {
        site.Start();

    }
    else
    {
        Console.WriteLine($"Website '{siteName}' is already running.");
    }
}

static void CheckPool(ServerManager serverManager, string appPoolName)
{
    ApplicationPool appPool = serverManager.ApplicationPools[appPoolName];

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