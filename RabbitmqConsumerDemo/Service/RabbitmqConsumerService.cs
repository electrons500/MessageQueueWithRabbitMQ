using Microsoft.Extensions.Caching.Memory;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitmqConsumerDemo.Service
{
    public class RabbitmqConsumerService
    {
        private string queueName = "ProductOrders";
        private string cacheKey = "ProductOrdersKey";
        private IMemoryCache _cache;
        public RabbitmqConsumerService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void RecieveMessage() 
        {

            //defining imemory cache properties
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(20))
                                                                 .SetAbsoluteExpiration(TimeSpan.FromSeconds(30))
                                                                 .SetPriority(CacheItemPriority.Normal)
                                                                 .SetSize(1024);
          
            //defining rabbitmq
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672, 
                UserName = "guest",
                Password = "guest"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            
            var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, eventArgs) =>
                {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    //pass data from rabbitmq to in-memory cache
                    _cache.Set(cacheKey, message, cacheEntryOptions);
                };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

        }

    }
}
