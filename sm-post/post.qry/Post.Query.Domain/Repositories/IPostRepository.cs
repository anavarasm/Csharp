using Post.Query.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Post.Query.Domain.Repositories
{
    public interface IPostRepository
    {
        Task CreateAsync(PostEntity post);

        Task UpdateAsync(PostEntity post);

        Task DeleteAsync(Guid postId);

        Task <PostEntity> GetByIdAsync(Guid id);

        Task<List<PostEntity>> ListAllAsync();

        Task<List<PostEntity>> GetPostsByAuthorAsync(string author);

        Task<List<PostEntity>> GetWithLikesAsync(int numberOfLikes);

        Task<List<PostEntity>> GetPostsWithCommentsAsync();
    }
}
