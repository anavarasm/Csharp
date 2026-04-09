using CQRS.core.Domain;
using Post.Common.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace Post.Cmd.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        private bool _active;
        private string _author;
        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();


        public bool Active
        {
            get => _active; set => _active = value;
        }

        public PostAggregate()
        {

        }

        public PostAggregate(Guid id, string author, string message)
        {
            //In constrctor we always raise the event that actually creates new aggreate instance
            //and in our case invoke the new post command 

            RaiseEvent(new PostCreatedEvent
            {
                Id = id,
                Author = author,
                Message = message,
                DatePosted = DateTime.Now
            });
        }

        public void Apply(PostCreatedEvent @event)
        {
            _id = @event.Id;
            _active = true;
            _author = @event.Author;    
        }

        public void EditMessge(string message)
        {
            if (!_active)
            {
                throw new InvalidOperationException("cannot edit inactive");
            }

            if(string.IsNullOrEmpty(message))
            {
                throw new InvalidOperationException($"the value of {nameof(message)} .provide valid mesage");
            }

            RaiseEvent(new MessageUpdatedEvent
            {
                Id = _id,
                Message = message,
            });
        }

        public void Apply(MessageUpdatedEvent @event)
        {
            _id = @event.Id;
        }


        public void LikePost()
        {
            if (!_active)
            {
                throw new InvalidOperationException("cannot like inactive post");
            }
            RaiseEvent(new PostLikeEvent
            {
                Id = _id
            });
        }

        public void Apply(PostLikeEvent @event)
        {
            _id = @event.Id;
        }

        public void AddComment(string comment,string username)
        {
            if (!_active) 
            {
                throw new InvalidOperationException("cannot add comment for invalid  post!");
            }

            if(string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($" the {nameof(comment)} should be valid valus dould not be empty");
            }

            RaiseEvent( new CommentAddedEvent{
                Id = _id,
                CommentId = Guid.NewGuid(),
                Comment = comment,
                Username = username,
                CommentDate = DateTime.Now, 
            });
        }

        public void Apply(CommentAddedEvent @event)
        {
            _id = @event.Id;
            
            _comments.Add(@event.CommentId,new Tuple<string,string>(@event.Comment,@event.Username));
        }

        public void EditCommand(Guid commentId, string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("post in inactive");  
            }
            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException("Comment should not be empty");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException($"you are not allowed to edit other user comments");
            }

            RaiseEvent(new CommentUpdatedEvent
            {
                    CommentId = commentId,
                    Id = _id,
                    Comment = comment,
                    Username=username,
                    EditDate = DateTime.Now,
            });

        }

        public void Apply(CommentUpdatedEvent @event)
        {
            _id = @event.Id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
        }

        public void RemoveComment(Guid commentId,string username)
        {
            if (!_active) 
            {
                throw new InvalidOperationException("Inactive comment cannot be removed");
            }
            //if (string.IsNullOrWhiteSpace(comment))
            //{
            //    throw new InvalidOperationException("Comment should not be empty");
            //}
            if (!_comments.ContainsKey(commentId))
            {
                throw new InvalidOperationException("comment not found");
            }
            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException($"you are not allowed to edit other user comments");
            }

            RaiseEvent(new CommentRemoveEvent
            {
                Id = _id,
                CommentId = commentId,
            });
        }

        public void Apply(CommentRemoveEvent @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.CommentId);
        }

        public void DeletePost(string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("inactive post");
            }
            if (_author == null)
            {
                throw new InvalidOperationException("Post not found");
            }
            if (!_author.Equals(username,StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("you did not post owner so cant delete");
            }


            RaiseEvent(new PostRemovedEvent
            {
                Id = _id
            });
        }

        public void Apply(PostRemovedEvent @event)
        {
            _id = @event.Id;
            _active = false;
        }
    }
}
