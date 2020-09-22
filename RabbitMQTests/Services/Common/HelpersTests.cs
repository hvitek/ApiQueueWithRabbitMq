using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQ.Services.Common;
using RabbitMQ.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Services.Common.Tests
{
    [TestClass()]
    public class HelpersTests
    {
        [TestMethod()]
        public void BytesToObjectShoudBeRightType()
        {
            var item = new MQItem
            {
                Action = CrudAction.Add,
                Item = "",
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            var bytes = Encoding.UTF8.GetBytes(json);

            var toConvert = Helpers.BytesToObject<MQItem>(bytes);
            Assert.IsInstanceOfType(toConvert, typeof(MQItem));
        }

        [TestMethod()]
        public void ObjectToBytesShoudBeRightType()
        {
            var toConvert = Helpers.ObjectToBytes("sdgdfbdfbd");
            Assert.IsInstanceOfType(toConvert, typeof(byte[]));
        }
    }
}