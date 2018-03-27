using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using YK.PropertyMgr.CompositeAppService;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;

namespace YK.PropertyMgr.ApplicationService
{
    public partial class PaymentTasksAppService
    {

        #region 日常收费查询页面数据
        public IList<PaymentTasksDTO> GetPaymentTasksDTOList(PaymentTasksSearchDTO searchDto, out int totalCount)
        {
            if (searchDto.DeptType != EDeptType.XiaoQu)
            {
                totalCount = 0;
                return new List<PaymentTasksDTO>();
            }

            Condition<PaymentTasks> condition = new Condition<PaymentTasks>(c => true);
            if (!string.IsNullOrEmpty(searchDto.ApplicantName))
            {
                condition = condition & new Condition<PaymentTasks>(c => c.ApplicantName.Contains(searchDto.ApplicantName));
            }

            if (searchDto.PaymentDateMin.HasValue)
            {
                condition = condition & new Condition<PaymentTasks>(c => c.PaymentDate >= searchDto.PaymentDateMin);
            }

            if (searchDto.PaymentDateMax.HasValue)
            {
                searchDto.PaymentDateMax = searchDto.PaymentDateMax.Value.AddDays(1).AddSeconds(-1);
                condition = condition & new Condition<PaymentTasks>(c => c.PaymentDate <= searchDto.PaymentDateMax);
            }

            if (searchDto.DeptId > 0)
            {
                condition = condition & new Condition<PaymentTasks>(c => c.ComDeptId == searchDto.DeptId);

            }

            if (searchDto.Status != 0)
            {

                condition = condition & new Condition<PaymentTasks>(c => c.Status == searchDto.Status);
            }

            if (!string.IsNullOrEmpty(searchDto.Code))
            {
                condition = condition & new Condition<PaymentTasks>(c => c.Code.Contains(searchDto.Code ));
            }

            condition = condition & new Condition<PaymentTasks>(c => c.IsDel == false);

            string expressions = "PaymentDate desc";
            //获取domain entity
            var domainList = PaymentTasksService.GetPaymentTasksList(searchDto.PageIndex, searchDto.PageSize, condition.ExpressionBody, expressions, out totalCount);
            var dtoList = PaymentTasksMappers.ChangePaymentTasksToDTOs(domainList);


            return dtoList.ToList();
        }


        public IEnumerable<DictionaryModel> GetPayTypeList()
        {
            var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
            return propertyService.GetDictionaryModels(PropertyEnumType.PayType.ToString());
        }


        public IEnumerable<TemplateModel> GetPaymentTasksViewTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {

                new TemplateColumn(){ ColumnName = "Code", ColumnDesc = "交款编号", Seq = i++},
                new TemplateColumn(){ ColumnName = "ApplicantName", ColumnDesc = "交款人", Seq = i++},
                new TemplateColumn(){ ColumnName = "PaymentDate", ColumnDesc = "交款时间", Seq = i++, Type = "date"},
                new TemplateColumn(){ ColumnName = "Money", ColumnDesc = "交款金额", Seq = i++},
                new TemplateColumn(){ ColumnName = "Status", ColumnDesc = "审核状态", Seq = i++,DictId = PropertyEnumType.PaymentTaskStatus.ToString()},
                new TemplateColumn(){ ColumnName = "ReviewerName", ColumnDesc = "审核人", Seq = i++},
                new TemplateColumn(){ ColumnName = "ReviewDate", ColumnDesc = "审核时间", Seq = i++, Type = "date"},
                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++},
                new TemplateColumn(){ ColumnName = "CheckRemark", ColumnDesc = "审核备注", Seq = i++}
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(PaymentTasksDTO), showColumns);
            return template;
        }

        #endregion

        #region  交款

        public ResultModel PaymentTasksAdd(string[] ids, int Operator, string OperatorName,string Remark, DateTime PaymentDate)
        {
            return PaymentAppService.GenerateBillPaymentTask(ids, Remark, Operator, OperatorName, PaymentDate);
        }



        #endregion

        #region 收费项目汇总表

        public IEnumerable<TemplateModel> GetPaymentTasksBySubjectViewTemplate()
        {
            int i = 1;
            List<TemplateColumn> showColumnslist = new List<TemplateColumn>();
            showColumnslist.Add(new TemplateColumn()
            {
                ColumnName = "Name",
                ColumnDesc = "项目名称",
                Seq = i++
            });
            var paytypelist = GetPayTypeList().ToList();
            foreach (DictionaryModel DM in paytypelist)
            {
                showColumnslist.Add(new TemplateColumn()
                {
                    ColumnName = DM.EnName,
                    ColumnDesc = DM.CnName,
                    Seq = i++
                });
            }

          

            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(PaymentTaskBySubjetDTO), showColumnslist.ToArray());
            return template;
        }



        public ReportArrearsModels GetPaymentTasksBySubjectList(int PaymentTasksId, DateTime? PaymentDateMax, int ComDeptId, out int totalCount)
        {
            return PaymentTasksService.GetPaymentTasksBySubjectList_All(PaymentDateMax.Value, ComDeptId, out totalCount, PaymentTasksId);
        }

        public PaymentTaskBySubjetDTO GetPaymentTaskPayMthodIdList(int PaymentTasksId, DateTime? PaymentDateMax, int ComDeptId, int? CheckAdminId)
        {
            if (CheckAdminId < 0)
                return new PaymentTaskBySubjetDTO();
            return PaymentTasksService.GetPaymentTaskPayMthodIdList(PaymentDateMax.Value, ComDeptId, CheckAdminId, PaymentTasksId);
        }

        public string GetLastPaymentTaskDate(int ComDeptId)
        {
            return PaymentTasksService.GetLastPaymentTaskDate(ComDeptId);
        }


        public List<PaymentTaskBySubjetDTO> GetPaymentTaskSubjectList(int PaymentTasksId, DateTime? PaymentDateMax,int ComDeptId,int? CheckAdminId)
        {
            Condition<ChargeRecord> condition = new Condition<ChargeRecord>(c => true);
            if (CheckAdminId > 0)
            {
                condition = new Condition<ChargeRecord>(c => c.Operator == CheckAdminId);
            }
            else if (CheckAdminId < 0)
            {
                return new List<PaymentTaskBySubjetDTO>();
            }
           

            return PaymentTasksService.GetPaymentTaskSubjectList(PaymentDateMax.Value, ComDeptId, condition.ExpressionBody,PaymentTasksId);
        }



        #endregion

    }
}
