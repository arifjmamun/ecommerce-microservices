using System;
using System.Text;
using System.Text.Json;
using EventBus.Events;

namespace EventBus.Producers
{
    public class EventBusProducer
    {
        private readonly IRabbitMQConnection _connection;

        public EventBusProducer(IRabbitMQConnection connection)
        {
            _connection = connection;
        }

        public void PublishBasketCheckout(string queueName, BasketCheckoutEvent publishModel)
        {
            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var message = JsonSerializer.Serialize(publishModel);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.DeliveryMode = 2;

            channel.ConfirmSelect();
            channel.BasicPublish(exchange: "", routingKey: queueName, mandatory: true, basicProperties: properties, body: body);
            channel.WaitForConfirmsOrDie();

            channel.BasicAcks += (sender, eventArgs) =>
            {
                Console.WriteLine("Sent RabbitMQ");
            };
            channel.ConfirmSelect();
        }
    }
}
