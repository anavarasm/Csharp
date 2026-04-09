using CQRS.core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Query.Api.DTOs;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PostLookUpController : ControllerBase
    {
        private readonly ILogger<PostLookUpController> _logger;
        private readonly IQueryDispatcher<PostEntity> _queryDispatcher;

        public PostLookUpController(ILogger<PostLookUpController> logger, IQueryDispatcher<PostEntity> queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPostsAsync()
        {
            try
            {

                var posts = await _queryDispatcher.SendAsync(new FindAllPostQueries());
                if (posts == null || !posts.Any()) { return NoContent(); }
                var count = posts.Count();
                return Ok(new PostLookupResponse
                {
                    Posts = posts,
                    Message = $"Successfully returned {count} post {(count > 1 ? "s" : "")} post(s)"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing {0} request", nameof(GetAllPostsAsync));
                return StatusCode(StatusCodes.Status500InternalServerError, new PostLookupResponse
                {
                    Message = $"Error while processing {nameof(GetAllPostsAsync)} request"
                });
            }
        }

        [HttpGet("byId/{postId}")]
        public async Task<ActionResult> GetPostByIdAsync(Guid postId)
        {
            try
            {
                var post = await _queryDispatcher.SendAsync(new FindPostByIdQuery { Id = postId });
                if (post == null || !post.Any()) { return NoContent(); }
                return Ok(new PostLookupResponse
                {
                    Posts = post,
                    Message = $"Successfully returned post with id {postId}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing {0} request", nameof(GetPostByIdAsync));
                return StatusCode(StatusCodes.Status500InternalServerError, new PostLookupResponse
                {
                    Message = $"Error while processing {nameof(GetPostByIdAsync)} request"
                });
            }
        }

        [HttpGet("byAuthor/{author}")]
        public async Task<ActionResult> GetPostByAuthorAsync(string author)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostByAuthorQuery { Author = author });
                if (posts == null || !posts.Any()) { return NoContent(); }
                var count = posts.Count();
                return Ok(new PostLookupResponse
                {
                    Posts = posts,
                    Message = $"Successfully returned {count} post(s) by author {author}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing {0} request", nameof(GetPostByAuthorAsync));
                return StatusCode(StatusCodes.Status500InternalServerError, new PostLookupResponse
                {
                    Message = $"Error while processing {nameof(GetPostByAuthorAsync)} request"
                });
            }
        }

        [HttpGet("withComments")]
        public async Task<ActionResult> GetPostsWithCommentsAsync()
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostWithCommentsQuery());
                if (posts == null || !posts.Any()) { return NoContent(); }
                var count = posts.Count();
                return Ok(new PostLookupResponse
                {
                    Posts = posts,
                    Message = $"Successfully returned {count} post(s) with comments"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing {0} request", nameof(GetPostsWithCommentsAsync));
                return StatusCode(StatusCodes.Status500InternalServerError, new PostLookupResponse
                {
                    Message = $"Error while processing {nameof(GetPostsWithCommentsAsync)} request"
                });
            }
        }

        [HttpGet("withLikes/{likes}")]
        public async Task<ActionResult> GetPostsWithLikesAsync(int likes)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostWithLikesQuery { NumberOfLikes = likes });
                if (posts == null || !posts.Any()) { return NoContent(); }
                var count = posts.Count();
                return Ok(new PostLookupResponse
                {
                    Posts = posts,
                    Message = $"Successfully returned {count} post(s) with at least {likes} like(s)"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing {0} request", nameof(GetPostsWithLikesAsync));
                return StatusCode(StatusCodes.Status500InternalServerError, new PostLookupResponse
                {
                    Message = $"Error while processing {nameof(GetPostsWithLikesAsync)} request"
                });
            }
        }
    }
}
