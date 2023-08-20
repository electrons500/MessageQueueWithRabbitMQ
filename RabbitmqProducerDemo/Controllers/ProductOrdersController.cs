using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitmqProducerDemo.Model;
using RabbitmqProducerDemo.Service;

namespace RabbitmqProducerDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductOrdersController : ControllerBase
    {
        private RabbitmqService _rabbitmqService;
        List<OrderRequestModel> orderRequests = new();  
        public ProductOrdersController(RabbitmqService rabbitmqService)
        {
            _rabbitmqService = rabbitmqService;
        }

        [HttpPost]
        public ActionResult Get([FromBody] OrderRequestModel order)
        {
            //In memory db
            orderRequests.Add(order);

            //send order to rabbitmq
            bool sentOrder = _rabbitmqService.SendMessage(order);
            if (sentOrder)
            {
                return Ok("Order sent to RabbitMQ");
            }
            return BadRequest("Order faild to be sent to RabbitMQ");
        }
    }
}
