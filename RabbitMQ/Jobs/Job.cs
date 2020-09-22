using Microsoft.Extensions.Logging;
using Quartz;
using RabbitMQ.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ
{
    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        public Job(ILogger<Job> logger, IApiMiddlewareService api)
        {
            this.logger = logger;
            this.api = api;
        }

        private readonly IApiMiddlewareService api;

        private readonly ILogger<Job> logger;

        public async Task Execute(IJobExecutionContext context)
        {
            await api.ExecuteRequests(5);
            logger.LogInformation("Request from queue was executed!");
        }
    }
}