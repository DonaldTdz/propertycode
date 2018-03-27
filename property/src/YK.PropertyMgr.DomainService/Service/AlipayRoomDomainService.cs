using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using Microsoft.Practices.Unity;

namespace YK.PropertyMgr.DomainService
{
    public partial class AlipayRoomDomainService
    {

        public IList<AlipayRoom> GetAlipayRoomList(Expression<Func<AlipayRoom, bool>> predicate)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.AlipayRoomRepository.GetAll().Where(predicate).ToList();
            }
        }

        public List<string> GetSynchronizationRoomInfoID(int? ComDeptId, List<int> RoomHouseDeptId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {

                var DataBaseIds = propertyMgrUnitOfWork.AlipayRoomRepository.GetAll().Where(o => o.ComDeptId == ComDeptId && o.IsDel == false && RoomHouseDeptId.Contains(o.HouseDeptId.Value)).Select(o=>o.HouseDeptId).ToList();

                var DataBaseIdstr= DataBaseIds.ConvertAll<string>(x => x.ToString());
                var RoomHouseDeptIdstr =RoomHouseDeptId.ConvertAll<string>(x => x.ToString());
                var expectedList = RoomHouseDeptIdstr.Except(DataBaseIdstr).ToList();
                return expectedList;

            }
        }

        public bool InsertAlipayRoomBat(  List<AlipayRoom> list)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.AlipayRoomRepository.AddRange(list);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
             
        }
    }
}
