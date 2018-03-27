using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.ApplicationMapper;
using YK.BackgroundMgr.DomainService;

namespace YK.BackgroundMgr.ApplicationService
{
	public partial class Sys_DictionaryItemAppService
	{
		private Sys_DictionaryItemDomainService _Sys_DictionaryItemDomainService;
        protected Sys_DictionaryItemDomainService Sys_DictionaryItemService
        {
            get
            {
                if (_Sys_DictionaryItemDomainService == null)
                {
                    _Sys_DictionaryItemDomainService = new Sys_DictionaryItemDomainService();
                }

                return _Sys_DictionaryItemDomainService;
            }
        }   

        public bool InsertSys_DictionaryItem(Sys_DictionaryItemDTO dtoSys_DictionaryItem)
        {
            var domainSys_DictionaryItem = Sys_DictionaryItemMappers.ChangeDTOToSys_DictionaryItemNew(dtoSys_DictionaryItem);

            return Sys_DictionaryItemService.InsertSys_DictionaryItem(domainSys_DictionaryItem);
        }

        public bool UpdateSys_DictionaryItem(Sys_DictionaryItemDTO dtoSys_DictionaryItem)
        {
            var domainSys_DictionaryItem = Sys_DictionaryItemMappers.ChangeDTOToSys_DictionaryItemNew(dtoSys_DictionaryItem);

            return Sys_DictionaryItemService.UpdateSys_DictionaryItem(domainSys_DictionaryItem);
        }

        public bool DeleteSys_DictionaryItem(object id)
        {
            return Sys_DictionaryItemService.DeleteSys_DictionaryItem(id);
        }

        public List<Sys_DictionaryItemDTO> GetSys_DictionaryItems()
        {
            var domainSys_DictionaryItems = Sys_DictionaryItemService.GetSys_DictionaryItems();

            return Sys_DictionaryItemMappers.ChangeSys_DictionaryItemToDTOs(domainSys_DictionaryItems);
        }

		public Sys_DictionaryItemDTO GetSys_DictionaryItemByKey(object id)
        {
            var domainSys_DictionaryItem = Sys_DictionaryItemService.GetSys_DictionaryItemByKey(id);

            return Sys_DictionaryItemMappers.ChangeSys_DictionaryItemToDTO(domainSys_DictionaryItem);
        }
	}
}
