using CQRS.core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.core.Handlers
{
    public interface IEventSourcingHandler<T>
    {
        Task SaveAsync(AggregateRoot aggregate);

        Task<T> GetByIdAsync(Guid aggregateId);
    }
}
