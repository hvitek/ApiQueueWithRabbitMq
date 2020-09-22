using RabbitMQ.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.Mockups
{
    public static class OldApiMockup
    {
        public static List<Customer> CustomerMockupDb { get; set; } = new List<Customer>();

        public static Customer Add(Customer customer)
        {
            Wait();
            var newId = 1L;
            if (CustomerMockupDb.Any())
            {
                newId = CustomerMockupDb.Max(x => x.Id) + 1;
            }
            var newCustomer = new Customer { Id = newId, Name = customer.Name };
            CustomerMockupDb.Add(newCustomer);
            return newCustomer;
        }

        public static Customer Delete(long id)
        {
            var toDelete = CustomerMockupDb.Where(x => x.Id == id).FirstOrDefault();
            if (toDelete != null)
            {
                Wait();
                CustomerMockupDb.Remove(toDelete);
            }

            return toDelete;
        }

        public static Customer Get(long id)
        {
            Wait();
            return CustomerMockupDb.Where(x => x.Id == id).FirstOrDefault();
        }

        public static Customer Update(Customer customer)
        {
            var original = CustomerMockupDb.Where(x => x.Id == customer.Id).FirstOrDefault();

            original.Name = customer.Name;
            Wait();
            return original;
        }

        private static void Wait()
        {
            var wait = new Random().Next(0, 10);
            System.Threading.Thread.Sleep(wait * 1000);
        }
    }
}