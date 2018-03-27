using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using YK.PropertyMgr.RepositoryContract;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.Crosscuting;
using System.Linq.Expressions;

namespace YK.PropertyMgr.DomainService
{
    public partial class TemplatePrintRecordDomainService
    {
        public bool InserCreateTemplatePrint(TemplatePrintRecord domainTemplatePrintRecord,List<TemplatePrintRecordDetail> DetailList )
        {
             

            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.TemplatePrintRecordRepository.Add(domainTemplatePrintRecord);

                propertyMgrUnitOfWork.TemplatePrintRecordRepository.DatabaseContext.SaveChanges();

                var id = domainTemplatePrintRecord.Id;
                 DetailList.ForEach(o => o.TemplatePrintRecordId = id.Value);
                try
                {

                    propertyMgrUnitOfWork.TemplatePrintRecordDetailRepository.AddRange(DetailList);
                    propertyMgrUnitOfWork.Commit();
                    return true;
                }
                catch (Exception e)
                {
                     
                }

                return false;

            }
         }


        public TemplatePrintRecord GetModelByQuery(Expression<Func<TemplatePrintRecord, bool>> predicate)
        {

            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
              return  propertyMgrUnitOfWork.TemplatePrintRecordRepository.GetAll().Where(predicate).FirstOrDefault();
            }
        }



    }



    
}
    