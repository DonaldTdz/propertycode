using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class ReportMonthDTO
    {
        public ReportMonthDTO()
        {
            HasData = false;
            GroupId = 1;
        }
        /// <summary>
        /// 收费项目Id
        /// </summary>
        public int? Id { get; set; }

        public string SubjectName { get; set; }
        /// <summary>
        /// 应收情况
        /// </summary>
        public string TitleShould { get; set; }
        /// <summary>
        /// 实收情况
        /// </summary>
        public string TitleActual { get; set; }
        /// <summary>
        /// 欠收情况
        /// </summary>
        public string TitleArrears { get; set; }
        /// <summary>
        /// 往期
        /// </summary>
        public string TitlePast { get; set; }

        /// <summary>
        /// 预收预存
        /// </summary>
        public string TitlePreStore { get; set; }

        public int? SubjectType { get; set; }
        /// <summary>
        /// 应收户数
        /// </summary>
        public int? ShouldMonthHouses { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal? ShouldMonthAmount { get; set; }
        /// <summary>
        /// 实收户数
        /// </summary>
        public int? ActualMonthHouses { get; set; }
        /// <summary>
        /// 实收金额=“当月”+“预收金额”+“预存金额”
        /// </summary>
        //public decimal? ActualAmount
        //{
        //    get
        //    {
        //        decimal temp = 0;
        //        if (CurrentMonthAmount.HasValue)
        //        {
        //            temp += CurrentMonthAmount.Value;
        //        }
        //        if (PreAmount.HasValue)
        //        {
        //            temp += PreAmount.Value;
        //        }
        //        if (PreStoreAmount.HasValue)
        //        {
        //            temp += PreStoreAmount.Value;
        //        }
        //        return temp;
        //    }
        //}
        /// <summary>
        /// 当月收取
        /// </summary>
        public decimal? CurrentMonthAmount { get; set; }
        /// <summary>
        /// 预存抵扣
        /// </summary>
        public decimal? PreStoreDeduction { get; set; }
        /// <summary>
        /// 预收金额
        /// </summary>
        public decimal? PreAmount { get; set; }
        /// <summary>
        /// 预存金额
        /// </summary>
        public decimal? PreStoreAmount { get; set; }
        /// <summary>
        /// 当月欠款户数
        /// </summary>
        public int? MonthArrearsHouses { get; set; }
        /// <summary>
        /// 当月欠款
        /// </summary>
        public decimal? MonthArrears { get; set; }
        /// <summary>
        /// 往期欠款
        /// </summary>
        public decimal? BeforeMonthArrears { get; set; }
        /// <summary>
        /// 收往期
        /// </summary>
        public decimal? BeforeMonthAmount { get; set; }
        /// <summary>
        /// 收取率=“实收金额”/“应收金额”
        /// </summary>
        public string ReceivedRatio
        {
            get
            {
                decimal temp = 0;
                if (CurrentMonthAmount.HasValue)
                {
                    temp += CurrentMonthAmount.Value;
                }
                //add 2017-7-19 v2.8 6
                if (PreStoreDeduction.HasValue)
                {
                    temp += PreStoreDeduction.Value;
                }
                decimal should = 0;
                if (ShouldMonthAmount.HasValue)
                {
                    should += ShouldMonthAmount.Value;
                }
                if (should > 0 && temp > 0)
                {
                    decimal ratio = (temp / should) * 100;
                    return Math.Round(ratio, 2).ToString() + "%";
                }
                else
                    return "0.00%";
            }
        }

        public bool HasData { get; set; }

        public int GroupId { get; set; }
    }
}
