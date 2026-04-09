using Castle.Core.Logging;
using CQRS.core.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Post.Query.Infrasturcture.Consumers
{
    public class ConsumerHostedService : IHostedService
    {
        private readonly ILogger<ConsumerHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ConsumerHostedService(ILogger<ConsumerHostedService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            _logger.LogInformation("Starting Consumer Hosted Service...");
            using (IServiceScope scope= _serviceProvider.CreateScope()) 
            {
                var eventConsumers = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
                // we can have multiple topics to consume from, so we can use a list of topics and consume from them in a loop
                var topics = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                Task.Run(() => eventConsumers.Consume(topics),cancellationToken);
                //foreach (var topic in topics)
                //{
                //    // we can run the consume method in a separate thread to avoid blocking the main thread
                //    Task.Run(() => eventConsumers.Consume(topic), cancellationToken);
                //}
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("stopped Consumer Hosted Service...");
            return Task.CompletedTask;
        }
    }
}
