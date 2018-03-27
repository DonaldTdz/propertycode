using System;
using System.Collections.Generic;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.DomainService.Service;

namespace YK.PropertyMgr.ApplicationService
{
    public partial class CommunityConfigAppService
    {
        /// <summary>
        /// 查找小区票据配置信息
        /// </summary>
        public CommunityConfig GetCommunityConfig(int? CommunityDeptId)
        {
            CommunityConfigDomainService Service = new CommunityConfigDomainService();
            return Service.GetCommunityConfig(CommunityDeptId);
        }

        /// <summary>
        /// 插入配置信息
        /// </summary>
        public ReturnResult SaveData(CommunityConfigDTO model)
        {
            ReturnResult res = Validation(model);
            if (res.IsSuccess)
            {
                //新增
                if (model.Id == 0)
                {
                    res.IsSuccess = InsertCommunityConfig(model);     
                }
                //编辑
                else
                {
                    res.IsSuccess = UpdateCommunityConfig(model);
                }
                res.Msg = res.IsSuccess ? "配置成功！" : "配置失败！";
            } 
            return res;
        }

        /// <summary>
        /// 数据验证
        /// </summary>
        public ReturnResult Validation(CommunityConfigDTO model)
        {

            ReturnResult res = new ReturnResult()
            {
                IsSuccess = true
            };
            if (model.ComDeptId == 0)
            {
                res.Msg = "小区ID不能为空!";
                res.IsSuccess = false;
                return res;
            }
            if (model.IsBuilding==false && model.IsFloor==false && model.IsNumber==false && model.IsUnit==false)
            {
                res.Msg = "请至少选择一个房号打印项!";
                res.IsSuccess = false;
                return res;
            }
            return res;

        }

        /// <summary>
        /// 通过资源Id和资源类型获取小区配置
        /// </summary>
        public CommunityConfigDTO GetCommunityConfigByResourceDeptId(int? resourceId, int? resourceType)
        {
            CommunityConfigDomainService service = new CommunityConfigDomainService();
            var comConfig = service.GetCommunityConfigByResourceDeptId(resourceId, resourceType);
            return CommunityConfigMappers.ChangeCommunityConfigToDTO(comConfig);
        }
    }
}
