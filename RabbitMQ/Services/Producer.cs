using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Services.Common;
using RabbitMQ.Services.Dtos;

namespace RabbitMQ.Services
{
    public class Producer : IProducer
    {
        public Producer(ICacheAccessor cacheAccessor, IConfiguration configuration)
        {
            this.cacheAccessor = cacheAccessor;
            this.configuration = configuration;
        }
        private readonly IConfiguration configuration;

        private readonly ICacheAccessor cacheAccessor;

        public async Task<bool> PushMessageToQ(MQItem mQItem)
        {
            try
            {
                //TODO: move hostname to localhost
                var factory = new ConnectionFactory() { HostName = configuration.GetValue<string>("RabbitMQ:HostName") };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "counter",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                        await cacheAccessor.SetRequestToCache(mQItem);

                        channel.BasicPublish(exchange: "counter", routingKey: "counter", body: Helpers.ObjectToBytes(mQItem), basicProperties: null);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} | {ex.StackTrace}");
                return false;
            }
        }
    }
}