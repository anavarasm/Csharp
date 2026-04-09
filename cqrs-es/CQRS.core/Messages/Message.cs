using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.core.Messages
{
    public abstract class Message
    {
        public Guid Id { get; set; }
    }
}
