using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using Microsoft.Practices.Unity;
using YK.PropertyMgr.RepositoryContract;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.ApplicationDTO.Enums;

namespace YK.PropertyMgr.DomainService
{
    public partial class NotificeConfigDomainService
    {
        public NotificeConfig GetNotificeConfigByComDeptId(int comDeptId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var config = propertyMgrUnitOfWork.NotificeConfigRepository.GetAll().Where(p => p.ComDeptId == comDeptId).FirstOrDefault();
                if (config == null)
                {
                    config = new NotificeConfig();
                    config.ComDeptId = comDeptId;
                    config.APPNotice = true;
                    config.ArrearsAmount = 500;
                    config.IsEnable = (int)EnableEnum.N;
                    config.ArrearsMonth = 3;
                    config.FrequencyType = (int)FrequencyTypeEnum.Weekly;
                    config.NoticeTime = 14;
                    config.NoticeDay = 1;//周一
                    config.Id = 0;
                    config.SMSNotice = false;
                }
                return config;
            }
        }
    }
}
