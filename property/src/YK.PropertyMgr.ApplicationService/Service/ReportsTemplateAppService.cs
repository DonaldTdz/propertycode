using Aspose.Cells;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.PresentationService;
using YK.FrameworkTools.ExcelService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.CompositeDomainService;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.DomainService;
using YK.PropertyMgr.RepositoryContract;

namespace YK.PropertyMgr.ApplicationService
{
    public class ReportsTemplateAppService
    {



        public IEnumerable<TemplateModel> GetArrearsChargeSubjectListTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "ChargeSubjectName", ColumnDesc = "收费项目", Seq = i++},
                new TemplateColumn(){ ColumnName = "TotalRecAmount", ColumnDesc = "金额", Seq = i++},


            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ReportTableDTO), showColumns);
            return template;
        }

        public IEnumerable<TemplateModel> GetArrearsCommunityListTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "CommunityName", ColumnDesc = "小区名称", Seq = i++},
                new TemplateColumn(){ ColumnName = "TotalRecAmount", ColumnDesc = "金额", Seq = i++},


            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ReportTableDTO), showColumns);
            return template;
        }

        public IEnumerable<TemplateModel> GetArrearsDetailListTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "CommunityName", ColumnDesc = "小区名称", Seq = i++},
                new TemplateColumn(){ ColumnName = "ResourceName", ColumnDesc = "资源名称", Seq = i++},
                new TemplateColumn(){ ColumnName = "ChargeSubjectName", ColumnDesc = "科目名称", Seq = i++},
                new TemplateColumn(){ ColumnName = "TotalRecAmount", ColumnDesc = "应收金额", Seq = i++},
                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++},
                new TemplateColumn(){ ColumnName = "CreateTime", ColumnDesc = "生成时间", Seq = i++,Type = "date"},
                new TemplateColumn(){ ColumnName = "BeginDate", ColumnDesc = "开始时间", Seq = i++,Type = "date"},
                new TemplateColumn(){ ColumnName = "EndDate", ColumnDesc = "结束时间", Seq = i++,Type = "date"},
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ReportTableDTO), showColumns);
            return template;
        }


        public IEnumerable<TemplateModel> GetCollectionsChargeSubjectListTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "ChargeSubjectName", ColumnDesc = "收费项目", Seq = i++},
                new TemplateColumn(){ ColumnName = "TotalRecAmount", ColumnDesc = "金额", Seq = i++},


            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ReportTableDTO), showColumns);
            return template;
        }


        public IEnumerable<TemplateModel> GetCollectionsCommunityListTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "CommunityName", ColumnDesc = "小区名称", Seq = i++},
                new TemplateColumn(){ ColumnName = "TotalRecAmount", ColumnDesc = "金额", Seq = i++},


            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ReportTableDTO), showColumns);
            return template;
        }


        public IEnumerable<TemplateModel> GetCollectionsDetailListTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {


                new TemplateColumn(){ ColumnName = "ComDeptName", ColumnDesc = "小区", Seq = i++},
                    new TemplateColumn(){ ColumnName = "ReceiptNum", ColumnDesc = "票据号", Seq = i++},
                new TemplateColumn(){ ColumnName = "PayDate", ColumnDesc = "收费日期", Seq = i++, Type = "datetime"},
                new TemplateColumn(){ ColumnName = "Amount", ColumnDesc = "收费金额", Seq = i++},
                new TemplateColumn(){ ColumnName = "ChargeSubjectName", ColumnDesc = "科目名称", Seq = i++},
                new TemplateColumn(){ ColumnName = "ChargeType", ColumnDesc = "收费类型", Seq = i++, DictId = PropertyEnumType.ChargeType.ToString()},
                new TemplateColumn(){ ColumnName = "HouseDoorNoFormat", ColumnDesc = "房屋号", Seq = i++},
                new TemplateColumn(){ ColumnName = "PayMthodId", ColumnDesc = "付款方式", Seq = i++, DictId = PropertyEnumType.PayType.ToString()},

                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++}

            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ChargeRecordDTO), showColumns);
            return template;
        }


        public IEnumerable<TemplateModel> GetIntegratedReportByChargeSubjectTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {

                new TemplateColumn(){ ColumnName = "ChargeSubjectName", ColumnDesc = "收费项目", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "TotalRecAmount", ColumnDesc = "账单应收金额", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "RececiveTotal", ColumnDesc = "账单已收金额", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "ReliefAmountTotal", ColumnDesc = "账单优惠金额", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "UnPaidAmountTotal", ColumnDesc = "账单欠收金额", Seq = i++,IsExport=true,DictId="-1"},
                 new TemplateColumn(){ ColumnName = "PayRate", ColumnDesc = "账单收缴率", Seq = i++,IsExport=true,DictId="-1"}

            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ReportTableDTO), showColumns);
            return template;
        }

        public IEnumerable<TemplateModel> GetIntegratedReportByHouseTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "ResourceName", ColumnDesc = "资源名称", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "OwnerUserName", ColumnDesc = "业主姓名", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "TotalRecAmount", ColumnDesc = "应收金额", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "RececiveTotal", ColumnDesc = "实收金额", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "UnPaidAmountTotal", ColumnDesc = "欠收金额", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "PayRate", ColumnDesc = "收费率", Seq = i++,IsExport=true,DictId="-1"}

            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ReportTableDTO), showColumns);
            return template;
        }

        #region 收款日报表

        public IEnumerable<TemplateModel> GetDayReportTemplate(string NMonth)
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "SubjectName", ColumnDesc = "收费项目", Seq = i++, IsExport = true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "BeforeMonthAmount", ColumnDesc = NMonth + "月前期", Seq = i++,IsExport = true, DictId="-1"},
                new TemplateColumn(){ ColumnName = "CurrentMonthAmount", ColumnDesc = "当月", Seq = i++, IsExport = true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "PreAmount", ColumnDesc = "预收金额", Seq = i++, IsExport = true, DictId="-1"},
                new TemplateColumn(){ ColumnName = "PreStoreAmount", ColumnDesc = "预存金额", Seq = i++, IsExport = true, DictId="-1"},
                new TemplateColumn(){ ColumnName = "ShowActualAmount", ColumnDesc = "实收金额", Seq = i++, IsExport = true, DictId="-1"},
                new TemplateColumn(){ ColumnName = "Refund", ColumnDesc = "实收退款", Seq = i++, IsExport = true, DictId="-1"},
                new TemplateColumn(){ ColumnName = "PreRefund", ColumnDesc = "预收退款", Seq = i++, IsExport = true, DictId="-1"},
                new TemplateColumn(){ ColumnName = "PreStoreRefund", ColumnDesc = "预存退款", Seq = i++, IsExport = true, DictId="-1"},
                new TemplateColumn(){ ColumnName = "ShowTotalAmount", ColumnDesc = "合计", Seq = i++, IsExport = true, DictId="-1"},
                new TemplateColumn(){ ColumnName = "PreStoreDeduction", ColumnDesc = "预存抵扣", Seq = i++, IsExport = true, DictId="-1"}
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ReportDayDTO), showColumns);
            return template;
        }

        public IEnumerable<TemplateModel> GetDayMonthReportDetailTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "ResourcesName", ColumnDesc = "资源名称", Seq = i++, IsExport = true},
                new TemplateColumn(){ ColumnName = "CustomerName", ColumnDesc = "客户", Seq = i++, IsExport = true},
                new TemplateColumn(){ ColumnName = "BillDesc", ColumnDesc = "收费项目", Seq = i++, IsExport = true},
                new TemplateColumn(){ ColumnName = "ChargeType", ColumnDesc = "收费类型", Seq = i++, IsExport = true, DictId = PropertyEnumType.ChargeType.ToString()},
                new TemplateColumn(){ ColumnName = "StartDate", ColumnDesc = "账单开始", Seq = i++, IsExport = true, Type = "date"},
                new TemplateColumn(){ ColumnName = "EndDate", ColumnDesc = "账单结束", Seq = i++, IsExport = true, Type = "date" },
                new TemplateColumn(){ ColumnName = "BillAmount", ColumnDesc = "账单金额", Seq = i++, IsExport = true},
                new TemplateColumn(){ ColumnName = "Amount", ColumnDesc = "收费金额", Seq = i++, IsExport = true},
                new TemplateColumn(){ ColumnName = "ChargeDate", ColumnDesc = "收费日期", Seq = i++, IsExport = true, Type = "date"},
                new TemplateColumn(){ ColumnName = "ReceiptNum", ColumnDesc = "票据号", Seq = i++, IsExport = true},
                new TemplateColumn(){ ColumnName = "OperatorName", ColumnDesc = "操作人", Seq = i++, IsExport = true},
                new TemplateColumn(){ ColumnName = "PayType", ColumnDesc = "支付方式", Seq = i++, IsExport = true, DictId = PropertyEnumType.PayType.ToString()},
                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++, IsExport = true}
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(BillDetailInfo), showColumns);
            return template;
        }

        public IList<ReportDayDTO> GetDayReportDataList(ReportDaySearchDTO search, out int outCount)
        {
            if (!search.ComDeptId.HasValue)
            {
                outCount = 0;
                return new List<ReportDayDTO>();
            }
            outCount = 0;
            return new List<ReportDayDTO>();
        }

        public IList<ReportDayDTO> GetDayReportDataList(ReportDaySearchDTO search)
        {
            RePortService service = new RePortService();
            return service.GetDayReportDataList(search);
        }

        public IList<BillDetailInfo> GetDayDetailReportDataList(ReportDaySearchDTO search, out int outCount)
        {
            if (!search.ComDeptId.HasValue)
            {
                outCount = 0;
                return new List<BillDetailInfo>();
            }
            Condition<BillDetailInfo> condition = new Condition<BillDetailInfo>(c => c.DeptId == search.ComDeptId);
            Condition<ChargBill> conditions = new Condition<ChargBill>(o => true);
            if (search.ChargeDate.HasValue)
            {
                var maxDate = search.ChargeDate.Value.AddDays(1);
                condition = condition & new Condition<BillDetailInfo>(c => c.ChargeDate >= search.ChargeDate && c.ChargeDate < maxDate);
            }

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






            RePortService service = new RePortService();
            return service.GetBillDetailReportDataList(condition.ExpressionBody, conditions.ExpressionBody, "ChargeDate desc", out outCount, search.PageStart, search.PageSize); 
        }

        public IList<BillDetailInfo> GetDayDetailReportExportData(ReportDaySearchDTO search)
        {
            if (!search.ComDeptId.HasValue)
            {
                return new List<BillDetailInfo>();
            }
            Condition<BillDetailInfo> condition = new Condition<BillDetailInfo>(c => c.DeptId == search.ComDeptId);
            Condition<ChargBill> conditions = new Condition<ChargBill>(o => true);
            if (search.ChargeDate.HasValue)
            {
                var maxDate = search.ChargeDate.Value.AddDays(1);
                condition = condition & new Condition<BillDetailInfo>(c => c.ChargeDate >= search.ChargeDate && c.ChargeDate < maxDate);
            }
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
            RePortService service = new RePortService();
            return service.GetDayDetailReportExportData(condition.ExpressionBody, conditions.ExpressionBody, "ChargeDate desc");
        }

        #endregion

        #region 收款月报表

        public List<IEnumerable<ExcelTemplateModel>> GetMonthReportColumns()
        {

            int i = 1;
            TemplateColumn[] showColumns1 = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "SubjectName", ColumnDesc = "收费项目",Seq = i++, IsExport = true, Rowspan = 2},
                new TemplateColumn(){ ColumnName = "TitleShould", ColumnDesc = "本期应收",Seq = i++, IsExport = true, IsListColumn = false, Colspan = 2},
                new TemplateColumn(){ ColumnName = "TitleActual", ColumnDesc = "本期实收",Seq = i++, IsExport = true, IsListColumn = false, Colspan = 3},
                new TemplateColumn(){ ColumnName = "TitleArrears", ColumnDesc = "本期欠收",Seq = i++, IsExport = true, IsListColumn = false, Colspan = 2},
                new TemplateColumn(){ ColumnName = "TitlePreStore", ColumnDesc = "预收预存",Seq = i++, IsExport = true, IsListColumn = false, Colspan = 2},
                new TemplateColumn(){ ColumnName = "TitlePast", ColumnDesc = "往期",Seq = i++, IsExport = true, IsListColumn = false, Colspan = 2},
                new TemplateColumn(){ ColumnName = "ReceivedRatio", ColumnDesc = "本期收费率", Seq = 18, IsExport = true, Rowspan = 2}
            };
            IEnumerable<ExcelTemplateModel> template1 = TemplateModelHelper.GetTemplateModels(typeof(ReportMonthDTO), showColumns1);
            TemplateColumn[] showColumns2 = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "ShouldMonthHouses", ColumnDesc = "户数",Seq = i++, IsExport = true},
                new TemplateColumn(){ ColumnName = "ShouldMonthAmount", ColumnDesc = "应收金额", Seq = i++, IsExport = true},
                new TemplateColumn(){ ColumnName = "ActualMonthHouses", ColumnDesc = "户数", Seq = i++, IsExport = true},
                new TemplateColumn(){ ColumnName = "CurrentMonthAmount", ColumnDesc = "实收金额", Seq = i++, IsExport = true},
                new TemplateColumn(){ ColumnName = "PreStoreDeduction", ColumnDesc = "预存抵扣", Seq = i++, IsExport = true},
                new TemplateColumn(){ ColumnName = "MonthArrearsHouses", ColumnDesc = "户数", Seq = i++, IsExport = true},
                new TemplateColumn(){ ColumnName = "MonthArrears", ColumnDesc = "欠收金额", Seq = i++, IsExport = true},
                new TemplateColumn(){ ColumnName = "PreAmount", ColumnDesc = "预收金额", Seq = i++, IsExport = true},
                new TemplateColumn(){ ColumnName = "PreStoreAmount", ColumnDesc = "预存金额", Seq = i++, IsExport = true},
                new TemplateColumn(){ ColumnName = "BeforeMonthArrears", ColumnDesc = "欠收金额", Seq = i++, IsExport = true},
                new TemplateColumn(){ ColumnName = "BeforeMonthAmount", ColumnDesc = "收往期", Seq = i++, IsExport = true}
            };
            IEnumerable<ExcelTemplateModel> template2 = TemplateModelHelper.GetTemplateModels(typeof(ReportMonthDTO), showColumns2);
            List<IEnumerable<ExcelTemplateModel>> lst = new List<IEnumerable<ExcelTemplateModel>>();
            lst.Add(template1);
            lst.Add(template2);
            return lst;
        }

        public IList<ReportMonthDTO> GetMonthReportDataList(ReportDaySearchDTO search)
        {
            RePortService service = new RePortService();
            return service.GetMonthReportDataList(search);
        }

        public IList<BillDetailInfo> GetMonthDetailReportDataList(ReportDaySearchDTO search, out int outCount)
        {
            if (!search.ComDeptId.HasValue)
            {
                outCount = 0;
                return new List<BillDetailInfo>();
            }
            Condition<BillDetailInfo> condition = new Condition<BillDetailInfo>(c => c.DeptId == search.ComDeptId);
            Condition<ChargBill> conditions = new Condition<ChargBill>(o => true);
            if (search.ChargeDate.HasValue)
            {
                var maxDate = search.ChargeDate.Value.AddMonths(1);
                condition = condition & new Condition<BillDetailInfo>(c => c.ChargeDate >= search.ChargeDate && c.ChargeDate < maxDate);
            }
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

            RePortService service = new RePortService();




            return service.GetBillDetailReportDataList(condition.ExpressionBody, conditions.ExpressionBody, "ChargeDate desc", out outCount, search.PageStart, search.PageSize);
        }

        public IList<BillDetailInfo> GetMonthDetailReportExportData(ReportDaySearchDTO search)
        {
            if (!search.ComDeptId.HasValue)
            {
                return new List<BillDetailInfo>();
            }
            Condition<BillDetailInfo> condition = new Condition<BillDetailInfo>(c => c.DeptId == search.ComDeptId);
            Condition<ChargBill> conditions = new Condition<ChargBill>(o => true);
            if (search.ChargeDate.HasValue)
            {
                var maxDate = search.ChargeDate.Value.AddMonths(1);
                condition = condition & new Condition<BillDetailInfo>(c => c.ChargeDate >= search.ChargeDate && c.ChargeDate < maxDate);
            }
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
            RePortService service = new RePortService();
            return service.GetDayDetailReportExportData(condition.ExpressionBody, conditions.ExpressionBody, "ChargeDate desc");
        }

        #endregion

        #region 三表收费报表
        public ReportMeterModels GetMeterReportDataList(ReportMeterSearchDTO search, out int totalCount)
        {
            RePortService service = new RePortService();
            var retunrModel = service.GetMeterReportDataList(search.PageIndex, search.PageSize, search, out totalCount);
            return retunrModel;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="exportDatas"></param>
        /// <param name="templateModels"></param>
        /// <returns></returns>
        public ReportMeterExportModel MeterReportExport(ReportMeterSearchDTO search)
        {
            RePortService service = new RePortService();
            var model = service.GetMeterReportExport(search);
            return model;
        }

        /// <summary>
        /// 明细报表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TemplateModel> GetMeterDetailReportTemplateModels()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "DoorNoResourcesName", ColumnDesc = "资源名称", Seq = i++,IsExport=true,DictId="-1"},//修改4575 2017/7/5 zzh
                new TemplateColumn(){ ColumnName = "OwnerName", ColumnDesc = "业主姓名", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "ChargeSubjectName", ColumnDesc = "收费项目", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "ChargeType", ColumnDesc = "收费类型", Seq = i++,IsExport=true,DictId=PropertyEnumType.ChargeType.ToString()},
                new TemplateColumn(){ ColumnName = "BeginDateFormat", ColumnDesc = "账单开始", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "EndDateFormat", ColumnDesc = "账单结束", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "BillAmount", ColumnDesc = "账单金额", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "ArrearsAmount", ColumnDesc = "收费金额", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "PayDateFormat", ColumnDesc = "收费日期", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "Number", ColumnDesc = "票据号", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "OperatorName", ColumnDesc = "操作人", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "PayMthodId", ColumnDesc = "付款方式", Seq = i++,IsExport=true,DictId= PropertyEnumType.PayType.ToString()},
                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++,IsExport=true,DictId="-1"}
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ReportMeterDetailDTO), showColumns);
            return template;
        }
        #endregion

        #region 对外收费报表
        public ReportExternalchargeModels GetExternalchargeReportDataList(ReportExternalchargeSearchDTO search, out int totalCount)
        {
            RePortService service = new RePortService();
            var retunrModel = service.GetExternalchargeReportDataList(search.PageIndex, search.PageSize, search, out totalCount);
            return retunrModel;
        }

        /// <summary>
        /// 对外收费表导出
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public ReportExternalchargeExportModel ExternalchargeReportExport(ReportExternalchargeSearchDTO search)
        {
            RePortService service = new RePortService();
            var model = service.GetExternalchargeReportExport(search);
            return model;
        }
        /// <summary>
        /// 对外收费明细报表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TemplateModel> GetExternalchargeDetailReportTemplateModels()
        {
            int i = 1; 
             TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "ResourcesName", ColumnDesc = "收费对象", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "ChargeSubjectName", ColumnDesc = "收费项目", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "BeginDateFormat", ColumnDesc = "账单开始", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "EndDateFormat", ColumnDesc = "账单结束", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "BillAmount", ColumnDesc = "账单金额", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "ArrearsAmount", ColumnDesc = "收费金额", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "PayDateFormat", ColumnDesc = "收费日期", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "Number", ColumnDesc = "票据号", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "OperatorName", ColumnDesc = "操作人", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "PayMthodId", ColumnDesc = "付款方式", Seq = i++,IsExport=true,DictId= PropertyEnumType.PayType.ToString()},
                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++,IsExport=true,DictId="-1"}
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ReportExternalchargeDetailDTO), showColumns);
            return template;
        }
        #endregion

        #region  欠费报表

        public ReportArrearsModels GetArrearsReportDataList(ReportArrearsSearchDTO search, out int totalCount)
        {
            RePortService service = new RePortService();

           var retunrModel = service.GetArrearsReportDataList(search.PageIndex, search.PageSize, search, out totalCount);
           // var retunrModel = service.GetArrearsReportDataList(1, 10, search, out totalCount);
            return retunrModel;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="exportDatas"></param>
        /// <param name="templateModels"></param>
        /// <returns></returns>
        public ReportArrearsExportModel ArrearsReportExport(ReportArrearsSearchDTO search)
        {
            RePortService service = new RePortService();
            var model = service.GetArrearsReportExport(search);
            return model;
        }


        private  void SetIndex(ref string startIndex, ref string startRange, ref string cellIndex)
        {
            if (startIndex == "Z")
            {
                startIndex = "A";
                startRange = string.IsNullOrEmpty(startRange) ? "A" : ((char)(startRange[0] + 1)).ToString();
            }
            else
            {
                startIndex = ((char)(startIndex[0] + 1)).ToString();
            }
            cellIndex = startRange + startIndex;

        }



        #endregion

        #region 预交费明细表
        public IEnumerable<TemplateModel> GetPrePaymentDetalReportTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "ResourcesName", ColumnDesc = "资源名称", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "CustomerName", ColumnDesc = "业主姓名", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "ReceiptNum", ColumnDesc = "票据号", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "ChargeSubjectName", ColumnDesc = "收费项目", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "ChargePayDateStr", ColumnDesc = "收费日期", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "BillAbstract", ColumnDesc = "摘要", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "PreType", ColumnDesc = "预交方式", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "Amount", ColumnDesc = "金额", Seq = i++,IsExport=true,DictId="-1"}
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(PrePaymentDetailReportDTO), showColumns);
            return template;
        }

        #endregion

        #region 预交费抵扣明细表
        public IEnumerable<TemplateModel> GetPrePaymentdeductionDetetailReportTemplateModels()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "DoorNoResourcesName", ColumnDesc = "资源名称", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "OwnerName", ColumnDesc = "业主姓名", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "ChargeSubjectName", ColumnDesc = "收费项目", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "ChargeType", ColumnDesc = "收费类型", Seq = i++,IsExport=true,DictId=PropertyEnumType.ChargeType.ToString()},
                new TemplateColumn(){ ColumnName = "BeginDateFormat", ColumnDesc = "账单开始", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "EndDateFormat", ColumnDesc = "账单结束", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "BillAmount", ColumnDesc = "账单金额", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "ArrearsAmount", ColumnDesc = "收费金额", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "PayDateFormat", ColumnDesc = "收费日期", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "Number", ColumnDesc = "票据号", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "OperatorName", ColumnDesc = "操作人", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++,IsExport=true,DictId="-1"}
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(PrePaymentdeductionDetailReportDTO), showColumns);
            return template;
        }

        #endregion

        #region 欠费明细报表
        public IEnumerable<TemplateModel> GetArrearsDetailReportDataList()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "ResourcesName", ColumnDesc = "资源名称", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "OwnerName", ColumnDesc = "业主姓名", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "ChargeSubjectName", ColumnDesc = "收费项目", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "BeginDateStr", ColumnDesc = "账单开始", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "EndDateStr", ColumnDesc = "账单结束", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "BillAmount", ColumnDesc = "账单金额", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "ArrearsAmount", ColumnDesc = "欠费金额", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "CreateTimeStr", ColumnDesc = "生成时间", Seq = i++,IsExport=true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++,IsExport=true,DictId="-1"}
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ReportArrearsDetailDTO), showColumns);
            return template;
        }

        #endregion

        #region 综合报表---2.8版本
        public IEnumerable<TemplateModel> GetIntegratedReportChargeSubjectTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "ChargeSubjectName", ColumnDesc = "收费项目", Seq = i++, IsExport = true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "TotalRecAmount", ColumnDesc = "应收金额", Seq = i++,IsExport = true, DictId="-1"},
                new TemplateColumn(){ ColumnName = "RececiveTotal", ColumnDesc = "实收金额", Seq = i++, IsExport = true,DictId="-1"},
                new TemplateColumn(){ ColumnName = "UnPaidAmountTotal", ColumnDesc = "欠款金额", Seq = i++, IsExport = true, DictId="-1"},
                new TemplateColumn(){ ColumnName = "PayRate", ColumnDesc = "收费率", Seq = i++, IsExport = true, DictId="-1"}
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ReportTableDTO), showColumns);
            return template;
        }


        #endregion


    }
}
