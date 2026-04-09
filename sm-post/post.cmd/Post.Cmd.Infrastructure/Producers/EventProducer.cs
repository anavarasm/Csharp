using Confluent.Kafka;
using CQRS.core.Events;
using CQRS.core.Producers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Post.Cmd.Infrastructure.Producers
{
    public class EventProducer : IEventProducer
    {
        private readonly ProducerConfig _config;

        //inject COnfig we should use options pattern here but for simplicity we will just inject the config directly

        public EventProducer(IOptions<ProducerConfig> config)
        {
            _config = config.Value;
        }


        public async Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent
        {
            //throw new NotImplementedException();
            using var producer = new ProducerBuilder<string, string>(_config)
                .SetKeySerializer(Serializers.Utf8)
                .SetValueSerializer(Serializers.Utf8)
                .Build();

            var EventMessage = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonSerializer.Serialize(@event,@event.GetType())

            };

            var deliveryResult = await producer.ProduceAsync(topic, EventMessage);

            if (deliveryResult.Status == PersistenceStatus.NotPersisted)
            {
                throw new Exception($"could not produce {@event.GetType().Name} to topic {topic}  due to the following reson :{deliveryResult.Message} " );
            }
        }
    }
}
