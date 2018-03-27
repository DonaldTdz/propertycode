using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;
using YK.BackgroundMgr.RepositoryContract;
using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.Crosscuting;

namespace YK.BackgroundMgr.DomainService
{
	public partial class SEC_WeChatOpenIdDomainService
	{
		public bool InsertSEC_WeChatOpenId(SEC_WeChatOpenId domainSEC_WeChatOpenId)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                _BackgroundMgrUnitOfWork.SEC_WeChatOpenIdRepository.Add(domainSEC_WeChatOpenId);
                _BackgroundMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateSEC_WeChatOpenId(SEC_WeChatOpenId domainSEC_WeChatOpenId)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                _BackgroundMgrUnitOfWork.SEC_WeChatOpenIdRepository.Update(domainSEC_WeChatOpenId);
                _BackgroundMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteSEC_WeChatOpenId(object id)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                _BackgroundMgrUnitOfWork.SEC_WeChatOpenIdRepository.Delete(id);
                _BackgroundMgrUnitOfWork.Commit();
                return true;
            }
        }

		public SEC_WeChatOpenId GetSEC_WeChatOpenIdByKey(object id)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                return _BackgroundMgrUnitOfWork.SEC_WeChatOpenIdRepository.GetByKey(id);
            }
        }

        public List<SEC_WeChatOpenId> GetSEC_WeChatOpenIds()
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                return _BackgroundMgrUnitOfWork.SEC_WeChatOpenIdRepository.GetAll().ToList();
            }
        }
	}
}
