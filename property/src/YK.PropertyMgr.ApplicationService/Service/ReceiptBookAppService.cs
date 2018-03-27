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
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.CompositeDomainService;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using YK.PropertyMgr.DomainService;
using YK.PropertyMgr.ApplicationMapper;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace YK.PropertyMgr.ApplicationService
{
   public  partial class ReceiptBookAppService
    {
        public IEnumerable<TemplateModel> GetReceiptBookListTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "Name", ColumnDesc = "票据名称", Seq = i++,DictId="-1"},
                new TemplateColumn(){ ColumnName = "ReceiptBookType", ColumnDesc = "票据类型", Seq = i++,DictId = PropertyEnumType.ReceiptBookType.ToString()},
                new TemplateColumn(){ ColumnName = "Prefix", ColumnDesc = "前缀", Seq = i++,DictId="-1"},
                new TemplateColumn(){ ColumnName = "Suffix", ColumnDesc = "后缀位数", Seq = i++,DictId="-1"},
                new TemplateColumn(){ ColumnName = "BeginCode", ColumnDesc = "起号", Seq = i++,DictId="-1"},
                new TemplateColumn(){ ColumnName = "EndCode", ColumnDesc = "止号", Seq = i++,DictId="-1"},
                new TemplateColumn(){ ColumnName = "receiptCurrentNumberGridView", ColumnDesc = "当前票据号", Seq = i++,DictId="-1"},
                new TemplateColumn(){ ColumnName = "TotalNumber", ColumnDesc = "份数", Seq = i++,DictId="-1"},
                new TemplateColumn(){ ColumnName = "UsedNumber", ColumnDesc = "已使用", Seq = i++,DictId="-1"},
                new TemplateColumn(){ ColumnName = "Status", ColumnDesc = "状态", Seq = i++,DictId=PropertyEnumType.ReceiptBookTypeStatus.ToString()}

            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ReceiptBookDTO), showColumns);
            return template;
        }

        public IList<ReceiptBookDTO> GetReceiptBookDTOList(ReceiptBookSearchDTO searchDto, out int totalCount)
        {
            ReceiptBookDomainService _ReceiptBookDomainService = new ReceiptBookDomainService();
            Condition<ReceiptBook> condition = new Condition<ReceiptBook>(c => c.IsDel==false&&c.DeptId==searchDto.DeptId);
            if(!string.IsNullOrEmpty(searchDto.Name))
            {
                condition = condition & new Condition<ReceiptBook>(c=>c.Name.Contains(searchDto.Name));
            }
            if (searchDto.RceciptType != null)
            {
                condition = condition & new Condition<ReceiptBook>(c => c.ReceiptBookType== searchDto.RceciptType);
            }
            if (searchDto.Status != null)
            {
                condition = condition & new Condition<ReceiptBook>(c => c.Status == searchDto.Status);
            }
            string expressions = "CreateTime desc";
            var ReceiptBookList=  _ReceiptBookDomainService.GetReceiptBookList(searchDto.PageStart, searchDto.PageSize, condition.ExpressionBody, expressions, out totalCount);

            return ReceiptBookMappers.ChangeReceiptBookToDTOs(ReceiptBookList.ToList());
            
        }


        #region 新增票据
        /// <summary>
        /// 新增科目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnResult InsertReceiptBookDTO(ReceiptBookDTO model,string OperatorName)
        {
            ReturnResult res = new ReturnResult();
            try
            {
                ReceiptBookDomainService _ReceiptBookDomainService = new ReceiptBookDomainService();
                var receiptbook= ReceiptBookMappers.ChangeDTOToReceiptBookNew(model);
                ReceiptBookHistory receiptbookhistory = new ReceiptBookHistory();
                model.IsDel = false;
                model.CreateTime = DateTime.Now;
                model.UpdateTime = DateTime.Now;
                model.ReceiptAmount = 0;
                model.ReceiptBookType = Convert.ToInt32(model.ReceiptBookTypeStr);
                model.Status = Convert.ToInt32(model.StatusStr);
                
                model.CurrentReceiptNum = BillCommonService.Instance.HandleCurrendReceiptBookNumber(model);
                model.TotalNumber = (model.EndCode.Value - model.BeginCode.Value) + 1;
                model.InvalidNumber = 0;
                model.UsedNumber = 0;
                GenerateReceiptHistory(receiptbookhistory, model, OperatorName);
                receiptbook = ReceiptBookMappers.ChangeDTOToReceiptBookNew(model);
                if (model.Status == (int)ReceiptBookStatusEnum.Enabled)
                    ModifyOtherReceiptDisable(model);
                bool isSuccess = _ReceiptBookDomainService.InserReceiptBookAndHistory(receiptbook, receiptbookhistory);
                if (isSuccess)
                {
                    res.IsSuccess = true;
                    res.Msg = "处理成功!";
                    return res;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Msg = "处理失败!";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Msg = " 数据异常!"+ex;
                return res;
            }

        }
        #endregion


        #region 票据更新
        /// <summary>
        /// 新增科目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnResult EditReceiptBookDTO(ReceiptBookDTO model, string OperatorName)
        {
            ReturnResult res = new ReturnResult();
            try
            {
                ReceiptBookDomainService _ReceiptBookDomainService = new ReceiptBookDomainService();
                model.Status = Convert.ToInt32(model.StatusStr);
                model.ReceiptBookType = Convert.ToInt32(model.ReceiptBookTypeStr);
                model.UpdateTime = DateTime.Now;
                model.CurrentReceiptNum = BillCommonService.Instance.HandleCurrendReceiptBookNumber(model);
                var receiptbook = ReceiptBookMappers.ChangeDTOToReceiptBookNew(model);
                ReceiptBookHistory receiptbookhistory = new ReceiptBookHistory();
                receiptbook.TotalNumber = (receiptbook.EndCode.Value - receiptbook.BeginCode.Value) + 1;
                GenerateReceiptHistory(receiptbookhistory, model, OperatorName);
                if (model.Status == (int)ReceiptBookStatusEnum.Enabled)
                    ModifyOtherReceiptDisable(model);
                bool isSuccess = _ReceiptBookDomainService.UpdateReceiptBookAndHistory(receiptbook, receiptbookhistory);
                if (isSuccess)
                {
                    res.IsSuccess = true;
                    res.Msg = "处理成功!";
                    return res;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Msg = "处理失败!";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Msg = " 数据异常!";
                return res;
            }

        }
        #endregion

        #region 票据号常规检测

        public ReturnResult RoutineTestingModifyReceiptBookDTO(ReceiptBookModifyDTO model, string OpratorName)
        {
            ReturnResult res = new ReturnResult();
            try
            {
                ReceiptBookDetailAppService _ReceiptBookDetailAppService = new ReceiptBookDetailAppService();
                ReceiptBookDomainService _ReceiptBookDomainService = new ReceiptBookDomainService();
                Condition<ReceiptBook> condition = new Condition<ReceiptBook>(c => c.IsDel == false && c.DeptId == model.ComDeptId && c.ReceiptBookType == model.ReceiptBookType);

                var ReceiptBookModel = _ReceiptBookDomainService.GetReceiptBookByNumber(condition.ExpressionBody, model.OldReceiptNum);
               
                //获取新票据号的票据本。进行判断
                var NewReceiptBookModel = _ReceiptBookDomainService.GetReceiptBookByNumber(condition.ExpressionBody, model.NewReceiptNum);

                condition = condition & new Condition<ReceiptBook>(o => o.Status == (int)ReceiptBookStatusEnum.Enabled);
                //获取当前小区启用票据
                var  CurrentReceiptBook = _ReceiptBookDomainService.GetReceiptBookList(condition.ExpressionBody).FirstOrDefault();

                if (NewReceiptBookModel != null&& CurrentReceiptBook!=null&&(CurrentReceiptBook.Id!=NewReceiptBookModel.Id))
                {
                    res.IsSuccess = false;
                    res.Msg = "新票据号所在票据本未启用";
                    res.Data = 1;//正常弹出错误
                    return res;
                }

                if (ReceiptBookModel == null || ReceiptBookModel.Id == 0)
                {
                    res.IsSuccess = false;
                    res.Msg = "未能找到票据号：" + model.OldReceiptNum + "所在的票据本，请检查票据号";
                    res.Data = 1;//正常弹出错误
                    return res;
                }
                var taskResult = BillCommonService.Instance.AuthenticationReBookHasPaymentTasks(model.OldReceiptNum, model.NewReceiptNum);
                if (!taskResult.IsSuccess)
                {
                    res.IsSuccess = false;
                    res.Msg = taskResult.Msg;
                    res.Data = 1;//正常弹出错误
                    return res;
                }

                //验证该小区是否有修改的票据号
                if (!_ReceiptBookDetailAppService.ExistReceiptBookNum(model.OldReceiptNum, model.ComDeptId, model.ReceiptBookType))
                {

                    res.IsSuccess = false;
                    res.Msg = "票据号：" + model.OldReceiptNum + "不存在，请重新输入";
                    res.Data = 1;//正常弹出错误
                    return res;
                }


                condition = new Condition<ReceiptBook>(c => c.IsDel == false && c.DeptId == model.ComDeptId && c.Status == (int)ReceiptBookStatusEnum.Enabled && c.ReceiptBookType == model.ReceiptBookType);
                var EnabledReceiptBookModel = _ReceiptBookDomainService.GetReceiptBookList(condition.ExpressionBody).FirstOrDefault();
                if (EnabledReceiptBookModel == null)
                {
                    res.IsSuccess = false;
                    res.Msg = "该小区未有启用的票据本";
                    res.Data = 1;//正常弹出错误
                    return res;
                }


                string msg = "";
                if (!VerificationReceiptNumFormat(model.NewReceiptNum, EnabledReceiptBookModel, ref msg))
                {
                    res.IsSuccess = false;
                    res.Msg = msg;
                    res.Data = 1;//正常弹出错误
                    return res;
                }

                if (!model.IsModify)
                {
                    //新票据号存在，提示
                    if (_ReceiptBookDetailAppService.ExistReceiptBookNum(model.NewReceiptNum, model.ComDeptId, model.ReceiptBookType))
                    {

                        res.IsSuccess = true;
                        res.Msg = "新票据号已存在，如果确定修改，会将两个票据号进行交换";
                        res.Data = 2;//需要用户确认
                        return res;
                    }
                    else
                    {//新票据号”不存在，并且超出/小于 当前票据号
                        var ReceiptBook = EnabledReceiptBookModel;
                        var CurrentNumber = 0;
                        var NowSNumber = 0;
                        if (ReceiptBook.CurrentReceiptNum == "")
                        {
                            res.IsSuccess = true;
                            res.Msg = "新票据号与当前票据号不符，如果确定修改，当前票据号与新票据号之间的票号将不能再使用";
                            res.Data = 2;//需要用户确认
                            return res;

                        }
                        else
                        {
                            CurrentNumber = BillCommonService.Instance.ReceiptNumberToIntNumber(ReceiptBook);

                            NowSNumber = BillCommonService.Instance.ReceiptNumberToIntNumber(ReceiptBook, model.NewReceiptNum) - 1;
                        }
                        if (NowSNumber > CurrentNumber)
                        {
                            res.IsSuccess = true;
                            res.Msg = "新票据号与当前票据号不符，如果确定修改，当前票据号与新票据号之间的票号将不能再使用";
                            res.Data = 2;//需要用户确认
                            return res;

                        }
                        else if (NowSNumber < CurrentNumber)
                        {
                            res.IsSuccess = false;
                            res.Msg = "新票据号未使用且小于当前票据号，不能再使用";
                            res.Data = 1;//正常弹出错误
                            return res;
                        }

                    }
                }

                return ModifyReceiptBookDTO(model, EnabledReceiptBookModel, OpratorName, ReceiptBookModel.Id.Value);


            }
            catch (Exception)
            {
                res.IsSuccess = false;
                res.Msg = " 数据异常,请联系工作人员";
                return res;
            }
        }


        /// <summary>
        /// 票据号修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnResult ModifyReceiptBookDTO(ReceiptBookModifyDTO model,ReceiptBook receiptbook,string OperatorName,int oldReceiptBookId=0)
        {
            ReturnResult res = new ReturnResult();
            ReceiptBookDetailAppService _ReceiptBookDetailAppService = new ReceiptBookDetailAppService();
            ReceiptBookDomainService _ReceiptBookDomainService = new ReceiptBookDomainService();
            try
            {
                //验证是否是交换、如果不是则直接保存。如果是则开始进行交换

                if (_ReceiptBookDetailAppService.ExistReceiptBookNum(model.NewReceiptNum, model.ComDeptId, model.ReceiptBookType))
                {
                    model.IsExChange = true;

                }
                else
                {
                    //进行修改
                    if(receiptbook.CurrentReceiptNum=="")
                        model.IsCurrent = true;
                    else
                    {
                        var CurrentNumber = BillCommonService.Instance.ReceiptNumberToIntNumber(receiptbook);
                        var NowSNumber = BillCommonService.Instance.ReceiptNumberToIntNumber(receiptbook, model.NewReceiptNum);
                        if (NowSNumber > CurrentNumber)
                        {    //有新的当前号码
                            model.IsCurrent = true;

                        }
                    }

                  


                }
                BillCommonService.Instance.ReceiptBookModifyNumber(model, receiptbook, OperatorName, oldReceiptBookId);

                res.IsSuccess = true;
                res.Msg = "修改成功";
                res.Data = 1;//正常弹出错误
                return res;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                res.IsSuccess = false;
                res.Msg = "错误";
                res.Data = 1;//正常弹出错误
                return res;
            }

        }
        #endregion

        #region 验证票据号格式

        /// <summary>
        /// 验证票据号格式
        /// </summary>
        /// <returns></returns>
        private bool VerificationReceiptNumFormat(string Number,ReceiptBook receiptbook, ref string msg)
        {
            return BillCommonService.Instance.VerificationReceiptNumFormat(Number, receiptbook, ref msg);
            //if (receiptbook.BeginCode > receiptbook.EndCode)
            //{
            //    msg = "起号不能大于止号";
            //    return false;
            //}

            ////1.验证前缀
            //var poPrefixstr = Number.Substring(0, receiptbook.Prefix.Length);
            //if (poPrefixstr != receiptbook.Prefix)
            //{//前缀验证不通过
            //    msg = "前缀验证不通过";
            //    return false;
            //}
            ////2.验证后缀位数
            //string str1 = Number.Remove(0, receiptbook.Prefix.Length);
            //if (receiptbook.Suffix.Value != str1.Length)
            //{
            //    msg = "后缀位数验证不通过";
            //    return false;
            //}
            ////验证后缀是否在范围内
            //try
            //{
            //    int SuffixInt = Convert.ToInt32(str1);
            //    if (SuffixInt < receiptbook.BeginCode || SuffixInt > receiptbook.EndCode)
            //    {
            //        msg = "当前票据不在范围内";
            //        return false;
            //    }
            //}
            //catch (Exception)
            //{
            //    msg = "票据位："+str1+"不合法，请输入正确的格式";
            //    return false;
            //}
            //return true;
        }
        #endregion

        #region 验证当前票据号是否符合规律
        public ReturnResult VerifyCurrentReceiptNumber(ReceiptBookDTO model)
        {
            var ReceiptBookModel = ReceiptBookMappers.ChangeDTOToReceiptBookNew(model);

            ReturnResult res = new ReturnResult();
            string msg = string.Empty;
            if (!VerificationReceiptNumFormat(model.CurrentReceiptNum, ReceiptBookModel, ref msg))
            {
                res.IsSuccess = false;
                res.Msg = msg;
                return res;
            }
            var NowNumber = BillCommonService.Instance.ReceiptNumberToIntNumber(ReceiptBookModel);
 
            


            if (model.Id != null && model.Id > 0)
            {//修改

               
                var ReceiptBookModelDataBase = ReceiptBookMappers.ChangeDTOToReceiptBookNew(GetReceiptBookByKey(model.Id));
                if (model.CurrentReceiptNum != ReceiptBookModelDataBase.CurrentReceiptNum)
                {
                    //获取修改票据号的数字位
                    int DataBaseNumber = 0;
                    //获取数据库票据号的数字位
                    if (!string.IsNullOrEmpty(ReceiptBookModelDataBase.CurrentReceiptNum))
                      DataBaseNumber = BillCommonService.Instance.ReceiptNumberToIntNumber(ReceiptBookModelDataBase);

                    if (NowNumber < DataBaseNumber)
                    {
                        if(ReceiptBookModelDataBase.BeginCode== model.BeginCode&& ReceiptBookModelDataBase.UsedNumber>0)
                        { 
                            res.IsSuccess = false;
                            res.Msg = "当前票据号不能小于" + ReceiptBookModelDataBase.CurrentReceiptNum;
                            return res;
                        }
                    }
                  
                }
                res.IsSuccess = true;
                res.Msg = "处理成功!";
                return res;
            }
            if (NowNumber > model.EndCode.Value)
            {
                res.IsSuccess = false;
                res.Msg = "当前票据号不能大于票据结束号";
                return res;
            }
            if (NowNumber < model.BeginCode.Value)
            {
                res.IsSuccess = false;
                res.Msg = "当前票据号不能小于票据起始号";
                return res;
            }
            if (model.BeginCode.Value > model.EndCode)
            {
                res.IsSuccess = false;
                res.Msg = " 票据起始号不能大于票据结束号";
                return res;
            }

            res.IsSuccess = true;
            res.Msg = "";
            return res;




        }

        #endregion

        #region 判断该小区是否有启用的票据
        public bool CheckRecepitBookStatusByComDeptId(int ComDeptId,int ReceiptBookType)
        {

            ReceiptBookDomainService _ReceiptBookDomainService = new ReceiptBookDomainService();
            Condition<ReceiptBook> condition = new Condition<ReceiptBook>(c => c.IsDel == false&&c.DeptId== ComDeptId&&c.Status== (int)ReceiptBookStatusEnum.Enabled&&c.ReceiptBookType== ReceiptBookType);
            var reList=   _ReceiptBookDomainService.GetReceiptBookList(condition.ExpressionBody);
            if (reList.Count > 0)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region 将该小区其他同类型票据修改为停用
        public void ModifyOtherReceiptDisable(ReceiptBookDTO model)
        {
           
            if (model.Id == null && model.Id == 0)
            {
                model.Id = 0;
            }
            ReceiptBookDomainService _ReceiptBookDomainService = new ReceiptBookDomainService();
            Condition<ReceiptBook> condition = new Condition<ReceiptBook>(c => c.IsDel == false && c.DeptId == model.DeptId && c.Status == (int)ReceiptBookStatusEnum.Enabled && c.ReceiptBookType == model.ReceiptBookType);
            var reList = _ReceiptBookDomainService.GetReceiptBookList(condition.ExpressionBody).Where(o=>o.Id!=model.Id);
            foreach (var a in reList)
            {
                a.Status = (int)ReceiptBookStatusEnum.Disabled;
                 var receiptModelDTO=   ReceiptBookMappers.ChangeReceiptBookToDTO(a);
                UpdateReceiptBook(receiptModelDTO);
            }

        }

        #endregion

        #region 判断该小区票据是否需要状态更换提示框
        public bool IsReceiptBookStatusPrompt(int RceciptType, int ComDeptId, int ReceiptBookId)
        {
            ReceiptBookDomainService _ReceiptBookDomainService = new ReceiptBookDomainService();
            Condition<ReceiptBook> condition = new Condition<ReceiptBook>(c => c.IsDel == false && c.DeptId == ComDeptId&&c.ReceiptBookType== RceciptType);
            if (ReceiptBookId>0)
            {
                condition = condition & new Condition<ReceiptBook>(c =>c.Id!= ReceiptBookId);
            }
            var retrunList = _ReceiptBookDomainService.GetReceiptBookList(condition.ExpressionBody);
            if (retrunList.Count > 0)
            {
                //如果其中有启用则弹出提示
                if (retrunList.Any(o => o.Status == (int)ReceiptBookStatusEnum.Enabled))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 日志的生成
        private void GenerateReceiptHistory(ReceiptBookHistory receiptbookhistory, ReceiptBookDTO receiptbookDTO,string OperatorName)
        {

           

            var receiptbook = ReceiptBookMappers.ChangeDTOToReceiptBookNew(receiptbookDTO);
            StringBuilder CcontentStr = new StringBuilder();

            receiptbookhistory.ReceiptBookHistoryType = (int)ReceiptBookHistoryTypeEnum.ReceiptConfig;//都是票据配置
            receiptbookhistory.CreateTime = DateTime.Now;
            receiptbookhistory.IsDel = false;
            receiptbookhistory.OperatorName = OperatorName;
            receiptbookhistory.UpdateTime = DateTime.Now;
            receiptbookhistory.DeptId = receiptbookDTO.DeptId;
            if (receiptbookDTO.Id != null && receiptbookDTO.Id.Value > 0)
            {//更新

                var OldReceiptBook = GetReceiptBookByKey(receiptbookDTO.Id.Value);
                if (OldReceiptBook.Status != receiptbookDTO.Status)
                {//启用或者停用
                    if (receiptbookDTO.Status == (int)ReceiptBookStatusEnum.Disabled)
                    {
                        CcontentStr.Append("停用收费票据 “" + receiptbookDTO.Name+"”");
                    }
                    else
                    {
                        CcontentStr.Append("启用收费票据“" + receiptbookDTO.Name+ "”");
                    }
                }
                else
                {
                    CcontentStr.Append("修改收费票据“" + receiptbookDTO.Name+"”");
                    CcontentStr.Append(",当前票据号" + receiptbookDTO.ReceiptCurrentNumberView);

                }
            }
            else
            {//新增
                if (receiptbookDTO.Status == (int)ReceiptBookStatusEnum.Disabled)
                {

                    CcontentStr.Append("新增收费票据“" + receiptbookDTO.Name+"”");
                }
                else
                {
                    CcontentStr.Append("新增并启用收费票据“" + receiptbookDTO.Name+"”");
                    
                }
                CcontentStr.Append(",票据号" + receiptbookDTO.ReceiptCurrentNumberView + "至" + BillCommonService.Instance.CreateReceiptBookNumber(receiptbook,receiptbookDTO.EndCode));
                CcontentStr.Append(",当前票据号" + receiptbookDTO.ReceiptCurrentNumberView);


            }
            receiptbookhistory.OperatorContent = CcontentStr.ToString();
        }

        #endregion


    }
}
