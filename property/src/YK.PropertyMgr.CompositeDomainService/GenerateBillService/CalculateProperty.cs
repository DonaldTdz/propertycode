using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.CompositeDomainService.GenerateBillService.Interface;

namespace YK.PropertyMgr.CompositeDomainService.GenerateBillService
{
    public class CalculateProperty : ICalculateProperty
    {
        public CalculateProperty()
        {
            PublicAreaProperty = new AreaProperty();
        }

        /// <summary>
        /// 计算数量 如房屋面积，车位面积，三表读数
        /// key:计算对象枚举
        /// value:计算对象属性值
        /// </summary>
        public Dictionary<ChargeFormulaEnum, string> Properties
        {
            get;
            set;
        }
        /// <summary>
        /// 资源ID
        /// </summary>
        public int ResourcesId
        {
            get;
            set;
        }
        /// <summary>
        /// 小区DeptId 
        /// </summary>
        public int ComDeptId
        {
            get;
            set;
        }
        /// <summary>
        /// 房屋DeptId 
        /// </summary>
        public int? HouseDeptID
        {
            get;
            set;
        }

        /// <summary>
        /// 房屋状态 需要过滤掉未交房的
        /// </summary>
        public HouseStatusEnum? HouseStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 未售房是否绑定开发商
        /// </summary>
        public bool? UnsoldIsBindDeveloper { get; set; }

        public object ExtendProperty
        {
            get;
            set;
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourcesName
        {
            get;
            set;
        }

        /// <summary>
        /// 房屋门牌号
        /// </summary>
        public string HouseDoorNo { get; set; }

        #region 公区表属性

        //public bool? IsPublicAreaMeter { get; set; }

        //public int? HouseNumber { get; set; }

        //public decimal? TotalBuildArea { get; set; }

        //public decimal? BuildArea { get; set; }

        //public AllocationTypeEnum AllocationType { get; set; }

        public AreaProperty PublicAreaProperty { get; set; }

        #endregion
    }

    /// <summary>
    /// 公区表属性
    /// </summary>
    public class AreaProperty
    {
        //public List<CalculateProperty> HousePropertyList { get; set; }
        public bool? IsPublicAreaMeter { get; set; }

        public int? HouseNumber { get; set; }

        public decimal? TotalBuildArea { get; set; }

        public decimal? BuildArea { get; set; }

        public AllocationTypeEnum AllocationType { get; set; }
    }
}
