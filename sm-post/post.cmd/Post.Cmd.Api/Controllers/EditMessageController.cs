using CQRS.core.Exceptions;
using CQRS.core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.DTOs;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EditMessageController : ControllerBase
    {
        private readonly ILogger<EditMessageController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public EditMessageController(ILogger<EditMessageController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditMesssageAsync(Guid id,EditMessageCommand command)
        {
            command.Id = id;
            try
            {
                await _commandDispatcher.SendAsync(command);
                return Ok(new BaseResponse
                {
                    Message = "Edit message request  updated successfully"

                });
            }
            catch(AggregateNotFoundException exc)
            {
                _logger.Log(LogLevel.Warning,exc,"Aggregate not found");
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
