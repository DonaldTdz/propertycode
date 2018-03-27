using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplatePrintPDF;
using TemplatePrintPDF.Models;
using YK.BackgroundMgr.DomainInterface;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.ApplicationDTO.Resources;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.DomainService;
using YK.PropertyMgr.RepositoryContract;


namespace YK.PropertyMgr.ApplicationService
{
    public partial class TemplatePrintRecordAppService
    {
        #region 物业公司模板
        /// <summary>
        /// 泾华物业
        /// </summary>
        public void CreateJingHuaTemplatePrint()
        {
            /*生成泾华物业打印单
             * 第一排 小区名称    年  月  日
             * 第二排 房间号      业主姓名
             * 表格6行3-8排 项目 摘要 金额 
             * 第九排：人民币大写     金额合计
             * 第十排： 负责人   收款人   开票人  
             */

            //单据
            TemplatePrintRecord templateprintrecord = new TemplatePrintRecord()
            {
                Name = "泾华物业套打模板",
                DeptId = 113343,
                templateId = (int)PrintTemplateEnum.JINHuaTemplatePrint,
                PageWidth = 20.9f,
                PageHigh = 12.3f,
                RowNumber = 6,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false
            };
            List<TemplatePrintRecordDetail> DetailList = new List<TemplatePrintRecordDetail>();
            //明细

            #region 第一排
            //第一排 小区名字
            TemplatePrintRecordDetail templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 2.6f,
                YAxis = 9.1f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = false,
                RowIncrement = 0f,
                AttributeName = "CommunityName",
                AttributeCNName = "小区名称"
            };
            DetailList.Add(templateprintrecorddetail);

            //第一排 年
            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 9.2f,
                YAxis = 9.1f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = false,
                RowIncrement = 0f,
                AttributeName = "YearCode",
                AttributeCNName = "年"
            };
            DetailList.Add(templateprintrecorddetail);

            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 11f,
                YAxis = 9.1f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = false,
                RowIncrement = 0f,
                AttributeName = "MonthCode",
                AttributeCNName = "月"
            };
            DetailList.Add(templateprintrecorddetail);

            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 12.2f,
                YAxis = 9.1f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = false,
                RowIncrement = 0f,
                AttributeName = "DayCode",
                AttributeCNName = "日"
            };
            DetailList.Add(templateprintrecorddetail);
            #endregion

            #region 第二排
            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 1.8f,
                YAxis = 8.3f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = false,
                RowIncrement = 0f,
                AttributeName = "ResourceName",
                AttributeCNName = "房间号"
            };
            DetailList.Add(templateprintrecorddetail);

            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 12.4f,
                YAxis = 8.3f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = false,
                RowIncrement = 0f,
                AttributeName = "OwnerName",
                AttributeCNName = "业主姓名"
            };
            DetailList.Add(templateprintrecorddetail);
            #endregion

            #region 第三排-第八排 表格 循环
            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 1f,
                YAxis = 7.2f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = true,
                RowIncrement = -0.85f,
                AttributeName = "ChargeSubjectName",
                AttributeCNName = "项目/科目"
            };
            DetailList.Add(templateprintrecorddetail);

            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 4.8f,
                YAxis = 7.2f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = true,
                RowIncrement = -0.85f,
                AttributeName = "SummaryStr",
                AttributeCNName = "摘要"
            };
            DetailList.Add(templateprintrecorddetail);

            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 17.4f,
                YAxis = 7.2f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = true,
                RowIncrement = -0.85f,
                AttributeName = "Amount",
                AttributeCNName = "金额"
            };
            DetailList.Add(templateprintrecorddetail);
            #endregion

            #region 第九排
            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 7.2f,
                YAxis = 2.5f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = false,
                RowIncrement = 0f,
                AttributeName = "TotalAmountCN",
                AttributeCNName = "人民币（大写）"
            };
            DetailList.Add(templateprintrecorddetail);

            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 17.4f,
                YAxis = 2.5f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = false,
                RowIncrement = 0f,
                AttributeName = "TotalAmount",
                AttributeCNName = "合计金额"
            };
            DetailList.Add(templateprintrecorddetail);



            #endregion

            #region 第十排

            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 10.3f,
                YAxis = 0.9f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = false,
                RowIncrement = 0f,
                AttributeName = "OperatorName",
                AttributeCNName = "收款人"
            };
            DetailList.Add(templateprintrecorddetail);

            #endregion

            TemplatePrintRecordDomainService _TemplatePrintRecordDomainService = new TemplatePrintRecordDomainService();
            _TemplatePrintRecordDomainService.InserCreateTemplatePrint(templateprintrecord, DetailList);




        }

        /// <summary>
        /// 川港物业
        /// </summary>
        public void CreateChuanGangTemplatePrint()
        {
            /*生成川港物业打印单
            * 第一排  年  月  日
            * 第二排 项目 房号  业主姓名
            * 表格4行3-6排 项目 摘要 金额 
            * 第七排：人民币大写    
            * 第八排： 收款单位   交款人   收款人  
            */

            //单据
            TemplatePrintRecord templateprintrecord = new TemplatePrintRecord()
            {
                Name = "川港物业套打模板",
                DeptId = 113343,
                templateId = (int)PrintTemplateEnum.ChuanGangTemplatePrint,
                PageWidth = 17.5f,
                PageHigh = 8.5f,
                RowNumber = 4,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false
            };
            List<TemplatePrintRecordDetail> DetailList = new List<TemplatePrintRecordDetail>();
            #region 第一排
            //第一排 年
            TemplatePrintRecordDetail templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis =6.2f,
                YAxis = 6.15f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = false,
                RowIncrement = 0f,
                AttributeName = "YearCode",
                AttributeCNName = "年"
            };
            DetailList.Add(templateprintrecorddetail);
            //月
            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 7.5f,
                YAxis = 6.15f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = false,
                RowIncrement = 0f,
                AttributeName = "MonthCode",
                AttributeCNName = "月"
            };
            DetailList.Add(templateprintrecorddetail);

            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 9.1f,
                YAxis = 6.15f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = false,
                RowIncrement = 0f,
                AttributeName = "DayCode",
                AttributeCNName = "日"
            };
            DetailList.Add(templateprintrecorddetail);
            #endregion

            #region 第二排
            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 2.2f,
                YAxis = 5.6f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = false,
                RowIncrement = 0f,
                AttributeName = "CommunityName",
                AttributeCNName = "小区名称"
            };
            DetailList.Add(templateprintrecorddetail);

            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 6.6f,
                YAxis = 5.6f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = false,
                RowIncrement = 0f,
                AttributeName = "ResourceName",
                AttributeCNName = "房号"
            };
            DetailList.Add(templateprintrecorddetail);

            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 12.6f,
                YAxis = 5.6f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = false,
                RowIncrement = 0f,
                AttributeName = "OwnerName",
                AttributeCNName = "业主姓名"
            };
            DetailList.Add(templateprintrecorddetail);
            #endregion

            #region 第三排-第六排 表格 循环
            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 1.6f,
                YAxis = 4.2f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = true,
                RowIncrement = -0.6f,
                AttributeName = "ChargeSubjectName",
                AttributeCNName = "项目/科目"
            };
            DetailList.Add(templateprintrecorddetail);

            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 4.4f,
                YAxis = 4.2f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = true,
                RowIncrement = -0.6f,
                AttributeName = "SummaryStr",
                AttributeCNName = "摘要"
            };
            DetailList.Add(templateprintrecorddetail);

            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 13.2f,
                YAxis = 4.2f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = true,
                RowIncrement = -0.6f,
                AttributeName = "Amount",
                AttributeCNName = "金额"
            };
            DetailList.Add(templateprintrecorddetail);
            #endregion

            #region 第七排
          
            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 6.7f,
                YAxis = 1.8f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = false,
                RowIncrement = 0f,
                AttributeName = "TotalAmountCN",
                AttributeCNName = "人民币（大写）"
            };
            DetailList.Add(templateprintrecorddetail);





            #endregion

            #region 第八排

            templateprintrecorddetail = new TemplatePrintRecordDetail()
            {
                XAxis = 13.3f,
                YAxis = 0.4f,
                FontSize = 10f,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDel = false,
                Isloop = false,
                RowIncrement = 0f,
                AttributeName = "OperatorName",
                AttributeCNName = "收款人"
            };
            DetailList.Add(templateprintrecorddetail);

            #endregion
            TemplatePrintRecordDomainService _TemplatePrintRecordDomainService = new TemplatePrintRecordDomainService();
            _TemplatePrintRecordDomainService.InserCreateTemplatePrint(templateprintrecord, DetailList);
        }

        #endregion

        /// <summary>
        /// 打印模板转换为PDF字节流
        /// </summary>
        /// <param name="templateprintmodel"></param>
        /// <returns></returns>
        public byte[] GetTemplatePrintByte(TemplatePrintModel templateprintmodel,int PropertyId,int templateId)
        {
            TemplatePrintRecordDomainService _TemplatePrintRecordDomainService = new TemplatePrintRecordDomainService();
            //获取数据库存的模板
            Condition<TemplatePrintRecord> condition = new Condition<TemplatePrintRecord>(c => c.templateId== templateId);
            var TemplatePrintRecordModel=  _TemplatePrintRecordDomainService.GetModelByQuery(condition.ExpressionBody);

            if (TemplatePrintRecordModel != null)
            {
               PrintPDF _PrintPDF = new PrintPDF();
               var printList= GetTemplatePrintModelToPdfPageItem(templateprintmodel, TemplatePrintRecordModel);
               return   _PrintPDF.TemplatePrint(printList, TemplatePrintRecordModel.PageWidth.Value, TemplatePrintRecordModel.PageHigh.Value);
            }
           
            return null;
        }


        private List<PdfPageItem> GetTemplatePrintModelToPdfPageItem(TemplatePrintModel templateprintmodel, TemplatePrintRecord templateprintrecord)
        {


            TemplatePrintRecordDetailDomainService _TemplatePrintRecordDetailDomainService = new TemplatePrintRecordDetailDomainService();
            Condition<TemplatePrintRecordDetail> condition = new Condition<TemplatePrintRecordDetail>(c => c.TemplatePrintRecordId == templateprintrecord.Id&&c.IsDel==false);
            var DetailList=   _TemplatePrintRecordDetailDomainService.GetTemplatePrintRecordDetails(condition.ExpressionBody);
            List<PdfPageItem> printList = new List<PdfPageItem>();
            PdfPageItem pdfpageitem = new PdfPageItem();
            List<PdfCell> CellList = new List<PdfCell>();
            //获取打印数据
            foreach (var printmodel in DetailList)
             {
                if (!printmodel.Isloop.Value)
                {
                    PdfCell Cell = new PdfCell();
                    var Value = templateprintmodel.GetValue(printmodel.AttributeName);
                    Cell.ContentText = Value.ToString();
                    Cell.XAxis = printmodel.XAxis.Value;
                    Cell.YAxis = printmodel.YAxis.Value;
                    Cell.FontSize = printmodel.FontSize.Value;
                    CellList.Add(Cell);
                }
 
             }
            float i = 1;
            foreach (var DataDetail in templateprintmodel.FormList)
            {
                foreach (var printmodel in DetailList.Where(o => o.Isloop == true).ToList())
                {
                    PdfCell Cell = new PdfCell();
                    var Value = DataDetail.GetValue(printmodel.AttributeName);
                    Cell.ContentText = Value.ToString();
                    Cell.XAxis = printmodel.XAxis.Value;
                    Cell.YAxis = (printmodel.YAxis.Value + (printmodel.RowIncrement * i)).Value;
                    Cell.FontSize = printmodel.FontSize.Value;
                    CellList.Add(Cell);
                }
                i++;
            }


            pdfpageitem.CellList = CellList;
            printList.Add(pdfpageitem);



            return printList;
        }
        



    }
}
 