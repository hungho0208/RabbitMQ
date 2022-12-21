using RabbitMQ.Client;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        ConnectionFactory factory = new ConnectionFactory();
        factory.UserName = "send";
        factory.Password = "send";
        factory.VirtualHost = "/";
        factory.HostName = "localhost";
        factory.Port = AmqpTcpEndpoint.UseDefaultPort;
        IConnection connection = factory.CreateConnection();

        //using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

            string message = "Hello World!";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
            Console.WriteLine(" [x] Sent {0}", message);
        }

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}