using System;
using System.Text;
using System.Text.Json;
using AutoMapper;
using EventBus;
using EventBus.Common;
using EventBus.Events;
using MediatR;
using Ordering.Application.Commands;
using Ordering.Core.Repositories;
using RabbitMQ.Client.Events;

namespace Ordering.API.Consumers
{
    public class EventBusConsumer
    {
        private readonly IRabbitMQConnection _connection;
        private readonly IMediator _mediatr;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;

        public EventBusConsumer(IRabbitMQConnection connection, IMediator mediatr, IMapper mapper, IOrderRepository repository)
        {
            _connection = connection;
            _mediatr = mediatr;
            _mapper = mapper;
            _repository = repository;
        }

        public void Consume()
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(
                queue: EventBusConstants.BasketCheckoutQueue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += ReceivedEvent;

            channel.BasicConsume(
                queue: EventBusConstants.BasketCheckoutQueue,
                autoAck: true,
                consumer: consumer,
                consumerTag: null, // todo check
                noLocal: false,
                exclusive: false,
                arguments: null
            );
        }

        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == EvenBusConstants.BasketCheckoutQueue)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var basketCheckoutEvent = JsonSerializer.Deserialize<BasketCheckoutEvent>(message);

                var command = _mapper.Map<CheckoutOrderCommand>(basketCheckoutEvent);
                await _mediatr.Send(command);
            }
        }

        public void Disconnect()
        {
            _connection.Dispose();
        }
    }
}
