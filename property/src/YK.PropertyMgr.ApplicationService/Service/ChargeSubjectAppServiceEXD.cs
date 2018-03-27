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
using YK.PropertyMgr.DomainService;

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
                    res.Msg = "处理失败!";
                    return res;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Msg = "处理成功!";
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
                ChargeSubjectDTO modelUpdate = ChargeSubjectMappers.ChangeChargeSubjectToDTO(GetChargeSubjectSingle(o => o.Id == model.Id));
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

        #region 获取单个对象
        /// <summary>
        /// 获取单个ChargeSubject
        /// </summary>
        /// <returns></returns>
        public ChargeSubject GetChargeSubjectSingle(Expression<Func<ChargeSubject, bool>> where)
        {
            return ChargeSubjectService.GetChargeSubjectSingle(where);
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
                string temp = @"(^(\d(\.\d{2})?){1}$)";
                Regex rex = new Regex(temp);
                if (!rex.IsMatch(model.Price.ToString()))
                {
                    res.Msg = "单价请保留两位小数";
                    res.IsSuccess = false;
                    return res;
                }
            }

            if (string.IsNullOrEmpty(model.ChargeFormula))
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
                    res.Msg = "请正确填写账单日[1,31]";
                    res.IsSuccess = false;
                    return res;
                }
            }
            return res;
        }
        #endregion

        #region 获取对象分页集合
        public IList<ChargeSubjectDTO> Paging(int PageIndex, int PageSize, ChargeSubjectDTO chargeSubjectDTO, string expressions, out int totalCount)
        {
            var dataList = ChargeSubjectService.Paging(PageIndex, PageSize, c => c.Name.Contains(chargeSubjectDTO.Name), expressions, out totalCount);
            return ChargeSubjectMappers.ChangeChargeSubjectToDTOs(dataList).ToList();
        }
        #endregion
    }
}
