using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.RepositoryContract;
using System.Linq.Expressions;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.BackgroundMgr.DomainInterface;
using YK.PropertyMgr.CompositeDomainService;

namespace YK.PropertyMgr.DomainService.Service
{
    public class CommunityConfigDomainService
    {
        /// <summary>
        /// 获得对应小区配置
        /// </summary>
        /// <param name="CommunityDeptId"></param>
        /// <returns></returns>
        public CommunityConfig GetCommunityConfig(int? CommunityDeptId)
        {
            
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return BillCommonService.Instance.GetCommunityConfig(pmUnitWork, CommunityDeptId);
            }
        }

        /// <summary>
        /// 通过资源和资源类型 获取小区配置
        /// </summary>
        /// <param name="resourceId">资源Id</param>
        /// <param name="resourceType">资源类型 1 房屋 2 车位 3 三表</param>
        /// <returns></returns>
        public CommunityConfig GetCommunityConfigByResourceDeptId(int? resourceId, int? resourceType)
        {
            int comDeptId = 0;
            //通过资源Id和资源类型获取小区Id
            switch (resourceType)
            {
                case 1://房屋
                    {
                        comDeptId = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCommunityDeptIdByHouseDeptId(resourceId.Value);
                    }; break;
                case 2://车位
                    {

                    }; break;
                case 3://三表
                    {

                    }; break;
                default:
                    break;
            }
            return GetCommunityConfig(comDeptId);
        }
    }
}
