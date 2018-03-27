using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.ApplicationService.Service;
using YK.PropertyMgr.CompositeAppService;
using YK.PropertyMgr.Crosscuting;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class CarParkController : BaseController
    {
        // GET: CarPark
        public ActionResult Index()
        {
            CarParkListData carParkListData = new CarParkListData();
            carParkListData.Language = Language;
            return View(carParkListData);
        }

        public ActionResult CarParkChargeBillContainerIndex()
        {
            CarParkContainerData deptContainerData = new CarParkContainerData();
            deptContainerData.PageType = (int)EDeptContainerType.CarParkChargBill;
            deptContainerData.ContentUrl = "PropertyMgr/CarPark/GetContainerSelectTree?contentType=" + (int)EDeptContainerType.CarParkChargBill;
            deptContainerData.ContentChildUrl = "PropertyMgr/CarPark/GetContainerSelectChildTree?contentType=" + (int)EDeptContainerType.CarParkChargBill;
            deptContainerData.ContentSearchUrl = "PropertyMgr/CarPark/GetContainerSearchTree?contentType=" + (int)EDeptContainerType.CarParkChargBill;
            return View("CarParkContainerIndex", deptContainerData);
        }

        [HttpGet]
        public ActionResult GetContainerSelectTree(int contentType)
        {
            var jResult = new JsonResult();
     
            CarParkAppService carParkAppService = new CarParkAppService(); ;
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;


            /*科目绑定时小区下加载科目*/
            var treeData = carParkAppService.GetCarParkTree(CurrentAdminUser.UserName);//GetDeptCustomTree(CurrentAdminUser.UserName, strFilter);
            jResult.Data = treeData;
            return jResult;

        }



        [HttpGet]
        public ActionResult GetContainerSelectChildTree(int contentType, string id, int type)
        {
            var jResult = new JsonResult();
            CarParkAppService carParkAppService = new CarParkAppService(); 
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (type == (int)EDeptType.XiaoQu)
            {//获取停车场
                var treeData = carParkAppService.GetCarParkByCommunityId(id);
                jResult.Data = treeData;
            }
            else if(type == (int)EDeptType.CheKu)
            {
                var treeData = carParkAppService.GetCarportByParkId(id);
                jResult.Data = treeData;
            }
            return jResult;
        }

        [HttpGet]
        public ActionResult GetContainerSearchTree(string keyWord, int contentType)
        {
            List<CustomTreeNodeModel> list = new List<CustomTreeNodeModel>();
            CarParkAppService carParkAppService = new CarParkAppService();
            switch (contentType)
            {
                case (int)EDeptContainerType.CarParkChargBill:
                    list = carParkAppService.GetCarParkTree(CurrentAdminUser.UserName, keyWord).ToList();//deptService.GetDeptTree(this.CurrentAdminUser.UserName, keyWord.Trim());
                    break;
            }
            if (list == null || !(list.Count > 0))
            {
                list = new List<CustomTreeNodeModel>();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }





    }





 
    public class CarParkListData
    {
        public string Language { get; set; }
    }

    public class CarParkContainerData
    {
        public string ContentUrl { get; set; }
        public string ContentChildUrl { get; set; }
        public string ContentSearchUrl { get; set; }
        public int PageType { get; set; }
    }

}