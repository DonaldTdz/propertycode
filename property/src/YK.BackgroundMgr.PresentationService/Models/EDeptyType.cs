using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.PresentationService
{
    /// <summary>
    /// 组织架构类型
    /// </summary>
    public enum EDeptType
    {
        /// <summary>
        /// 根节点
        /// </summary>
        RootNode = 1,

        /// <summary>
        /// 物业
        /// </summary>
        WuYE = 10,

        /// <summary>
        /// 小区
        /// </summary>
        XiaoQu = 11,

        /// <summary>
        /// 楼宇
        /// </summary>
        LouYu = 12,

        /// <summary>
        /// 车库
        /// </summary>
        CheKu = 13,

        /// <summary>
        /// 房屋
        /// </summary>
        FangWu = 20,

        /// <summary>
        /// 车位
        /// </summary>
        CheWei = 21,

        /// <summary>
        /// 车辆
        /// </summary>
        CheLiang = 22,

        /// <summary>
        /// 公共资源
        /// </summary>
        GongGongZiYuan = 23,

        /// <summary>
        /// 设备
        /// </summary>
        SheBei = 24,

        /// <summary>
        /// 设备
        /// </summary>
        GateWay = 25,

        /// <summary>
        /// 开发商
        /// </summary>
        KaiFaShang = 30,

        /// <summary>
        /// 业主
        /// </summary>
        UserOwner = 31,

        /// <summary>
        /// 其他
        /// </summary>
        Others = 100,

        /// <summary>
        /// 自定义
        /// </summary>
        Custom = 200
    }
}
