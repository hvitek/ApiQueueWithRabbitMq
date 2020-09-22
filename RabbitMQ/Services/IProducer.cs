using RabbitMQ.Services.Dtos;
using System.Threading.Tasks;

namespace RabbitMQ.Services
{
    public interface IProducer
    {
        Task<bool> PushMessageToQ(MQItem mQItem);
    }
}