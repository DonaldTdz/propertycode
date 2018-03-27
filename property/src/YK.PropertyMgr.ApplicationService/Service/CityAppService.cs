using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.DomainEntity;

namespace YK.PropertyMgr.ApplicationService
{
    public partial class CityAppService
    {
        #region 获取对象集合
        /// <summary>
        /// 根据条件获取City集合
        /// </summary>
        /// <returns></returns>
        public List<CityDTO> GetCityList(int provinceId)
        {
            return CityMappers.ChangeCityToDTOs(CityService.GetCityList(o => o.ProvinceID == provinceId));
        }
        #endregion
    }
}
