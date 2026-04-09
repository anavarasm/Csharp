using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.core.Events
{
    public class EventModel
    {
        //all mongo db document have _id field known as OBject id PK
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime TimeStamp { get; set; }
        
        [BsonRepresentation(BsonType.String)]
        public Guid AggregateIdentitfier { get; set; }
        public string AggregateType { get; set; }
        public int Version { get; set; }
        public string EventType { get; set; }
        //thanks to polymorphism we can assign an instance of concrete event object to the base event
        //that will ensure we have all the information and the most important to replay the eventstore
        public BaseEvent EventData { get; set; }


    }
}
