using CQRS.core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.core.Infrastructure
{
    public interface IEventStore
    {
        Task SaveEventAsync(Guid aggregateId,IEnumerable<BaseEvent> events, int expectedVersion);
        Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId);
    }
}
