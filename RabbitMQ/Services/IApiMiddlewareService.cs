using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbitMQ.Services.Dtos;

namespace RabbitMQ.Services
{
    public interface IApiMiddlewareService
    {
        Task ExecuteRequests(int batchSize = 5);

        Task<Dictionary<DateTime, MQItem>> GetRequests();

        Task<Dictionary<Guid, ResponseItem>> GetRespones();
    }
}