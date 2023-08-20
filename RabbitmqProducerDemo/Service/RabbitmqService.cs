using RabbitMQ.Client;
using RabbitmqProducerDemo.Model;
using System.Text;
using System.Text.Json;

namespace RabbitmqProducerDemo.Service
{
    public class RabbitmqService
    {
        public bool SendMessage(OrderRequestModel orderRequest)
        {
            //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
            var factory = new ConnectionFactory { 
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
                };

            //Create the RabbitMQ connection using connection factory details as i mentioned above
            using var connection = factory.CreateConnection();

            //Here we create channel with session and model
            using var channel = connection.CreateModel();

            //declare the queue after mentioning name and a few property related to that.
            channel.QueueDeclare(queue: "ProductOrders",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            //serialize the message
            var OrderRequestInJson = JsonSerializer.Serialize(orderRequest);
            //Encode json 
            var orderRequestBytes = Encoding.UTF8.GetBytes(OrderRequestInJson);

            //Put the data into the ProductOrders queue
            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "ProductOrders",
                                 basicProperties: null,
                                 body: orderRequestBytes);

            return true;
        }
    }
}
