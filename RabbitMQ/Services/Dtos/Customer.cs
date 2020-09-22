using System;

namespace RabbitMQ.Services
{
    public class Customer
    {
        public DateTime CreationDateTime { get; set; } = DateTime.UtcNow;

        public long Id { get; set; }

        public string Name { get; set; }
    }
}