using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.ApplicationDTO.Resources;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.ApplicationService.Service;
using YK.PropertyMgr.CompositeAppService;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.DomainService;
using YK.PropertyMgr.RepositoryContract;

namespace YK.PropertyMgr.ApplicationService
{
    public partial class ChargeRecordAppService
    {
        public static IList<DictionaryModel> ChargeTypeModel
        {
            get
            {
                var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
                return propertyService.GetDictionaryModels(PropertyEnumType.ChargeType.ToString());
            }
        }

        #region 获取单条费用记录

        public ChargeRecordDTO GetChargeRecordById(string Id)
        {
            var record = ChargeRecordService.GetChargeRecordById(Id);
            var recordDto = ChargeRecordMappers.ChangeChargeRecordToDTO(record);
            recordDto.ReceiptNum = record.Receipt.Number;
            recordDto.ReceiptStatus = record.Receipt.Status.Value;
            recordDto.HouseDoorNo = recordDto.HouseDeptNos;
            if (!string.IsNullOrEmpty(recordDto.HouseDeptNos))
            {
                recordDto.HouseDoorNoFormat = (recordDto.HouseDeptNos.Length > 12 ? "<label style='font-weight:400' title='" + recordDto.HouseDeptNos + "'>" + recordDto.HouseDeptNos.Substring(0, 12) + "...</label>" : recordDto.HouseDeptNos);
            }
            if (!string.IsNullOrEmpty(recordDto.ResourcesNames))
            {

                recordDto.ResourcesNamesFormat = (recordDto.ResourcesNames.Length > 12 ? "<label style='font-weight:400' title='" + recordDto.ResourcesNames + "'>" + recordDto.ResourcesNames.Substring(0, 12) + "...</label>" : recordDto.ResourcesNames);
            }
            if (recordDto.Amount < 0)
            {
                recordDto.Amount = recordDto.Amount * -1;
            }

            return recordDto;
        }

        #endregion

        #region 费用记录查询

        /// <summary>
        /// 标准费用记录查询
        /// </summary>
        /// <param name="searchDto"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<ChargeRecordDTO> GetChargeRecordDTOList(ChargeRecordSearchDTO searchDto, out int totalCount)
        {
            if (searchDto.DeptType == EDeptType.FangWu ||searchDto.DeptType == EDeptType.CheWei || (searchDto.IsDeveloper && searchDto.DeptType == EDeptType.XiaoQu))
            {
                //条件
                Condition<ChargeRecord> condition = new Condition<ChargeRecord>(c => true);
                if (!string.IsNullOrEmpty(searchDto.ReceiptNum))
                {
                    condition = condition & new Condition<ChargeRecord>(c => c.Receipt.Number.Contains(searchDto.ReceiptNum));
                }
                if (searchDto.ChargeStartDate.HasValue)
                {
                    condition = condition & new Condition<ChargeRecord>(c => c.PayDate >= searchDto.ChargeStartDate);
                }
                if (searchDto.ChargeEndDate.HasValue)
                {
                    searchDto.ChargeEndDate = searchDto.ChargeEndDate.Value.AddDays(1).AddSeconds(-1);
                    condition = condition & new Condition<ChargeRecord>(c => c.PayDate <= searchDto.ChargeEndDate);
                }
                if (!string.IsNullOrEmpty(searchDto.SerialNumber))
                {
                    condition = condition & new Condition<ChargeRecord>(c => c.SerialNumber.Contains(searchDto.SerialNumber));
                }
                if (searchDto.AccountingStatus.HasValue)
                {
                    condition = condition & new Condition<ChargeRecord>(c => c.AccountingStatus == searchDto.AccountingStatus);
                }
                if (searchDto.ChargeType.HasValue)
                {
                    int chargeType = searchDto.ChargeType.Value.GetHashCode();
                    condition = condition & new Condition<ChargeRecord>(c => c.ChargeType == chargeType);
                }
                if (searchDto.DeptId != 0)
                {
                    //如果是开发商代缴
                    if (searchDto.IsDeveloper && searchDto.DeptType == EDeptType.XiaoQu)
                    {
                        //查询小区级别
                        condition = condition & new Condition<ChargeRecord>(c => c.ComDeptId == searchDto.DeptId);
                    }
                }
                //默认排序
                string expressions = "PayDate desc";
                //获取domain entity
                IList<ChargeRecord> domainList = new List<ChargeRecord>();
                //如果是开发商代缴
                if (searchDto.IsDeveloper)
                {
                    domainList = ChargeRecordService.GetChargeRecordList(searchDto.PageStart, searchDto.PageSize, condition.ExpressionBody, expressions, searchDto.IsDeveloper, out totalCount);
                }
                else
                {

                    if (searchDto.DeptType == EDeptType.FangWu)
                    {
                        domainList = ChargeRecordService.GetChargeRecordList(searchDto.PageStart, searchDto.PageSize, condition.ExpressionBody, expressions, searchDto.DeptId, out totalCount);
                    }
                    else
                    {
                        domainList = ChargeRecordService.GetChargeRecordList(searchDto.PageStart, searchDto.PageSize, condition.ExpressionBody, expressions, searchDto.DeptId, (int)ReourceTypeEnum.CarPark, out totalCount);
                    }

                }
                //转换为 dto
                var dtoList = ChargeRecordMappers.ChangeChargeRecordToDTOs(domainList);
                List<string> ChargeRecordIds = domainList.Where(o => o.ChargeType == (int)ChargeTypeEnum.Refund).Select(o => o.Id).ToList();
                Condition<RefundRecord> Refundcondition = new Condition<RefundRecord>(c => ChargeRecordIds.Contains(c.ChargeRecordId));
                RefundRecordDomainService _RefundRecordDomainService = new RefundRecordDomainService();
                var RefundRecordList = _RefundRecordDomainService.GetRefundRecordList(Refundcondition.ExpressionBody);

                //获取字典描述数据
                //var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
                //Dictionary<int, string> chargeTypeList = propertyService.GetDictionaryModels(PropertyEnumType.ChargeType).ToDictionary(key => key.Id, vaule => vaule.CnName);
                foreach (var item in dtoList)
                {
                    item.ReceiptNum = domainList.Where(d => d.Id == item.Id).First().Receipt.Number;
                    item.ReceiptStatus = domainList.Where(d => d.Id == item.Id).First().Receipt.Status.Value;
                    item.HouseDoorNo = item.HouseDeptNos;
                    item.RefundReason = RefundRecordList.Where(o => o.ChargeRecordId == item.Id).FirstOrDefault() == null ? "" : RefundRecordList.Where(o => o.ChargeRecordId == item.Id).FirstOrDefault().Reason;
                    if (!string.IsNullOrEmpty(item.HouseDeptNos))
                    {
                        item.HouseDoorNoFormat = (item.HouseDeptNos.Length > 12 ? "<label style='font-weight:400' title='" + item.HouseDeptNos + "'>" + item.HouseDeptNos.Substring(0, 12) + "...</label>" : item.HouseDeptNos);
                    }
                    if (!string.IsNullOrEmpty(item.ResourcesNames))
                    {
                  
                        item.ResourcesNamesFormat = (item.ResourcesNames.Length > 12 ? "<label style='font-weight:400' title='" + item.ResourcesNames + "'>" + item.ResourcesNames.Substring(0, 12) + "...</label>" : item.ResourcesNames);
                    }
                    

                    //item.HouseDoorNo = "<label style='font-weight:400' title='1-1-1-1,1-1-1-2,1-1-1-3'>1-1-1-1...</label>";
                    //修改bug #2377
                    if (item.Amount < 0)
                    {
                        item.Amount = item.Amount * -1;
                    }
                    if (item.DiscountAmount < 0)
                    {
                        item.DiscountAmount = item.DiscountAmount * -1;
                    }
                }
                //返回数据
                return dtoList.ToList();
            }
            else
            {
                totalCount = 0;
                return new List<ChargeRecordDTO>();
            }
        }


        public IList<ChargeRecordDTO> GetForgieChargeRecordDTOList(ChargeRecordSearchDTO searchDto, out int totalCount)
        {
            if (searchDto.DeptType == EDeptType.XiaoQu)
            {
                //条件
                Condition<ChargeRecord> condition = new Condition<ChargeRecord>(c => true);
                if (!string.IsNullOrEmpty(searchDto.ReceiptNum))
                {
                    condition = condition & new Condition<ChargeRecord>(c => c.Receipt.Number.Contains(searchDto.ReceiptNum));
                }
                if (searchDto.ChargeStartDate.HasValue)
                {
                    condition = condition & new Condition<ChargeRecord>(c => c.PayDate >= searchDto.ChargeStartDate);
                }
                if (searchDto.ChargeEndDate.HasValue)
                {
                    searchDto.ChargeEndDate = searchDto.ChargeEndDate.Value.AddDays(1).AddSeconds(-1);
                    condition = condition & new Condition<ChargeRecord>(c => c.PayDate <= searchDto.ChargeEndDate);
                }
                if (!string.IsNullOrEmpty(searchDto.SerialNumber))
                {
                    condition = condition & new Condition<ChargeRecord>(c => c.SerialNumber.Contains(searchDto.SerialNumber));
                }
                if (searchDto.AccountingStatus.HasValue)
                {
                    condition = condition & new Condition<ChargeRecord>(c => c.AccountingStatus == searchDto.AccountingStatus);
                }
                if (searchDto.ChargeType.HasValue)
                {
                    int chargeType = searchDto.ChargeType.Value.GetHashCode();
                    condition = condition & new Condition<ChargeRecord>(c => c.ChargeType == chargeType);
                }

                //默认排序
                string expressions = "PayDate desc";
                //获取domain entity
                IList<ChargeRecord> domainList = new List<ChargeRecord>();
 

                domainList = ChargeRecordService.GetForeigChargeRecordList(searchDto.PageStart, searchDto.PageSize, condition.ExpressionBody, expressions, searchDto.DeptId, out totalCount);

                List<string> ChargeRecordIds = domainList.Where(o => o.ChargeType == (int)ChargeTypeEnum.Refund).Select(o => o.Id).ToList();
                //获取退款数据

                Condition<RefundRecord> Refundcondition = new Condition<RefundRecord>(c => ChargeRecordIds.Contains(c.ChargeRecordId));
                RefundRecordDomainService _RefundRecordDomainService = new RefundRecordDomainService();
                var RefundRecordList = _RefundRecordDomainService.GetRefundRecordList(Refundcondition.ExpressionBody);
                //转换为 dto
                var dtoList = ChargeRecordMappers.ChangeChargeRecordToDTOs(domainList);
                //获取字典描述数据
                //var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
                //Dictionary<int, string> chargeTypeList = propertyService.GetDictionaryModels(PropertyEnumType.ChargeType).ToDictionary(key => key.Id, vaule => vaule.CnName);
                foreach (var item in dtoList)
                {
                    item.ReceiptNum = domainList.Where(d => d.Id == item.Id).First().Receipt.Number;
                    item.ReceiptStatus = domainList.Where(d => d.Id == item.Id).First().Receipt.Status.Value;
                    item.RefundReason = RefundRecordList.Where(o => o.ChargeRecordId == item.Id).FirstOrDefault() == null ? "" : RefundRecordList.Where(o => o.ChargeRecordId == item.Id).FirstOrDefault().Reason;


                    item.ResourcesNames = item.ResourcesNames;
                    if (!string.IsNullOrEmpty(item.HouseDeptNos))
                    {
                        item.ResourcesNamesFormat = (item.ResourcesNames.Length > 12 ? "<label style='font-weight:400' title='" + item.ResourcesNames + "'>" + item.ResourcesNames.Substring(0, 12) + "...</label>" : item.ResourcesNames);
                    }
                    if (item.Amount < 0)
                    {
                        item.Amount = item.Amount * -1;
                    }
                }
                //返回数据
                return dtoList.ToList();
            }
            else
            {
                totalCount = 0;
                return new List<ChargeRecordDTO>();
            }
        }

        #endregion

        #region 获取对象模板

        public IEnumerable<TemplateModel> GetChargeRecordListTemplate(bool IsShowHouse, bool SettleAccount)
        {
            int i = 1;
            List<TemplateColumn> showColumns = new List<TemplateColumn>();

            showColumns.Add(new TemplateColumn() { ColumnName = "ReceiptNum", ColumnDesc = "票据号", Seq = i++ });
            showColumns.Add(new TemplateColumn() { ColumnName = "PayDate", ColumnDesc = "收费日期", Seq = i++, Type = "date" });
            showColumns.Add(new TemplateColumn() { ColumnName = "Amount", ColumnDesc = "收费金额", Seq = i++ });
            showColumns.Add(new TemplateColumn() { ColumnName = "DiscountAmount", ColumnDesc = "优惠金额", Seq = i++ });
            showColumns.Add(new TemplateColumn() { ColumnName = "ChargeType", ColumnDesc = "收费类型", Seq = i++, DictId = PropertyEnumType.ChargeType.ToString() });
            if (IsShowHouse)
            {
                showColumns.Add(new TemplateColumn() { ColumnName = "ResourcesNamesFormat", ColumnDesc = "收费资源", Seq = i++ });
            }
            else
            {
                showColumns.Add(new TemplateColumn() { ColumnName = "ResourcesNames", ColumnDesc = "收费资源", Seq = i++ });
            }

            showColumns.Add(new TemplateColumn() { ColumnName = "CustomerName", ColumnDesc = "客户", Seq = i++ });
            showColumns.Add(new TemplateColumn() { ColumnName = "OperatorName", ColumnDesc = "操作人", Seq = i++ });
            showColumns.Add(new TemplateColumn() { ColumnName = "PayMthodId", ColumnDesc = "付款方式", Seq = i++, DictId = PropertyEnumType.PayType.ToString() });
            if (SettleAccount)
            {
                showColumns.Add(new TemplateColumn() { ColumnName = "SerialNumber", ColumnDesc = "序列号", Seq = i++ });
                showColumns.Add(new TemplateColumn() { ColumnName = "AccountingStatus", ColumnDesc = "预结算状态", Seq = i++, DictId = PropertyEnumType.AccountingStatus.ToString() });
            }
            showColumns.Add(new TemplateColumn() { ColumnName = "ReceiptStatus", ColumnDesc = "状态", Seq = i++, DictId = PropertyEnumType.ReceiptStatus.ToString() });
            showColumns.Add(new TemplateColumn() { ColumnName = "RefundReason", ColumnDesc = "退款原因", Seq = i++ });
            
            showColumns.Add(new TemplateColumn() { ColumnName = "Remark", ColumnDesc = "备注", Seq = i++ });

            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ChargeRecordDTO), showColumns.ToArray());
            return template;
        }

        public IEnumerable<TemplateModel> GetPaymentTaskChargeRecordListTemplate()
        {
            int i = 1;
            List<TemplateColumn> showColumns = new List<TemplateColumn>();

            showColumns.Add(new TemplateColumn() { ColumnName = "ReceiptNum", ColumnDesc = "票据号", Seq = i++ });
            showColumns.Add(new TemplateColumn() { ColumnName = "PayDate", ColumnDesc = "收费日期", Seq = i++, Type = "date" });
            showColumns.Add(new TemplateColumn() { ColumnName = "Amount", ColumnDesc = "收费金额", Seq = i++ });
            showColumns.Add(new TemplateColumn() { ColumnName = "DiscountAmount", ColumnDesc = "优惠金额", Seq = i++ });
            showColumns.Add(new TemplateColumn() { ColumnName = "ChargeType", ColumnDesc = "收费类型", Seq = i++, DictId = PropertyEnumType.ChargeType.ToString() });
            showColumns.Add(new TemplateColumn() { ColumnName = "ResourcesNames", ColumnDesc = "收费资源", Seq = i++ });
            showColumns.Add(new TemplateColumn() { ColumnName = "CustomerName", ColumnDesc = "客户", Seq = i++ });
            showColumns.Add(new TemplateColumn() { ColumnName = "OperatorName", ColumnDesc = "操作人", Seq = i++ });
            showColumns.Add(new TemplateColumn() { ColumnName = "PayMthodId", ColumnDesc = "付款方式", Seq = i++, DictId = PropertyEnumType.PayType.ToString() });
            showColumns.Add(new TemplateColumn() { ColumnName = "ReceiptStatus", ColumnDesc = "状态", Seq = i++, DictId = PropertyEnumType.ReceiptStatus.ToString() });
            showColumns.Add(new TemplateColumn() { ColumnName = "Remark", ColumnDesc = "备注", Seq = i++ });

            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ChargeRecordDTO), showColumns.ToArray());
            return template;
        }

        public IEnumerable<TemplateModel> GetChargeRecordViewTemplate(ChargeRecordDTO chargeRecordInfo, bool IsPrint = false)
        {
            var paytype = (PayTypeEnum)chargeRecordInfo.PayMthodId;
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "HouseDoorNo", ColumnDesc = "房屋号", Seq = i++, ElementType = "TextBox"},
                new TemplateColumn(){ ColumnName = "CustomerName", ColumnDesc = "客户", Seq = i++, ElementType = "TextBox"},
                new TemplateColumn(){ ColumnName = "ReceiptNum", ColumnDesc = "票据号", Seq = i++, ElementType = "TextBox", IsRequred = IsPrint, MaxLength = 50, ValidateType= (IsPrint?"stringLength":"") },
                new TemplateColumn(){ ColumnName = "Amount", ColumnDesc = "金额", Seq = i++, ElementType = "TextBox"},
                new TemplateColumn(){ ColumnName = "PayMthodId", ColumnDesc = "付款方式", Seq = i++, DictId = PropertyEnumType.PayType.ToString(), IsRequred = true},
                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++, ElementType="TextArea",MaxLength = 300, ValidateType="stringLength"}
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ChargeRecordDTO), showColumns);
            TemplateModel temp = template.Where(t => t.EnName == "PayMthodId").FirstOrDefault();
            if (temp != null && paytype != PayTypeEnum.Wallet)
            {
                //排除钱包抵扣
                temp.DictionaryModels = temp.DictionaryModels.Where(d => d.Code != PayTypeEnum.Wallet.GetHashCode().ToString());
            }
            return template;
        }

        public IEnumerable<TemplateModel> GetBillChargeRecordViewTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "TransactionDesc", ColumnDesc = "交易项", Seq = i++},
                new TemplateColumn(){ ColumnName = "ChargeType", ColumnDesc = "收费类型", Seq = i++, DictId = PropertyEnumType.ChargeType.ToString()},
                new TemplateColumn(){ ColumnName = "BeginDate", ColumnDesc = "账单开始", Seq = i++, Type="date"},
                new TemplateColumn(){ ColumnName = "EndDate", ColumnDesc = "账单结束", Seq = i++, Type="date"},
                new TemplateColumn(){ ColumnName = "Amount", ColumnDesc = "交易金额", Seq = i++},
                new TemplateColumn(){ ColumnName = "PayDate", ColumnDesc = "交易日期", Seq = i++, Type="date"},
                new TemplateColumn(){ ColumnName = "ReceiptNum", ColumnDesc = "票据号", Seq = i++},
                new TemplateColumn(){ ColumnName = "OperatorName", ColumnDesc = "操作人", Seq = i++},
                new TemplateColumn(){ ColumnName = "PayType", ColumnDesc = "付款方式", Seq = i++, DictId = PropertyEnumType.PayType.ToString()},
                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++}
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(BillChargeRecord), showColumns);
            return template;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsShowHouse"></param>
        /// <returns></returns>
        public IEnumerable<TemplateModel> GetForegiChargeRecordListTemplate(bool SettleAccount)
        {
            int i = 1;
            List<TemplateColumn> showColumns = new List<TemplateColumn>();

            showColumns.Add(new TemplateColumn() { ColumnName = "ReceiptNum", ColumnDesc = "票据号", Seq = i++ });
            showColumns.Add(new TemplateColumn() { ColumnName = "PayDate", ColumnDesc = "收费日期", Seq = i++, Type = "date" });
            showColumns.Add(new TemplateColumn() { ColumnName = "Amount", ColumnDesc = "收费金额", Seq = i++ });
            showColumns.Add(new TemplateColumn() { ColumnName = "ChargeType", ColumnDesc = "收费类型", Seq = i++, DictId = PropertyEnumType.ChargeType.ToString() });
            showColumns.Add(new TemplateColumn() { ColumnName = "ResourcesNames", ColumnDesc = "收费资源", Seq = i++ });
            showColumns.Add(new TemplateColumn() { ColumnName = "CustomerName", ColumnDesc = "客户", Seq = i++ });
            showColumns.Add(new TemplateColumn() { ColumnName = "OperatorName", ColumnDesc = "操作人", Seq = i++ });
            showColumns.Add(new TemplateColumn() { ColumnName = "PayMthodId", ColumnDesc = "付款方式", Seq = i++, DictId = PropertyEnumType.PayType.ToString() });
            if (SettleAccount)
            {
                showColumns.Add(new TemplateColumn() { ColumnName = "SerialNumber", ColumnDesc = "序列号", Seq = i++ });
                showColumns.Add(new TemplateColumn() { ColumnName = "AccountingStatus", ColumnDesc = "预结算状态", Seq = i++, DictId = PropertyEnumType.AccountingStatus.ToString() });
            }
            showColumns.Add(new TemplateColumn() { ColumnName = "ReceiptStatus", ColumnDesc = "状态", Seq = i++, DictId = PropertyEnumType.ReceiptStatus.ToString() });
            showColumns.Add(new TemplateColumn() { ColumnName = "RefundReason", ColumnDesc = "退款原因", Seq = i++ });
            showColumns.Add(new TemplateColumn() { ColumnName = "Remark", ColumnDesc = "备注", Seq = i++ });

            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ChargeRecordDTO), showColumns.ToArray());
            return template;
        }


        public IEnumerable<TemplateModel> GetForegiChargeRecordViewTemplate(ChargeRecordDTO chargeRecordInfo, bool IsPrint = false)
        {
            var paytype = (PayTypeEnum)chargeRecordInfo.PayMthodId;
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {

                new TemplateColumn(){ ColumnName = "CustomerName", ColumnDesc = "收费对象", Seq = i++, ElementType = "TextBox"},
                new TemplateColumn(){ ColumnName = "ReceiptNum", ColumnDesc = "票据号", Seq = i++, ElementType = "TextBox", IsRequred = IsPrint, MaxLength = 50, ValidateType= (IsPrint?"stringLength":"") },
                new TemplateColumn(){ ColumnName = "Amount", ColumnDesc = "金额", Seq = i++, ElementType = "TextBox"},
                new TemplateColumn(){ ColumnName = "PayMthodId", ColumnDesc = "付款方式", Seq = i++, DictId = PropertyEnumType.PayType.ToString(), IsRequred = true},
                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++, ElementType="TextArea",MaxLength = 300, ValidateType="stringLength"}
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ChargeRecordDTO), showColumns);
            TemplateModel temp = template.Where(t => t.EnName == "PayMthodId").FirstOrDefault();
            if (temp != null && paytype != PayTypeEnum.Wallet)
            {
                //排除钱包抵扣
                temp.DictionaryModels = temp.DictionaryModels.Where(d => d.Code != PayTypeEnum.Wallet.GetHashCode().ToString());
            }
            return template;
        }



        #endregion

        #region 退款

        public ResultModel Refund(RefundRecordDTO RefundRecord, int Operator, string OperatorName)
        {
            return PaymentAppService.Refund(RefundRecord, Operator, OperatorName);
        }

        #endregion

        #region 修改

        public bool IsSubmitted(ChargeRecordDTO ChargeRecordInfo)
        {
            return PaymentAppService.IsSubmitted(ChargeRecordInfo.Id);
        }

        public bool Update(ChargeRecordDTO ChargeRecordInfo)
        {
            ChargeRecord ChargeRecord = ChargeRecordMappers.ChangeDTOToChargeRecordNew(ChargeRecordInfo);
            return ChargeRecordService.Update(ChargeRecord);
        }

        #endregion

        #region 票据补打

        #region 标准模板
        public ResultModel ReceiptPrint(ChargeRecordDTO ChargeRecordInfo)
        {
            ChargeRecord ChargeRecord = ChargeRecordMappers.ChangeDTOToChargeRecordNew(ChargeRecordInfo);
            return ChargeRecordService.ReceiptPrint(ChargeRecord, ChargeRecordInfo.ReceiptNum);
        }

        public PrintDataModel GetPrintDataModel(string chargeRecordId, ref int countData, ref int everyBillCount, ref int pageReceipt, ref int footSize, int template, string propertyName, string number, string houseNo)
        {
            var chargeRecord = ChargeRecordService.GetChargeRecordAndReceiptByKey(chargeRecordId);
            var Community = DomainInterfaceHelper
                .LookUp<IPropertyDomainService>().GetGetCommunityById(chargeRecord.ComDeptId.Value);
            PrintDataModel model = new PrintDataModel();
            switch (template)
            {
                case (int)PrintTemplateEnum.TemplateOne:
                    model = TemplateDataOne(chargeRecord, ref countData, ref everyBillCount, ref pageReceipt, ref footSize, Community.Name, Community.BindingPhone, propertyName);
                    break;
                case (int)PrintTemplateEnum.TemplateTwo:
                    model = TemplateDataTwo(chargeRecord, ref countData, ref everyBillCount, ref pageReceipt, ref footSize, propertyName, number, houseNo, Community.Name, Community.BindingPhone);
                    break;
                
            }
            return model;
        }


        /*打印模板*/
        private List<PrintCell> GetPrintCellFoot(List<PrintCell> list, string BindingPhone, string OperatorName, string Remark, decimal? totalPQ, decimal? totalReliefAmount, decimal? totalAmount)
        {
            if (totalAmount < 0)
            {
                totalAmount = 0;
            }
            //list.Add(new PrintCell() { Title = "应缴：", Value = totalPQ.HasValue ? Math.Round(totalPQ.Value, 2).ToString() : "", Colspan = 2 });
            //list.Add(new PrintCell() { Title = "减免：", Value = totalReliefAmount.HasValue ? totalReliefAmount.Value.ToString() : "", Colspan = 2 });
            //list.Add(new PrintCell() { Title = "实缴：", Value = totalAmount.HasValue ? totalAmount.Value.ToString() : "", Colspan = 2 });
            list.Add(new PrintCell() { Title = "合计（大写）：", Value = totalAmount.HasValue ? PrintHelper.ToAmountUppercase(totalAmount.Value) : "", Colspan = 3 });
            list.Add(new PrintCell() { Title = "合计（小写）：", Value = totalAmount.HasValue ? totalAmount.Value.ToString() : "", Colspan = 3 });
            list.Add(new PrintCell() { Title = "备注：", Value = Remark, Colspan = 3 });
            list.Add(new PrintCell() { Title = "收费员：", Value = OperatorName, Colspan = 3 });
            //list.Add(new PrintCell() { Title = "物业电话：", Value = BindingPhone, Colspan = 6 });
            return list;
        }


        #region 打印数据分组
        public List<ReceiptPrintModel> ChargBillListGroup_OLD(List<ReceiptPrintModel> list)
        {
            try
            {

                var groups = list.GroupBy(o => o.SubjectId).ToList();
                List<ReceiptPrintModel> groupDataList;
                List<ReceiptPrintModel> listTemp = new List<ReceiptPrintModel>();
                List<ReceiptPrintModel> removeList = new List<ReceiptPrintModel>();
                List<ReceiptPrintModel> addList = new List<ReceiptPrintModel>();
                foreach (var group in groups)
                {
                    groupDataList = list.Where(o => o.SubjectId == group.Key).OrderBy(o => o.BeginDate).ToList();
                    if (groupDataList.Count > 1)
                    {
                        decimal Quantity = 0;
                        for (int i = 0; i < groupDataList.Count - 1; i++)
                        {
                            if (Convert.ToDateTime(groupDataList[i].EndDate).AddDays(1) == Convert.ToDateTime(groupDataList[i + 1].BeginDate))
                            {
                                if (!listTemp.Any(o => o.BillId == groupDataList[i].BillId))
                                {
                                    listTemp.Add(groupDataList[i]);
                                }

                                if (!listTemp.Any(o => o.BillId == groupDataList[i + 1].BillId))
                                {
                                    listTemp.Add(groupDataList[i + 1]);
                                }
                            }
                        }

                        if (listTemp.Count > 0)
                        {
                            ReceiptPrintModel model = listTemp.FirstOrDefault();
                            model.EndDate = listTemp.LastOrDefault().EndDate;
                            model.Amount = listTemp.Sum(o => o.Amount);
                            model.BillAmount = listTemp.Sum(o => o.BillAmount);
                            model.ReliefAmount = listTemp.Sum(o => o.ReliefAmount);
                            removeList.AddRange(listTemp);
                            listTemp.ForEach(o =>
                            {
                                if (o.RefType == (int)SubjectTypeEnum.Meter)
                                {
                                    Quantity = Quantity + Convert.ToDecimal(o.Quantity);
                                }
                            });
                            model.Quantity = Quantity.ToString();
                            addList.Add(model);
                            listTemp.Clear();
                        }
                    }
                }
                removeList.ForEach(m =>
                {
                    list.Remove(m);
                });
                list.AddRange(addList);
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<ReceiptPrintModel> ChargBillListGroup(List<ReceiptPrintModel> list)
        {
            List<ReceiptPrintModel> resultList = new List<ReceiptPrintModel>();

            //2.定义组List by 科目和房屋
            var groupList = (from m in list
                             group new { m.SubjectId, m.ResId, m.BillId } by new { m.SubjectId, m.ResId } into g
                             select new
                             {
                                 ChargeSubjectId = g.Key.SubjectId,
                                 ResourcesId = g.Key.ResId,
                                 Count = g.Count(m => m.BillId != null)
                             }).ToList();

            //3.循环组
            foreach (var item in groupList)
            {
                if (item.Count == 1)
                {
                    var pieceOfdata = list.Where(d => d.SubjectId == item.ChargeSubjectId && d.ResId == item.ResourcesId).First();
                    var temp = new List<ReceiptPrintModel>();
                    temp.Add(pieceOfdata);
                    SetGroupList(resultList, temp);
                }
                else
                {
                    //子组
                    var cgList = list.Where(d => d.SubjectId == item.ChargeSubjectId && d.ResId == item.ResourcesId).OrderBy(o => o.BeginDate).ToList();
                    List<ReceiptPrintModel> tempList = new List<ReceiptPrintModel>();
                    int count = cgList.Count() ;
                    for (int i = 0; i < count; i++)
                    {
                        //如果没有值
                        if (!cgList[i].BeginDate.HasValue)
                        {
                            //合并成一条
                            resultList.Add(cgList[i]);

                            //下一个组
                            tempList = new List<ReceiptPrintModel>();
                            if (i + 1 < count)
                            {
                                tempList.Add(cgList[i + 1]);
                            }
                            continue;
                        }

                        if (i == 0)
                        {
                            tempList.Add(cgList[i]);
                        }
              
                        if (i + 1 < count && cgList[i].EndDate.Value.AddDays(1) == cgList[i + 1].BeginDate.Value)
                        {
                            tempList.Add(cgList[i + 1]);
                        }
                        else
                        {
                            //设置分组 
                            if (tempList.Count() > 1)
                            {
                                SetGroupList(resultList, tempList);
                            }
                            else
                            {
                                resultList.AddRange(tempList);
                                if (cgList[i].RefType == (int)SubjectTypeEnum.Meter)
                                {
                                    SetGroupList(resultList, tempList);
                                }
                            }
                            //下一个组
                            tempList = new List<ReceiptPrintModel>();
                            if (i + 1 < count)
                            {
                                tempList.Add(cgList[i + 1]);
                            }
                        }

                    }
                    if (tempList.Count() > 0)
                    {
                        SetGroupList(resultList, tempList);
                    }
                }
            }
            return resultList;
        }
        private void SetGroupList(List<ReceiptPrintModel> resultList, List<ReceiptPrintModel> tempList)
        {
            List<string> listBillIds = new List<string>();
            ReceiptPrintModel model = tempList.FirstOrDefault();
            model.EndDate = tempList.LastOrDefault().EndDate;
            model.Amount = tempList.Sum(o => o.Amount);
            model.BillAmount = tempList.Sum(o => o.BillAmount);
            model.ReliefAmount = tempList.Sum(o => o.ReliefAmount);
            decimal meterNum = 0;
            for (int i = 0; i <= tempList.Count - 1; i++)
            {
                if (tempList[i].RefType == (int)SubjectTypeEnum.Meter)
                {
                    decimal quantiry = 0;
                    //fixed bug 3823 2017-03-21
                    //meterNum = meterNum + Convert.ToDecimal(tempList[i].Quantity);
                    if (decimal.TryParse(tempList[i].Quantity, out quantiry))
                    {
                        meterNum = meterNum + quantiry;
                    }
                }
            }
            for (int i = 0; i <= tempList.Count - 1; i++)
            {
                if (tempList[i].RefType == (int)SubjectTypeEnum.Meter)
                {
                    if (!listBillIds.Any(o => o.Contains(tempList[i].BillId)))
                    {
                        listBillIds.Add(tempList[i].BillId);
                    }
                }
            }
            if (listBillIds.Count > 0)
            {

                decimal num = MeterReadNum(model.ResId.Value, Convert.ToDateTime(model.BeginDate), Convert.ToDateTime(model.EndDate), listBillIds);
                if (num == 0 && meterNum == 0)
                {
                    model.Quantity = "";
                }
                else
                {
                    model.Quantity = num - Convert.ToDecimal(meterNum) + "-" + num;
                }
                
            }
            if (!resultList.Any(o => o.BillId == model.BillId))
            {
                resultList.Add(model);
            }

        }
        public decimal MeterReadNum(int meterId, DateTime beginDate, DateTime endDate, List<string> billIds)
        {
            MeterReadRecordAppService service = new MeterReadRecordAppService();
            var list = service.GetMeterRecords(meterId, beginDate.AddDays(-1), endDate, billIds).OrderBy(o => o.ReadDate).ToList();
            if (list != null && list.Count > 0)
            {
                return Convert.ToDecimal(list.LastOrDefault().MeterValue);
            }
            return 0;
        }
        #endregion

        /*模板一*/
        public PrintDataModel TemplateDataOne(ChargeRecord chargeRecord, ref int countData, ref int everyBillCount, ref int pageReceipt, ref int footSize, string communityName, string communityPhone, string propertyName)
        {
            footSize = 4;/*尾部单元格数*/
            pageReceipt = 1;/*每页显示几张收据*/
            int everyReceiptRow = 4;/*每张收据几行数据*/
            everyBillCount = everyReceiptRow * 6;/*每张收据显示条数*/
            var Community = DomainInterfaceHelper
               .LookUp<IPropertyDomainService>().GetGetCommunityById(chargeRecord.ComDeptId.Value);

            //var houseInfo = DomainInterfaceHelper
            // .LookUp<IPropertyDomainService>()
            // .GetHouseInfo(chargeRecord.ComDeptId.Value, chargeRecord.HouseDeptId.HasValue ? chargeRecord.HouseDeptId.Value : 0, true);

            //if (houseInfo == null)
            //{
            //    houseInfo = new HouseInfo();
            //}

            PrintDataModel data = new PrintDataModel(propertyName + " 专用收据      ");
            chargeRecord.HouseDeptNos = chargeRecord.HouseDeptNos ?? "";
            string houseNos = chargeRecord.HouseDeptNos.Length > 15 ? chargeRecord.HouseDeptNos.Substring(0, 15) + "..." : chargeRecord.HouseDeptNos;
            //当没有房号的车位时 显示 车位号 20170323
            string houseTitle = "房号：";
            if (chargeRecord.ChargeType != (int)ChargeTypeEnum.ForeignCharge && string.IsNullOrEmpty(houseNos))
            {
                houseNos = chargeRecord.ResourcesNames;
                houseTitle = "车位：";
            }
            //header
            data.PrintHeader.ColNum = 6;
            //float hw = PrintHelper.PageContentWidth / 6;
            data.PrintHeader.Widths = new float[] { 60, 110, 50, 110, 40, 110 };
            data.PrintHeader.PrintCellList.Add(new PrintCell() { Title = "管理小区：", HideBorder = true, Align = Element.ALIGN_RIGHT });
            data.PrintHeader.PrintCellList.Add(new PrintCell() { Value = Community.Name, HideBorder = true, Align = Element.ALIGN_LEFT });
            data.PrintHeader.PrintCellList.Add(new PrintCell() { Title = "日期：", HideBorder = true, Align = Element.ALIGN_RIGHT });
            data.PrintHeader.PrintCellList.Add(new PrintCell() { Value = DateTime.Now.ToString("yyyy年MM月dd日"), HideBorder = true, Align = Element.ALIGN_LEFT });
            data.PrintHeader.PrintCellList.Add(new PrintCell() { Title = "票号：", HideBorder = true, Align = Element.ALIGN_RIGHT });
            data.PrintHeader.PrintCellList.Add(new PrintCell() { Value = chargeRecord.Receipt.Number, HideBorder = true, Align = Element.ALIGN_LEFT });

            data.PrintHeader.PrintCellList.Add(new PrintCell() { Title = "客户：", HideBorder = true, Align = Element.ALIGN_RIGHT });
            data.PrintHeader.PrintCellList.Add(new PrintCell() { Value = chargeRecord.CustomerName == null ? "" : chargeRecord.CustomerName, HideBorder = true, Align = Element.ALIGN_LEFT });
            data.PrintHeader.PrintCellList.Add(new PrintCell() { Title = houseTitle, HideBorder = true, Align = Element.ALIGN_RIGHT });
            data.PrintHeader.PrintCellList.Add(new PrintCell() { Value = houseNos, HideBorder = true, Align = Element.ALIGN_LEFT });
            data.PrintHeader.PrintCellList.Add(new PrintCell() { Title = "电话：", HideBorder = true, Align = Element.ALIGN_RIGHT });
            data.PrintHeader.PrintCellList.Add(new PrintCell() { Value = Community.BindingPhone, HideBorder = true, Align = Element.ALIGN_LEFT });
            //body
            data.PrintBody.ColNum = 6;
            data.PrintBody.RowTitles = new string[] { "收费科目", "摘要", "单价", "应缴", "减免", "金额(元)" };
            data.PrintBody.Widths = new float[] { 100, 180, 50, 50, 50, 50 };
            var billList = ChargeRecordService.GetChargBillListByChargeRecordId(chargeRecord.Id);
            billList = ChargBillListGroup(billList);
            int billCount = billList.Count;
            foreach (var item in billList)
            {
                //if (item.ProjectDesc == "预存费")
                //{
                //    item.ProjectDesc = "物业管理费";
                //}
                item.PayMthodId = chargeRecord.PayMthodId.Value;
                data.PrintBody.AddRow(new string[]
                {
                    item.ProjectDesc,
                    item.AbstractDesc,
                    item.Price.HasValue? item.Price.ToString() : "",
                    item.BillAmount.HasValue? item.BillAmount.ToString() : "",
                    item.ReliefAmount.HasValue? item.ReliefAmount.ToString() : "",
                    item.Amount.HasValue? item.Amount.ToString() : ""
                });
            }

            if (billList.Count < everyReceiptRow)
            {
                /*不足3条*/
                GetPrintCellFoot(data.PrintBody.PrintFooterList, communityPhone, chargeRecord.OperatorName, chargeRecord.Receipt.Remark, billList.Sum(b => b.BillAmount), billList.Sum(b => b.ReliefAmount), billList.Sum(b => b.Amount));
                for (int i = 0; i < everyReceiptRow - billCount; i++)
                {
                    data.PrintBody.AddRow(new string[] { " ", " ", " ", " ", " ", " " });
                }
            }
            else
            {
                /*超过3条*/
                if (billCount % everyReceiptRow != 0)
                {
                    for (int i = 0; i < everyReceiptRow - billCount % everyReceiptRow; i++)
                    {
                        data.PrintBody.AddRow(new string[] { " ", " ", " ", " ", " ", " " });
                    }
                }

                //footer
                int pages = billCount % everyReceiptRow == 0 ? billCount / everyReceiptRow : billCount / everyReceiptRow + 1;
                for (int P = 1; P <= pages; P++)
                {
                    decimal billAmount = (decimal)billList.Skip((P - 1) * everyReceiptRow).Take(everyReceiptRow).Sum(o => o.BillAmount);
                    decimal reliefAmount = (decimal)billList.Skip((P - 1) * everyReceiptRow).Take(everyReceiptRow).Sum(o => o.ReliefAmount);
                    decimal amount = (decimal)billList.Skip((P - 1) * everyReceiptRow).Take(everyReceiptRow).Sum(o => o.Amount);
                    GetPrintCellFoot(
                        data.PrintBody.PrintFooterList,
                       communityPhone,
                        chargeRecord.OperatorName,
                        chargeRecord.Receipt.Remark,
                      billAmount,
                      reliefAmount,
                      amount
                        );
                }
            }
            if (billList == null)
            {
                countData = 0;
            }
            else
            {
                countData = data.PrintBody.PrintRowList.Count;
            }
            return data;

        }

        /*模板二*/
        public PrintDataModel TemplateDataTwo(ChargeRecord chargeRecord, ref int countData, ref int everyBillCount, ref int pageReceipt, ref int footSize, string propertyName, string number, string houseNo, string communityName, string phone)
        {
            footSize = 5;/*尾部单元格数*/
            everyBillCount = 12;/*每张收据显示条数*/
            pageReceipt = 1;/*每页显示几张收据*/
            int everyReceiptRow = 4;/*每张收据几行数据*/
            //var houseInfo = DomainInterfaceHelper
            //    .LookUp<IPropertyDomainService>()
            //    .GetHouseInfo(chargeRecord.ComDeptId.Value, chargeRecord.HouseDeptId.HasValue ? chargeRecord.HouseDeptId.Value : 0, true);
            //if (houseInfo == null)
            //{
            //    houseInfo = new HouseInfo();
            //}
            PrintDataModel data = new PrintDataModel(propertyName + "专用收据      ");
            //chargeRecord.HouseDeptNos = chargeRecord.HouseDeptNos ?? "";
            string houseNos = houseNo;
                //chargeRecord.HouseDeptNos.Length > 15 ? chargeRecord.HouseDeptNos.Substring(0, 15) + "..." : chargeRecord.HouseDeptNos;
            //当没有房号的车位时 显示 车位号 20170323
            string houseTitle = "房号：";
            if (chargeRecord.ChargeType != (int)ChargeTypeEnum.ForeignCharge && string.IsNullOrEmpty(houseNos))
            {
                houseNos = chargeRecord.ResourcesNames;
                houseTitle = "车位：";
            }
            //header
            data.PrintHeader.ColNum = 1;
            float hw = PrintHelper.PageContentWidth / 1;
            data.PrintHeader.Widths = new float[] { hw };
            data.PrintHeader.PrintCellList.Add(new PrintCell() { Value = string.Format("                       {0}                                                    No.{1}", DateTime.Now.ToString("yyyy年MM月dd日"), number), Colspan = 3, HideBorder = true });
            //body
            data.PrintBody.ColNum = 3;
            data.PrintBody.RotTitleTopList = new List<PrintCell>() { new PrintCell() { Colspan = data.PrintBody.ColNum, Value = "  项目：" + communityName + "                              "+ houseTitle + houseNos + "                  客户：" + chargeRecord.CustomerName + "" } };
            data.PrintBody.RowTitles = new string[] { "科目", "摘要", "金额(元)" };
            data.PrintBody.Widths = new float[] { 100, 250, 100 };
            var billList = ChargeRecordService.GetChargBillListByChargeRecordId(chargeRecord.Id);
            billList = ChargBillListGroup(billList);
            int billCount = billList.Count;

            foreach (var item in billList)
            {
                //if (item.ProjectDesc == "预存费")
                //{
                //    item.ProjectDesc = "物业管理费";
                //}
                item.PayMthodId = chargeRecord.PayMthodId.Value;
                data.PrintBody.AddRow(new string[]
                {
                  item.ProjectDesc,
                  item.AbstractDesc,
                  item.Amount.HasValue? item.Amount.ToString() : ""
                });
            }
            if (billList.Count < everyReceiptRow)
            { /*不足4条*/
                for (int i = 0; i < everyReceiptRow - billCount; i++)
                {
                    data.PrintBody.AddRow(new string[] { " ", " ", " " });
                }
            }
            else
            {
                /*超过4条*/
                if (billCount % everyReceiptRow != 0)
                {
                    for (int i = 0; i < everyReceiptRow - billCount % everyReceiptRow; i++)
                    {
                        data.PrintBody.AddRow(new string[] { " ", " ", " " });
                    }
                }
            }
            /*总共数据*/
            if (billList == null)
            {
                countData = 0;
            }
            else
            {
                countData = data.PrintBody.PrintRowList.Count;
            }
            //footer
            int pages = countData % everyBillCount == 0 ? countData / everyBillCount : countData / everyBillCount + 1;

            for (int P = 0; P < pages; P++)
            {
                decimal totalAmount = 0;
                decimal sum = 0;
                List<PrintCell> list = data.PrintBody.PrintRowList.Skip(P * everyBillCount).Take(everyBillCount).ToList();
                foreach (var item in list)
                {
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        if (decimal.TryParse(item.Value, out totalAmount))
                        {
                            sum = sum + totalAmount;
                        }

                    }
                }
                if (sum < 0)
                {
                    sum = 0;
                }
                data.PrintBody.PrintFooterList.Add(new PrintCell() { Title = "合计", Value = "", Colspan = 1 });
                data.PrintBody.PrintFooterList.Add(new PrintCell() { Title = "人民币大写： ", Value = sum != 0 ? PrintHelper.ToAmountUppercase(sum) : "", Colspan = 2 });
                data.PrintBody.PrintFooterList.Add(new PrintCell() { Title = "备注", Value = "", Colspan = 1 });
                data.PrintBody.PrintFooterList.Add(new PrintCell() { Title = "", Value = chargeRecord.Receipt.Remark, Colspan = 2 });
                data.PrintBody.PrintFooterList.Add(new PrintCell() { Title = "", Value = "            收款单位：" + propertyName + "            交款人：" + chargeRecord.CustomerName + "               收款人：" + chargeRecord.OperatorName + "                           ", Colspan = 3, HideBorder = true });
            }
            return data;
        }
        #endregion

        #region 套打模板

        public byte[] GetTemplatePrint(string chargeRecordId,int template)
        {
            TemplatePrintRecordAppService _TemplatePrintRecordAppService = new TemplatePrintRecordAppService();
            DeptAppService deptService = new DeptAppService();
            var chargeRecord = ChargeRecordService.GetChargeRecordAndReceiptByKey(chargeRecordId);
            var Community = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetGetCommunityById(chargeRecord.ComDeptId.Value);

            //生成TemplateprintModel

            var TemplateModel = GetChargeRecordToPrintModel(chargeRecord, Community.Name);
            int villageDeptId = Convert.ToInt32(chargeRecord.ComDeptId);
            var secModel = deptService.GetDeptInfoById(villageDeptId.ToString());
            int PropertyId = Convert.ToInt32(secModel.Code.Split('.')[1]);

           return  _TemplatePrintRecordAppService.GetTemplatePrintByte(TemplateModel, PropertyId, template);

           // return null;
        }


        private TemplatePrintModel GetChargeRecordToPrintModel(ChargeRecord chargeRecord,string CommunityName)
        {
            

            chargeRecord.HouseDeptNos = chargeRecord.HouseDeptNos ?? "";
            string houseNos = chargeRecord.HouseDeptNos.Length > 15 ? chargeRecord.HouseDeptNos.Substring(0, 15) + "..." : chargeRecord.HouseDeptNos;
            //当没有房号的车位时 显示 车位号 20170323
            if (chargeRecord.ChargeType != (int)ChargeTypeEnum.ForeignCharge && string.IsNullOrEmpty(houseNos))
            {
                houseNos = chargeRecord.ResourcesNames;
            }
          

            TemplatePrintModel templateprintmodel = new TemplatePrintModel()
            {
              
                CommunityName = CommunityName,
                ResourceName = houseNos,
                OwnerName = chargeRecord.CustomerName ?? "",
                YearCode = chargeRecord.PayDate.Value.Year.ToString(),
                MonthCode = chargeRecord.PayDate.Value.Month.ToString(),
                DayCode = chargeRecord.PayDate.Value.Day.ToString(),
                OperatorName = chargeRecord.OperatorName,
                TotalAmount = chargeRecord.Amount.ToString(),
                TotalAmountCN = PrintHelper.ToAmountUppercase(chargeRecord.Amount.ToString())
            };
            var billList = ChargeRecordService.GetChargBillListByChargeRecordId(chargeRecord.Id);
            billList = ChargBillListGroup(billList);
            templateprintmodel.FormList = GetBillListToTemplate(billList);
            return templateprintmodel;
        }

        private List<TemplatePrintFormDetail> GetBillListToTemplate(List<ReceiptPrintModel> list)
        {

            List<TemplatePrintFormDetail> Templatelist = new List<TemplatePrintFormDetail>();

            foreach (var item in list)
            {
                TemplatePrintFormDetail templateprintformdetail = new TemplatePrintFormDetail()
                {
                    ChargeSubjectName = item.ProjectDesc,
                    SummaryStr = item.AbstractDesc,
                    Amount = item.Amount.HasValue ? item.Amount.ToString() : ""   
                };
                Templatelist.Add(templateprintformdetail);
           }


            return Templatelist;
        }


        #endregion

     

        #endregion

        #region 账户详情

        public IList<BillChargeRecord> GetBillChargeRecordList(BillChargeRecordSearchDTO searchDto, out int totalCount)
        {
            //是否是开发商代缴
            bool isDeveloper = false;
            //是否对外收费
            bool isForegi = false;
            //查询是否属于外部收费
            if (searchDto.DeptType != EDeptType.FangWu&& searchDto.DeptType!=EDeptType.CheWei)
            {
                //如果查询某条收费记录的详细信息 并选中小区
                if (!string.IsNullOrEmpty(searchDto.RecordId) && searchDto.DeptType == EDeptType.XiaoQu)
                {
                    if (ChargeRecordService.CheckChargeReocordIsForegi(searchDto.RecordId))
                    {
                        isForegi = true;
                    }
                    else
                    {
                        isDeveloper = true;
                    }
                }
                else
                {
                    totalCount = 0;
                    return new List<BillChargeRecord>();
                }
            }
            //条件
            Condition<BillChargeRecord> condition = new Condition<BillChargeRecord>(c => true);
            if (!string.IsNullOrEmpty(searchDto.TransactionDesc))
            {
                condition = condition & new Condition<BillChargeRecord>(c => c.TransactionDesc.Contains(searchDto.TransactionDesc));
            }
            if (!string.IsNullOrEmpty(searchDto.ReceiptNum))
            {
                condition = condition & new Condition<BillChargeRecord>(c => c.ReceiptNum.Contains(searchDto.ReceiptNum));
            }
            if (searchDto.StartDate.HasValue)
            {
                condition = condition & new Condition<BillChargeRecord>(c => c.PayDate >= searchDto.StartDate);
            }
            if (searchDto.EndDate.HasValue)
            {
                searchDto.EndDate = searchDto.EndDate.Value.AddDays(1).AddSeconds(-1);
                condition = condition & new Condition<BillChargeRecord>(c => c.PayDate <= searchDto.EndDate);
            }
            if (searchDto.DeptId != 0)
            {
                if (searchDto.DeptType == EDeptType.CheWei)
                {   //车位
                    //注释,需要显示预存费 预存费是与房屋 2017-3-16
                    //condition = condition & new Condition<BillChargeRecord>(c => c.ResourcesId == searchDto.DeptId&&c.RefType== (int)ReourceTypeEnum.CarPark);
                }
                else
                { //非车位的情况
                    if (!isDeveloper && !isForegi)
                    {
                        condition = condition & new Condition<BillChargeRecord>(c => c.HouseDeptId == searchDto.DeptId);
                    }
                    else //如果是对外收费或开发商收费 添加小区的条件
                    {
                        condition = condition & new Condition<BillChargeRecord>(c => c.ComDeptId == searchDto.DeptId);
                    }
                }

            }
            if (!string.IsNullOrEmpty(searchDto.RecordId))
            {
                condition = condition & new Condition<BillChargeRecord>(c => c.Id == searchDto.RecordId);
            }
            //默认排序
            string expressions = "PayDate desc";
            return ChargeRecordService.GetBillChargeRecordList(searchDto.PageStart, searchDto.PageSize, condition.ExpressionBody, expressions, out totalCount);
        }

        public string GetBalanceAmountByHouseDeptId(int HouseDeptId, string Separator)
        {
            return ChargeRecordService.GetBalanceAmountByHouseDeptId(HouseDeptId, Separator);
        }

        public ChargeRecordDTO GetChargeRecordDiscountById(string Id)
        {
            var record = ChargeRecordService.GetChargeRecordDiscountById(Id);
            ///如果收费记录是退款 且有优惠券
            if (record.ChargeType == (int)ChargeTypeEnum.Refund)
            {
                record.Amount = record.Amount * -1;
                if (record.DiscountAmount != 0)
                {
                    ChargeRecordService.RefPaymentDiscountList(record);
                    record.DiscountAmount = record.DiscountAmount * -1;
                }
            }
            var recordDto = ChargeRecordMappers.ChangeChargeRecordToDTO(record);
            recordDto.PaymentDiscountDTOList = PaymentDiscountInfoMappers.ChangePaymentDiscountInfoToDTOs(record.PaymentDiscountList.ToList());
            var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
            var paytypeinfo = propertyService.GetDictionaryModels("PayType").Where(p => p.Code == recordDto.PayMthodId.ToString()).FirstOrDefault();
            if (paytypeinfo != null)
            {
                recordDto.PayTypeName = paytypeinfo.CnName;
            }
            return recordDto;
        }

        #endregion

        #region 交款费用记录查询
        public IList<BillChargeRecord> GetPaymentTasksBillChargeRecordList(BillChargeRecordSearchDTO searchDto, out int totalCount)
        {
            if (searchDto.DeptType != EDeptType.XiaoQu)
            {
                totalCount = 0;
                return new List<BillChargeRecord>();
            }
            //条件
            //条件
            Condition<BillChargeRecord> condition = new Condition<BillChargeRecord>(c => true);
            if (searchDto.EndDate.HasValue)
            {
                searchDto.EndDate = searchDto.EndDate.Value.AddDays(1).AddSeconds(-1);
                condition = condition & new Condition<BillChargeRecord>(c => c.PayDate <= searchDto.EndDate);
            }
            if (searchDto.DeptId != 0)
            {
                condition = condition & new Condition<BillChargeRecord>(c => c.ComDeptId == searchDto.DeptId);
            }
            //默认排序
            string expressions = "PayDate desc";
            return ChargeRecordService.GetPaymentTasksBillChargeRecordList(searchDto.PageStart, searchDto.PageSize, condition.ExpressionBody, expressions, out totalCount);
        }
        #endregion

        #region 交款明细查询
        public IList<ChargeRecordDTO> GetPaymentTasksDetailList(BillChargeRecordSearchDTO searchDto, out int totalCount)
        {
            try
            {
                if (searchDto.DeptType != EDeptType.XiaoQu|| searchDto.SECRole_AdminId<0)
                {
                    totalCount = 0;
                    return new List<ChargeRecordDTO>();
                }
                //条件
                //条件
                Condition<ChargeRecord> condition = new Condition<ChargeRecord>(c => true);
                IList<ChargeRecord> domainList = new List<ChargeRecord>();
                IList<ChargeRecordDTO> dtoList = new List<ChargeRecordDTO>();
                //   var limitHouseDeptInfos = DomainInterfaceHelper .LookUp<IPropertyDomainService>().GetHouDeptListByCommunityDeptId(searchDto.DeptId.ToString());//获取该小区房屋
                //默认排序
                string expressions = "PayDate desc";
                if (searchDto.PaymentTaskId > 0)
                {
                    domainList = ChargeRecordService.GetPaymentTasksDetailList(searchDto.PageStart, searchDto.PageSize, condition.ExpressionBody, expressions, out totalCount, (int)searchDto.PaymentTaskId);
                    dtoList = ChargeRecordMappers.ChangeChargeRecordToDTOs(domainList.ToList());
                    foreach (var item in dtoList)
                    {
                        item.ReceiptNum = domainList.Where(d => d.Id == item.Id).First().Receipt.Number;
                        item.ReceiptStatus = domainList.Where(d => d.Id == item.Id).First().Receipt.Status.Value;
                        //修改bug #2377
                        if (item.HouseDeptNos != null)
                            item.HouseDoorNoFormat = (item.HouseDeptNos.Length > 12 ? "<label style='font-weight:400' title='" + item.HouseDeptNos + "'>" + item.HouseDeptNos.Substring(0, 12) + "...</label>" : item.HouseDeptNos);
                        item.Amount = item.Amount;

                    }
                    return dtoList;
                }
                else if (searchDto.PaymentDateMax != null)
                {

                    if (searchDto.SECRole_AdminId > 0)
                        condition = condition & new Condition<ChargeRecord>(o => o.Operator == searchDto.SECRole_AdminId);

                    domainList = ChargeRecordService.GetPaymentTasksDetailList_Add(searchDto.PageStart, searchDto.PageSize, condition.ExpressionBody, expressions, out totalCount, ((DateTime)searchDto.PaymentDateMax), searchDto.DeptId);
                    dtoList = ChargeRecordMappers.ChangeChargeRecordToDTOs(domainList.ToList());
                    foreach (var item in dtoList)
                    {
                        item.ReceiptNum = domainList.Where(d => d.Id == item.Id).First().Receipt.Number;
                        item.ReceiptStatus = domainList.Where(d => d.Id == item.Id).First().Receipt.Status.Value;
                        if (item.HouseDeptNos != null)
                            item.HouseDoorNoFormat = (item.HouseDeptNos.Length > 12 ? "<label style='font-weight:400' title='" + item.HouseDeptNos + "'>" + item.HouseDeptNos.Substring(0, 12) + "...</label>" : item.HouseDeptNos);

                        //修改bug #2377
                        item.Amount = item.Amount;

                    }
                    return dtoList;
                }
                else
                {
                    totalCount = 0;
                    return new List<ChargeRecordDTO>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region 交款明细查询合计金额
        public decimal GetPaymentTasksDetailListById(DateTime? PayDateMax, int? PaymentTaskId, int? ComDeptId)
        {
            try
            {

                List<ChargeRecord> dtoList = new List<ChargeRecord>();
                if (PaymentTaskId != null && PaymentTaskId.Value > 0)
                    dtoList = ChargeRecordService.GetPaymentTasksDetailListById(PaymentTaskId.Value).ToList();
                else if (PayDateMax != null && PayDateMax.Value.Year > 1900)
                    dtoList = ChargeRecordService.GetPaymentTasksDetailListBySubject(ComDeptId.Value, PayDateMax.Value).ToList();
                var sum = dtoList.Sum(p => p.Amount);
                return sum == null ? 0 : sum.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取符合条件的交易记录
        public IList<BillChargeRecord> GetPaymentTasksDetailList(DateTime PaymentDateMax, int ComDeptId)
        {
            return ChargeRecordService.GetPaymentTasksDetailList_Save(PaymentDateMax, ComDeptId);
        }
        #endregion

        /// <summary>
        /// 根据收费记录获取打印模板
        /// </summary>
        /// <returns></returns>
        public int GetPrintTemplate(string chargeRecordId, ref string propertyName, ref string number, ref string houseNo)
        {
            var service = DomainInterfaceHelper.LookUp<IPropertyDomainService>();
            DeptAppService deptService = new DeptAppService();
            ReceiptAppService receiptService = new ReceiptAppService();
            var model = ChargeRecordService.GetChargeRecordByKey(chargeRecordId);
            var receiptModel = receiptService.GetReceiptByKey(model.ReceiptId);
            if (model.HouseDeptId.HasValue && model.HouseDeptId > 0)
            {
                houseNo = deptService.GetDeptInfoById(model.HouseDeptId.ToString()).Name;
                CommunityConfig CommunityConfig = new CommunityConfig();
                CommunityConfigAppService Service = new CommunityConfigAppService();
                CommunityConfig = Service.GetCommunityConfig(model.ComDeptId);
                string[] houseNosingle =new string[houseNo.Split('-').Count()];
                for (int i = 0; i < houseNo.Split('-').Count(); i++)
                {
                    houseNosingle[i]=houseNo.Split('-')[i];
                }
                if (!string.IsNullOrEmpty(houseNo))
                {
                    houseNo = "";
                    if (CommunityConfig.IsBuilding==true)
                    {
                        houseNo += houseNosingle[0]+'-';
                    }
                    if (CommunityConfig.IsUnit == true)
                    {
                        houseNo += houseNosingle[1]+'-';
                    }
                    if (CommunityConfig.IsFloor == true)
                    {
                        houseNo += houseNosingle[2]+'-';
                    }
                    if (CommunityConfig.IsNumber == true)
                    {
                        houseNo += houseNosingle[3];
                    }
                    houseNo = houseNo.TrimEnd('-');
                }
            }
            

            number = receiptModel.Number;
            int villageDeptId = Convert.ToInt32(model.ComDeptId);
            var secModel = deptService.GetDeptInfoById(villageDeptId.ToString());
            int propertyDeptId = Convert.ToInt32(secModel.PId);
            //var property = service.GetSECProperty(propertyDeptId);
            return (int)TemplateModelHelper.GetPrintTemplate(propertyDeptId, EPrintTemplate.BillTemplate, ref propertyName);
            //if (property == null)
            //{

            //    return (int)PrintTemplateEnum.TemplateOne;
            //}
            //else
            //{
            //    propertyName = property.PropertyName;
            //    return property.BillTemplate.HasValue ? Convert.ToInt32(property.BillTemplate) : (int)PrintTemplateEnum.TemplateOne;
            //}
        }

        #region 物业所有费用记录 2017-5-24

        public IEnumerable<TemplateModel> GetFullChargeRecordTemplate()
        {
            int i = 1;
            List<TemplateColumn> showColumns = new List<TemplateColumn>();
            showColumns.Add(new TemplateColumn() { ColumnName = "ResourcesNames", ColumnDesc = "收费资源", Seq = i++, IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "ChargeType", ColumnDesc = "收费类型", Seq = i++, DictId = PropertyEnumType.ChargeType.ToString(), IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "Amount", ColumnDesc = "收费金额", Seq = i++, IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "DiscountAmount", ColumnDesc = "优惠金额", Seq = i++, IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "PayDate", ColumnDesc = "收费日期", Seq = i++, Type = "date", IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "ReceiptNum", ColumnDesc = "票据号", Seq = i++, IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "CustomerName", ColumnDesc = "客户", Seq = i++, IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "OperatorName", ColumnDesc = "操作人", Seq = i++, IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "ReceiptStatus", ColumnDesc = "状态", Seq = i++, DictId = PropertyEnumType.ReceiptStatus.ToString(), IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "PayMthodId", ColumnDesc = "付款方式", Seq = i++, DictId = PropertyEnumType.PayType.ToString(), IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "RefundReason", ColumnDesc = "退款原因", Seq = i++, IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "Remark", ColumnDesc = "备注", Seq = i++, IsExport = true });

            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ChargeRecordDTO), showColumns.ToArray());
            return template;
        }

        public IList<ChargeRecordDTO> GetFullChargeRecordList(BillDetailSearchDTO searchDto, out int totalCount)
        {
            if (searchDto.DeptType != EDeptType.XiaoQu)
            {
                totalCount = 0;
                return new List<ChargeRecordDTO>();
            }
            Condition<ChargeRecordDTO> condition = new Condition<ChargeRecordDTO>(c => c.ComDeptId == searchDto.DeptId);
            if (!string.IsNullOrEmpty(searchDto.ResourcesName))
            {
                condition = condition & new Condition<ChargeRecordDTO>(c => c.ResourcesNames.Contains(searchDto.ResourcesName));
            }
            if (!string.IsNullOrEmpty(searchDto.ReceiptNum))
            {
                condition = condition & new Condition<ChargeRecordDTO>(c => c.ReceiptNum.Contains(searchDto.ReceiptNum));
            }
            if (searchDto.BillStatus.HasValue)
            {
                condition = condition & new Condition<ChargeRecordDTO>(c => c.ReceiptStatus == searchDto.BillStatus);
            }
            if (!string.IsNullOrEmpty(searchDto.CustomerName))
            {
                condition = condition & new Condition<ChargeRecordDTO>(c => c.CustomerName.Contains(searchDto.CustomerName));
            }
            if (!string.IsNullOrEmpty(searchDto.OperatorName))
            {
                condition = condition & new Condition<ChargeRecordDTO>(c => c.OperatorName.Contains(searchDto.OperatorName));
            }
            if (searchDto.PayType.HasValue)
            {
                condition = condition & new Condition<ChargeRecordDTO>(c => c.PayMthodId == searchDto.PayType);
            }

            if (searchDto.ChargeType.HasValue)
            {
                condition = condition & new Condition<ChargeRecordDTO>(c => c.ChargeType == searchDto.ChargeType);
            }
            if (searchDto.StartDate.HasValue)
            {
                condition = condition & new Condition<ChargeRecordDTO>(c => c.PayDate >= searchDto.StartDate);
            }
            if (searchDto.EndDate.HasValue)
            {
                searchDto.EndDate = searchDto.EndDate.Value.AddDays(1).AddSeconds(-1);
                condition = condition & new Condition<ChargeRecordDTO>(c => c.PayDate <= searchDto.EndDate);
            }

            string expressions = "PayDate desc";
            var dataList = ChargeRecordService.GetFullChargeRecordList(condition.ExpressionBody, expressions, out totalCount, searchDto.PageStart, searchDto.PageSize);
            return dataList;
        }

        #endregion

        #region 获取最后一次缴费记录

        public APILastChargeRecord GetLastChargeRecord(int? houseDeptId)
        {
            var chargeRecord = ChargeRecordService.GetLastChargeRecord(houseDeptId);
            if (chargeRecord == null)
            {
                return null;
            }
            APILastChargeRecord record = new APILastChargeRecord();
            record.Id = chargeRecord.Id;
            record.LastTime = chargeRecord.PayDate;
            record.Amount = chargeRecord.Amount;
            return record;
        }

        #endregion
    }
}
