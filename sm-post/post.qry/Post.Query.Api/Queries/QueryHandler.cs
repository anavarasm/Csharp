using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Api.Queries
{
    public class QueryHandler : IQueryHandler
    {
        private readonly IPostRepository _postRepository;

        public QueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<List<PostEntity>> HandleAsync(FindAllPostQueries query)
        {
            //throw new NotImplementedException();
            return await _postRepository.ListAllAsync();
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostByAuthorQuery query)
        {
            //throw new NotImplementedException();
            return await _postRepository.GetPostsByAuthorAsync(query.Author);
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query)
        {
            //throw new NotImplementedException();
            var post = await _postRepository.GetByIdAsync(query.Id);
            return new List<PostEntity> { post };
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostWithCommentsQuery query)
        {
            //throw new NotImplementedException();
            return await _postRepository.GetPostsWithCommentsAsync();
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostWithLikesQuery query)
        {
            //throw new NotImplementedException();

            return await _postRepository.GetWithLikesAsync(query.NumberOfLikes);
        }
    }
}
