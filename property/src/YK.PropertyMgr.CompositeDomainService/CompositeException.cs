using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.CompositeDomainService
{
    public class CompositeException : Exception
    {
        public CompositeException(string message)
            : base(message)
        {
           
        }
    }
}
