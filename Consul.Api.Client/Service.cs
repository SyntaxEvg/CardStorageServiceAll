using Consul;
using System.Collections.Concurrent;

public class Service : IService
{
    private readonly IConfiguration _configuration;
    private readonly ConsulClient _consulClient;
    private ConcurrentBag<string> _serviceAUrls;
    private ConcurrentBag<string> _serviceBUrls;

    private IHttpClientFactory _httpClient;

    public Service(IConfiguration configuration, IHttpClientFactory httpClient)
    {
        _configuration = configuration;

        _consulClient = new ConsulClient(options =>
        {
            options.Address = new Uri(_configuration["Consul:Address"]);
        });

        _httpClient = httpClient;
    }

    public async Task<string> GetService1()
    {
        if (_serviceAUrls == null)
            return await Task.FromResult("Service1");

        using var httpClient = _httpClient.CreateClient();
        var serviceUrl = _serviceAUrls.ElementAt(new Random().Next(_serviceAUrls.Count()));
        var result = await httpClient.GetStringAsync($"{serviceUrl}/api/service1");

        return result;
    }

    public async Task<string> GetService2()
    {
        if (_serviceBUrls == null)
            return await Task.FromResult("Service2");

        using var httpClient = _httpClient.CreateClient();
        var serviceUrl = _serviceBUrls.ElementAt(new Random().Next(_serviceBUrls.Count()));
        var result = await httpClient.GetStringAsync($"{serviceUrl}/api/service2");

        return result;
    }

    public void InitServices()
    {
        var serviceNames = new string[] { "Service1", "Service2" };

        foreach (var item in serviceNames)
        {
            Task.Run(async () =>
            {
                var queryOptions = new QueryOptions
                {
                    WaitTime = TimeSpan.FromMinutes(5)
                };
                while (true)
                {
                    await InitServicesAsync(queryOptions, item);
                }
            });
        }

        async Task InitServicesAsync(QueryOptions queryOptions, string serviceName)
        {
            var result = await _consulClient.Health.Service(serviceName, null, true, queryOptions);

            if (queryOptions.WaitIndex != result.LastIndex)
            {
                queryOptions.WaitIndex = result.LastIndex;

                var services = result.Response.Select(x => $"http://{x.Service.Address}:{x.Service.Port}");

                if (serviceName == "Service1")
                {
                    _serviceAUrls = new ConcurrentBag<string>(services);
                }
                else if (serviceName == "Service2")
                {
                    _serviceBUrls = new ConcurrentBag<string>(services);
                }
            }
        }
    }
}