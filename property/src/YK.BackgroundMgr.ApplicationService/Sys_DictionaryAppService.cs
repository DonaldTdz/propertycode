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
	public partial class Sys_DictionaryAppService
	{
		private Sys_DictionaryDomainService _Sys_DictionaryDomainService;
        protected Sys_DictionaryDomainService Sys_DictionaryService
        {
            get
            {
                if (_Sys_DictionaryDomainService == null)
                {
                    _Sys_DictionaryDomainService = new Sys_DictionaryDomainService();
                }

                return _Sys_DictionaryDomainService;
            }
        }   

        public bool InsertSys_Dictionary(Sys_DictionaryDTO dtoSys_Dictionary)
        {
            var domainSys_Dictionary = Sys_DictionaryMappers.ChangeDTOToSys_DictionaryNew(dtoSys_Dictionary);

            return Sys_DictionaryService.InsertSys_Dictionary(domainSys_Dictionary);
        }

        public bool UpdateSys_Dictionary(Sys_DictionaryDTO dtoSys_Dictionary)
        {
            var domainSys_Dictionary = Sys_DictionaryMappers.ChangeDTOToSys_DictionaryNew(dtoSys_Dictionary);

            return Sys_DictionaryService.UpdateSys_Dictionary(domainSys_Dictionary);
        }

        public bool DeleteSys_Dictionary(object id)
        {
            return Sys_DictionaryService.DeleteSys_Dictionary(id);
        }

        public List<Sys_DictionaryDTO> GetSys_Dictionarys()
        {
            var domainSys_Dictionarys = Sys_DictionaryService.GetSys_Dictionarys();

            return Sys_DictionaryMappers.ChangeSys_DictionaryToDTOs(domainSys_Dictionarys);
        }

		public Sys_DictionaryDTO GetSys_DictionaryByKey(object id)
        {
            var domainSys_Dictionary = Sys_DictionaryService.GetSys_DictionaryByKey(id);

            return Sys_DictionaryMappers.ChangeSys_DictionaryToDTO(domainSys_Dictionary);
        }
	}
}
