using System;
using System.Collections.Generic;
using System.Linq;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.DomainEntity;


namespace YK.PropertyMgr.ApplicationService
{
    public partial class SubjectHouseRefAppService
    {
        #region 新增绑定收费项目
        /// <summary>
        /// 绑定收费项目
        /// </summary>
        /// <param name="model">绑定收费项目</param>
        /// <returns></returns>
        public ReturnResult InsertSubjectHouseRefCus(SubjectHouseRefDTO model)
        {
            ReturnResult res = new ReturnResult();
            try
            {
                res = Validation(model);
                if (!res.IsSuccess)
                {
                    return res;
                }
                bool isSuccess = InsertSubjectHouseRef(model);
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

        #region 更新绑定收费项目
        /// <summary>
        /// 绑定收费项目
        /// </summary>
        /// <param name="model">绑定收费项目</param>
        /// <returns></returns>
        public ReturnResult UpdateSubjectHouseRefCus(SubjectHouseRefDTO model)
        {
            ReturnResult res = new ReturnResult();
            try
            {
                res = Validation(model);
                if (!res.IsSuccess)
                {
                    return res;
                }
                bool isSuccess = UpdateSubjectHouseRef(model);
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

        #region 数据校验

        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnResult Validation(SubjectHouseRefDTO model)
        {

            ReturnResult res = new ReturnResult()
            {
                IsSuccess = true
            };
            if (string.IsNullOrEmpty(model.ChargeSubjecId.ToString()) || !(model.ChargeSubjecId > 0))
            {
                res.Msg = "科目信息不能为空!";
                res.IsSuccess = false;
                return res;
            }
            //if (string.IsNullOrEmpty(model.RefType.ToString()) || !(model.RefType > 0))
            //{
            //    res.Msg = "关联类型不能为空!";
            //    res.IsSuccess = false;
            //    return res;
            //}
            if (string.IsNullOrEmpty(model.ResourcesId.ToString()) || !(model.ResourcesId > 0))
            {
                res.Msg = "资源信息不能为空!";
                res.IsSuccess = false;
                return res;
            }

            if (string.IsNullOrEmpty(model.DevBeginDate.ToString()) || string.IsNullOrEmpty(model.DevEndDate.ToString()))
            {
                if (model.IsDevPay)
                {
                    res.Msg = "开发商代缴时间不能为空!";
                    res.IsSuccess = false;
                    return res;
                }
            }
            else
            {
                if (model.DevBeginDate > model.DevEndDate)
                {
                    res.Msg = "代缴初始时间必须大于结束时间!";
                    res.IsSuccess = false;
                    return res;
                }
            }
            return res;
        }
        #endregion

        #region 获取对象分页集合
        public IList<SubjectHouseRefDTO> Paging(int PageIndex, int PageSize, SubjectHouseRefDTO chargeSubjectDTO, string expressions, out int totalCount)
        {
            var dataList = SubjectHouseRefService.Paging(PageIndex, PageSize, c => c.ChargeSubjecId == chargeSubjectDTO.Id, expressions, out totalCount);
            return SubjectHouseRefMappers.ChangeSubjectHouseRefToDTOs(dataList).ToList();
        }
        #endregion
    }
}
