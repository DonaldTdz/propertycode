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

namespace YK.PropertyMgr.ApplicationService
{
   public partial   class ReceiptBookHistoryAppService
    {

        public IEnumerable<TemplateModel> GetReceiptBookHistoryListTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
          
                new TemplateColumn(){ ColumnName = "ReceiptBookHistoryType", ColumnDesc = "操作类型", Seq = i++,DictId = PropertyEnumType.ReceiptBookHistoryType.ToString()},
                new TemplateColumn(){ ColumnName = "OperatorContent", ColumnDesc = "内容", Seq = i++,DictId="-1"},
                new TemplateColumn(){ ColumnName = "CreateTimeStr", ColumnDesc = "操作时间", Seq = i++,DictId="-1" },
                new TemplateColumn(){ ColumnName = "OperatorName", ColumnDesc = "操作人员", Seq = i++,DictId="-1"}

            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ReceiptBookHistoryDTO), showColumns);
            return template;
        }


        public IList<ReceiptBookHistoryDTO> GetRReceiptBookHistoryDTOList(ReceiptBookSearchDTO searchDto, out int totalCount)
        {
            ReceiptBookHistoryDomainService _ReceiptBookHistoryDomainService = new ReceiptBookHistoryDomainService();
            Condition<ReceiptBookHistory> condition = new Condition<ReceiptBookHistory>(c => c.IsDel == false&&c.DeptId==searchDto.DeptId);


            if (searchDto.ReceiptBookHistoryType != null && searchDto.ReceiptBookHistoryType.Value > 0)
            {
                 
                condition = condition & new Condition<ReceiptBookHistory>(c => c.ReceiptBookHistoryType== searchDto.ReceiptBookHistoryType);
            }
            if (!string.IsNullOrEmpty(searchDto.OperatorName))
            {
                condition = condition & new Condition<ReceiptBookHistory>(c => c.OperatorName.Contains(searchDto.OperatorName));
            }

            if (!string.IsNullOrEmpty(searchDto.OperatorContent))
            {
                condition = condition & new Condition<ReceiptBookHistory>(c => c.OperatorContent.Contains(searchDto.OperatorContent));
            }
            if (searchDto.MinDate != null&&searchDto.MinDate>DateTime.MinValue)
            {
                condition = condition & new Condition<ReceiptBookHistory>(c => c.CreateTime>= searchDto.MinDate);
            }

            if (searchDto.MaxDate != null&& searchDto.MinDate > DateTime.MinValue)
            {
                var EndDate = searchDto.MaxDate.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
                condition = condition & new Condition<ReceiptBookHistory>(c => c.CreateTime <= EndDate);
            }



            string expressions = "CreateTime desc";
            var ReceiptBookHistoryList = _ReceiptBookHistoryDomainService.GetReceiptBookHistoryList(searchDto.PageStart, searchDto.PageSize, condition.ExpressionBody, expressions, out totalCount);
            return ReceiptBookHistoryMappers.ChangeReceiptBookHistoryToDTOs(ReceiptBookHistoryList).ToList();  
            
         
        }



    }
}
