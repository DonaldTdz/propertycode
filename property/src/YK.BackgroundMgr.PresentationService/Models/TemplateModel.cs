using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.FrameworkTools.ExcelService;

namespace YK.BackgroundMgr.PresentationService
{
    /*
         <Field>field</Field>
      <EnName>英文名称</EnName>
      <CnName>中文名称</CnName>
      <Type>C#Type:string,int,datetime,bool</Type>
      <DictId>数据字典名称</DictId>
      <ExportComments>导出模版备注</ExportComments>
      <Seq>排序字段</Seq>
      <ElementType>UI控件名称:TextBox,Lable,Select,CheckBox</ElementType>
      <IsRequred>是否必填</IsRequred>
      <CnPlaceHolder>中文Placeholder名称</CnPlaceHolder>
      <EnPlaceHolder>英文Placeholder名称</EnPlaceHolder>
      <MaxLength>最大长度</MaxLength>
      <IsListColumn>是否在列表中显示</IsListColumn>
      <IsSearchColumn>是否在列表查询中显示</IsSearchColumn>
      <Regular>表单正则表达式验证</Regular>
      <ValidateType>验证类型（需要和前端UI对应）</ValidateType>
         */
    [Serializable]
    public class TemplateModel:ExcelTemplateModel
    {
        public TemplateModel()
        {
            ValidateType = "";
        }
        public string ElementType { get; set; }
        public bool IsOrderColumn { get; set; }
        public string Regular { get; set; }
        public bool SkipSpecialCheck { get; set; }
        public bool IsAttr { get; set; }

        public Attr[] Attrs { get; set; }
    }

    public class Attr
    {
        public string Name { get; set; }

        public string Val { get; set; }

        public Attr(string name, string val)
        {
            Name = name;
            Val = val;
        }
    }

    public class DictionaryModel:ExcelDictionaryModel
    {
        public int Id { get; set; }
        public string Order { get; set; }
        public bool IsUsed { get; set; }
    }
}
