using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RabbitMQController : ControllerBase
    {


        private readonly ILogger<RabbitMQController> _logger;
        private readonly IRabbitMQApi _rabbitMQApi;

        public RabbitMQController(ILogger<RabbitMQController> logger, IRabbitMQApi rabbitMQApi)
        {
            _logger = logger;
            _rabbitMQApi = rabbitMQApi;
        }

        [HttpGet]
        public string Get()
        {
            return _rabbitMQApi.GetVHosts().Result;
        }

    }
}
