using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.RepositoryContract;

namespace YK.PropertyMgr.Repository
{

    public abstract class PropertyMgrRepository<TAggregateRoot> : Repository<PropertyMgrDataBaseContext, TAggregateRoot>, IPaging<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        protected PropertyMgrRepository(PropertyMgrDataBaseContext context)
            : base(context)
        {
        }

        public abstract override string TableName
        {
            get;
        }

        public virtual IQueryable<TAggregateRoot> Paging(int PageIndex, int PageSize, Expression<Func<TAggregateRoot, bool>> predicate, string expressions, out int totalCount)
        {
            totalCount = GetAll().Where(predicate).Count();
            if (PageSize < 0)
            {
                return GetAll().Where(predicate).Sorting(expressions);
            }
            var dataList = GetAll().Where(predicate).Sorting(expressions).Skip((PageIndex - 1) * PageSize).Take(PageSize);
            return dataList;
        }
    }
}
