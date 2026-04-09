using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Post.Query.Infrasturcture.Handlers
{
    public class EventHandler : IEventHandler
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;

        public EventHandler(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }
        public async Task On(PostCreatedEvent @event)
        {
            //throw new NotImplementedException();
            var post = new PostEntity
            {
                PostId = @event.Id,
                Author = @event.Author,
                DatePosted = @event.DatePosted,
                Message = @event.Message
            };
            await _postRepository.CreateAsync(post);
        }

        public async Task On(MessageUpdatedEvent @event)
        {
            //throw new NotImplementedException();
            var post = await _postRepository.GetByIdAsync(@event.Id);
            if (post == null) return;

            post.Message = @event.Message;
            await _postRepository.UpdateAsync(post);
        }

        public async Task On(PostLikeEvent @event)
        {
            var post = await _postRepository.GetByIdAsync(@event.Id);
            if (post == null) return;

            post.Likes++;
            await _postRepository.UpdateAsync(post);

        }

        public async Task On(CommentAddedEvent @event)
        {
            //throw new NotImplementedException();
            var comment = new CommentEntity
            {
                PostId = @event.Id,
                CommentId = @event.CommentId,
                CommentDate = @event.CommentDate,
                Comment = @event.Comment,
                username = @event.Username,
                Edited = false
            };
            await _commentRepository.CreateAsync(comment);
        }

        public async Task On(CommentUpdatedEvent @event)
        {
            //throw new NotImplementedException();
            var comment = await _commentRepository.GetByIdAsync(@event.CommentId);
            if (comment == null) return;

            comment.Comment = @event.Comment;
            comment.Edited = true;
            comment.CommentDate = @event.EditDate;

            await _commentRepository.UpdateAsync(comment);
        }

        public async Task On(CommentRemoveEvent @event)
        {
            //    throw new NotImplementedException();
            await _commentRepository.DeleteAsync(@event.CommentId);
        }

        public async Task On(PostRemovedEvent @event)
        {
            //throw new NotImplementedException();
            await _postRepository.DeleteAsync(@event.Id);
        }
    }
}
