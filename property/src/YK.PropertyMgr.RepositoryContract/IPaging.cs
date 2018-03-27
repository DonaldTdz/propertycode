using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.RepositoryContract
{
    public interface IPaging<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        IQueryable<TAggregateRoot> Paging(int PageIndex, int PageSize, Expression<Func<TAggregateRoot, bool>> predicate, string expressions, out int totalCount);
    }
}
