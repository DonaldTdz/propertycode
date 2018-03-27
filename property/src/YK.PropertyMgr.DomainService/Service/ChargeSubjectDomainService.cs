using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.RepositoryContract;
using System.Linq.Expressions;
using YK.PropertyMgr.CompositeDomainService.GenerateBillService;
using YK.PropertyMgr.ApplicationDTO;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using YK.BackgroundMgr.DomainInterface;
using YK.PropertyMgr.CompositeDomainService;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.BackgroundMgr.PresentationService;

namespace YK.PropertyMgr.DomainService
{
    public partial class ChargeSubjectDomainService
    {
        /// <summary>
        /// 获取单个ChargeSubject
        /// </summary>
        /// <returns></returns>
        public ChargeSubject GetChargeSubjectSingle(Expression<Func<ChargeSubject, bool>> where)
        {
            ChargeSubject model = null;
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                model = propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll().Where(where).FirstOrDefault();
                return model;
            }
        }
        /// <summary>
        /// 根据条件获取ChargeSubject集合
        /// </summary>
        /// <returns></returns>
        public List<ChargeSubject> GetChargeSubjectList(Expression<Func<ChargeSubject, bool>> where)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll().Where(where).ToList();
            }
        }

        /// <summary>
        /// 根据房屋HouseDeptId获取ChargeSubject集合
        /// </summary>
        public List<ChargeSubject> GetChargeSubjectListByHouseDeptId(int HouseDeptId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return (from c in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll()
                       join r in propertyMgrUnitOfWork.SubjectHouseRefRepository.GetAll() on c.Id equals r.ChargeSubjecId
                       where r.HouseDeptId == HouseDeptId && c.IsDel == false && r.IsDel ==  false
                       select c).Distinct().ToList();
            }
        }

        /// <summary>
        /// 获取收费项目树
        /// </summary>
        /// <returns></returns>
        public List<CustomTreeNodeModel> GetChargeSubjectTree(int HouseDeptId, string DeptName)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var query = from c in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll()
                            join r in propertyMgrUnitOfWork.SubjectHouseRefRepository.GetAll() 
                            on c.Id equals r.ChargeSubjecId
                            where r.HouseDeptId == HouseDeptId 
                            && c.IsDel == false 
                            && r.IsDel == false
                            select new
                            {
                                c.Id,
                                c.Name,
                                c.SubjectType,
                                //r.ResourceName,
                                r.ResourcesId
                            };
                var resultList = query.Distinct().OrderBy(r => r.Name).ToList();
                List<CustomTreeNodeModel> treeList = new List<CustomTreeNodeModel>();
                foreach (var item in resultList)
                {
                    CustomTreeNodeModel model = new CustomTreeNodeModel();
                    model.id = item.Id.ToString() + "_" + item.ResourcesId.ToString();
                    model.text = item.Name;
                    //如果是三表去截止读数
                    if (item.SubjectType == (int)SubjectTypeEnum.Meter)
                    {
                        var meter = propertyMgrUnitOfWork.MeterRepository.GetAll()
                            .Where(m => m.HouseDeptID == HouseDeptId && m.Id == item.ResourcesId && m.IsEnabled == true)
                            .FirstOrDefault();                                      

                        if (meter != null)
                        {
                            var lastMeter = propertyMgrUnitOfWork.MeterReadRecordRepository
                                                               .GetAll()
                                                               .Where(m => m.MeterId == meter.Id)
                                                               .OrderByDescending(m => m.ReadDate)
                                                               .FirstOrDefault();
                            var mtype = (MeterTypeEnum)meter.MeterType;
                            string mname = string.Empty;
                            switch (mtype)
                            {
                                case MeterTypeEnum.WaterMeter:
                                    {
                                        mname = "水表";
                                    }
                                    break;
                                case MeterTypeEnum.GasMeter:
                                    {
                                        mname = "气表";
                                    }
                                    break;
                                case MeterTypeEnum.WattHourMeter:
                                    {
                                        mname = "电表";
                                    }
                                    break;
                            }
                            if (lastMeter != null)
                            {
                                model.text += "(" + mname + meter.MeterNum + "  截止读数  " + lastMeter.MeterValue + ")";
                            }
                            else
                            {
                                model.text += "(" + mname + meter.MeterNum + ")";
                            }
                        }

                    }
                    //取截止时间
                    else
                    {
                        var bill = propertyMgrUnitOfWork.ChargBillRepository.GetAll()
                       .Where(b => b.ChargeSubjectId == item.Id && b.HouseDeptId == HouseDeptId && b.ResourcesId == item.ResourcesId && b.IsDel == false)
                       .OrderByDescending(b => b.EndDate)
                       .FirstOrDefault();
                        var tname = string.Empty;
                        if (item.SubjectType == (int)SubjectTypeEnum.House)
                        {
                            tname = "房屋";
                            if (bill != null)
                            {
                                model.text += "(" + tname + bill.ResourcesName + "  截止时间  " + bill.EndDate.Value.ToString("yyyy-MM-dd") + ")";
                            }
                            else
                            {
                                model.text += "(" + tname + DeptName + ")";
                            }
                        }
                        if (item.SubjectType == (int)SubjectTypeEnum.ParkingSpace)
                        {
                            tname = "车位";
                            if (bill != null)
                            {
                                model.text += "(" + tname + bill.ResourcesName + "  截止时间  " + bill.EndDate.Value.ToString("yyyy-MM-dd") + ")";
                            }
                            //修改bug #4584 2017-7-7 tdz 当绑定的车位没有生成过账单 需要重新去取
                            else
                            {
                                var carport = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCarPortById(item.ResourcesId.Value);
                                if (carport == null)
                                {
                                    model.text += "(" + tname + ")";
                                }
                                else
                                {
                                    model.text += "(" + tname + carport.CarportNum + ")";
                                }
                            }
                        }            
                    }
                    model.children = new List<CustomTreeNodeModel>();
                    model.icon = "fa fa-tags";
                    model.state = new { selected = true };
                    treeList.Add(model);
                }
                return treeList;
            }
        }

        public List<CustomTreeNodeModel> GetParkingSpaceChargeSubjectTree(int ResourcesId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var query = from c in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll()
                            join r in propertyMgrUnitOfWork.SubjectHouseRefRepository.GetAll()
                            on c.Id equals r.ChargeSubjecId
                            where r.ResourcesId == ResourcesId
                            && r.SubjectType == (int)SubjectTypeEnum.ParkingSpace//车位
                            && c.IsDel == false
                            && r.IsDel == false
                            select new
                            {
                                c.Id,
                                c.Name,
                                c.SubjectType,
                                //r.ResourceName,
                                r.ResourcesId
                            };
                var resultList = query.Distinct().OrderBy(r => r.Name).ToList();
                List<CustomTreeNodeModel> treeList = new List<CustomTreeNodeModel>();
                foreach (var item in resultList)
                {
                    CustomTreeNodeModel model = new CustomTreeNodeModel();
                    model.id = item.Id.ToString() + "_" + item.ResourcesId.ToString();
                    model.text = item.Name;

                    var bill = propertyMgrUnitOfWork.ChargBillRepository.GetAll()
                    .Where(b => b.ChargeSubjectId == item.Id && b.ResourcesId == item.ResourcesId && b.IsDel == false)
                    .OrderByDescending(b => b.EndDate)
                    .FirstOrDefault();
                    if (bill != null)
                    {
                        model.text += "(截止时间  " + bill.EndDate.Value.ToString("yyyy-MM-dd") + ")";
                    }
                    model.children = new List<CustomTreeNodeModel>();
                    model.icon = "fa fa-tags";
                    model.state = new { selected = true };
                    treeList.Add(model);
                }
                return treeList;
            }
        }

        /// <summary>
        /// 根据车位获取
        /// </summary>
        /// <param name="ResourcesId"></param>
        /// <param name="RefType"></param>
        /// <returns></returns>
        public List<ChargeSubject> GetChargeSubjectListByResourcesId(int ResourcesId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return (from c in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll()
                        join r in propertyMgrUnitOfWork.SubjectHouseRefRepository.GetAll() on c.Id equals r.ChargeSubjecId
                        where r.ResourcesId == ResourcesId&& c.IsDel == false && r.IsDel == false&&r.SubjectType==(int)SubjectTypeEnum.ParkingSpace
                        select c).Distinct().ToList();
            }
        }




        public decimal ComputeChargeSubjectAmount(int ChargeSubjectId,int ResourcesId, int RefTypeId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                decimal? calculationAmout = 0;
                //房屋计算
                var chargesubject = propertyMgrUnitOfWork.ChargeSubjectRepository.GetByKey(ChargeSubjectId);
                IList<CalculateProperty> hmpList = CalculatePropertyHelper.GetHouseCalculateProperty(chargesubject.ComDeptId.Value, ResourcesId);
                if (RefTypeId == (int)ReourceTypeEnum.House)
                {
                    IList<CalculateProperty> pmpList = CalculatePropertyHelper.GetParkingSpaceCalculateProperty(chargesubject.ComDeptId.Value, hmpList, ResourcesId);
                    string qstr = string.Empty;
                    if (chargesubject.SubjectType == (int)SubjectTypeEnum.House)
                    {

                        calculationAmout = BillCommonService.Instance.CalculateAmount(chargesubject, hmpList.First(), ref qstr);

                    }
                    else if (chargesubject.SubjectType == (int)SubjectTypeEnum.ParkingSpace)
                    {
                        foreach (var pitem in pmpList)
                        {
                            calculationAmout += BillCommonService.Instance.CalculateAmount(chargesubject, pitem, ref qstr);
                        }
                    }
                    else
                    {//其他的返回单价
                        calculationAmout = chargesubject.Price.Value;

                    }
                }
                else if(RefTypeId== (int)ReourceTypeEnum.CarPark)
                {//车位计算
                    //var chargesubject = propertyMgrUnitOfWork.ChargeSubjectRepository.GetByKey(ChargeSubjectId);
                    IList<CalculateProperty> pmpList = CalculatePropertyHelper.GetParkingSpaceCalculateProperty(chargesubject.ComDeptId.Value, hmpList, 0, ResourcesId, RefTypeId);
                    string qstr = string.Empty;
                    foreach (var pitem in pmpList)
                    {
                        calculationAmout += BillCommonService.Instance.CalculateAmount(chargesubject, pitem, ref qstr);
                    }

                }
                return calculationAmout.Value;


            }
        }

    }
}
