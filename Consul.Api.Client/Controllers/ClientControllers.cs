using Microsoft.AspNetCore.Mvc;

namespace Consul.Api.Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientControllers : ControllerBase
    {
        public ClientControllers()
        {

        }
        [HttpGet]
        [Route("getservice")]//конкретные 2 сервиса
        public async Task<IActionResult> GetService([FromServices] IService service)
        {
            return Ok(new
            {
                service1 = await service.GetService1(),
                service2 = await service.GetService2()
            });
        }
        [HttpGet]
        [Route("getservices")]
        public async Task<IActionResult> GetServices([FromServices] IConsulClient client)
        {
            var consulServiceList = client.Agent.Services().Result.Response;//x.Value.Service
            return Ok(consulServiceList);
        }
        

    }
}