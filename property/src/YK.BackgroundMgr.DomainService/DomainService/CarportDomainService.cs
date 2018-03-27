using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using YK.ParkingSys.RepositoryContract;
using YK.ParkingSys.DomainEntity;
using YK.BackgroundMgr.Crosscuting;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using YK.BackgroundMgr.PresentationService;
using YK.BackgroundMgr.RepositoryContract;
using System.Data.Entity.SqlServer;
using YK.BackgroundMgr.DomainEntity;

namespace YK.BackgroundMgr.DomainService
{
    public partial class CarportDomainService
    {
    

        public List<ParkingSpaceInfo> GetParkingSpaceListByCommunityDeptId(int communityDeptId)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var query = from p in _ParkingSysUnitOfWork.ParkingRepository.GetAll()
                            join c in _ParkingSysUnitOfWork.CarportRepository.GetAll() on p.Parking_id equals c.Parking_id
                            where p.Dept_id == communityDeptId
                            select new ParkingSpaceInfo()
                            {
                                ParkingId = p.Parking_id,
                                HouseDeptID = c.HouseDeptID,
                                ParkingSpaceId = c.Id,
                                CarportNum = c.CarportNum,
                                Area = c.Area
                            };
                return query.ToList();
            }
        }
        public List<ParkingSpaceInfo> GetParkingSpaceListByResourcesId(int ResourcesId)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var query = from  c in _ParkingSysUnitOfWork.CarportRepository.GetAll() 
                                  join p in _ParkingSysUnitOfWork.ParkingRepository.GetAll() on c.Parking_id equals p.Parking_id
                            where c.Id== ResourcesId
                            select new ParkingSpaceInfo()
                            {
                                ParkingId = p.Parking_id,
                                HouseDeptID = c.HouseDeptID??0,
                                ParkingSpaceId = c.Id,
                                CarportNum = c.CarportNum,
                                Area = c.Area
                            };

                return query.ToList();
            }
        }


        

        public List<CustomTreeNodeModel> GetAsynParkingSpaceTree(string parkingId)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var query = _ParkingSysUnitOfWork.CarportRepository
                    .GetAll()
                    .Where(c => c.Parking_id == parkingId)
                    .Select(c => new { c.Id, c.CarportNum, c.Parking_id, c.HouseDeptID }).ToList();
                return query
                    .Select(q => new CustomTreeNodeModel()
                    {
                        id = q.Id.ToString() + "_" + q.Parking_id.ToString() + "_" + (q.HouseDeptID.HasValue ? q.HouseDeptID.ToString() : "0"),
                        icon = "fa fa-inbox",
                        children = false,
                        text = q.CarportNum
                    }).ToList();
            }
        }
        public List<TreeNodeModel> GetAsynParkingSpaceTree_TreeNode(string parkingId)
        {
            //Guid gid = Guid.Parse(parkingId);
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var query = _ParkingSysUnitOfWork.CarportRepository
                    .GetAll()
                    .Where(c => c.Parking_id == parkingId)
                    .Select(c => new { c.Id, c.CarportNum, c.Parking_id, c.HouseDeptID }).ToList();
                return query
                    .Select(q => new TreeNodeModel()
                    {
                        id = q.Id.ToString(),
                        icon = "fa fa-inbox",
                        children = null,
                        text = q.CarportNum
                    }).ToList();
            }
        }

        public List<AsynTreeNodeModel> GetAsynParkingSpaceTree(string parkingId, int carportState)
        {
            //Guid gid = Guid.Parse(parkingId);
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var query = _ParkingSysUnitOfWork.CarportRepository
                    .GetAll()
                    .Where(c => c.Parking_id == parkingId && c.CarportState == carportState)
                    .Select(c => new { c.Id, c.CarportNum, c.Parking_id, c.HouseDeptID }).ToList();
                return query
                    .Select(q => new AsynTreeNodeModel()
                    {
                        id = q.Id.ToString() + "_" + q.Parking_id.ToString() + "_" + (q.HouseDeptID.HasValue ? q.HouseDeptID.ToString() : "0"),
                        icon = "fa fa-inbox",
                        children = false,
                        text = q.CarportNum
                    }).ToList();
            }
        }
        public Dictionary<int, string> GetParkingSpace(List<int?> ids)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
           
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var query = _ParkingSysUnitOfWork.CarportRepository
                    .GetAll()
                    .Where(c => ids.Contains(c.Id))
                .Select(c => new { c.CarportNum, c.Id }).ToList();
                query.ForEach(o =>
                {
                    dic.Add(Convert.ToInt32(o.Id), o.CarportNum);
                });
                return dic;
            }
        }

        public int GetHoseDeptIdByParkingSpaceId(int parkingSpaceId)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var query = _ParkingSysUnitOfWork.CarportRepository
                    .GetAll()
                    .Where(c => c.Id == parkingSpaceId)
                    .FirstOrDefault();
                if (query != null && query.HouseDeptID.HasValue)
                {
                    return query.HouseDeptID.Value;
                }
                return 0;
            }
        }

        public List<CustomTreeNodeModel> GetParkingSpaceCarportStateAndType(string parkingId, string carportStateAndType)
        {

            string[] arr = carportStateAndType.Split('|');
            //Guid gid = Guid.Parse(parkingId);
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                if (!string.IsNullOrEmpty(arr[0]) && !string.IsNullOrEmpty(arr[1]))
                {

                    int CarportType = Convert.ToInt32(arr[1]);
                    int CarportState = Convert.ToInt32(arr[0]);

                    var query = _ParkingSysUnitOfWork.CarportRepository
                  .GetAll()
                  .Where(c => c.Parking_id == parkingId && c.CarportState == CarportState && c.CarportType == CarportType)
                  .Select(c => new { c.Id, c.CarportNum, c.Parking_id, c.HouseDeptID }).ToList();
                    return query
                        .Select(q => new CustomTreeNodeModel()
                        {
                            id = q.Id.ToString() + "_" + q.Parking_id.ToString() + "_" + (q.HouseDeptID.HasValue ? q.HouseDeptID.ToString() : "0"),
                            icon = "fa fa-inbox",
                            children = false,
                            text = q.CarportNum
                        }).ToList();
                }
                if (!string.IsNullOrEmpty(arr[1]))
                {
                    int CarportType = Convert.ToInt32(arr[1]);
                    var query = _ParkingSysUnitOfWork.CarportRepository
                  .GetAll()
                  .Where(c => c.Parking_id == parkingId && c.CarportType == CarportType)
                  .Select(c => new { c.Id, c.CarportNum, c.Parking_id, c.HouseDeptID }).ToList();
                    return query
                        .Select(q => new CustomTreeNodeModel()
                        {
                            id = q.Id.ToString() + "_" + q.Parking_id.ToString() + "_" + (q.HouseDeptID.HasValue ? q.HouseDeptID.ToString() : "0"),
                            icon = "fa fa-inbox",
                            children = false,
                            text = q.CarportNum
                        }).ToList();
                }
                if (!string.IsNullOrEmpty(arr[0]))
                {
                    int CarportState = Convert.ToInt32(arr[0]);
                    var query = _ParkingSysUnitOfWork.CarportRepository
                  .GetAll()
                  .Where(c => c.Parking_id == parkingId && c.CarportState == CarportState)
                  .Select(c => new { c.Id, c.CarportNum, c.Parking_id, c.HouseDeptID }).ToList();
                    return query
                        .Select(q => new CustomTreeNodeModel()
                        {
                            id = q.Id.ToString() + "_" + q.Parking_id.ToString() + "_" + (q.HouseDeptID.HasValue ? q.HouseDeptID.ToString() : "0"),
                            icon = "fa fa-inbox",
                            children = false,
                            text = q.CarportNum
                        }).ToList();
                }
                if (string.IsNullOrEmpty(arr[0]) && string.IsNullOrEmpty(arr[1]))
                {
                    var query = _ParkingSysUnitOfWork.CarportRepository
                  .GetAll()
                  .Where(c => c.Parking_id == parkingId)
                  .Select(c => new { c.Id, c.CarportNum, c.Parking_id, c.HouseDeptID }).ToList();
                    return query
                        .Select(q => new CustomTreeNodeModel()
                        {
                            id = q.Id.ToString() + "_" + q.Parking_id.ToString() + "_" + (q.HouseDeptID.HasValue ? q.HouseDeptID.ToString() : "0"),
                            icon = "fa fa-inbox",
                            children = false,
                            text = q.CarportNum
                        }).ToList();
                }
                return new List<CustomTreeNodeModel>();

            }
        }

        public List<AsynTreeNodeModel> GetAsynParkingTree(int communityDeptId)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var query = (from p in _ParkingSysUnitOfWork.ParkingRepository.GetAll()
                             join c in _ParkingSysUnitOfWork.CarportRepository.GetAll() on p.Parking_id equals c.Parking_id
                             into temp
                             from t in temp.DefaultIfEmpty()
                             where p.Dept_id == communityDeptId
                             group new { p.Parking_id, p.Parking_name, t.Id } by new { Parking_id = p.Parking_id, Parking_name = p.Parking_name } into g
                             select new
                             {
                                 g.Key.Parking_id,
                                 g.Key.Parking_name,
                                 CarportCount = g.Count(c => c.Id != null)
                             }).ToList();
                return query
                    .Select(q => new AsynTreeNodeModel()
                    {
                        id = q.Parking_id.ToString() + "_" + communityDeptId.ToString(),
                        icon = "fa fa-uppercase-p",
                        children = q.CarportCount > 0,
                        text = q.Parking_name
                    }).ToList();
            }
        }
        public List<AsynTreeNodeModel> GetAsynParkingTree(int communityDeptId, ref Dictionary<string, List<string>> dics)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var query = (from p in _ParkingSysUnitOfWork.ParkingRepository.GetAll()
                             join c in _ParkingSysUnitOfWork.CarportRepository.GetAll() on p.Parking_id equals c.Parking_id
                             into temp
                             from t in temp.DefaultIfEmpty()
                             where p.Dept_id == communityDeptId
                             group new { p.Parking_id, p.Parking_name, t.Id } by new { Parking_id = p.Parking_id, Parking_name = p.Parking_name, } into g
                             select new
                             {
                                 g.Key.Parking_id,
                                 g.Key.Parking_name,
                                 CarportCount = g.Count(c => c.Id != null)
                             }).ToList();

                foreach (var m in query)
                {
                    List<string> listIds = new List<string>();
                    foreach (var n in GetAsynParkingSpaceTree(m.Parking_id.ToString()))
                    {
                        listIds.Add(n.id);
                    }
                    dics.Add(m.Parking_id.ToString(), listIds);
                }
                return query
                    .Select(q => new AsynTreeNodeModel()
                    {
                        id = q.Parking_id.ToString() + "_" + communityDeptId.ToString(),
                        icon = "fa fa-uppercase-p",
                        children = q.CarportCount > 0,
                        text = q.Parking_name
                    }).ToList();
            }
        }

        public List<CustomTreeNodeModel> GetParkingSpaceAndCarPortTree(int communityDeptId, ref Dictionary<string, List<string>> dics, string carportState)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var query = (from p in _ParkingSysUnitOfWork.ParkingRepository.GetAll()
                             join c in _ParkingSysUnitOfWork.CarportRepository.GetAll() on p.Parking_id equals c.Parking_id
                             into temp
                             from t in temp.DefaultIfEmpty()
                             where p.Dept_id == communityDeptId
                             group new { p.Parking_id, p.Parking_name, t.Id } by new { Parking_id = p.Parking_id, Parking_name = p.Parking_name, } into g
                             select new
                             {
                                 g.Key.Parking_id,
                                 g.Key.Parking_name,
                                 CarportCount = g.Count(c => c.Id != null)
                             }).ToList();

                foreach (var m in query)
                {
                    List<string> listIds = new List<string>();
                    foreach (var n in GetAsynParkingSpaceTree(m.Parking_id.ToString()))
                    {
                        listIds.Add(n.id);
                    }
                    dics.Add(m.Parking_id.ToString(), listIds);
                }
                return query
                    .Select(q => new CustomTreeNodeModel()
                    {
                        id = q.Parking_id.ToString() + "_" + communityDeptId.ToString(),
                        icon = "fa fa-uppercase-p",
                        children = GetAsynParkingSpaceTree(q.Parking_id.ToString(), Convert.ToInt32(carportState)),
                        text = q.Parking_name,
                        state = new { opened = true }
                    }).ToList();
            }
        }

        public List<TreeNodeModel> GetParkingSpaceAndCarPortTree(int communityDeptId)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var query = (from p in _ParkingSysUnitOfWork.ParkingRepository.GetAll()
                             join c in _ParkingSysUnitOfWork.CarportRepository.GetAll() on p.Parking_id equals c.Parking_id
                             into temp
                             from t in temp.DefaultIfEmpty()
                             where p.Dept_id == communityDeptId
                             group new { p.Parking_id, p.Parking_name, t.Id } by new { Parking_id = p.Parking_id, Parking_name = p.Parking_name, } into g
                             select new
                             {
                                 g.Key.Parking_id,
                                 g.Key.Parking_name,
                                 CarportCount = g.Count(c => c.Id != null)
                             }).ToList();

                foreach (var m in query)
                {
                    List<string> listIds = new List<string>();
                    foreach (var n in GetAsynParkingSpaceTree(m.Parking_id.ToString()))
                    {
                        listIds.Add(n.id);
                    }
                
                }
                return query
                    .Select(q => new TreeNodeModel()
                    {
                        id = q.Parking_id.ToString(),
                        icon = "fa fa-uppercase-p",
                        state = new { selected = true },
                        children = GetAsynParkingSpaceTree_TreeNode(q.Parking_id.ToString()),
                        text = q.Parking_name
                       
                    }).ToList();
            }

            
        }


        public List<ParkingSpaceInfo> GetParkingSpaceInfo(int communityDeptId, int houseDeptID)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var query = from p in _ParkingSysUnitOfWork.ParkingRepository.GetAll()
                            join c in _ParkingSysUnitOfWork.CarportRepository.GetAll() on p.Parking_id equals c.Parking_id
                            where p.Dept_id == communityDeptId && c.HouseDeptID == houseDeptID
                            select new ParkingSpaceInfo()
                            {
                                ParkingId = p.Parking_id,
                                HouseDeptID = c.HouseDeptID,
                                ParkingSpaceId = c.Id,
                                CarportNum = c.CarportNum,
                                Area = c.Area
                            };
                return query.ToList();
            }
        }


        public ParkingSpaceInfo GetParkingSpaceInfoBySpaceNo(int communityDeptId, string parkingName, string spaceNo)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var query = from p in _ParkingSysUnitOfWork.ParkingRepository.GetAll()
                            join c in _ParkingSysUnitOfWork.CarportRepository.GetAll() on p.Parking_id equals c.Parking_id
                            where p.Dept_id == communityDeptId && p.Parking_name == parkingName
                            && c.CarportNum == spaceNo
                            select new ParkingSpaceInfo()
                            {
                                ParkingId = p.Parking_id,
                                HouseDeptID = c.HouseDeptID,
                                ParkingSpaceId = c.Id,
                                CarportNum = c.CarportNum,
                                Area = c.Area
                            };
                return query.FirstOrDefault();
            }
        }





        
        public List<CustomTreeNodeModel> GetCarParkTree(string UserName)
        {
            int parentId = 3;
             List<CustomTreeNodeModel> treenNodeModels = new List<CustomTreeNodeModel>();
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var IsParentSelected = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Any(r => r.Id == parentId && r.SEC_AdminUsers.Any(t => t.UserName == UserName));
                var CommunityDeptCode = (from adminUser in _BackgroundMgrUnitOfWork.SEC_AdminUserRepository.GetAll()
                                 from dept in adminUser.SEC_Depts
                                 where (adminUser.UserName == UserName)
                                 && dept.DeptType == (int)EDeptType.XiaoQu
                                 select dept.Code).Distinct();
                var queryDept = from dept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                                where dept.PId == parentId
                                && (IsParentSelected || dept.SEC_AdminUsers.Any(u => u.UserName == UserName)
                                || CommunityDeptCode.Any(h => h.Contains("." + SqlFunctions.StringConvert((double)dept.Id).Trim() + ".")))
                                orderby dept.Name
                                select new
                                {
                                    Id = dept.Id,
                                    DeptType = dept.DeptType,
                                    DeptName = dept.Name
                                };

                var CommunityDeptIds= (from adminUser in _BackgroundMgrUnitOfWork.SEC_AdminUserRepository.GetAll()
                                         from dept in adminUser.SEC_Depts
                                         where (adminUser.UserName == UserName)
                                         && dept.DeptType == (int)EDeptType.XiaoQu
                                         select dept.Id).Distinct();

                var ParkingList = GetParkingListByCommunityIds(CommunityDeptIds.ToArray());

                foreach (var tempDeptInfo in queryDept)
                {
                    CustomTreeNodeModel tempTreeNodeModel = new CustomTreeNodeModel();
                    tempTreeNodeModel.id = tempDeptInfo.Id.Value + "_" + tempDeptInfo.DeptType;
                    tempTreeNodeModel.text = tempDeptInfo.DeptName;
                    SEC_DeptDomainService.SetIcon(tempDeptInfo.DeptType.Value, tempTreeNodeModel);
                    bool IsParentPermission = SEC_DeptDomainService.GetParentPermission(_BackgroundMgrUnitOfWork, tempDeptInfo.Id.Value, UserName);
                    //找小区
                    var xqDept = from dept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                                 where dept.PId == tempDeptInfo.Id
                                 && (IsParentPermission || IsParentSelected || dept.SEC_AdminUsers.Any(u => u.UserName == UserName)
                                 || CommunityDeptCode.Any(h => h.Contains("." + SqlFunctions.StringConvert((double)dept.Id).Trim() + ".")))
                                 orderby dept.Name
                                 select new
                                 {
                                     Id = dept.Id,
                                     DeptType = dept.DeptType,
                                     DeptName = dept.Name
                                 };
                    List<CustomTreeNodeModel> xqNodeModels = new List<CustomTreeNodeModel>();
                    foreach (var xqitem in xqDept)
                    {
                        CustomTreeNodeModel xqTreeNodeModel = new CustomTreeNodeModel();
                        xqTreeNodeModel.id = xqitem.Id.Value + "_" + xqitem.DeptType;
                        xqTreeNodeModel.text = xqitem.DeptName;
                        SEC_DeptDomainService.SetIcon(xqitem.DeptType.Value, xqTreeNodeModel);
                        xqTreeNodeModel.children = ParkingList.Any(r => r.DeptId == xqitem.Id);
                        xqNodeModels.Add(xqTreeNodeModel);
                    }
                    tempTreeNodeModel.state = new { opened = true };
                    tempTreeNodeModel.children = xqNodeModels;
                    treenNodeModels.Add(tempTreeNodeModel);
                }
                //默认选中第一个小区
            
                    var wytree = treenNodeModels.FirstOrDefault();
                    if (wytree != null)
                    {
                        var xqtree = (wytree.children as List<CustomTreeNodeModel>).FirstOrDefault();
                        if (xqtree != null)
                        {
                            xqtree.state = new { selected = true };
                        }
                    }

              


                }
            return treenNodeModels;

        }


        /// <summary>
        /// add by donald 关键字查询 同步树
        /// </summary>
        public List<CustomTreeNodeModel> GetCarParkTree(string UserName, string keyWord)
        {
            List<CustomTreeNodeModel> treenNodeModels = new List<CustomTreeNodeModel>();
            int parentId = 3;
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var carlist = GetCarPortByKey(keyWord).ToList();
                var ParkList = GetParkListByCarList(carlist).ToList();


                var IsParentSelected = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Any(r => r.Id == parentId && r.SEC_AdminUsers.Any(t => t.UserName == UserName));
                var CommunityDeptCode = (from adminUser in _BackgroundMgrUnitOfWork.SEC_AdminUserRepository.GetAll()
                                         from dept in adminUser.SEC_Depts
                                         where (adminUser.UserName == UserName)
                                         && dept.DeptType == (int)EDeptType.XiaoQu
                                         select dept.Code).Distinct();
                var queryDept = from dept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                                where dept.PId == parentId
                                && (IsParentSelected || dept.SEC_AdminUsers.Any(u => u.UserName == UserName)
                                || CommunityDeptCode.Any(h => h.Contains("." + SqlFunctions.StringConvert((double)dept.Id).Trim() + ".")))
                                orderby dept.Name
                                select new
                                {
                                    Id = dept.Id,
                                    DeptType = dept.DeptType,
                                    DeptName = dept.Name
                                };

                var CommunityDeptIds = (from adminUser in _BackgroundMgrUnitOfWork.SEC_AdminUserRepository.GetAll()
                                        from dept in adminUser.SEC_Depts
                                        where (adminUser.UserName == UserName)
                                        && dept.DeptType == (int)EDeptType.XiaoQu
                                        select dept.Id).Distinct();
                foreach (var tempDeptInfo in queryDept)
                {
                    CustomTreeNodeModel tempTreeNodeModel = new CustomTreeNodeModel();
                    tempTreeNodeModel.id = tempDeptInfo.Id.Value + "_" + tempDeptInfo.DeptType;
                    tempTreeNodeModel.text = tempDeptInfo.DeptName;
                    SEC_DeptDomainService.SetIcon(tempDeptInfo.DeptType.Value, tempTreeNodeModel);
                    bool IsParentPermission = SEC_DeptDomainService.GetParentPermission(_BackgroundMgrUnitOfWork, tempDeptInfo.Id.Value, UserName);
                    //找小区
                    var xqDept = from dept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                                 where dept.PId == tempDeptInfo.Id
                                 && (IsParentPermission || IsParentSelected || dept.SEC_AdminUsers.Any(u => u.UserName == UserName)
                                 || CommunityDeptCode.Any(h => h.Contains("." + SqlFunctions.StringConvert((double)dept.Id).Trim() + ".")))
                                 orderby dept.Name
                                 select new
                                 {
                                     Id = dept.Id,
                                     DeptType = dept.DeptType,
                                     DeptName = dept.Name
                                 };
                    List<CustomTreeNodeModel> xqNodeModels = new List<CustomTreeNodeModel>();
                    foreach (var xqitem in xqDept)
                    {
                        CustomTreeNodeModel xqTreeNodeModel = new CustomTreeNodeModel();
                        xqTreeNodeModel.id = xqitem.Id.Value + "_" + xqitem.DeptType;
                        xqTreeNodeModel.text = xqitem.DeptName;
                        xqTreeNodeModel.state = new { opened = true };
                        SEC_DeptDomainService.SetIcon(xqitem.DeptType.Value, xqTreeNodeModel);
                        var childlist = GetParkTreeChildList(carlist, ParkList, xqitem.Id.Value);
                        if (childlist.Count > 0)
                        {
                            xqTreeNodeModel.children = childlist;
                            xqNodeModels.Add(xqTreeNodeModel);
                        }
                        else
                            xqTreeNodeModel.children = null;
                    }
                    tempTreeNodeModel.state = new { opened = true };
                    tempTreeNodeModel.children = xqNodeModels;
                    if(xqNodeModels.Count>0)
                    treenNodeModels.Add(tempTreeNodeModel);
                }

            }
            if (treenNodeModels.Count() < 1)
            {
                treenNodeModels.Add(new CustomTreeNodeModel
                {
                    id = "3_" + EDeptType.RootNode.GetHashCode(),
                    text = "没有搜索到数据",
                    icon = "fa fa-frown-o",
                    children = new List<CustomTreeNodeModel>()
                });
            }
            return treenNodeModels;
        }



        public IList<ParkingInfo> GetParkingListByCommunityIds(int?[] CommunityIds)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var query = from p in _ParkingSysUnitOfWork.ParkingRepository.GetAll()
                            where CommunityIds.Contains(p.Dept_id)
                            select new ParkingInfo()
                            {
                                 Id = p.Parking_id,
                                  DeptId =p.Dept_id,
                                   Name =p.Parking_name
                               
                            };
                return query.ToList();
            }
        }
        #region   车辆查询树构造
        public IList<Carport> GetCarPortByKey(string Keyword)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var query = from car in _ParkingSysUnitOfWork.CarportRepository.GetAll()
                            where car.CarportNum.Contains(Keyword)
                            select car;
                return query.ToList();
            }
        }

        public IList<ParkingInfo> GetParkListByCarList(List<Carport> list)
        {
            //取出list中的parkingId集合
            var parkIds = list.Select(o => o.Parking_id).Distinct().ToArray();
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var query = from park in _ParkingSysUnitOfWork.ParkingRepository.GetAll()
                            where parkIds.Contains(park.Parking_id)
                            select new ParkingInfo()
                            {
                                Id = park.Parking_id,
                                DeptId = park.Dept_id,
                                Name = park.Parking_name

                            };

                return query.ToList();
            }
           
        }

        public IList<CustomTreeNodeModel> GetParkTreeChildList(List<Carport>Clist,List<ParkingInfo> Plist,int DeptId)
        {
            List<CustomTreeNodeModel> treenNodeModels = new List<CustomTreeNodeModel>();
            if (Plist.Any(o => o.DeptId == DeptId))
            {//该小区下面有停车场。
                var parklist = Plist.Where(o => o.DeptId == DeptId);

                foreach (var parkobj in parklist)
                {
                    if (Clist.Any(o => o.Parking_id == parkobj.Id))
                    {  //有车辆   
                        CustomTreeNodeModel treeParkNode = new CustomTreeNodeModel();
                        treeParkNode.id = parkobj.Id + "_" + EDeptType.CheKu;
                        treeParkNode.text = parkobj.Name;
                        treeParkNode.state = new { opened = true };
                        SEC_DeptDomainService.SetIcon((int)EDeptType.CheKu, treeParkNode);
                        var carportlist = Clist.Where(o=> o.Parking_id == parkobj.Id);
                        List<CustomTreeNodeModel> childrenlist = new List<CustomTreeNodeModel>();
                        foreach (var carportobj in carportlist)
                        {
                            CustomTreeNodeModel treeCarPortNode = new CustomTreeNodeModel();
                            treeCarPortNode.id = carportobj.Id + "_" + (int)EDeptType.CheWei;
                            treeCarPortNode.text = carportobj.CarportNum;
                            SEC_DeptDomainService.SetIcon((int)EDeptType.CheWei, treeCarPortNode);
                            childrenlist.Add(treeCarPortNode);
                        }
                        treeParkNode.children = childrenlist;
                        treenNodeModels.Add(treeParkNode);
                    }
                }


            }


            return treenNodeModels;

        }


        #endregion
        public IList<CustomTreeNodeModel> GetCarParkByCommunityId(string CommunityId)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                int CommunityIdint = Convert.ToInt32(CommunityId);
                var query = (from p in _ParkingSysUnitOfWork.ParkingRepository.GetAll()
                             join c in _ParkingSysUnitOfWork.CarportRepository.GetAll() on p.Parking_id equals c.Parking_id
                             into temp
                             from t in temp.DefaultIfEmpty()
                             where p.Dept_id == CommunityIdint
                             group new { p.Parking_id, p.Parking_name, t.Id } by new { Parking_id = p.Parking_id, Parking_name = p.Parking_name } into g
                             select new
                             {
                                 g.Key.Parking_id,
                                 g.Key.Parking_name,
                                 CarportCount = g.Count(c => c.Id != null)
                             }).ToList();
                return query
                    .Select(q => new CustomTreeNodeModel()
                    {
                        id = q.Parking_id.ToString() + "_" + ((int)EDeptType.CheKu).ToString(),
                        icon = "fa fa-uppercase-p",
                        children = q.CarportCount > 0,
                        text = q.Parking_name
                    }).ToList();
            }
        }

        public List<CustomTreeNodeModel> GetCarportByParkId(string parkingId)
        {
            //Guid gid = Guid.Parse(parkingId);
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var query = _ParkingSysUnitOfWork.CarportRepository
                    .GetAll()
                    .Where(c => c.Parking_id == parkingId)
                    .Select(c => new { c.Id, c.CarportNum, c.Parking_id }).ToList();
                return query
                    .Select(q => new CustomTreeNodeModel()
                    {
                        id = q.Id.ToString() + "_" + (int)EDeptType.CheWei,
                        icon = "fa fa-inbox",
                        children = null,
                        text = q.CarportNum
                    }).ToList();
            }
        }

        public int GetCommunityDeptIdByCarPortId(int CarPortId)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var query = (from c in _ParkingSysUnitOfWork.CarportRepository.GetAll()
                             join p in _ParkingSysUnitOfWork.ParkingRepository.GetAll() on c.Parking_id equals p.Parking_id
                             where c.Id == CarPortId
                             select p.Dept_id
                             );
                if (query.Count() > 0)
                    return query.First().Value;
                else
                return 0;
            }
        }

        public Carport GetCarPortById(int CarPortId)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
               var obj=  _ParkingSysUnitOfWork.CarportRepository.GetByKey(CarPortId);

                return obj;
            }
        }

        public ParkingSpaceInfo GetParkingSpaceInfoModelById(int CarPortId)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var obj = _ParkingSysUnitOfWork.CarportRepository.GetByKey(CarPortId);

                ParkingSpaceInfo ParkingSpaceInfo = new ParkingSpaceInfo()
                {
                    Area = obj.Area,
                     CarportNum =obj.CarportNum,
                      ParkingSpaceId =obj.Id,
                       ParkingId =obj.Parking_id
                
                       
                };


                return ParkingSpaceInfo;
            }
        }


        



        public List<Carport> GetCarPortListByComDeptId(int ComDeptId)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                var obj = from p in _ParkingSysUnitOfWork.ParkingRepository.GetAll().Where(o => o.Dept_id == ComDeptId)
                          join c in _ParkingSysUnitOfWork.CarportRepository.GetAll() on p.Parking_id equals c.Parking_id
                          select c;
                return obj.ToList();


             }
        }

        public SEC_User_Owner GetUserOwnerMasterByCarPortId(int ResourcesId)
        {
            var CarPort = GetCarPortById(ResourcesId);
            if (CarPort.HouseDeptID == null || CarPort.HouseDeptID == 0)
            {
                return null;
            }
            else
            {
              return  new SEC_User_OwnerDomainService().GetUserOwnerMasterByHouseDeptId(CarPort.HouseDeptID.Value);
     
            }
        }




    }
}
