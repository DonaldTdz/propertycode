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

namespace YK.BackgroundMgr.DomainService
{
    public partial class SEC_HouseDomainService
    {
        public List<SEC_House> GetHouseInfosByCommunityDeptId(string communityDeptId)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var query = from houseDept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                            join house in _BackgroundMgrUnitOfWork.SEC_HouseRepository.GetAll() on houseDept.OrgId equals house.OrgId
                            where houseDept.DeptType == (int)EDeptType.FangWu && houseDept.Code.Contains("." + communityDeptId + ".")
                            select house;

                return query.ToList();
            }
        }

        public List<HouseInfo> GetHouseListByCommunityDeptId(string communityDeptId)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                int cid = int.Parse(communityDeptId);
                var community = _BackgroundMgrUnitOfWork.SEC_CommunityRepository.GetAll().Where(c => c.DeptId == cid).FirstOrDefault();
                bool unsoldCharge = false;
                if (community != null)
                {
                    unsoldCharge = community.UnsoldCharge == 1 ? true : false;
                }
                var query = from houseDept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                            join house in _BackgroundMgrUnitOfWork.SEC_HouseRepository.GetAll() on houseDept.OrgId equals house.OrgId
                            where houseDept.DeptType == (int)EDeptType.FangWu && houseDept.Code.Contains("." + communityDeptId + ".")
                            select new HouseInfo()
                            {
                                Id = houseDept.Id,
                                HouseDeptID = houseDept.Id,
                                BuildArea = house.BuildArea,
                                HouseInArea = house.HouseInArea,
                                HouseStatus = house.HouseState_PM,
                                UnsoldIsBindDeveloper = unsoldCharge,
                                DoorNo = house.DoorNo
                            };


                return query.ToList();
            }
        }

        public List<HouseInfo> GetHouseListByHouseDeptIds(List<int?> deptIds)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {

                var query = from houseDept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                            join house in _BackgroundMgrUnitOfWork.SEC_HouseRepository.GetAll() on houseDept.OrgId equals house.OrgId
                            where houseDept.DeptType == (int)EDeptType.FangWu && deptIds.Contains(houseDept.Id)
                            select new HouseInfo()
                            {
                                Id = houseDept.Id,
                                HouseDeptID = houseDept.Id,
                                DoorNo = house.DoorNo,
                                BuildArea = house.BuildArea,
                                HouseInArea = house.HouseInArea,
                                HouseStatus = house.HouseState_PM,
                            };

                _BackgroundMgrUnitOfWork.SEC_DeptRepository.DatabaseContext.Database.CommandTimeout = 360;
                return query.ToList();
            }
        }





        public HouseInfo GetHouseInfo(string communityDeptId, int houseDeptID, bool isGetName = false)
        {
            int cid = int.Parse(communityDeptId);
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var query = (from houseDept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                             join house in _BackgroundMgrUnitOfWork.SEC_HouseRepository.GetAll() on houseDept.OrgId equals house.OrgId
                             //where (houseDeptID == 0 || houseDept.Id == houseDeptID) //排除0 2017-5-5 线上问题对外收费票据打不出
                             where houseDept.Id == houseDeptID
                             where houseDept.DeptType == (int)EDeptType.FangWu && houseDept.Code.Contains("." + communityDeptId + ".")
                             select new HouseInfo()
                             {
                                 Id = houseDept.Id,
                                 HouseDeptID = houseDept.Id,
                                 BuildArea = house.BuildArea,
                                 HouseInArea = house.HouseInArea,
                                 DoorNo = house.DoorNo,
                                 HouseStatus = house.HouseState_PM
                             }).FirstOrDefault();
                var unsoldCharge = _BackgroundMgrUnitOfWork.SEC_CommunityRepository.GetAll().Where(c => c.DeptId == cid).FirstOrDefault();
                if (query != null && unsoldCharge != null)
                {
                    query.UnsoldIsBindDeveloper = unsoldCharge.UnsoldCharge == 1 ? true : false;
                }
                if (isGetName)
                {
                    var communityDept = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(s => s.Id == cid).FirstOrDefault();
                    if (communityDept != null && query != null)
                    {
                        query.CommunityName = communityDept.Name;
                        var propertyDept = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(s => s.Id == communityDept.PId).FirstOrDefault();
                        if (propertyDept != null)
                        {
                            query.PropertyName = propertyDept.Name;
                        }
                    }
                }

                return query;
            }
        }

        public int GetCommunityDeptIdByHouseDeptId(int houseDeptID)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var query = (from houseDept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                             join house in _BackgroundMgrUnitOfWork.SEC_HouseRepository.GetAll() on houseDept.OrgId equals house.OrgId
                             where houseDept.Id == houseDeptID
                             where houseDept.DeptType == (int)EDeptType.FangWu
                             select new
                             {
                                 Id = houseDept.Id,
                                 HouseDeptID = houseDept.Id,
                                 BuildArea = house.BuildArea,
                                 HouseInArea = house.HouseInArea,
                                 DoorNo = house.DoorNo,
                                 Code = houseDept.Code
                             }).FirstOrDefault();
                int?[] codes = Array.ConvertAll(query.Code.Split('.').Where(s => !string.IsNullOrEmpty(s)).ToArray(), new Converter<string, int?>(s => int.Parse(s)));

                int cid = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                    .Where(s => s.DeptType == (int)EDeptType.XiaoQu && codes.Contains(s.Id)).FirstOrDefault().Id.Value;

                return cid;
            }
        }




     





        public List<AppHouseView> GetHouseListByDoorNo(string communityDeptId, string doorNo)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var query = from houseDept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                            join house in _BackgroundMgrUnitOfWork.SEC_HouseRepository.GetAll() on houseDept.OrgId equals house.OrgId
                            where houseDept.DeptType == (int)EDeptType.FangWu
                            && house.DoorNo.Contains(doorNo)
                            && houseDept.Code.Contains("." + communityDeptId + ".")
                            select new AppHouseView()
                            {
                                HouseDeptID = houseDept.Id,
                                HouseDoorNo = house.DoorNo
                            };

                return query.ToList();
            }
        }

        public HouseInfo GetHouseInfoByHouseNo(string communityDeptId, string houseNo)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var query = from houseDept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                            join house in _BackgroundMgrUnitOfWork.SEC_HouseRepository.GetAll() on houseDept.OrgId equals house.OrgId
                            where houseDept.DeptType == (int)EDeptType.FangWu
                            && house.DoorNo == houseNo
                            && houseDept.Code.Contains("." + communityDeptId + ".")
                            select new HouseInfo()
                            {
                                Id = houseDept.Id,
                                HouseDeptID = houseDept.Id,
                                BuildArea = house.BuildArea,
                                HouseInArea = house.HouseInArea,
                                DoorNo = house.DoorNo
                            };

                return query.FirstOrDefault();
            }
        }
    }
}
