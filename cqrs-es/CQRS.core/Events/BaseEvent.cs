using CQRS.core.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.core.Events
{
    public abstract class BaseEvent : Message
    {
        protected BaseEvent(string type)
        { 
            this.Type = type;
        }
        public string version { get; set; }
        public string Type { get; set; }

    }
}
