using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;

namespace YK.PropertyMgr.ApplicationService
{
    public partial class NotificeConfigAppService
    {
        public NotificeConfigDTO GetNotificeConfigByComDeptId(int deptId, string deptName)
        {
            var domainNotificeConfig = NotificeConfigService.GetNotificeConfigByComDeptId(deptId);
            domainNotificeConfig.ComDeptName = deptName;
            return NotificeConfigMappers.ChangeNotificeConfigToDTO(domainNotificeConfig); 
        }

        public ReturnResult SaveNotice(NotificeConfigDTO input)
        {
            var domainEntity = NotificeConfigMappers.ChangeDTOToNotificeConfigNew(input);
            bool resultBool = false;
            if (domainEntity.Id == 0)
            {
                domainEntity.CreateTime = DateTime.Now;
                domainEntity.UpdateTime = DateTime.Now;
                resultBool = NotificeConfigService.InsertNotificeConfig(domainEntity);
            }
            else
            {
                domainEntity.UpdateTime = DateTime.Now;
                resultBool = NotificeConfigService.UpdateNotificeConfig(domainEntity);
            }
            if (resultBool)
            {
                return new ReturnResult() { IsSuccess = resultBool, Msg = "欠费通知设置成功", Data = domainEntity.Id };
            }
            return new ReturnResult() { IsSuccess = resultBool, Msg = "欠费通知设置失败" };
        }
    }
}
