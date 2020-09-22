using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RabbitMQ.Services.Dtos;

namespace RabbitMQ.Services
{
    public interface ICacheAccessor
    {
        Task DeleteRequestsFromCache(MQItem item);

        Task<Dictionary<DateTime, MQItem>> GetRequestsFromCache();

        Task<Dictionary<Guid, ResponseItem>> GetResponsesFromCache();

        Task<Dictionary<DateTime, MQItem>> SetRequestToCache(MQItem value);

        Task<Dictionary<Guid, ResponseItem>> SetResponseToCache(ResponseItem value);
    }
}