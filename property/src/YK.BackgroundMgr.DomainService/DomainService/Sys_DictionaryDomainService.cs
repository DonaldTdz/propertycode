using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;
using YK.BackgroundMgr.RepositoryContract;
using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.Crosscuting;

namespace YK.BackgroundMgr.DomainService
{
	public partial class Sys_DictionaryDomainService
	{
        public List<Sys_DictionaryItem> GetSys_DictionaryItems(int dictionaryId)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var dictionary = _BackgroundMgrUnitOfWork.Sys_DictionaryRepository.GetByKey(dictionaryId);
                if (dictionary != null)
                {
                    return dictionary.Sys_DictionaryItems.ToList();
                }
                return new List<Sys_DictionaryItem>();
            }
        }

        public List<Sys_DictionaryItem> GetSys_DictionaryItems(string code)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var dictionary = _BackgroundMgrUnitOfWork.Sys_DictionaryRepository.GetAll().Where(s => s.EnName == code).FirstOrDefault();
                if (dictionary != null)
                {
                    return dictionary.Sys_DictionaryItems.ToList();
                }
                return new List<Sys_DictionaryItem>();
            }
        }
    }
}
