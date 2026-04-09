using CQRS.core.Events;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;

namespace Post.Common.Events
{
    public class PostLikeEvent : BaseEvent
    {
        public PostLikeEvent() : base(nameof(PostLikeEvent))
        {

        }

    }
}
