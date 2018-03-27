using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.Crosscuting
{
  public static class ObjectHelper
    {
        #region 方法
        /// <summary>
        /// 比较--两个类型一样的实体类对象的值
        /// </summary>
        /// <param name="oneT"></param>
        /// <returns></returns>
        public  static string CompareTypeString<T>(T oneT, T twoT)
        {
            #region
            string Remark = string.Empty;
            Type typeOne = oneT.GetType();
            Type typeTwo = twoT.GetType();
            //如果两个T类型不一样  就不作比较
            if (!typeOne.Equals(typeTwo)) { return ""; }
            PropertyInfo[] pisOne = typeOne.GetProperties(); //获取所有公共属性(Public)
            PropertyInfo[] pisTwo = typeTwo.GetProperties();
            //如果长度为0返回false
            if (pisOne.Length <= 0 || pisTwo.Length <= 0)
            {
                return "";
            }
            //如果长度不一样，返回false
            if (!(pisOne.Length.Equals(pisTwo.Length))) { return ""; }
            //遍历两个T类型，遍历属性，并作比较
            for (int i = 0; i < pisOne.Length; i++)
            {
                //获取属性名
                string oneName = pisOne[i].Name;
                string twoName = pisTwo[i].Name;
                //获取属性的值
                object oneValue = pisOne[i].GetValue(oneT, null);
                object twoValue = pisTwo[i].GetValue(twoT, null);
                //比较,只比较值类型
                if ((pisOne[i].PropertyType.IsValueType || pisOne[i].PropertyType.Name.StartsWith("String")) && (pisTwo[i].PropertyType.IsValueType || pisTwo[i].PropertyType.Name.StartsWith("String")))
                {

                    if (oneValue != null && twoValue == null)
                    {
                        Remark += "属性" + oneName + "值由" + oneValue + "更改为";
                    }
                    else if (oneValue == null && twoValue != null)
                    {
                        Remark += "属性" + oneName + "值由更改为" + twoValue;
                    }
                    else if(oneValue != null && twoValue != null)
                    {
                        if (!oneValue.Equals(twoValue))
                        {
                            Remark += "属性" + oneName + "值由" + oneValue + "更改为" + twoValue;
                        }
                    }
                    
                }
            }
            return Remark;
            #endregion
        }
        #endregion
    }
}
