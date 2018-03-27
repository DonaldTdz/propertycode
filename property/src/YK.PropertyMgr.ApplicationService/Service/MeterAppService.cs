using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.ApplicationService.Service;
using YK.PropertyMgr.DomainEntity;

namespace YK.PropertyMgr.ApplicationService
{
    public partial class MeterAppService
    {
        #region 获取对象分页集合
        public IList<MeterDTO> Paging(int PageIndex, int PageSize, MeterDTO meterDTO, string expressions, out int totalCount)
        {
            var dataList = MeterService.Paging(PageIndex, PageSize, c => c.Id > 0, expressions, out totalCount);
            return MeterMappers.ChangeMeterToDTOs(dataList).ToList();
        }
        #endregion


        /// <summary>
        /// 获取三表信息
        /// </summary>
        /// <param name="houseDeptId"></param>
        /// <returns></returns>
        public List<MeterDTO> GetMeterDTO(int houseDeptId, int meterType)
        {
            List<Meter> list = MeterService.GetMeterList(o => o.HouseDeptID == houseDeptId && o.MeterType == meterType);
            return MeterMappers.ChangeMeterToDTOs(list).ToList();
        }

        /// <summary>
        /// 获取三表信息
        /// </summary>
        /// <param name="houseDeptIds">房屋IDS</param>
        /// <param name="meterType">0:返回全部类型三表</param>
        /// <returns></returns>
        public List<MeterDTO> GetMeterDTOS(List<int?> houseDeptIds, int meterType)
        {
            List<MeterDTO> list;
            List<Meter> meterList;
            if (houseDeptIds.Count > 0)
            {
                if (meterType == 0)
                {
                    meterList = MeterService.GetMeterList(o => houseDeptIds.Contains(o.HouseDeptID));
                }
                else
                {
                    meterList = MeterService.GetMeterList(o => houseDeptIds.Contains(o.HouseDeptID) && o.MeterType == meterType);
                }
                return MeterMappers.ChangeMeterToDTOs(meterList).ToList();
            }
            else
            {
                list = new List<MeterDTO>();
            }
            return list;
        }

        public List<MeterDTO> GetMeterDTOS(int? comDeptId, int meterType)
        {
            if (comDeptId.HasValue)
            {
                List<Meter> meterList;
                if (meterType == 0)
                {
                    meterList = MeterService.GetMeterList(o => o.ComDeptId == comDeptId);
                }
                else
                {
                    meterList = MeterService.GetMeterList(o => o.ComDeptId == comDeptId && o.IsPublicArea == false && o.MeterType == meterType);
                }
                return MeterMappers.ChangeMeterToDTOs(meterList).ToList();
            }
            else
            {
                return new List<MeterDTO>();
            }
        }

        /// <summary>
        /// 获取公区表
        /// </summary>
        /// <param name="comDeptId"></param>
        /// <param name="meterType"></param>
        /// <returns></returns>
        public List<MeterDTO> GetPublicMeterDTOS(int? comDeptId, MeterTypeEnum meterType)
        {
            if (comDeptId.HasValue)
            {
                var meterList = MeterService.GetMeterList(o => o.ComDeptId == comDeptId 
                && o.IsPublicArea == true && o.MeterType == (int)meterType);
                return MeterMappers.ChangeMeterToDTOs(meterList).ToList();
            }
            else
            {
                return new List<MeterDTO>();
            }
        }
       


        /// <summary>
        /// 通过ID获取公区表
        /// </summary>
        /// <param name="comDeptId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<MeterDTO> GetPublicMeterDTOSById(int? comDeptId, int? Id)
        {
            if (comDeptId.HasValue)
            {
                var meterList = MeterService.GetMeterList(o => o.ComDeptId == comDeptId
                && o.IsPublicArea == true && o.Id==Id);
                return MeterMappers.ChangeMeterToDTOs(meterList).ToList();
            }
            else
            {
                return new List<MeterDTO>();
            }
        }
        public List<MeterDTO> GetMeterDTOS(List<int?> ids)
        {
            return MeterMappers.ChangeMeterToDTOs(MeterService.GetMeterList(o => ids.Contains(o.Id)));
        }
        /// <summary>
        /// 通过Id集合获取三表
        /// </summary>
        /// <param name="MeterId"></param>
        /// <returns></returns>
        public List<MeterDTO> GetMeterDTOS(int?[] MeterId)
        {
            List<MeterDTO> list;
            List<Meter> meterList;
            if (MeterId.Count() > 0)
            {
               
                 meterList = MeterService.GetMeterList(o => MeterId.Contains(o.Id));
                
                return MeterMappers.ChangeMeterToDTOs(meterList).ToList();
            }
            else
            {
                list = new List<MeterDTO>();
            }
            return list;
        }

        #region 加载三表（单一加载水、电、气,设置时间  ）
        /// <summary>
        /// 加载三表（单一加载水、电、气）
        /// </summary>
        /// <param name="id">资源ID</param>
        /// <param name="typeEle">类型为房屋</param>
        /// <param name="strFilter">过滤</param>
        /// <param name="threeType">三表类型（水、电、气）</param>
        /// <returns></returns>
        public List<CustomTreeNodeModel> GetListByResType(SEC_AdminUserDTO currentAdminUser, int id, string strFilter, int threeType, int? comDeptId)
        {
            DeptAppService deptService = new DeptAppService();
            //如果等于小区Id，表示返回楼栋信息
            if (id == comDeptId)
            {
                var dataList = deptService.GetDeptTree(currentAdminUser, id, strFilter);
                var mList = GetPublicMeterDTOS(comDeptId, (MeterTypeEnum)threeType);
                if (mList.Count() > 0)
                {
                    string strPublicMeter = string.Empty;
                    //获取公共表
                    switch (threeType)
                    {
                        case (int)MeterTypeEnum.WaterMeter://水
                            {
                                strPublicMeter = "公区水表";
                            }; break;
                        case (int)MeterTypeEnum.WattHourMeter://电
                            {
                                strPublicMeter = "公区电表";
                            }; break;
                        case (int)MeterTypeEnum.GasMeter://气
                            {
                                strPublicMeter = "公区气表";
                            }; break;
                        default:
                            break;
                    }

                    CustomTreeNodeModel tree = new CustomTreeNodeModel()
                    {
                        id = "0",
                        icon = "fa fa-dashboard",
                        text = strPublicMeter,
                        children = mList.Select(m => new CustomTreeNodeModel()
                        {
                            id = "0_1000_" + m.Id.ToString(),
                            icon = "fa fa-dashboard",
                            children = false,
                            text = m.MeterNum
                        }).ToList()
                    };
                    dataList.Add(tree);
                }
                return dataList;
            }
            //List<int?> listIds = new List<int?>();
            //string[] arr = new string[2];
            ///*获取房屋的ID信息*/
            //foreach (var item in list)
            //{
            //    arr = item.id.Split('_');
            //    if (Convert.ToInt32(arr[1]) == (int)EDeptType.FangWu)
            //    {
            //        if (!listIds.Contains(Convert.ToInt32(arr[0])))
            //        {
            //            listIds.Add(Convert.ToInt32(arr[0]));
            //        }
            //    }
            //}

            //获取该栋楼下的三表
            List<MeterDTO> meterDTOList = GetMeterDTOS(comDeptId, threeType);
            //获取房屋资源树
            List<CustomTreeNodeModel> list = deptService.GetDeptTree(currentAdminUser, id, strFilter).Where(d => meterDTOList.Any(m => m.HouseDeptID.ToString() == d.id.Split('_')[0])).ToList();
            List<CustomTreeNodeModel> listHousHaveThreeMeter = new List<CustomTreeNodeModel>();
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            List<string> listStr = new List<string>();
            /*一个房屋可能会有多只表*/
            foreach (var item in meterDTOList)
            {
                listHousHaveThreeMeter.AddRange(list.Where(o => o.id.Split('_')[0] == item.HouseDeptID.ToString()));
                if (dic.ContainsKey(item.HouseDeptID.ToString()))
                {
                    dic[item.HouseDeptID.ToString()].Add(item.MeterNum + "_" + item.Id);/*表号和表ID*/

                }
                else
                {
                    dic.Add(item.HouseDeptID.ToString(), new List<string>() { item.MeterNum + "_" + item.Id });
                }
            }
            string[] meterInfo = new string[2];
            List<string> isExitsKey = new List<string>();
            //listIds.Clear();
            /*哪些房屋有表*/
            foreach (var item in listHousHaveThreeMeter.Distinct())
            {
                if (!isExitsKey.Contains(item.id.Split('_')[0]))
                {
                    isExitsKey.Add(item.id.Split('_')[0]);
                    list.Remove(item);/*移除原来的元素（原来只有一个元素、如果这个房屋下有两只表那么会新增两个元素、以此类推）*/
                    if (dic.Keys.Contains(item.id.Split('_')[0]))
                    {
                        foreach (string value in dic[item.id.Split('_')[0]])
                        {
                            meterInfo = value.Split('_');
                            list.Add(new CustomTreeNodeModel()
                            {
                                children = false,
                                id = item.id.Split('_')[0] + "_1000_" + meterInfo[1],
                                icon = item.icon,
                                text = meterInfo[0] + "(" + item.text + ")"
                            });
                            /*获取表的ID、方便后面获取表的绑定的时间*/
                            //listIds.Add(Convert.ToInt32(meterInfo[1]));
                        }
                    }
                }
                else
                {
                    list.Add(new CustomTreeNodeModel()
                    {
                        children = false,
                        id = item.id,
                        icon = item.icon,
                        text = item.text,

                    });
                }
            }
            return list;
        }
        #endregion

        #region 获取资源树
        /// <summary>
        /// 获取资源树
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<CustomTreeNodeModel> GetTree(SEC_AdminUserDTO currentAdminUser, int id, int type, int? comDeptId)
        {
            //过滤条件
            string strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";" + (int)EDeptType.LouYu + ";" + (int)EDeptType.FangWu;
            //定义返回的资源树
            List<CustomTreeNodeModel> list = new List<CustomTreeNodeModel>();
            switch (type)
            {
                case (int)EDeptType.LouYu://楼宇
                    {
                        List<CustomTreeNodeModel> listAsynTree = new List<CustomTreeNodeModel>();
                        DeptAppService deptService = new DeptAppService();
                        list = deptService.GetDeptTree(currentAdminUser, id, strFilter);
                    };
                    break;
                case (int)EDeptType.CheKu://车库
                    {
                        var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
                        list = propertyService.GetAsynParkingTree(id).Where(o => o.children == true).Select(s => new CustomTreeNodeModel()
                        {
                            id = s.id,
                            icon = s.icon,
                            text = s.text,
                            children = s.children,
                        }).ToList();
                    };
                    break;
                case (int)MeterTypeEnum.WaterMeter://水表
                    {
                        list = GetListByResType(currentAdminUser, id, strFilter, (int)MeterTypeEnum.WaterMeter, comDeptId);
                    };
                    break;
                case (int)MeterTypeEnum.WattHourMeter://电表
                    {
                        list = GetListByResType(currentAdminUser,id, strFilter, (int)MeterTypeEnum.WattHourMeter, comDeptId);
                    };
                    break;
                case (int)MeterTypeEnum.GasMeter://气表
                    {
                        list = GetListByResType(currentAdminUser, id, strFilter, (int)MeterTypeEnum.GasMeter, comDeptId);
                    };
                    break;
            }
            return list;
        }
        #endregion
    }
}
