using System.Threading.Tasks;
using System.Linq.Expressions;

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.RepositoryContract;

namespace YK.PropertyMgr.DomainService
{
    public partial class EntranceUserDomainService
    {
        /// <summary>
        /// 获取单个EntranceUser
        /// </summary>
        /// <returns></returns>
        public EntranceUser GetEntranceUser(Expression<Func<EntranceUser, bool>> where)
        {
            EntranceUser model = null;
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                model = propertyMgrUnitOfWork.EntranceUserRepository.GetAll().Where(where).FirstOrDefault();
                return model;
            }
        }
        /// <summary>
        /// 获取EntranceUserList
        /// </summary>
        /// <returns></returns>
        public List<EntranceUser> GetEntranceUserList(Expression<Func<EntranceUser, bool>> where)
        {
         
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.EntranceUserRepository.GetAll().Where(where).ToList();
            }
        }

        /// <summary>
        /// 批量授权处理
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool BatchUserRightPower(List<EntranceUser> list)
        {

            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.EntranceUserRepository.AddRange(list.Where(o => o.Id == 0));
                propertyMgrUnitOfWork.EntranceUserRepository.UpdateRange(list.Where(o => o.Id > 0));
                return propertyMgrUnitOfWork.Commit();
            }
        }

      
    }
}
