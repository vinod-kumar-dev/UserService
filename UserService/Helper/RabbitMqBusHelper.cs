using Azure.Core;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace UserService.Helper
{
    public class RabbitMqBusHelper : IRabbitMqBusHelper
    {
        private readonly string _queueName = "BobSQueue";
        private readonly string _exchangeName = "bob-fanout-exchange";

        // channel publish
        public async Task Publish<T>(T command)
        {
            var coonectionFactor = SetUpConnection();
            // Create Connection
            using (var connection = coonectionFactor.CreateConnectionAsync().Result)
            {
                // Create Channel
                using (var channel = connection.CreateChannelAsync().Result)
                {
                    // Create a fanout exchange
                   await channel.QueueDeclareAsync(
                        _exchangeName, //name of exchange
                       false,
                        false, // durable
                        false,
                        null// durable
                        );

                    // channel QueueDeclare = create 1st queue
                  await  channel.QueueDeclareAsync(_queueName + 1,
                                            false, //durable: messages sent using this method persist only in the memory and not survive a server restart.
                                            false, //exclusive
                                            false, // auto-delete
                                            null); // arguments



                    // channel QueueDeclare = create 2nd queue
                   await channel.QueueDeclareAsync(_queueName + 2,
                                            false, //durable: messages sent using this method persist only in the memory and not survive a server restart.
                                            false, //exclusive
                                            false, // auto-delete
                                            null); // arguments

                    // Create the message
                    var message = CreateBody(command);
                    var basicProperties = new BasicProperties
                    {
                        Persistent = true // Example: Make the message persistent
                    };
                    // publish the message
                    await channel.BasicPublishAsync
                        (
                        _exchangeName, //exchangeName
                        string.Empty, //routingKey > not needed for fanout exchange
                        false, //mandatory
                        basicProperties,
                        message //body
                        );
                }
            }
        }
        private ConnectionFactory SetUpConnection()
        {
          return new ConnectionFactory()
            {
                Uri = new Uri("amqps://kvpmuftr:ibuumh3S1nsMCBp2UC8oBV1kvpUxNAlf@lionfish.rmq.cloudamqp.com/kvpmuftr"),
                ConsumerDispatchConcurrency = 1
            };
        }
           

        // Encode message body
        private byte[] CreateBody<T>(T command)
        {
            var message = JsonConvert.SerializeObject(command);

            return Encoding.UTF8.GetBytes(message);
        }
    }
}
