using Post.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Post.Query.Infrasturcture.Handlers
{
    public interface IEventHandler
    {
        Task On(PostCreatedEvent @event);
        Task On(MessageUpdatedEvent @event);
        Task On(PostLikeEvent @event); 
        Task On(CommentAddedEvent @event); 
        Task On(CommentUpdatedEvent @event); 
        Task On(CommentRemoveEvent @event); 
        Task On(PostRemovedEvent @event); 
    }
}
