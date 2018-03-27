using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using Microsoft.Practices.Unity;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.RepositoryContract;
using System.Linq.Expressions;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.DomainEntity.DomainEntity;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO;
using System.Web.Http;
using YK.BackgroundMgr.DomainInterface;

namespace YK.PropertyMgr.DomainService
{
    public partial class SubjectHouseRefDomainService
    {
        public IList<SubjectBindLogDTO> GetSubjectBindLog(int PageSatrt, int PageSize, Expression<Func<SubjectHouseRef, bool>> predicate, DateTime? beginDate, DateTime? endDate, string expressions, out int totalCount)
        {
            beginDate = beginDate ?? DateTime.MinValue;
            endDate = endDate.HasValue ? endDate.Value.AddDays(1).AddSeconds(-1) : DateTime.MaxValue;
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var query = (from b in propertyMgrUnitOfWork.SubjectHouseRefRepository.GetAll().Where(predicate)//绑定记录
                             where beginDate <= b.CreateTime && b.CreateTime <= endDate
                             select new SubjectBindLogDTO { ResourceName = b.ResourceName, SubjectName = b.ChargeSubject.Name, Operator = b.Operator, OperateTime = b.CreateTime, IsDel = false }).Concat(
                             from nb in propertyMgrUnitOfWork.SubjectHouseRefRepository.GetAll().Where(predicate)//解绑记录
                             where beginDate <= nb.UpdateTime && nb.UpdateTime <= endDate && nb.IsDel == true
                             select new SubjectBindLogDTO { ResourceName = nb.ResourceName, SubjectName = nb.ChargeSubject.Name, Operator = nb.RelieveOperator, OperateTime = nb.UpdateTime, IsDel = true });
                totalCount = query.Count();
                var dataList = query.OrderBy(q => q.ResourceName).ThenBy(q => q.SubjectName).ThenBy(q => q.OperateTime).Skip(PageSatrt).Take(PageSize).ToList();
                return dataList;
            }
        }





        /// <summary>
        /// 获取单个绑定项目
        /// </summary>
        /// <returns></returns>
        public SubjectHouseRef GetSubjectHouseRefSingle(Expression<Func<SubjectHouseRef, bool>> where)
        {
            SubjectHouseRef model = null;
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                model = propertyMgrUnitOfWork.SubjectHouseRefRepository.GetAll().Where(where).FirstOrDefault();
                return model;
            }
        }
        /// <summary>
        /// 根据条件获取绑定项目集合
        /// </summary>
        /// <returns></returns>
        public List<SubjectHouseRef> GetChargeSubjectList(Expression<Func<SubjectHouseRef, bool>> where)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.SubjectHouseRefRepository.DatabaseContext.Database.CommandTimeout = 360;
                return propertyMgrUnitOfWork.SubjectHouseRefRepository.GetAll().Where(where).ToList();
            }
        }

        public List<SubjectHouseRefDTO> GetChargeSubjectInfoList(int resId, int subjectType)
        {
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //propertyMgrUnitOfWork.SubjectHouseRefRepository.DatabaseContext.Database.CommandTimeout = 360;
                var billQuery = pmUnitWork.ChargBillRepository.GetAll().Where(p => p.ResourcesId == resId && p.RefType == subjectType && p.IsDel == false);
                var query = from s in pmUnitWork.SubjectHouseRefRepository.GetAll()
                            where s.ResourcesId == resId
                            && s.SubjectType == subjectType
                            && s.IsDel ==  false
                            select new SubjectHouseRefDTO()
                            {
                                Id = s.Id,
                                ChargeSubjecId = s.ChargeSubjecId,
                                DevBeginDate = s.DevBeginDate,
                                DevEndDate = s.DevEndDate,
                                BeginDateBill = s.BeginDateBill,
                                IsBill = billQuery.Any(b => b.ResourcesId == s.ResourcesId && b.RefType == s.SubjectType && b.ChargeSubjectId == s.ChargeSubjecId)
                            };
                return query.ToList();
            }
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="domainSubjectHouseRefList"></param>
        /// <returns></returns>
        public bool InsertSubjectHouseRefList(List<SubjectHouseRef> domainSubjectHouseRefList)
        {
            try
            {
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    foreach (var item in domainSubjectHouseRefList)
                    {
                        propertyMgrUnitOfWork.SubjectHouseRefRepository.Add(item);
                    }
                    propertyMgrUnitOfWork.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[批量绑定项目]   ErrorMsg:{1}", ex.Message), "SubjectHouseRefDomainService_InsertSubjectHouseRefList", FileLogType.Exception);
                throw ex;
            }
        }
        /// <summary>
        /// 批量新增或修改
        /// </summary>
        /// <param name="domainSubjectHouseRefList"></param>
        /// <returns></returns>
        public bool BatchInsertOrUpdateSubjectHouseRefList(List<SubjectHouseRef> domainSubjectHouseRefList, int operaterId, DeveloperSetTimeListDTO model)
        {
            
            try
            {
                List<SubjectHouseRef> updatedDmainSubjectHouseRefList = new List<SubjectHouseRef>();
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    foreach (var item in domainSubjectHouseRefList)
                    {
                        if (!(item.Id > 0))
                        {
                            propertyMgrUnitOfWork.SubjectHouseRefRepository.Add(item);
                        }
                        else
                        {
                            if (item.IsDel.Value)
                            {
                                item.UpdateTime = DateTime.Now;
                                item.RelieveOperator = operaterId;
                            }
                            else
                            {
                                if (model !=null)
                                {
                                    var timelist = model.DeveloperSetTimelist;
                                    foreach (var time in timelist)
                                    {
                                        if (item.ChargeSubjecId == time.SubjectId)
                                        {
                                            if (time.BeginDateBill != null)
                                            {
                                                item.BeginDateBill = time.BeginDateBill;
                                            }
                                        }
                                    }
                                } 
                            }
                            updatedDmainSubjectHouseRefList.Add(item);
                        }
                    }
                    propertyMgrUnitOfWork.SubjectHouseRefRepository.UpdateRange(updatedDmainSubjectHouseRefList);
                    propertyMgrUnitOfWork.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[批量绑定项目]   ErrorMsg:{1}", ex.Message), "SubjectHouseRefDomainService_InsertSubjectHouseRefList", FileLogType.Exception);
                throw ex;
            }
        }

        /// <summary>
        /// 接口单一解除车位绑定
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void CarportChangeNotice([FromBody]APICarportChangeParameter para)
        {
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var updateEntity = pmUnitWork.SubjectHouseRefRepository.GetAll().Where
                                                 (s => s.HouseDeptId == para.HouseDeptId
                                                    && s.ResourcesId == para.CarportId
                                                    && s.IsDel == false
                                                    && s.ChargeSubject.SubjectType == (int)SubjectTypeEnum.ParkingSpace
                                                ).FirstOrDefault();
                if (updateEntity != null)
                {
                    updateEntity.IsDel = true;
                    updateEntity.UpdateTime = DateTime.Now;
                    updateEntity.RelieveOperator = para.RelieveOperator;
                    pmUnitWork.SubjectHouseRefRepository.Update(updateEntity);
                    pmUnitWork.Commit();

                }
            }
        }

        /// <summary>
        /// 接口：通过电话号码验证进行自助缴费
        /// </summary>
        /// <returns></returns>
        public OwnerInformationDTO GetOwnerByBindingPhonerNumber(string BindingPhonerNumber)
        {
            OwnerInformationDTO ownerinfor = new OwnerInformationDTO();
            var Owner = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetOwnerByBindingPhonerNumber(BindingPhonerNumber);
            foreach (var item in Owner)
            {
                ownerinfor.DoorNo = item.DoorNo;
                ownerinfor.BindingPhonerNumber = item.BindingPhonerNumber;
                ownerinfor.UserName = item.UserName;
            }
            return ownerinfor;

        }

        /// <summary>
        ///  通过小区ID获取楼栋信息
        /// </summary>
        /// <param name="ComDeptId"></param>
        /// <returns></returns>
        public DeptInfoDTO GetBuildingByComDeptId(int? ComDeptId)
        {
            DeptInfoDTO deptinfo = new DeptInfoDTO();
            var dept = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetBuildingByComDeptId(ComDeptId);
            foreach (var item in dept)
            {
                deptinfo.Id = item.Id;
                deptinfo.Name = item.Name;
                deptinfo.Code = item.Code;
            }
            return deptinfo;
        }
        /// <summary>
        /// 通过房屋选择并验证后获取住户详细信息
        /// </summary>
        /// <param name="RoomId"></param>
        /// <param name="BindingPhonerNumber"></param>
        /// <returns></returns>
        public OwnerInformationDTO GetOwnerByValidatePhonerNumber(int? RoomId, string BindingPhonerNumber)
        {
            OwnerInformationDTO ownerinfor = new OwnerInformationDTO();
            var Owner = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetOwnerByValidatePhonerNumber(RoomId, BindingPhonerNumber);
            foreach (var item in Owner)
            {
                ownerinfor.DoorNo = item.DoorNo;
                ownerinfor.BindingPhonerNumber = item.BindingPhonerNumber;
                ownerinfor.UserName = item.UserName;
            }
            return ownerinfor;

        }

        /// <summary>
        /// 获取公区表绑定关系
        /// </summary>
        /// <param name="comDeptId"></param>
        /// <param name="meterType"></param>
        /// <returns></returns>
        public List<MeterBindSubjectHouse> GetSelectPublicMeter(int? subjectId)
        {

                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                var query = from m in propertyMgrUnitOfWork.MeterRepository.GetAll().Where(o =>o.IsPublicArea==true && o.IsEnabled==true)
                            join s in propertyMgrUnitOfWork.SubjectHouseRefRepository.GetAll().Where(o => o.ChargeSubjecId== subjectId && o.IsDel==false)
                            on m.Id equals s.ResourcesId
                            select new MeterBindSubjectHouse
                            {
                                Id = m.Id,
                                MeterNum = m.MeterNum,
                                IsDevPay=s.IsDevPay,
                                DevBeginDate=s.DevBeginDate,
                                DevEndDate=s.DevEndDate
                            };
                return query.ToList();
                }

        }

    }
}
