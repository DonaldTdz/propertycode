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
using PropertyAlipay.Entity.model;
using PropertyAlipay.Service.Services;
using YK.PropertyMgr.Crosscuting;

namespace YK.PropertyMgr.ApplicationService
{
    public partial class AlipayAPPAuthTokenAppService
    {
        public bool IsCheckIsOAuth(int ProDeptId)
        {

            Condition<AlipayAPPAuthToken> condition = new Condition<AlipayAPPAuthToken>(o => o.ProDeptId == ProDeptId);
            AlipayAPPAuthTokenDomainService _AlipayAPPAuthTokenDomainService = new AlipayAPPAuthTokenDomainService();

            var List = _AlipayAPPAuthTokenDomainService.GetAlipayAppAuthTokenList(condition.ExpressionBody);


            if (List != null && List.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public AlipayAPPAuthTokenDTO GetAlipayAPPAuthTokenDTOByProDepetId(int ProDeptId)
        {
            Condition<AlipayAPPAuthToken> condition = new Condition<AlipayAPPAuthToken>(o => o.ProDeptId == ProDeptId);
            AlipayAPPAuthTokenDomainService _AlipayAPPAuthTokenDomainService = new AlipayAPPAuthTokenDomainService();
            var List = _AlipayAPPAuthTokenDomainService.GetAlipayAppAuthTokenList(condition.ExpressionBody);
            if (List != null && List.Count > 0)
            {
                var ListDTO = AlipayAPPAuthTokenMappers.ChangeAlipayAPPAuthTokenToDTOs(List).ToList();
                return ListDTO.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }


        public string  GetAppAuthTokenQuery(int DeptId)
        {

            Condition<AlipayAPPAuthToken> condition = new Condition<AlipayAPPAuthToken>(o => o.ProDeptId == DeptId);
            AlipayAPPAuthTokenDomainService _AlipayAPPAuthTokenDomainService = new AlipayAPPAuthTokenDomainService();
            var List = _AlipayAPPAuthTokenDomainService.GetAlipayAppAuthTokenList(condition.ExpressionBody);
            if (List != null && List.Count > 0)
            {
                var ListDTO = AlipayAPPAuthTokenMappers.ChangeAlipayAPPAuthTokenToDTOs(List).ToList().FirstOrDefault();
                var resultModel=  AlipayOAuthTokenService.Instance.GetAuthTokenAppQuery(ListDTO.app_auth_token);

                var ResponseModel = JsonHelper.JsonDeserializeByNewtonsoft<AppAuthTokenQueryResponseModel>(resultModel.data);

                return JsonHelper.JsonSerializerByNewtonsoft<Alipay_Open_Auth_Token_App_Query_Response>(ResponseModel.alipay_open_auth_token_app_query_response);
            }
            else
            {
                return JsonHelper.JsonSerializerByNewtonsoft<Alipay_Open_Auth_Token_App_Query_Response>(new Alipay_Open_Auth_Token_App_Query_Response() );
            }

        }

        public bool SaveAppAuthToken(int? DeptId,bool Isauthorization_code, string app_auth_code=null)
        {

            var DataBaseAPPAuthToken = GetAlipayAPPAuthTokenDTOByProDepetId(DeptId.Value);
            AlipayAPPAuthTokenDTO alipayAPPAuthTokenDTO = new AlipayAPPAuthTokenDTO();
            var AlipayResult = new  PayResultModel();
            if (DataBaseAPPAuthToken != null)
            {
                alipayAPPAuthTokenDTO = DataBaseAPPAuthToken;
            }

            if (Isauthorization_code)
            {//换取授权码
                 AlipayResult = AlipayOAuthTokenService.Instance.GetAppOAuthAuthorizationCode(app_auth_code);
            }
            else
            {//刷新授权码
                if (DataBaseAPPAuthToken != null)
                    AlipayResult = AlipayOAuthTokenService.Instance.GetAppOAuthrefresh_token(DataBaseAPPAuthToken.app_refresh_token);
                else
                {
                    LogProperty.WriteLoginToFile(string.Format("DeptId:{0}", DeptId), "AlipayAPPAuthTokenAppService/SaveAppAuthToken", FileLogType.Exception);
                    throw new Exception("该物业没有授权信息存储，无法更新令牌");
                }
            }
            var AppAuthTokenResponse = JsonConvert.DeserializeObject<AppAuthTokenResponseModel>(AlipayResult.data);
            var resonseModel = AppAuthTokenResponse.alipay_open_auth_token_app_response;
            if (DataBaseAPPAuthToken != null)
            {
                alipayAPPAuthTokenDTO = DataBaseAPPAuthToken;

            }
            if (string.IsNullOrEmpty(alipayAPPAuthTokenDTO.Id))
                alipayAPPAuthTokenDTO.Id = Guid.NewGuid().ToString();
            alipayAPPAuthTokenDTO.ProDeptId = DeptId;
            alipayAPPAuthTokenDTO.auth_app_id = resonseModel.auth_app_id;
            alipayAPPAuthTokenDTO.app_auth_token = resonseModel.app_auth_token;
            alipayAPPAuthTokenDTO.app_refresh_token = resonseModel.app_refresh_token;
            alipayAPPAuthTokenDTO.expires_in = resonseModel.expires_in;
            alipayAPPAuthTokenDTO.re_expires_in = resonseModel.re_expires_in;
            alipayAPPAuthTokenDTO.user_id = resonseModel.user_id;

            if (DataBaseAPPAuthToken != null)
            {
                return UpdateAlipayAPPAuthToken(alipayAPPAuthTokenDTO);
            }
            else
            {
                 return InsertAlipayAPPAuthToken(alipayAPPAuthTokenDTO);
              
            }


               
        }



    }
}
