using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;

namespace YK.PropertyMgr.ApplicationService
{
    public partial class CountyAppService
    {
        #region 获取对象集合
        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <returns></returns>
        public List<CountyDTO> GetCountyList(int cityId)
        {
            return CountyMappers.ChangeCountyToDTOs(CountyService.GetCountryList(o => o.CityID == cityId));
        }
        #endregion
    }
}
