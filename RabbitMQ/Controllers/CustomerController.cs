using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Services;
using RabbitMQ.Services.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RabbitMQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        public CustomerController(IProducer producer, IApiMiddlewareService apiService)
        {
            this.producer = producer;
            this.apiService = apiService;
        }

        private readonly IApiMiddlewareService apiService;

        private readonly IProducer producer;

        [HttpPost("create")]
        public async Task Create([FromBody]CustomerCreateModel customer)
        {
            var mqItem = new Services.Dtos.MQItem
            {
                Action = CrudAction.Add,
                Item = new Customer { Name = customer.Name }
            };

            //not necessarily wait for answer
            _ = Task.Run(() => producer.PushMessageToQ(mqItem));
        }

        [HttpDelete("delete/{id}")]
        public async Task Delete(long id)
        {
            var mqItem = new Services.Dtos.MQItem
            {
                Action = CrudAction.Delete,
                Item = new Customer { Id = id }
            };

            //not necessarily wait for answer
            _ = Task.Run(() => producer.PushMessageToQ(mqItem));
        }

        [HttpGet("getcustomerbyid/{id}")]
        public async Task<Customer> GetCustomerById(long id)
        {
            var mqItem = new Services.Dtos.MQItem
            {
                Action = CrudAction.Get,
                Item = new Customer { Id = id }
            };

            _ = Task.Run(() => producer.PushMessageToQ(mqItem));
            return await WaitForResponse(mqItem, id);
        }

        [HttpPost("update")]
        public async Task Update([FromBody]Customer customer)
        {
            var mqItem = new Services.Dtos.MQItem
            {
                Action = CrudAction.Update,
                Item = customer
            };

            //not necessarily wait for answer
            _ = Task.Run(() => producer.PushMessageToQ(mqItem));
        }

        private async Task<Customer> WaitForResponse(MQItem mQItem, long id)
        {
            var step = 0;
            bool responseExist = false;
            while (!responseExist)
            {
                if (step > 500)
                    throw new Exception("User not exists");

                System.Threading.Thread.Sleep(500);
                var responses = (await apiService.GetRespones());
                var response = responses.Where(x => x.Value.RequestGuid == mQItem.Request && x.Value.Customer.Id == id).FirstOrDefault();

                if (response.Value != null)
                {
                    return response.Value.Customer;
                }
            }

            throw new Exception("User not exists");
        }
    }
}