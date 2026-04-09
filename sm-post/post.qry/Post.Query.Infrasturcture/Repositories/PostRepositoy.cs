using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrasturcture.DataAccess;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Post.Query.Infrasturcture.Repositories
{
    public class PostRepositoy : IPostRepository
    {
        private readonly DataBaseContextFactory _dbContextFactory;


        public PostRepositoy(DataBaseContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public async Task CreateAsync(PostEntity post)
        {
            //throw new NotImplementedException();
            using DataBaseContext context = _dbContextFactory.createDbContext();
            context.Posts.Add(post);
            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid postId)
        {
            using DataBaseContext context = _dbContextFactory.createDbContext();
            var post = await GetByIdAsync(postId);

            if(post == null) return;
            
            context.Posts.Remove(post);
            await context.SaveChangesAsync();

            //throw new NotImplementedException();
        }

        public async Task<PostEntity> GetByIdAsync(Guid id)
        {
            //throw new NotImplementedException();
            using DataBaseContext context = _dbContextFactory.createDbContext();
            return await context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(x => x.PostId == id);

        }

        public async Task<List<PostEntity>> GetPostsByAuthorAsync(string author)
        {
            //throw new NotImplementedException();
            using DataBaseContext context = _dbContextFactory.createDbContext();
            return await context.Posts.AsNoTracking()
                .Include(p => p.Comments).AsNoTracking()
                .Where(x => x.Author.Contains(author) )
                .ToListAsync();

        }

        public async Task<List<PostEntity>> GetPostsWithCommentsAsync()
        {
            //throw new NotImplementedException();
            using DataBaseContext context = _dbContextFactory.createDbContext();
            return await context.Posts.AsNoTracking()
                .Include(p => p.Comments).AsNoTracking()
                .Where(x => x.Comments != null && x.Comments.Any())
                .ToListAsync();

        }

        public async Task<List<PostEntity>> GetWithLikesAsync(int numberOfLikes)
        {
            //throw new NotImplementedException();
            using DataBaseContext context = _dbContextFactory.createDbContext();
            return await context.Posts.AsNoTracking()
                .Include(p => p.Comments).AsNoTracking()
                .Where(x => x.Likes >= numberOfLikes)
                .ToListAsync();

        }

        public async Task<List<PostEntity>> ListAllAsync()
        {
            //throw new NotImplementedException();
            using DataBaseContext context = _dbContextFactory.createDbContext();
            return await context.Posts.AsNoTracking()
                .Include(i => i.Comments).AsNoTracking()
                .ToListAsync();

        }

        public async Task UpdateAsync(PostEntity post)
        {
            //throw new NotImplementedException();
            using DataBaseContext context = _dbContextFactory.createDbContext();
            context.Posts.Update(post);
            await context.SaveChangesAsync();

        }
    }
}
