using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.Crosscuting
{
    public class StringHelper
    {
        public static string StrPadRightFormat(string str,int len, string rStr)
        {
            return str.PadRight(len, ' ').Replace(" ", rStr);
        }
    }
}
