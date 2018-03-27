using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.Crosscuting;
using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.RepositoryContract;
using Microsoft.Practices.Unity;

namespace YK.BackgroundMgr.DomainService
{
    public partial class SEC_User_OwnerDomainService
    {
        public SEC_User_Owner GetUserOwnerMasterByHouseDeptId(int houseDeptId)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var houseUserList = from o in _BackgroundMgrUnitOfWork.SEC_User_OwnerRepository.GetAll()
                                    join h in _BackgroundMgrUnitOfWork.SEC_User_OwnerSEC_DeptRepository.GetAll()
                                    on o.Id equals h.SEC_User_Owner_Id
                                    where h.SEC_Dept_Id == houseDeptId && o.IsDelete == 0
                                    select o;
                if (houseUserList.Where(o => o.IsMaster == 1).Count() > 0)
                {
                    return houseUserList.Where(o => o.IsMaster == 1).FirstOrDefault();
                }
                return houseUserList.FirstOrDefault();
            }
        }
    }
}
