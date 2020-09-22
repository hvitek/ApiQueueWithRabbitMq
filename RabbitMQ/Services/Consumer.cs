using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Services.Common;
using RabbitMQ.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.Services
{
    public class Consumer : IConsumer
    {
        public Consumer(ICacheAccessor cacheAccessor, IConfiguration configuration)
        {
            this.cacheAccessor = cacheAccessor;
            this.configuration = configuration;
        }
        private readonly IConfiguration configuration;

        private readonly ICacheAccessor cacheAccessor;

        private IConnection _connection { get; set; }

        private ConnectionFactory _factory { get; set; }

        private IModel _channel { get; set; }


        public void ReceiveMessageFromQ()
        {
            try
            {
                //TODO: move hostname to localhost
                _factory = new ConnectionFactory() { HostName = configuration.GetValue<string>("RabbitMQ:HostName") };
                _connection = _factory.CreateConnection();
                _channel = _connection.CreateModel();

                {
                    _channel.QueueDeclare(queue: "counter",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    _channel.BasicQos(prefetchSize: 0, prefetchCount: 3, global: false);

                    var consumer = new EventingBasicConsumer(_channel);
                    consumer.Received += async (model, ea) =>
                    {
                        var item = Helpers.BytesToObject<MQItem>(ea.Body);
                        await cacheAccessor.DeleteRequestsFromCache(item);

                        _channel.BasicAck(ea.DeliveryTag, false);
                    };

                    _channel.BasicConsume(queue: "counter", autoAck: false, consumer: consumer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} | {ex.StackTrace}");
            }
        }
    }
}