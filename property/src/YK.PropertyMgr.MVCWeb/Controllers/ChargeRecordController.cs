using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.ApplicationMapper;
using YK.BackgroundMgr.Crosscuting;
using YK.BackgroundMgr.PresentationService;
using YK.FrameworkTools.ExcelService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.CompositeAppService;
using YK.PropertyMgr.Crosscuting;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class ChargeRecordController : BaseController
    {
        //
        // GET: /ChargeRecord/

        public ActionResult Index()
        {
            return View();
        }

        #region 费用记录

        public ActionResult ChargeRecordList(bool? IsShowHouse,bool? SettleAccount)
        {
            IsShowHouse = IsShowHouse ?? true;
            ChargeRecordListData listData = new ChargeRecordListData();
            listData.Language = this.Language;
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            listData.TemplateModels = chargeRecordAppService.GetChargeRecordListTemplate(IsShowHouse.Value, SettleAccount.Value);
            return View(listData);
        }
        /// <summary>
        /// 收费记录异步查询
        /// </summary>
        [HttpPost]
        public ActionResult GetChargeRecordList(ChargeRecordSearchDTO search)
        {
            int outCount = 0;
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            IList<ChargeRecordDTO> dataList = chargeRecordAppService.GetChargeRecordDTOList(search, out outCount);
            SearchResultData<ChargeRecordDTO> queryResult = new SearchResultData<ChargeRecordDTO>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };

            return Json(queryResult);
        }

        public ActionResult ForegiChargeRecordList(bool SettleAccount)
        {
            
            ChargeRecordListData listData = new ChargeRecordListData();
            listData.Language = this.Language;
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            listData.TemplateModels = chargeRecordAppService.GetForegiChargeRecordListTemplate(SettleAccount);
            return View(listData);
        }

        /// <summary>
        /// 收费记录异步查询
        /// </summary>
        [HttpPost]
        public ActionResult GetForegiChargeRecordList(ChargeRecordSearchDTO search)
        {
            int outCount = 0;
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            IList<ChargeRecordDTO> dataList = chargeRecordAppService.GetForgieChargeRecordDTOList(search, out outCount);
            SearchResultData<ChargeRecordDTO> queryResult = new SearchResultData<ChargeRecordDTO>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };

            return Json(queryResult);
        }


        #endregion

        #region 生成预结算
        /// <summary>
        /// 费用结算
        /// </summary>
        /// <param name="chargeRecordId"></param>
        /// <returns></returns>
        public ActionResult SettleAccount(string ChargeRecordId)
        {
            var resultModel = PaymentAppService.SettleAccount(ChargeRecordId);
            return Json(resultModel);
        }
        #endregion

        #region 退款

        /// <summary>
        /// 退款
        /// </summary>
        [HttpPost]
        public ActionResult Refund(RefundRecordDTO RefundRecord)
        {
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            var restult = chargeRecordAppService.Refund(RefundRecord, this.CurrentAdminUser.Id.Value, this.CurrentAdminUser.RealName);
            return Json(restult);
        }

        #endregion

        #region 修改

        /// <summary>
        /// 编辑视图
        /// </summary>
        public ActionResult ChargeRecordView(ChargeRecordDTO chargeRecordInfo)
        {
            if (chargeRecordInfo == null)
            {
                chargeRecordInfo = new ChargeRecordDTO();
            }
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            if (!string.IsNullOrEmpty(chargeRecordInfo.Id) && string.IsNullOrEmpty(chargeRecordInfo.ReceiptId))
            {
                var ValidationType = chargeRecordInfo.ValidationType;
                chargeRecordInfo = chargeRecordAppService.GetChargeRecordById(chargeRecordInfo.Id);
                chargeRecordInfo.ValidationType = ValidationType;
            }
            ChargeRecordViewData chargeRecordViewData = new ChargeRecordViewData();
            chargeRecordViewData.Language = this.Language;
            chargeRecordViewData.ChargeRecordInfo = chargeRecordInfo;
            chargeRecordViewData.TemplateModels = chargeRecordAppService.GetChargeRecordViewTemplate(chargeRecordInfo);
            return View(chargeRecordViewData);
        }

        /// <summary>
        /// 编辑外部收费视图
        /// </summary>
        public ActionResult ForegiChargeRecordView(ChargeRecordDTO chargeRecordInfo)
        {
            if (chargeRecordInfo == null)
            {
                chargeRecordInfo = new ChargeRecordDTO();
            }
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            if (!string.IsNullOrEmpty(chargeRecordInfo.Id) && string.IsNullOrEmpty(chargeRecordInfo.ReceiptId))
            {
                var ValidationType = chargeRecordInfo.ValidationType;
                chargeRecordInfo = chargeRecordAppService.GetChargeRecordById(chargeRecordInfo.Id);
                chargeRecordInfo.ValidationType = ValidationType;
            }
            ChargeRecordViewData chargeRecordViewData = new ChargeRecordViewData();
            chargeRecordViewData.Language = this.Language;
            chargeRecordViewData.ChargeRecordInfo = chargeRecordInfo;
            chargeRecordViewData.TemplateModels = chargeRecordAppService.GetForegiChargeRecordViewTemplate(chargeRecordInfo);
            return View("ChargeRecordView", chargeRecordViewData);
        }


        /// <summary>
        /// 修改
        /// </summary>
        [HttpPost]
        public ActionResult Update(ChargeRecordDTO ChargeRecord)
        {
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            if (chargeRecordAppService.IsSubmitted(ChargeRecord))
            {
                return Json(new ResultModel() { IsSuccess = false, Msg = "该收费记录已交款，不能修改" });
            }
            ChargeRecord.Operator = this.CurrentAdminUser.Id;
            ChargeRecord.OperatorName = this.CurrentAdminUser.RealName;
            bool bo = chargeRecordAppService.Update(ChargeRecord);
            ResultModel result = new ResultModel();
            result.IsSuccess = bo;
            if (bo)
            {
                result.Msg = "修改记录成功";
            }
            else
            {
                result.Msg = "修改记录失败";
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult ChargeRecordValidation(ChargeRecordDTO ChargeRecord)
        {
            switch (ChargeRecord.ValidationType)
            {
                case CRValidationEnum.Update:
                    {
                        ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
                        if (chargeRecordAppService.IsSubmitted(ChargeRecord))
                        {
                            return Json(new ResultModel() { IsSuccess = false, Msg = "该收费记录已交款，不能修改" });
                        }
                    }
                    break;
                default:
                    break;
            }

            return Json(new ResultModel() { IsSuccess = true, Msg = "验证成功！" });
        }

        #endregion

        #region 收费项目

        public ActionResult BillChargeRecordView(int? HouseDeptId, string RecordId, string ReceiptNum)
        {
            HouseDeptId = HouseDeptId ?? 0;
            BillChargeRecordViewData billChargeRecordViewData = new BillChargeRecordViewData();
            billChargeRecordViewData.Language = this.Language;
            billChargeRecordViewData.RecordId = RecordId;
            billChargeRecordViewData.ReceiptNum = ReceiptNum;
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            billChargeRecordViewData.TemplateModels = chargeRecordAppService.GetBillChargeRecordViewTemplate();
            //费用记录ID为空 为费用详情页面
            if (string.IsNullOrEmpty(RecordId))
            {
                billChargeRecordViewData.BalanceAmount = chargeRecordAppService.GetBalanceAmountByHouseDeptId(HouseDeptId.Value, " ");
            }
            //为费用记录 账单明细页面
            else
            {
                var record = chargeRecordAppService.GetChargeRecordDiscountById(RecordId);
                //费用记录信息
                billChargeRecordViewData.PayInfoDesc = record.PayTypeName+ ":¥" + (record.Amount).ToString();
                if (record.DiscountAmount > 0)
                {
                    //如存在优惠信息，费用记录金额-优惠金额
                    var amount = record.Amount - record.DiscountAmount;
                    amount = amount > 0 ? amount : 0;
                    billChargeRecordViewData.PayInfoDesc = record.PayTypeName + ":¥" + (amount).ToString();
                    //优惠券信息
                    var discountInfoDesc = string.Empty;
                    foreach (var item in record.PaymentDiscountDTOList)
                    {
                        switch (item.EDiscountType)
                        {
                            case ApplicationDTO.Enums.DiscountTypeEnum.Coupon:
                                {
                                    discountInfoDesc += " 优惠券（" + item.DiscountDesc + "):¥" + item.DiscountAmount.ToString();
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    billChargeRecordViewData.DiscountInfoDesc = discountInfoDesc;
                }
            }
            return View(billChargeRecordViewData);
        }

        [HttpPost]
        public ActionResult GetBillChargeRecordViewList(BillChargeRecordSearchDTO search)
        {
            int outCount = 0;
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            IList<BillChargeRecord> dataList = chargeRecordAppService.GetBillChargeRecordList(search, out outCount);
            SearchResultData<BillChargeRecord> queryResult = new SearchResultData<BillChargeRecord>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };

            return Json(queryResult);
        }

        #endregion

        #region 票据补打

        /// <summary>
        /// 视图
        /// </summary>
        public ActionResult ReceiptPrintView(ChargeRecordDTO chargeRecordInfo)
        {
            if (chargeRecordInfo == null)
            {
                chargeRecordInfo = new ChargeRecordDTO();
            }
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            if (!string.IsNullOrEmpty(chargeRecordInfo.Id) && string.IsNullOrEmpty(chargeRecordInfo.ReceiptId))
            {
                chargeRecordInfo = chargeRecordAppService.GetChargeRecordById(chargeRecordInfo.Id);
            }
            ChargeRecordViewData chargeRecordViewData = new ChargeRecordViewData();
            chargeRecordViewData.Language = this.Language;
            chargeRecordViewData.ChargeRecordInfo = chargeRecordInfo;
            //票据号为空 可以编辑
            //if (string.IsNullOrEmpty(chargeRecordInfo.ReceiptNum))
            //{
            //    chargeRecordViewData.CanEditReceiptNum = true;
            //}
            chargeRecordViewData.TemplateModels = chargeRecordAppService.GetChargeRecordViewTemplate(chargeRecordInfo);
            return View(chargeRecordViewData);
        }

        /// <summary>
        /// 外部费用视图
        /// </summary>
        public ActionResult ForegiReceiptPrintView(ChargeRecordDTO chargeRecordInfo)
        {
            if (chargeRecordInfo == null)
            {
                chargeRecordInfo = new ChargeRecordDTO();
            }
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            if (!string.IsNullOrEmpty(chargeRecordInfo.Id) && string.IsNullOrEmpty(chargeRecordInfo.ReceiptId))
            {
                chargeRecordInfo = chargeRecordAppService.GetChargeRecordById(chargeRecordInfo.Id);
            }
            ChargeRecordViewData chargeRecordViewData = new ChargeRecordViewData();
            chargeRecordViewData.Language = this.Language;
            chargeRecordViewData.ChargeRecordInfo = chargeRecordInfo;
            chargeRecordViewData.TemplateModels = chargeRecordAppService.GetForegiChargeRecordViewTemplate(chargeRecordInfo);
            return View("ReceiptPrintView", chargeRecordViewData);
        }


        /// <summary>
        /// 补打功能
        /// </summary>
        [HttpPost]
        public ActionResult ReceiptPrint(ChargeRecordDTO ChargeRecord)
        {
            //if (string.IsNullOrEmpty(ChargeRecord.ReceiptNum))
            //{
            //    return Json(new ResultModel() { IsSuccess = false, Msg = "票据号不能为空" });
            //}
            //验证票据号基于物业唯一
            //if (PaymentAppService.CheckReceiptNumRepeat(ChargeRecord.ReceiptNum, ChargeRecord.ComDeptId.Value, ChargeRecord.ReceiptId))
            //{
            //    return Json(new ResultModel() { IsSuccess = false, Msg = "票据号不能重复" });
            //}
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            ChargeRecord.Operator = this.CurrentAdminUser.Id;
            ChargeRecord.OperatorName = this.CurrentAdminUser.RealName;
            var bo = chargeRecordAppService.ReceiptPrint(ChargeRecord);
            ResultModel result = new ResultModel();
            result.IsSuccess = bo.IsSuccess;
            if (bo.IsSuccess)
            {
                result.Data = new { ChargeRecordId = ChargeRecord.Id };
                result.Msg = "票据打印成功";
            }
            else
            {
                result.Msg = bo.Msg;
            }
            return Json(result);
        }

        public ActionResult ReceiptPrintPDF(string chargeRecordId)
        {
            string propertyName = string.Empty;
            string number = string.Empty;
            string houseNo = string.Empty;
            int countData = 0;
            int everyBillCount = 0;
            int pageReceipt = 0;
            int footSize = 0;

            try
            {
                byte[] byteOutputStream = null;
                ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
                int template = chargeRecordAppService.GetPrintTemplate(chargeRecordId, ref propertyName, ref number, ref houseNo);/*根据缴费记录获取模板、物业、缴费人*/
                PrintDataModel data = chargeRecordAppService.GetPrintDataModel(chargeRecordId, ref countData, ref everyBillCount, ref pageReceipt, ref footSize, template, propertyName, number, houseNo);
                if (template==1)
                {
                    if (!string.IsNullOrEmpty(houseNo))
                    {
                        for (int i = data.PrintHeader.PrintCellList.Count() - 1; i > 0; i--)
                        {
                            if (!string.IsNullOrEmpty(data.PrintHeader.PrintCellList[i].Title))
                            {
                                if (data.PrintHeader.PrintCellList[i].Title.Contains("房号"))
                                {
                                    data.PrintHeader.PrintCellList[i + 1].Value = houseNo;
                                    break;
                                }
                            }
                        }
                    }
                }
               
                if (template>100)
                {
                    byteOutputStream= chargeRecordAppService.GetTemplatePrint(chargeRecordId, template);
                }
                else
                {
                    byteOutputStream = PrintHelper.GetPdfFileStream(data, countData, everyBillCount, pageReceipt, footSize);
                }
               
                Response.AddHeader("Content-Length", byteOutputStream.Length.ToString());
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "inline;FileName=out.pdf");
                Response.BinaryWrite(byteOutputStream);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile("ReceiptPrintPDF 异常：" + ex.Message, "ReceiptPrintPDF", FileLogType.Exception);
            }
            return View();
        }
        #endregion

        #region 物业所有费用记录 2017-05-24

        public ActionResult FullChargeRecordListIndex(int? DeptId, EDeptType? DeptType, string DeptName)
        {
            ChargeRecordAppService service = new ChargeRecordAppService();
            FullChargeRecordListData data = new FullChargeRecordListData();
            data.TemplateModels = service.GetFullChargeRecordTemplate();
            return View(data);
        }

        [HttpPost]
        public ActionResult GetFullChargeList(BillDetailSearchDTO search)
        {
            int outCount = 0;
            ChargeRecordAppService service = new ChargeRecordAppService();
            IList<ChargeRecordDTO> dataList = service.GetFullChargeRecordList(search, out outCount);
            SearchResultData<ChargeRecordDTO> queryResult = new SearchResultData<ChargeRecordDTO>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };

            return Json(queryResult);
        }

        public void ExportData(BillDetailSearchDTO search)
        {
            search.PageSize = int.MaxValue;
            int outCount = 0;
            ChargeRecordAppService service = new ChargeRecordAppService();
            IList<ChargeRecordDTO> dataList = service.GetFullChargeRecordList(search, out outCount);
            var tmodules = TemplateModelsMapper.ChangeTemplateModelToDTOs(service.GetFullChargeRecordTemplate());
            var exprotResult = ExcelHelper.Export<ChargeRecordDTO>(dataList, tmodules);
            ExportExcel("费用记录" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", exprotResult.SaveToStream().ToArray());
        }

        #endregion
    }

    public class ChargeRecordListData
    {
        public string Language { get; set; }
        public IEnumerable<TemplateModel> TemplateModels { get; set; }

    }

    public class ChargeRecordViewData
    {
        public string Language { get; set; }
        public ChargeRecordDTO ChargeRecordInfo { get; set; }
        public IEnumerable<TemplateModel> TemplateModels { get; set; }

        public bool CanEditReceiptNum { get; set; }
    }

    public class BillChargeRecordViewData
    {
        public string Language { get; set; }
        public string BalanceAmount { get; set; }
        public string RecordId { get; set; }
        public string ReceiptNum { get; set; }
        public IEnumerable<TemplateModel> TemplateModels { get; set; }

        public string PayInfoDesc{ get; set; }
        public string DiscountInfoDesc { get; set; }
    }

    public class FullChargeRecordListData
    {
        public List<object> BillStatusList { get; set; }

        public string Language { get; set; }

        public IEnumerable<TemplateModel> TemplateModels { get; set; }
    }
}