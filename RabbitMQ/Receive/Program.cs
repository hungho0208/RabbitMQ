

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        //建立接收者連線資訊
        ConnectionFactory factory = new ConnectionFactory();
        factory.UserName = "receive";
        factory.Password = "receive";
        factory.VirtualHost = "/";
        factory.HostName = "localhost";
        factory.Port = AmqpTcpEndpoint.UseDefaultPort;
        IConnection connection = factory.CreateConnection();

        //產生通道
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };
            channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

        }
    }
}