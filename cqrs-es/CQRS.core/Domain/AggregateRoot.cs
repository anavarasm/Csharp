using CQRS.core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.core.Domain
{
    public abstract class AggregateRoot
    {
        protected Guid _id;

        private readonly List<BaseEvent> _changes = new();

        public Guid Id { get { return _id; } }

        public int Version { get; set; } = -1;

        public IEnumerable<BaseEvent> GetUncommitedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommited() {
            _changes.Clear();
        }

        private void ApplyChanges( BaseEvent @event,bool isNew) 
        {
            // reflection used here
            var method = this.GetType().GetMethod("Apply",new Type[] { @event.GetType()});

            if (method == null)
            {
                throw new ArgumentNullException(nameof(method),$"The APply method was not found {@event.GetType().Name}!");
            }

            method.Invoke(this, new object[] { @event });
            // We dont want to add uncommitted changes if they come from event store
            if (isNew) 
            {
                _changes.Add(@event );
            }

        }
        //so  that only concrete methods aggregate types can invoke it
        protected void RaiseEvent(BaseEvent @event) { 
            ApplyChanges( @event, true );
        }

        public void ReplayEvents(IEnumerable<BaseEvent> events)
        { 
            foreach(var @event in events)
            {
                //so wont added as uncommited
                
                ApplyChanges(@event,false);
            }
        }                                                            
    }
}
