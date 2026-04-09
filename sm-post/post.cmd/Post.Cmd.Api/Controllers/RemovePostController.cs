using CQRS.core.Exceptions;
using CQRS.core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTOs;
using Post.Common.Events;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RemovePostController : ControllerBase
    {
        private readonly ILogger<RemovePostController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public RemovePostController(ILogger<RemovePostController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemovePostAsync(Guid id,DeletePostCommand command)
        {
            command.Id = id;
            try
            {
                await _commandDispatcher.SendAsync(command);

                return Ok(new BaseResponse
                {
                    Message = "Post removed Sccessfully"
                });
            }
            catch (AggregateNotFoundException exc)
            {
                _logger.Log(LogLevel.Warning, exc, "Aggregate not found");
                return BadRequest(new BaseResponse
                {
                    Message = exc.Message
                });

            }
            catch (InvalidOperationException ex)
            {

                _logger.Log(LogLevel.Warning, ex, "client made bad request");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                const string safeErrorMessage = "An error occurred while processing your request.";
                _logger.LogError(ex, safeErrorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = safeErrorMessage
                });
            }

        }

    }
}
