using CQRS.core.Domain;
using CQRS.core.Handlers;
using CQRS.core.Infrastructure;
using Post.Cmd.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Post.Cmd.Infrastructure.Handlers
{
    public class EventSourcingHandler : IEventSourcingHandler<PostAggregate>
    {
        private readonly IEventStore _eventStore;

        public EventSourcingHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }
        public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
        {
            //throw new NotImplementedException();

            var aggregate = new PostAggregate();
            var events = await _eventStore.GetEventsAsync(aggregateId);

            if (events == null|| !events.Any() ){
                return aggregate;
            }
            aggregate.ReplayEvents(events);
            aggregate.Version = events.Max(x => int.Parse(x.version));

            return aggregate;
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            //throw new NotImplementedException();
            //getting uncommitted changes from the aggregate and saving to the event store
            await _eventStore.SaveEventAsync(aggregate.Id, aggregate.GetUncommitedChanges(), aggregate.Version);
            aggregate.MarkChangesAsCommited();
        }
    }
}
