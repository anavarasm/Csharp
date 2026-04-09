using CQRS.core.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.core.Infrastructure
{
    public interface IQueryDispatcher<TEntity>
    {
        // using generic and delegates
        void RegisterHandler<TQuery>(Func<TQuery, Task<List<TEntity>>> handler) where TQuery : BaseQuery;
        // leskvov subs  base should be substutable  applied here , we are using 2 methods
        Task<List<TEntity>> SendAsync(BaseQuery query);
    }
}
