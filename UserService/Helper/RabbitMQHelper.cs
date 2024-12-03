using Hangfire;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;
namespace UserService.Helper
{
    public class RabbitMQHelper
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;

        public RabbitMQHelper(IConnection connection)
        {
            _connection = connection;
            _channel = _connection.CreateChannelAsync().Result;
        }

        // Method to publish a message to a queue
        public async Task PublishMessage(string routingKey, string message)
        {
            await _channel.QueueDeclareAsync(queue: routingKey, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(message);
            var basicProperties = new BasicProperties
            {
                Persistent = true
            };
            await _channel.BasicPublishAsync(exchange: "", routingKey: routingKey, false, basicProperties: basicProperties, body: body);
            Console.WriteLine($" [x] Sent {message}");
        }

        // Method to consume messages from a queue
        public async Task ConsumeMessage(string queueName)
        {
           await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (sender, e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                BackgroundJob.Enqueue(() => ProcessMessage(queueName, message));
                await Task.CompletedTask;  // Return a completed task to satisfy the async requirement
            };

            await _channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
            Console.WriteLine($" [*] Waiting for messages in {queueName}.");
        }
        public void ProcessMessage(string queueName, string message)
        {
            Console.WriteLine($"Processing message from {queueName}: {message}");
           
        }
    }
}
