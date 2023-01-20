﻿using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ServiceRegistration.Models;

namespace ServiceRegistration.Services
{

    public class RegisterConsulHostedService : IHostedService
    {
        //  private readonly ICacheService _cacheService;
        private CancellationTokenSource _cts;
        ServiceInfo _serviceInfo;
        private Task _executingTask;
        private IConsulClient _consulClient;

        public RegisterConsulHostedService(IConsulClient consulClient, IConfiguration config)
        {
            _consulClient = consulClient;
            _serviceInfo = new ServiceInfo();
            var sc = config.GetSection("serviceInfo");

            _serviceInfo.Id = sc["id"];
            _serviceInfo.Name = sc["name"];
            _serviceInfo.IP = sc["ip"];
            _serviceInfo.HealthCheckAddress = sc["HealthCheckAddress"];
            _serviceInfo.Port = int.Parse(sc["Port"]);

            _consulClient = consulClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var ursl = $"http://{_serviceInfo.IP}:{_serviceInfo.Port}{_serviceInfo.HealthCheckAddress}";
            Console.WriteLine($"start to register service {_serviceInfo.Id} to consul client ...");
            await _consulClient.Agent.ServiceDeregister(_serviceInfo.Id, cancellationToken);
            await _consulClient.Agent.ServiceRegister(new AgentServiceRegistration
            {
                ID = _serviceInfo.Id,
                Name = _serviceInfo.Name,
                Address = _serviceInfo.IP,
                Port = _serviceInfo.Port,
                Check = new AgentServiceCheck()
                {
                    Status = HealthStatus.Passing,
                    //DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(0),// Указывает, что проверяет связанные со службой должны отменить регистрацию по истечении этого времени. Это указано как продолжительность времени с суффиксом, например, «10 м». Если чек находится в критическом состоянии больше, чем это настроенное значение, то связанная с ним служба (и все связанные с ним чеки) будут автоматически сняты с регистрации. Минимальный тайм-аут составляет 1 минуту, а процесс, который собирает критические сервисы, запускается каждые 30 минут. секунд, поэтому для срабатывания может потребоваться немного больше времени, чем настроенное время ожидания. снятие с учета. Обычно это должно быть настроено с тайм-аутом, который намного, намного дольше, чем любой ожидаемый восстанавливаемый сбой для данной службы
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(5),
                    HTTP = ursl,
                    // Timeout = TimeSpan.FromSeconds(5)
                }
            });

            Console.WriteLine("register service info to consul client Successful ...");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _consulClient.Agent.ServiceDeregister(_serviceInfo.Id, cancellationToken);
            _consulClient?.Dispose();
            Console.WriteLine($"Deregister service {_serviceInfo.Id} from consul client Successful ...");
        }
    }
}