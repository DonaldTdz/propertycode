using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using Microsoft.Practices.Unity;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;

namespace YK.PropertyMgr.DomainService
{
    public partial class EntranceDomainService
    {
        public IList<Entrance> GetEntranceDTOList(int PageIndex, int PageSize, Expression<Func<Entrance, bool>> predicate, string expressions, out int totalCount)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var dataList = propertyMgrUnitOfWork.EntranceRepository.Paging(PageIndex, PageSize, predicate, expressions, out totalCount).ToList();
                foreach (var item in dataList)
                {
                    item.ProvinceID = item.Province.Id;
                    item.CityID = item.City.Id;
                    item.CountyID = item.County.Id;
                }
                return dataList;
            }
        }
        /// <summary>
        /// 获取设备
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IList<Entrance> GetEntrances(Expression<Func<Entrance, bool>> predicate)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var dataList = propertyMgrUnitOfWork.EntranceRepository.GetAll().Where(predicate).ToList();
                return dataList;
            }
        }
        /// <summary>
        /// 获取设备
        /// </summary>
        /// <param name="keyId">设备的ID</param>
        /// <returns></returns>
        public Entrance GetEntrance(int keyId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var model = propertyMgrUnitOfWork.EntranceRepository.GetAll().Where(o => o.KeyID == keyId && o.State == 1).FirstOrDefault();
                return model;
            }
        }
        public Entrance GetEntranceById(int entranceId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var model = propertyMgrUnitOfWork.EntranceRepository.GetAll().Where(o => o.Id == entranceId && o.State == 1).FirstOrDefault();
                return model;
            }
        }
        /// <summary>
        /// 獲取用戶授權的設備
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<EntrancePersonal> GetEntrancePersonal(Expression<Func<EntrancePersonal, bool>> where)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var entranceUsers = propertyMgrUnitOfWork.EntranceUserRepository.GetAll();
                var entrances = propertyMgrUnitOfWork.EntranceRepository.GetAll();
                var data = (from a in entranceUsers
                            join b in entrances on a.EntranceID equals b.Id
                            select new EntrancePersonal()
                            {
                                Id = b.Id,
                                KeyID = b.KeyID,
                                Name = b.Name,
                                CreateTime = a.CreateTime,
                                KeyExpireTime = a.KeyExpireTime,
                                VillageID = b.VillageID,
                                State = b.State,
                                UserId = a.UserOwnerInfoId,
                                Address = b.Address,
                                BuildId = b.BuildId,
                                UnitName = b.UnitName,
                                DeviceType = string.Empty
                            }).Where(where).ToList();
                return data;
            }
        }
        /// <summary>
        /// 新增大门返回大门ID信息
        /// </summary>
        /// <param name="domainEntrance"></param>
        /// <returns></returns>
        public int InsertEntranceReturnId(Entrance domainEntrance)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.EntranceRepository.Add(domainEntrance);
                propertyMgrUnitOfWork.Commit();
                return Convert.ToInt32(domainEntrance.Id);
            }
        }
        /// <summary>
        /// 重新绑定设备的KEYID
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool ChangeEntranceKey(Entrance entrance)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.EntranceRepository.Update(entrance);
                return propertyMgrUnitOfWork.Commit();
            }
        }

        /// <summary>
        /// 重新绑定设备的KEYID
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool ChangeEntranceKey(List<Entrance> list )
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.EntranceRepository.UpdateRange(list);
                return propertyMgrUnitOfWork.Commit();
            }
        }
    }
}
