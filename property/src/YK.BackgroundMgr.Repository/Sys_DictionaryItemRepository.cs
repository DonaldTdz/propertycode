using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.Repository
{
    public partial class Sys_DictionaryItemRepository: BackgroundMgrRepository<Sys_DictionaryItem>, ISys_DictionaryItemRepository
    {
        public Sys_DictionaryItemRepository(BackgroundMgrDataBaseContext context)
            : base(context)
        {
        }

        public override string TableName
        {
            get
            {
                return "Sys_DictionaryItem";
            }
        }
    }
}

