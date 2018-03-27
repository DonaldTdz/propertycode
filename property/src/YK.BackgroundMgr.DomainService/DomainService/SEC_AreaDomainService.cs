using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;
using YK.BackgroundMgr.RepositoryContract;
using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.Crosscuting;
using YK.BackgroundMgr.PresentationService;
using System.Data.Entity.SqlServer;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using System.Linq.Expressions;
using YK.BackgroundMgr.ApplicationMapper;
using YK.BackgroundMgr.ApplicationDTO;

namespace YK.BackgroundMgr.DomainService
{
   public partial class SEC_AreaDomainService
    {
        public IList<SEC_AreaDTO> GetSEC_AreaList(Expression<Func<SEC_Area, bool>> predicate)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                return  SEC_AreaMappers.ChangeSEC_AreaToDTOs(_BackgroundMgrUnitOfWork.SEC_AreaRepository.GetAll().Where(predicate).ToList());
            }
        }
    }
}
