using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using Microsoft.Practices.Unity;
using YK.PropertyMgr.RepositoryContract;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.Crosscuting;

namespace YK.PropertyMgr.DomainService
{
	public partial class AlipayRoomDomainService
	{
		public bool InsertAlipayRoom(AlipayRoom domainAlipayRoom)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.AlipayRoomRepository.Add(domainAlipayRoom);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateAlipayRoom(AlipayRoom domainAlipayRoom)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.AlipayRoomRepository.Update(domainAlipayRoom);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteAlipayRoom(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.AlipayRoomRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<AlipayRoom> GetAlipayRooms()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.AlipayRoomRepository.GetAll().ToList();
            }
        }

		public AlipayRoom GetAlipayRoomByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.AlipayRoomRepository.GetByKey(id);
            }
        }

		public IList<AlipayRoom> Paging(int PageIndex, int PageSize, Expression<Func<AlipayRoom, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.AlipayRoomRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
