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
namespace YK.BackgroundMgr.DomainService
{
    public partial class SEC_BuildingDomainService
    {

        public List<BuildingInfo> GetBuildsInfoByBuildDeptId(List<int >BuildDeptIdList)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var BuildList = (from build in _BackgroundMgrUnitOfWork.SEC_BuildingRepository.GetAll().Where(o => BuildDeptIdList.Contains(o.DeptId.Value))
                                 select new BuildingInfo
                                 {
                                     DeptId = build.DeptId,
                                     Building_code = build.Building_code,
                                     Building_name = build.Building_name

                                 }
                                 ).ToList();
            
                return BuildList;
                
                
            }
        }



    }
}
