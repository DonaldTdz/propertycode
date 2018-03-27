using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.PresentationService;

namespace YK.BackgroundMgr.MVCWeb
{
    public class TestController : Controller
    {
        public List<AsynTreeNodeModel> GetAsynDeptTreeTest(string userName, int parentId)
        {
            //return PresentationServiceHelper.LookUp<IPropertyService>().GetAsynDeptTree(userName, parentId);
            return new List<AsynTreeNodeModel>();
        }
    }
}
