using System;
using System.Collections.Generic;
using System.Linq;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using YK.PropertyMgr.CompositeAppService;
using YK.BackgroundMgr.DomainInterface;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.DomainService;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.Crosscuting;
using iTextSharp.text;
using Microsoft.Practices.Unity;
using YK.PropertyMgr.ApplicationService.Service;
using YK.PropertyMgr.ApplicationDTO.Resources;

namespace YK.PropertyMgr.ApplicationService
{
    public partial class ChargBillAppService
    {

        #region 获取账单对象非DTO
        public ChargBill GetChargBillById(object Id)
        {
            ChargBillDomainService _ChargBillDomainService = new ChargBillDomainService();
            return _ChargBillDomainService.GetChargBillByKey(Id);
        }

        #endregion

        #region 判断科目是否有账单
        /// <summary>
        /// 判断科目是否有账单
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<ChargBillDTO> GetChargBillDTOList(int subjectId, out int totalCount)
        {
            string expressions = "CreateTime desc";
            Condition<ChargBill> condition = new Condition<ChargBill>(c => c.ChargeSubjectId == subjectId);
            var list = ChargBillService.GetChargBillList(condition.ExpressionBody, expressions, out totalCount);
            return ChargBillMappers.ChangeChargBillToDTOs(list).ToList();

        }
        public IList<ChargBillDTO> GetChargBillDTOList(List<int?> subjectIds, out int totalCount)
        {
            string expressions = "CreateTime desc";
            Condition<ChargBill> condition = new Condition<ChargBill>(c => subjectIds.Contains(c.ChargeSubjectId));
            var list = ChargBillService.GetChargBillList(condition.ExpressionBody, expressions, out totalCount);
            return ChargBillMappers.ChangeChargBillToDTOs(list).ToList();

        }
        #endregion

        #region 根据科目、资源获取账单
        /// <summary>
        /// 根据科目、资源获取账单
        /// </summary>
        /// <param name="subjectId">科目</param>
        /// <param name="resType">资源类型</param>
        /// <param name="resIds">资源ID</param>
        /// <returns></returns>
        public IList<ChargBillDTO> GetChargBillDTOList(int subjectId, int resType, List<int?> resIds)
        {
            int totalCount = 0;
            string expressions = "CreateTime desc";
            Condition<ChargBill> condition = new Condition<ChargBill>(c => c.ChargeSubjectId == subjectId && c.RefType == resType && resIds.Contains(c.ResourcesId) && c.IsDel == false);
            var list = ChargBillService.GetChargBillList(condition.ExpressionBody, expressions, out totalCount);
            return ChargBillMappers.ChangeChargBillToDTOs(list).ToList();

        }
        public IList<ChargBillDTO> GetChargBillDTOList(List<int?> subjectIds, int resType, List<int?> resIds)
        {
            int totalCount = 0;
            string expressions = "CreateTime desc";
            Condition<ChargBill> condition = new Condition<ChargBill>(c => subjectIds.Contains(c.ChargeSubjectId) && c.RefType == resType && resIds.Contains(c.ResourcesId) && c.IsDel == false);
            var list = ChargBillService.GetChargBillList(condition.ExpressionBody, expressions, out totalCount);
            return ChargBillMappers.ChangeChargBillToDTOs(list).ToList();

        }
        #endregion

        #region 日常收费查询页面数据
        /// <summary>
        /// 查询日常收费界面Model数据
        /// </summary>
        public ChargeBillListData GetDailyChargList(int? DeptId, EDeptType? DeptType, string DeptName, bool IsCarPark, string Language)
        {
            DeptId = DeptId ?? 0;
            DeptType = DeptType ?? 0;
            ChargBillSearchDTO search = new ChargBillSearchDTO();
            search.DeptId = DeptId.Value;
            search.DeptType = DeptType;
            search.DeptName = DeptName;
            search.IsCarPark = IsCarPark;
            search.IsDevPay = false;
            ChargeBillListData cbData = GetDailyChargList(search);
            cbData.Language = Language;
            cbData.PayTypeList = GetPayTypeList().ToList();
            cbData.ChargBillSearch = search;
            cbData.IsCarPark = IsCarPark;
            cbData.PageModel.SmallToPrepaySubjectId = 0;
            return cbData;
        }

        /// <summary>
        /// 跟进搜索添加查询日常收费界面Model数据
        /// </summary>
        public ChargeBillListData GetDailyChargList(ChargBillSearchDTO search)
        {
            ChargeBillListData billData = new ChargeBillListData();
            int outCount = 0;
            billData.ChargeBillList = GetChargBillDTOList(search, out outCount);
            billData.PageModel = new DailyChargBillDTO() { Remark = StaticResourceHelper.ReceiptRemark };
            billData.PageModel.AmountShouldTotal = billData.ChargeBillList.Where(o => o.RowType != RowTypeEnum.FatherRow).Sum(c => c.AmountShould);
            billData.PageModel.AmountShouldAllTotal = billData.PageModel.AmountShouldTotal;
            billData.PageModel.PayTypeId = PayTypeEnum.Cash.GetHashCode();
            billData.PageModel.IsSmallToPrepay = true;//!search.IsCarPark;
            ChargeSubjectAppService subjService = new ChargeSubjectAppService();
            billData.IsChargeSubject = subjService.CheckChargeSubject(search.DeptId, (int)search.DeptType);
            billData.ChargeSubjectList = GetHouseSubjectList(search.DeptId, search.IsCarPark, search.DeptType, true).ToList();
            if (search.DeptType == EDeptType.FangWu)
            {
                CommunityConfigAppService service = new CommunityConfigAppService();
                billData.ComConfig = service.GetCommunityConfigByResourceDeptId(search.DeptId, 1);
                ChargeRecordAppService recordAppService = new ChargeRecordAppService();
                billData.PageModel.PreAmountInfo = recordAppService.GetBalanceAmountByHouseDeptId(search.DeptId, "：");
                //billData.PageModel.PreDeductibleAmount = 5;
                PrepayAccountDomainService preService = new PrepayAccountDomainService();
                billData.PreAccountList = preService.GetailyChargPrepayAccountList(search.DeptId);
            }
            //实收金额 = 应收金额 - 预存抵扣金额
            billData.PageModel.ReceivedAmountTotal = Math.Ceiling(billData.PageModel.AmountShouldTotal - billData.PageModel.PreDeductibleAmount);
            //找零 = 实收金额 + 预存抵扣金额 - 应收金额
            billData.PageModel.SmallChange = (billData.PageModel.ReceivedAmountTotal + billData.PageModel.PreDeductibleAmount - billData.PageModel.AmountShouldTotal).ToString("0.##");
            return billData;
        }

        /// <summary>
        /// 根据条件获取账单
        /// </summary>
        public IList<ChargBillDTO> GetChargBillDTOList(ChargBillSearchDTO searchDto, out int totalCount)
        {
            List<EDeptType?> DeptTypelist = new List<EDeptType?>() { EDeptType.CheWei, EDeptType.FangWu };

            if (!DeptTypelist.Contains(searchDto.DeptType))
            {
                totalCount = 0;
                return new List<ChargBillDTO>();
            }
            Condition<ChargBill> condition = new Condition<ChargBill>(c => true);
            int RecourceType = 1;
            switch (searchDto.DeptType)
            {
                case EDeptType.FangWu:
                    RecourceType = (int)ReourceTypeEnum.House;
                    condition = condition & new Condition<ChargBill>(c => c.HouseDeptId == searchDto.DeptId && c.Status == (int)BillStatusEnum.NoPayment && (c.ChargeSubject.BillPeriod != (int)BillPeriodEnum.Once || (c.ChargeSubject.BillPeriod == (int)BillPeriodEnum.Once && c.ChargeSubject.SubjectType == (int)SubjectTypeEnum.SystemPreset)));
                    break;

                case EDeptType.CheWei:
                    RecourceType = (int)ReourceTypeEnum.CarPark;
                    condition = condition & new Condition<ChargBill>(c => c.ResourcesId == searchDto.DeptId && c.RefType == RecourceType && c.Status == (int)BillStatusEnum.NoPayment && (c.ChargeSubject.BillPeriod != (int)BillPeriodEnum.Once || (c.ChargeSubject.BillPeriod == (int)BillPeriodEnum.Once && c.ChargeSubject.SubjectType == (int)SubjectTypeEnum.SystemPreset)));
                    break;
            }

            //修改bug #2311 默认不显示开发商代缴 2016-10-11
            //if (searchDto.IsDevPay) 
            //{
            //   condition = condition & new Condition<ChargBill>(c => c.IsDevPay == searchDto.IsDevPay);
            //}
            condition = condition & new Condition<ChargBill>(c => c.IsDevPay == searchDto.IsDevPay && c.IsDel == false);

            string expressions = "BeginDate";
            var domainList = ChargBillService.GetChargBillList(condition.ExpressionBody, expressions, out totalCount);
            var dtoList = ChargBillMappers.ChangeChargBillToDTOs(domainList).ToList();
            foreach (var item in dtoList)
            {
                item.ChargeSubjectName = domainList.Where(d => d.Id == item.Id).First().ChargeSubject.Name;
                item.HouseDoorNo = searchDto.DeptName;
                item.ResourcesName = searchDto.DeptName;
                item.IsChecked = true;
            }
            //v2.9 如果是房屋 计算预存抵扣金额
            //if (searchDto.DeptType == EDeptType.FangWu)
            //{
            //    SetBillPreAmount(dtoList, searchDto.DeptId);
            //}
            //分组计算
            return ChargBillDTOListGroup(dtoList);
        }

        public ChargBillDTO GetEasyDailyChargList(int? ResourcesId, int? ChargeSubjecId,int? resType,int? CarHouseDeptId)
        {
            ChargBillDTO ChargBill = new ChargBillDTO();
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var list = propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(o => o.ResourcesId == ResourcesId && o.ChargeSubjectId == ChargeSubjecId).Where(o => o.IsDel == false).OrderByDescending(o => o.EndDate).FirstOrDefault();
                return ChargBillMappers.ChangeChargBillToDTO(list);
            }
        }

        public IEnumerable<DictionaryModel> GetPayTypeList()
        {
            var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
            return propertyService.GetDictionaryModels(PropertyEnumType.PayType.ToString())
                .Where(o => o.Code != PayTypeEnum.Wallet.GetHashCode().ToString()
                && o.Code != PayTypeEnum.InternalTransfer.GetHashCode().ToString()
                && o.Code != PayTypeEnum.OneNetcom.GetHashCode().ToString()
                );
        }

        public IEnumerable<TemplateModel> GetChargBillViewTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {

                new TemplateColumn(){ ColumnName = "ResourcesName", ColumnDesc = "收费对象", Seq = i++},
                new TemplateColumn(){ ColumnName = "ChargeSubjectName", ColumnDesc = "收费项目", Seq = i++},
                new TemplateColumn(){ ColumnName = "BillAmount", ColumnDesc = "计费金额", Seq = i++},
                new TemplateColumn(){ ColumnName = "ReceivedAmount", ColumnDesc = "已交金额", Seq = i++},
                new TemplateColumn(){ ColumnName = "AmountShould", ColumnDesc = "应收金额", Seq = i++},
                new TemplateColumn(){ ColumnName = "BeginDate", ColumnDesc = "开始日期", Seq = i++, Type = "date"},
                new TemplateColumn(){ ColumnName = "EndDate", ColumnDesc = "结束日期", Seq = i++, Type = "date"},
                new TemplateColumn(){ ColumnName = "ReliefAmount", ColumnDesc = "减免金额", Seq = i++},
                new TemplateColumn(){ ColumnName = "Description", ColumnDesc = "描述", Seq = i++}

            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ChargBillDTO), showColumns);
            return template;
        }

        public IEnumerable<TemplateModel> GetDeveloperChargBillViewTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                 new TemplateColumn(){ ColumnName = "DeptName", ColumnDesc = "小区", Seq = i++},
                new TemplateColumn(){ ColumnName = "ResourcesName", ColumnDesc = "收费对象", Seq = i++},
                new TemplateColumn(){ ColumnName = "ChargeSubjectName", ColumnDesc = "收费项目", Seq = i++},
                new TemplateColumn(){ ColumnName = "BillAmount", ColumnDesc = "计费金额", Seq = i++},
                new TemplateColumn(){ ColumnName = "ReceivedAmount", ColumnDesc = "已交金额", Seq = i++},
                new TemplateColumn(){ ColumnName = "AmountShould", ColumnDesc = "应收金额", Seq = i++},
                new TemplateColumn(){ ColumnName = "BeginDate", ColumnDesc = "开始日期", Seq = i++, Type = "date"},
                new TemplateColumn(){ ColumnName = "EndDate", ColumnDesc = "结束日期", Seq = i++, Type = "date"},
                new TemplateColumn(){ ColumnName = "ReliefAmount", ColumnDesc = "减免金额", Seq = i++},
                new TemplateColumn(){ ColumnName = "Description", ColumnDesc = "描述", Seq = i++}

            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ChargBillDTO), showColumns);
            return template;
        }

        #endregion

        #region 账单拆分

        public IEnumerable<TemplateModel> GetSplitBillViewTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "HouseDoorNo", ColumnDesc = "资源", Seq = i++, ElementType = "TextBox"},
                new TemplateColumn(){ ColumnName = "Description", ColumnDesc = "收费项目", Seq = i++, ElementType = "TextBox"},
                new TemplateColumn(){ ColumnName = "BeginDate", ColumnDesc = "开始日期", Seq = i++,ElementType = "TextBox", Type = "date"},
                new TemplateColumn(){ ColumnName = "SplitDate", ColumnDesc = "结束日期", Seq = i++, Type = "date", IsRequred = true},
                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++, ElementType="TextArea",MaxLength = 300, ValidateType="stringLength" }

            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ChargBillDTO), showColumns);
            return template;
        }

        #endregion

        #region 账单删除
        public IEnumerable<TemplateModel> GetDeleteBillViewTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "HouseDoorNo", ColumnDesc = "资源", Seq = i++, ElementType = "TextBox"},
                new TemplateColumn(){ ColumnName = "Description", ColumnDesc = "收费项目", Seq = i++, ElementType = "TextBox"},
                new TemplateColumn(){ ColumnName = "BeginDateFormat", ColumnDesc = "开始日期", Seq = i++,ElementType = "TextBox"},
                new TemplateColumn(){ ColumnName = "EndDateFormat", ColumnDesc = "结束日期", Seq = i++,ElementType = "TextBox"},
                new TemplateColumn(){ ColumnName = "BillAmount", ColumnDesc = "金额", Seq = i++, ElementType = "TextBox" },

                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++, ElementType="TextArea",MaxLength = 300, IsRequred = true,ValidateType="stringLength" }

            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ChargBillDTO), showColumns);
            return template;
        }

        #endregion

        #region 获取房屋收费项目

        public IList<ChargeSubjectDTO> GetHouseSubjectList(int deptId, bool IsCarPark, EDeptType? DeptType, bool addAll)
        {
            if (DeptType != EDeptType.FangWu && DeptType != EDeptType.CheWei)
            {
                var dataList = new List<ChargeSubjectDTO>();
                dataList.Add(new ChargeSubjectDTO() { Id = 0, Name = "全部收费项目" });
                return dataList;
            }
            if (IsCarPark)
            {
                deptId = PresentationServiceHelper.LookUp<IPropertyService>().GetHoseDeptIdByParkingSpaceId(deptId);
                if (deptId == 0)
                {
                    var dataList = new List<ChargeSubjectDTO>();
                    dataList.Add(new ChargeSubjectDTO() { Id = 0, Name = "全部收费项目" });
                    return dataList;
                }
            }
            var subjectList = ChargBillService.GetHouseSubjectList(deptId);
            if (addAll)
            {
                subjectList.Insert(0, new ChargeSubject() { Id = 0, Name = "全部收费项目" });
            }
            return ChargeSubjectMappers.ChangeChargeSubjectToDTOs(subjectList).ToList();
        }

        #endregion

        #region 预付款

        public IEnumerable<TemplateModel> GetPrepareAmountViewTemplate(bool isDepPay)
        {
            int i = 1;
            if (isDepPay)
            {
                TemplateColumn[] dshowColumns = new TemplateColumn[]
                {
                    new TemplateColumn(){ ColumnName = "HouseDoorNo", ColumnDesc = "资源", Seq = i++, ElementType = "TextBox"},
                    new TemplateColumn(){ ColumnName = "BillAmount", ColumnDesc = "金额", Seq = i++, ElementType = "TextBox"
                    , IsRequred = true, ValidateType = "double", IsAttr=true, Attrs = new Attr[] { new Attr("ng-change", "BillAmountChange()") } },
                    new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++, ElementType="TextArea",MaxLength = 300, ValidateType="stringLength" }

                };
                IEnumerable<TemplateModel> dtemplate = TemplateModelHelper.GetTemplateModels(typeof(ChargBillDTO), dshowColumns);
                return dtemplate;
            }

            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "HouseDoorNo", ColumnDesc = "资源", Seq = i++, ElementType = "TextBox"},
                new TemplateColumn(){ ColumnName = "Months", ColumnDesc = "预交月数", Seq = i++, ElementType = "TextBox"
                , IsRequred = false, ValidateType = "double", IsAttr=true, Attrs = new Attr[] { new Attr("ng-change", "MonthsChange()") } },
                new TemplateColumn(){ ColumnName = "BeginDateFormat", ColumnDesc = "开始日期", Seq = i++, ElementType = "TextBox" },
                new TemplateColumn(){ ColumnName = "EndDateFormat", ColumnDesc = "结束日期", Seq = i++, ElementType = "TextBox" },
                new TemplateColumn(){ ColumnName = "BillAmount", ColumnDesc = "金额", Seq = i++, ElementType = "TextBox"
                , IsRequred = true, ValidateType = "double", IsAttr=true, Attrs = new Attr[] { new Attr("ng-change", "BillAmountChange()") } },
                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++, ElementType="TextArea",MaxLength = 300, ValidateType="stringLength" }

            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ChargBillDTO), showColumns);
            return template;
        }

        public IEnumerable<TemplateModel> GetPrepareAmountViewTemplate(int houseDeptId)
        {
            int i = 1;

            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "HouseDoorNo", ColumnDesc = "资源", Seq = i++, ElementType = "TextBox"},
                 new TemplateColumn(){ ColumnName = "PreChargeSubjectId", ColumnDesc = "预存收费项目", Seq = i++
                 , IsAttr=true, Attrs = new Attr[] { new Attr("ng-change", "SubjectChange()") } },
                new TemplateColumn(){ ColumnName = "Months", ColumnDesc = "预交月数", Seq = i++, ElementType = "TextBox"
                , IsRequred = false, ValidateType = "double", IsAttr=true, Attrs = new Attr[] { new Attr("ng-change", "MonthsChange()") } },
                new TemplateColumn(){ ColumnName = "BillAmount", ColumnDesc = "金额", Seq = i++, ElementType = "TextBox"
                , IsRequred = true, ValidateType = "double", IsAttr=true, Attrs = new Attr[] { new Attr("ng-change", "BillAmountChange()") } },
                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++, ElementType="TextArea",MaxLength = 300, ValidateType="stringLength" }

            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ChargBillDTO), showColumns);
            var model = template.Where(t => t.EnName == "PreChargeSubjectId").FirstOrDefault();
            if (model != null)
            {
                model.DictId = "0";
                model.Type = "dict";
                model.ElementType = "Select";
                var DictionaryModels = new List<DictionaryModel>();
                //DictionaryModels.Add(new DictionaryModel() { Code = "0" , CnName="全部收费项目"});
                var sublist = GetHouseSubjectList(houseDeptId, false, EDeptType.FangWu, true).Select(h => new DictionaryModel() { Code = h.Id.ToString(), CnName = h.Name }).ToList();
                DictionaryModels.AddRange(sublist);
                model.DictionaryModels = DictionaryModels;
            }

            return template;
        }

        #endregion

        #region 临时收费
        /// <summary>
        /// 临时收费
        /// </summary>
        /// <param name="searchDto"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<ChargBillDTO> GetTempChargBillDTOList(ChargBillSearchDTO searchDto, out int totalCount)
        {
            Condition<ChargBill> condition = new Condition<ChargBill>(c => c.HouseDeptId == searchDto.DeptId && c.Status == (int)BillStatusEnum.NoPayment);
            if (searchDto.IsDevPay)
            {
                condition = condition & new Condition<ChargBill>(c => c.IsDevPay == searchDto.IsDevPay);
            }
            string expressions = "CreateTime desc";
            var domainList = ChargBillService.GetChargBillList(condition.ExpressionBody, expressions, out totalCount);
            var dtoList = ChargBillMappers.ChangeChargBillToDTOs(domainList);
            foreach (var item in dtoList)
            {
                item.ChargeSubjectName = domainList.Where(d => d.Id == item.Id).First().ChargeSubject.Name;
                item.SubjectType = domainList.Where(d => d.Id == item.Id).First().ChargeSubject.SubjectType;
                item.IsChecked = true;
                item.HouseDoorNo = searchDto.DeptName;
                item.BillPeriod = domainList.Where(d => d.Id == item.Id).First().ChargeSubject.BillPeriod;
                ChargBill chargbill = domainList.Where(d => d.Id == item.Id).First();
                //item.AmountShould = chargbill.BillAmount.Value - chargbill.ReceivedAmount.Value - chargbill.ReliefAmount.Value;
            }

            totalCount = dtoList.Where(o => o.BillPeriod == (int)BillPeriodEnum.Once && o.SubjectType != (int)SubjectTypeEnum.SystemPreset).Count();
            return dtoList.Where(o => o.BillPeriod == (int)BillPeriodEnum.Once && o.SubjectType != (int)SubjectTypeEnum.SystemPreset).ToList();
        }
        #endregion

        #region 微信 平板 App接口

        public IEnumerable<AppChargBillView> GetChargBillListByHouseDeptId(int houseDeptId, DateTime beginDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1).AddSeconds(-1);
            Condition<ChargBill> condition = new Condition<ChargBill>(c => c.HouseDeptId == houseDeptId
                && c.Status == (int)BillStatusEnum.NoPayment//未付款
                && c.IsDel == false
                && c.IsDevPay == false //非开发商
                && c.ChargeSubject.IsOnline == true //支持在线缴费
                && c.BeginDate >= beginDate
                && c.EndDate <= endDate
                && c.ChargeSubject.BillPeriod != (int)BillPeriodEnum.Once //排除一次性收费
                );

            var billdata = ChargBillService.GetChargBillAll(condition.ExpressionBody).ToList();
            if (billdata.Count() > 0)
            {
                return billdata.Select(s => new AppChargBillView()
                {
                    Id = s.Id,
                    Description = s.Description,
                    BeginDate = s.BeginDate.HasValue ? s.BeginDate.Value.ToString("yyyy/MM/dd") : null,
                    EndDate = s.EndDate.HasValue ? s.EndDate.Value.ToString("yyyy/MM/dd") : null,
                    //应收金额=计费金额+滞纳金-已交金额-减免金额
                    Amount = Math.Round(s.BillAmount.Value + s.PenaltyAmount.Value - s.ReceivedAmount.Value - s.ReliefAmount.Value, 2)
                });
            }
            return new List<AppChargBillView>();
        }

        public IEnumerable<ChargBillDTO> GetChargBillListByHouseDeptId(int houseDeptId)
        {
            Condition<ChargBill> condition = new Condition<ChargBill>(c => c.HouseDeptId == houseDeptId
                && c.Status == (int)BillStatusEnum.NoPayment//未付款
                && c.IsDel == false
                && c.IsDevPay == false //非开发商
                && c.ChargeSubject.IsOnline == true //支持在线缴费
                && c.ChargeSubject.BillPeriod != (int)BillPeriodEnum.Once //排除一次性收费
                );

            var billdata = ChargBillService.GetChargBillAll(condition.ExpressionBody).OrderBy(c => c.BeginDate).ToList();
            return ChargBillMappers.ChangeChargBillToDTOs(billdata);
        }

        public IEnumerable<AppOldBillView> GetChargBillListByHouseDeptId(int houseDeptId, BillStatusEnum state, int subjectId, DateTime? beginDate, DateTime? endDate)
        {
            Condition<ChargBill> condition = new Condition<ChargBill>(c => c.HouseDeptId == houseDeptId
               && c.Status == (int)state
               && c.IsDel == false
               && (subjectId == 0 || c.ChargeSubjectId == subjectId)
               && c.IsDevPay == false //非开发商
                                      //&& c.ChargeSubject.IsOnline == true //支持在线缴费
                                      //&& c.BeginDate >= beginDate
                                      //&& c.EndDate <= endDate
                                      //&& c.ChargeSubject.BillPeriod != (int)BillPeriodEnum.Once //排除一次性收费
               );
            //如果是未付款 只查出支持在线缴费的
            if (state == BillStatusEnum.NoPayment)
            {
                condition = condition & new Condition<ChargBill>(c => c.ChargeSubject.IsOnline == true);
            }

            if (beginDate.HasValue)
            {
                DateTime bd = beginDate.Value;
                condition = condition & new Condition<ChargBill>(c => c.BeginDate >= bd);
            }

            if (endDate.HasValue)
            {
                DateTime ed = endDate.Value.AddDays(1).AddSeconds(-1);
                condition = condition & new Condition<ChargBill>(c => c.EndDate <= ed);
            }
            var billdata = ChargBillService.GetChargBillAll(condition.ExpressionBody).OrderByDescending(s => s.UpdateTime).ToList();
            if (billdata.Count() > 0)
            {
                return billdata.Select(s => new AppOldBillView()
                {
                    ChargePlanId = s.Id,
                    BeginTime = s.BeginDate.HasValue ? s.BeginDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                    EndTime = s.EndDate.HasValue ? s.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                    //应收金额=计费金额+滞纳金-已交金额-减免金额
                    Money = (state == BillStatusEnum.Paid ? s.ReceivedAmount : Math.Round(s.BillAmount.Value + s.PenaltyAmount.Value - s.ReceivedAmount.Value - s.ReliefAmount.Value, 2)),
                    ReceivableDate = s.UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    Remark = s.Remark,
                    Subject_Id = s.ChargeSubjectId.Value,
                    SubjectName = s.ChargeSubject.Name
                });
            }
            return new List<AppOldBillView>();
        }

        public IEnumerable<AppOldBillView> GetBillListByIds(string[] BillIds)
        {
            Condition<ChargBill> condition = new Condition<ChargBill>(c => c.IsDel == false
               && BillIds.Contains(c.Id)
               && c.IsDevPay == false //非开发商
                                      //&& c.ChargeSubject.IsOnline == true //支持在线缴费
                                      //&& c.ChargeSubject.BillPeriod != (int)BillPeriodEnum.Once //排除一次性收费
               );
            var billdata = ChargBillService.GetChargBillAll(condition.ExpressionBody).OrderByDescending(s => s.UpdateTime).ToList();
            if (billdata.Count() > 0)
            {
                return billdata.Select(s => new AppOldBillView()
                {
                    ChargePlanId = s.Id,
                    BeginTime = s.BeginDate.HasValue ? s.BeginDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                    EndTime = s.EndDate.HasValue ? s.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                    //应收金额=计费金额+滞纳金-已交金额-减免金额
                    //Money = Math.Round(s.BillAmount.Value + s.PenaltyAmount.Value - s.ReceivedAmount.Value - s.ReliefAmount.Value, 2),
                    Money = (s.Status == (int)BillStatusEnum.Paid ? s.ReceivedAmount : Math.Round(s.BillAmount.Value + s.PenaltyAmount.Value - s.ReceivedAmount.Value - s.ReliefAmount.Value, 2)),
                    ReceivableDate = s.UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    Remark = s.Remark,
                    Subject_Id = s.ChargeSubjectId.Value,
                    SubjectName = s.ChargeSubject.Name
                });
            }
            return new List<AppOldBillView>();
        }

        #endregion

        #region 日常收费保存功能

        public ResultModel PayChargBill(DailyChargBillDTO SaveModel, int Operator, string OperatorName)
        {
            if (SaveModel.ReceivedAmountTotal == 0 && !SaveModel.IsPreDeductible)
            {
                return new ResultModel() { IsSuccess = false, Msg = "请输入缴费金额" };
            }

            if (SaveModel.ReceivedAmountTotal == 0 && SaveModel.IsPreDeductible && SaveModel.PreDeductibleAmount == 0)
            {
                return new ResultModel() { IsSuccess = false, Msg = "请输入缴费金额" };
            }

            if ((SaveModel.BillIds == null || SaveModel.BillIds.Length < 1)
                && (SaveModel.NewBillList == null || SaveModel.NewBillList.Count() < 1))
            {
                return new ResultModel() { IsSuccess = false, Msg = "请选择账单" };
            }
            if (SaveModel.Remark.Length > 35)
            {
                return new ResultModel() { IsSuccess = false, Msg = "备注长度不能超过35个字符" };
            }

            return PaymentAppService.BillsDailyPayment(SaveModel, Operator, OperatorName);

            //IList<ChargBill> newList = new List<ChargBill>();
            //IList<ChargBill> updateList = new List<ChargBill>();
            //List<ChargBill> onlyUpdateList = new List<ChargBill>();
            //List<ChargBill> onlyNewList = new List<ChargBill>();
            //if (SaveModel.NewBillList != null)
            //{
            //    foreach (var item in SaveModel.NewBillList)
            //    {
            //        item.CreateTime = DateTime.Now;
            //        if (item.ActionStatus == ActionStatusEnum.New)
            //        {
            //            if (item.IsChecked)
            //            {
            //                newList.Add(ChargBillMappers.ChangeDTOToChargBillNew(item));
            //            }
            //            else
            //            {
            //                onlyNewList.Add(ChargBillMappers.ChangeDTOToChargBillNew(item));
            //            }
            //        }
            //        else if (item.ActionStatus == ActionStatusEnum.Update)
            //        {
            //            if (item.IsChecked)
            //            {
            //                updateList.Add(ChargBillMappers.ChangeDTOToChargBillNew(item));
            //            }
            //            else
            //            {
            //                onlyUpdateList.Add(ChargBillMappers.ChangeDTOToChargBillNew(item));
            //            }
            //        }
            //    }
            //}

            //ResultModel returnModel = PaymentAppService.BillsDailyPayment(SaveModel.BillIds, newList
            //    , updateList, onlyUpdateList, onlyNewList, SaveModel.ReceivedAmountTotal
            //    , true, (PayTypeEnum)SaveModel.PayTypeId, SaveModel.IsSmallToPrepay, Operator
            //    , OperatorName, SaveModel.Remark, SaveModel.SmallToPrepaySubjectId);

            //return returnModel;
        }

        #endregion

        #region 临时收费
        public ResultModel PayBillsTemporary(DailyChargBillDTO SaveModel, int Operator, string OperatorName)
        {
            IList<ChargBill> billList = new List<ChargBill>();
            if (SaveModel.NewBillList != null)
            {
                foreach (var item in SaveModel.NewBillList)
                {
                    item.CreateTime = DateTime.Now;
                    if (item.ActionStatus == ActionStatusEnum.New)
                    {
                        billList.Add(ChargBillMappers.ChangeDTOToChargBillNew(item));
                    }
                }
            }

            ResultModel returnModel = PaymentAppService.BillsTemporaryPayment(SaveModel.BillIds, billList, SaveModel.ReceivedAmountTotal
            , true, (PayTypeEnum)SaveModel.PayTypeId, SaveModel.IsSmallToPrepay, Operator, OperatorName, SaveModel.Remark, SaveModel.SmallToPrepaySubjectId);

            return returnModel;
        }

        #endregion

        #region 对外收费
        public ResultModel BillsForeigBillPayment(DailyChargBillDTO SaveModel, int Operator, string OperatorName)
        {
            SaveModel.IsSmallToPrepay = false;//不存入预存费
            IList<ChargBill> billList = new List<ChargBill>();
            if (SaveModel.NewBillList != null)
            {
                foreach (var item in SaveModel.NewBillList)
                {
                    item.CreateTime = DateTime.Now;
                    if (item.ActionStatus == ActionStatusEnum.New)
                    {
                        billList.Add(ChargBillMappers.ChangeDTOToChargBillNew(item));
                    }
                }
            }

            ResultModel returnModel = PaymentAppService.BillsForeigBillPayment(SaveModel.BillIds, billList, SaveModel.ReceivedAmountTotal
            , true, (PayTypeEnum)SaveModel.PayTypeId, SaveModel.IsSmallToPrepay, Operator, OperatorName, SaveModel.Remark, SaveModel.CustomerName);

            return returnModel;
        }


        #endregion


        #region 生成临时收费账单
        /// <summary>
        /// 生成临时收费账单
        /// </summary>
        /// <param name="ComDeptId">小区ID</param>
        /// <param name="HouseDeptId">房屋ID</param>
        /// <param name="ResourcesId">资源ID</param>
        /// <param name="ChargeSubjectId">科目ID</param>
        /// <param name="BeginDateTime">开始时间</param>
        /// <param name="EndDateTime">结束时间</param>
        /// <param name="Amount">金额</param>
        /// <param name="Remark">备注</param>
        /// <returns></returns>
        public ResultModel GenerateTemporaryBill(int ComDeptId, int HouseDeptId, int ChargeSubjectId, DateTime? BeginDateTime, DateTime? EndDateTime, decimal Amount, string Remark)
        {
            int ResourcesId = 0;
            //TODO: 资源ID暂时未房屋ID
            ResourcesId = HouseDeptId;

            ChargBillDTO bill = GenerateBillAppService.GenerateTemporaryBill(ComDeptId, HouseDeptId, ResourcesId, ChargeSubjectId, BeginDateTime, EndDateTime, Amount, Remark);
            if (null != bill)
            {
                return new ResultModel() { IsSuccess = true, Msg = "处理成功", Data = bill };
            }
            else
            {
                return new ResultModel() { IsSuccess = false, Msg = "处理失败" };
            }
        }

        public ResultModel GenerateTemporaryBill(int ComDeptId, int HouseDeptId, int ResourcesId, int ChargeSubjectId, DateTime? BeginDateTime, DateTime? EndDateTime, decimal Amount, string Remark, int ReourceType = (int)ReourceTypeEnum.House)
        {
            ChargBillDTO bill = GenerateBillAppService.GenerateTemporaryBill(ComDeptId, HouseDeptId, ResourcesId, ChargeSubjectId, BeginDateTime, EndDateTime, Amount, Remark, ReourceType);
            if (null != bill)
            {
                return new ResultModel() { IsSuccess = true, Msg = "处理成功", Data = bill };
            }
            else
            {
                return new ResultModel() { IsSuccess = false, Msg = "处理失败" };
            }
        }
        #endregion

        #region 生成对外收费账单
        /// <summary>
        /// 生成临时收费账单
        /// </summary>
        /// <param name="ComDeptId">小区ID</param>
        /// <param name="HouseDeptId">房屋ID</param>
        /// <param name="ResourcesId">资源ID</param>
        /// <param name="ChargeSubjectId">科目ID</param>
        /// <param name="BeginDateTime">开始时间</param>
        /// <param name="EndDateTime">结束时间</param>
        /// <param name="Amount">金额</param>
        /// <param name="Remark">备注</param>
        /// <param name="CustomerName">收费对象</param>
        /// <returns></returns>
        public ResultModel GenerateForeigChargeBill(int ComDeptId, int ChargeSubjectId, DateTime? BeginDateTime, DateTime? EndDateTime, decimal Amount, string Remark, string CustomerName)
        {
            int ResourcesId = 0;
            //TODO: 资源ID暂时未房屋ID
            ResourcesId = ComDeptId;

            ChargBillDTO bill = GenerateBillAppService.GenerateTemporaryBill(ComDeptId, 0, ResourcesId, ChargeSubjectId, BeginDateTime, EndDateTime, Amount, Remark);
            bill.CustomerName = CustomerName;
            if (null != bill)
            {
                return new ResultModel() { IsSuccess = true, Msg = "处理成功", Data = bill };
            }
            else
            {
                return new ResultModel() { IsSuccess = false, Msg = "处理失败" };
            }
        }
        #endregion

        #region 开发商缴费

        public IList<ChargBillDTO> GetDeveloperChargBillDTOList(ChargBillSearchDTO searchDto, out int totalCount)
        {

            if (searchDto.DeptType != EDeptType.XiaoQu)
            {
                totalCount = 0;
                return new List<ChargBillDTO>();
            }
            Condition<ChargBill> condition = new Condition<ChargBill>(c => c.ComDeptId == searchDto.DeptId && c.Status == (int)BillStatusEnum.NoPayment && c.IsDevPay == true && c.IsDel == false);

            string expressions = "BeginDate";
            var domainList = ChargBillService.GetChargBillList(condition.ExpressionBody, expressions, out totalCount);
            //var limitHouseDeptInfos = DomainInterfaceHelper
            //    .LookUp<IPropertyDomainService>().GetHouDeptListByCommunityDeptId(searchDto.DeptId.ToString());
            //获取该小区房屋

            //var limitCarPortInfoList = new List<Carport>();

            //if (domainList.Any(o => o.RefType == (int)ReourceTypeEnum.CarPark))
            //{
            //     limitCarPortInfoList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCarPortListByComDeptId(searchDto.DeptId);
            //}





            var dtoList = ChargBillMappers.ChangeChargBillToDTOs(domainList);
            foreach (var item in dtoList)
            {
                item.ChargeSubjectName = domainList.Where(d => d.Id == item.Id).First().ChargeSubject.Name;
                //注释，现已存储快照 修改bug #4674 2017-7-11 
                //if ((int)ReourceTypeEnum.House==item.RefType)
                //{ 
                //    item.HouseDoorNo = limitHouseDeptInfos.Where(d => d.Id == item.HouseDeptId).First().Name;
                //    item.ResourcesName= limitHouseDeptInfos.Where(d => d.Id == item.HouseDeptId).First().Name;
                //}
                //else if((int )ReourceTypeEnum.CarPark==item.RefType)
                //{
                //    item.HouseDoorNo = limitCarPortInfoList.Where(d => d.Id == item.ResourcesId).First().CarportNum;
                //    item.ResourcesName = limitCarPortInfoList.Where(d => d.Id == item.ResourcesId).First().CarportNum;
                //}
                item.DeptName = searchDto.DeptName;
                item.IsChecked = true;
                ChargBill chargbill = domainList.Where(d => d.Id == item.Id).First();
                //item.AmountShould = chargbill.BillAmount.Value - chargbill.ReceivedAmount.Value - chargbill.ReliefAmount.Value;
            }

            return ChargBillDTOListGroup(dtoList.ToList());
        }




        #endregion

        #region 根据收费公式计算收费金额
        public decimal ComputeChargeSubjectAmount(int ChargeSubjectId, int ResourcesId, int RefTypeId)
        {
            ChargeSubjectDomainService _ChargeSubjectDomainService = new ChargeSubjectDomainService();

            return _ChargeSubjectDomainService.ComputeChargeSubjectAmount(ChargeSubjectId, ResourcesId, RefTypeId);

        }
        #endregion

        #region 获取批量生成账单—房屋列表
        /// <summary>
        ///  获取批量生成账单—房屋列表
        /// </summary>
        /// <returns></returns>
        public IList<TreeNodeModel> GetBatchGenerateBillHouseTree(int ChargeSubjectId)
        {

            ChargeSubjectAppService _ChargeSubjectAppService = new ChargeSubjectAppService();
            SubjectHouseRefDomainService _SubjectHouseRefDomainService = new SubjectHouseRefDomainService();
            var ChargeSubject = _ChargeSubjectAppService.GetChargeSubjectSingle(ChargeSubjectId);
            List<SubjectHouseRef> SubjectHouseRefList = _SubjectHouseRefDomainService.GetChargeSubjectList(o => o.ChargeSubjecId == ChargeSubjectId && o.IsDel == false);
            var DeptInfoList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetBuildAndHouseDeptInfoByComId(ChargeSubject.ComDeptId.Value);
            //获取根节点
            var RootTreeList = DeptInfoList.Where(o => o.DeptType == (int)EDeptType.LouYu).Select(o => new TreeNodeModel { id = o.Id.Value.ToString(), text = o.Name, icon = "fa fa-building", state = new { selected = true } }).ToList();
            List<TreeNodeModel> resultlist = new List<TreeNodeModel>();
            foreach (var root in RootTreeList)
            {
                var treelist = from c in DeptInfoList
                               join s in SubjectHouseRefList on c.Id equals s.ResourcesId
                               where c.DeptType == (int)EDeptType.FangWu && c.PId == Convert.ToInt32(root.id)
                               select new TreeNodeModel
                               {
                                   id = c.Id.Value.ToString(),
                                   text = c.Name,
                                   icon = "fa fa-institution"

                               };
                var testlist = treelist.ToList();
                if (treelist.Count() > 0)
                {
                    var resultRoot = root;


                    resultRoot.id = root.id + "_root";
                    resultRoot.children = testlist;
                    resultRoot.children.Sort(Factory.Comparer);
                    resultlist.Add(resultRoot);
                }
            }
            resultlist.Sort(Factory.Comparer);
            return resultlist;
        }
        #endregion

        #region 获取批量生成账单-车位列表

        public IList<TreeNodeModel> GetBatchGenerateBillCarParkTree(int ChargeSubjectId)
        {
            ChargeSubjectAppService _ChargeSubjectAppService = new ChargeSubjectAppService();
            SubjectHouseRefDomainService _SubjectHouseRefDomainService = new SubjectHouseRefDomainService();
            var ChargeSubject = _ChargeSubjectAppService.GetChargeSubjectSingle(ChargeSubjectId);
            List<SubjectHouseRef> SubjectHouseRefList = _SubjectHouseRefDomainService.GetChargeSubjectList(o => o.ChargeSubjecId == ChargeSubjectId && o.IsDel == false);
            var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
            var CarPortList = propertyService.GetParkingSpaceAndCarPortTree(ChargeSubject.ComDeptId.Value);
            var DeptInfoList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetBuildAndHouseDeptInfoByComId(ChargeSubject.ComDeptId.Value);
            List<TreeNodeModel> resultlist = new List<TreeNodeModel>();
            foreach (var root in CarPortList)
            {
                if (root.children != null)
                {
                    var childrenlist = from s in SubjectHouseRefList
                                       join p in root.children on s.ResourcesId.Value.ToString() equals p.id

                                       select new TreeNodeModel
                                       {
                                           id = p.id,
                                           icon = p.icon,
                                           state = p.state,
                                           text = DeptInfoList.Where(o => o.Id == s.HouseDeptId).FirstOrDefault() == null ? p.text + "" : p.text + "(" + DeptInfoList.Where(o => o.Id == s.HouseDeptId).FirstOrDefault().Name + ")"
                                       };
                    root.children = childrenlist.ToList();
                }
                if (root.children != null && root.children.Count > 0)
                {
                    var resultRoot = root;
                    resultRoot.id = root.id + "_root";
                    resultlist.Add(resultRoot);
                }

            }

            return resultlist;

        }


        #endregion

        #region 获取批量生成账单-三表

        public IList<TreeNodeModel> GetBatchGenerateBillMeterTree(int ChargeSubjectId)
        {
            MeterAppService meterAppservice = new MeterAppService();
            ChargeSubjectAppService _ChargeSubjectAppService = new ChargeSubjectAppService();
            SubjectHouseRefDomainService _SubjectHouseRefDomainService = new SubjectHouseRefDomainService();
            var ChargeSubject = _ChargeSubjectAppService.GetChargeSubjectSingle(ChargeSubjectId);
            List<TreeNodeModel> resultlist = new List<TreeNodeModel>();
            List<SubjectHouseRef> SubjectHouseRefList = _SubjectHouseRefDomainService.GetChargeSubjectList(o => o.ChargeSubjecId == ChargeSubjectId && o.IsDel == false);
            var DeptInfoList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetBuildAndHouseDeptInfoByComId(ChargeSubject.ComDeptId.Value);
           var TreeList = DeptInfoList.Where(o => o.DeptType == (int)EDeptType.LouYu).Select(o => new TreeNodeModel { id = o.Id.Value.ToString(), text = o.Name, icon = "fa fa-building", state = new { selected = true } }).ToList();
           
            //var DeptInfoList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetBuildAndHouseDeptInfoByComId(ChargeSubject.ComDeptId.Value);
            var listids = SubjectHouseRefList.Select(o => o.ResourcesId).ToArray();
            //获取三表信息、通过SubjectHouseRefList 中的[ResourcesId]
            MeterAppService meterAppServer = new MeterAppService();
            List<MeterDTO> meterDTOList = meterAppServer.GetMeterDTOS(listids);/*获取该栋楼下的三表*/

            var RootTreeList = DeptInfoList.Where(o => o.DeptType == (int)EDeptType.LouYu).Select(o => new TreeNodeModel { id = o.Id.Value.ToString(), text = o.Name, icon = "fa fa-building", state = new { selected = true } }).ToList();
           
            
                foreach (var root in RootTreeList)
            {
                var treelist = from c in DeptInfoList
                               join s in SubjectHouseRefList on c.Id equals s.HouseDeptId
                               join m in meterDTOList on s.ResourcesId equals m.Id
                               where c.DeptType == (int)EDeptType.FangWu && c.PId == Convert.ToInt32(root.id)
                               select new TreeNodeModel
                               {
                                   id = m.Id.Value.ToString(),
                                   text = c.Name + "(" + m.MeterNum + ")",
                                   icon = "fa fa-institution"
                               };

                var testlist = treelist.ToList();
                if (treelist.Count() > 0)
                {
                    var resultRoot = root;
                    resultRoot.id = root.id + "_root";
                    resultRoot.children = testlist;
                    resultRoot.children.Sort(Factory.Comparer);
                    resultlist.Add(resultRoot);
                }
            }
            List<MeterDTO> meterlist = new List<MeterDTO>();
            foreach (var item in SubjectHouseRefList)
            {
                var mList = meterAppservice.GetPublicMeterDTOSById(ChargeSubject.ComDeptId, item.ResourcesId);
                meterlist.AddRange(mList);
            }
                if (meterlist.Count() > 0)
                {
                    string strPublicMeter = string.Empty;
                    //获取公共表
             
                        switch (meterlist.FirstOrDefault().MeterType)
                        {
                            case (int)MeterTypeEnum.WaterMeter://水
                                {
                                    strPublicMeter = "公区水表";
                                }; break;
                            case (int)MeterTypeEnum.WattHourMeter://电
                                {
                                    strPublicMeter = "公区电表";
                                }; break;
                            case (int)MeterTypeEnum.GasMeter://气
                                {
                                    strPublicMeter = "公区气表";
                                }; break;
                            default:
                                break;
                        }
                        TreeNodeModel tree = new TreeNodeModel()
                        {
                            id = "0",
                            icon = "fa fa-dashboard",
                            text = strPublicMeter,
                            state = new { selected = true },
                            children = meterlist.Select(m => new TreeNodeModel()
                            {
                                //id = "0_1000_" + m.Id.ToString(),
                                id = m.Id.ToString(),
                                icon = "fa fa-dashboard",
                                //children = false,
                                text = m.MeterNum,
                                state = new { selected = true }
                            }).ToList()
                        };
                        resultlist.Add(tree);
                    }

                

            
            return resultlist;

        }


        #endregion

        #region 获取生成缴费通知单房屋列表
        /// <summary>
        ///  获取生成缴费通知单房屋列表
        /// </summary>
        /// <returns></returns>
        public IList<CustomTreeNodeModel> GetGeneratePaymentNoticeHouseTree(int ComDeptId)
        {

            ChargeSubjectAppService _ChargeSubjectAppService = new ChargeSubjectAppService();
            SubjectHouseRefDomainService _SubjectHouseRefDomainService = new SubjectHouseRefDomainService();
            var DeptInfoList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetBuildAndHouseDeptInfoByComId(ComDeptId);

            //获取小区
            var SEC_CommunityModel = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetGetCommunityById(ComDeptId);
            if (SEC_CommunityModel == null)
            {
                return new List<CustomTreeNodeModel>();
            }
            List<CustomTreeNodeModel> returnList = new List<CustomTreeNodeModel>();
            CustomTreeNodeModel CommumnityNode = new CustomTreeNodeModel()
            {
                id = ComDeptId.ToString() + "_Comroot",
                text = SEC_CommunityModel.Name,
                icon = "fa fa-comments",
                state = new { opened = true }
            };

            //获取楼宇节点
            var RootTreeList = DeptInfoList.Where(o => o.DeptType == (int)EDeptType.LouYu)
                .OrderBy(o => o.Id)
                .Select(o => new CustomTreeNodeModel
                {
                    id = o.Id.Value.ToString(),
                    text = o.Name,
                    icon = "fa fa-building",
                    state = new { opened = false, selected = true }
                }).ToList();

            RootTreeList.Sort(Factory.Comparer);
            CommumnityNode.children = RootTreeList;

            List<CustomTreeNodeModel> resultlist = new List<CustomTreeNodeModel>();
            foreach (var root in (List<CustomTreeNodeModel>)CommumnityNode.children)
            {
                var treelist = from c in DeptInfoList
                               where c.DeptType == (int)EDeptType.FangWu && c.PId == Convert.ToInt32(root.id)
                               select new CustomTreeNodeModel
                               {
                                   id = c.Id.Value.ToString(),
                                   text = c.Name,
                                   icon = "fa fa-institution"

                               };
                var testlist = treelist.ToList();
                if (treelist.Count() > 0)
                {
                    var resultRoot = root;
                    resultRoot.id = root.id + "_root";
                    resultRoot.children = true;
                    resultlist.Add(resultRoot);
                }
            }
            //resultlist.Sort(Factory.Comparer);
            returnList.Add(CommumnityNode);
            return returnList;
        }


        public IList<CustomTreeNodeModel> GetGeneratePaymentNoticeHouseTreeAysn(int LouyuId)
        {
            var DeptInfoList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouseDeptInfobyLouyuDeptId(LouyuId);

            return DeptInfoList.Select(o => new CustomTreeNodeModel { id = o.Id.ToString(), text = o.Name, icon = "fa fa-institution", children = false }).ToList();
        }


        #endregion

        #region 生成缴费通知单
        public List<PrintDataModel> GetPrintDataModel(int ComDeptId, List<int?> housesList, DateTime EndDate, string Remarks, ref int everyPayNotCount, ref int pagePayNot, ref int footSize)
        {
            Service.DeptAppService deptService = new Service.DeptAppService();
            var secModel = deptService.GetDeptInfoById(ComDeptId.ToString());
            int propertyDeptId = Convert.ToInt32(secModel.PId);//通过小区获取物业ID

            List<PrintDataModel> model = new List<PrintDataModel>();
            string propertyName = string.Empty;
            PrintTemplateEnum template = TemplateModelHelper.GetPrintTemplate(propertyDeptId, EPrintTemplate.FeeNotify, ref propertyName);
            switch (template)
            {
                case PrintTemplateEnum.TemplateOne:
                    model = TemplateFeeNotifyOne(ComDeptId, housesList, EndDate, Remarks, propertyName, ref everyPayNotCount, ref pagePayNot, ref footSize);
                    break;
            }
            return model;
        }
        /*模板三*/
        public List<PrintDataModel> TemplateFeeNotifyOne(int ComDeptId, List<int?> housesList, DateTime EndDate, string Remarks, string propertyName, ref int everyPayNotCount, ref int pagePayNot, ref int footSize)
        {
            footSize = 3;/*尾部单元格数*/
            pagePayNot = 1;/*每页显示几张收据*/
            int everyReceiptRow = 4;/*每张收据几行数据*/
            everyPayNotCount = everyReceiptRow * 6;/*每张收据显示条数*/


            var payNotList = GetPayNotList(housesList, EndDate);
            if (payNotList.Count <= 0)
                return new List<PrintDataModel>();
            //设置打印数据
            List<PrintDataModel> dataList = new List<PrintDataModel>();
            //if (payNotList.Count < housesList.Count) //如何合并后的数据小于传入的资源数，则使用合并后的资源，数据量小有助于提高查询速度
            //    housesList = payNotList.Select(s => s.HouseDeptId).ToList();
            var Community = DomainInterfaceHelper //小区信息
               .LookUp<IPropertyDomainService>().GetGetCommunityById(ComDeptId);
            //var HouseMasters = DomainInterfaceHelper //资源对应的住户信息
            //   .LookUp<IPropertyDomainService>().GetUserOwnerMasterByHouseDeptIds(housesList);
            var HouseMasters = DomainInterfaceHelper //获取小区下的房屋及业主信息
               .LookUp<IPropertyDomainService>().GetHouDeptAndOwnerListByComDeptId(ComDeptId.ToString());

            while (payNotList.Count > 0)
            {
                var payNot = payNotList.FirstOrDefault();
                var sameHouse = payNotList.Where(c => c.HouseDeptId == payNot.HouseDeptId).OrderBy(o => o.ChargeSubjectId).ThenBy(o => o.BeginDate).ToList();

                PrintDataModel data = new PrintDataModel(propertyName + "缴费通知单");

                //header
                data.PrintHeader.ColNum = 6;
                //float hw = PrintHelper.PageContentWidth / 6;
                data.PrintHeader.Widths = new float[] { 60, 110, 50, 110, 40, 110 };
                data.PrintHeader.PrintCellList.Add(new PrintCell() { Title = "管理小区：", HideBorder = true, Align = Element.ALIGN_RIGHT });
                data.PrintHeader.PrintCellList.Add(new PrintCell() { Value = Community.Name, HideBorder = true, Align = Element.ALIGN_LEFT });
                data.PrintHeader.PrintCellList.Add(new PrintCell() { Title = "日期 ：", HideBorder = true, Align = Element.ALIGN_RIGHT });
                data.PrintHeader.PrintCellList.Add(new PrintCell() { Value = DateTime.Now.ToString("yyyy年MM月dd日"), HideBorder = true, Align = Element.ALIGN_LEFT, Colspan = 3 });
                data.PrintHeader.PrintCellList.Add(new PrintCell() { Title = "电话：", HideBorder = true, Align = Element.ALIGN_RIGHT });
                data.PrintHeader.PrintCellList.Add(new PrintCell() { Value = Community.BindingPhone, HideBorder = true, Align = Element.ALIGN_LEFT });
                //body


                var customer = HouseMasters.FirstOrDefault(c => c.Id == payNot.HouseDeptId);
                data.PrintHeader.PrintCellList.Add(new PrintCell() { Title = "客户：", HideBorder = true, Align = Element.ALIGN_RIGHT });
                data.PrintHeader.PrintCellList.Add(new PrintCell() { Value = customer == null ? "" : customer.OwnerUserName, HideBorder = true, Align = Element.ALIGN_LEFT });
                data.PrintHeader.PrintCellList.Add(new PrintCell() { Title = "房号：", HideBorder = true, Align = Element.ALIGN_RIGHT });
                data.PrintHeader.PrintCellList.Add(new PrintCell() { Value = customer == null ? payNot.ResourcesName : customer.Name, HideBorder = true, Align = Element.ALIGN_LEFT });
                data.PrintHeader.PrintCellList.Add(new PrintCell() { Title = "", HideBorder = true, Align = Element.ALIGN_RIGHT });//空白单元格
                data.PrintHeader.PrintCellList.Add(new PrintCell() { Value = "", HideBorder = true, Align = Element.ALIGN_LEFT });//空白单元格

                data.PrintBody.ColNum = 6;
                data.PrintBody.RowTitles = new string[] { "收费科目", "摘要", "单价", "应缴", "减免", "金额(元)" };
                data.PrintBody.Widths = new float[] { 100, 180, 50, 50, 50, 50 };

                int resCount = sameHouse.Count;
                foreach (var resPn in sameHouse)
                {
                    data.PrintBody.AddRow(new string[]
                    {
                        resPn.Description,
                        resPn.AbstractDesc,
                        resPn.Price.HasValue? resPn.Price.ToString() : "",
                        resPn.AmountShould.ToString(),
                        resPn.ReliefAmount.HasValue? resPn.ReliefAmount.ToString() : "",
                        resPn.Amount.ToString()
                    });

                    payNotList.Remove(resPn);
                }
                if (resCount < everyReceiptRow)
                {
                    /*不足3条*/
                    GetPrintCellFoot(data.PrintBody.PrintFooterList, Remarks, sameHouse.Sum(b => b.Amount));
                    for (int i = 0; i < everyReceiptRow - resCount; i++)
                    {
                        data.PrintBody.AddRow(new string[] { " ", " ", " ", " ", " ", " " });
                    }
                }
                else
                {
                    /*超过3条*/
                    if (resCount % everyReceiptRow != 0)
                    {
                        for (int i = 0; i < everyReceiptRow - resCount % everyReceiptRow; i++)
                        {
                            data.PrintBody.AddRow(new string[] { " ", " ", " ", " ", " ", " " });
                        }
                    }
                    //footer
                    int pages = resCount % everyReceiptRow == 0 ? resCount / everyReceiptRow : resCount / everyReceiptRow + 1;
                    for (int P = 1; P <= pages; P++)
                    {
                        decimal amount = sameHouse.Skip((P - 1) * everyReceiptRow).Take(everyReceiptRow).Sum(o => o.Amount);
                        GetPrintCellFoot(data.PrintBody.PrintFooterList, Remarks, amount);
                    }
                }

                dataList.Add(data);
            }

            return dataList;
        }
        /*打印模板*/
        private List<PrintCell> GetPrintCellFoot(List<PrintCell> list, string Remark, decimal? totalAmount)
        {
            list.Add(new PrintCell() { Title = "合计（大写）：", Value = totalAmount.HasValue ? PrintHelper.ToAmountUppercase(totalAmount.Value) : "", Colspan = 3 });
            list.Add(new PrintCell() { Title = "合计（小写）：", Value = totalAmount.HasValue ? totalAmount.Value.ToString() : "", Colspan = 3 });
            list.Add(new PrintCell() { Title = "温馨提示：", Value = Remark, Colspan = 6 });
            return list;
        }
        /// <summary>
        /// 获取合并费用
        /// </summary>
        /// <param name="housesList">房间列表</param>
        /// <param name="EndDate">结束日期</param>
        /// <returns></returns>
        private IList<PaymentNoticePrintModel> GetPayNotList(List<int?> housesList, DateTime EndDate)
        {
            Condition<ChargBill> condition = new Condition<ChargBill>(c => c.IsDevPay != true //排除开发商代收
            && c.IsDel == false //没有作废的记录
            && c.BillAmount + c.PenaltyAmount > c.ReceivedAmount + c.ReliefAmount //账单金额+滞纳金额>已收金额+减免金额 表示欠费
            && c.Status == (int)BillStatusEnum.NoPayment
            && c.EndDate <= EndDate //结束日期在账单日之前的
            && housesList.Contains(c.HouseDeptId) //已选的资源
            && c.ChargeSubject.IsDel == false //没有作废的费用项目
            && (c.ChargeSubject.BillPeriod == EnumHelper.BillPeriod.DailyCharge
                || c.ChargeSubject.BillPeriod == EnumHelper.BillPeriod.MonthlyCharge
                || c.ChargeSubject.BillPeriod == EnumHelper.BillPeriod.MeterCharge
            ));//包括周期性费用和三表费用

            var domainList = ChargBillService.GetPaymentNotice(condition.ExpressionBody);
            //合并周期性费用数据
            List<PaymentNoticePrintModel> payNotList = new List<PaymentNoticePrintModel>();
            while (domainList.Count > 0)
            {
                PaymentNoticePrintModel pn = domainList.FirstOrDefault();
                PaymentNoticePrintModel payNot = null;
                bool ifDelete = true;
                if (pn.EndDate.HasValue)
                {
                    var periodicCost = domainList.Where(c =>
                    c.ChargeSubjectId == pn.ChargeSubjectId //相同费用
                    && c.HouseDeptId == pn.HouseDeptId) //相同资源
                    .OrderBy(o => o.BeginDate).ToList();

                    if (periodicCost.Count() > 1) //超过一条记录才合并
                    {
                        foreach (PaymentNoticePrintModel pc in periodicCost)
                        {
                            if (payNot == null)
                            {
                                payNot = pc;
                                domainList.Remove(pc);
                                continue;
                            }
                            if (pc.BeginDate != payNot.EndDate.Value.AddDays(1))
                            {
                                ifDelete = false;
                                break;
                            }

                            payNot.EndDate = pc.EndDate;
                            payNot.BillAmount += pc.BillAmount;
                            payNot.PenaltyAmount += pc.PenaltyAmount;
                            payNot.ReceivedAmount += pc.ReceivedAmount;
                            payNot.ReliefAmount += pc.ReliefAmount;

                            domainList.Remove(pc);
                        }
                    }
                }
                payNot = payNot ?? pn;
                payNotList.Add(payNot);
                if (ifDelete) //跳出循环后，未添加项，所以不移除
                    domainList.Remove(pn);
            }
            return payNotList.OrderBy(o => o.HouseDeptId).ToList();//按资源排序
        }

        #endregion

        #region 生成账单（做科目解除时）
        /// <summary>
        /// 生成账单（做科目解除时）
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="resourceId"></param>
        /// <param name="houseDeptId"></param>
        /// <returns></returns>
        public IList<ResultModel> GenerationBillUnbundling(List<int> subjectIds, List<UnbundlingDto> list)
        {
            ChargeSubjectAppService service = new ChargeSubjectAppService();
            Dictionary<int, IEnumerable<ApplicationDTO.ApplicationDTO.UnbundlingDto>> dic = new Dictionary<int, IEnumerable<ApplicationDTO.ApplicationDTO.UnbundlingDto>>();
            for (int i = 0; i < subjectIds.Count; i++)
            {
                dic.Add(subjectIds[i], list);
            }
            int comDeptId = Convert.ToInt32(service.GetChargeSubjectByKey(subjectIds[0]).ComDeptId);
            return GenerateBillAppService.ManualUnbundlingGenerationBill(comDeptId, dic);
        }

        #endregion

        #region 账单分组
        public IList<ChargBillDTO> ChargBillDTOListGroup_OLD(List<ChargBillDTO> dtoList)
        {
            dtoList.ForEach(o => { o.IsShow = true; });
            var list = (from m in dtoList
                        group m by new { m.ChargeSubjectId, m.HouseDeptId } into g
                        select new
                        {
                            ChargeSubjectId = g.Key.ChargeSubjectId,
                            HouseDeptId = g.Key.HouseDeptId
                        }).ToList();
            int groupbyId = 0;
            List<ChargBillDTO> bill = new List<ChargBillDTO>();
            List<ChargBillDTO> billgGroup = new List<ChargBillDTO>();
            Dictionary<int, List<ChargBillDTO>> dic = new Dictionary<int, List<ChargBillDTO>>();

            foreach (var m in list)
            {
                bill = dtoList.Where(o => o.ChargeSubjectId == m.ChargeSubjectId && o.ReceivedAmount == 0 && o.HouseDeptId == m.HouseDeptId).OrderBy(o => o.BeginDate).ToList();
                if (bill.Count > 1)
                {

                    groupbyId = groupbyId + 1;
                    for (int i = 0; i < bill.Count - 1; i++)
                    {

                        if (Convert.ToDateTime(bill[i].EndDate).AddDays(1) == Convert.ToDateTime(bill[i + 1].BeginDate))
                        {

                            if (!billgGroup.Any(o => o.Id == bill[i].Id))
                            {
                                bill[i].CccordionClass = string.Empty;
                                bill[i].IsShow = false;
                                bill[i].GroupId = groupbyId.ToString();
                                bill[i].RowType = RowTypeEnum.ChildRow;
                                billgGroup.Add(bill[i]);

                            }

                            if (!billgGroup.Any(o => o.Id == bill[i + 1].Id))
                            {
                                bill[i].CccordionClass = string.Empty;
                                bill[i + 1].IsShow = false;
                                bill[i + 1].GroupId = groupbyId.ToString();
                                bill[i + 1].RowType = RowTypeEnum.ChildRow;
                                billgGroup.Add(bill[i + 1]);
                            }
                        }
                    }
                }

            }
            List<string> groupIds = new List<string>();
            List<ChargBillDTO> billTemp = new List<ChargBillDTO>();
            List<ChargBillDTO> bills = new List<ChargBillDTO>();
            ChargBillDTO model;
            foreach (var o in billgGroup)
            {
                dtoList.Remove(dtoList.Where(x => x.Id == o.Id).FirstOrDefault());
                if (!groupIds.Contains(o.GroupId))
                {
                    groupIds.Add(o.GroupId);
                    billTemp = billgGroup.Where(m => m.GroupId == o.GroupId).OrderBy(n => n.BeginDate).ToList();
                    if (billTemp != null && billTemp.Count > 0)
                    {
                        var firstModel = billTemp.FirstOrDefault();
                        model = new ChargBillDTO();
                        model.Id = o.GroupId;
                        model.IsShow = true;
                        model.IsChecked = true;
                        model.RowType = RowTypeEnum.FatherRow;
                        model.HouseDeptId = firstModel.HouseDeptId;
                        model.HouseDoorNo = firstModel.HouseDoorNo;
                        model.ChargeSubjectName = firstModel.ChargeSubjectName;
                        model.ChargeSubjectId = firstModel.ChargeSubjectId;
                        model.SubjectType = firstModel.SubjectType;
                        model.BeginDate = firstModel.BeginDate;
                        model.EndDate = billTemp.LastOrDefault().EndDate;
                        model.ReceivedAmount = billTemp.Sum(m => m.ReceivedAmount);
                        model.PenaltyAmount = billTemp.Sum(m => m.PenaltyAmount);
                        model.ReliefAmount = billTemp.Sum(m => m.ReliefAmount);
                        model.PenaltyAmount = billTemp.Sum(m => m.PenaltyAmount);
                        model.BillAmount = billTemp.Sum(m => m.BillAmount);
                        model.CccordionClass = "fa fa-plus";
                        // model.GroupId = model.Id;
                        bills.Add(model);
                    }
                }
                dtoList.Add(o);
            }
            dtoList.AddRange(bills);
            // var a= dtoList.OrderBy(o => o.ChargeSubjectId).OrderBy(o => o.GroupId).OrderBy(o => o.BeginDate).ToList();
            var dataList = (from o in dtoList orderby o.HouseDeptId, o.ChargeSubjectId, o.BeginDate, o.GroupId select o).ToList();
            return dataList;
        }

        private void SetGroupList(List<ChargBillDTO> resultList, IList<ChargBillDTO> tempList, int groupId)
        {

            var firstModel = tempList.First();
            var endModel = tempList.Last();
            //添加组记录
            ChargBillDTO model = new ChargBillDTO();
            model.Id = groupId.ToString();
            model.IsShow = true;
            model.IsChecked = true;
            model.RowType = RowTypeEnum.FatherRow;
            model.HouseDeptId = firstModel.HouseDeptId;
            model.HouseDoorNo = firstModel.HouseDoorNo;
            model.ResourcesName = firstModel.ResourcesName;
            model.ChargeSubjectName = firstModel.ChargeSubjectName;
            model.ChargeSubjectId = firstModel.ChargeSubjectId;
            model.SubjectType = firstModel.SubjectType;
            model.BeginDate = firstModel.BeginDate;
            model.EndDate = endModel.EndDate;
            model.ReceivedAmount = tempList.Sum(m => m.ReceivedAmount);
            model.PenaltyAmount = tempList.Sum(m => m.PenaltyAmount);
            model.ReliefAmount = tempList.Sum(m => m.ReliefAmount);
            model.PenaltyAmount = tempList.Sum(m => m.PenaltyAmount);
            model.BillAmount = tempList.Sum(m => m.BillAmount);
            model.CccordionClass = "fa fa-plus";

            if (tempList.Count > 1)
            {
                resultList.Add(model);
                foreach (var cc in tempList)
                {
                    cc.GroupId = groupId.ToString();
                    cc.RowType = RowTypeEnum.ChildRow;
                    cc.IsShow = false;
                    resultList.Add(cc);
                }
            }
            else
            {
                foreach (var cc in tempList)
                {
                    cc.RowType = RowTypeEnum.ChildRow;
                    cc.IsShow = true;
                    resultList.Add(cc);
                }
            }
        }

        public IList<ChargBillDTO> ChargBillDTOListGroup(List<ChargBillDTO> dtoList)
        {
            //1.定义返回的List
            List<ChargBillDTO> resultList = new List<ChargBillDTO>();

            //2.定义组List by 科目和房屋
            var groupList = (from m in dtoList
                             group new { m.ChargeSubjectId, m.ResourcesId, m.Id } by new { m.ChargeSubjectId, m.ResourcesId } into g
                             select new
                             {
                                 ChargeSubjectId = g.Key.ChargeSubjectId,
                                 ResourcesId = g.Key.ResourcesId,
                                 Count = g.Count(m => m.Id != null)
                             }).ToList();
            int groupId = 1;
            //3.循环组
            foreach (var item in groupList)
            {
                if (item.Count == 1)
                {
                    var pieceOfdata = dtoList.Where(d => d.ChargeSubjectId == item.ChargeSubjectId && d.ResourcesId == item.ResourcesId).First();
                    pieceOfdata.IsShow = true;
                    resultList.Add(pieceOfdata);
                }
                else
                {
                    //子组
                    var cgList = dtoList.Where(d => d.ChargeSubjectId == item.ChargeSubjectId && d.ResourcesId == item.ResourcesId).ToList();
                    IList<ChargBillDTO> tempList = new List<ChargBillDTO>();
                    int count = cgList.Count() - 1;
                    for (int i = 0; i < count; i++)
                    {
                        //如果没有值
                        if (!cgList[i].BeginDate.HasValue)
                        {
                            cgList[i].IsShow = true;
                            resultList.Add(cgList[i]);
                            continue;
                        }

                        if (i == 0)
                        {
                            tempList.Add(cgList[i]);
                            //continue;
                        }

                        if (cgList[i].EndDate.Value.AddDays(1) == cgList[i + 1].BeginDate.Value && cgList[i].ReceivedAmount == 0 && cgList[i + 1].ReceivedAmount == 0)
                        {
                            tempList.Add(cgList[i + 1]);
                        }
                        else
                        {
                            //分组
                            if (tempList.Count() > 1)
                            {
                                SetGroupList(resultList, tempList, groupId);
                                groupId++;
                            }
                            else
                            {
                                tempList.First().IsShow = true;
                                resultList.AddRange(tempList);
                            }

                            tempList = new List<ChargBillDTO>();
                            tempList.Add(cgList[i + 1]);
                        }
                    }
                    if (tempList.Count() > 0)
                    {
                        SetGroupList(resultList, tempList, groupId);
                        groupId++;

                    }
                }
            }

            return resultList;
        }
        #endregion

        #region 对外收费

        public IList<ChargBillDTO> GetForeigChargBillDTOList(ChargBillSearchDTO searchDto, out int totalCount)
        {
            if (searchDto.DeptType != EDeptType.XiaoQu)
            {
                totalCount = 0;
                return new List<ChargBillDTO>();
            }
            Condition<ChargBill> condition = new Condition<ChargBill>(c => c.ComDeptId == searchDto.DeptId && c.Status == (int)BillStatusEnum.NoPayment && c.IsDel == false && c.ChargeSubject.SubjectType == (int)SubjectTypeEnum.Foreig);

            string expressions = "BeginDate";
            var domainList = ChargBillService.GetChargBillList(condition.ExpressionBody, expressions, out totalCount);

            var dtoList = ChargBillMappers.ChangeChargBillToDTOs(domainList);
            foreach (var item in dtoList)
            {
                item.ChargeSubjectName = domainList.Where(d => d.Id == item.Id).First().ChargeSubject.Name;
                item.IsChecked = true;
                ChargBill chargbill = domainList.Where(d => d.Id == item.Id).First();

            }

            return dtoList.ToList();
        }

        #endregion

        #region 作废账单
        public IList<ChargBillDTO> GetDeleteChargBillDTOList(ChargBillSearchDTO searchDto, out int totalCount)
        {
            if (searchDto.DeptType != EDeptType.XiaoQu)
            {
                totalCount = 0;
                return new List<ChargBillDTO>();
            }
            Condition<ChargBill> condition = new Condition<ChargBill>(c => c.IsDel == true && c.ComDeptId == searchDto.DeptId);//首先是删除订单

            if (!string.IsNullOrEmpty(searchDto.ResourcesName))
            {
                condition = condition & new Condition<ChargBill>(c => c.ResourcesName.Contains(searchDto.ResourcesName));
            }
            if ((!string.IsNullOrEmpty(searchDto.ChargeSubjectId)))
            {
                int ChargeSubjectId = int.Parse(searchDto.ChargeSubjectId.Replace("number:", ""));
                if (ChargeSubjectId > 0)
                    condition = condition & new Condition<ChargBill>(c => c.ChargeSubjectId == ChargeSubjectId);
            }
            string expressions = "BeginDate";
            var domainList = ChargBillService.GetChargBillListPage(condition.ExpressionBody, expressions, out totalCount, searchDto.PageIndex, searchDto.PageSize);
            var dtoList = ChargBillMappers.ChangeChargBillToDTOs(domainList);
            foreach (var item in dtoList)
            {
                item.ChargeSubjectName = domainList.Where(d => d.Id == item.Id).First().ChargeSubject.Name;
                ChargBill chargbill = domainList.Where(d => d.Id == item.Id).First();
            }
            return dtoList.ToList();
        }


        public IEnumerable<TemplateModel> GetDeleteChargBillListTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {

                new TemplateColumn(){ ColumnName = "ResourcesName", ColumnDesc = "资源", Seq = i++},
                new TemplateColumn(){ ColumnName = "ChargeSubjectName", ColumnDesc = "收费项目", Seq = i++},
                new TemplateColumn(){ ColumnName = "BillAmount", ColumnDesc = "计费金额", Seq = i++},
                new TemplateColumn(){ ColumnName = "ReceivedAmount", ColumnDesc = "已交金额", Seq = i++},
                new TemplateColumn(){ ColumnName = "AmountShould", ColumnDesc = "应收金额", Seq = i++},
                new TemplateColumn(){ ColumnName = "BeginDate", ColumnDesc = "开始日期", Seq = i++, Type = "date"},
                new TemplateColumn(){ ColumnName = "EndDate", ColumnDesc = "结束日期", Seq = i++, Type = "date"},
                new TemplateColumn(){ ColumnName = "ReliefAmount", ColumnDesc = "减免金额", Seq = i++},
                new TemplateColumn(){ ColumnName = "UpdateTime", ColumnDesc = "作废时间", Seq = i++, Type = "date"},

                new TemplateColumn(){ ColumnName = "Description", ColumnDesc = "描述", Seq = i++},
                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++}

            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ChargBillDTO), showColumns);
            return template;
        }
        #endregion

        #region 预存费抵扣部分

        /// <summary>
        /// 设置账单可抵扣的预存费
        /// </summary>
        public void SetBillPreAmount(IList<ChargBillDTO> chargBillList, int? houseDeptId)
        {
            //1.获取该房屋下面的预存费
            PrepayAccountDomainService preService = new PrepayAccountDomainService();
            var preList = preService.GetPrepayAccountListByHouseDeptId(houseDeptId);//ToDictionary(key => key.ChargeSubjectID, value => value);
           
            //2.按账单最早优先抵扣分配 预存抵扣金额
            var orderBillList = chargBillList.OrderBy(b => b.BeginDate);

            //3.收费项目账户抵扣
            var subjectAccountList = preList.Where(k => k.ChargeSubjectID != 0).ToList();
            foreach (var item in subjectAccountList)
            {
                var subBillList = orderBillList.Where(o => o.ChargeSubjectId == item.ChargeSubjectID);
                foreach (var bill in subBillList)
                {
                    //如余额抵扣完 跳出循环
                    if (item.Balance == 0)
                    {
                        break;
                    }
                    //欠款 = 账单金额 - 已收金额
                    var arrears = bill.BillAmount - bill.ReceivedAmount;
                    if (item.Balance > arrears)//余额大于欠款
                    {
                        bill.PreAmount = arrears.HasValue ? arrears.Value : 0;//可抵扣预存费 = 欠款
                        item.Balance -= arrears; 
                    }
                    else //余额小于等于欠款
                    {
                        bill.PreAmount = item.Balance.HasValue ? item.Balance.Value : 0;//可抵扣预存费 = 剩余余额
                        item.Balance = 0;   //全部抵扣完
                    }
                }
            }

            //4.全部收费项目抵扣
            var allSubjectAccount = preList.Where(k => k.ChargeSubjectID == 0).FirstOrDefault();
            if (allSubjectAccount != null)
            {
                //收费项目未抵扣完的账单
                var billList = orderBillList.Where(b => (b.BillAmount - b.ReceivedAmount - b.PreAmount) > 0);
                foreach (var bill in billList)
                {
                    //如余额抵扣完 跳出循环
                    if (allSubjectAccount.Balance == 0)
                    {
                        break;
                    }
                    //欠款 = 账单金额 - 已收金额
                    var arrears = bill.BillAmount - bill.ReceivedAmount;
                    if (allSubjectAccount.Balance > arrears)//余额大于欠款
                    {
                        bill.PreAmount = arrears.HasValue ? arrears.Value : 0;//可抵扣预存费 = 欠款
                        allSubjectAccount.Balance -= arrears;
                    }
                    else //余额小于等于欠款
                    {
                        bill.PreAmount = allSubjectAccount.Balance.HasValue ? allSubjectAccount.Balance.Value : 0;//可抵扣预存费 = 剩余余额
                        allSubjectAccount.Balance = 0;   //全部抵扣完
                    }
                }
            }
        }

        #endregion

        class Factory : IComparer<BaseTreeNodeModel>
        {
            private Factory() { }
            public static IComparer<BaseTreeNodeModel> Comparer
            {
                get { return new Factory(); }
            }
            public int Compare(BaseTreeNodeModel x, BaseTreeNodeModel y)
            {
                try
                {
                    return x.text.Length == y.text.Length ? x.text.CompareTo(y.text) : x.text.Length - y.text.Length;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
