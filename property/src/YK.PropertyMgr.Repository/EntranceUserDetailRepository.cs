﻿using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.Repository
{
    public partial class EntranceUserDetailRepository: PropertyMgrRepository<EntranceUserDetail>, IEntranceUserDetailRepository
    {
        public EntranceUserDetailRepository(PropertyMgrDataBaseContext context)
            : base(context)
        {
        }

        public override string TableName
        {
            get
            {
                return "EntranceUserDetail";
            }
        }
    }
}

