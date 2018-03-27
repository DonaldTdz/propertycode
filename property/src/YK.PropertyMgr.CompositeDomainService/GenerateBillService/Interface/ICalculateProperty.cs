using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;

namespace YK.PropertyMgr.CompositeDomainService.GenerateBillService.Interface
{
    public interface ICalculateProperty
    {
        /// <summary>
        /// 计算数量 如房屋面积，车位面积，三表读数
        /// key:计算对象枚举
        /// value:计算对象属性值
        /// </summary>
        Dictionary<ChargeFormulaEnum, string> Properties { get; }

        /// <summary>
        /// 资源ID
        /// </summary>
        int ResourcesId { get; }

        /// <summary>
        /// 资源名称
        /// </summary>
        string ResourcesName { get; }

        /// <summary>
        /// 小区DeptId 
        /// </summary>
        int ComDeptId { get; }

        /// <summary>
        /// 房屋DeptId 
        /// </summary>
        int? HouseDeptID { get; }

        /// <summary>
        /// 房屋状态
        /// </summary>
        HouseStatusEnum? HouseStatus { get; }

        /// <summary>
        /// 未售房是否绑定开发商
        /// </summary>
        bool? UnsoldIsBindDeveloper { get; }

        /// <summary>
        /// 扩展属性增加
        /// </summary>
        object ExtendProperty { get; }

        /// <summary>
        /// 房屋门牌号
        /// </summary>
        string HouseDoorNo { get; }

        #region 公区表属性

        //bool? IsPublicAreaMeter { get;  }

        AreaProperty PublicAreaProperty { get;  }

        #endregion
    }
}
