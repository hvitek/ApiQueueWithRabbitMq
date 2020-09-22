using Microsoft.Extensions.Caching.Memory;
using RabbitMQ.Mockups;
using RabbitMQ.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.Services
{
    public class ApiMiddlewareService : IApiMiddlewareService
    {
        public ApiMiddlewareService(ICacheAccessor cacheAccessor)
        {
            this.cacheAccessor = cacheAccessor;
        }

        private readonly ICacheAccessor cacheAccessor;

        public async Task ExecuteRequests(int batchSize = 5)
        {
            if (batchSize > 5)
            {
                batchSize = 5;
            }

            var requests = (await GetRequests()).OrderByDescending(x => x.Key).Take(5);

            foreach (var item in requests)
            {
                try
                {
                    //call mockup
                    var response = await ExecuteRequest(item.Value);

                    //delete request from cache
                    var requestToDelete = requests.Where(x => x.Value.Request == item.Value.Request).FirstOrDefault();
                    await cacheAccessor.DeleteRequestsFromCache(requestToDelete.Value);

                    //save response
                    await cacheAccessor.SetResponseToCache(
                        new ResponseItem
                        {
                            Customer = response,
                            RequestGuid = item.Value.Request
                        });
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }

        public async Task<Dictionary<DateTime, MQItem>> GetRequests()
        {
            return await cacheAccessor.GetRequestsFromCache();
        }

        public async Task<Dictionary<Guid, ResponseItem>> GetRespones()
        {
            return await cacheAccessor.GetResponsesFromCache();
        }

        private async Task<Customer> ExecuteRequest(MQItem item)
        {
            var customer = (Customer)item.Item;
            //mockup call
            switch (item.Action)
            {
                case CrudAction.Update:
                    return OldApiMockup.Update(customer);
                    break;

                case CrudAction.Add:
                    return OldApiMockup.Add(customer);
                    break;

                case CrudAction.Delete:
                    return OldApiMockup.Delete(customer.Id);
                    break;

                case CrudAction.Get:
                    return OldApiMockup.Get(customer.Id);

                default:
                    break;
            }

            return (Customer)item.Item;
        }
    }
}