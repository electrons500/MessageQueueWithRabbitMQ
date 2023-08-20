using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;


string queueName = "ProductOrders";

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
    Console.WriteLine($"Product message received: {message}");
   
};
channel.BasicConsume(queue: queueName,
                     autoAck: true,
                     consumer: consumer);

Console.ReadKey();
