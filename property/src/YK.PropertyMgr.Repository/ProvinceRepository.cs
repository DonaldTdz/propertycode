﻿using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.Repository
{
    public partial class ProvinceRepository: PropertyMgrRepository<Province>, IProvinceRepository
    {
        public ProvinceRepository(PropertyMgrDataBaseContext context)
            : base(context)
        {
        }

        public override string TableName
        {
            get
            {
                return "Province";
            }
        }
    }
}

