using CQRS.core.Domain;
using CQRS.core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post.Cmd.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Post.Cmd.Infrastructure.Repositories
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IMongoCollection<EventModel> _eventStoreCollection;

        public EventStoreRepository(IOptions<MongoDbConfig> config)
        {
           //IOptions  allows us to inject the config via DI

            var mongoClient = new MongoClient(config.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(config.Value.DataBase);

            _eventStoreCollection = mongoDataBase.GetCollection<EventModel>(config.Value.Collection);

        }
        public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
        {
            //throw new NotImplementedException();
            //configure await is false for used to avoid deadlocks in certain synchronization contexts, such as UI applications. It allows the continuation of the async operation to run on a different thread, rather than trying to resume on the original context, which can lead to deadlocks if that context is blocked.
            return await _eventStoreCollection.Find(x => x.AggregateIdentitfier == aggregateId).ToListAsync().ConfigureAwait(false);

        }

        public async Task saveAsync(EventModel @event)
        {
            //throw new NotImplementedException();
            await _eventStoreCollection.InsertOneAsync(@event).ConfigureAwait(false);

        }
    }
}
