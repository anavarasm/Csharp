using CQRS.core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.core.Domain
{
    public interface IEventStoreRepository
    {
        //No Update or delete in Eventstore its only for add/list
        Task saveAsync(EventModel @event);

        Task<List<EventModel>> FindByAggregateId(Guid aggregateId);

    }
}
