﻿using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.Repository
{
    public partial class SEC_DeveloperRepository: BackgroundMgrRepository<SEC_Developer>, ISEC_DeveloperRepository
    {
        public SEC_DeveloperRepository(BackgroundMgrDataBaseContext context)
            : base(context)
        {
        }

        public override string TableName
        {
            get
            {
                return "SEC_Developer";
            }
        }
    }
}
