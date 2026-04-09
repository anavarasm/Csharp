using Post.Query.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Post.Query.Domain.Repositories
{
    public interface ICommentRepository
    {
        Task CreateAsync(CommentEntity comment);
        Task UpdateAsync(CommentEntity comment);

        Task<CommentEntity> GetByIdAsync(Guid commentId);

        Task DeleteAsync(Guid commentId);
    }
}
