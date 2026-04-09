using CQRS.core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Post.Common.Events
{
    public class CommentRemoveEvent : BaseEvent
    {
        public CommentRemoveEvent() : base(nameof(CommentRemoveEvent))
        {
        }
        public Guid CommentId { get; set; }
    }
}
