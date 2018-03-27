using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YK.BackgroundMgr.PresentationService;
using Microsoft.Practices.Unity;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using YK.PropertyMgr.ApplicationDTO.Enums;
using System.Timers;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainInterface;

namespace YK.PropertyMgr.CompositeDomainService.NoticeService
{
    public class NoticeTaskService
    {
        #region 单例

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NoticeTaskService Instance { get { return SingletonInstance; } }

        private static readonly NoticeTaskService SingletonInstance = new NoticeTaskService();

        #endregion

        #region 属性

        Timer intervalTimer;
        Timer startTimer;

        #endregion

        #region 获取小区配置 和 发送通知信息

        /// <summary>
        /// 获取满足当前条件的配置
        /// </summary>
        /// <returns></returns>
        private IList<NotificeConfig> GetNoticeConfigList()
        {
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var monthEndDay = (new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)).AddMonths(1).AddDays(-1).Day;
                var monthDay = (monthEndDay == DateTime.Today.Day ? 0 : DateTime.Today.Day);
                var weekDay = DateTime.Today.DayOfWeek.GetHashCode();
                var hour = DateTime.Now.Hour;
                var query = pmUnitWork.NotificeConfigRepository.GetAll()
                            .Where(p => p.IsEnable == (int)EnableEnum.Y
                            && (p.APPNotice == true || p.SMSNotice == true)
                            && ((p.FrequencyType == (int)FrequencyTypeEnum.Month && p.NoticeDay == monthDay)
                            || (p.FrequencyType == (int)FrequencyTypeEnum.Weekly && p.NoticeDay == weekDay)
                            ) && p.NoticeTime == hour);

                return query.ToList();
            }
        }

        /// <summary>
        /// 根据配置组装通知信息
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private NoticeMsg GetNoticeMsg(NotificeConfig config)
        {
            NoticeMsg msg = new NoticeMsg();
            msg.ComDeptId = config.ComDeptId;
            msg.ComDeptName = config.ComDeptName;
            msg.Content = config.Content;
            msg.NoticeType = NoticeTypeEnum.Arrears;
            msg.Title = config.ComDeptName;
            msg.UserPhones = GetUserPhonesByHouseDeptIds(GetHouseDeptIdArrByConfig(config));
            return msg;
        }

        /// <summary>
        /// 跟进房屋DeptId 获取用户手机号
        /// </summary>
        /// <param name="houseDeptIdArr"></param>
        /// <returns></returns>
        private string[] GetUserPhonesByHouseDeptIds(int?[] houseDeptIdArr)
        {
            if (houseDeptIdArr.Length == 0)
            {
                return new string[0];
            }
            return DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetUserPhonesByHouseDeptIds(houseDeptIdArr);
        }

        /// <summary>
        /// 获取满足条件的房屋DeptId列表
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private int?[] GetHouseDeptIdArrByConfig(NotificeConfig config)
        {
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var houseDeptIds = new List<int?>();
                int month = config.ArrearsMonth ?? 0;
                decimal money = config.ArrearsAmount ?? 0;
                var billQuery = from p in pmUnitWork.ChargBillRepository.GetAll()
                                where p.Status == (int)BillStatusEnum.NoPayment
                                && p.ComDeptId == config.ComDeptId
                                && p.IsDel == false    //排除作废账单   2017-8-31
                                && p.IsDevPay == false //排除开发商代缴 2017-8-31
                                select p;
                //欠费月数
                if (month != 0)
                {
                    //按月份算
                    //当前日期为5-5，“欠费月数”为3，如果业主有账单的结束时间小于3-1就满足通知条件。
                    var endDate = DateTime.Today.AddMonths(-1 * (month - 1));//除去本月
                    endDate = new DateTime(endDate.Year, endDate.Month, 1);
                    var monthList = (from b in billQuery
                                     where b.EndDate < endDate 
                                     select b.HouseDeptId).Distinct().ToList();
                    if (monthList.Count() > 0)
                    {
                        houseDeptIds.AddRange(monthList);
                    }
                }
                //欠费金额
                if (money != 0)
                {
                    var moneyList = (from b in billQuery
                                     group new { b.HouseDeptId, b.BillAmount, b.ReceivedAmount }
                                     by b.HouseDeptId into gb
                                     select new
                                     {
                                         houseDeptId = gb.Key,
                                         amount = gb.Sum(g => g.BillAmount - g.ReceivedAmount)
                                     })
                                     .Where(a => a.amount >= money)
                                     .Select(b => b.houseDeptId)
                                     .ToList();
                    if (moneyList.Count() > 0)
                    {
                        houseDeptIds.AddRange(moneyList);
                    }
                }
                return houseDeptIds.Distinct().ToArray();
            }
        }

        /// <summary>
        /// 循环发送欠费通知
        /// </summary>
        public void SendArrearsMsg(bool IsTaskRun = true)
        {
            var noticeList = GetNoticeConfigList();
            foreach (var item in noticeList)
            {
                var msg = GetNoticeMsg(item);
                if (msg.UserPhones.Length > 0)
                {
                    if (IsTaskRun)
                    {
                        Task.Run(() =>
                        {
                            //发送APP欠费通知
                            if (item.APPNotice.HasValue && item.APPNotice.Value)
                            {
                                APPNoticeService.Instance.SendArrearsNoticeMsg(msg);
                            }
                            //发送短信欠费通知
                            if (item.SMSNotice.HasValue && item.SMSNotice.Value)
                            {
                                SMSNoticeService.Instance.SendArrearsNoticeMsg(msg);
                            }
                        });
                    }
                    else
                    {
                        //发送APP欠费通知
                        if (item.APPNotice.HasValue && item.APPNotice.Value)
                        {
                            APPNoticeService.Instance.SendArrearsNoticeMsg(msg);
                        }
                        //发送短信欠费通知
                        if (item.SMSNotice.HasValue && item.SMSNotice.Value)
                        {
                            SMSNoticeService.Instance.SendArrearsNoticeMsg(msg);
                        }
                    }
                }
            }
        }

        #endregion

        #region 循环运行

        /// <summary>
        /// 整点循环
        /// </summary>
        public void FullPointRun()
        {
            var diff = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0)).AddHours(1) - DateTime.Now;
            var interval = diff.Minutes * 60 + diff.Seconds + 1;
            //var interval = ((new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0)).AddMinutes(1) - DateTime.Now).Seconds + 1;
            startTimer = new Timer();
            startTimer.Elapsed += new ElapsedEventHandler(OnFullTimedEvent);
            startTimer.Interval = interval*1000;
            startTimer.Enabled = true;
            startTimer.AutoReset = false;
            LogProperty.WriteLoginToFile(string.Format("startTimer.Interval = {0}", interval), "NoticeTaskRun", FileLogType.Info);
        }

        private void OnFullTimedEvent(object source, ElapsedEventArgs e)
        {
            LogProperty.WriteLoginToFile("整点循环", "NoticeTaskRun", FileLogType.Info);
            Run();
            //startTimer.Enabled = false;
            //startTimer.Stop();
        }

        public void Run()
        {
            // 在应用程序启动时运行的代码  
            if (intervalTimer == null)
            {
                intervalTimer = new Timer();
                intervalTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                //intervalTimer.Interval = 60000; //间隔1分钟循环 
                intervalTimer.Interval = 3600000;//一个小时
                intervalTimer.Enabled = true;
                intervalTimer.AutoReset = true;
            }
            else
            {
                intervalTimer.Enabled = true;
            }
            
            LogProperty.WriteLoginToFile("欠费通知循环开始", "NoticeTaskRun", FileLogType.Info);
            SendArrearsMsg(true);
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            LogProperty.WriteLoginToFile("欠费通知循环", "NoticeTaskRun", FileLogType.Info);
            SendArrearsMsg(true);
        }

        #endregion

    }
}
