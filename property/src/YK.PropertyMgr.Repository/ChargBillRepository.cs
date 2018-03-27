﻿using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.Repository
{
    public partial class ChargBillRepository: PropertyMgrRepository<ChargBill>, IChargBillRepository
    {
        public ChargBillRepository(PropertyMgrDataBaseContext context)
            : base(context)
        {
        }

        public override string TableName
        {
            get
            {
                return "ChargBill";
            }
        }
    }
}

