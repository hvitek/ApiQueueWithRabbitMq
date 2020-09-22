using System;

namespace RabbitMQ.Services
{
    public class ResponseItem
    {
        public Customer Customer { get; set; }

        public Guid RequestGuid { get; set; }
    }
}