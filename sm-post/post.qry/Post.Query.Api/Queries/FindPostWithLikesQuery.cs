using CQRS.core.Queries;

namespace Post.Query.Api.Queries
{
    public class FindPostWithLikesQuery : BaseQuery
    {
        public int NumberOfLikes { get; set; }
    }
}
