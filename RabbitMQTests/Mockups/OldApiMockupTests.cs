using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQ.Mockups;
using RabbitMQ.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Mockups.Tests
{
    [TestClass()]
    public class OldApiMockupTests
    {
        [TestMethod()]
        public void AddShouldBeTrue()
        {
            PrepareDb();
            Assert.IsTrue(OldApiMockup.CustomerMockupDb.Count == 1);
        }

        [TestMethod()]
        public void DeleteShouldBeTrue()
        {
            PrepareDb();
            OldApiMockup.Delete(555L);
            Assert.IsTrue(OldApiMockup.CustomerMockupDb.Count == 0);
        }

        [TestMethod()]
        public void GetShouldBeTrue()
        {
            PrepareDb();
            Assert.IsTrue(OldApiMockup.Get(555) != null);
        }

        [TestMethod()]
        public void UpdateShouldBeTrue()
        {
            PrepareDb();
            OldApiMockup.Update(new Customer { Id = 555, Name = "Arya Stark" });
            var record = OldApiMockup.Get(555);
            Assert.IsTrue(record.Name == "Arya Stark");
        }

        private void PrepareDb()
        {
            OldApiMockup.CustomerMockupDb.Clear();
            OldApiMockup.CustomerMockupDb.Add(new Customer
            {
                Name = "John Snow",
                Id = 555
            });
        }
    }
}