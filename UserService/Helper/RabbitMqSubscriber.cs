using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace UserService.Helper
{
    public class RabbitMqSubscriber:IRabbitMqBusHelper
    {
        //private readonly string _queueName = "BobSQueue";
        //private readonly string _exchangeName = "bob-fanout-exchange";

        //public void Subscribe()
        //{
        //    var coonectionFactor = SetUpConnection();
        //    using (var connection = coonectionFactor.CreateConnectionAsync().Result)
        //    {
        //        using (var channel = connection.CreateChannelAsync().Result)
        //        {
        //            // Bind queue 1
        //            channel.QueueBindAsync(
        //                _queueName + 1, // queue name
        //                _exchangeName, // exchange name
        //                String.Empty, // routing key
        //                null // arguments
        //                );

        //            // Bind queue 2
        //            channel.QueueBindAsync(
        //                _queueName + 2, // queue name
        //                _exchangeName, // exchange name
        //                String.Empty, // routing key
        //                null // arguments
        //                );

        //            // Create consumer
        //            var consumer = new AsyncEventingBasicConsumer(channel);

        //            // Receive Message
        //            consumer.ReceivedAsync += (sender, e) =>
        //            {
        //                var message = Encoding.UTF8.GetString(e.Body.ToArray());
        //                Console.WriteLine(message);
        //            };

        //            // Subscribe to the queue
        //            var result1 = channel.BasicConsumeAsync(_queueName + 1, true, consumer);
        //            var result2 = channel.BasicConsumeAsync(_queueName + 2, true, consumer);

        //            Console.WriteLine(result1);
        //            Console.WriteLine(result2);
        //        }
        //    }
        //}

        //private ConnectionFactory SetUpConnection()
        //{
        //    return new ConnectionFactory()
        //    {
        //        Uri = new Uri("amqps://kvpmuftr:ibuumh3S1nsMCBp2UC8oBV1kvpUxNAlf@lionfish.rmq.cloudamqp.com/kvpmuftr"),
        //        ConsumerDispatchConcurrency = 1
        //    };
        //}

      
    }
}
