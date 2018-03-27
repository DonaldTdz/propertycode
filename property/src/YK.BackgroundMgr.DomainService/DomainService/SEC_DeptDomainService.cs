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

namespace YK.BackgroundMgr.DomainService
{
    public partial class SEC_DeptDomainService
    {
        private const string UserIcon = "fa fa-user";
        private const string RootDeptIcon = "fa fa-sitemap";
        private const string WuyeIcon = "fa fa-briefcase";
        private const string XiaoquIcon = "fa fa-comments";
        private const string CheweiIcon = "fa fa-car";
        private const string ChekuIcon = "fa fa-home";
        private const string FangwuIcon = "fa fa-institution";
        private const string GatewayIcon = "fa fa-wifi";
        private const string KaifashangIcon = "fa fa-group";
        private const string OtherIcon = "fa fa-recycle";
        private const string LouyuIcon = "fa fa-building";
        private const string OwnerIcon = "fa fa-user-md";
        private const string CachKey = "DeptInfo";

        public List<CustomTreeNodeModel> GetAsynDeptTree(string userName, int parentId, int?[] types)
        {
            if (parentId == 0)
            {
                parentId = 3;
            }
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                List<CustomTreeNodeModel> treenNodeModels = new List<CustomTreeNodeModel>();
                //var rootDept = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetByKey(parentId);
                //treenNodeModels.Add(new AsynTreeNodeModel() { children = true, icon = RootDeptIcon, id = rootDept.Id.Value + "_" + rootDept.DeptType, text = rootDept.Name});
                // 判断Dept路径上父级节点是否有被选中
                bool IsPathAnySelected = false;
                var isParentSelected = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Any(r => r.Id == parentId && types.Contains(r.DeptType) && r.SEC_AdminUsers.Any(t => t.UserName == userName));
                int? ptype = 0;
                var parentInfo = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetByKey(parentId);
                ptype = parentInfo.DeptType;
                if (!isParentSelected)
                {
                    var parentDeptIds = parentInfo.Code.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Select(r => int.Parse(r));
                    foreach (var parentDeptId in parentDeptIds)
                    {
                        if (_BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Any(r => r.Id == parentDeptId && types.Contains(r.DeptType) && r.SEC_AdminUsers.Any(t => t.UserName == userName)))
                        {
                            IsPathAnySelected = true;
                            break;
                        }
                    }
                }
                else
                {
                    IsPathAnySelected = true;
                }

                if (IsPathAnySelected)
                {
                    var query = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(r => r.PId == parentId && types.Contains(r.DeptType)).Select(dept => new
                    {
                        Id = dept.Id,
                        DeptType = dept.DeptType,
                        DeptName = dept.Name,
                        Code = dept.Name
                    });
                    //如果子集合是楼栋，需按楼栋编号排序
                    if (ptype == (int)EDeptType.XiaoQu)
                    {
                        query = from d in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                                join l in _BackgroundMgrUnitOfWork.SEC_BuildingRepository.GetAll()
                                on d.Id equals l.DeptId
                                where d.PId == parentId && types.Contains(d.DeptType)
                                select new
                                {
                                    Id = d.Id,
                                    DeptType = d.DeptType,
                                    DeptName = d.Name,
                                    Code = l.Building_code
                                };
                    }

                    var queryDept = query.ToList();
                    bool IsEndChildren = (queryDept.Count() > 0 && queryDept.First().DeptType == types[types.Length - 1]) ? true : false;
                    foreach (var tempDeptInfo in queryDept)
                    {
                        CustomTreeNodeModel tempAsynTreeNodeModel = new CustomTreeNodeModel();
                        tempAsynTreeNodeModel.id = tempDeptInfo.Id.Value + "_" + tempDeptInfo.DeptType;
                        tempAsynTreeNodeModel.text = tempDeptInfo.DeptName;
                        tempAsynTreeNodeModel.code = tempDeptInfo.Code;
                        SetIcon(tempDeptInfo.DeptType.Value, tempAsynTreeNodeModel);
                        //是否是最后一级 房屋 是 则直接为FALSE
                        if (IsEndChildren)
                        {
                            tempAsynTreeNodeModel.children = false;
                        }
                        else
                        {
                            tempAsynTreeNodeModel.children = true;//_BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Any(r => r.PId == tempDeptInfo.Id);
                        }
                        treenNodeModels.Add(tempAsynTreeNodeModel);
                    }
                }
                else
                {
                    //权限级别最低为楼栋 修改by donald 2016-9-28 只设置了楼栋权限 查询不出上级菜单问题
                    var loudoDept = (from adminUser in _BackgroundMgrUnitOfWork.SEC_AdminUserRepository.GetAll()
                                     from dept in adminUser.SEC_Depts
                                     where (adminUser.UserName == userName)
                                     && dept.DeptType == (int)EDeptType.LouYu
                                     select dept.Code).Distinct();

                    //判断父级是否授权
                    bool IsParentPermission = GetParentPermission(_BackgroundMgrUnitOfWork, parentId, userName);

                    var query = from dept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                                    where dept.PId == parentId
                                    && (IsParentPermission
                                    || dept.SEC_AdminUsers.Any(u => u.UserName == userName)
                                    || loudoDept.Any(h => h.Contains("." + SqlFunctions.StringConvert((double)dept.Id).Trim() + ".")))
                                    //orderby dept.Name
                                    select new
                                    {
                                        Id = dept.Id,
                                        DeptType = dept.DeptType,
                                        DeptName = dept.Name,
                                        Code = dept.Code
                                    };
                    //如果子集合是楼栋，需按楼栋编号排序
                    if (ptype == (int)EDeptType.XiaoQu)
                    {
                        query = from d in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                                join l in _BackgroundMgrUnitOfWork.SEC_BuildingRepository.GetAll()
                                on d.Id equals l.DeptId
                                where d.PId == parentId
                                   && (IsParentPermission
                                   || d.SEC_AdminUsers.Any(u => u.UserName == userName)
                                   || loudoDept.Any(h => h.Contains("." + SqlFunctions.StringConvert((double)d.Id).Trim() + ".")))
                                select new
                                {
                                    Id = d.Id,
                                    DeptType = d.DeptType,
                                    DeptName = d.Name,
                                    Code = l.Building_code
                                };
                    }

                    var queryDept = query.ToList();
                    //var queryDept = from adminUser in _BackgroundMgrUnitOfWork.SEC_AdminUserRepository.GetAll()
                    //                from dept in adminUser.SEC_Depts
                    //                where (adminUser.UserName == userName) 
                    //                && dept.PId == parentId
                    //                orderby dept.Name
                    //                select new
                    //                {
                    //                    Id = dept.Id,
                    //                    DeptType = dept.DeptType,
                    //                    DeptName = dept.Name
                    //                };

                    foreach (var tempDeptInfo in queryDept)
                    {
                        CustomTreeNodeModel tempAsynTreeNodeModel = new CustomTreeNodeModel();
                        tempAsynTreeNodeModel.id = tempDeptInfo.Id.Value + "_" + tempDeptInfo.DeptType;
                        tempAsynTreeNodeModel.text = tempDeptInfo.DeptName;
                        SetIcon(tempDeptInfo.DeptType.Value, tempAsynTreeNodeModel);
                        tempAsynTreeNodeModel.children = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Any(r => r.PId == tempDeptInfo.Id && types.Contains(r.DeptType));

                        treenNodeModels.Add(tempAsynTreeNodeModel);
                    }
                }
                try
                {

                    treenNodeModels.Sort(Factory.Comparer);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return treenNodeModels;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="parentId"></param>
        /// <param name="types"></param>
        /// <param name="dicChildHaveChildCount"> Dictionary【子节点id，子节点的子节点Id】 子节点包含几个子节点</param>
        /// <returns></returns>
        public List<AsynTreeNodeModel> GetAsynDeptTree(string userName, int parentId, int?[] types, ref Dictionary<string, List<string>> dicChildHaveChildCount)
        {
            if (parentId == 0)
            {
                parentId = 3;
            }
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                List<AsynTreeNodeModel> treenNodeModels = new List<AsynTreeNodeModel>();
                //var rootDept = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetByKey(parentId);
                //treenNodeModels.Add(new AsynTreeNodeModel() { children = true, icon = RootDeptIcon, id = rootDept.Id.Value + "_" + rootDept.DeptType, text = rootDept.Name});
                // 判断Dept路径上父级节点是否有被选中
                bool IsPathAnySelected = false;
                var isParentSelected = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Any(r => r.Id == parentId && types.Contains(r.DeptType) && r.SEC_AdminUsers.Any(t => t.UserName == userName));
                if (!isParentSelected)
                {
                    var parentDeptIds = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetByKey(parentId).Code.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Select(r => int.Parse(r));
                    foreach (var parentDeptId in parentDeptIds)
                    {
                        if (_BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Any(r => r.Id == parentDeptId && types.Contains(r.DeptType) && r.SEC_AdminUsers.Any(t => t.UserName == userName)))
                        {
                            IsPathAnySelected = true;
                            break;
                        }
                    }
                }
                else
                {
                    IsPathAnySelected = true;
                }

                if (IsPathAnySelected)
                {
                    var queryDept = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(r => r.PId == parentId && types.Contains(r.DeptType)).OrderBy(d => d.Name).Select(dept => new
                    {
                        Id = dept.Id,
                        DeptType = dept.DeptType,
                        DeptName = dept.Name
                    });

                    int? type = 0;
                    if (queryDept.Count() > 0)
                    {
                        type = queryDept.First().DeptType;
                    }
                    foreach (var tempDeptInfo in queryDept)
                    {
                        List<string> childIds = new List<string>();
                        //var depChilds = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(r => r.PId == tempDeptInfo.Id && types.Contains(r.DeptType));
                        AsynTreeNodeModel tempAsynTreeNodeModel = new AsynTreeNodeModel();
                        tempAsynTreeNodeModel.id = tempDeptInfo.Id.Value + "_" + tempDeptInfo.DeptType;
                        tempAsynTreeNodeModel.text = tempDeptInfo.DeptName;
                        SetIcon(tempDeptInfo.DeptType.Value, tempAsynTreeNodeModel);
                        //tempAsynTreeNodeModel.children = depChilds.Count() > 0 ? true : false;
                        //treenNodeModels.Add(tempAsynTreeNodeModel);

                        //foreach (var item in depChilds)
                        //{
                        //    childIds.Add(item.Id.ToString());
                        //}
                        //if (!dicChildHaveChildCount.Keys.Contains(tempAsynTreeNodeModel.id))
                        //{
                        //    dicChildHaveChildCount.Add(tempAsynTreeNodeModel.id, childIds);
                        //}

                        //表示最后一级
                        if (types.Length > 0 && type == types[types.Length - 1])
                        {
                            tempAsynTreeNodeModel.children = false;
                        }
                        else
                        {
                            var depChilds = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(r => r.PId == tempDeptInfo.Id && types.Contains(r.DeptType));
                            tempAsynTreeNodeModel.children = depChilds.Count() > 0 ? true : false;
                            foreach (var item in depChilds)
                            {
                                childIds.Add(item.Id.ToString());
                            }
                            if (!dicChildHaveChildCount.Keys.Contains(tempAsynTreeNodeModel.id))
                            {
                                dicChildHaveChildCount.Add(tempAsynTreeNodeModel.id, childIds);
                            }
                        }
                        treenNodeModels.Add(tempAsynTreeNodeModel);
                    }
                }
                else
                {
                    //权限级别最低为楼栋 修改by donald 2016-9-28 只设置了楼栋权限 查询不出上级菜单问题
                    var loudoDept = (from adminUser in _BackgroundMgrUnitOfWork.SEC_AdminUserRepository.GetAll()
                                     from dept in adminUser.SEC_Depts
                                     where (adminUser.UserName == userName)
                                     && dept.DeptType == (int)EDeptType.LouYu
                                     select dept.Code).Distinct();
                    //判断父级是否授权
                    bool IsParentPermission = GetParentPermission(_BackgroundMgrUnitOfWork, parentId, userName);

                    var queryDept = from dept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                                    where dept.PId == parentId
                                    && (IsParentPermission
                                    || dept.SEC_AdminUsers.Any(u => u.UserName == userName)
                                    || loudoDept.Any(h => h.Contains("." + SqlFunctions.StringConvert((double)dept.Id).Trim() + ".")))
                                    orderby dept.Name
                                    select new
                                    {
                                        Id = dept.Id,
                                        DeptType = dept.DeptType,
                                        DeptName = dept.Name
                                    };
                    int? type = 0;
                    if (queryDept.Count() > 0)
                    {
                        type = queryDept.First().DeptType;
                    }
                    foreach (var tempDeptInfo in queryDept)
                    {
                        List<string> childIds = new List<string>();
                        //var depChilds = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(r => r.PId == tempDeptInfo.Id && types.Contains(r.DeptType));
                        AsynTreeNodeModel tempAsynTreeNodeModel = new AsynTreeNodeModel();
                        tempAsynTreeNodeModel.id = tempDeptInfo.Id.Value + "_" + tempDeptInfo.DeptType;
                        tempAsynTreeNodeModel.text = tempDeptInfo.DeptName;
                        SetIcon(tempDeptInfo.DeptType.Value, tempAsynTreeNodeModel);
                        //tempAsynTreeNodeModel.children = depChilds.Count() > 0 ? true : false;
                        //treenNodeModels.Add(tempAsynTreeNodeModel);
                        //foreach (var item in depChilds)
                        //{
                        //    childIds.Add(item.Id.ToString());
                        //}
                        //if (!dicChildHaveChildCount.Keys.Contains(tempAsynTreeNodeModel.id))
                        //{
                        //    dicChildHaveChildCount.Add(tempAsynTreeNodeModel.id, childIds);
                        //}
                        if (types.Length > 0 && type == types[types.Length - 1])
                        {
                            tempAsynTreeNodeModel.children = false;
                        }
                        else
                        {
                            var depChilds = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(r => r.PId == tempDeptInfo.Id && types.Contains(r.DeptType));
                            tempAsynTreeNodeModel.children = depChilds.Count() > 0 ? true : false;
                            foreach (var item in depChilds)
                            {
                                childIds.Add(item.Id.ToString());
                            }
                            if (!dicChildHaveChildCount.Keys.Contains(tempAsynTreeNodeModel.id))
                            {
                                dicChildHaveChildCount.Add(tempAsynTreeNodeModel.id, childIds);
                            }
                        }
                        treenNodeModels.Add(tempAsynTreeNodeModel);
                    }
                }

                return treenNodeModels;
            }
        }


        /// <summary>
        /// 默认取到小区树 展开到小区级别
        /// </summary>
        /// <returns></returns>
        public List<CustomTreeNodeModel> GetDeptTree(string userName, int?[] types)
        {
            int parentId = 3;
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                List<CustomTreeNodeModel> treenNodeModels = new List<CustomTreeNodeModel>();
                //是否逸社区被选中 因为基础组设计 当逸社区节点被选中，下面子节点不会存储到权限表
                var IsParentSelected = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Any(r => r.Id == parentId && types.Contains(r.DeptType) && r.SEC_AdminUsers.Any(t => t.UserName == userName));

                //var loudoDept = (from adminUser in _BackgroundMgrUnitOfWork.SEC_AdminUserRepository.GetAll()
                //                 from dept in adminUser.SEC_Depts
                //                 where (adminUser.UserName == userName)
                //                 && dept.DeptType == (int)EDeptType.LouYu
                //                 select dept.Code).Distinct();
                //修改 对于基础组改权限控制 当选了小区或物业 不会选中子级 2017-9-1
                var userDept = (from adminUser in _BackgroundMgrUnitOfWork.SEC_AdminUserRepository.GetAll()
                                 from dept in adminUser.SEC_Depts
                                 where (adminUser.UserName == userName)
                                 select dept);
                var loudoDept = (from loudo in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                                 where loudo.DeptType == (int)EDeptType.LouYu
                                 //等于楼栋自己 或 自己上级包含
                                 && userDept.Any(u => loudo.Id == u.Id || loudo.Code.Contains("." + SqlFunctions.StringConvert((double)u.Id).Trim() + "."))
                                 select loudo.Code).Distinct();

                var queryDept = from dept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                                where dept.PId == parentId
                                && (IsParentSelected || dept.SEC_AdminUsers.Any(u => u.UserName == userName)
                                || loudoDept.Any(h => h.Contains("." + SqlFunctions.StringConvert((double)dept.Id).Trim() + ".")))
                                orderby dept.Name
                                select new
                                {
                                    Id = dept.Id,
                                    DeptType = dept.DeptType,
                                    DeptName = dept.Name
                                };

                foreach (var tempDeptInfo in queryDept)
                {
                    CustomTreeNodeModel tempTreeNodeModel = new CustomTreeNodeModel();
                    tempTreeNodeModel.id = tempDeptInfo.Id.Value + "_" + tempDeptInfo.DeptType;
                    tempTreeNodeModel.text = tempDeptInfo.DeptName;
                    SetIcon(tempDeptInfo.DeptType.Value, tempTreeNodeModel);
                    bool IsParentPermission = GetParentPermission(_BackgroundMgrUnitOfWork, tempDeptInfo.Id.Value, userName);
                    //找小区
                    var xqDept = from dept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                                 where dept.PId == tempDeptInfo.Id
                                 && (IsParentPermission || IsParentSelected || dept.SEC_AdminUsers.Any(u => u.UserName == userName)
                                 || loudoDept.Any(h => h.Contains("." + SqlFunctions.StringConvert((double)dept.Id).Trim() + ".")))
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
                        SetIcon(xqitem.DeptType.Value, xqTreeNodeModel);
                        if (types.Length > 3)
                        {
                            xqTreeNodeModel.children = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Any(r => r.PId == xqitem.Id && types.Contains(r.DeptType));
                        }
                        else
                        {
                            xqTreeNodeModel.children = false;
                        }

                        xqNodeModels.Add(xqTreeNodeModel);
                    }
                   
                    if(types.Any(o=>o.Value== (int)EDeptType.XiaoQu))
                    {
                       
                        tempTreeNodeModel.children = xqNodeModels;
                    }
                    else
                    {
                        tempTreeNodeModel.state = new { opened = false };
                        tempTreeNodeModel.children =null;
                    }
                       
                    treenNodeModels.Add(tempTreeNodeModel);
                }
                //默认选中第一个小区
                if (types.Length >= 2 && types.Any(o => o.Value==(int)EDeptType.XiaoQu))
                {
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
                else
                {
                    
                    var wytree = treenNodeModels.FirstOrDefault();
                    if(wytree!=null)
                     wytree.state= new { selected = true }; 
                }
                return treenNodeModels;
            }
        }

        /// <summary>
        /// add by donald 关键字查询 同步树
        /// </summary>
        public List<CustomTreeNodeModel> GetDeptTree(string userName, string keyWord)
        {
            List<CustomTreeNodeModel> treenNodeModels = new List<CustomTreeNodeModel>();
            int parentId = 3;
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                //是否逸社区被选中 因为基础组设计 当逸社区节点被选中，下面子节点不会存储到权限表
                var IsParentSelected = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Any(r => r.Id == parentId && r.SEC_AdminUsers.Any(t => t.UserName == userName));

                //通过关键字（房屋、客户名、电话）查出房屋的DeptId
                var masterUserHose = from u in _BackgroundMgrUnitOfWork.SEC_User_OwnerRepository.GetAll()
                                     join ud in _BackgroundMgrUnitOfWork.SEC_User_OwnerSEC_DeptRepository.GetAll() on u.Id equals ud.SEC_User_Owner_Id
                                     where u.IsDelete == 0 //&& u.IsMaster == 1
                                     where ud.IsDelete == 0
                                     select new
                                     {
                                         u.RealName,//真实姓名
                                         u.UserName,
                                         u.BindingPhonerNumber,//绑定电话
                                         ud.SEC_Dept_Id//房屋DeptId
                                     };
                /*var houseDeptList = (from h in _BackgroundMgrUnitOfWork.SEC_HouseRepository.GetAll()
                                     join hd in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll() on h.OrgId equals hd.OrgId
                                     join mu in masterUserHose on hd.Id equals mu.SEC_Dept_Id into temp
                                     from tt in temp.DefaultIfEmpty()
                                     where hd.Name.Contains(keyWord)
                                     || tt.RealName.Contains(keyWord)
                                     || tt.BindingPhonerNumber.Contains(keyWord)
                                     select new
                                     {
                                         hd.Id,
                                         hd.Code,
                                         hd.DeptType,
                                         hd.PId,
                                         hd.Name
                                     }).Distinct()
                                   .ToList();

                string[] hCodes = houseDeptList.Select(h => h.Code).ToArray();
                //找上级 和权限
                var wyList = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                    .Where(d => d.DeptType != (int)EDeptType.RootNode 
                    && hCodes.Any(c => c.Contains("." + SqlFunctions.StringConvert((double)d.Id) + "."))
                    && d.SEC_AdminUsers.Any(t => t.UserName == userName))
                    .ToList();*/
                //只有楼宇以上才设置权限，找出权限设置
                //var wyQuery = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(d => (IsParentSelected || d.SEC_AdminUsers.Any(t => t.UserName == userName)) && d.DeptType == (int)EDeptType.LouYu);
                //找出用户的所有权限
                var userPermissions = _BackgroundMgrUnitOfWork.SEC_AdminUserRepository.GetAll()
                    .Where(a => a.UserName == userName).FirstOrDefault().SEC_Depts;
                //string[] uCodes = userPermissions.Select(h => h.Code).Distinct().ToArray();
                //IEnumerable<string> uPids = new List<string>();
                //foreach (var item in uCodes)
                //{
                //    IEnumerable<string> temp = item.Split('.').Where(s => !string.IsNullOrEmpty(s));
                //    uPids = uPids.Concat(temp);
                //}
                //string[] newuCodes = uPids.Distinct().ToArray();
                string[] newuCodes = userPermissions.Select(h => h.Id.ToString()).ToArray();

                //int?[] pids = wyList.Where(w => w.DeptType == (int)EDeptType.LouYu).Select(w => w.Id).ToArray();
                //查询房屋信息
                var allhouseDeptList = (from h in _BackgroundMgrUnitOfWork.SEC_HouseRepository.GetAll()
                                        join hd in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll() on h.OrgId equals hd.OrgId
                                        join mu in masterUserHose on hd.Id equals mu.SEC_Dept_Id into temp
                                        from tt in temp.DefaultIfEmpty()
                                        where hd.Name.Contains(keyWord)
                                        || tt.UserName.Contains(keyWord)
                                        || tt.RealName.Contains(keyWord)
                                        || tt.BindingPhonerNumber.Contains(keyWord)
                                        //where wyQuery.Any(w => w.Id == hd.PId)
                                        //where (IsParentSelected || userPermissions.Any(w => hd.Code.Contains("." + SqlFunctions.StringConvert((double)w.Id).Trim() + ".")))
                                        //where (IsParentSelected || hd.Code.Contains("." +  + "."))
                                        select new
                                        {
                                            hd.Id,
                                            hd.Code,
                                            hd.DeptType,
                                            hd.PId,
                                            hd.Name
                                        }).Distinct().ToList();
                var houseDeptList = from a in allhouseDeptList
                                    where (IsParentSelected || newuCodes.Any(n => a.Code.Contains("." + n + ".")))
                                    select a;
                //通过房屋找上级节点
                string[] hCodes = houseDeptList.Select(h => h.Code).Distinct().ToArray();
                IEnumerable<int?> hPids = new List<int?>();
                foreach (var item in hCodes)
                {
                    IEnumerable<int?> temp = Array.ConvertAll<string, int?>(item.Split('.').Where(s => !string.IsNullOrEmpty(s)).ToArray(), s => int.Parse(s));
                    hPids = hPids.Concat(temp);
                }
                int?[] newCodes = hPids.Distinct().ToArray();
                var wyList = (from d in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll() //.Where(d => IsParentSelected || d.SEC_AdminUsers.Any(t => t.UserName == userName))
                              where d.DeptType != (int)EDeptType.RootNode
                              && newCodes.Contains(d.Id)
                              //&& hCodes.Any(c => c.Contains("." + SqlFunctions.StringConvert((double)d.Id) + "."))
                              select d)
                              .ToList();
                //循环资源
                if (wyList.Count() > 0)
                {
                    treenNodeModels = wyList
                        .Where(w => w.DeptType == (int)EDeptType.WuYE) //物业
                        .Select(w => new CustomTreeNodeModel()
                        {
                            id = w.Id.Value + "_" + w.DeptType,
                            text = w.Name,
                            icon = SetIcon((int)w.DeptType),
                            state = new { opened = true },
                            children = wyList
                            .Where(x => x.DeptType == (int)EDeptType.XiaoQu && x.PId == w.Id)//小区
                            .Select(x => new CustomTreeNodeModel()
                            {
                                id = x.Id.Value + "_" + x.DeptType,
                                text = x.Name,
                                icon = SetIcon((int)x.DeptType),
                                state = new { opened = true },
                                children = wyList
                                .Where(d => d.DeptType == (int)EDeptType.LouYu && d.PId == x.Id)//楼宇
                                .Select(d => new CustomTreeNodeModel()
                                {
                                    id = d.Id.Value + "_" + d.DeptType,
                                    text = d.Name,
                                    icon = SetIcon((int)d.DeptType),
                                    state = new { opened = true },
                                    children = houseDeptList.Where(h => h.PId == d.Id) //房屋
                                               .Select
                                               (h => new TreeNodeModel()
                                               {
                                                   id = h.Id.Value + "_" + h.DeptType,
                                                   text = h.Name,
                                                   icon = SetIcon((int)h.DeptType),
                                                   children = new List<TreeNodeModel>()
                                               }).ToList()
                                })
                                .ToList()
                            })
                            .ToList()
                        })
                        .ToList();
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

        /// <summary>
        /// 根据小区Id获取房屋列表
        /// </summary>
        /// <param name="ComDeptId"></param>
        /// <returns></returns>
        public List<DeptInfo> GetHouDeptListByComDeptId(string ComDeptId)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {  
                var query = from houseDept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                            where houseDept.DeptType == (int)EDeptType.FangWu && houseDept.Code.Contains("." + ComDeptId + ".")
                            select new DeptInfo
                            {
                                Id = houseDept.Id,
                                Code = houseDept.Code,
                                Name = houseDept.Name,
                                DeptType = houseDept.DeptType
                            };

                return query.ToList();
            }
        }

        /// <summary>
        /// 根据小区Id获取房屋列表包含业主信息
        /// </summary>
        /// <param name="ComDeptId"></param>
        /// <returns></returns>
        public List<DeptInfo> GetHouDeptAndOwnerListByComDeptId(string ComDeptId)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                //业主
                var uquery = from o in _BackgroundMgrUnitOfWork.SEC_User_OwnerRepository.GetAll().Where(o => o.IsDelete == 0)
                             join h in _BackgroundMgrUnitOfWork.SEC_User_OwnerSEC_DeptRepository.GetAll()
                             on o.Id equals h.SEC_User_Owner_Id
                             group new { h.SEC_Dept_Id, o.IsMaster, o.UserName } by new { h.SEC_Dept_Id } into gh
                             orderby gh.FirstOrDefault().IsMaster descending
                             //from g in gh
                             select new
                             {
                                 houseDeptId = gh.Key.SEC_Dept_Id,
                                 UserName = gh.FirstOrDefault().UserName
                             };

                var query = from houseDept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                            join u in uquery
                            on houseDept.Id equals u.houseDeptId into temp from lu in temp.DefaultIfEmpty()
                            where houseDept.DeptType == (int)EDeptType.FangWu && houseDept.Code.Contains("." + ComDeptId + ".")
                            select new DeptInfo
                            {
                                Id = houseDept.Id,
                                Code = houseDept.Code,
                                Name = houseDept.Name,
                                DeptType = houseDept.DeptType,
                                OwnerUserName = lu.UserName == null? "" : lu.UserName
                            };

                return query.ToList();
            }
        }

        /// <summary>
        /// 通过业主姓名进行查找
        /// </summary>
        /// <param name="OwnerName"></param>
        /// <param name="ComDeptId"></param>
        /// <returns></returns>
        public List<DeptInfo> GetHouDeptAndOwnerListByOwnerName(string OwnerName, string ComDeptId)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                //业主
                var uquery = from o in _BackgroundMgrUnitOfWork.SEC_User_OwnerRepository.GetAll().Where(o => o.IsDelete == 0)
                             join h in _BackgroundMgrUnitOfWork.SEC_User_OwnerSEC_DeptRepository.GetAll()
                             on o.Id equals h.SEC_User_Owner_Id
                             where o.UserName.Contains(OwnerName)
                             group new { h.SEC_Dept_Id, o.IsMaster, o.UserName } by new { h.SEC_Dept_Id } into gh
                             orderby gh.FirstOrDefault().IsMaster descending
                             //from g in gh
                             select new
                             {
                                 houseDeptId = gh.Key.SEC_Dept_Id,
                                 UserName = gh.FirstOrDefault().UserName
                             };

                var query = from houseDept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                            join u in uquery on houseDept.Id equals u.houseDeptId 
                            where houseDept.DeptType == (int)EDeptType.FangWu && houseDept.Code.Contains("." + ComDeptId + ".")
                            select new DeptInfo
                            {
                                Id = houseDept.Id,
                                Code = houseDept.Code,
                                Name = houseDept.Name,
                                DeptType = houseDept.DeptType,
                                OwnerUserName = u.UserName == null ? "" : u.UserName
                            };

                return query.ToList();
            }
        }

        /// <summary>
        /// 通过业主姓名进行查找
        /// </summary>
        /// <param name="OwnerName"></param>
        /// <param name="ComDeptId"></param>
        /// <returns></returns>
        public List<DeptInfo> GetHouDeptAndOwnerListByIdArr(List<int?>  IdList)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                //业主
                var uquery = from o in _BackgroundMgrUnitOfWork.SEC_User_OwnerRepository.GetAll().Where(o => o.IsDelete == 0)
                             join h in _BackgroundMgrUnitOfWork.SEC_User_OwnerSEC_DeptRepository.GetAll()
                             on o.Id equals h.SEC_User_Owner_Id
                             group new { h.SEC_Dept_Id, o.IsMaster, o.UserName } by new { h.SEC_Dept_Id } into gh
                             orderby gh.FirstOrDefault().IsMaster descending
                             //from g in gh
                             select new
                             {
                                 houseDeptId = gh.Key.SEC_Dept_Id,
                                 UserName = gh.FirstOrDefault().UserName
                             };

                var query = from houseDept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                            join u in uquery on houseDept.Id equals u.houseDeptId
                            where houseDept.DeptType == (int)EDeptType.FangWu && IdList.Contains(houseDept.Id.Value) 
                            select new DeptInfo
                            {
                                Id = houseDept.Id,
                                Code = houseDept.Code,
                                Name = houseDept.Name,
                                DeptType = houseDept.DeptType,
                                OwnerUserName = u.UserName == null ? "" : u.UserName
                            };

                return query.ToList();
            }
        }




        /// <summary>
        /// 根据小区Id获取楼宇列表
        /// </summary>
        /// <param name="ComDeptId"></param>
        /// <returns></returns>
        public List<DeptInfo> GetBuildsByComDeptId(int ComDeptId)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                string villageId = ComDeptId.ToString();
                var query = from buildDept in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                            where buildDept.DeptType == (int)EDeptType.LouYu && buildDept.Code.Contains(villageId)

                            select new DeptInfo
                            {
                                Id = buildDept.Id,
                                Code = buildDept.Code,
                                Name = buildDept.Name,
                                DeptType = buildDept.DeptType
                            };
                var DeptInfoList = query.ToList();
                DeptInfoList.Sort(DeptFactory.Comparer);

                return DeptInfoList;
            }
        }





        /// <summary>
        /// 根据楼宇Id获取房屋列表
        /// </summary>
        /// <param name="BuildDeptIds"></param>
        /// <returns></returns>
        public List<DeptInfo> GetHouDeptListByBuildDeptId(List<int?> BuildDeptIds)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {

                //var query = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().AsEnumerable().Where(o => o.DeptType == (int)EDeptType.FangWu && BuildDeptIds.Contains(o.PId)).Select(m => new
                var query = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(o => o.DeptType == (int)EDeptType.FangWu && BuildDeptIds.Contains(o.PId)).Select(m => new
               DeptInfo
                {
                    Id = m.Id,
                    Code = m.Code,
                    Name = m.Name,
                    DeptType = m.DeptType,
                    PId=m.PId
                });
                return query.ToList();
            }
        }

        /// <summary>
        /// 根据楼宇Id获取独栋信息
        /// </summary>
        /// <param name="BuildDeptId"></param>
        /// <returns></returns>
        public BuildingSingle GetHouDeptListBySingleBuildDeptId(int? BuildDeptId)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var query =from d in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(c => c.Id == BuildDeptId)
                           join b in  _BackgroundMgrUnitOfWork.SEC_BuildingRepository.GetAll().Where(o => o.DeptId == BuildDeptId) 
                           on d.Id equals b.DeptId
                           select new BuildingSingle()
                           {
                            BuildinDeptId = d.Id,
                            BuildingCode = b.Building_code,
                            ComDeptId = d.PId
                           };
                return query.FirstOrDefault();
            }
        }

        /// <summary>
        /// 通过电话号码获取业主信息
        /// </summary>
        /// <param name="BindingPhonerNumber"></param>
        /// <returns></returns>
        public IList<OwnerInformation> GetOwnerByBindingPhonerNumber(string BindingPhonerNumber)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var query = from a in _BackgroundMgrUnitOfWork.SEC_User_OwnerRepository.GetAll().Where(s => s.BindingPhonerNumber.Contains(BindingPhonerNumber))
                            select new OwnerInformation()
                            {
                                DoorNo = a.DoorNo,
                                UserName = a.UserName,
                                BindingPhonerNumber = a.BindingPhonerNumber
                            };
                return query.ToList();
            }
        }

        /// <summary>
        /// 通过电话号码获取业主信息
        /// </summary>
        /// <param name="BindingPhonerNumber"></param>
        /// <returns></returns>
        public OwnerInformation GetUserOwnerByPhoneNum(int communityDeptId, string phoneNum)
        {
            using (var bmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var query = from u in bmUnitWork.SEC_User_OwnerRepository.GetAll().Where(s => s.BindingPhonerNumber == phoneNum && s.IsDelete == 0)
                            join h in bmUnitWork.SEC_User_OwnerSEC_DeptRepository.GetAll()
                            on u.Id equals h.SEC_User_Owner_Id
                            join d in bmUnitWork.SEC_DeptRepository.GetAll().Where(d => d.DeptType == 20)
                            on h.SEC_Dept_Id equals d.Id
                            select new
                            {
                                Uid = u.Id,
                                u.UserName,
                                u.BindingPhonerNumber,
                                d.Id,
                                d.Name,
                                d.Code
                            };
                var allHouseList = query.ToList();
                var strComDeptId = "." + communityDeptId.ToString() + ".";
                //过滤小区
                var comHouseList = allHouseList.Where(a => ("." + a.Code).Contains(strComDeptId)).ToList();              
                if (comHouseList.Count() > 0)
                {
                    OwnerInformation owner = new OwnerInformation();
                    owner.UserName = comHouseList.First().UserName;
                    owner.UserId = comHouseList.First().Uid.ToString();
                    owner.BindingPhonerNumber = comHouseList.First().BindingPhonerNumber;
                    owner.HouseList = comHouseList.Select(c => new HouseInfo() { Id = c.Id, HouseDeptID = c.Id, DoorNo = c.Name }).ToList();
                    return owner;
                }
                return null;
            }
        }

        /// <summary>
        /// 通过RoomId和电话号码获取业主信息
        /// </summary>
        /// <param name="RoomId"></param>
        /// <returns></returns>
        public IList<OwnerInformation> GetOwnerByValidatePhonerNumber(int? RoomId, string BindingPhonerNumber)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var query = from a in _BackgroundMgrUnitOfWork.SEC_User_OwnerRepository.GetAll().Where(o => o.BindingPhonerNumber.Contains(BindingPhonerNumber))
                            join s in _BackgroundMgrUnitOfWork.SEC_User_OwnerSEC_DeptRepository.GetAll().Where(o => o.SEC_Dept_Id==RoomId)
                            on a.Id equals s.SEC_User_Owner_Id
                            select new OwnerInformation()
                            {
                                DoorNo = a.DoorNo,
                                UserName = a.UserName,
                                BindingPhonerNumber = a.BindingPhonerNumber
                            };
                return query.ToList();
            }
        }
        /// <summary>
        /// 根据DEPTID获取信息
        /// </summary>
        /// <param name="deptIds"></param>
        /// <returns></returns>
        public List<DeptInfo> GetDeptsByIds(List<int?> deptIds)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {

                var query = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(o => deptIds.Contains(o.Id)).Select(m => new
                DeptInfo
                {
                    Id = m.Id,
                    Code = m.Code,
                    Name = m.Name,
                    DeptType = m.DeptType
                });
                var queryList = query.ToList();
                queryList.Sort(DeptFactory.Comparer);
                return queryList;
            }
        }

        /// <summary>
        /// 根据ComDeptId获得楼栋，房屋信息
        /// </summary>
        /// <param name="ComDeptId"></param>
        /// <returns></returns>
        public List<DeptInfo> GetBuildingByComDeptId(int? ComDeptId)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {

                var query = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(o =>o.PId==ComDeptId).Select(m => new
                DeptInfo
                {
                    Id = m.Id,
                    Code = m.Code,
                    Name = m.Name,
                    DeptType = m.DeptType
                });
                var dataList = query.ToList();
                //排序
                dataList.Sort(DeptFactory.Comparer);
                return dataList;
            }
        }
        /// <summary>
        /// 根据楼栋ID，按条件查找房屋
        /// </summary>
        /// <param name="ComDeptId"></param>
        /// <returns></returns>
        public List<HouseState> GetHouseByComDeptId(int? BuildId, int? DecorationState_PM, int? HouseState_PM)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {

                var query = from d in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(o => o.PId == BuildId)
                            join h in _BackgroundMgrUnitOfWork.SEC_HouseRepository.GetAll()
                            on d.OrgId equals h.OrgId
                            select (new HouseState
                            {
                                HouseId = d.Id,
                                DecorationState_PM = h.DecorationState_PM,
                                HouseState_PM = h.HouseState_PM
                            });
                if (HouseState_PM != 0 && DecorationState_PM == 0){
                    return query.Where(o => o.HouseState_PM == HouseState_PM).ToList();
                }
                if (HouseState_PM == 0 && DecorationState_PM != 0){
                    return query.Where(o => o.DecorationState_PM == DecorationState_PM).ToList();
                }
                if (HouseState_PM != 0 && DecorationState_PM != 0)
                {
                    return query.Where(o => o.HouseState_PM == HouseState_PM && o.DecorationState_PM == DecorationState_PM).ToList();
                }
                else {
                    return query.ToList();
                }
               
            }
        }

        public string GetDeptNosByHouseDeptIds(int?[] houseDeptIds)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                string[] doorNos = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(d => houseDeptIds.Contains(d.Id)).Select(d => d.Name).ToArray();
                if (doorNos == null || doorNos.Count() == 0)
                {
                    return string.Empty;
                }
                return String.Join(",", doorNos);
            }
        }

        public List<DeptInfo> GetOwnerHouDeptListByComDeptId(string ComDeptId)
        {

            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var query = from D in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                            join OD in _BackgroundMgrUnitOfWork.SEC_User_OwnerSEC_DeptRepository.GetAll() on D.Id equals OD.SEC_Dept_Id
                            join O in _BackgroundMgrUnitOfWork.SEC_User_OwnerRepository.GetAll() on OD.SEC_User_Owner_Id equals O.Id
                            where D.DeptType == (int)EDeptType.FangWu && D.Code.Contains("." + ComDeptId + ".")
                            where O.IsMaster == 1
                            select new DeptInfo
                            {
                                Id = D.Id,
                                Code = D.Code,
                                Name = D.Name,
                                DeptType = D.DeptType

                            };

                return query.ToList();
            }



        }


        public List<DeptInfo> GetComDeptByUserName(string UserName)
        {
            int parentId = 3;
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var IsParentSelected = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Any(r => r.Id == parentId && r.SEC_AdminUsers.Any(t => t.UserName == UserName));
                //找出用户的所有权限
                var userPermissions = _BackgroundMgrUnitOfWork.SEC_AdminUserRepository.GetAll().Where(a => a.UserName == UserName).FirstOrDefault().SEC_Depts;
                int?[] xqCodes = userPermissions.Where(h => h.DeptType == (int)EDeptType.XiaoQu).Select(h => h.Id).Distinct().ToArray();
                int?[] wyCodes = userPermissions.Where(h => h.DeptType == (int)EDeptType.WuYE).Select(h => h.Id).Distinct().ToArray();
                int?[] lyCodes = userPermissions.Where(h => h.DeptType == (int)EDeptType.LouYu).Select(h => h.PId).Distinct().ToArray();
                //IEnumerable<string> uPids = new List<string>();
                //foreach (var item in uCodes)
                //{
                //    IEnumerable<string> temp = item.Split('.').Where(s => !string.IsNullOrEmpty(s));
                //    uPids = uPids.Concat(temp);
                //}
                //string[] newuCodes = uPids.Distinct().ToArray();

                var wyList = from cd in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(d => d.DeptType == (int)EDeptType.XiaoQu)
                                 //where (IsParentSelected || userPermissions.Any(w => w.SEC_Depts.Any(d => cd.Code.Contains("." + SqlFunctions.StringConvert((double)d.Id).Trim() + "."))))
                             select new DeptInfo
                             {
                                 Id = cd.Id,
                                 PId = cd.PId,
                                 Code = cd.Code,
                                 Name = cd.Name,
                                 DeptType = cd.DeptType
                             };

                return wyList.Where(w => IsParentSelected //逸社区
                || xqCodes.Contains(w.Id) //小区
                || wyCodes.Contains(w.PId) //物业
                || lyCodes.Contains(w.Id) //楼宇 子
                ).OrderBy(w => w.Name).ToList();
            }
        }

        public static bool GetParentPermission(IBackgroundMgrUnitOfWork _BackgroundMgrUnitOfWork, int parentId, string userName)
        {
            //判断父级是否授权
            string code = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(d => d.Id == parentId).Select(d => d.Code).FirstOrDefault();
            code += parentId.ToString();
            int?[] pids = Array.ConvertAll<string, int?>(code.Split('.').Where(s => !string.IsNullOrEmpty(s)).ToArray(), s => int.Parse(s));
            bool IsParentPermission = (from adminUser in _BackgroundMgrUnitOfWork.SEC_AdminUserRepository.GetAll()
                                       from dept in adminUser.SEC_Depts
                                       where (adminUser.UserName == userName)
                                       && pids.Contains(dept.Id)
                                       select dept.Id).Count() > 0;
            return IsParentPermission;
        }

 
        /// <summary>
        /// 通过小区Id获取 Build House
        /// </summary>
        /// <param name="ComDeptId"></param>
        /// <returns></returns>
        public  IList<DeptInfo> GetBuildAndHouseDeptInfoByComId(int ComDeptId)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                string ComDeptIdstr = ComDeptId.ToString();
                var query = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(o => (o.DeptType == (int)EDeptType.FangWu||o.DeptType== (int)EDeptType.LouYu)&&o.Code.Contains(ComDeptIdstr)).Select(m => new
               DeptInfo
                {
                    Id = m.Id,
                    Code = m.Code,
                    Name = m.Name,
                    DeptType = m.DeptType,
                     PId = m.PId
                    
                });
                var returnlist = query.ToList();
             
                return returnlist;
            }
           
        }

        public IList<DeptInfo> GetHouseDeptInfobyLouyuDeptId(int LouyuId)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
            
                var query = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(o=>o.DeptType== (int)EDeptType.FangWu&&o.PId== LouyuId).Select(m => new
                    DeptInfo
                {
                    Id = m.Id,
                    Code = m.Code,
                    Name = m.Name,
                    DeptType = m.DeptType,
                    PId = m.PId

                });
                var returnlist = query.ToList();
                //排序 2017-8-3
                returnlist.Sort(DeptFactory.Comparer);
                return returnlist;
            }
        }

        public SEC_Dept GetDeptInfoById(string ID)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                int deptId = int.Parse(ID);
                return _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(o => o.Id == deptId).FirstOrDefault();
            }
        }

        public int?[] GetComDeptIdsByComDeptId(int ComDeptId)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                //查出物业公司deptID
                int? pid = 0;
                var dept = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(d => d.Id == ComDeptId).FirstOrDefault();
                if (dept != null)
                {
                    pid = dept.PId;
                }
                //查出物业公司下面的所有小区deptID
                return _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                    .Where(d => d.PId == pid && d.DeptType == (int)EDeptType.XiaoQu)
                    .Select(d => d.Id).ToArray();
            }
        }


        public List<DeptInfo> GetDeptInfoListByPropertyId(int? ProDeptId)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var ComDeptList = (_BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                       .Where(d => d.PId == ProDeptId && d.DeptType == (int)EDeptType.XiaoQu)
                       .Select(d => new DeptInfo
                       {
                           Id = d.Id,
                           Name = d.Name
                       })).ToList();

                return ComDeptList;

            }
        }




        /// <summary>
        /// 根据查询条件获取对象集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<DeptInfo> GetDeptInfoByQuery(Expression<Func<SEC_Dept, bool>> predicate)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var query = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(predicate).Select(o=>new DeptInfo {  Id=o.Id, Code =o.Code, DeptType=o.DeptType,Name=o.Name, PId=o.PId});
                return query.ToList();

            }
        }

        public DeptInfo GetComDeptInfoByName(string comName)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {

                var query = _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                    .Where(d => d.DeptType == (int)EDeptType.XiaoQu
                    && d.Name == comName).FirstOrDefault();
                if (query == null)
                {
                    return null;
                }
                DeptInfo info = new DeptInfo();
                info.Id = query.Id;
                info.Name = query.Name;
                return info;
            }
        }

        public int?[] GetHouseDeptIdsByHouseNum(int comDeptId, string houseNum)
        {
            string comdeptidstr = "." + comDeptId.ToString() + ".";
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                return _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll()
                .Where(d => d.Name.Contains(houseNum) 
                && d.DeptType == (int)EDeptType.FangWu
                && d.Code.Contains(comdeptidstr)).Select(s => s.Id).ToArray();
            }
        }

        public string[] GetUserPhonesByHouseDeptIds(int?[] houseDeptIdArr)
        {
            using (var bmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var query = from h in bmUnitWork.SEC_User_OwnerSEC_DeptRepository.GetAll()
                            join u in bmUnitWork.SEC_User_OwnerRepository.GetAll()
                            on h.SEC_User_Owner_Id equals u.Id
                            where houseDeptIdArr.Contains(h.SEC_Dept_Id)
                            && u.BindingPhonerNumber != ""
                            && u.IsDelete == 0
                            select u.BindingPhonerNumber;
                return query.Distinct().ToArray();
            }
        }

        /// <summary>
        /// 返回小区房屋树形对象（排除传入的id集合）
        /// </summary>
        /// <param name="ExcludeIds"></param>
        /// <param name="ComDeptId"></param>
        /// <returns></returns>
        public CustomTreeNodeModel GetDeptTreeExcludeIdsbyComDeptId(List<int?> ExcludeIds,int ComDeptId)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                string ComDeptIdStr = ComDeptId.ToString();
                var Deptquery = from d in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(o => o.Code.Contains(ComDeptIdStr) || o.Id == ComDeptId)
                                select d;
               var a= Deptquery.ToList();

                var ExcludeIdsDeptQuery = from d in _BackgroundMgrUnitOfWork.SEC_DeptRepository.GetAll().Where(o => (ExcludeIds.Contains(o.Id.Value)))
                                          select d;


                var b = ExcludeIdsDeptQuery.ToList();
                var ResultDeptQuery = (from d in Deptquery
                                       where !ExcludeIdsDeptQuery.Any(o=>o.Id==d.Id)
                                       select new DeptInfo
                                       {
                                           Id = d.Id,
                                           Code = d.Code,
                                           Name = d.Name,
                                           DeptType = d.DeptType,
                                           PId = d.PId
                                       }).ToList();

                var CommunityDept = ResultDeptQuery.Where(o => o.DeptType == (int)EDeptType.XiaoQu).ToList().FirstOrDefault();
                var BuildingDeptList = ResultDeptQuery.Where(o => o.DeptType == (int)EDeptType.LouYu).ToList();
                CustomTreeNodeModel CommunityTreeNodeModel = new CustomTreeNodeModel()
                {
                    id = CommunityDept.Id.ToString(),
                    icon = SetIcon(CommunityDept.DeptType.Value),
                    text = CommunityDept.Name,
                    state = new { selected = true }

                };
                List<CustomTreeNodeModel> CommunityChildlist = new List<CustomTreeNodeModel>();

                foreach (var buildingDept in BuildingDeptList)
                {
                    //查找楼宇下的房屋

                    var HouseDeptList=  ResultDeptQuery.Where(o => o.PId == buildingDept.Id).ToList();
                    if (HouseDeptList.Count > 0)
                    {
                     var HouseTreeNodelist= HouseDeptList.Select(o => new CustomTreeNodeModel
                        {
                            id = o.Id.ToString(),
                            code = o.Code,
                            text = o.Name,
                            icon = SetIcon(o.DeptType.Value)
                        });

                        var BuildingTreeNode = new CustomTreeNodeModel()
                        {
                            id = buildingDept.Id.ToString(),
                            code = buildingDept.Code,
                            text = buildingDept.Name,
                            icon = SetIcon(buildingDept.DeptType.Value),
                            children = HouseTreeNodelist.ToList()
                        };
                        CommunityChildlist.Add(BuildingTreeNode);
                    }
                }
                CommunityTreeNodeModel.children = CommunityChildlist;
                return CommunityTreeNodeModel;
            }
        }


        public static string SetIcon(int deptType)
        {
            switch (deptType)
            {
                case (int)EDeptType.UserOwner:
                    return OwnerIcon;
                case (int)EDeptType.RootNode:
                    return RootDeptIcon;
                case (int)EDeptType.WuYE:
                    return WuyeIcon;
                case (int)EDeptType.XiaoQu:
                    return XiaoquIcon;
                case (int)EDeptType.LouYu:
                    return LouyuIcon;
                case (int)EDeptType.CheKu:
                    return ChekuIcon;
                case (int)EDeptType.CheWei:
                    return CheweiIcon;
                case (int)EDeptType.FangWu:
                    return FangwuIcon;
                case (int)EDeptType.GateWay:
                    return GatewayIcon;
                case (int)EDeptType.KaiFaShang:
                    return KaifashangIcon;
                case (int)EDeptType.Others:
                    return OtherIcon;
                default:
                    return OtherIcon;
            }
        }



        public static void SetIcon(int deptType, BaseTreeNodeModel treeNodeModel)
        {
            switch (deptType)
            {
                case (int)EDeptType.UserOwner:
                    treeNodeModel.icon = OwnerIcon;
                    break;
                case (int)EDeptType.RootNode:
                    treeNodeModel.icon = RootDeptIcon;
                    break;
                case (int)EDeptType.WuYE:
                    treeNodeModel.icon = WuyeIcon;
                    break;
                case (int)EDeptType.XiaoQu:
                    treeNodeModel.icon = XiaoquIcon;
                    break;
                case (int)EDeptType.LouYu:
                    treeNodeModel.icon = LouyuIcon;
                    break;
                case (int)EDeptType.CheKu:
                    treeNodeModel.icon = ChekuIcon;
                    break;
                case (int)EDeptType.CheWei:
                    treeNodeModel.icon = CheweiIcon;
                    break;
                case (int)EDeptType.FangWu:
                    treeNodeModel.icon = FangwuIcon;
                    break;
                case (int)EDeptType.GateWay:
                    treeNodeModel.icon = GatewayIcon;
                    break;
                case (int)EDeptType.KaiFaShang:
                    treeNodeModel.icon = KaifashangIcon;
                    break;
                case (int)EDeptType.Others:
                    treeNodeModel.icon = OtherIcon;
                    break;
                default:
                    treeNodeModel.icon = OtherIcon;
                    break;
            }
        }

        class Factory : IComparer<BaseTreeNodeModel>
        {
            private Factory() { }
            public static IComparer<BaseTreeNodeModel> Comparer
            {
                get { return new Factory(); }
            }
            public int Compare(BaseTreeNodeModel x, BaseTreeNodeModel y)
            {
                try
                {
                    if (string.IsNullOrEmpty(x.code))
                    {
                        x.code = x.text;
                    }
                    return x.code.Length == y.code.Length ? x.code.CompareTo(y.code) : x.code.Length - y.code.Length;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        class DeptFactory : IComparer<DeptInfo>
        {
            private DeptFactory() { }
            public static IComparer<DeptInfo> Comparer
            {
                get { return new DeptFactory(); }
            }
            public int Compare(DeptInfo x, DeptInfo y)
            {
                try
                {
                    
                    return x.Name.Length == y.Name.Length ? x.Name.CompareTo(y.Name) : x.Name.Length - y.Name.Length;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


    }
}
