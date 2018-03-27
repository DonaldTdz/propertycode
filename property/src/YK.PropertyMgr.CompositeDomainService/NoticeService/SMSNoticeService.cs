using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.Crosscuting;

namespace YK.PropertyMgr.CompositeDomainService.NoticeService
{
    public class SMSNoticeService : NoticeDecorator
    {
        #region 单例

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static SMSNoticeService Instance { get { return SingletonInstance; } }

        private static readonly SMSNoticeService SingletonInstance = new SMSNoticeService();

        #endregion
        public SMSNoticeService(INoticeService _service) : base(_service)
        {
        }
        public SMSNoticeService()
        {

        }
        public override void SendNoticeMsg(NoticeMsg msg)
        {
            if (this._service != null)
            {
                this._service.SendNoticeMsg(msg);
            }
            switch (msg.NoticeType)
            {
                case NoticeTypeEnum.Arrears:
                    {
                        SendArrearsNoticeMsg(msg);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 发送欠费通知
        /// </summary>
        /// <param name="msg"></param>
        public void SendArrearsNoticeMsg(NoticeMsg msg)
        {
            HttpClientService service = new HttpClientService();
            try
            {
                //content先固定 2017-06-19
                //【逸社区】尊敬的【XX】业主您好，您的物业费已欠费，请及时到物业服务中心或通过逸社区APP进行缴纳，感谢您的理解和支持，谢谢。
                var content = "尊敬的[{0}]业主您好，您的物业费已欠费，请及时到物业服务中心或通过逸社区APP进行缴纳，感谢您的理解和支持，谢谢！";

                //foreach (var phone in msg.UserPhones)
                //{
                //    msg.Content = string.Format(content, msg.ComDeptName);
                //service.SMSSend(phone, "逸社区", msg.Content);
                //service.SMSSend(phone, msg.ComDeptName, msg.Content);
                //}

                if (msg.UserPhones == null || msg.UserPhones.Where(u => !string.IsNullOrEmpty(u)).Count() == 0)
                {
                    LogProperty.WriteLoginToFile(string.Format("[欠费短信通知]ComDeptId:{0} ComDeptName:{1} Phones:{2} msg:{3}", msg.ComDeptId, msg.ComDeptName, msg.UserPhones, "手机号为空"), "SMSSend", FileLogType.Info);
                    return;
                }

                //改为收费短信 2017-8-23
                SmsEntityModel model = new SmsEntityModel();
                model.Content = string.Format(content, msg.ComDeptName);
                model.IsPay = true; //付费
                model.Phones = string.Join(",", msg.UserPhones.Where(u => !string.IsNullOrEmpty(u)).ToArray());//排除空手机号
                model.RequestFrom = ERequestFrom.物业收费;
                model.RequestScope = "欠费通知";
                model.SmsAccountId = msg.ComDeptId.ToString(); //账号小区
                model.Title = "逸社区";
                service.SmsSendWithAccount(model);
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[欠费短信通知]ComDeptId:{0} ComDeptName:{1} Phones:{2} Error:{3} Exception：{4}", msg.ComDeptId, msg.ComDeptName, string.Join(",", msg.UserPhones), ex.Message, ex), "SMSSend", FileLogType.Exception);
            }
        }
    }
}
