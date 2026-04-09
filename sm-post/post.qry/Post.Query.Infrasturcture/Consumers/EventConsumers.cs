using Confluent.Kafka;
using CQRS.core.Consumers;
using CQRS.core.Events;
using Microsoft.Extensions.Options;
using Post.Query.Infrastructure.Converters;
using Post.Query.Infrasturcture.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Post.Query.Infrasturcture.Consumers
{
    public class EventConsumers : IEventConsumer
    {

        private readonly ConsumerConfig _config;
        private readonly IEventHandler _eventHandler;


        public EventConsumers(IOptions<ConsumerConfig> config,IEventHandler eventHandler)
        {
            _config = config.Value;
            _eventHandler = eventHandler;
        }
        public void Consume(string topic)
        {
            //throw new NotImplementedException();
            using var consumer = new ConsumerBuilder<string, string>(_config)
                .SetKeyDeserializer(Deserializers.Utf8)
                .SetValueDeserializer(Deserializers.Utf8)
                .Build();

            consumer.Subscribe(topic);
            while (true)
            {
                var consumeResult = consumer.Consume();
                if (consumeResult.Message == null) return;

                var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
                //baseevent is abstract class and we have to deserialize the json string to the correct event type, so we need to use the custom converter to achieve this
                var @event = JsonSerializer.Deserialize<BaseEvent> (consumeResult.Message.Value, options);
                var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { @event.GetType() });

                if(handlerMethod == null)
                {
                    throw new ArgumentNullException($"No handler found for event type {@event.GetType().Name}");
                }
                handlerMethod.Invoke(_eventHandler, new object[] { @event });
                // commit the offset after processing the event
                // This is commit which will say the event is consumed and handled
                // and will set the commit offset 
                // invoke will increase the offset by 1 and will move to the next event in the topic
                consumer.Commit(consumeResult);
            }
        }
    }
}
