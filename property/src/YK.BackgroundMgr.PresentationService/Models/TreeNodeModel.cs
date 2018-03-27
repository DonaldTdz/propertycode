using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.PresentationService
{
    [Serializable]
    public class BaseTreeNodeModel
    {
        public string id { get; set; }

        public string text { get; set; }
        public string icon { get; set; }

        [NonSerialized]
        public string code;

    }

    [Serializable]
    public class TreeNodeModel : BaseTreeNodeModel
    {
        public List<TreeNodeModel> children { get; set; }
        public object state { get; set; }
    }

    [Serializable]
    public class AsynTreeNodeModel : BaseTreeNodeModel
    {
        public bool children { get; set; }
    }

    [Serializable]
    public class CustomTreeNodeModel : BaseTreeNodeModel
    {
        public object children { get; set; }
        public object state { get; set; }
    }
}
