using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.core.Consumers
{
    public interface IEventConsumer
    {
        void Consume(string message);
    }
}
