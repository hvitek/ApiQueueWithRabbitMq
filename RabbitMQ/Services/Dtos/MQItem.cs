using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Services.Dtos
{
    public class MQItem
    {
        public CrudAction Action { get; set; }

        public Object Item { get; set; }

        public Guid Request { get; set; } = Guid.NewGuid();

        public DateTime RequestCreationTime { get; set; } = DateTime.UtcNow;
    }
}