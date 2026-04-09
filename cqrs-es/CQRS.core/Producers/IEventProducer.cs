using CQRS.core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.core.Producers
{
    public interface IEventProducer
    {
        Task ProduceAsync<T>(string topic,T @event) where T : BaseEvent;
        //so All our concrete events will inherit from base event and we can use this method to produce any event that inherits from base event
    }
}
