using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.RepositoryContract;


namespace YK.BackgroundMgr.Repository
{
    public partial class SEC_User_OwnerSEC_DeptRepository : BackgroundMgrRepository<SEC_User_OwnerSEC_Dept>, ISEC_User_OwnerSEC_DeptRepository
    {
        public SEC_User_OwnerSEC_DeptRepository(BackgroundMgrDataBaseContext context)
            : base(context)
        {
        }

        public override string TableName
        {
            get
            {
                return "SEC_User_OwnerSEC_Dept";
            }
        }
    }
}

