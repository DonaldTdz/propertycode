using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.ApplicationService;
using YK.BackgroundMgr.PresentationService;
using System.Reflection;
using Newtonsoft.Json;
using YK.PropertyMgr.RepositoryContract;
using YK.BackgroundMgr.DomainInterface;
using YK.PropertyMgr.ApplicationDTO.Enums;

namespace YK.PropertyMgr.ApplicationService
{

    public partial class ChargeSubjectAppService
    {
        #region 新增科目
        /// <summary>
        /// 新增科目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnResult InsertChargeSubjectCus(ChargeSubjectDTO model)
        {
            ReturnResult res = new ReturnResult();
            try
            {
                res = Validation(model);
                if (!res.IsSuccess)
                {
                    return res;
                }
                bool isSuccess = InsertChargeSubject(model);
                if (isSuccess)
                {
                    res.IsSuccess = true;
                    res.Msg = "处理成功!";
                    return res;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Msg = "处理失败!";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Msg = " 数据异常!";
                return res;
            }

        }
        #endregion

        #region 修改科目
        /// <summary>
        /// 修改科目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnResult UpdateChargeSubjectCus(ChargeSubjectDTO model)
        {
            ReturnResult res = new ReturnResult();
            try
            {
                res = Validation(model);
                if (!res.IsSuccess)
                {
                    return res;
                }
                //ChargeSubjectDTO modelUpdate = GetChargeSubjectSingle((int)model.Id);//liyu测试后不需要
                /*需要更改的赋值*/
                bool isSuccess = UpdateChargeSubject(model);
                if (isSuccess)
                {
                    res.IsSuccess = true;
                    res.Msg = "处理成功!";
                    return res;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Msg = "处理失败!";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Msg = " 数据异常!";
                return res;
            }

        }
        #endregion

        #region 删除科目
        /// <summary>
        /// 删除科目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnResult DeleteChargeSubjectCus(int subjectId)
        {
            ReturnResult res = new ReturnResult();
            try
            {
                ChargeSubjectDTO subject = GetChargeSubjectByKey(subjectId);
                if (null == subject)
                {
                    res.IsSuccess = false;
                    res.Msg = "没有该对象信息";
                    return res;
                }
                subject.IsDel = true;
                bool isSuccess = UpdateChargeSubject(subject);
                if (isSuccess)
                {
                    res.IsSuccess = true;
                    res.Msg = "删除对象成功!";
                    return res;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Msg = "删除对象失败!";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Msg = " 数据异常!";
                return res;
            }

        }
        #endregion

        #region 获取单个对象
        /// <summary>
        /// 获取单个ChargeSubject
        /// </summary>
        /// <returns></returns>
        public ChargeSubjectDTO GetChargeSubjectSingle(int id)
        {
            return ChargeSubjectMappers.ChangeChargeSubjectToDTO(ChargeSubjectService.GetChargeSubjectByKey(id));
        }
        #endregion

        #region 获取小区下的科目信息
        /// <summary>
        /// 获取小区下的科目信息
        /// </summary>
        /// <param name="communityDeptId">小区ID</param>
        /// <returns></returns>
        public List<ChargeSubjectDTO> GetChargeSubjectsByComDeptId(int communityDeptId)
        {
            return ChargeSubjectMappers.ChangeChargeSubjectToDTOs(ChargeSubjectService.GetChargeSubjectList(o => o.ComDeptId == communityDeptId && o.IsDel == false));
        }
        #endregion


        #region 获取小区下的科目信息（含全部）
        /// <summary>
        /// 获取小区下的科目信息
        /// </summary>
        /// <param name="communityDeptId">小区ID</param>
        /// <returns></returns>
        public List<ChargeSubjectDTO> GetAllChargeSubjectsByComDeptId(int communityDeptId)
        {
            var AllChargeList = new List<ChargeSubjectDTO>();
            AllChargeList.Add(new ChargeSubjectDTO() { Id=0,Name= "-- 全部 --" });
            var ChargeListDTOList= ChargeSubjectMappers.ChangeChargeSubjectToDTOs(ChargeSubjectService.GetChargeSubjectList(o => o.ComDeptId == communityDeptId && o.IsDel == false));
            AllChargeList.AddRange(ChargeListDTOList);
            return AllChargeList;
        }
        #endregion


        #region 获取小区下的科目信息
        /// <summary>
        /// 获取小区下的科目信息,排除 预存费，其他、一次性费用
        /// </summary>
        /// <param name="communityDeptId">小区ID</param>
        /// <param name="resType">资源类型【12楼宇，（1.水，2.电，3.气）表，13.车库】</param>
        /// <returns></returns>
        public List<ChargeSubjectDTO> GetChargeSubjectsByComDeptId(int communityDeptId, int resType)
        {
            var list = GetChargeSubjectsByComDeptId(communityDeptId);
            list = list.Where(o => o.SubjectType != (int)SubjectTypeEnum.SystemPreset && o.SubjectType != (int)SubjectTypeEnum.Other && o.BillPeriod != (int)BillPeriodEnum.Once).ToList();
            int originalType = resType;/*三表的原始类型1.水表、2.电表、3.气表*/
            if (resType == (int)MeterTypeEnum.GasMeter || resType == (int)MeterTypeEnum.WaterMeter || resType == (int)MeterTypeEnum.WattHourMeter)
            {
                resType = (int)SubjectTypeEnum.Meter;
            }
            switch (resType)
            {
                case (int)EDeptType.LouYu:
                    list = list.Where(o => o.SubjectType == (int)SubjectTypeEnum.House).ToList();
                    break;
                case (int)EDeptType.CheKu:
                    list = list.Where(o => o.SubjectType == (int)SubjectTypeEnum.ParkingSpace).ToList();
                    break;
                case (int)SubjectTypeEnum.Meter:
                    Dictionary<int, int> dic = new Dictionary<int, int>();
                    dic.Add((int)MeterTypeEnum.WaterMeter, (int)ChargeFormulaEnum.WaterUnit);
                    dic.Add((int)MeterTypeEnum.WattHourMeter, (int)ChargeFormulaEnum.ElectricUnit);
                    dic.Add((int)MeterTypeEnum.GasMeter, (int)ChargeFormulaEnum.GasUnit);
                    list = list.Where(o => o.SubjectType == (int)SubjectTypeEnum.Meter).ToList();
                    var meterSubjects = new List<ChargeSubjectDTO>();
                    string[] arr = null;
                    list.ForEach(o =>
                    {
                        arr = o.ChargeFormula.Split('+', '-', '*', '/', '(', ')', ',');
                        if (arr != null && arr.Length > 0)
                        {
                            foreach (var item in arr)
                            {
                                if (item == dic[originalType].ToString())/*根据公式判断是那种类型的三表科目【水、电、气】*/
                                {
                                    meterSubjects.Add(o);
                                }
                            }
                        }
                    });
                    list = meterSubjects;
                    break;
            }
            return list;
        }
        #endregion


        #region 获取对象集合
        /// <summary>
        /// 根据条件获取ChargeSubject集合
        /// </summary>
        /// <returns></returns>
        public List<ChargeSubjectDTO> GetChargeSubjectList(Expression<Func<ChargeSubject, bool>> where)
        {
            return ChargeSubjectMappers.ChangeChargeSubjectToDTOs(ChargeSubjectService.GetChargeSubjectList(where));
        }
        public List<ChargeSubjectDTO> GetChargeSubjectList(int villageDeptId)
        {
            return ChargeSubjectMappers.ChangeChargeSubjectToDTOs(ChargeSubjectService.GetChargeSubjectList(o => o.ComDeptId == villageDeptId&&o.IsDel==false&&o.SubjectType!=(int)SubjectTypeEnum.SystemPreset));
        }
        #endregion

        #region 数据校验

        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnResult Validation(ChargeSubjectDTO model)
        {

            ReturnResult res = new ReturnResult()
            {
                IsSuccess = true
            };
            if (string.IsNullOrEmpty(model.Name))
            {
                res.Msg = "科目名称不能为空";
                res.IsSuccess = false;
                return res;
            }

            ChargeSubject isExist = null;
            if (model.Id > 0)
            {
                isExist = ChargeSubjectService.GetChargeSubjectSingle(o => o.Id == model.Id);
                if (null == isExist)
                {
                    res.Msg = "该小区下没有此条收费信息!";
                    res.IsSuccess = false;
                    return res;
                }
                isExist = ChargeSubjectService.GetChargeSubjectSingle(o => o.Name == model.Name && o.ComDeptId == model.ComDeptId && o.Id != model.Id);

            }
            else
            {
                isExist = ChargeSubjectService.GetChargeSubjectSingle(O => O.Name == model.Name && O.ComDeptId == model.ComDeptId);

            }
            if (null != isExist)
            {
                res.Msg = "该小区下已经存在该科目!";
                res.IsSuccess = false;
                return res;
            }
            if (!(model.AutomaticBill >= 0))
            {
                res.Msg = "请选择是否自动生成账单!";
                res.IsSuccess = false;
                return res;
            }
            if (string.IsNullOrEmpty(model.Code))
            {
                res.Msg = "编码不能为空";
                res.IsSuccess = false;
                return res;
            }
            if (string.IsNullOrEmpty(model.Price.ToString()))
            {
                res.Msg = "单价不能为空";
                res.IsSuccess = false;
                return res;
            }
            else
            {
                if (!(model.Price > 0))
                {
                    res.Msg = "单价必须大于0";
                    res.IsSuccess = false;
                    return res;
                }
                string temp = @"^[0-9]+([.]\d{1,2})?$";
                Regex rex = new Regex(temp);
                if (!rex.IsMatch(model.Price.ToString()))
                {
                    res.Msg = "单价请保留两位小数";
                    res.IsSuccess = false;
                    return res;
                }
            }

            if (string.IsNullOrEmpty(model.ChargeFormulaShow))
            {
                res.Msg = "公式名称不能为空!";
                res.IsSuccess = false;
                return res;
            }
            if (string.IsNullOrEmpty(model.ChargeFormula))
            {
                res.Msg = "计算公式不能为空!";
                res.IsSuccess = false;
                return res;
            }

            if (model.AutomaticBill == (int)AutomaticBillEnum.AutomaticBillEnumY)
            {
                if (string.IsNullOrEmpty(model.BillDay.ToString()))
                {
                    res.Msg = "账单日不能为空!";
                    res.IsSuccess = false;
                    return res;
                }
                else
                {

                    if (!(model.BillDay >= 1 && model.BillDay <= 31))
                    {
                        res.Msg = "请正确填写账单日[1,26]";
                        res.IsSuccess = false;
                        return res;
                    }
                }
            }

            return res;
        }
        #endregion

        #region 构建查询对象参数
        /// <summary>
        /// 构建查询对象参数
        /// </summary>
        /// <param name="queryParams"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>

        public ChargeSubjectDTO GetSearchParms(DTParameterModel queryParams, out int pageIndex)
        {

            ChargeSubjectDTO model = new ChargeSubjectDTO();
            string searchParm = queryParams.CustomSearch;
            pageIndex = (queryParams.Start % queryParams.Length) + 1;
            Type type = model.GetType();
            List<SearchParm> searchParms = JsonConvert.DeserializeObject<List<SearchParm>>(queryParams.CustomSearch);
            foreach (var item in searchParms)
            {
                if (item.name == "DeptId")
                {
                    item.name = "ComDeptId";
                }
                foreach (PropertyInfo p in type.GetProperties())
                {
                    if (p.Name == item.name)
                    {
                        if (!string.IsNullOrEmpty(item.value.ToString()))
                        {
                            if (!p.PropertyType.IsGenericType)
                            {
                                /*非泛型*/
                                p.SetValue(model, string.IsNullOrEmpty(item.value) ? null : Convert.ChangeType(item.value, p.PropertyType), null);
                            }
                            else
                            {
                                /*泛型Nullable<>*/
                                Type genericTypeDefinition = p.PropertyType.GetGenericTypeDefinition();
                                if (genericTypeDefinition == typeof(Nullable<>))
                                {
                                    p.SetValue(model, string.IsNullOrEmpty(item.value) ? null : Convert.ChangeType(item.value, Nullable.GetUnderlyingType(p.PropertyType)), null);
                                }
                            }
                        }
                    }
                }
            }
            return model;
        }
        #endregion

        #region 获取字典item信息
        /// <summary>
        /// 获取字典item信息
        /// </summary>
        /// <param name="dictionaryId">字典的外键Id</param>
        /// <returns></returns>
        public List<DictionaryModel> GetDictionaryModels(int dictionaryId)
        {
            List<DictionaryModel> dicModels = new List<DictionaryModel>();
            dicModels = PresentationServiceHelper.LookUp<IPropertyService>().GetDictionaryModels(dictionaryId);
            return dicModels;
        }
        #endregion

        #region 获取对象分页集合
        public IList<ChargeSubjectDTO> Paging(List<int?> listIds, int PageIndex, int PageSize, ChargeSubjectDTO subject, string expressions, out int totalCount)
        {

            ChargeSubject model = ChargeSubjectMappers.ChangeDTOToChargeSubjectNew(subject);
            IList<ChargeSubject> dataList = null;
            Condition<ChargeSubject> condition = new Condition<ChargeSubject>(o => true && o.SubjectType != (int)(SubjectTypeEnum.SystemPreset));

            if (model.IsDel.HasValue)
            {
                condition = condition & new Condition<ChargeSubject>(o => o.IsDel == model.IsDel);
            }

            if (!string.IsNullOrEmpty(model.Name))
            {
                condition = condition & new Condition<ChargeSubject>(o => o.Name.Contains(model.Name));
            }
            if (!string.IsNullOrEmpty(model.Code))
            {
                condition = condition & new Condition<ChargeSubject>(o => o.Code.Contains(model.Code));
            }

            if (model.ComDeptId != 3)
            {
                condition = condition & new Condition<ChargeSubject>(o => o.ComDeptId == model.ComDeptId);
            }
            else
            {
                condition = condition & new Condition<ChargeSubject>(o => listIds.Contains(o.ComDeptId));
            }
            dataList = ChargeSubjectService.Paging(PageIndex, PageSize, condition.ExpressionBody, expressions, out totalCount)
                .OrderByDescending(o => o.CreateTime).ToList();//修改按创建时间倒序排  2017-9-8
            dataList = dataList.Select(o => new ChargeSubject()
            {
                Id = o.Id,
                Code = o.Code,
                Name = o.Name,
                BillPeriod = o.BillPeriod,
                Price = o.Price,
                ChargeFormula = o.ChargeFormula,
                ChargeFormulaShow = o.ChargeFormulaShow,
                PenaltyRate = o.PenaltyRate,
                BillDay = o.BillDay,
                IsOnline = o.IsOnline,
                IsDel = Convert.ToBoolean(o.IsDel) ? false : true,/*是否启用标识*/
                ComDeptId = o.ComDeptId,
                SubjectType = o.SubjectType,
                BeginDate = o.BeginDate,
                Remark = o.Remark,
                AutomaticBill = o.AutomaticBill
            }).ToList();
            return ChargeSubjectMappers.ChangeChargeSubjectToDTOs(dataList).ToList();
        }
        #endregion

        #region 获取一个账户下面的收费项目

        /// <summary>
        /// 根据房屋HouseDeptId获取ChargeSubject集合
        /// </summary>
        public List<ChargeSubjectDTO> GetChargeSubjectListByHouseDeptId(int HouseDeptId,int DeptTypeId)
        {

            switch (DeptTypeId)
            {
                case (int)EDeptType.FangWu:
                    var domainList = ChargeSubjectService.GetChargeSubjectListByHouseDeptId(HouseDeptId);
                    return ChargeSubjectMappers.ChangeChargeSubjectToDTOs(domainList);
                case (int)EDeptType.CheWei:
                    var domainCarList = ChargeSubjectService.GetChargeSubjectListByResourcesId(HouseDeptId);
                    return ChargeSubjectMappers.ChangeChargeSubjectToDTOs(domainCarList);
                default:
                    return new List<ChargeSubjectDTO>();
            }
         
        
        }

        #endregion


        #region 获取一个账户下面的收费项目(周期)

        /// <summary>
        /// 根据房屋HouseDeptId获取ChargeSubject集合(周期收费)
        /// </summary>
        public List<ChargeSubjectDTO> GetCycleChargeSubjectListByHouseDeptId(int DeptId,int DeptTypeId)
        {
            List<ChargeSubject> domainList = new List<ChargeSubject>();

            switch (DeptTypeId)
            {
                case (int)EDeptType.FangWu:
                    { 
                        domainList = ChargeSubjectService.GetChargeSubjectListByHouseDeptId(DeptId)
                            .Where(o => o.BillPeriod == BillPeriodEnum.DailyCharge.GetHashCode() 
                            || o.BillPeriod == BillPeriodEnum.MonthlyCharge.GetHashCode() 
                            || o.BillPeriod == BillPeriodEnum.MeterCharge.GetHashCode()).ToList();
                        return ChargeSubjectMappers.ChangeChargeSubjectToDTOs(domainList);
                    }
                case (int)EDeptType.CheWei:
                    { 
                        domainList = ChargeSubjectService.GetChargeSubjectListByResourcesId(DeptId)
                            .Where(o => o.BillPeriod == BillPeriodEnum.DailyCharge.GetHashCode() 
                            || o.BillPeriod == BillPeriodEnum.MonthlyCharge.GetHashCode() 
                            || o.BillPeriod == BillPeriodEnum.MeterCharge.GetHashCode()).ToList();
                        return ChargeSubjectMappers.ChangeChargeSubjectToDTOs(domainList);
                    }
                default:
                    return new List<ChargeSubjectDTO>();
            }
        }

        /// <summary>
        /// 根据房屋HouseDeptId获取ChargeSubject集合(周期收费)
        /// </summary>
        public List<CustomTreeNodeModel> GetChargeSubjectTreeByDeptId(int DeptId, int DeptTypeId, string DeptName)
        {
            switch (DeptTypeId)
            {
                case (int)EDeptType.FangWu:
                    {
                        return ChargeSubjectService.GetChargeSubjectTree(DeptId, DeptName);
                    }
                case (int)EDeptType.CheWei:
                    {
                        //domainList = ChargeSubjectService.GetChargeSubjectListByResourcesId(DeptId)
                        //    .Where(o => o.BillPeriod == BillPeriodEnum.DailyCharge.GetHashCode()
                        //    || o.BillPeriod == BillPeriodEnum.MonthlyCharge.GetHashCode()
                        //    || o.BillPeriod == BillPeriodEnum.MeterCharge.GetHashCode()).ToList();
                        //return ChargeSubjectMappers.ChangeChargeSubjectToDTOs(domainList);
                        return ChargeSubjectService.GetParkingSpaceChargeSubjectTree(DeptId);
                    }
                default:
                    return new List<CustomTreeNodeModel>();
            }
        }

        public bool CheckChargeSubject(int HouseDeptId,int DeptType)
        {
            int count = 0;
            switch (DeptType)
            {
                case (int)EDeptType.FangWu:
                   count = ChargeSubjectService.GetChargeSubjectListByHouseDeptId(HouseDeptId).Where(o => o.BillPeriod == BillPeriodEnum.DailyCharge.GetHashCode() || o.BillPeriod == BillPeriodEnum.MonthlyCharge.GetHashCode() || o.BillPeriod == BillPeriodEnum.MeterCharge.GetHashCode()).Count();
                    break;
                case (int)EDeptType.CheWei:
                    count = ChargeSubjectService.GetChargeSubjectListByResourcesId(HouseDeptId).Where(o => o.BillPeriod == BillPeriodEnum.DailyCharge.GetHashCode() || o.BillPeriod == BillPeriodEnum.MonthlyCharge.GetHashCode() || o.BillPeriod == BillPeriodEnum.MeterCharge.GetHashCode()).Count();
                    break;
            }
            
          
            return count > 0;
        }

        #endregion

        #region 获取APP 微信 平板下小区下的收费项目
        /// <summary>
        /// 获取小区下的科目信息
        /// </summary>
        /// <param name="communityDeptId">小区ID</param>
        /// <returns></returns>
        public IEnumerable<ChargeSubjectDTO> GetMobileChargeSubjectsByComDeptId(int communityDeptId)
        {
            return ChargeSubjectMappers.ChangeChargeSubjectToDTOs(ChargeSubjectService.GetChargeSubjectList(o => o.ComDeptId == communityDeptId && o.IsDel == false && o.IsOnline == true));
        }
        #endregion

    }
}
