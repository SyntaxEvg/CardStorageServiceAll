using DataKey.Properties;

internal partial class Program
{
    private static void Main(string[] args)
    {
#if DEBUG
        Console.Title = Resources.ApplicationNameDebug;
#else
            Console.Title = Resources.ApplicationName;
#endif
        CacheProvider cacheProvider = new CacheProvider();
        List<ConnectionString> connections = cacheProvider.GetConnectionFromCache();

        foreach (var connection in connections)
        {
            Console.WriteLine($"{connection.Host} {connection.DatabaseName} {connection.UserName} {connection.Password}");
        }
        Console.ReadKey();
    }
}