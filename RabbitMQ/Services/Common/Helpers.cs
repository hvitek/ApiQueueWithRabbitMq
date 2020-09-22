using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Services.Common
{
    public static class Helpers
    {
        public static T BytesToObject<T>(byte[] bytes)
        {
            var json = Encoding.UTF8.GetString(bytes);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static byte[] ObjectToBytes(Object toConvert)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(toConvert);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}