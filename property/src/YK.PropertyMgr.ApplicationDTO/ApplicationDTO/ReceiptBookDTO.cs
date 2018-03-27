using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public partial class ReceiptBookDTO
    {
        /// <summary>
        /// 类型字符串
        /// </summary>
        public string ReceiptBookTypeStr { get; set; }

        private string receiptCurrentNumberView;
        public string ReceiptCurrentNumberView
        {
            get
            {
                if (!string.IsNullOrEmpty(receiptCurrentNumberView))
                {
                    return receiptCurrentNumberView;
                }
                if (CurrentReceiptNum == null)
                {
                    return string.Empty;
                }

                    if (CurrentReceiptNum == string.Empty)
                {

                    var CurrentCode = this.BeginCode;
                    string CurrentCodestr = CurrentCode.Value.ToString().PadLeft(this.Suffix.Value, '0');
                    receiptCurrentNumberView = this.Prefix + CurrentCodestr;
                    return receiptCurrentNumberView;
                }
                else
                {//加一

                    
                    var CurrentInt = Convert.ToInt32(this.CurrentReceiptNum.Remove(0, this.Prefix.Length)) + 1;
                    if (CurrentInt > this.EndCode)
                    {
                        receiptCurrentNumberView = this.CurrentReceiptNum;
                    }
                    else
                    {
                        string CurrentCodestr = CurrentInt.ToString().PadLeft(this.Suffix.Value, '0');
                        receiptCurrentNumberView = this.Prefix + CurrentCodestr;

                    }

                  
                    return receiptCurrentNumberView;

                }
                 
            }

            set
            {
                receiptCurrentNumberView = value;
            }
        }



        public string receiptCurrentNumberGridView
        {
            get {
                
                if (CurrentReceiptNum == null)
                {
                    return string.Empty;
                }

                if (CurrentReceiptNum == string.Empty)
                {

                    var CurrentCode = this.BeginCode;
                    string CurrentCodestr = CurrentCode.Value.ToString().PadLeft(this.Suffix.Value, '0');
                    receiptCurrentNumberView = this.Prefix + CurrentCodestr;
                    return receiptCurrentNumberView;
                }
                else
                {//加一
                     

                    var CurrentInt = Convert.ToInt32(this.CurrentReceiptNum.Remove(0, this.Prefix.Length));
                    if (CurrentInt == this.EndCode)
                    {
                        return "已用完";
                    }
                    else
                    {
                        CurrentInt += 1;
                        string CurrentCodestr = CurrentInt.ToString().PadLeft(this.Suffix.Value, '0');
                        return this.Prefix + CurrentCodestr;

                    }

                    


                }
            }
        }

        public bool IsStatusPrompt { get; set; }
        /// <summary>
        /// 类型字符串
        /// </summary>
        public string StatusStr { get; set; }

    
    }
}
