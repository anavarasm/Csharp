using CQRS.core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.DTOs;

namespace Post.Cmd.Api.Controllers
{
   
    
    [ApiController]
    [Route("Api/v1/[controller]")]
    public class NewPostController : ControllerBase
    {
        private readonly ILogger<NewPostController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public NewPostController(ILogger<NewPostController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        // Controller methods for creating a new post

        [HttpPost]
        public async Task<ActionResult> NewPostAsync(NewPostCommand command)
        {
            var id = Guid.NewGuid();
            try
            {
                command.Id = id;

                await _commandDispatcher.SendAsync(command);
                return StatusCode(StatusCodes.Status201Created,new NewPostResponse
                {
                    Message = "New post created successfully",
                    Id = id
                });
            }
            catch (InvalidOperationException ex)
            {

                _logger.Log(LogLevel.Warning,ex,"client made bad request");
                return BadRequest(new NewPostResponse
                {
                    Message = ex.Message
                });
            }
            catch(Exception ex)
            {
                const string safeErrorMessage = "An error occurred while processing your request.";
                _logger.LogError(ex, safeErrorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse
                {
                    Id = id,
                    Message = safeErrorMessage
                });
            }

        }

    }
}
