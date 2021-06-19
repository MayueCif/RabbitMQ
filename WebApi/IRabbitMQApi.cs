using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    [Headers("ContentType:application/json", "Authorization: Basic")]
    public interface IRabbitMQApi
    {
        [Get("/vhosts/")]
        Task<string> GetVHosts();
    }
}
