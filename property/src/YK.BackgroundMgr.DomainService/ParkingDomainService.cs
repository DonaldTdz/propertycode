using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;
using YK.ParkingSys.RepositoryContract;
using YK.ParkingSys.DomainEntity;
using YK.BackgroundMgr.Crosscuting;

namespace YK.ParkingSys.DomainService
{
	public partial class ParkingDomainService
	{
		public bool InsertParking(Parking domainParking)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                _ParkingSysUnitOfWork.ParkingRepository.Add(domainParking);
                _ParkingSysUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateParking(Parking domainParking)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                _ParkingSysUnitOfWork.ParkingRepository.Update(domainParking);
                _ParkingSysUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteParking(object id)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                _ParkingSysUnitOfWork.ParkingRepository.Delete(id);
                _ParkingSysUnitOfWork.Commit();
                return true;
            }
        }

		public Parking GetParkingByKey(object id)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                return _ParkingSysUnitOfWork.ParkingRepository.GetByKey(id);
            }
        }

        public List<Parking> GetParkings()
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                return _ParkingSysUnitOfWork.ParkingRepository.GetAll().ToList();
            }
        }
	}
}
