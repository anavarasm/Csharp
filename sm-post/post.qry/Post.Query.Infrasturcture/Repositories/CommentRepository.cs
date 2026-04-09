using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrasturcture.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Post.Query.Infrasturcture.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataBaseContextFactory _contextFactory;

        public CommentRepository(DataBaseContextFactory dataBaseContextFactory)
        {
            _contextFactory = dataBaseContextFactory;
        }
        public async Task CreateAsync(CommentEntity comment)
        {
            //throw new NotImplementedException();
            using DataBaseContext dbContext = _contextFactory.createDbContext();
            dbContext.Comments.Add(comment);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid commentId)
        {
            //throw new NotImplementedException();
            using DataBaseContext dbContext = _contextFactory.createDbContext();
            var comment = await GetByIdAsync(commentId);
            if (comment == null) return;

           dbContext.Comments.Remove(comment);
            await dbContext.SaveChangesAsync();
        }

        public async Task<CommentEntity> GetByIdAsync(Guid commentId)
        {
            //throw new NotImplementedException();
            using DataBaseContext dbContext = _contextFactory.createDbContext();
            return await dbContext.Comments.FirstOrDefaultAsync(x => x.CommentId == commentId);
        }

        public async Task UpdateAsync(CommentEntity comment)
        {
            //throw new NotImplementedException();
            using DataBaseContext dbContext = _contextFactory.createDbContext();
            dbContext.Comments.Update(comment);
            await dbContext.SaveChangesAsync();

        }
    }
}
