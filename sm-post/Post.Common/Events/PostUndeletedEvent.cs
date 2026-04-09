using System;

namespace Post.Common.Events
{
    public class PostUndeletedEvent
    {
        public Guid Id { get; set; }
        public DateTime DateRestored { get; set; }
    }
}