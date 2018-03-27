using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using YK.PropertyMgr.CompositeAppService;
using YK.PropertyMgr.ApplicationService;
using YK.BackgroundMgr.DomainInterface;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.DomainService;
using YK.PropertyMgr.CompositeDomainService;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;

namespace YK.PropertyMgr.ApplicationService
{
    public partial class CarParkAppService
    {
        public IList<CustomTreeNodeModel> GetCarParkTree(string UserName,string keyword="")
        {
            return  DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCarParkTree(UserName, keyword);
        }

        public IList<CustomTreeNodeModel> GetCarParkByCommunityId(string CommunityId)
        {
            return DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCarParkByCommunityId(CommunityId);
        }

        public IList<CustomTreeNodeModel> GetCarportByParkId(string ParkingId)
        {
            return DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCarportByParkId(ParkingId);
        }

        public int GetHouseDeptIdByCarPort(int CarPortId)
        {

             var  CarPort=   DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCarPortById(CarPortId);

            if (CarPort != null)
            {
                return CarPort.HouseDeptID ?? 0;
            }
            return 0;

           
        }


    }
}
