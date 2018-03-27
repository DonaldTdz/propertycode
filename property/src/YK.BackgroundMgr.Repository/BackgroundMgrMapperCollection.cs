using KW.Sprite.Common.Repository;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.Repository
{
    public class BackgroundMgrMapperCollection : IMapperCollection
    {
        public IEnumerable<IEntityMapper> Mappers { get; private set; }

        public BackgroundMgrMapperCollection()
        {
            Mappers = new List<IEntityMapper>
            {
                new SEC_AdminUserMapper(),
                new SEC_UserMapper(),
                new SEC_DeptMapper(),
                new Sys_DictionaryMapper(),
                new Sys_DictionaryItemMapper(),
                new SEC_RoleMapper(),
                new SEC_FieldMapper(),
                new SEC_OperateMapper(),
                new SEC_ModuleMapper(),
                new SEC_User_OwnerMapper(),
                new SEC_CommunityMapper(),
                new SEC_BuildingMapper(),
                new SEC_HouseMapper(),
                new SEC_CarportMapper(),
                new SEC_WeChatPublicNumberMapper(),
                new SEC_WeChatOpenIdMapper(),
                new SEC_GarageMapper(),
                new SEC_DeveloperMapper(),
                new SEC_GatewayMapper(),
                new SEC_GatewayAuthMapper(),
                new OtherSysErrorEntityMapper(),
                new SEC_PropertyMapper(),
                new SEC_AreaMapper(),
				new Sms_LogMapper(),
                new Sms_IdentifyingCodeMapper(),
				            };
        }
    }
}
