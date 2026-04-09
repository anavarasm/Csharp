using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;

namespace Post.Query.Infrasturcture.DataAccess
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext( DbContextOptions options) :base(options)
        {

        }

        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
    }
}
