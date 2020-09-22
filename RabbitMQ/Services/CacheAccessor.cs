using Microsoft.Extensions.Caching.Memory;
using RabbitMQ.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.Services
{
    public class CacheAccessor : ICacheAccessor
    {
        public CacheAccessor(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        private readonly IMemoryCache memoryCache;

        public async Task DeleteRequestsFromCache(MQItem item)
        {
            await DeleteRequestFromCache(item);
        }

        public async Task<Dictionary<DateTime, MQItem>> GetRequestsFromCache()
        {
            Dictionary<DateTime, MQItem> mQItemsDictionary = null;
            memoryCache.TryGetValue<Dictionary<DateTime, MQItem>>(CachingKeys.Request, out mQItemsDictionary);
            if (mQItemsDictionary == null)
            {
                mQItemsDictionary = new Dictionary<DateTime, MQItem>();
            }

            return mQItemsDictionary;
        }

        public async Task<Dictionary<Guid, ResponseItem>> GetResponsesFromCache()
        {
            Dictionary<Guid, ResponseItem> responseDictionary = null;
            memoryCache.TryGetValue<Dictionary<Guid, ResponseItem>>(CachingKeys.Response, out responseDictionary);
            if (responseDictionary == null)
            {
                responseDictionary = new Dictionary<Guid, ResponseItem>();
            }

            return responseDictionary;
        }

        public async Task<Dictionary<DateTime, MQItem>> SetRequestToCache(MQItem value)
        {
            var cache = await GetRequestsFromCache();
            cache.Add(value.RequestCreationTime, value);

            memoryCache.Set<Dictionary<DateTime, MQItem>>(CachingKeys.Request, cache);
            return cache;
        }

        public async Task<Dictionary<Guid, ResponseItem>> SetResponseToCache(ResponseItem value)
        {
            var cache = await GetResponsesFromCache();
            cache.Add(value.RequestGuid, value);
            memoryCache.Set<Dictionary<Guid, ResponseItem>>(CachingKeys.Response, cache);
            return cache;
        }

        private async Task<Dictionary<DateTime, MQItem>> DeleteRequestFromCache(MQItem value)
        {
            var cache = await GetRequestsFromCache();
            cache.Remove(value.RequestCreationTime);

            memoryCache.Set<Dictionary<DateTime, MQItem>>(CachingKeys.Request, cache);
            return cache;
        }
    }
}