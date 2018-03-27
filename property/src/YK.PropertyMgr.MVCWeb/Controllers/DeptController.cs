using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.ApplicationService.Service;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class DeptController : BaseController
    {
        // GET: Dept
        public ActionResult Index()
        {
            DeptListData deptData = new DeptListData();
            deptData.Language = Language;
            return View(deptData);
        }

        public ActionResult DeptContainerPrepayAccountIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.PrepayAccount;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.PrepayAccount;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.PrepayAccount;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.PrepayAccount;
            return View("DeptContainerIndex", deptContainerData);
        }

        public ActionResult DeptChargeSubjectContainerIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.ChargeSubject;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.ChargeSubject;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.ChargeSubject;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.ChargeSubject;
            return View("DeptContainerIndex", deptContainerData);
        }

        public ActionResult DeptSubjectHouseRefContainerIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.SubjectHouseRef;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.SubjectHouseRef;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.SubjectHouseRef;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.SubjectHouseRef;
            return View("DeptContainerIndex", deptContainerData);
        }

        public ActionResult DeptChargeBillContainerIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.ChargeBill;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.ChargeBill;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.ChargeBill;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.ChargeBill;
            return View("DeptContainerIndex", deptContainerData);
        }

        public ActionResult DeptEntranceContainerIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.Entrance;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.Entrance;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.Entrance;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.Entrance;
            return View("DeptContainerIndex", deptContainerData);
        }

        public ActionResult DeptPaymentTaskContainerIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.PaymentTasks;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.PaymentTasks;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.PaymentTasks;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.PaymentTasks;
            return View("DeptContainerIndex", deptContainerData);
        }

        public ActionResult DeptEntranceBindIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.EntranceBind;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.EntranceBind;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.EntranceBind;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.EntranceBind;
            return View("DeptContainerIndex", deptContainerData);
        }

        /// <summary>
        /// 开发商缴费
        /// </summary>
        /// <returns></returns>
        public ActionResult DepDeveloperContainerIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.Developer;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.Developer;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.Developer;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.Developer;
            return View("DeptContainerIndex", deptContainerData);
        }

        /// <summary>
        /// 门禁授权
        /// </summary>
        /// <returns></returns>
        public ActionResult DepEntrancePowerContainerIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.EntrancePower;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.EntrancePower;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.EntrancePower;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree=" + (int)EDeptContainerType.EntrancePower;
            return View("DeptContainerIndex", deptContainerData);
        }


        /// <summary>
        /// 批量生成账单
        /// </summary>
        /// <returns></returns>
        public ActionResult DeptBatchGenerateBillContainerIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.BatchGenerateBil;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.BatchGenerateBil;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.BatchGenerateBil;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.BatchGenerateBil;
            return View("DeptContainerIndex", deptContainerData);
        }
        /// <summary>
        /// 生成缴费通知单
        /// </summary>
        /// <returns></returns>
        public ActionResult DeptGeneratePaymentNoticeContainerIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.GeneratePayNot;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.GeneratePayNot;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.GeneratePayNot;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.GeneratePayNot;
            return View("DeptContainerIndex", deptContainerData);
        }

        public ActionResult DeptForeigBillContainerIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.ForeigBill;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.ForeigBill;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.ForeigBill;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.ForeigBill;
            return View("DeptContainerIndex", deptContainerData);
        }

        public ActionResult DeptDeleteChargBillContainerIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.DeleteChargBill;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.DeleteChargBill;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.DeleteChargBill;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.DeleteChargBill;
            return View("DeptContainerIndex", deptContainerData);
        }

        ///// <summary>
        ///// 支付宝接入
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult DeptAlipayInsertContainerIndex()
        //{
        //    DeptContainerData deptContainerData = new DeptContainerData();
        //    deptContainerData.PageType = (int)EDeptContainerType.AlipayInsert;
        //    deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.AlipayInsert;
        //    deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.AlipayInsert;
        //    deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.AlipayInsert;
        //    return View("DeptContainerIndex", deptContainerData);
        //}
        /// <summary>
        /// 个性化配置页面
        /// </summary>
        /// <returns></returns>
        public ActionResult DeptPersonalizedConfigurationContainerIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.PersonalizedConfiguration;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.PersonalizedConfiguration;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.PersonalizedConfiguration;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.PersonalizedConfiguration;
            return View("DeptContainerIndex", deptContainerData);
        }
        /// <summary>
        /// 支付宝物业页面
        /// </summary>
        /// <returns></returns>
        public ActionResult DeptAlipayPropertyContainerIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.AlipayProperty;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.AlipayProperty;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.AlipayProperty;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.AlipayProperty;
            return View("DeptContainerIndex", deptContainerData);
        }

        #region 收费详情 2017-02-27

        public ActionResult DeptBillDetailIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.BillDetail;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.BillDetail;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.BillDetail;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.BillDetail;
            return View("DeptContainerIndex", deptContainerData);
        }

        #endregion

        #region 预存账户管理 2017-05-09

        public ActionResult PreAccountManageIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.PreAccountManage;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.PreAccountManage;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.PreAccountManage;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.PreAccountManage;
            return View("DeptContainerIndex", deptContainerData);
        }

        #endregion

        #region 欠费通知设置 2017-05-11

        public ActionResult NotificeConfigIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.NotificeConfig;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.NotificeConfig;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.NotificeConfig;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.NotificeConfig;
            return View("DeptContainerIndex", deptContainerData);
        }

        #endregion

        #region 账单详情 2017-05-24

        public ActionResult ChargeRecordListIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.ChargeRecordList;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.ChargeRecordList;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.ChargeRecordList;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.ChargeRecordList;
            return View("DeptContainerIndex", deptContainerData);
        }

        #endregion

        #region 票据管理 V2.6
        public ActionResult ReceiptBookContainerIndex()
        {
            DeptContainerData deptContainerData = new DeptContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.ReceiptBook;
            deptContainerData.ContentUrl = "PropertyMgr/Dept/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.ReceiptBook;
            deptContainerData.ContentChildUrl = "PropertyMgr/Dept/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.ReceiptBook;
            deptContainerData.ContentSearchUrl = "PropertyMgr/Dept/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.ReceiptBook;
            return View("DeptContainerIndex", deptContainerData);
        }
        #endregion 

        #region 获取资源树

        [HttpGet]
        public ActionResult GetContainerSelectTree(int contentType)
        {
            var jResult = new JsonResult();
            DeptAppService deptService = new DeptAppService();
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jResult.MaxJsonLength = int.MaxValue;
            string strFilter = string.Empty;
            switch (contentType)
            {
                case (int)EDeptContainerType.PrepayAccount://显示到小区级别
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
                    break;
                case (int)EDeptContainerType.ChargeSubject://显示到小区级别
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
                    break;
                case (int)EDeptContainerType.SubjectHouseRef://显示到小区级别
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;

                    break;
                case (int)EDeptContainerType.ChargeBill://显示到房间级别
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";" + (int)EDeptType.LouYu + ";" + (int)EDeptType.FangWu;
                    break;
                case (int)EDeptContainerType.Entrance://显示到小区级别
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";";
                    break;
                case (int)EDeptContainerType.PaymentTasks:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
                    break;
                case (int)EDeptContainerType.Developer:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
                    break;
                //case (int)EDeptContainerType.AlipayInsert:
                //    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
                //    break;
                case (int)EDeptContainerType.PersonalizedConfiguration:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
                    break;
                case (int)EDeptContainerType.AlipayProperty:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE;
                    break;
                case (int)EDeptContainerType.EntrancePower:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";" + (int)EDeptType.LouYu;
                    break;
                case (int)EDeptContainerType.EntranceBind:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";" + (int)EDeptType.LouYu;
                    break;

                case (int)EDeptContainerType.BatchGenerateBil:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
                    break;
                case (int)EDeptContainerType.GeneratePayNot://显示到小区级别
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
                    break;
                case (int)EDeptContainerType.ForeigBill:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";";
                    break;

                case (int)EDeptContainerType.DeleteChargBill:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";";
                    break;

                case (int)EDeptContainerType.BillDetail:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";";
                    break;
                case (int)EDeptContainerType.PreAccountManage:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";" + (int)EDeptType.LouYu + ";" + (int)EDeptType.FangWu;
                    break;
                case (int)EDeptContainerType.NotificeConfig://显示到小区级别
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
                    break;
                case (int)EDeptContainerType.ChargeRecordList:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";";
                    break;

                case (int)EDeptContainerType.ReceiptBook:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";";
                    break;




            }
            /*科目绑定时小区下加载科目*/
            var treeData = deptService.GetDeptCustomTree(CurrentAdminUser.UserName, strFilter);
            bool selectedFlag = true;
            if (contentType == (int)EDeptContainerType.BatchGenerateBil)
            {
                List<CustomTreeNodeModel> resultList = new List<CustomTreeNodeModel>();
                List<CustomTreeNodeModel> tempList = new List<CustomTreeNodeModel>();
                foreach (var item in treeData)
                {
                    bool isIN = false;
                    tempList = (List<CustomTreeNodeModel>)item.children;
                    List<CustomTreeNodeModel> villageList = new List<CustomTreeNodeModel>();
                    foreach (var village in tempList)
                    {
                        var subjects = GetSubjectListNodes(Convert.ToInt32(village.id.Split('_')[0]));
                        village.children = subjects;
                        village.state = new { opened = true };
                        if (subjects.Count > 0)
                        {
                            isIN = true;
                            villageList.Add(village);
                        }                           
                    }
                    if (villageList.Count() > 0 && selectedFlag)
                    {
                        villageList.First().state = new { opened = true, selected = true };
                        selectedFlag = false;
                    }
                    item.children = villageList;
                    if (isIN)
                    {
                        resultList.Add(item);
                    }
                }
                treeData = resultList;
            }
            jResult.Data = treeData;
            return jResult;
        }

        [HttpGet]
        public ActionResult GetContainerSelectChildTree(int contentType, int id, int type)
        {
            var jResult = new JsonResult();
            DeptAppService deptService = new DeptAppService();
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jResult.MaxJsonLength = int.MaxValue;
            string strFilter = string.Empty;
            switch (contentType)
            {
                case (int)EDeptContainerType.PrepayAccount://显示到小区级别
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
                    break;
                case (int)EDeptContainerType.ChargeSubject://显示到小区级别
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
                    break;
                case (int)EDeptContainerType.SubjectHouseRef://显示到小区级别
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";" + (int)EDeptType.LouYu;
                    if (type == (int)EDeptType.XiaoQu)
                    {
                        jResult.Data = GetSubjectListNodes(id);
                        return jResult;
                    }
                    break;
                case (int)EDeptContainerType.ChargeBill://显示到房屋级别
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";" + (int)EDeptType.LouYu + ";" + (int)EDeptType.FangWu;
                    break;
                case (int)EDeptContainerType.Entrance://显示到小区级别
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";";
                    /*加载单元信息*/
                    if (type == (int)EDeptType.LouYu)
                    {
                        jResult.Data = deptService.GetDeptTree(CurrentAdminUser, id, strFilter);
                        return jResult;
                    }
                    break;
                case (int)EDeptContainerType.PaymentTasks:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
                    break;

                case (int)EDeptContainerType.Developer:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
                    break;
                //case (int)EDeptContainerType.AlipayInsert:
                //    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
                //    break;
                case (int)EDeptContainerType.PersonalizedConfiguration:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
                    break;
                case (int)EDeptContainerType.AlipayProperty:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE;
                    break;
                case (int)EDeptContainerType.EntrancePower:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";" + (int)EDeptType.LouYu;
                    break;
                case (int)EDeptContainerType.EntranceBind:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";" + (int)EDeptType.LouYu;
                    break;
                case (int)EDeptContainerType.BatchGenerateBil://显示到小区级别
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";" + (int)EDeptType.LouYu;
                    if (type == (int)EDeptType.XiaoQu)
                    {
                        jResult.Data = GetSubjectListNodes(id);
                        return jResult;
                    }
                    break;
                case (int)EDeptContainerType.GeneratePayNot://显示到小区级别
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";" + (int)EDeptType.LouYu;
                    break;
                case (int)EDeptContainerType.ForeigBill:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
                    break;

                case (int)EDeptContainerType.BillDetail:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
                    break;
                case (int)EDeptContainerType.PreAccountManage:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";" + (int)EDeptType.LouYu + ";" + (int)EDeptType.FangWu;
                    break;
                case (int)EDeptContainerType.NotificeConfig://显示到小区级别
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
                    break;
                case (int)EDeptContainerType.ChargeRecordList:
                    strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
                    break;
            }

            jResult.Data = deptService.GetDeptTree(CurrentAdminUser, id, strFilter);
            return jResult;
        }

        [HttpGet]
        public ActionResult GetContainerSearchTree(string keyWord, int contentType)
        {
            List<CustomTreeNodeModel> list = new List<CustomTreeNodeModel>();
            DeptAppService deptService = new DeptAppService();
            switch (contentType)
            {
                case (int)EDeptContainerType.ChargeBill:
                    list = deptService.GetDeptTree(this.CurrentAdminUser.UserName, keyWord.Trim());
                    break;
                case (int)EDeptContainerType.PreAccountManage:
                    list = deptService.GetDeptTree(this.CurrentAdminUser.UserName, keyWord.Trim());
                    break;
                    //case (int)EDeptContainerType.Entrance:
                    //    string strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";";
                    //    list = deptService.GetDeptTreeByVillageName(CurrentAdminUser.UserName, strFilter, keyWord);

                    //    break;
            }
            if (list == null || !(list.Count > 0))
            {
                list = new List<CustomTreeNodeModel>();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /*根据小区加载科目,绑定科目时加载科目信息*/
        public List<AsynTreeNodeModel> GetSubjectListNodes(int comDeptId)
        {
            List<AsynTreeNodeModel> list = new List<AsynTreeNodeModel>();

            ChargeSubjectAppService chargeSubjectAppService = new ChargeSubjectAppService();
            List<ChargeSubjectDTO> listChargeSubject = chargeSubjectAppService
                                    .GetChargeSubjectsByComDeptId(comDeptId)
                                    .Where(o => (o.SubjectType != (int)SubjectTypeEnum.Other
                                    && o.SubjectType != (int)SubjectTypeEnum.SystemPreset 
                                    && (o.BillPeriod != (int)BillPeriodEnum.Once)) 
                                    && o.IsDel == false).ToList();

            return listChargeSubject.Select(o => 
                new AsynTreeNodeModel()
                {
                    id = o.Id + "_1001_" + o.BillPeriod+"_"+o.SubjectType+"_"+o.BillPeriod,
                    text = o.Name,
                    children = false,
                    icon = "fa fa-book"
                }).ToList();
        }

        #endregion
    }





    public enum EDeptContainerType
    {
        PrepayAccount = 1,//账户管理使用
        ChargeSubject = 2,/*科目管理*/
        SubjectHouseRef = 3,/*科目绑定*/
        ChargeBill = 4,/*账单绑定*/
        Entrance = 5,/*设备管理*/
        PaymentTasks = 6,/*交款*/
        Developer = 7/*开发商*/,
        EntrancePower = 8,/*门禁权限*/
        EntranceBind = 9,/*设备绑定*/
        BatchGenerateBil=10,/*批量生成账单*/
        ForeigBill=11,/*对外收费*/
        DeleteChargBill=12,/*删除账单*/
        BillDetail = 13, //账单详情
        CarParkChargBill =14,//车位收款
        GeneratePayNot = 15, //生成缴费通知单
        PreAccountManage = 16, //预存账户管理
        NotificeConfig = 17,//欠费通知
        ReceiptBook = 18,//票据管理
        ChargeRecordList = 19, //费用记录
        //AlipayInsert = 20 ,//支付宝接入
        AlipayProperty = 20, //支付宝物业接入
        PersonalizedConfiguration = 21//个性化配置
        
    };

    public class DeptListData
    {
        public string Language { get; set; }
    }


    public class DeptContainerData
    {
        public string ContentUrl { get; set; }
        public string ContentChildUrl { get; set; }
        public string ContentSearchUrl { get; set; }
        public int PageType { get; set; }
    }






}