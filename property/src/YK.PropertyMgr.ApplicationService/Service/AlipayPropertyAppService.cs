using System.Linq;
using Microsoft.Practices.Unity;
using YK.PropertyMgr.RepositoryContract;
using YK.PropertyMgr.Crosscuting;
using System.Collections.Generic;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainInterface;
using YK.PropertyMgr.DomainService;
using YK.BackgroundMgr.DomainEntity;
using PropertyAlipay.Entity.model;
using PropertyAlipay.Service.Services;
using System;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.BackgroundMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;
using Newtonsoft.Json.Linq;
using PropertyAlipay.Service.StatusHelper;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using System.Threading.Tasks;
using YK.PropertyMgr.CompositeDomainService;
using System.Threading;

namespace YK.PropertyMgr.ApplicationService.Service
{
    public class AlipayPropertyAppService
    {
        private const string prefix = "RAP";
        private const string Deleteprefix = "DAP";
        private const string SuccessCode = "10000";
        private const int SynchronizationPageSize = 500;
        private const string AlipayCommunityqueryResponse = "alipay_eco_cplife_community_details_query_response";
        private const string AlipayBasicServiceInitializeResponse = "alipay_eco_cplife_basicservice_initialize_response";
        #region 小区
        public List<AlipayCommunityDTO> GetAlipayCommunityList(int DeptId)
        {
            AlipayCommunityDomainService _AlipayCommunityDomainService = new AlipayCommunityDomainService();
            Condition<AlipayCommunity> condition = new Condition<AlipayCommunity>(o => o.ProDeptId == DeptId && o.IsDel == false);
            //获取该物业下的小区
            var ComDeptInfoList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoListByPropertyId(DeptId);
            //然后获取保存的支付宝同步小区
            var AlipayCommunityList = _AlipayCommunityDomainService.GetAlipayCommunityList(condition.ExpressionBody).ToList();
            //物业小区左连接支付宝
            var query = from d in ComDeptInfoList
                        join ac in AlipayCommunityList on d.Id equals ac.ComDeptId into ac_left
                        from acTemple in ac_left.DefaultIfEmpty()
                        select new AlipayCommunityDTO
                        {
                            Id = acTemple == null ? 0 : acTemple.Id,
                            ComDeptId = d.Id,
                            CommunityName = acTemple != null ? acTemple.CommunityName : d.Name,
                            AlipayCommunityId = acTemple != null ? acTemple.AlipayCommunityId : "",
                            ProDeptId = d.PId,
                            Status = acTemple != null ? acTemple.Status : 0,
                            CreateTime = acTemple != null ? acTemple.CreateTime : null,
                            StatusStr = acTemple != null ? "已接入" : "未接入",
                            CreateTimeStr = acTemple != null ? acTemple.CreateTime.Value.ToString("yyyy-MM-dd HH:mm") : ""
                        };
            return query.ToList();
        }
        public List<AlipayCommunityDTO> GetAlipayCommunityBasicService(int ProDeptId)
        {
            AlipayCommunityDomainService _AlipayCommunityDomainService = new AlipayCommunityDomainService();
            Condition<AlipayCommunity> condition = new Condition<AlipayCommunity>(o => o.ProDeptId == ProDeptId);
            var AlipayCommunityList = AlipayCommunityMappers.ChangeAlipayCommunityToDTOs(_AlipayCommunityDomainService.GetAlipayCommunityList(condition.ExpressionBody).ToList());
            return AlipayCommunityList;
        }

        public Community GetNewCommunityResponseModelByComDeptId(int? ComDeptId)
        {
            var Community = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetGetCommunityById(ComDeptId.Value);

            Community NewModel = new Community()
            {
                community_name = Community.Name,
                out_community_id = ComDeptId.ToString()
            };
            return NewModel;
        }

        public IList<SEC_AreaDTO> GetSec_AreaList(int Type, int PId = 0)
        {
            Condition<SEC_Area> condition = new Condition<SEC_Area>(o => true);
            if (PId == 0)
            {//省份
                condition = condition & new Condition<SEC_Area>(o => o.AreaType == Type);
            }
            else
            {//市 区
                condition = condition & new Condition<SEC_Area>(o => o.PId == PId);
            }
            var SEC_AreaList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetSec_AreaList(condition.ExpressionBody);

            return SEC_AreaList.ToList();
        }


        public ResultModel SaveAlipayCommunity(Community Community, string CreateOperator)
        {
            //修改坐标格式

            var Location = (Community.community_locations[0]).ToString().Split('/');

            if (Location.Count() == 0)
            {
                return new ResultModel()
                {
                    IsSuccess = false,
                    Msg = "请填写正确格式的经纬度"
                };
            }
            var _locationsList = new List<string>();

            foreach (string c in Location)
            {
                var a = c.Replace(',', '|');
                _locationsList.Add(a);
            }
            //获取APPToken


            var CommunityDept = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoById(Community.out_community_id);
            Community.community_locations = _locationsList.ToArray();
            AlipayAPPAuthTokenAppService _AlipayAPPAuthTokenAppService = new AlipayAPPAuthTokenAppService();
            var APPAuthToken = _AlipayAPPAuthTokenAppService.GetAlipayAPPAuthTokenDTOByProDepetId(CommunityDept.PId.Value);
            AlipayCommunityDomainService _AlipayCommunityDomainService = new AlipayCommunityDomainService();
            if (string.IsNullOrEmpty(Community.community_id))
            {




                var AlipayResult = AliPayCommunityService.Instance.CommunityCreate(Community, APPAuthToken.app_auth_token);
                var ResponseModel = JsonHelper.JsonDeserialize<CommunityCreateResponseModel>(AlipayResult.data);
                if (ResponseModel.alipay_eco_cplife_community_create_response.code == "10000")
                {
                    AlipayCommunity AlipayCommunity = new AlipayCommunity()
                    {
                        AlipayCommunityId = ResponseModel.alipay_eco_cplife_community_create_response.community_id,
                        ComDeptId = CommunityDept.Id,
                        CommunityName = Community.community_name,
                        CreateTime = DateTime.Now,
                        CreateOperator = CreateOperator,
                        UpdateOperator = CreateOperator,
                        UpdateTime = DateTime.Now,
                        ProDeptId = CommunityDept.PId,
                        IsDel = false,
                        Status = (int)AlipayCommunityCreateStatusEnum.Enable,
                         IsInitialize=false,


                    };
                    _AlipayCommunityDomainService.InsertAlipayCommunity(AlipayCommunity);
                    return new ResultModel()
                    {
                        IsSuccess = true,
                        Msg = "保存成功"
                    };


                }
                else
                {
                    LogProperty.WriteLoginToFile(string.Format("DeptId:{0},Message:{1}", Community.out_community_id, AlipayResult.data), "AlipayPropertyAppService/SaveAlipayCommunity", FileLogType.Exception);
                    return new ResultModel()
                    {
                        IsSuccess = false,
                        Msg = "保存失败" + ResponseModel.alipay_eco_cplife_community_create_response.msg
                    };
                }



            }
            else
            {//修改
                var AlipayResult = AliPayCommunityService.Instance.CommunityModify(Community, APPAuthToken.app_auth_token);
                var anonymous = new { alipay_eco_cplife_community_modify_response = new { msg = string.Empty, code = string.Empty }, sign = string.Empty };
                var ResponAnonymous = JsonHelper.DeserializeAnonymousTypeByNewtonsoft(AlipayResult.data, anonymous);
                if (ResponAnonymous.alipay_eco_cplife_community_modify_response.code == SuccessCode)
                {
                    return new ResultModel()
                    {
                        IsSuccess = true,
                        Msg = "保存成功"
                    };
                }
                else
                {
                    LogProperty.WriteLoginToFile(string.Format("DeptId:{0},Message:{1}", Community.out_community_id, AlipayResult.data), "AlipayPropertyAppService/SaveAlipayCommunity", FileLogType.Exception);
                    return new ResultModel()
                    {
                        IsSuccess = false,
                        Msg = "失败" + ResponAnonymous.alipay_eco_cplife_community_modify_response.msg
                    };
                }


            }

        }


        public Community GetCommunityById(string AlipayCommunityId, int ProDeptID)
        {
            AlipayAPPAuthTokenAppService _AlipayAPPAuthTokenAppService = new AlipayAPPAuthTokenAppService();

            var AlipayAPPAuthToken = _AlipayAPPAuthTokenAppService.GetAlipayAPPAuthTokenDTOByProDepetId(ProDeptID);
            //获取Token授权
            var ResponModel = AliPayCommunityService.Instance.CommunityDetailsQuery(AlipayCommunityId, AlipayAPPAuthToken.app_auth_token);
            var anonymous = new { alipay_eco_cplife_community_details_query_response = new { next_action = string.Empty, community_name = string.Empty, hotline = string.Empty, province_code = string.Empty, community_address = string.Empty, city_code = string.Empty, district_code = string.Empty, out_community_id = string.Empty, community_locations = new string[] { } }, sign = string.Empty };
            var ResponAnonymous = JsonHelper.DeserializeAnonymousTypeByNewtonsoft(ResponModel.data, anonymous);

            return new Community()
            {
                community_name = ResponAnonymous.alipay_eco_cplife_community_details_query_response.community_name,
                province_code = ResponAnonymous.alipay_eco_cplife_community_details_query_response.province_code,
                city_code = ResponAnonymous.alipay_eco_cplife_community_details_query_response.city_code,
                district_code = ResponAnonymous.alipay_eco_cplife_community_details_query_response.district_code,
                community_address = ResponAnonymous.alipay_eco_cplife_community_details_query_response.community_address,
                hotline = ResponAnonymous.alipay_eco_cplife_community_details_query_response.hotline,
                out_community_id = ResponAnonymous.alipay_eco_cplife_community_details_query_response.out_community_id,
                community_id = AlipayCommunityId,
                community_locations = ResponAnonymous.alipay_eco_cplife_community_details_query_response.community_locations

            };

        }


        public string GetQRCodeImage(string AlipayCommunityId, int ProDeptID)
        {
            AlipayAPPAuthTokenAppService _AlipayAPPAuthTokenAppService = new AlipayAPPAuthTokenAppService();
            var AlipayAPPAuthToken = _AlipayAPPAuthTokenAppService.GetAlipayAPPAuthTokenDTOByProDepetId(ProDeptID);
            //获取Token授权
            var ResponModel = AliPayCommunityService.Instance.CommunityDetailsQuery(AlipayCommunityId, AlipayAPPAuthToken.app_auth_token);
            JObject obj = JsonHelper.JsonDeserializeByNewtonsoft(ResponModel.data) as JObject;
          
            if (obj[AlipayCommunityqueryResponse]["community_services"].Count() == 0)
                return string.Empty;
            return (string)obj[AlipayCommunityqueryResponse]["community_services"][0]["qr_code_image"];
        }

        public bool CheckCommunityBaseService(string AlipayCommunityId, int ProDeptID)
        {
          
                AlipayAPPAuthTokenAppService _AlipayAPPAuthTokenAppService = new AlipayAPPAuthTokenAppService();
                var AlipayAPPAuthToken = _AlipayAPPAuthTokenAppService.GetAlipayAPPAuthTokenDTOByProDepetId(ProDeptID);
                //获取Token授权
                var ResponModel = AliPayCommunityService.Instance.CommunityDetailsQuery(AlipayCommunityId, AlipayAPPAuthToken.app_auth_token);
                JObject obj = JsonHelper.JsonDeserializeByNewtonsoft(ResponModel.data) as JObject;

            if (obj[AlipayCommunityqueryResponse]["community_services"].Count() == 0)
                return false;
            else
                return true;
            
           
        }

        #endregion

        #region 基础服务初始化

        public AlipayCommunityBasicserviceDTO GetBasicserviceInfomation(string AlipayCommunityId, int ProDeptID)
        {
            AlipayAPPAuthTokenAppService _AlipayAPPAuthTokenAppService = new AlipayAPPAuthTokenAppService();

            var AlipayAPPAuthToken = _AlipayAPPAuthTokenAppService.GetAlipayAPPAuthTokenDTOByProDepetId(ProDeptID);
            //获取Token授权
            var ResponModel = AliPayCommunityService.Instance.CommunityDetailsQuery(AlipayCommunityId, AlipayAPPAuthToken.app_auth_token);
            JObject obj = JsonHelper.JsonDeserializeByNewtonsoft(ResponModel.data) as JObject;

            AlipayCommunityBasicserviceDTO alipayCommunityBasicserviceDTO = new AlipayCommunityBasicserviceDTO()
            {
                community_id = AlipayCommunityId,
                service_type = "PROPERTY_PAY_BILL_MODE",
                external_invoke_address = (string)obj["alipay_eco_cplife_community_details_query_response"]["community_services"][0]["external_invoke_address"],
                status = (string)obj["alipay_eco_cplife_community_details_query_response"]["community_services"][0]["status"],
                service_expires = ((DateTime)obj["alipay_eco_cplife_community_details_query_response"]["community_services"][0]["service_expires"]).ToString("yyyy-MM-dd")
            };


            return alipayCommunityBasicserviceDTO;
        }


        public List<Status> GetBasicServiceStatus()
        {
            return ConversionAlipayStatus.GetBasicServiceStatus().ToList();
        }
        public List<Status> GetBasicServiceType()
        {
            return ConversionAlipayStatus.GetBasicServiceType().ToList();
        }

        public ResultModel SaveAlipayCommunityBasicservice(AlipayCommunityBasicserviceDTO model, string CreateOperator)
        {
            AlipayCommunityDomainService _AlipayCommunityDomainService = new AlipayCommunityDomainService();
            Condition<AlipayCommunity> condition = new Condition<AlipayCommunity>(o => o.AlipayCommunityId == model.community_id && o.IsDel == false);
            var AlipayCommunityModelList = _AlipayCommunityDomainService.GetAlipayCommunityList(condition.ExpressionBody);
            if (AlipayCommunityModelList.Count > 0)
            {
                //获取Token 
                var AlipayCommunityModel = AlipayCommunityModelList.FirstOrDefault();
                var APPAuthTokenstr = this.GetAPPAuthToken(AlipayCommunityModel.ProDeptId.Value);

                if (AlipayCommunityModel.IsInitialize != null&& AlipayCommunityModel.IsInitialize.Value)
                {//修改

                    InitializeCommunityModify initializeCommunityModify = new InitializeCommunityModify()
                    {
                        community_id = model.community_id,
                        external_invoke_address = model.external_invoke_address,
                        service_expires = model.service_expires + " 23:59:59",
                        service_type = model.service_type


                    };
                    if (model.status != "PENDING_ONLINE")
                        initializeCommunityModify.status = model.status;

                    var AlipayResult = AlipayCommunityInitializeService.Instance.CommunityInitializeModify(initializeCommunityModify, APPAuthTokenstr);
                    if (AlipayResult.isSuccess)
                        return new ResultModel() { IsSuccess = true, Msg = "保存成功" };
                    else
                    {
                        LogProperty.WriteLoginToFile(string.Format("AlipayCommunityId:{0},Message:{1}", model.community_id, AlipayResult.data), "AlipayPropertyAppService/SaveAlipayCommunityBasicservice", FileLogType.Exception);

                        return new ResultModel() { IsSuccess = false, Msg = "保存失败" + AlipayResult.msg };
                    }

                }
                else
                {//新增

                    InitializeCommunity initializeCommunity = new InitializeCommunity()
                    {
                        community_id = model.community_id,
                        external_invoke_address = model.external_invoke_address,
                        service_expires = model.service_expires + " 23:59:59",
                        service_type = model.service_type


                    };

                    var AlipayResult = AlipayCommunityInitializeService.Instance.CommunityInitialize(initializeCommunity, APPAuthTokenstr);

                    JObject obj = JsonHelper.JsonDeserializeByNewtonsoft(AlipayResult.data) as JObject;
                    var ResponseCode = (string)obj[AlipayBasicServiceInitializeResponse]["code"];
                    var msg ="支付宝网关反馈"+ (string)obj[AlipayBasicServiceInitializeResponse]["msg"];

                    if (ResponseCode == "10000")
                    {


                        AlipayCommunityModel.IsInitialize = true;
                        _AlipayCommunityDomainService.UpdateAlipayCommunity(AlipayCommunityModel);

                        return new ResultModel() { IsSuccess = true, Msg = "保存成功" };
                    }
                    else if (ResponseCode == "SYSTEM_ERROR")
                    {//重新查询后进行

                        var IsCheck = CheckCommunityBaseService(AlipayCommunityModel.AlipayCommunityId, AlipayCommunityModel.ProDeptId.Value);
                        AlipayCommunityModel.IsInitialize = IsCheck;
                        _AlipayCommunityDomainService.UpdateAlipayCommunity(AlipayCommunityModel);
                        return new ResultModel() { IsSuccess = true, Msg = "保存成功" };

                    }
                    else if (ResponseCode == "SERVICE_UNIQUENESS_VIOLATION")
                    {
                        AlipayCommunityModel.IsInitialize = true;
                        _AlipayCommunityDomainService.UpdateAlipayCommunity(AlipayCommunityModel);
                        return new ResultModel() { IsSuccess = true, Msg = "保存成功" };
                    }

                    else
                    {
                        LogProperty.WriteLoginToFile(string.Format("AlipayCommunityId:{0},Message:{1}", model.community_id, AlipayResult.data), "AlipayPropertyAppService/SaveAlipayCommunityBasicservice", FileLogType.Exception);

                        return new ResultModel() { IsSuccess = false, Msg = "保存失败" + msg };
                    }
                }


            }
            else
            {
                return new ResultModel() { IsSuccess = false, Msg = "未找到对应的支付宝小区记录" };
            }








        }



        #endregion

        #region 小区房间
        public IEnumerable<TemplateModel> GetALipayRoomTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "HouseNumber", ColumnDesc = "房间号", Seq = i++},
                new TemplateColumn(){ ColumnName = "BatchCode", ColumnDesc = "批次号", Seq = i++},
                new TemplateColumn(){ ColumnName = "CreateTime", ColumnDesc = "上传时间", Seq = i++}

            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(AlipayRoom), showColumns);
            return template;
        }
        public IList<AlipayRoomDTO> GetALipayRoomList(AlipayPropertySearchDTO searchDto, out int totalCount)
        {
            AlipayRoomDomainService _AlipayRoomDomainService = new AlipayRoomDomainService();
            Condition<AlipayRoom> condition = new Condition<AlipayRoom>(c => c.IsDel == false);
            if (searchDto.ComDeptId != null)
            {
                condition = condition & new Condition<AlipayRoom>(c => c.ComDeptId == searchDto.ComDeptId);
            }
            else
            {
                totalCount = 0;
                return new List<AlipayRoomDTO>();
            }

            string expressions = "CreateTime desc";
            var Roominfolist = _AlipayRoomDomainService.Paging(searchDto.PageIndex, searchDto.PageSize, condition.ExpressionBody, expressions, out totalCount).ToList();

            return AlipayRoomMappers.ChangeAlipayRoomToDTOs(Roominfolist);

        }
        public CustomTreeNodeModel GetAlipayRoomSelectHouseDeptList(int ComDeptId)
        {

            //获取该小区的上传记录
            AlipayRoomDomainService _AlipayRoomDomainService = new AlipayRoomDomainService();
            Condition<AlipayRoom> condition = new Condition<AlipayRoom>(o => o.ComDeptId == ComDeptId && o.IsDel == false);
            var AlipayRoomList = _AlipayRoomDomainService.GetAlipayRoomList(condition.ExpressionBody).ToList();
            var HouseDeptIdlist = AlipayRoomList.Select(o => o.HouseDeptId).ToList();

            return DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptTreeExcludeIdsbyComDeptId(HouseDeptIdlist, ComDeptId);
        }
        public async Task<ResultModel> SaveAlipayRoomUpload(int?[] ids, int? ComDeptId, string OperatorName, int OperatorId)
        {
            //获取alipay小区
            var AlipayCommunity = this.GetAlipayCommunityId(ComDeptId);
            //获取Token
            var AppAuthToken = this.GetAPPAuthToken(AlipayCommunity.ProDeptId);
            var HouseDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptsByIds(ids.ToList()).Where(o => o.DeptType == (int)EDeptType.FangWu).ToList();
            var GroupHouseDeptList = ListHelper.GetListGroup<DeptInfo>(HouseDeptList, 200);
            List<List<RoomInfo>> RoomList = new List<List<RoomInfo>>();
            foreach (List<DeptInfo> DeptLlist in GroupHouseDeptList)
            {
                var RoominfoList = DeptLlist.Select(o => new RoomInfo()
                {
                    address = o.Name,
                    out_room_id = o.Id.Value.ToString(),
                    building = (o.Name.Split('-'))[0],
                    unit = (o.Name.Split('-'))[1],
                    room = ((o.Name.Split('-'))[2].Length==0?0+(o.Name.Split('-'))[2]: (o.Name.Split('-'))[2])+ ((o.Name.Split('-'))[3].Length == 0 ? 0 + (o.Name.Split('-'))[3] : (o.Name.Split('-'))[3])+"室"  // (o.Name.Split('-'))[2] + (o.Name.Split('-'))[3] + "室"
                }).ToList();
                RoomList.Add(RoominfoList);
            }


            var re = await UpLoadAlipayRoomAsync(RoomList, AlipayCommunity.AlipayCommunityId, AppAuthToken, ComDeptId, OperatorName, OperatorId);




            return new ResultModel() { IsSuccess = false, Msg = "提交成功" };
        }
        private async Task<ResultModel> UpLoadAlipayRoomAsync(List<List<RoomInfo>> UpLoadlist, string AlipayCommunityId, string APPAuthToken, int? ComDeptId, string OperatorName, int OperatorId)
        {


            foreach (var list in UpLoadlist)
            {

                var BatchCode = BillCommonService.Instance.GetSerialNumber(prefix);

                UploadRoom uploadRoom = new UploadRoom()
                {
                    batch_id = BatchCode,
                    community_id = AlipayCommunityId,
                    room_info_set = list.ToArray()
                };

                var c = await Task.FromResult(AlipayRoomUploadService.Instance.RoomInfoUploadAsync(uploadRoom, APPAuthToken));

                if (c.Result.isSuccess || c.Result.code == SuccessCode)
                {
                    JObject obj = JsonHelper.JsonDeserializeByNewtonsoft(c.Result.data) as JObject;
                    var returnCount = obj["alipay_eco_cplife_roominfo_upload_response"]["room_info_set"].Count();
                    var NowDATE = DateTime.Now;
                    for (int i = 0; i <= returnCount - 1; i++)
                    {
                        var RoomInfoResponse = obj["alipay_eco_cplife_roominfo_upload_response"]["room_info_set"][i];
                        var out_room_id = (string)RoomInfoResponse["out_room_id"];
                        var room_id = (string)RoomInfoResponse["room_id"];
                        AlipayRoom alipayroom = new AlipayRoom()
                        {
                            BatchCode = BatchCode,
                            AlipayRoomId = room_id,
                            ComDeptId = ComDeptId,
                            HouseDeptId = Convert.ToInt32(out_room_id),
                            IsDel = false,
                            HouseNumber = list.Where(o => o.out_room_id == out_room_id).FirstOrDefault().address,
                            CreateTime = NowDATE,
                            UpdateTime = NowDATE,
                            OperatorId = OperatorId.ToString(),
                            OperatorName = OperatorName,
                            UpdateOperatorId = OperatorId.ToString(),
                            UpdateOperatorName = OperatorName
                        };
                        AlipayRoomDomainService _AlipayRoomDomainService = new AlipayRoomDomainService();
                        _AlipayRoomDomainService.InsertAlipayRoom(alipayroom);

                    }

                    // return new ResultModel() { IsSuccess = true, Msg = "已提交支付宝" };


                }
                else
                {
                    return new ResultModel() { IsSuccess = false, Msg = "错误：" + c.Result.msg };
                }

                


            }
            return new ResultModel() { IsSuccess = true, Msg = "已提交支付宝进行处理" };



        }
        public ResultModel DeleteAlipayRoom(string Ids, int? ComDeptId)
        {

            AlipayRoomDomainService _AlipayRoomDomainService = new AlipayRoomDomainService();
            var idsList = Ids.Split(',').ToList();
            var idIntlist = idsList.Select(o => Convert.ToInt32(o)).ToList();
            Condition<AlipayRoom> condition = new Condition<AlipayRoom>(c => c.IsDel == false && idIntlist.Contains(c.Id.Value));
            var DeleteList = _AlipayRoomDomainService.GetAlipayRoomList(condition.ExpressionBody).Select(o => o.HouseDeptId.ToString()).ToList();
            DeleteRoomInfo deleteroominfo = new DeleteRoomInfo();
            var AlipayCommunityId = this.GetAlipayCommunityId(ComDeptId).AlipayCommunityId;
            var AppAuthToken = GetAPPAuthTokenByComDeptId(ComDeptId);
            var BatchCode = BillCommonService.Instance.GetSerialNumber(Deleteprefix);
            DeleteRoomInfo deleteRoomInfo = new DeleteRoomInfo()
            {
                batch_id = BatchCode,
                community_id = AlipayCommunityId,
                out_room_id_set = DeleteList.ToArray()

            };
            var ResultResponse = AlipayRoomUploadService.Instance.RoomInfoDelete(deleteRoomInfo, AppAuthToken);
            JObject obj = JsonHelper.JsonDeserializeByNewtonsoft(ResultResponse.data) as JObject;
            var ResultCode = (string)obj["alipay_eco_cplife_roominfo_delete_response"]["code"];

            if (ResultCode == "10000")
            {
                idIntlist.ForEach(o =>
                {
                    var Alipayroom = _AlipayRoomDomainService.GetAlipayRoomByKey(o);
                    Alipayroom.IsDel = true;
                    _AlipayRoomDomainService.UpdateAlipayRoom(Alipayroom);
                });

                return new ResultModel() { IsSuccess = true, Msg = "删除成功" };
            }

            return new ResultModel() { IsSuccess = false, Msg = "删除失败" };
        }

        public void SynchronizationRoomInfo(int? ComDeptId, string OperatorName, int OperatorId)
        {
            Task.Run(() =>
            {
                //第一步、获取总数计算取出几次数据
                var AlipayCommunity = this.GetAlipayCommunityId(ComDeptId);
                var AppAuthToken = this.GetAPPAuthTokenByComDeptId(ComDeptId);

                SeachRoomInfo seachRoomInfo = new SeachRoomInfo()
                {
                    community_id = AlipayCommunity.AlipayCommunityId,
                    page_num = 1,
                    page_size = 1,
                };

                var re = AlipayRoomUploadService.Instance.RoominfoQuery(seachRoomInfo, AppAuthToken);
                JObject obj = JsonHelper.JsonDeserializeByNewtonsoft(re.data) as JObject;
                var ResultCode = (string)obj["alipay_eco_cplife_roominfo_query_response"]["code"];
                if (ResultCode == "10000")
                {
                    //获取所有数据
                    var total_room_number = (int)obj["alipay_eco_cplife_roominfo_query_response"]["total_room_number"];
                    if (total_room_number > SynchronizationPageSize)
                    {//分批处理
                        var Remainder = total_room_number % SynchronizationPageSize;//余数
                        var integerModel = (int)(total_room_number / SynchronizationPageSize);
                        if (Remainder > 0)
                        {
                            integerModel++;
                        }
                        HandleReturnData(AlipayCommunity.AlipayCommunityId, AppAuthToken, integerModel, ComDeptId, OperatorName, OperatorId);



                    }
                    else
                    {//一次性处理
                        HandleReturnData(AlipayCommunity.AlipayCommunityId, AppAuthToken, 1, ComDeptId, OperatorName, OperatorId);
                    }


                }
                
            });
        }

        private void HandleReturnData(string AlipayCommunityId,string AppAuthToken,int PageNumberTotal,int? ComDeptId, string OperatorName, int OperatorId)
        {
            var BatchCode = BillCommonService.Instance.GetSerialNumber(Deleteprefix);
            for (int i=1;i<= PageNumberTotal;i++)
            {
                SeachRoomInfo seachRoomInfo = new SeachRoomInfo()
                {
                    community_id = AlipayCommunityId,
                    page_num = i,
                    page_size = SynchronizationPageSize,
                };
                var result = AlipayRoomUploadService.Instance.RoominfoQuery(seachRoomInfo, AppAuthToken);
                JObject obj = JsonHelper.JsonDeserializeByNewtonsoft(result.data) as JObject;
                var total_room_number = obj["alipay_eco_cplife_roominfo_query_response"]["room_info"].ToString();
                var room_infoList=  JsonHelper.JsonDeserializeByNewtonsoft <List<room_info>>(total_room_number);

                GetNonExistentRoom(ComDeptId, room_infoList,BatchCode, OperatorName, OperatorId);

            }
           
        }

        private void GetNonExistentRoom(int? ComDeptId, List<room_info> room_infoList,string BatchCode, string OperatorName, int OperatorId)
        {

          
            var  AlipayHouseDeptId= room_infoList.Select(o => Convert.ToInt32(o.out_room_id)).ToList();
            AlipayRoomDomainService _AlipayRoomDomainService = new AlipayRoomDomainService();
            var Idstr= _AlipayRoomDomainService.GetSynchronizationRoomInfoID(ComDeptId, AlipayHouseDeptId);
            var returnlist=  room_infoList.Where(o => Idstr.Contains(o.out_room_id)).ToList();
            var SaveList = returnlist.Select(o => new AlipayRoom
            {
                AlipayRoomId = o.room_id,
                BatchCode = BatchCode,
                ComDeptId = ComDeptId,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                OperatorId = OperatorId.ToString(),
                UpdateOperatorId = OperatorId.ToString(),
                OperatorName = OperatorName,
                UpdateOperatorName = OperatorName,
                HouseDeptId = Convert.ToInt32(o.out_room_id),
                HouseNumber = o.address,
                IsDel = false 


            }).ToList();
            _AlipayRoomDomainService.InsertAlipayRoomBat(SaveList);

        }




        #endregion

        #region 账单
        /// <summary>
        /// 账单列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TemplateModel> GetALipayChargeBillTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "HouseNumber", ColumnDesc = "房间号", Seq = i++},
                new TemplateColumn(){ ColumnName = "CostType", ColumnDesc = "收费项目", Seq = i++},
                new TemplateColumn(){ ColumnName = "ChargeBillAmount", ColumnDesc = "账单金额", Seq = i++},
                new TemplateColumn(){ ColumnName = "BillAmount", ColumnDesc = "上传金额", Seq = i++},
                new TemplateColumn(){ ColumnName = "BatchCode", ColumnDesc = "批次号", Seq = i++},
                new TemplateColumn(){ ColumnName = "AlipayChargeBillStatus", ColumnDesc = "付款状态", Seq = i++, DictId = PropertyEnumType.AlipayChargeBillStatus.ToString() }

        };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(AlipayChargeBillDTO), showColumns);
            return template;
        }
        /// <summary>
        /// 选取账单列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TemplateModel> GetALipayChargeBillViewTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "HouseDoorNo", ColumnDesc = "房间号", Seq = i++},
                new TemplateColumn(){ ColumnName = "ChargeSubjectName", ColumnDesc = "收费项目", Seq = i++},
                new TemplateColumn(){ ColumnName = "BillAmount", ColumnDesc = "账单金额", Seq = i++},
                new TemplateColumn(){ ColumnName = "ReceivedAmount", ColumnDesc = "已缴金额", Seq = i++},
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ChargBillDTO), showColumns);
            return template;
        }

        public List<AlipayChargeBillDTO> GetAlipayChargeBillList(AlipayPropertySearchDTO search, out int totalcount)
        {
            string expressions = "CreateTime desc";
            AlipayChargeBillDomainService _AlipayChargeBillDomainService = new AlipayChargeBillDomainService();
            Condition<AlipayChargeBill> condition = new Condition<AlipayChargeBill>(o => o.ComDeptId == search.ComDeptId && o.IsDel == false);
           var AlipayChargeBillList= _AlipayChargeBillDomainService.Paging(search.PageIndex, search.PageSize, condition.ExpressionBody, expressions, out totalcount);

            return AlipayChargeBillMappers.ChangeAlipayChargeBillToDTOs(AlipayChargeBillList).ToList();

        }



        public List<ChargBillDTO> GetAlipayChargeBillViewList(AlipayPropertySearchDTO search,out int totalcount)
        {
            Condition<AlipayRoom> condition_ar = new Condition<AlipayRoom>(o => o.ComDeptId == search.ComDeptId && o.IsDel == false);//该小区存在的房间
            Condition<AlipayChargeBill> condition_ac = new Condition<AlipayChargeBill>(o => o.IsDel == false && o.ComDeptId == search.ComDeptId);//该小区已经存在的支付宝对接账单
            Condition<ChargBill> condition_cb = new Condition<ChargBill>(o => o.Status == (int)BillStatusEnum.NoPayment && o.IsDel == false && o.ComDeptId == search.ComDeptId);//该小区的账单
            if (search.ChargeSubjectId != null && search.ChargeSubjectId > 0)
            {
                condition_cb = condition_cb & new Condition<ChargBill>(o => o.ChargeSubjectId == search.ChargeSubjectId);
            }
            AlipayChargeBillDomainService _AlipayChargeBillDomainService = new AlipayChargeBillDomainService();
           
            return  AlipayService.Instance.GetAlipayUpLoadChargeBillList(condition_cb.ExpressionBody, condition_ar.ExpressionBody, condition_ac.ExpressionBody, search.PageSize,search.PageIndex, out totalcount);

        }

        public ResultModel SaveAlipayChargeBillUpload(List<string> ChargeBillIdList, int? ComDeptId, string OperatorName, int OperatorId)
        {
            
            Condition<ChargBill> condition = new Condition<ChargBill>(o=> ChargeBillIdList.Contains(o.Id));
            ChargBillDomainService _ChargBillDomainService = new ChargBillDomainService();
            var ChargeBillList= _ChargBillDomainService.GetChargBillAll(condition.ExpressionBody).ToList();
            var AlpayCommunityId = GetAlipayCommunityId(ComDeptId);
            var AppAuthToken = GetAPPAuthTokenByComDeptId(ComDeptId);
            return AlipayService.Instance.SaveUploadAlipayChargeBill(ChargeBillList, AlpayCommunityId.AlipayCommunityId, AppAuthToken, ComDeptId, OperatorId.ToString(), OperatorName);
        }

        public ResultModel DeleteAlipayChargeBill(List<string> Ids, int? ComDeptId)
        {
            List<int?> AlipayChargeBill = new List<int?>();

            Ids.ForEach(o =>
            {
                var id = Convert.ToInt32(o);
                AlipayChargeBill.Add(id);

            });
            var AlpayCommunityId = GetAlipayCommunityId(ComDeptId);
            var AppAuthToken = GetAPPAuthTokenByComDeptId(ComDeptId);
            return AlipayService.Instance.DeleteAlipayChargeBill(AlipayChargeBill, AlpayCommunityId.AlipayCommunityId, AppAuthToken);

             
        }

        #endregion




        /// <summary>
        /// 获取TOKEN
        /// </summary>
        /// <param name="ProDeptId"></param>
        /// <returns></returns>
        private string GetAPPAuthToken(int? ProDeptId)
        {
            AlipayAPPAuthTokenAppService _AlipayAPPAuthTokenAppService = new AlipayAPPAuthTokenAppService();
            var AlipayAPPAuthToken = _AlipayAPPAuthTokenAppService.GetAlipayAPPAuthTokenDTOByProDepetId(ProDeptId.Value);
            return AlipayAPPAuthToken.app_auth_token;
        }

        private string GetAPPAuthTokenByComDeptId(int? ComDeptId)
        {
            AlipayCommunityDomainService _AlipayCommunityDomainService = new AlipayCommunityDomainService();
            Condition<AlipayCommunity> condition = new Condition<AlipayCommunity>(o => o.ComDeptId == ComDeptId && o.IsDel == false);
            var AlipayCommunity= _AlipayCommunityDomainService.GetAlipayCommunityList(condition.ExpressionBody).FirstOrDefault();
            return GetAPPAuthToken(AlipayCommunity.ProDeptId);


        }

        private AlipayCommunity GetAlipayCommunityId(int? ComDeptId)
        {
            AlipayCommunityDomainService _AlipayCommunityDomainService = new AlipayCommunityDomainService();
            Condition<AlipayCommunity> condition = new Condition<AlipayCommunity>(o => o.ComDeptId == ComDeptId && o.IsDel == false);
            var AlipayCommunityModelList = _AlipayCommunityDomainService.GetAlipayCommunityList(condition.ExpressionBody);
            return AlipayCommunityModelList.FirstOrDefault();
        }
        






    }
    
}
 