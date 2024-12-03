using RabbitMQ.Client;

namespace UserService.Helper
{
    public interface IRabbitMqSubscriber
    {
        void Subscribe();
    }
}