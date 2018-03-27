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
    public class APPNoticeService : NoticeDecorator
    {
        #region 单例

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static APPNoticeService Instance { get { return SingletonInstance; } }

        private static readonly APPNoticeService SingletonInstance = new APPNoticeService();

        #endregion

        public APPNoticeService(INoticeService _service) : base(_service)
        {
        }
        public APPNoticeService()
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
            MessageInfo msgInfo = new MessageInfo()
            {
                Title = "欠费通知",
                //【小区名称】尊敬的业主您好，您的物业管理费已欠费，请及时进行缴纳，谢谢。点击可查看详情。
                Content = string.Format("【{0}】尊敬的业主您好，您的物业管理费已欠费，请及时进行缴纳，谢谢！", msg.ComDeptName),
                ActionUrl = "" //注：需要跳转到物业APP缴费页面
            };

            HttpClientService service = new HttpClientService();
            try
            {
                service.SendPushWithJson(msg.UserPhones, msgInfo, msg.ComDeptId.ToString());
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[欠费APP通知]ComDeptId:{0} ComDeptName:{1} Phones:{2} Error:{3}", msg.ComDeptId.ToString(), msg.ComDeptName, msg.UserPhones, ex.Message), "SendPushWithJson", FileLogType.Exception);
            }
        }
    }
}
