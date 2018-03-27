using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.Crosscuting;

namespace YK.PropertyMgr.ApplicationService
{
    public class TemplateModelHelper
    {
        public static IEnumerable<TemplateModel> GetTemplateModels(Type type, TemplateColumn[] showColumns)
        {
            List<TemplateModel> templateList = new List<TemplateModel>();
            var Properties = type.GetProperties();
            var propertie = Properties.Where(p => p.Name.ToLower() == "id").FirstOrDefault();
            if (propertie != null)
            {
                TemplateModel model = new TemplateModel();
                model.EnName = propertie.Name;
                model.CnName = propertie.Name;
                model.DictId = "-1";
                model.Seq = 0;
                model.Type = GetTemplateType(propertie);
                model.IsListColumn = false;
                model.Field = propertie.Name;
                templateList.Add(model);
            }
            var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
            foreach (PropertyInfo p in Properties)
            {
                var column = showColumns.Where(s => s.ColumnName == p.Name).FirstOrDefault();
                if (column != null)
                {
                    TemplateModel model = new TemplateModel();
                    model.EnName = p.Name;
                    model.CnName = column.ColumnDesc;
                    model.Seq = column.Seq;
                    model.ElementType = column.ElementType;
                    model.DictId = column.DictId;
                    model.IsRequred = column.IsRequred;
                    model.MaxLength = column.MaxLength;
                    model.Regular = column.Regular;
                    model.ValidateType = column.ValidateType;
                    model.Rowspan = column.Rowspan;
                    model.Colspan = column.Colspan;

                    if (!string.IsNullOrEmpty(column.Type))
                    {
                        model.Type = column.Type;
                    }
                    else
                    {
                        model.Type = GetTemplateType(p);
                    }

                    if (column.DictId != "-1")
                    {
                        model.DictId = column.DictId;
                        model.Type = "dict";
                        model.ElementType = "Select";
                        model.DictionaryModels = propertyService.GetDictionaryModels(model.DictId);
                    }
                    if (column.IsListColumn.HasValue)
                    {
                        model.IsListColumn = column.IsListColumn.Value;
                    }
                    else
                    {
                        model.IsListColumn = true;
                    }


                 

                    model.Field = column.ColumnName;

                    model.IsAttr = column.IsAttr;
                    if (model.IsAttr)
                    {
                        model.Attrs = column.Attrs;
                    }
                    model.IsExport = column.IsExport;

                    if (model.IsExport)
                    {
                        if (column.Type == "date")
                        {
                            model.ExportFormat = "yyyy-MM-dd";
                        }
                        if (column.Type == "datetime")
                        {
                            model.ExportFormat = "yyyy-MM-dd HH:mm";
                        }
                    }

                    templateList.Add(model);
                }
            }

            return templateList.OrderBy(t => t.Seq);
        }

        private static string GetTemplateType(PropertyInfo p)
        {
            if (p.PropertyType.Name.ToLower() == "string")
            {
                return "string";
            }
            if (p.PropertyType.Name.Contains("Int32") || p.PropertyType.Name.Contains("Int64") || p.PropertyType.Name.Contains("Int16"))
            {
                return "int";
            }
            if (p.PropertyType.Name.Contains("Decimal"))
            {
                return "double";
            }
            if (p.PropertyType.Name.Contains("Single"))
            {
                return "double";
            }
            if (p.PropertyType.Name.Contains("Double"))
            {
                return "double";
            }
            if (p.PropertyType.Name.Contains("Boolean"))
            {
                return "bool";
            }
            if (p.PropertyType.FullName.Contains("DateTime"))
            {
                return "datetime";
            }
            return "string";
        }
        /// <summary>
        /// 获取打印模板
        /// </summary>
        /// <param name="ComDeptId">物业ID</param>
        /// <param name="printTemp">模板类型</param>
        /// <returns></returns>
        public static PrintTemplateEnum GetPrintTemplate(int ComDeptId, EPrintTemplate printTemp, ref string propertyName)
        {
            PrintTemplateEnum defaultTemp = PrintTemplateEnum.TemplateOne;
            try
            {
                var service = DomainInterfaceHelper.LookUp<IPropertyDomainService>();
                var property = service.GetSECProperty(ComDeptId);
                propertyName = property.PropertyName;

                if (property != null)
                {
                    string strPriTem = property.PrintTemplate;

                    if (!string.IsNullOrEmpty(strPriTem))
                    {
                        var temps = strPriTem.Split(';');
                        var dic = temps.ToDictionary(
                            key => (EPrintTemplate)Enum.ToObject(typeof(EPrintTemplate), Convert.ToInt32(key.Split(',')[0])),
                            value => (PrintTemplateEnum)Enum.ToObject(typeof(PrintTemplateEnum), Convert.ToInt32(value.Split(',')[1])));
                        if (dic.ContainsKey(printTemp))
                            defaultTemp = dic[printTemp];
                    }
                }
        }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("ComDeptId:{0} error msg:{1}", ComDeptId, ex.Message), "GetPrintTemplate", FileLogType.Exception);
            }
           
            return defaultTemp;
        }
    }

    public class TemplateColumn
    {
        public TemplateColumn()
        {
            DictId = "-1";
            //ValidateType = string.Empty;
        }
        public string ColumnName { get; set; }

        public string ColumnDesc { get; set; }

        public int Seq { get; set; }

        public string DictId { get; set; }

        public bool? IsListColumn { get; set; }
        public string Type { get; set; }

        public string ElementType { get; set; }
        public bool IsRequred { get; set; }
        public int MaxLength { get; set; }

        public string Regular { get; set; }
        public string ValidateType { get; set; }


        public bool IsExport { get; set; }
        public bool IsAttr { get; set; }

        public Attr[] Attrs { get; set; }
        public int Colspan { get; set; }
        public int Rowspan { get; set; }
    }
}
