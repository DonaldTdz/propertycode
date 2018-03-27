using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using Microsoft.Practices.Unity;
using System.Linq.Expressions;
using YK.PropertyMgr.ApplicationDTO;
using PropertyAlipay.Entity.model;
using YK.BackgroundMgr.DomainInterface;
using PropertyAlipay.Service.Services;
using Newtonsoft.Json.Linq;
using YK.PropertyMgr.ApplicationDTO.Enums;

namespace YK.PropertyMgr.CompositeDomainService
{
   public class AlipayService:IAlipayService
    {
        #region 单例

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static AlipayService Instance { get { return SingletonInstance; } }
        private static readonly AlipayService SingletonInstance = new AlipayService();

        #endregion

        #region 支付宝对接操作
        //上传前缀
        private const string Billprefix = "BAP";
        private const string AlipayBillUploadResponse = "alipay_eco_cplife_bill_batch_upload_response";
        private const string AlipayBillDeleteResponse = "alipay_eco_cplife_bill_delete_response";

        public List<ChargBillDTO> GetAlipayUpLoadChargeBillList(Expression<Func<ChargBill, bool>> predicate_ChargeBill, Expression<Func<AlipayRoom, bool>> predicate_AlipayRoom, Expression<Func<AlipayChargeBill, bool>> predicate_AlipayChargeBill, int PageSize, int PageIndex, out int totalcount)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
   
                //以有账单
                var AlipayChargeBillList = propertyMgrUnitOfWork.AlipayChargeBillRepository.GetAll().Where(predicate_AlipayChargeBill);
                //只取上传房间的账单
                var query = (from c in propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(predicate_ChargeBill)
                             join a in propertyMgrUnitOfWork.AlipayRoomRepository.GetAll().Where(predicate_AlipayRoom) on c.HouseDeptId equals a.HouseDeptId
                             join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll() on c.ChargeSubjectId equals s.Id
                             where !AlipayChargeBillList.Any(o => o.ChargeBillId == c.Id)
                             select new ChargBillDTO
                             {
                                 Id=c.Id,
                                 HouseDoorNo = c.HouseDoorNo,
                                 ChargeSubjectName = s.Name,
                                 BillAmount = c.BillAmount,
                                 ReceivedAmount = c.ReceivedAmount
                             });
                totalcount = query.Count();

                return query.OrderBy(o => o.HouseDoorNo).Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            }     

        }

        #region 账单保存
        /// <summary>
        /// 提交账单给支付宝
        /// </summary>
        /// <param name="SaveChargBillList"></param>
        /// <param name="AlipayCommunityId"></param>
        /// <param name="AppAuthToken"></param>
        /// <returns></returns>
        public ResultModel SaveUploadAlipayChargeBill(List<ChargBill> SaveChargBillList, string AlipayCommunityId, string AppAuthToken,int? ComDeptId,string OperatorId,string OperatorName)
        {
            try
            {

                var upload = GenerateUploadBill(SaveChargBillList, AlipayCommunityId);
                return UploadAlipay(upload, AppAuthToken, SaveChargBillList, ComDeptId, OperatorId, OperatorName);
            }
            catch (Exception ex)
            {
                return new ResultModel() { IsSuccess = false, Msg = ex.Message };
            }
        }
        /// <summary>
        /// 生成上传数据
        /// </summary>
        /// <param name="SaveChargBillList"></param>
        /// <param name="AlipayCommunityId"></param>
        /// <returns></returns>
        private UploadBill GenerateUploadBill(List<ChargBill> SaveChargBillList, string AlipayCommunityId)
        {
            var HouseDeptIdList = SaveChargBillList.Select(o => o.HouseDeptId).ToList();
            var OwnerDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptAndOwnerListByIdArr(HouseDeptIdList).ToList();
            OwnerDeptList.ForEach(o => {
                o.OwnerUserName = RemoveSensitivityName(o.OwnerUserName);
            });
            var BatchCode = BillCommonService.Instance.GetSerialNumber(Billprefix);
            //生成上传对象
            UploadBill upload = new UploadBill();
            var BillList = (from o in SaveChargBillList
                            join d in OwnerDeptList on o.HouseDeptId equals d.Id
                            select new Bill
                            {
                                out_room_id = o.HouseDeptId.ToString(),
                                bill_entry_id = o.Id,
                                room_address = o.HouseDoorNo,
                                cost_type = o.ChargeSubject.Name,
                                bill_entry_amount = (Convert.ToDecimal(o.BillAmount) - Convert.ToDecimal(o.ReceivedAmount.Value)).ToString("0.00"),
                                acct_period = o.BeginDate.Value.Year + "年" + o.BeginDate.Value.Month + "月",
                                release_day = o.BeginDate.Value.ToString("yyyyMMdd"),
                                deadline = o.EndDate.Value.AddDays(1).ToString("yyyyMMdd"),
                                remark_str = "详细账单期为：" + o.BeginDate.Value.ToString("yyyy-MM-dd") + "至" + o.EndDate.Value.ToString("yyyy-MM-dd") + "客户姓名:" + d.OwnerUserName
                            }).ToList();
            if (BillList.Count() == 0)
                throw new Exception("上传房间里未有业主");
            upload.batch_id = BatchCode;
            upload.community_id = AlipayCommunityId;
            upload.bill_set = BillList.ToArray();
            return upload;
        }

        /// <summary>
        /// 传输给支付宝
        /// </summary>
        /// <param name="upload"></param>
        /// <param name="AppAuthToken"></param>
        /// <returns></returns>
        private ResultModel UploadAlipay(UploadBill upload, string AppAuthToken, List<ChargBill> SaveChargBillList, int? ComDeptId, string OperatorId, string OperatorName)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                AlipayChargeBillSynchronizer alipayChargeBillSynchronizer = new AlipayChargeBillSynchronizer()
                {
                    Id = Guid.NewGuid().ToString(),
                    BatchCode = upload.batch_id,
                    ComDeptId = ComDeptId,
                    CountNumber = 0,
                    IsDel = false,
                    IsFinish = false,
                    CreateTime = DateTime.Now
                };
                var alipayChargeBillSynchronizerDetail = SaveChargBillList.Select(o => new AlipayChargeBillSynchronizerDetail
                {
                    AlipayChargeBillSynchronizerId = alipayChargeBillSynchronizer.Id,
                    ChargeBillId = o.Id,
                    CreateTime = DateTime.Now,
                    IsDel = false
                }).ToList();

                var ResultModel = AlipayBillService.Instance.BillUpload(upload, AppAuthToken);

                // 成功返回后同步对象存入数据库。
                propertyMgrUnitOfWork.AlipayChargeBillSynchronizerRepository.Add(alipayChargeBillSynchronizer);
                propertyMgrUnitOfWork.AlipayChargeBillSynchronizerDetailRepository.AddRange(alipayChargeBillSynchronizerDetail);
                

                //处理返回值
                return HandleAlipayBillResponse(ResultModel.data, AlipayBillUploadResponse, upload.batch_id, SaveChargBillList,OperatorId,OperatorName, propertyMgrUnitOfWork);
            }
          
        }
        /// <summary>
        /// 解析支付宝的参数
        /// </summary>
        /// <param name="jsontsr"></param>
        /// <returns></returns>
        private ResultModel HandleAlipayBillResponse(string jsontsr,string ResponseName, string BatchCode, List<ChargBill> SaveChargBillList, string OperatorId, string OperatorName,IPropertyMgrUnitOfWork propertyMgrUnitOfWork)
        {
            JObject obj = JsonHelper.JsonDeserializeByNewtonsoft(jsontsr) as JObject;
            var ResponseCode = (string)obj[ResponseName]["code"];
            switch(ResponseCode) 
            {
                case "10000":
                    //成功
                    propertyMgrUnitOfWork.Commit();
                    return SuccessAlipayBillUpload((string)obj[ResponseName]["batch_id"], SaveChargBillList, OperatorId,OperatorName);
                case "20000":
                    //处理网络
                    propertyMgrUnitOfWork.Commit();
                    return new ResultModel() { IsSuccess = false, Msg = "支付宝网络忙，请稍2分钟后点击同步查看数据，如遇数据未同步请联系管理员" };
                default:
                   
                    return new ResultModel() { IsSuccess = true, Msg = "支付宝保存失败，失败代码："+ (string)obj[ResponseName]["code"]+"业务失败原因："+ (string)obj[ResponseName]["sub_msg"] };

            }
        }

        private ResultModel SuccessAlipayBillUpload(string BatchCode, List<ChargBill> SaveChargBillList, string OperatorId, string OperatorName)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //生成AlipayChargeBill
                var SaveAlipayChargeBillList = SaveChargBillList.Select(o => new AlipayChargeBill
                {
                    ComDeptId = o.ComDeptId,
                    OperatorName = OperatorName,
                    OperatorId = OperatorId,
                    UpdateOperatorId = OperatorId,
                    UpdateOperatorName = OperatorName,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    IsDel = false,
                    BatchCode = BatchCode,
                    HouseNumber = o.HouseDoorNo,
                    ChargeBillAmount = o.BillAmount.Value.ToString("0.00"),
                    BillAmount = (o.BillAmount - o.ReceivedAmount).Value.ToString(),
                    ChargeBillId = o.Id,
                    CostType = o.ChargeSubject.Name,
                    AlipayChargeBillStatus = (int)AlipayChargeBillStatusEnum.WAIT_PAYMENT,
                    Acct_Period = o.BeginDate.Value.Year + "年" + o.BeginDate.Value.Month + "月"
                }).ToList();

                //取出同步账单数据
  
                var alipayChargeBillSynchronizer =  propertyMgrUnitOfWork.AlipayChargeBillSynchronizerRepository.GetAll().Where(o => o.BatchCode == BatchCode).FirstOrDefault();

                alipayChargeBillSynchronizer.IsFinish = true;

                propertyMgrUnitOfWork.AlipayChargeBillSynchronizerRepository.Update(alipayChargeBillSynchronizer);
                propertyMgrUnitOfWork.AlipayChargeBillRepository.AddRange(SaveAlipayChargeBillList);
                propertyMgrUnitOfWork.Commit();
                ResultModel resultModel = new ResultModel()
                {
                    IsSuccess = true,
                    Msg = "保存成功"
                };


                return resultModel;
            }
        }


        #endregion

        #region 账单删除
        public ResultModel DeleteAlipayChargeBill(List<int?> Ids, string AlipayCommunityId,string AppAuthToken)
        {
 
 
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {

              var DeleteBillId = propertyMgrUnitOfWork.AlipayChargeBillRepository.GetAll().Where(o => Ids.Contains(o.Id)).Select(o=>o.ChargeBillId).ToArray();

                var DeleteBillList = propertyMgrUnitOfWork.AlipayChargeBillRepository.GetAll().Where(o => Ids.Contains(o.Id)).ToList();
                DeleteBill deletebill = new DeleteBill()
                {
                    community_id = AlipayCommunityId,
                    bill_entry_id_list = DeleteBillId
                };
                var reponse=  AlipayBillService.Instance.BillDelete(deletebill, AppAuthToken);
                JObject obj = JsonHelper.JsonDeserializeByNewtonsoft(reponse.data) as JObject;
                var ResponseCode = (string)obj[AlipayBillDeleteResponse]["code"];
                switch (ResponseCode)
                {
                    case "10000":
                        //成功
                        return SuccessAlipayBillDelte(obj[AlipayBillDeleteResponse]["alive_bill_entry_list"], DeleteBillList);
                    case "20000":
                        //处理网络
                        return new ResultModel() { IsSuccess = false, Msg = "支付宝网络忙，请稍2分钟后点击同步查看数据，如遇数据未同步请联系管理员" };
                    default:
                        return new ResultModel() { IsSuccess = true, Msg = "支付宝保存失败，失败代码：" + (string)obj[AlipayBillDeleteResponse]["code"] + "业务失败原因：" + (string)obj[AlipayBillDeleteResponse]["sub_msg"] };

                }



            }

        }

        public ResultModel SuccessAlipayBillDelte(JToken alive_bill, List<AlipayChargeBill> DeleteBillList)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var DeleteList = new List<AlipayChargeBill>();
                var ErrorStr = "已提交支付宝删除\n";
                if (alive_bill != null)
                {
                    for (int i = 0; i < alive_bill.Count(); i++)
                    {
                        var aliveInfoResponse = alive_bill[i];
                        var bill_entry_id = (string)aliveInfoResponse["bill_entry_id"];
                        var status = (string)aliveInfoResponse["status"];
                        if (DeleteBillList.Any(o => o.ChargeBillId == bill_entry_id))
                        {
                            ErrorStr += "  账单：" + bill_entry_id + "无法被删除,原因：" + RetrunAlipayBillDelteErrorstr(status) + "\n";

                        }
                        else
                        {
                            DeleteList.Add(DeleteBillList.Where(o => o.ChargeBillId == bill_entry_id).First());
                        }

                    }
                }
                else
                {
                    DeleteList = DeleteBillList;
                }

                //删除
                DeleteList.ForEach(o => o.IsDel = true);
                propertyMgrUnitOfWork.AlipayChargeBillRepository.UpdateRange(DeleteList);
                propertyMgrUnitOfWork.Commit();
                return new ResultModel() { IsSuccess = true, Msg = ErrorStr };

            }


        }


        private string RetrunAlipayBillDelteErrorstr(string Status)
        {
            switch (Status)
            {
                case "FINISH_PAYMENT":
                    return "用户完成支付和销账";
                case "UNDER_PAYMENT ":
                    return "账单锁定待用户完成支付";
                default:
                    return string.Empty;
                    
            }
        }
        #endregion

        #region 账单同步
        public void SynchronizeAlipayChargeBill(int? ComDeptId)
        {
            //首先取出所有未删除未同步的订单
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var SynBillList = propertyMgrUnitOfWork.AlipayChargeBillSynchronizerRepository.GetAll().Where(o => o.IsFinish == false && o.IsDel == false).ToList();
                if(SynBillList.Count>0)
                {
                    //处理


                }
            }

           //进行同步
         }

        private void HandleSynchronizeBill(IList<AlipayChargeBillSynchronizer> list)
        {

        }


        #endregion

        #endregion

        #region 名称脱敏
        /// <summary>
        /// 名称脱敏
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public string RemoveSensitivityName(string Name)
        {
            if (string.IsNullOrEmpty(Name))
            {
                return string.Empty;
            }
            else
            {
                var len = Name.Length;
                return "**" + Name.Substring(len - 1);

            }
        }
        #endregion


         
    }
}
