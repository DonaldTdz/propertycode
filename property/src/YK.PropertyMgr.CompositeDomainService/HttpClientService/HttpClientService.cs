using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using YK.BackgroundMgr.Crosscuting;
//using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.CompositeDomainService.Model;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.DomainEntity;
using YK.SysPay.Entity;
using YK.SysPay.SDK.Command;
using YK.SysPay.SDK.Entity;

namespace YK.PropertyMgr.CompositeDomainService
{
    public class HttpClientService
    {
        private HttpClient m_HttpClient;

        #region 调用基础服务

        protected HttpClient YCFrameworkHttpClient
        {
            get
            {
                if (m_HttpClient == null)
                {
                    m_HttpClient = new HttpClient();
                    m_HttpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["BasicUrl"]);//new Uri("http://172.16.20.33/"); 
                    m_HttpClient.DefaultRequestHeaders.Accept.Clear();
                    m_HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    m_HttpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));
                }
                return m_HttpClient;
            }
        }

        public bool SendPushWithJson(string[] phone, MessageInfo entity, string CommunityKey)
        {
            //如果跳转URL为空 为纯文本通知
            //if (string.IsNullOrEmpty(entity.ActionUrl))
            //{
            //    entity.TemplateType = "Txt";
            //}
            entity.IconUrl = HttpUtility.UrlEncode(entity.IconUrl, Encoding.UTF8);
            entity.ActionUrl = HttpUtility.UrlEncode(entity.ActionUrl, Encoding.UTF8);

            var obj = new
            {
                platform = "all",
                audience = new { alias = phone },
                notification = new
                {
                    android = new { alert = entity.Content, title = entity.Title, builder_id = 1, extras = new { newsid = 4001 } },
                    ios = new { alert = entity.Content, sound = "default", badge = +1, extras = new { newsid = 4001 } }
                },
                message = new { msg_content = entity },
                options = new { time_to_live = 60, apns_production = false }

            };
            string json = JsonHelper.JsonSerializerByNewtonsoft(obj);
            HttpResponseMessage response = PostApiResponse(ConfigurationManager.AppSettings["BasicUrl"], "api/JPushManage", "SendPushWithJson?CommunityKey=" + CommunityKey, JObject.Parse(json));
            LogProperty.WriteLoginToFile(json, "JPushMsg", FileLogType.Info);
            return response.IsSuccessStatusCode; // 调用结果
        }

        public bool SendPushWithJson(string phone, TxtMsg entity, string CommunityKey)
        {
            //entity.TemplateType = "Txt";
            var obj = new
            {
                platform = "all",
                audience = new { alias = new string[] { phone } },
                notification = new
                {
                    android = new { alert = entity.Content, title = entity.Title, builder_id = 1, extras = new { newsid = 4001 } },
                    ios = new { alert = entity.Content, sound = "default", badge = +1, extras = new { newsid = 4001 } }
                },
                message = new { msg_content = entity },
                options = new { time_to_live = 60, apns_production = false }

            };
            string json = JsonHelper.JsonSerializerByNewtonsoft(obj);
            HttpResponseMessage response = PostApiResponse(ConfigurationManager.AppSettings["BasicUrl"], "api/JPushManage", "SendPushWithJson?CommunityKey=" + CommunityKey, JObject.Parse(json));
            LogProperty.WriteLoginToFile(json, "JPushMsg", FileLogType.Info);
            return response.IsSuccessStatusCode; // 调用结果
        }

        public bool SMSSend(string phone, string title, string content)
        {
            string param = string.Format("?Phone={0}&Title={1}&SmsContent={2}", phone, title, content);
            HttpResponseMessage response = GetApiResponse(ConfigurationManager.AppSettings["BasicUrl"], "SMSManage", "SmsSend", param);
            return response.IsSuccessStatusCode;
        }

        public bool SmsSendWithAccount(SmsEntityModel model)
        {
            //签名固定为此格式
            model.Sign = DataEncrypt.MD5(model.Phones + "|" + model.Content + "|" + model.Title + "|" + model.SmsAccountId + "|" + model.IsPay);
            string strModel = JsonHelper.JsonSerializerByNewtonsoft(model);
            HttpResponseMessage response = PostApiResponse(ConfigurationManager.AppSettings["BasicUrl"], "api/SMSManage", "SmsSendWithAccount", JObject.Parse(strModel));
            LogProperty.WriteLoginToFile(strModel, "SmsSendWithAccount", FileLogType.Info);
            return response.IsSuccessStatusCode; // 调用结果
        }

        #region 建立指定的连接POST，返回查询方法
        /// <summary>
        /// 建立指定的连接POST，返回查询方法
        /// </summary>
        /// <param name="baseAddress">连接地址</param>
        /// <param name="action">控制器</param>
        /// <param name="method">方法</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        private static HttpResponseMessage PostApiResponse(string baseAddress, string action, string method, object param)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseAddress);
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = string.Format("{0}/{1}", action, method);
                HttpResponseMessage response = client.PostAsJsonAsync(url, param).Result;

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                else
                {
                    throw new Exception("Error Code: " + response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 建立指定的连接GET，返回查询方法
        /// <summary>
        /// 建立指定的连接GET，返回查询方法
        /// </summary>
        /// <param name="baseAddress">连接地址</param>
        /// <param name="action">控制器</param>
        /// <param name="method">方法</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        private static HttpResponseMessage GetApiResponse(string baseAddress, string action, string method, string param)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("api/" + action + "/" + method + param).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                else
                {
                    throw new Exception("Error Code" +
                     response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 非api
        /// </summary>
        private static HttpResponseMessage GetResponse(string baseAddress, string action, string method, string param)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(action + "/" + method + param).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                else
                {
                    throw new Exception("Error Code" +
                     response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Form请求

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }

        private static string Post(string json, string url, int timeout)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            string result = "";//返回结果
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = timeout * 1000;

                //设置代理服务器
                //WebProxy proxy = new WebProxy();                          //定义一个网关对象
                //proxy.Address = new Uri(WxPayConfig.PROXY_URL);              //网关服务器端口:端口
                //request.Proxy = proxy;

                //设置POST的数据类型和长度
                request.ContentType = "text/json";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(json);
                request.ContentLength = data.Length;

                //是否使用证书
                //if (isUseCert)
                //{
                //    string path = HttpContext.Current.Request.PhysicalApplicationPath;
                //    X509Certificate2 cert = new X509Certificate2(path + WxPayConfig.SSLCERT_PATH, WxPayConfig.SSLCERT_PASSWORD);
                //    request.ClientCertificates.Add(cert);
                //    Log.Debug("WxPayApi", "PostXml used cert");
                //}

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                //Log.Error("HttpService", "Thread - caught ThreadAbortException - resetting.");
                //Log.Error("Exception message: {0}", e.Message);
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                //Log.Error("HttpService", e.ToString());
                //if (e.Status == WebExceptionStatus.ProtocolError)
                //{
                //    Log.Error("HttpService", "StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                //    Log.Error("HttpService", "StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                //}
                throw e;
            }
            catch (Exception e)
            {
                //Log.Error("HttpService", e.ToString());
                throw e;
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        /// <summary>
        /// 模拟页面web form表单提交
        /// </summary>
        /// <param name="url">post URL</param>
        /// <param name="queryString">参数 格式和get参数一致 如：a=avlue&b=bvalue</param>
        /// <returns></returns>
        private static string HttpWebFormPost(string url, string queryString)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentType = "multipart/form-data";
                request.Method = "POST";
                Encoding encoding = Encoding.UTF8;
                byte[] postData = encoding.GetBytes(queryString);
                Stream requestStream = request.GetRequestStreamAsync().Result;
                requestStream.Write(postData, 0, postData.Length);
                requestStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponseAsync().Result;

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                var result = sr.ReadToEnd().Trim();
                sr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //记录异常
                throw ex;
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }

        }

        #endregion

        #endregion

        #region 调用电商服务

        protected HttpClient YCSQHttpClient
        {
            get
            {
                if (m_HttpClient == null)
                {
                    m_HttpClient = new HttpClient();
                    m_HttpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["SQUrl"]);//new Uri("http://172.16.20.33/"); 
                    m_HttpClient.DefaultRequestHeaders.Accept.Clear();
                    m_HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    m_HttpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));
                }
                return m_HttpClient;
            }
        }

        /// <summary>
        /// 更用户优惠券为已使用或解除锁定
        /// </summary>
        /// <param name="userId">用户Id（统一账户ID）</param>
        /// <param name="userCouponId">用户优惠券ID</param>
        /// <param name="type">改变状态(unlock为解锁优惠券，used为变更已使用状态， unused 改为 未使用状态)</param>
        /// <returns>
        /// 名称	类型	备注
        ///IsResult Bool    返回是否成功 true为验证成功 false为验证失败
        ///errCode Int 错误代码 成功时返回0
        ///data    Json 返回空字符串
        ///Msg String  返回错误信息 IsResult为true则返回空
        /// </returns>
        public bool ChangeUserCoupon(string userId, int userCouponId, string type)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["SQUrl"]);
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "CouponService/ChangeUserCoupon";
                var param = new { userId = userId, userCouponId = userCouponId, type = type };
                string json = JsonHelper.JsonSerializerByNewtonsoft(param);
                HttpResponseMessage response = client.PostAsJsonAsync(url, JObject.Parse(json)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var sr = response.Content.ReadAsStringAsync().Result;
                    SQResult result = JsonHelper.JsonDeserializeByNewtonsoft<SQResult>(sr);
                    if (result.IsResult)
                    {
                        LogProperty.WriteLoginToFile(string.Format("调用退优惠券成功：errCode:{0} Msg:{1} userId:{2} userCouponId:{3} IsResult:{4}", result.errCode, result.Msg, userId, userCouponId, result.IsResult), "ChangeUserCoupon", FileLogType.Info);
                        return true;
                    }
                    else
                    {
                        LogProperty.WriteLoginToFile(string.Format("errCode:{0} Msg:{1} userId:{2} userCouponId:{3}", result.errCode, result.Msg, userId, userCouponId), "ChangeUserCoupon", FileLogType.Exception);
                        return false;
                    }
                }
                else
                {
                    LogProperty.WriteLoginToFile(string.Format("StatusCode:{0} RequestMessage:{1} userId:{2} userCouponId:{3}", response.StatusCode, response.RequestMessage, userId, userCouponId), "ChangeUserCoupon", FileLogType.Exception);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("errormsg:{0} userId:{1} userCouponId:{2}", ex.Message, userId, userCouponId), "ChangeUserCoupon", FileLogType.Exception);
                return false;
            }
        }

        public UserCouponpModel GetUserCouponById(string userId, int userCouponId)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["SQUrl"]);
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = "CouponService/GetUserCouponById";
                var param = new { userId = userId, userCouponId = userCouponId };
                string json = JsonHelper.JsonSerializerByNewtonsoft(param);
                HttpResponseMessage response = client.PostAsJsonAsync(url, JObject.Parse(json)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var sr = response.Content.ReadAsStringAsync().Result;
                    SQResult result = JsonHelper.JsonDeserializeByNewtonsoft<SQResult>(sr);
                    if (result.IsResult)
                    {
                        UserCouponpModel model = JsonHelper.JsonDeserializeByNewtonsoft<UserCouponpModel>(result.data.ToString());
                        return model;
                    }
                    else
                    {
                        LogProperty.WriteLoginToFile(string.Format("errCode:{0} Msg:{1} userId:{2} userCouponId:{3}", result.errCode, result.Msg, userId, userCouponId), "GetUserCouponById", FileLogType.Exception);
                        return null;
                    }
                }
                else
                {
                    LogProperty.WriteLoginToFile(string.Format("StatusCode:{0} RequestMessage:{1} userId:{2} userCouponId:{3}", response.StatusCode, response.RequestMessage, userId, userCouponId), "GetUserCouponById", FileLogType.Exception);
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("errormsg:{0} userId:{1} userCouponId:{2}", ex.Message, userId, userCouponId), "GetUserCouponById", FileLogType.Exception);
                return null;
            }
        }

        #endregion

        #region 接口地址
        private static string PublicApiWebUrl = ConfigurationManager.AppSettings["PublicApiWebReference"];
        private static string PublicApiWebZNMSUrl = ConfigurationManager.AppSettings["PublicApiWebZNMSReference"];
        #endregion

        #region 门禁授权接口信息[获取基础组接口]

        #region    门禁授权时获取的房屋、楼栋、单元ID
        /// <summary>
        /// 门禁授权时获取的房屋、楼栋、单元ID
        /// </summary>
        /// <param name="UserOwnerId">用户的ID</param>
        /// <returns></returns>
        public static List<YK.Framework.ApplicationDTO.InterfaceDTO.UserOwnerEntrance> GetUserOwnerEntrances(Guid UserOwnerId)
        {
            try
            {
                string uid = "?UserOwnerId=" + UserOwnerId;
                HttpResponseMessage httpres = GetApiResponse(PublicApiWebUrl, "PropertyManage", "GetUserOwnerEntrances", uid);
                var userOwnerEntrance = httpres.Content.ReadAsAsync<List<YK.Framework.ApplicationDTO.InterfaceDTO.UserOwnerEntrance>>().Result;
                return (List<YK.Framework.ApplicationDTO.InterfaceDTO.UserOwnerEntrance>)userOwnerEntrance;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region 楼栋获取用户
        public static List<YK.Framework.ApplicationDTO.SQUserOwnerInfo> GetUserOwnerInfoByBuildingDeptId(int BuildingDeptId)
        {
            try
            {
                string SeachCode = "?BuildingDeptId=" + BuildingDeptId;
                HttpResponseMessage httpres = GetApiResponse(PublicApiWebUrl, "CommunityManage", "GetUserOwnerInfoByBuildingDeptId", SeachCode);
                var users = httpres.Content.ReadAsAsync<List<YK.Framework.ApplicationDTO.SQUserOwnerInfo>>().Result;
                return users;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 根据小区获取用户信息
        /// <summary>
        /// 根据小区获取用户信息
        /// </summary>
        /// <param name="CommunityDeptId"></param>
        /// <returns></returns>
        public static List<YK.Framework.ApplicationDTO.SQUserOwnerInfo> GetSQUserOwnerInfoByCommunityDeptId(int CommunityDeptId)
        {
            try
            {
                string SeachCode = "?CommunityDeptId=" + CommunityDeptId;
                HttpResponseMessage httpres = GetApiResponse(PublicApiWebUrl, "CommunityManage", "GetSQUserOwnerInfoByCommunityDeptId", SeachCode);
                var users = httpres.Content.ReadAsAsync<List<YK.Framework.ApplicationDTO.SQUserOwnerInfo>>().Result;
                return users;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion



        #endregion

        #region 新增设备信息获取KeyId和DoorId【获取硬件组接口】
        public class ResultAPI
        {
            public bool Result { get; set; }
            public string Msg { get; set; }
            public System.Object Data { get; set; }
        }
        /// <summary>
        /// 获取DOORID和KEYID   OLD_VERSION（新版新增时已经不用获取了）
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="villageName"></param>
        /// <returns></returns>
        public static string GetKeyIdNew(Entrance obj, string villageName)
        {
            try
            {
                try
                {

                    string method = string.Empty;
                    string SeachCode = "?provinceId=" + obj.ProvinceID + "&cityId=" + obj.CityID + "&countyId=" + obj.CountyID + "&address=" + obj.Address + "&cellName=" + villageName + "&cellId=" + obj.VillageID + "&doorId=" + obj.DoorId + "&doorName=" + obj.DoorName;
                    method = "AddLocknewpublic";
                    HttpResponseMessage httpres = GetApiResponse(PublicApiWebZNMSUrl, "PropertyWeb", method, SeachCode);
                    var users = httpres.Content.ReadAsAsync<string>().Result;
                    return users;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string AddDoor(Entrance obj, string villageName)
        {
            try
            {

                string method = string.Empty;
                string SeachCode = "?doorId=" + obj.Id + "&provinceId=" + obj.ProvinceID + "&cityId=" + obj.CityID + "&countyId=" + obj.CountyID + "&address=" + obj.Address + "&cellName=" + villageName + "&cellId=" + obj.VillageID;
                method = "AddDoor";
                HttpResponseMessage httpres = GetApiResponse(PublicApiWebZNMSUrl, "PropertyWeb", method, SeachCode);
                var users = httpres.Content.ReadAsAsync<string>().Result;
                return users;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static string UpdateDoor(int doorId, string name, string address)
        {

            try
            {
                string method = string.Empty;
                string SeachCode = "?doorId=" + doorId + "&name=" + name + "&address=" + address;
                method = "UpdateDoor";
                HttpResponseMessage httpres = GetApiResponse(PublicApiWebZNMSUrl, "PropertyWeb", method, SeachCode);
                var users = httpres.Content.ReadAsAsync<string>().Result;
                return users;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region 统一支付 20170810

        /// <summary>
        /// 获取扫描支付二维码
        /// </summary>
        /// <param name="payOrder">支付订单信息</param>
        /// <returns></returns>
        public static QRCode GetPayQRCodeUrl(ClientPayOrder payOrder)
        {
            RetNumerical numerical = new RetNumerical();
            //流水编码
            numerical.NumericalNumber = SysPay.SDK.Command.StringHelper.GetNumericalNumber(ERequestFromSys.物业收费);
            payOrder.OrderNum = BillCommonService.Instance.GetSerialNumber("CP");
            //订单列表
            numerical.listOrder = new List<RetOrder>() {
                    new  RetOrder(){
                    OrderName = "物业终端订单",
                    OrderNumber = payOrder.OrderNum,
                    Price = payOrder.PayAmount.Value
                    }
            };
            //流水描述
            if (payOrder.PayType == 1)
            {
                numerical.Msg = string.Format("{0} 房屋{1}物业终端自助缴费", payOrder.CommunityName, payOrder.HouseNo);
            }
            else
            {
                numerical.Msg = string.Format("{0} 房屋{1}物业终端自助预存费", payOrder.CommunityName, payOrder.HouseNo);
            }

            var host = ConfigurationManager.AppSettings["PayCallBackHost"];
            //"异常跳转地址";
            numerical.GoUrl = string.Format("{0}/api/PropertyClient/PayCallBack?CallType=1&NumericalNumber={1}", host, numerical.NumericalNumber);
            //"异步回调地址";
            numerical.NotifyUrl = string.Format("{0}/api/PropertyClient/PayCallBack?CallType=2", host);
            //"同步回调地址";
            numerical.CallBackUrl = string.Format("{0}/api/PropertyClient/PayCallBack?CallType=3&NumericalNumber={1}", host, numerical.NumericalNumber);

            //系统来源
            numerical.RequestFromSys = ERequestFromSys.物业收费;
            //用户ID 支持业主Guid和IntId
            numerical.UserID = payOrder.UserId;//"EF682331-AFF3-4C5C-B5F6-01137DFA6F57";
            //认证Key，需向统一支付申请Key
            numerical.Key = ConfigurationManager.AppSettings["PayAuthenticationKey"];
            //支付场景，默认为线上支付
            numerical.PayScenario = EPayScenario.线上支付;

            //线下消费时使用 商户Id
            numerical.MerchantId = "";

            //无用户支付时使用 获取支付配置的Key，逸社区为物业Id
            numerical.PayConfigKey = "";

            //是否返回二维码URL 用于二维码扫码支付
            //默认否:由统一支付生成支付URL并渲染成二维码
            //是:由统一支付生成支付URL并返回支付URL
            //返回支付URL字符串格式:$"{Url.Encode(支付宝支付URL)};{Url.Encode(微信支付URL)}"
            numerical.IsReturnQRUrl = true;
            try
            {
                //记录支付订单信息
                PaymentService.Instance.CreateClientPaymentLog(numerical.NumericalNumber, payOrder);

                //支付信息
                ResponseResult msgResult = RequestSumbit.RequestLog(numerical);
                string _Msg = string.Empty;
                if (msgResult.IsResult)
                {
                    _Msg = msgResult.data.ToString();
                }
                else
                {
                    //处理异常
                    throw new Exception("Error: 支付信息异常" +
                     msgResult.data + " : Message - " + msgResult.Msg);
                }

                //签名，需向统一支付申请Value
                ResponseResult singaResult = RequestSumbit.RetLogSinge(_Msg, ConfigurationManager.AppSettings["PaySingaKey"]);
                string _Singa = string.Empty;
                if (singaResult.IsResult)
                {
                    _Singa = singaResult.data.ToString();
                }
                else
                {
                    //处理异常
                    throw new Exception("Error: 签名异常" +
                    singaResult.data + " : Message - " + singaResult.Msg);
                }

                //get调用方式
                //HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri(ConfigurationManager.AppSettings["PayUrl"]);
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage response = client.GetAsync(string.Format("YKQRPayLog/QRPayLog?Msg={0}&Signa={1}", WebUtility.UrlEncode(_Msg), WebUtility.UrlEncode(_Singa))).Result;

                var queryString = string.Format("Msg={0}&Signa={1}", WebUtility.UrlEncode(_Msg), WebUtility.UrlEncode(_Singa));
                var url = ConfigurationManager.AppSettings["PayUrl"] + "YKQRPayLog/QRPayLogPost";
                var result = HttpWebFormPost(url, queryString);
                QRCode qrcode = new QRCode();
                if (!string.IsNullOrEmpty(result) && result.IndexOf(';') > 0)
                {
                    var arr = result.Split(';');
                    qrcode.NumericalNumber = numerical.NumericalNumber;
                    qrcode.AlipayUrl = WebUtility.UrlDecode(arr[0]);
                    qrcode.WeChatUrl = WebUtility.UrlDecode(arr[1]);
                }
                LogProperty.WriteLoginToFile(string.Format("获取扫描支付URL：result:{0} ", result), "GetPayQRCodeUrl", FileLogType.Info);
                return qrcode;
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("获取扫描支付URL异常：Exception msg:{0} Exception:{1} ", ex.Message, ex), "GetPayQRCodeUrl", FileLogType.Exception);
                return new QRCode();
            }
        }

        /// <summary>
        /// 获取支付流水状态
        /// </summary>
        /// <param name="numericalNumber">支付流水号</param>
        /// <returns>
        /// 待支付 = 0,支付中 = 1,支付成功 = 2,支付失败 = 3,冻结中 = 4,取消 = 5
        /// 若订单号不存在，会返回 订单号'ZNSB20178917240319FG1VOS41'不存在, 请核实
        /// </returns>
        public static string GetNumericalState(string numericalNumber)
        {
            string param = string.Format("?numericalNumber={0}", numericalNumber);
            HttpResponseMessage response = GetResponse(ConfigurationManager.AppSettings["PayUrl"], "YKQRPayLog", "GetNumericalStateByNumericalNumber", param);
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return string.Empty;
        }

        #endregion
    }

    public class SQResult
    {
        public bool IsResult { get; set; }

        public int errCode { get; set; }

        public object data { get; set; }

        public string Msg { get; set; }
    }
}
