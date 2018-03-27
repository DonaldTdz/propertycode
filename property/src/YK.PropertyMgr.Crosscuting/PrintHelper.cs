using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.Crosscuting
{
    public class PrintHelper
    {
        public static string HeaderFontPath = "C:/Windows/Fonts/msyh.ttf";
        public static string BodyFontPath = "C:/Windows/Fonts/msyh.ttf";
        public static float PageContentWidth = 480;//420;
        public static byte[] GetPdfFileTestStream()
        {

           
            MemoryStream outputStream = new MemoryStream();
            // 创建 PDF 文档
            Document document = new Document();
            // 创建写入器实例，PDF 文件将会保存到这里
            PdfWriter.GetInstance(document, outputStream);
            // 打开文档
            document.Open();
            BaseFont bf = BaseFont.CreateFont("C:/Windows/Fonts/msyh.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font = new Font(bf);
            //写入一个段落, Paragraph
            document.Add(new Paragraph("报表title", font));
            //Header
            PdfPTable baseTable = new PdfPTable(2);
            PdfPCell headerCell = new PdfPCell(new Paragraph("开始", font));
            baseTable.AddCell(headerCell);
            PdfPCell headerCell2 = new PdfPCell(new Paragraph("结束", font));
            baseTable.AddCell(headerCell2);
            PdfPCell cell = new PdfPCell(new Paragraph("c1", font));
            baseTable.AddCell(cell);
            PdfPCell cell2 = new PdfPCell(new Paragraph("c2", font));
            baseTable.AddCell(cell2);
            //Body
            //Footer
            document.Add(baseTable);
            document.Close();
            outputStream.Close();
            return outputStream.ToArray();
        }

        public static byte[] GetPdfFileStreamByTemplate(PrintDataModel data, int Template)
        {
            if (Template == 1)
            {
                return GetPdfFileStream(data);
            }
            return null;
        }

        /// <summary>
        /// 打印模板的页数
        /// </summary>
        /// <param name="billList"></param>
        /// <param name="numEvery">每张收据的条数</param>
        /// <param name="num">每页可显示几张收据</param>
        /// <returns></returns>
        private static int GetPageCount(int count, int numEvery, int num)
        {
            int pageCount = 0;
            if (count > 0)
            {
                pageCount = count % (num * numEvery);
                pageCount = (pageCount == 0 ? count / (num * numEvery) : count / (num * numEvery) + 1);
            }
            else
            {
                pageCount = 1;
            }
            return pageCount;
        }

        /// <summary>
        /// 打印的DATA
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="countData">总共数据</param>
        /// <param name="everyBillCount">每张收据显示条数</param>
        /// <param name="pageReceipt">每页几张收据k</param>
        /// <param name="everyReceiptBodyData">每张收据BODY单元格共多少数据</param>
        /// <returns></returns>
        public static byte[] GetPdfFileStream(PrintDataModel data, int countData, int everyBillCount, int pageReceipt, int footSize)
        {
            MemoryStream outputStream = new MemoryStream();
            // 创建 PDF 文档
            //Rectangle pageSize = new Rectangle(210,297);
            Rectangle pageSize = new Rectangle(595f, 263.65f);
            //Rectangle pageSize = new Rectangle(683, 794);
            Document document = new Document(pageSize);
            document.SetMargins(0, 0, 10, 10);
            // 创建写入器实例，PDF 文件将会保存到这里
            PdfWriter.GetInstance(document, outputStream);
            // 打开文档
            document.Open();

            int paddingTop = 0;/*专用收据名称距离上层的位置*/
            /*总共页数*/
            int pages = GetPageCount(countData, everyBillCount, pageReceipt);
            for (int page = 1; page <= pages; page++)/*页数*/
            {


                /*数据条数*/
                if (page == 1)
                {
                    for (int M = 1; M <= pageReceipt; M++)/*每页打印张数*/
                    {
                        if (everyBillCount * (M - 1) < countData)
                        {
                            int dataSkip = (M - 1) * everyBillCount;
                            document.Add(data.TitleToPdfPTable(paddingTop));
                            document.Add(data.PrintHeader.ToPdfPTable());
                            document.Add(data.PrintBody.ToPdfPTable(dataSkip, everyBillCount, (M - 1) * footSize, footSize));
                        }
                    }
                }
                else
                {
                    for (int M = 1; M <= pageReceipt; M++)/*每页打印张数*/
                    {
                        int skipData = (everyBillCount * (M - 1)) + pageReceipt * everyBillCount * (page - 1);
                        if (skipData < countData)
                        {

                            document.Add(data.TitleToPdfPTable(paddingTop));
                            document.Add(data.PrintHeader.ToPdfPTable());
                            document.Add(data.PrintBody.ToPdfPTable(skipData, everyBillCount, (M - 1) * footSize + ((page - 1) * pageReceipt * footSize), footSize));
                        }
                    }
                }
                if (page < pages)
                {
                    document.NewPage();
                }
            }
            document.Close();
            outputStream.Close();
            return outputStream.ToArray();
        }
        /// <summary>
        /// 打印多个模板
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="countData">总共数据</param>
        /// <param name="everyBillCount">每张收据显示条数</param>
        /// <param name="pageReceipt">每页几张收据k</param>
        /// <param name="everyReceiptBodyData">每张收据BODY单元格共多少数据</param>
        /// <returns></returns>
        public static byte[] GetPdfFileStream(List<PrintDataModel> dataList, int everyBillCount, int pageReceipt, int footSize)
        {
            if (dataList.Count <= 0)
                return new byte[0];
            MemoryStream outputStream = new MemoryStream();
            // 创建 PDF 文档
            //Rectangle pageSize = new Rectangle(210,297);
            Rectangle pageSize = new Rectangle(595f, 263.65f);
            //Rectangle pageSize = new Rectangle(683, 794);
            Document document = new Document(pageSize);
            document.SetMargins(0, 0, 10, 10);
            // 创建写入器实例，PDF 文件将会保存到这里
            PdfWriter.GetInstance(document, outputStream);
            // 打开文档
            document.Open();

            int paddingTop = 0;/*专用收据名称距离上层的位置*/
            /*总共页数*/
            foreach (var data in dataList)
            {
                int countData = data.PrintBody.PrintRowList.Count;
                int pages = GetPageCount(countData, everyBillCount, pageReceipt);
                for (int page = 1; page <= pages; page++)/*页数*/
                {
                    /*数据条数*/
                    if (page == 1)
                    {
                        for (int M = 1; M <= pageReceipt; M++)/*每页打印张数*/
                        {
                            if (everyBillCount * (M - 1) < countData)
                            {
                                int dataSkip = (M - 1) * everyBillCount;
                                document.Add(data.TitleToPdfPTable(paddingTop));
                                document.Add(data.PrintHeader.ToPdfPTable());
                                document.Add(data.PrintBody.ToPdfPTable(dataSkip, everyBillCount, (M - 1) * footSize, footSize));
                            }
                        }
                    }
                    else
                    {
                        for (int M = 1; M <= pageReceipt; M++)/*每页打印张数*/
                        {
                            int skipData = (everyBillCount * (M - 1)) + pageReceipt * everyBillCount * (page - 1);
                            if (skipData < countData)
                            {

                                document.Add(data.TitleToPdfPTable(paddingTop));
                                document.Add(data.PrintHeader.ToPdfPTable());
                                document.Add(data.PrintBody.ToPdfPTable(skipData, everyBillCount, (M - 1) * footSize + ((page - 1) * pageReceipt * footSize), footSize));
                            }
                        }
                    }
                    if (page < pages)
                    {
                        document.NewPage();
                    }
                }
                document.NewPage();
            }
            document.Close();
            outputStream.Close();
            return outputStream.ToArray();
        }

        public static byte[] GetPdfFileStream(PrintDataModel data)
        {
            MemoryStream outputStream = new MemoryStream();
            // 创建 PDF 文档
            //Rectangle pageSize = new Rectangle(210,297);
            Rectangle pageSize = new Rectangle(595f, 263.65f);
            Document document = new Document(pageSize);
            document.SetMargins(0, 0, 10, 10);
            // 创建写入器实例，PDF 文件将会保存到这里
            PdfWriter.GetInstance(document, outputStream);
            // 打开文档
            document.Open();
            int pageCount = 2;

            for (int page = 1; page <= pageCount; page++)/*页数*/
            {

                for (int M = 1; M <= 3; M++)/*每页打印张数*/
                {
                    document.Add(data.TitleToPdfPTable(20));
                    document.Add(data.PrintHeader.ToPdfPTable());
                    document.Add(data.PrintBody.ToPdfPTable());
                }
                if (page < pageCount)
                {
                    document.NewPage();
                }
            }
            document.Close();
            outputStream.Close();
            return outputStream.ToArray();
        }

        public static string ToAmountUppercase(string money)
        {
            //将小写金额转换成大写金额           
            double MyNumber = Convert.ToDouble(money);
            String[] MyScale = { "分", "角", "元", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰", "仟", "兆", "拾", "佰", "仟" };
            String[] MyBase = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
            String M = "";
            bool isPoint = false;
            if (money.IndexOf(".") != -1)
            {
                money = money.Remove(money.IndexOf("."), 1);
                isPoint = true;
            }
            for (int i = money.Length; i > 0; i--)
            {
                int MyData = Convert.ToInt16(money[money.Length - i].ToString());
                M += MyBase[MyData];
                if (isPoint == true)
                {
                    M += MyScale[i - 1];
                }
                else
                {
                    M += MyScale[i + 1];
                }
            }
            return M;
        }


        public static string ToAmountUppercase(decimal num)
        {
            string str1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字 
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字 
            string str3 = "";    //从原num值中取出的值 
            string str4 = "";    //数字的字符串形式 
            string str5 = "";  //人民币大写金额形式 
            int i;    //循环变量 
            int j;    //num的值乘以100的字符串长度 
            string ch1 = "";    //数字的汉语读法 
            string ch2 = "";    //数字位的汉字读法 
            int nzero = 0;  //用来计算连续的零值是几个 
            int temp;            //从原num值中取出的值 

            num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数 
            str4 = ((long)(num * 100)).ToString();        //将num乘100并转换成字符串形式 
            j = str4.Length;      //找出最高位 
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以tr2=佰拾元角分 

            //循环取出每一位需要转换的值 
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //取出需转换的某一位的值 
                temp = Convert.ToInt32(str3);      //转换为数字 
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时 
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位 
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上 
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //最后一位（分）为0时，加上“整” 
                    str5 = str5 + '整';
                }
            }
            if (num == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }


        public static byte[] TestPdfFile()
        {
            MemoryStream outputStream = new MemoryStream();
            Rectangle pageSize = new Rectangle(595f, 263.65f);
            Document document = new Document(pageSize);
            PdfWriter.GetInstance(document, outputStream);
            document.Open();
            Image image = Image.GetInstance("C:\\Users\\maosiyue\\Desktop\\打印模板.jpg");
            image.ScalePercent(35f);
            document.Add(image);
        

            PdfPTable table = new PdfPTable(1);
            table.SetWidths(new float[] { PrintHelper.PageContentWidth });
            BaseFont bf = BaseFont.CreateFont(PrintHelper.HeaderFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font = new Font(bf, 18);
            Paragraph reportTitle = new Paragraph("1111", font);
            PdfPCell cell = new PdfPCell(reportTitle);
            cell.Border = 0;
            cell.PaddingTop = 10;
            cell.HorizontalAlignment = (Element.ALIGN_CENTER);// 设置内容水平居中显示  
            cell.VerticalAlignment = (Element.ALIGN_MIDDLE);  // 设置垂直居中  
            table.AddCell(cell);

            document.Add(table);


            document.Close();
            outputStream.Close();
            return outputStream.ToArray();
        }


       
       

    }

    public class PrintDataModel
    {
        public string Title { get; set; }

        public PrintHeaderModel PrintHeader { get; set; }

        public PrintBodyModel PrintBody { get; set; }

        public virtual PdfPTable TitleToPdfPTable()
        {
            PdfPTable table = new PdfPTable(1);
            table.SetWidths(new float[] { PrintHelper.PageContentWidth });
            BaseFont bf = BaseFont.CreateFont(PrintHelper.HeaderFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font = new Font(bf, 18);
            Paragraph reportTitle = new Paragraph(Title, font);
            PdfPCell cell = new PdfPCell(reportTitle);
            cell.Border = 0;
            cell.PaddingTop = 10;
            cell.HorizontalAlignment = (Element.ALIGN_CENTER);// 设置内容水平居中显示  
            cell.VerticalAlignment = (Element.ALIGN_MIDDLE);  // 设置垂直居中  
            table.AddCell(cell);
            return table;
        }


        /// <summary>
        /// 厘米转换为分辨率
        /// </summary>
        /// <param name="num"></param>
        /// <param name="ResolutionNum"> DPI 默认为72DPI</param>
        /// <returns></returns>
        public float ConvertCMToPX(float num, float ResolutionNum=72f)
        {
            var BaseDPI = Math.Round(ResolutionNum / 2.54, 2);

            return float.Parse((num * BaseDPI).ToString());
      
        }


        public virtual PdfPTable TitleToPdfPTable(float position)
        {
            PdfPTable table = new PdfPTable(1);
            table.SetWidths(new float[] { PrintHelper.PageContentWidth });
            BaseFont bf = BaseFont.CreateFont(PrintHelper.HeaderFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font = new Font(bf, 18);
            Paragraph reportTitle = new Paragraph(Title, font);
            PdfPCell cell = new PdfPCell(reportTitle);
            cell.Border = 0;
            cell.PaddingTop = position;
            cell.HorizontalAlignment = (Element.ALIGN_CENTER);// 设置内容水平居中显示  
            cell.VerticalAlignment = (Element.ALIGN_MIDDLE);  // 设置垂直居中  
            table.AddCell(cell);
            return table;
        }

        public PrintDataModel(string title)
        {
            Title = title;
            PrintHeader = new PrintHeaderModel();
            PrintBody = new PrintBodyModel();
        }

        public PrintDataModel()
        {
            PrintHeader = new PrintHeaderModel();
            PrintBody = new PrintBodyModel();
        }
    }

    public class PrintHeaderModel
    {
        public int ColNum { get; set; }

        public float[] Widths { get; set; }

        public List<PrintCell> PrintCellList { get; set; }

        public PrintHeaderModel(int colNum)
        {
            ColNum = colNum;
            PrintCellList = new List<PrintCell>();
        }

        public PrintHeaderModel()
        {
            PrintCellList = new List<PrintCell>();
        }

        public virtual PdfPTable ToPdfPTable()
        {
            PdfPTable table = new PdfPTable(ColNum);
            table.SetWidths(Widths);
            if (PrintCellList == null)
            {
                return table;
            }
            BaseFont bf = BaseFont.CreateFont(PrintHelper.HeaderFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font = new Font(bf, 10);
            foreach (var item in PrintCellList)
            {
                item.Title = item.Title ?? "";
                item.Value = item.Value ?? "";
                Paragraph gtitle = new Paragraph(item.Title + item.Value, font);
                PdfPCell cellTitle = new PdfPCell(gtitle);
                if (item.HideBorder)
                {
                    cellTitle.Border = 0;
                }
                if (item.Align.HasValue)
                {
                    cellTitle.HorizontalAlignment = Convert.ToInt32(item.Align);
                }
                else
                {
                    cellTitle.HorizontalAlignment = (Element.ALIGN_CENTER);// 设置内容水平居左显示  
                }
                cellTitle.VerticalAlignment = (Element.ALIGN_MIDDLE);  // 设置垂直居中  
                cellTitle.PaddingTop = 10;

                //PdfPCell cellValue = new PdfPCell(new Paragraph(item.Value, font));
                //cellValue.Border = 0;
                //cellValue.HorizontalAlignment = (Element.ALIGN_LEFT);// 设置内容水平居中显示  
                //cellValue.VerticalAlignment = (Element.ALIGN_MIDDLE);  // 设置垂直居中  
                //cellTitle.PaddingTop = 5;
                //cellTitle.PaddingBottom = 5;
                table.AddCell(cellTitle);
                //table.AddCell(cellValue);
            }
            return table;
        }
    }

    public class PrintBodyModel
    {
        public int ColNum { get; set; }

        public List<PrintCell> RotTitleTopList { get; set; }

        public string[] RowTitles { get; set; }

        public float[] Widths { get; set; }

        public List<PrintCell> PrintRowList { get; set; }

        public List<PrintCell> PrintFooterList { get; set; }

        public PrintBodyModel(int colNum)
        {
            ColNum = colNum;
            PrintRowList = new List<PrintCell>();
            PrintFooterList = new List<PrintCell>();
        }

        public PrintBodyModel()
        {
            PrintRowList = new List<PrintCell>();
            PrintFooterList = new List<PrintCell>();
        }

        public virtual void AddRow(string[] rowCellVals)
        {
            foreach (var val in rowCellVals)
            {
                PrintCell cell = new PrintCell();
                cell.Value = val;
                PrintRowList.Add(cell);
            }
        }
        public virtual void AddRow(List<PrintCell> list)
        {
            foreach (var val in list)
            {
                PrintRowList.Add(val);
            }
        }
        public virtual void AddRow(PrintCell printCell)
        {
            PrintRowList.Add(printCell);
        }
        public virtual PdfPTable ToPdfPTable(int skipNum, int numEverReceipt, int footSkip, int footSize)
        {
            PdfPTable table = new PdfPTable(ColNum);
            table.SetWidths(Widths);
            BaseFont bf = BaseFont.CreateFont(PrintHelper.BodyFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font = new Font(bf, 10);
            if (RotTitleTopList != null)
            {
                foreach (var titleTop in RotTitleTopList)
                {
                    PdfPCell cellValue = new PdfPCell(new Paragraph(titleTop.Value, font));
                    cellValue.Colspan = titleTop.Colspan;
                    cellValue.Padding = 5;
                    table.AddCell(cellValue);
                }
            }
            if (RowTitles != null)
            {


                foreach (var title in RowTitles)
                {
                    PdfPCell cellValue = new PdfPCell(new Paragraph(title, font));
                    cellValue.HorizontalAlignment = (Element.ALIGN_CENTER);// 设置内容水平居中显示  
                    cellValue.VerticalAlignment = (Element.ALIGN_MIDDLE);  // 设置垂直居中  
                    cellValue.Padding = 5;
                    table.AddCell(cellValue);
                }
            }

            //BaseFont vbf = BaseFont.CreateFont(PrintHelper.BodyFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //Font vfont = new Font(vbf);
            if (PrintRowList != null)
            {
                foreach (var item in PrintRowList.Skip(skipNum).Take(numEverReceipt))
                {
                    PdfPCell cellValue = new PdfPCell(new Paragraph(item.Value, font));
                    cellValue.HorizontalAlignment = (Element.ALIGN_CENTER);// 设置内容水平居中显示  
                    cellValue.VerticalAlignment = (Element.ALIGN_MIDDLE);  // 设置垂直居中  
                    cellValue.Padding = 5;

                    table.AddCell(cellValue);
                }
            }

            if (PrintFooterList != null)
            {
                foreach (var item in PrintFooterList.Skip(footSkip).Take(footSize))
                {
                    item.Title = item.Title ?? "";
                    item.Value = item.Value ?? "";
                    PdfPCell cellValue = new PdfPCell(new Paragraph(item.Title + item.Value, font));
                    cellValue.Colspan = item.Colspan;
                    if (item.Title == "合计" || item.Title == "备注")
                    {
                        cellValue.HorizontalAlignment = (Element.ALIGN_CENTER);// 设置内容水平居中显示  
                    }
                    else
                    {
                        cellValue.HorizontalAlignment = (Element.ALIGN_LEFT);// 设置内容水平居左显示  
                    }
                    cellValue.VerticalAlignment = (Element.ALIGN_MIDDLE);  // 设置垂直居中  
                    cellValue.Padding = 5;
                    if (item.HideBorder)
                    {
                        cellValue.Border = 0;
                    }
                    table.AddCell(cellValue);

                }
            }
            //table.SpacingAfter = 30;
            table.SpacingBefore = 5;
            return table;
        }

        public virtual PdfPTable ToPdfPTable()
        {
            PdfPTable table = new PdfPTable(ColNum);
            table.SetWidths(Widths);
            BaseFont bf = BaseFont.CreateFont(PrintHelper.BodyFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font = new Font(bf, 10);
            if (RotTitleTopList != null)
            {
                foreach (var titleTop in RotTitleTopList)
                {
                    PdfPCell cellValue = new PdfPCell(new Paragraph(titleTop.Value, font));
                    cellValue.Colspan = titleTop.Colspan;
                    cellValue.Padding = 5;
                    table.AddCell(cellValue);
                }
            }
            if (RowTitles != null)
            {


                foreach (var title in RowTitles)
                {
                    PdfPCell cellValue = new PdfPCell(new Paragraph(title, font));
                    cellValue.HorizontalAlignment = (Element.ALIGN_CENTER);// 设置内容水平居中显示  
                    cellValue.VerticalAlignment = (Element.ALIGN_MIDDLE);  // 设置垂直居中  
                    cellValue.Padding = 5;
                    table.AddCell(cellValue);
                }
            }

            //BaseFont vbf = BaseFont.CreateFont(PrintHelper.BodyFontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //Font vfont = new Font(vbf);
            if (PrintRowList != null)
            {
                foreach (var item in PrintRowList)
                {
                    PdfPCell cellValue = new PdfPCell(new Paragraph(item.Value, font));
                    cellValue.HorizontalAlignment = (Element.ALIGN_CENTER);// 设置内容水平居中显示  
                    cellValue.VerticalAlignment = (Element.ALIGN_MIDDLE);  // 设置垂直居中  
                    cellValue.Padding = 5;

                    table.AddCell(cellValue);
                }
            }

            if (PrintFooterList != null)
            {
                foreach (var item in PrintFooterList)
                {
                    item.Title = item.Title ?? "";
                    item.Value = item.Value ?? "";
                    PdfPCell cellValue = new PdfPCell(new Paragraph(item.Title + item.Value, font));
                    cellValue.Colspan = item.Colspan;
                    cellValue.HorizontalAlignment = (Element.ALIGN_LEFT);// 设置内容水平居左显示  
                    cellValue.VerticalAlignment = (Element.ALIGN_MIDDLE);  // 设置垂直居中  
                    cellValue.Padding = 5;
                    if (item.HideBorder)
                    {
                        cellValue.Border = 0;
                    }
                    table.AddCell(cellValue);

                }
            }
            //table.SpacingAfter = 30;
            table.SpacingBefore = 5;
            return table;
        }
    }

    public class PrintCell
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public int Colspan { get; set; }
        public bool HideBorder { get; set; }
        public int? Align { get; set; }
    }
}
