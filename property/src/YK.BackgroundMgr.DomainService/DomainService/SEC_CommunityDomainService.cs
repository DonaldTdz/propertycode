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
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using YK.BackgroundMgr.ApplicationDTO;


namespace YK.BackgroundMgr.DomainService
{
    public partial class SEC_CommunityDomainService
    {
        public SEC_Community GetCommunityById(int communityDeptId)
        {

            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {

                var query = from CommunityDept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                            join Community in _BackgroundMgrUnitOfWork.SEC_CommunityRepository.GetAll() on CommunityDept.Id equals Community.DeptId
                            where CommunityDept.DeptType == (int)EDeptType.XiaoQu
                            where CommunityDept.Id == communityDeptId
                            select Community;

                return query.ToList().First();

            }
        }

        public IList<SEC_Community> GetCommunityList(string villageName)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {

                var query = from Community in _BackgroundMgrUnitOfWork.SEC_CommunityRepository.GetAll()

                            where Community.Name.Contains(villageName)
                            select Community;

                return query.ToList();

            }
        }
        //public IList<SEC_Community> GetCommunityListByCity(string cityName)
        //{
        //    using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
        //    {
        //        var cityList = _BackgroundMgrUnitOfWork.SEC_AreaRepository.GetAll().Where(c => c.Name.Contains(cityName) && c.AreaType == 2);
        //        if (cityList != null)
        //        {
        //            List<string> ids = new List<string>();
        //            foreach (var city in cityList)
        //            {
        //                if (!ids.Contains(city.Id.ToString()))
        //                {
        //                    ids.Add(city.Id.ToString());
        //                }
        //            }
        //            var query = _BackgroundMgrUnitOfWork.SEC_CommunityRepository.GetAll().Where(d => ids.Contains(d.City));
        //            return query.ToList();
        //        }
        //        return new List<SEC_Community>();
        //    }
           
        //}
        public IList<SEC_Community> GetCommunityListByCity(string cityName)
        {

            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {

                var query = from Community in _BackgroundMgrUnitOfWork.SEC_CommunityRepository.GetAll()

                            where Community.City.Contains(cityName)
                            select Community;

                return query.ToList();

            }
        }
    }
}

