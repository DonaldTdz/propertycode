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
	public partial class CarportDomainService
	{
		public bool InsertCarport(Carport domainCarport)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                _ParkingSysUnitOfWork.CarportRepository.Add(domainCarport);
                _ParkingSysUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateCarport(Carport domainCarport)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                _ParkingSysUnitOfWork.CarportRepository.Update(domainCarport);
                _ParkingSysUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteCarport(object id)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                _ParkingSysUnitOfWork.CarportRepository.Delete(id);
                _ParkingSysUnitOfWork.Commit();
                return true;
            }
        }

		public Carport GetCarportByKey(object id)
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                return _ParkingSysUnitOfWork.CarportRepository.GetByKey(id);
            }
        }

        public List<Carport> GetCarports()
        {
            using (var _ParkingSysUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IParkingSysUnitOfWork>())
            {
                return _ParkingSysUnitOfWork.CarportRepository.GetAll().ToList();
            }
        }
	}
}
