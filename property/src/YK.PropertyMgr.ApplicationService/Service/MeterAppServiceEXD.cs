using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;

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
    }
}
