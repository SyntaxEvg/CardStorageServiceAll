using Consul;
using Microsoft.Extensions.DependencyInjection;
using ServiceRegistration.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRegistration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseConul(this IServiceCollection services, string url, string datacenter = "DC")
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IConsulClient>(consul => new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(url);

            }));
            services.AddSingleton<IConsulClient, ConsulClient>(p => new
            ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(url);
                consulConfig.Datacenter = datacenter;
                //consulConfig.Token = "X-Consul-Token";
            }));
            services.AddHostedService<RegisterConsulHostedService>();
            return services;
        }

    }
}
//docker run -d -p 8500:8500 -p 8600:8600/udp --name=my-consul consul agent -server -ui -node=server-1 -bootstrap-expect=1 -client=0.0.0.0

