using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    #region 终端Model

    #region 基础部分

    /// <summary>
    /// 资源model
    /// </summary>
    public class ClientDeptInfo
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 格式化名称
        /// </summary>
        public string FName
        {
            get
            {
                if (DeptType == ClientDeptType.House)
                {
                    var harry = Name.Split('-');
                    if (harry.Length == 4)
                    {
                        if (harry[0].IndexOf("栋") <= 0)
                        {
                            harry[0] += "栋";
                        }
                        if (harry[3].Length == 1)
                        {
                            harry[3] = "0" + harry[3];
                        }
                        return harry[0] + harry[1] + "单元" + harry[2] + "楼" + harry[2] + harry[3];
                    }
                }
                if (DeptType == ClientDeptType.Build)
                {
                    if (Name.IndexOf("栋") <= 0)
                    {
                        Name += "栋";
                    }
                }
                return Name;
            }
        }

        /// <summary>
        /// 格式化房屋编号
        /// </summary>
        public string FDoorNo
        {
            get
            {
                if (DeptType == ClientDeptType.House)
                {
                    var harry = Name.Split('-');
                    if (harry.Length == 4)
                    {
                        if (harry[3].Length == 1)
                        {
                            harry[3] = "0" + harry[3];
                        }
                        return harry[2] + harry[3];
                    }
                }
                return Name;
            }
        }

        /// <summary>
        /// 序号
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>
        public int? PId { get; set; }

        /// <summary>
        /// 编号 已占房屋编号，过滤房屋用
        /// </summary>
        public string Code { get; set; }

        public object Children { get; set; }

        /// <summary>
        /// 类型 1楼栋 2单元 3楼层 4单元
        /// </summary>
        public ClientDeptType DeptType { get; set; }
    }

    /// <summary>
    /// 导航model
    /// </summary>
    public class PageNavigation
    {
        /// <summary>
        /// 当前页开始序号
        /// </summary>
        public int? StartIndex { get; set; }

        /// <summary>
        /// 结束序号
        /// </summary>
        public int? EndIndex { get; set; }

        /// <summary>
        /// 第几页
        /// </summary>
        public int? PageIndex { get; set; }

        public int? Index
        {
            get
            {
                return PageIndex - 1;
            }
        }


        /// <summary>
        /// 显示标签
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 类型 1楼栋 2单元 3楼层 4单元
        /// </summary>
        public ClientDeptType DeptType { get; set; }
    }

    /// <summary>
    /// 资源层次
    /// </summary>
    public class HouseNoHierarchy
    {
        public int? Id { get; set; }
        public string BuildCode { get; set; }

        public string UnitCode { get; set; }

        public string FloorCode { get; set; }

        public string HouseCode { get; set; }

        public string HouseDoorNo { get; set; }
    }

    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ClientDeptType
    {
        Build = 1,
        Unit = 2,
        Floor = 3,
        House = 4
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class ClientUserInfo
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string FUserName
        {
            get
            {
                if (string.IsNullOrEmpty(UserName))
                {
                    return "[资料未录入]";
                }
                var len = UserName.Length;
                return "**" + UserName.Substring(len - 1);
            }
        }

        public string FPhoneNumber
        {
            get
            {
                if (string.IsNullOrEmpty(PhoneNumber))
                {
                    return "[资料未录入]";
                }
                if (PhoneNumber.Length != 11)
                {
                    return "[手机号有误]";
                }
                return "*******" + PhoneNumber.Substring(7);
            }
        }

        public object HouseData { get; set; }
    }

    #endregion

    #region 账单部分

    /// <summary>
    /// 账单分组
    /// </summary>
    public class ClientBillGroup
    {
        public ClientBillGroup()
        {
            this.IsChecked = true;
        }
        public string Name { get; set; }

        public int Count { get; set; }

        public decimal? TotalAmount { get; set; }

        public bool IsChecked { get; set; }

        public List<ClientBillInfo> BillData { get; set; }
    }

    /// <summary>
    /// 账单
    /// </summary>
    public class ClientBillInfo
    {
        public ClientBillInfo()
        {
            this.IsChecked = true;
        }
        public string BillId { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsChecked { get; set; }

        /// <summary>
        /// 欠费金额
        /// </summary>
        public decimal? Amount { get; set; }

        public string FBeginDate
        {
            get
            {
                if (BeginDate.HasValue)
                {
                    return BeginDate.Value.ToString("yyyy.MM.dd");
                }
                return string.Empty;
            }
        }

        public string FEndDate
        {
            get
            {
                if (EndDate.HasValue)
                {
                    return EndDate.Value.ToString("yyyy.MM.dd");
                }
                return string.Empty;
            }
        }
    }

    #endregion

    #region 预存费部分

    [Serializable]
    public class ClientSubject
    {
        public ClientSubject()
        {
            IsChecked = true;
        }

        public int? SubjectId { get; set; }

        public string SubjectName { get; set; }

        public decimal? MonthAmount { get; set; }

        public decimal? PreAmount { get; set; }

        public bool IsChecked { get; set; }
    }

    #endregion

    #region 支付部分

    [Serializable]
    public class ClientPayOrder
    {
        /// <summary>
        /// 支付订单编号
        /// 收费记录Id
        /// </summary>
        public string OrderNum { get; set; }

        /// <summary>
        /// 小区名
        /// </summary>
        public string CommunityName { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal? PayAmount { get; set; }

        /// <summary>
        /// 支付类别
        /// 1：账单 2：预存费
        /// </summary>
        public int? PayType { get; set; }

        /// <summary>
        /// 账单Ids
        /// </summary>
        public string[] Ids { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 房屋deptId
        /// </summary>
        public int? HouseDeptId { get; set; }

        /// <summary>
        /// 房屋编号
        /// </summary>
        public string HouseNo { get; set; }

        /// <summary>
        /// 预存月数
        /// </summary>
        public int? Month { get; set; }

        /// <summary>
        /// 收费项目
        /// </summary>
        public List<ClientSubject> Subjects { get; set; }
    }

    public class QRCode
    {
        /// <summary>
        /// 支付流水号
        /// </summary>
        public string NumericalNumber { get; set; }

        /// <summary>
        /// 支付宝扫码URL
        /// </summary>
        public string AlipayUrl { get; set; }

        /// <summary>
        /// 微信扫码URL
        /// </summary>
        public string WeChatUrl { get; set; }
    }

    #endregion

    #endregion
}
