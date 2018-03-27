using System;
using System.Collections.Generic;
using System.Linq;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.CompositeDomainService;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;

namespace YK.PropertyMgr.CompositeAppService
{
    public class ReportAppService
    {

        #region 应收
        #region 应收报表-科目
        /// <summary>
        /// 应收报表-科目
        /// </summary>
        /// <param name="seachDTO"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static  IList<ReportTableDTO> ArrearsChargeSubjectReport(ReportSearchDTO seachDTO,out int totalCount)
        {

            List<string> strlist = new List<string>();
            string queryStr = string.Empty;

            strlist.Add(((int)BillStatusEnum.NoPayment).ToString());

            if (seachDTO.ComDeptList.Count == 0)
            {
                //没有小区权限
                totalCount = 0;
                return  new List<ReportTableDTO>() ;
            }
            else
            {
              List<string> ComDeptListstr=  seachDTO.ComDeptList.Select(o=>o.Id.ToString()).ToList<string>();
                strlist.Add("'"+string.Join(",", ComDeptListstr.ToArray())+"'");

            }

            if (seachDTO.ComDeptId > 0)
            {
                strlist.Add(seachDTO.ComDeptId.ToString());
            }
            else
               strlist.Add("NULL");
            

            if (!string.IsNullOrEmpty(seachDTO.DoorNumber))
            {
                strlist.Add("'"+seachDTO.DoorNumber+"'");
            }
            else
                strlist.Add("NULL");

            if (seachDTO.ChargeSubjectId > 0)
            {
                strlist.Add(seachDTO.ChargeSubjectId.ToString());
            }
            else
                strlist.Add("NULL");           


            if(seachDTO.BeginDate.Year>1900)
            {
                strlist.Add("'"+seachDTO.BeginDate.ToShortDateString()+"'");
            }
            else
                strlist.Add("NULL");

            if (seachDTO.EndDate.Year > 1900)
            {
                strlist.Add("'"+seachDTO.EndDate.AddDays(1).AddMilliseconds(-1).ToString()+"'");
            }
            else
                strlist.Add("NULL");

            queryStr = string.Join(",", strlist.ToArray());

            ResultModel resultModel = RePortService.Instance.ArrearsChargeSubjectReport(seachDTO.PageIndex, seachDTO.PageSize, out totalCount,queryStr);

            return (List<ReportTableDTO>)resultModel.Data;
        }





        #endregion


        #region 应收报表-小区
        /// <summary>
        /// 应收报表-科目
        /// </summary>
        /// <param name="seachDTO"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static IList<ReportTableDTO> ArrearsCommunityReport(ReportSearchDTO seachDTO, out int totalCount)
        {

            List<string> strlist = new List<string>();
            string queryStr = string.Empty;

            strlist.Add(((int)BillStatusEnum.NoPayment).ToString());

            if (seachDTO.ComDeptList.Count == 0)
            {
                //没有小区权限
                totalCount = 0;
                return new List<ReportTableDTO>();
            }
            else
            {
                List<string> ComDeptListstr = seachDTO.ComDeptList.Select(o => o.Id.ToString()).ToList<string>();
                strlist.Add("'" + string.Join(",", ComDeptListstr.ToArray()) + "'");

            }

            if (seachDTO.ComDeptId > 0)
            {
                strlist.Add(seachDTO.ComDeptId.ToString());
            }
            else
                strlist.Add("NULL");


            if (!string.IsNullOrEmpty(seachDTO.DoorNumber))
            {
                strlist.Add("'"+seachDTO.DoorNumber+"'");
            }
            else
                strlist.Add("NULL");

            if (seachDTO.ChargeSubjectId > 0)
            {
                strlist.Add(seachDTO.ChargeSubjectId.ToString());
            }
            else
                strlist.Add("NULL");


            if (seachDTO.BeginDate.Year > 1900)
            {
                strlist.Add("'"+seachDTO.BeginDate.ToShortDateString()+"'");
            }
            else
                strlist.Add("NULL");

            if (seachDTO.EndDate.Year > 1900)
            {
                strlist.Add("'"+seachDTO.EndDate.AddDays(1).AddMilliseconds(-1).ToString()+"'");
            }
            else
                strlist.Add("NULL");

            queryStr = string.Join(",", strlist.ToArray());

            ResultModel resultModel = RePortService.Instance.ArrearsCommunityReport(seachDTO.PageIndex, seachDTO.PageSize, out totalCount, queryStr);

            return (List<ReportTableDTO>)resultModel.Data;
        }





        #endregion


        #region 应收报表-明细
        /// <summary>
        /// 应收报表-科目
        /// </summary>
        /// <param name="seachDTO"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static IList<ReportTableDTO> ArrearsDetailReport(ReportSearchDTO seachDTO, out int totalCount)
        {

            List<string> strlist = new List<string>();
            string queryStr = string.Empty;

            strlist.Add(((int)BillStatusEnum.NoPayment).ToString());

            if (seachDTO.ComDeptList.Count == 0)
            {
                //没有小区权限
                totalCount = 0;
                return new List<ReportTableDTO>();
            }
            else
            {
                List<string> ComDeptListstr = seachDTO.ComDeptList.Select(o => o.Id.ToString()).ToList<string>();
                strlist.Add("'" + string.Join(",", ComDeptListstr.ToArray()) + "'");

            }

            if (seachDTO.ComDeptId > 0)
            {
                strlist.Add(seachDTO.ComDeptId.ToString());
            }
            else
                strlist.Add("NULL");


            if (!string.IsNullOrEmpty(seachDTO.DoorNumber))
            {
                strlist.Add("'"+seachDTO.DoorNumber+ "'");
            }
            else
                strlist.Add("NULL");

            if (seachDTO.ChargeSubjectId > 0)
            {
                strlist.Add(seachDTO.ChargeSubjectId.ToString());
            }
            else
                strlist.Add("NULL");


            if (seachDTO.BeginDate.Year > 1900)
            {
                strlist.Add("'" + seachDTO.BeginDate.ToShortDateString()+ "'");
            }
            else
                strlist.Add("NULL");

            if (seachDTO.EndDate.Year > 1900)
            {
                strlist.Add("'" + seachDTO.EndDate.AddDays(1).AddMilliseconds(-1).ToString()+ "'");
            }
            else
                strlist.Add("NULL");

            queryStr = string.Join(",", strlist.ToArray());

            ResultModel resultModel = RePortService.Instance.ArrearsDetailReport(seachDTO.PageIndex, seachDTO.PageSize, out totalCount, queryStr);

            return (List<ReportTableDTO>)resultModel.Data;
        }





        #endregion
        #endregion

        #region 实收

        #region 实收报表-科目
        /// <summary>
        /// 应收报表-科目
        /// </summary>
        /// <param name="seachDTO"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static IList<ReportTableDTO> CollectionsChargeSubjectReport(ReportSearchDTO seachDTO, out int totalCount)
        {

            List<string> strlist = new List<string>();
            string queryStr = string.Empty;

            strlist.Add(((int)BillStatusEnum.NoPayment).ToString());

            if (seachDTO.ComDeptList.Count == 0)
            {
                //没有小区权限
                totalCount = 0;
                return new List<ReportTableDTO>();
            }
            else
            {
                List<string> ComDeptListstr = seachDTO.ComDeptList.Select(o => o.Id.ToString()).ToList<string>();
                strlist.Add("'" + string.Join(",", ComDeptListstr.ToArray()) + "'");

            }

            if (seachDTO.ComDeptId > 0)
            {
                strlist.Add(seachDTO.ComDeptId.ToString());
            }
            else
                strlist.Add("NULL");


            if (!string.IsNullOrEmpty(seachDTO.DoorNumber))
            {
                strlist.Add("'" + seachDTO.DoorNumber+ "'");
            }
            else
                strlist.Add("NULL");

            if (seachDTO.ChargeSubjectId > 0)
            {
                strlist.Add(seachDTO.ChargeSubjectId.ToString());
            }
            else
                strlist.Add("NULL");


            if (seachDTO.BeginDate.Year > 1900)
            {
                strlist.Add("'"+seachDTO.BeginDate.ToShortDateString()+ "'");
            }
            else
                strlist.Add("NULL");

            if (seachDTO.EndDate.Year > 1900)
            {
                strlist.Add("'" + seachDTO.EndDate.AddDays(1).AddMilliseconds(-1).ToString()+ "'");
            }
            else
                strlist.Add("NULL");

            queryStr = string.Join(",", strlist.ToArray());

            ResultModel resultModel = RePortService.Instance.CollectionsChargeSubjectReport(seachDTO.PageIndex, seachDTO.PageSize, out totalCount, queryStr);

            return (List<ReportTableDTO>)resultModel.Data;
        }





        #endregion


        #region 实收报表-小区
        /// <summary>
        /// 应收报表-科目
        /// </summary>
        /// <param name="seachDTO"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static IList<ReportTableDTO> CollectionsCommunityReport(ReportSearchDTO seachDTO, out int totalCount)
        {

            List<string> strlist = new List<string>();
            string queryStr = string.Empty;

            strlist.Add(((int)BillStatusEnum.NoPayment).ToString());

            if (seachDTO.ComDeptList.Count == 0)
            {
                //没有小区权限
                totalCount = 0;
                return new List<ReportTableDTO>();
            }
            else
            {
                List<string> ComDeptListstr = seachDTO.ComDeptList.Select(o => o.Id.ToString()).ToList<string>();
                strlist.Add("'" + string.Join(",", ComDeptListstr.ToArray()) + "'");

            }

            if (seachDTO.ComDeptId > 0)
            {
                strlist.Add(seachDTO.ComDeptId.ToString());
            }
            else
                strlist.Add("NULL");


            if (!string.IsNullOrEmpty(seachDTO.DoorNumber))
            {
                strlist.Add("'" + seachDTO.DoorNumber+ "'");
            }
            else
                strlist.Add("NULL");

            if (seachDTO.ChargeSubjectId > 0)
            {
                strlist.Add(seachDTO.ChargeSubjectId.ToString());
            }
            else
                strlist.Add("NULL");


            if (seachDTO.BeginDate.Year > 1900)
            {
                strlist.Add("'" + seachDTO.BeginDate.ToShortDateString()+ "'");
            }
            else
                strlist.Add("NULL");

            if (seachDTO.EndDate.Year > 1900)
            {
                strlist.Add("'" + seachDTO.EndDate.AddDays(1).AddMilliseconds(-1).ToString()+ "'");
            }
            else
                strlist.Add("NULL");

            queryStr = string.Join(",", strlist.ToArray());

            ResultModel resultModel = RePortService.Instance.CollectionsCommunityReport(seachDTO.PageIndex, seachDTO.PageSize, out totalCount, queryStr);

            return (List<ReportTableDTO>)resultModel.Data;
        }





        #endregion


        #region 实收报表-明细
        /// <summary>
        /// 实收报表-科目
        /// </summary>
        /// <param name="seachDTO"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static IList<ChargeRecordDTO> CollectionsDetailReport(ReportSearchDTO seachDTO, out int totalCount)
        {

            if (seachDTO.ComDeptList.Count == 0)
            {
                //没有小区权限
                totalCount = 0;
                return new List<ChargeRecordDTO>();
            }
            Condition<ChargeRecord> condition = new Condition<ChargeRecord>(c => true);


            List<int?> ComDeptListstr = seachDTO.ComDeptList.Select(o => o.Id).ToList();

            condition = condition & new Condition<ChargeRecord>(c => ComDeptListstr.Contains(c.ComDeptId));


            if (seachDTO.ComDeptId > 0)
            {//小区
                condition = condition & new Condition<ChargeRecord>(c => c.ComDeptId == seachDTO.ComDeptId);
            }

            if (seachDTO.BeginDate.Year > 1900)
            {
                condition = condition & new Condition<ChargeRecord>(c => c.PayDate >= seachDTO.BeginDate);
            }

            if (seachDTO.EndDate.Year > 1900)
            {
                condition = condition & new Condition<ChargeRecord>(c => c.PayDate <= seachDTO.EndDate.AddDays(1).AddMilliseconds(-1));
            }

            totalCount = 0;
            string expressions = "PayDate desc";

             var dtoList =   RePortService.Instance.CollectionsDetailReport(seachDTO.PageStart, seachDTO.PageSize, condition.ExpressionBody, expressions, out totalCount);


            foreach (var item in dtoList)
            {
                item.HouseDoorNo = item.HouseDeptNos;
                if (!string.IsNullOrEmpty(item.HouseDeptNos))
                {
                    item.HouseDoorNoFormat = (item.HouseDeptNos.Length > 12 ? "<label style='font-weight:400' title='" + item.HouseDeptNos + "'>" + item.HouseDeptNos.Substring(0, 12) + "...</label>" : item.HouseDeptNos);
                }

                item.ComDeptName = seachDTO.ComDeptList.Where(o => o.Id == item.ComDeptId).First().Name;
            }


            return dtoList;
        }





        #endregion

        #region 实收报表金额合计
        public static decimal GetCollectionsTotaMoney(ReportSearchDTO seachDTO)
        {
            if (seachDTO.ComDeptList.Count == 0)
            {
                return 0;
            }
            Condition<ChargeRecord> condition = new Condition<ChargeRecord>(c => true);


            List<int?> ComDeptListstr = seachDTO.ComDeptList.Select(o => o.Id).ToList();

            condition = condition & new Condition<ChargeRecord>(c => ComDeptListstr.Contains(c.ComDeptId));


            if (seachDTO.ComDeptId > 0)
            {//小区
                condition = condition & new Condition<ChargeRecord>(c => c.ComDeptId == seachDTO.ComDeptId);
            }

            if (seachDTO.BeginDate.Year > 1900)
            {
                condition = condition & new Condition<ChargeRecord>(c => c.PayDate >= seachDTO.BeginDate);
            }

            if (seachDTO.EndDate.Year > 1900)
            {
                seachDTO.EndDate= seachDTO.EndDate.AddDays(1).AddMilliseconds(-1);
                condition = condition & new Condition<ChargeRecord>(c => c.PayDate <= seachDTO.EndDate);
            }

         return   RePortService.Instance.GetCollectionsMoney(condition.ExpressionBody);
        }
        #endregion


        #endregion

        #region 对比报表Echarts

        public static ReportTableDTO GetArrearsCollComparisoCharts(int ComDeptId)
        {
            var model = RePortService.Instance.GetArrearsCollComparisoCharts(ComDeptId);
            return (ReportTableDTO)model.Data;
        }
        #endregion

        #region  预交费明细报表
        public List<PrePaymentDetailReportDTO> GetPrePaymentDetailReportList(PrePaymentDetailSearchDTO search, out int totalCount, out decimal SumAmount ,bool IsExport = false)
        {
          return  RePortService.Instance.PrePaymentDetailReport(search, out totalCount,out SumAmount, IsExport);
        }
        #endregion

        #region 预交费抵扣明细报表
        public List<PrePaymentdeductionDetailReportDTO> GetPrePaymentdeductionDetailReportList(PrePaymentdeductionDetailSearchDTO search, out int totalCount, bool IsExport = false)
        {
            return RePortService.Instance.PrePaymentdeductionDetailReport(search, out totalCount, IsExport);
        }
        #endregion

        #region 欠费明细报表
        public List<ReportArrearsDetailDTO> GetArrearsReportDetailList(ReportArrearsSearchDTO search, out int totalCount, bool IsExport = false)
        {
            return RePortService.Instance.GetArrearsReportDetailList(search.PageIndex, search.PageSize, search, out  totalCount, IsExport);
        }
        #endregion

        #region 三表收费明细报表
        public List<ReportMeterDetailDTO> GetMeterReportDetailList(ReportMeterSearchDTO search, out int totalCount, bool IsExport = false)
        {
            return RePortService.Instance.GetMeterReportDetailList(search, out totalCount, IsExport);
        }
        #endregion

        #region 对外收费明细报表
        public List<ReportExternalchargeDetailDTO> GetExternalchargeReportDetailList(ReportExternalchargeSearchDTO search, out int totalCount, bool IsExport = false)
        {
            return RePortService.Instance.GetExternalchargeReportDetailList(search, out totalCount, IsExport);
        }
        #endregion

        #region 综合报表——2.8版本
        /// <summary>
        /// 收费项目汇总表
        /// </summary>
        /// <param name="seach"></param>
        /// <returns></returns>
        public IList<ReportTableDTO> GetIntegratedReportChargeSubjectList(ReportSearchDTO search)
        {
            //只显示未缴和已缴
            Condition<ChargBill> conditions = new Condition<ChargBill>(o => o.ComDeptId == search.ComDeptId && o.Status != (int)BillStatusEnum.Refunded && o.IsDel == false);
            Condition<ChargeRecord> conditions_Record = new Condition<ChargeRecord>(o => o.ComDeptId == search.ComDeptId && o.IsDel == false);

            if (search.LouyuIdStr != null && search.LouyuIdStr.Length > 0)
            {

                var LouyuList = search.LouyuIdStr.Split(',').ToList().ConvertAll(i => int.Parse(i));
                var bulidList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetBuildsInfoByBuildDeptId(LouyuList);
                string BuildCode = bulidList[0].Building_code;

                Condition<ChargBill> condition_bill_OR = new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(BuildCode));
                foreach (var c in bulidList)
                {
                    condition_bill_OR = condition_bill_OR | new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(c.Building_code));

                }
                conditions = conditions & condition_bill_OR;
            }




            //处理时间
            search.EndDate = search.EndDate.AddDays(1).AddMilliseconds(-1);
            search.Paydate = search.Paydate.AddDays(1).AddMilliseconds(-1);
            //时间
            conditions = conditions & new Condition<ChargBill>(o => ((o.BeginDate <= search.BeginDate) && (o.EndDate >= search.BeginDate)&&(search.EndDate>= search.BeginDate)) || ((search.BeginDate <= o.BeginDate) &&   (search.EndDate>=o.BeginDate)&&(search.EndDate >= search.BeginDate)));

            //缴费截止日期
            conditions_Record = conditions_Record & new Condition<ChargeRecord>(o => o.PayDate <= search.Paydate);
            return RePortService.Instance.GetIntegratedReportChargeSubjectList(conditions, conditions_Record);
        }

        /// <summary>
        /// 综合报表-资源明细表
        /// </summary>
        /// <param name="seach"></param>
        /// <returns></returns>
        public IList<ReportTableDTO> GetIntegratedReportHouseDetaillList(ReportSearchDTO seach, out int totalCount,bool IsExport=false)
        {
            //预存费不计入综合报表2.8需求
            Condition<ChargBill> conditions = new Condition<ChargBill>(o => o.ComDeptId == seach.ComDeptId && o.Status != (int)BillStatusEnum.Refunded&&o.IsDel==false);
            Condition<ChargeRecord> conditions_Record = new Condition<ChargeRecord>(o => o.ComDeptId == seach.ComDeptId&&o.IsDel==false);
            //处理时间
            seach.EndDate= seach.EndDate.AddDays(1).AddMilliseconds(-1);
            seach.Paydate=seach.Paydate.AddDays(1).AddMilliseconds(-1);
            //时间2
            conditions = conditions & new Condition<ChargBill>(o => ((o.BeginDate <= seach.BeginDate) && (o.EndDate >= seach.BeginDate) && (seach.EndDate >= seach.BeginDate)) || ((seach.BeginDate <= o.BeginDate) && (seach.EndDate >= o.BeginDate) && (seach.EndDate >= seach.BeginDate)));

            //缴费截止日期  
            conditions_Record = conditions_Record & new Condition<ChargeRecord>(o => o.PayDate <= seach.Paydate);

            if(!string.IsNullOrEmpty(seach.DoorNumber))
            {

                conditions = conditions & new Condition<ChargBill>(o => ((o.HouseDeptId > 0 || o.HouseDeptId != null)&&o.HouseDoorNo.Contains(seach.DoorNumber))||((o.HouseDeptId<=0||o.HouseDeptId==null)&&o.ResourcesName.Contains(seach.DoorNumber)));
            }

            if (seach.LouyuIdStr != null && seach.LouyuIdStr.Length > 0)
            {

                var LouyuList = seach.LouyuIdStr.Split(',').ToList().ConvertAll(i => int.Parse(i));
                var bulidList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetBuildsInfoByBuildDeptId(LouyuList);
                string BuildCode = bulidList[0].Building_code;

                Condition<ChargBill> condition_bill_OR = new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(BuildCode));
                foreach (var c in bulidList)
                {
                    condition_bill_OR = condition_bill_OR | new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(c.Building_code));

                }
                conditions = conditions & condition_bill_OR;
            }





            return RePortService.Instance.GetIntegratedReportHouseDetaillList(conditions, conditions_Record, out totalCount,seach.PageSize,seach.PageIndex,IsExport);
        }




        #endregion

    }
}
