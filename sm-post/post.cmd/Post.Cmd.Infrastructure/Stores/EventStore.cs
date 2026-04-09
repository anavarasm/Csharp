using CQRS.core.Domain;
using CQRS.core.Events;
using CQRS.core.Exceptions;
using CQRS.core.Infrastructure;
using CQRS.core.Producers;
using Post.Cmd.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Post.Cmd.Infrastructure.Stores
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IEventProducer _eventProducer;

        public EventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducer)
        {
            _eventStoreRepository = eventStoreRepository;
            _eventProducer = eventProducer;
        }

        public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
        {
            //throw new NotImplementedException();
            var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);
            if(eventStream == null || !eventStream.Any())
            {
                throw new AggregateNotFoundException($"Post not found {aggregateId}");
            }

            return eventStream.OrderBy(x=>x.Version).Select(x=>x.EventData).ToList();
        }

        public async Task SaveEventAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            //throw new NotImplementedException();
            var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);
            if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion) {
                throw new ConcurrencyException();
            }

            var version = expectedVersion;
            foreach (var @event in events)
            {
                version++;
                @event.version = version.ToString();
                var eventType = @event.GetType().Name;
                var eventModel = new EventModel
                {
                    TimeStamp = DateTime.UtcNow,
                    AggregateIdentitfier = aggregateId,
                    AggregateType = typeof(PostAggregate).Name,
                    Version = version,
                    EventType = eventType,
                    EventData = @event
                };
                await _eventStoreRepository.saveAsync(eventModel);

                var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC") ?? "SocialMediaPostEvents";
                await _eventProducer.ProduceAsync(topic, @event);
                //what if loop inside if it failed to prodce successful event to kafka? this canse we have already succcessfully persisted the event to the eventstore
                //will suggest a transaction over the code where you save the mongodb as well as the produce asyn method/
                //only if both the persisting to the eventstore and producing to kafka succeeds then you can commit the transaction.
                //mondodb supports transaction since we have single instance here
                //for use mongodb transactions you need mongodb to run as part of replicaset so try the transaction here.

            }
        }
    }
}
