using CQRS.core.Handlers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Api.Commands
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IEventSourcingHandler<PostAggregate> _eventSourcingHandler;

        public CommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }

        public async Task HandleAsync(NewPostCommand command)
        {
            //throw new NotImplementedException();
            var aggregate = new PostAggregate(command.Id, command.Author, command.Message);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditMessageCommand command)
        {
            //throw new NotImplementedException();
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.EditMessge(command.Message);
            await _eventSourcingHandler.SaveAsync(aggregate);   
        }

        public async Task HandleAsync(LikePostCommand command)
        {
            //throw new NotImplementedException();
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.LikePost();
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(AddCommentCommand command)
        {
            //throw new NotImplementedException();
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.AddComment(command.Comment, command.Username);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditCommentCommand command)
        {
            //throw new NotImplementedException();
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.EditCommand(command.CommentId, command.Comment, command.Username);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(RemoveCommentCommand command)
        {
            //throw new NotImplementedException();
            var aggregate =  _eventSourcingHandler.GetByIdAsync(command.Id).Result;
            aggregate.RemoveComment(command.CommentId, command.Username);
            await _eventSourcingHandler.SaveAsync(aggregate);   
        }

        public async Task HandleAsync(DeletePostCommand command)
        {
            //throw new NotImplementedException();
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.DeletePost(command.Username);
            await _eventSourcingHandler.SaveAsync(aggregate);

        }
    }
}
