using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RabbitmqConsumerDemo.Model;
using RabbitmqConsumerDemo.Service;
using System.Text.Json;

namespace RabbitmqConsumerDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumerController : ControllerBase
    {
        private RabbitmqConsumerService _RabbitmqConsumerService;
        private IMemoryCache _cache;
        private readonly string cacheKey = "ProductOrdersKey";
        public ConsumerController(RabbitmqConsumerService rabbitmqConsumerService, IMemoryCache cache)
        {
            _RabbitmqConsumerService = rabbitmqConsumerService;
            _cache = cache;
        }

        [HttpGet]
        public ActionResult Get()
        {
             _RabbitmqConsumerService.RecieveMessage();
            //get data from cache
           bool IsOrdersExist = _cache.TryGetValue(cacheKey, out _);
            if (IsOrdersExist)
            {
                //Deserialize json and return result
                var json = _cache.Get(cacheKey).ToString();
                OrderRequestModel order = JsonSerializer.Deserialize<OrderRequestModel>(json);
                return Ok(order);
            }

           return NotFound();
        }
    }
}
