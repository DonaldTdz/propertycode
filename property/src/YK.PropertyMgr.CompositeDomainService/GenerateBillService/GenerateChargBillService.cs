using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.RepositoryContract;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.CompositeDomainService.GenerateBillService.Interface;
using System.Data;
using System.Timers;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.BackgroundMgr.Common;
using YK.BackgroundMgr.CompositeAppService;
using YK.BackgroundMgr.DomainCompositeService;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;

namespace YK.PropertyMgr.CompositeDomainService.GenerateBillService
{
    /// <summary>
    /// 生成账单服务
    /// </summary>
    public class GenerateChargBillService : IGenerateChargBill
    {
        #region 单例

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static GenerateChargBillService Instance { get { return SingletonInstance; } }

        private static readonly GenerateChargBillService SingletonInstance = new GenerateChargBillService();

        #endregion

        #region 私有方法和属性

        Timer myTimer;//自动生成账单 时间戳
        int Hour;
        int Minute;
        DateTime StartDate = DateTime.Now;
        bool IsTaskRun = true;//是否启用异步任务
        //最后一次执行生成账单的时间
        DateTime LastExecutionTime = DateTime.Today.AddDays(-1);

        #region 日期范围

        /// <summary>
        /// 获取生成账单的时间范围列表
        /// 以账单日为分隔日期
        ///     例：账单日为25，账单生成周期为：1月25 - 2月24
        /// </summary>
        /// <param name="billBeginDate">账单开始日期</param>
        /// <param name="BillDay">账单日</param>
        /// <returns>账单时间范围列表</returns>
        private IList<BillDateRange> GetBillDateRangeList(DateTime billBeginDate, BillPeriodEnum billPeriod, int BillDay)
        {
            //去掉 时 分 秒
            billBeginDate = DateTime.Parse(billBeginDate.ToShortDateString());
            IList<BillDateRange> rangeList = new List<BillDateRange>();
            //获取当前日
            DateTime currentDate = DateTime.Today;
            //账单最后开始日 默认先取当前月为最后账单日
            DateTime lastBeginDate = new DateTime(currentDate.Year, currentDate.Month, BillDay);
            //如果当前月账单日还未到 就取上个月为最后账单日
            if (currentDate.Day < BillDay)
            {
                lastBeginDate = lastBeginDate.AddMonths(-1);
            }
            //如果是月账单 开始时间为这个月1号
            if (billPeriod == BillPeriodEnum.MonthlyCharge)
            {
                billBeginDate = new DateTime(billBeginDate.Year, billBeginDate.Month, 1);
            }
            //如果开始日期 大于 最后账单开始日 则不需要生成账单
            if (billBeginDate.Month > lastBeginDate.Month)
            {
                return rangeList;
            }
            //账单结束日 默认先取账单开始日期月
            DateTime endDate = (new DateTime(billBeginDate.Year, billBeginDate.Month, BillDay)).AddDays(-1);//注意：处理边界值
            //如果开始日大于等于账单日 需要结束日期后推一个月
            if (billBeginDate.Day >= BillDay)
            {
                endDate = (new DateTime(billBeginDate.Year, billBeginDate.Month, BillDay)).AddMonths(1).AddDays(-1);//注意：处理边界值
            }
            rangeList.Add(new BillDateRange()
            {
                BeginDate = billBeginDate,
                EndDate = endDate
            });
            //下一个开始 和 结束时间
            DateTime beginDate = endDate.AddDays(1);
            endDate = beginDate.AddMonths(1).AddDays(-1);
            while (beginDate <= lastBeginDate)
            {
                rangeList.Add(new BillDateRange()
                {
                    BeginDate = beginDate,
                    EndDate = endDate
                });
                beginDate = beginDate.AddMonths(1);
                endDate = beginDate.AddMonths(1).AddDays(-1);
            }

            return rangeList;
        }

        /// <summary>
        /// 获取生成账单的时间范围列表
        /// 以整月为账单分隔日
        ///   例：1月1日 - 1月31日
        /// </summary>
        /// <param name="billBeginDate">账单开始日期</param>
        /// <returns>账单时间范围列表</returns>
        private IList<BillDateRange> GetBillDateRangeList(DateTime billBeginDate, BillPeriodEnum billPeriod, DateTime EndDate)
        {
            //return GetBillDateRangeList(billBeginDate,billPeriod,1);
            //去掉 时 分 秒
            billBeginDate = DateTime.Parse(billBeginDate.ToShortDateString());
            IList<BillDateRange> rangeList = new List<BillDateRange>();
            //获取当前日
            //DateTime currentDate = DateTime.Today;
            //账单最后开始日
            DateTime lastBeginDate = new DateTime(EndDate.Year, EndDate.Month, 1);

            //如果是月账单 开始时间为这个月1号
            if (billPeriod == BillPeriodEnum.MonthlyCharge)
            {
                billBeginDate = new DateTime(billBeginDate.Year, billBeginDate.Month, 1);
            }
            //如果开始日期月大于 最后账单开始月 则不需要生成账单
            if (billBeginDate.Year > lastBeginDate.Year || (billBeginDate.Month > lastBeginDate.Month && billBeginDate.Year == lastBeginDate.Year))
            {
                return rangeList;
            }
            //账单结束日
            DateTime endDate = (new DateTime(billBeginDate.Year, billBeginDate.Month, 1)).AddMonths(1).AddDays(-1);//注意：处理边界值
            rangeList.Add(new BillDateRange()
            {
                BeginDate = billBeginDate,
                EndDate = endDate
            });
            //下一个开始 和 结束时间
            DateTime beginDate = endDate.AddDays(1);
            endDate = beginDate.AddMonths(1).AddDays(-1);
            while (beginDate <= lastBeginDate)
            {
                rangeList.Add(new BillDateRange()
                {
                    BeginDate = beginDate,
                    EndDate = endDate
                });
                beginDate = beginDate.AddMonths(1);
                endDate = beginDate.AddMonths(1).AddDays(-1);
            }

            //如果是每日计费 需要处理最后一个月结束时间 修改：2016-12-26
            int count = rangeList.Count();
            if (count > 0 && billPeriod == BillPeriodEnum.DailyCharge)
            {
                var endRange = rangeList[count - 1];
                //开始时间比结束时间大 则不需要生成
                if (endRange.BeginDate > EndDate)
                {
                    rangeList.Remove(endRange);
                }
                else
                {
                    if (endRange.EndDate > EndDate)
                    {
                        rangeList[count - 1].EndDate = EndDate;
                    }
                }
            }

            return rangeList;
        }

        #endregion

        #region 拆分账单

        /// <summary>
        /// 按天计费 开发商代缴结束日期为月中 需要拆分账单
        /// </summary>
        /// <returns>账单日期范围集合</returns>
        private IList<BillDateRange> SplitBill(SubjectHouseRef K, BillDateRange B)
        {
            IList<BillDateRange> bdRangeList = new List<BillDateRange>();
            //1.账单日期在开发商代缴范围内 返回一个账单
            if (K.DevBeginDate <= B.BeginDate && B.EndDate <= K.DevEndDate)
            {
                B.IsDevPay = true;
                bdRangeList.Add(B);
            }
            //2.开发商代缴开始时间在账单里 拆分为两个账单 或 一个
            else if (K.DevBeginDate >= B.BeginDate && K.DevBeginDate <= B.EndDate && K.DevEndDate > B.EndDate)
            {
                if (K.DevBeginDate == B.BeginDate)
                {
                    B.IsDevPay = true;
                    bdRangeList.Add(B);
                }
                else
                {
                    //业主账单
                    bdRangeList.Add(new BillDateRange()
                    {
                        BeginDate = B.BeginDate,
                        EndDate = K.DevBeginDate.Value.AddDays(-1),
                        IsDevPay = false
                    });
                    //开发商账单
                    bdRangeList.Add(new BillDateRange()
                    {
                        BeginDate = K.DevBeginDate.Value,
                        EndDate = B.EndDate,
                        IsDevPay = true
                    });
                }
            }
            //3. 开发商代缴结束时间在账单里 拆分为两个账单  或 一个
            else if (K.DevBeginDate < B.BeginDate && K.DevEndDate >= B.BeginDate && K.DevEndDate <= B.EndDate)
            {
                if (K.DevEndDate == B.EndDate)
                {
                    B.IsDevPay = true;
                    bdRangeList.Add(B);
                }
                else
                {
                    //开发商账单
                    bdRangeList.Add(new BillDateRange()
                    {
                        BeginDate = B.BeginDate,
                        EndDate = K.DevEndDate.Value,
                        IsDevPay = true
                    });
                    //业主账单
                    bdRangeList.Add(new BillDateRange()
                    {
                        BeginDate = K.DevEndDate.Value.AddDays(1),
                        EndDate = B.EndDate,
                        IsDevPay = false
                    });
                }
            }
            //4.开发商代缴时间在整个账单里 拆分为三个账单 或 两个
            else if (B.BeginDate <= K.DevBeginDate && K.DevEndDate <= B.EndDate)
            {
                if (B.BeginDate == K.DevBeginDate)
                {
                    //开发商账单
                    bdRangeList.Add(new BillDateRange()
                    {
                        BeginDate = B.BeginDate,
                        EndDate = K.DevEndDate.Value,
                        IsDevPay = true
                    });
                    //业主账单
                    bdRangeList.Add(new BillDateRange()
                    {
                        BeginDate = K.DevEndDate.Value.AddDays(1),
                        EndDate = B.EndDate,
                        IsDevPay = false
                    });
                }
                else if (B.EndDate == K.DevEndDate)
                {
                    //业主账单
                    bdRangeList.Add(new BillDateRange()
                    {
                        BeginDate = B.BeginDate,
                        EndDate = K.DevBeginDate.Value.AddDays(-1),
                        IsDevPay = false
                    });
                    //开发商账单
                    bdRangeList.Add(new BillDateRange()
                    {
                        BeginDate = K.DevBeginDate.Value,
                        EndDate = B.EndDate,
                        IsDevPay = true
                    });
                }
                else
                {
                    //业主账单
                    bdRangeList.Add(new BillDateRange()
                    {
                        BeginDate = B.BeginDate,
                        EndDate = K.DevBeginDate.Value.AddDays(-1),
                        IsDevPay = false
                    });
                    //开发商账单
                    bdRangeList.Add(new BillDateRange()
                    {
                        BeginDate = K.DevBeginDate.Value,
                        EndDate = K.DevEndDate.Value,
                        IsDevPay = true
                    });
                    //业主账单
                    bdRangeList.Add(new BillDateRange()
                    {
                        BeginDate = K.DevEndDate.Value.AddDays(1),
                        EndDate = B.EndDate,
                        IsDevPay = false
                    });
                }
            }

            return bdRangeList;
        }

        #endregion

        #region 计费金额
        //移至公共接口
        /*
        /// <summary>
        /// 计费金额
        /// </summary>
        /// <param name="chargeSubject">收费项目</param>
        /// <param name="calculateProperty">计费属性</param>
        /// <returns>计费金额</returns>
        private decimal CalculateAmount(ChargeSubject chargeSubject, ICalculateProperty calculateProperty)
        {
            if (calculateProperty == null)
            {
                return 0;
            }
            string resultFormula = chargeSubject.ChargeFormula;
            //替换单价
            resultFormula = resultFormula.Replace(CalculatePropertyHelper.GetChargeFormulaProperty(ChargeFormulaEnum.Price), chargeSubject.Price.ToString());
            //替换计算对象属性值
            if (calculateProperty.Properties != null)
            {
                foreach (var item in calculateProperty.Properties)
                {
                    resultFormula = resultFormula.Replace(CalculatePropertyHelper.GetChargeFormulaProperty(item.Key), item.Value);
                }
            }

            //替换掉多余的逗号
            resultFormula = resultFormula.Replace(",", "");
            try
            {
                DataTable dt = new DataTable();
                //公式计算
                decimal result = Math.Round(decimal.Parse(dt.Compute(resultFormula, "false").ToString()), 2);
                return result;
            }
            catch (Exception ex)
            {
                //记录错误日志
                LogProperty.WriteLoginToFile(string.Format("[计费金额]Code:702 ChargeSubjectId:{0} Formula:{1} ErrorMsg:{2}", chargeSubject.Id, resultFormula, ex.Message), "GenerateChargBill", FileLogType.Exception);
                return 0;
            }
        }
        */
        #endregion

        #region 自动划账

        /// <summary>
        /// 预付款自动划账
        /// 修改:2017-04-12
        /// 用户预存费余额即存在“全部收费项目”，又存在单独收费项目的预存余额时，首先抵扣收费项目对应的预存费，再抵扣“全部收费项目”
        /// </summary>
        /// <param name="propertyMgrUnitOfWork">工作单元</param>
        /// <param name="chargBill">账单</param>
        /// <param name="deductionAmount">应该付金额</param>
        /// <param name="ChargeRecordId">费用流水ID</param>
        /// <returns>返回已划账金额</returns>
        private decimal AutomaticDeduction(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, ChargBill chargBill, decimal deductionAmount, string ChargeRecordId)
        {
            return BillCommonService.Instance.AutomaticDeduction(propertyMgrUnitOfWork, chargBill, deductionAmount, ChargeRecordId).TotalDeductionAmount;
        }

        #endregion

        #region 收费流水

        /// <summary>
        /// 生成收费流水
        /// </summary>
        /// <param name="pmUnitWork">工作单元</param>
        /// <param name="chargBill">账单</param>
        /// <param name="ChargeRecordId">收费流水ID</param>
        private void GenerateChargeRecord(IPropertyMgrUnitOfWork pmUnitWork, ChargBill chargBill, string ChargeRecordId, string Remark)
        {
            //如果已划转金额为0，不需要生成流水
            if (chargBill.ReceivedAmount == 0)
            {
                return;
            }
            BillCommonService.Instance.GenerateChargeRecordByBill(pmUnitWork, chargBill, ChargeRecordId, chargBill.ReceivedAmount, Remark);
        }

        #endregion 

        #region 生成账单

        /// <summary>
        /// 生成收费项目下 单个账单
        /// </summary>
        /// <param name="pmUnitWork">工作单元</param>
        /// <param name="chargeSubject">收费项目</param>
        /// <param name="ditem">收费周期</param>
        /// <param name="calculateProperty">计算属性</param>
        /// <param name="subjectHouseItem">账号绑定信息</param>
        ///  <returns>账单ID</returns>
        private string GenerateSingleChargBill(ChargeSubject chargeSubject, BillDateRange ditem, ICalculateProperty calculateProperty, SubjectHouseRef subjectHouseItem, CommunityConfigDTO config,
            IPropertyMgrUnitOfWork pmUnitWork = null, bool IsCommit = true)
        {
            try
            {
                if (pmUnitWork == null)
                {
                    pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>();
                }

                //账单周期
                BillPeriodEnum billPeriod = (BillPeriodEnum)chargeSubject.BillPeriod;
                //本次生成账单
                ChargBill chargBill = new ChargBill();
                chargBill.ChargeSubjectId = chargeSubject.Id;
                chargBill.IsDel = false;
                chargBill.Description = chargeSubject.Name;
                chargBill.Id = Guid.NewGuid().ToString();
                chargBill.ComDeptId = calculateProperty.ComDeptId;
                chargBill.HouseDeptId = calculateProperty.HouseDeptID;
                chargBill.ResourcesId = calculateProperty.ResourcesId;
                //添加资源名称 修改：2017-01-13
                chargBill.ResourcesName = calculateProperty.ResourcesName;
                //添加房屋编号 修改: 2017-06-29
                chargBill.HouseDoorNo = calculateProperty.HouseDoorNo;
                chargBill.Remark = "";
                chargBill.RefType = chargeSubject.SubjectType;
                chargBill.UpdateTime = DateTime.Now;
                chargBill.CreateTime = DateTime.Now;

                //开发商代缴 月费用 三表账单取交集
                chargBill.IsDevPay = ditem.IsDevPay;

                string quantity = string.Empty;
                //计算账单计费金额
                decimal amount = BillCommonService.Instance.CalculateAmount(chargeSubject, calculateProperty, ref quantity);

                //如果是三表账单
                if (chargeSubject.SubjectType == SubjectTypeEnum.Meter.GetHashCode())
                {
                    chargBill.BeginDate = ditem.BeginDate;
                    chargBill.EndDate = ditem.EndDate;
                    //公区表规则处理 2017-7-24
                    if (calculateProperty.PublicAreaProperty.IsPublicAreaMeter.HasValue && calculateProperty.PublicAreaProperty.IsPublicAreaMeter.Value)
                    {
                        switch (calculateProperty.PublicAreaProperty.AllocationType)
                        {
                            //按房屋数分摊
                            case AllocationTypeEnum.HouseNumber:
                                {
                                    //房屋数量要大于0
                                    if (calculateProperty.PublicAreaProperty.HouseNumber > 0)
                                    {
                                        chargBill.BillAmount = Math.Round(amount / calculateProperty.PublicAreaProperty.HouseNumber.Value, 2);
                                        chargBill.Quantity = quantity + "/" + calculateProperty.PublicAreaProperty.HouseNumber.ToString();
                                        chargBill.Remark = calculateProperty.ExtendProperty.ToString() + "(房屋数分摊)";
                                    }
                                };
                                break;
                            //按建筑面积分摊
                            case AllocationTypeEnum.BuiltArea:
                                {
                                    //合计建筑面积和自身建筑面积要大于0
                                    if (calculateProperty.PublicAreaProperty.TotalBuildArea > 0 && calculateProperty.PublicAreaProperty.BuildArea > 0)
                                    {
                                        chargBill.BillAmount = Math.Round(amount *(calculateProperty.PublicAreaProperty.BuildArea.Value / calculateProperty.PublicAreaProperty.TotalBuildArea.Value), 2);
                                        chargBill.Quantity = quantity + "*(" + calculateProperty.PublicAreaProperty.BuildArea.ToString() +"/"+ calculateProperty.PublicAreaProperty.TotalBuildArea.ToString() + ")";
                                        chargBill.Remark = calculateProperty.ExtendProperty.ToString() + "(建筑面积分摊)";
                                    }
                                };
                                break;
                            default:
                                {
                                    chargBill.BillAmount = amount;
                                    chargBill.Quantity = quantity;
                                    chargBill.Remark = calculateProperty.ExtendProperty.ToString();
                                };
                                break;
                        }
                    }
                    else
                    {
                        chargBill.BillAmount = amount;
                        chargBill.Quantity = quantity;
                        chargBill.Remark = calculateProperty.ExtendProperty.ToString();
                    }       
                }
                else
                {
                    switch (billPeriod)
                    {
                        //天计费按月收取
                        case BillPeriodEnum.DailyCharge:
                            {
                                chargBill.BeginDate = ditem.BeginDate;
                                chargBill.EndDate = ditem.EndDate;
                                //如果账单开始时间不等于1日，为不足一个月，需要按天重新计算费用(主要情况是第一次生成按天计算账单，费用交接 在手动拆分账单处理)
                                decimal monthDay = (new DateTime(chargBill.EndDate.Value.Year, chargBill.EndDate.Value.Month, 1).AddMonths(1).AddDays(-1)).Day;  //计算月天
                                if (chargBill.BeginDate.Value.Day != 1 || chargBill.EndDate.Value.Day != monthDay)
                                {
                                    //计费天数
                                    decimal calculateDay = (chargBill.EndDate.Value.Day - chargBill.BeginDate.Value.Day + 1);
                                    //原计费金额/该月总天数*计费天数
                                    chargBill.BillAmount = Math.Round(amount / monthDay * calculateDay, 2);
                                    //数量拆分
                                    //chargBill.Quantity = Math.Round(quantity / monthDay * calculateDay, 2);
                                }
                                else
                                {
                                    chargBill.BillAmount = amount;
                                    chargBill.Quantity = quantity;
                                }

                            }
                            break;
                        //月计费按月收取
                        case BillPeriodEnum.MonthlyCharge:
                            {
                                chargBill.BeginDate = ditem.BeginDate;
                                chargBill.EndDate = ditem.EndDate;
                                chargBill.BillAmount = amount;
                                chargBill.Quantity = quantity;
                            }
                            break;
                            //三表
                            //case BillPeriodEnum.MeterCharge:
                            //    {
                            //        chargBill.BeginDate = ditem.BeginDate;
                            //        chargBill.EndDate = ditem.EndDate;
                            //        chargBill.BillAmount = amount;
                            //    }
                            //    break;
                    }
                }
                //减免金额 目前页面控制
                chargBill.ReliefAmount = 0;
                //滞纳金额 暂时为0 未处理
                chargBill.PenaltyAmount = chargeSubject.PenaltyRate * 0;
                //生成账户流水ID
                string ChargeRecordId = string.Empty;
                //配置预存费可抵扣 v2.9 2017-9-13
                if (config.IsPreAutomaticDeduction.Value)
                {
                    //配置合并收费记录 且操作是手动生成账单
                    if (config.IsPreMergeChargeRecord.Value && config.ActionType == ActionTypeEnum.ManualGenerationBill)
                    {
                        ChargeRecordId = config.ChargeRecordId;
                    }
                    else
                    {
                        ChargeRecordId = Guid.NewGuid().ToString();
                    }
                }

                //应划款金额=计费金额+滞纳金-减免金额
                decimal deductionAmount = chargBill.BillAmount.Value + chargBill.PenaltyAmount.Value - chargBill.ReliefAmount.Value;
                chargBill.ReceivedAmount = 0;
                if (deductionAmount <= 0)
                {
                    chargBill.Status = BillStatusEnum.Paid.GetHashCode();
                }
                else if (!chargBill.IsDevPay.HasValue || !chargBill.IsDevPay.Value)//开发商代缴 暂不支持预存划账
                {
                    //获取物业个性化配置 v2.9 2017-09-13
                    if (config.IsPreAutomaticDeduction.Value)
                    {
                        //预存划账 已交金额
                        chargBill.ReceivedAmount = AutomaticDeduction(pmUnitWork, chargBill, deductionAmount, ChargeRecordId);
                        if (deductionAmount == chargBill.ReceivedAmount)//如果已交完，更新账单状态
                        {
                            chargBill.Status = BillStatusEnum.Paid.GetHashCode();
                        }
                        else
                        {
                            chargBill.Status = BillStatusEnum.NoPayment.GetHashCode();
                        }
                    }
                    else
                    {
                        chargBill.Status = BillStatusEnum.NoPayment.GetHashCode();
                    }
                }
                else
                {
                    chargBill.Status = BillStatusEnum.NoPayment.GetHashCode();
                }
                //如果是车位 备注存储房屋编号 v2.7 6点 修改：2017-06-29
                if (chargeSubject.SubjectType == SubjectTypeEnum.ParkingSpace.GetHashCode())
                {
                    chargBill.Remark = chargBill.HouseDoorNo;
                }

                //配置预存费可抵扣 v2.9 2017-9-13
                if (config.IsPreAutomaticDeduction.Value)
                {
                    //配置合并收费记录 且操作是手动生成账单
                    if (config.IsPreMergeChargeRecord.Value && config.ActionType == ActionTypeEnum.ManualGenerationBill && chargBill.ReceivedAmount > 0)
                    {
                        config.ChargBillList.Add(chargBill);
                    }
                    else
                    {
                        //生成费用流水
                        GenerateChargeRecord(pmUnitWork, chargBill, ChargeRecordId, "");
                    }
                }
                
                //生成账单
                pmUnitWork.ChargBillRepository.Add(chargBill);
                //生成快照
                BillCommonService.Instance.GenerateChargeSubjectSna(pmUnitWork, chargBill, chargeSubject);
                //提交修改
                if (IsCommit)
                {
                    pmUnitWork.Commit();
                }

                //记录log
                Task.Run(() =>
                {
                    LogProperty.WriteLoginToFile(string.Format("[生成账单]Code:0 ChargeSubjectId:{0} ChargBillId:{1}", chargeSubject.Id, chargBill.Id), "GenerateChargBill", FileLogType.Info);
                });
                return chargBill.Id;
            }
            catch (Exception ex)
            {
                //记录异常log
                LogProperty.WriteLoginToFile(string.Format("[生成账单]Code:900 ChargeSubjectId:{0} ErrorMsg:{1}", chargeSubject.Id, ex.Message), "GenerateChargBill", FileLogType.Exception);
                return string.Empty;
            }
            finally
            {
                if (IsCommit)
                {
                    pmUnitWork.Dispose();
                }
            }
        }

        /// <summary>
        /// 生成收费项目账单 
        /// 对于物业管理费
        /// </summary>
        /// <param name="pmUnitWork">工作单元</param>
        /// <param name="chargeSubject">收费项目</param>
        /// <param name="propertyList">计算属性列表</param>
        private void GenerateHouseSubjectChargBill(IPropertyMgrUnitOfWork pmUnitWork, ChargeSubject chargeSubject
            , IList<CalculateProperty> propertyList, CommunityConfigDTO config, bool IsManual, int?[] resultIds = null, DateTime? EndDate = null)
        {
            var query = pmUnitWork.SubjectHouseRefRepository.GetAll()
                .Where(s => s.ChargeSubjecId == chargeSubject.Id && s.SubjectType == chargeSubject.SubjectType
                && s.IsDel == false);

            IList<SubjectHouseRef> subjectHouseList = new List<SubjectHouseRef>();
            if (resultIds == null)
            {
                subjectHouseList = query.ToList();
            }
            else
            {
                subjectHouseList = query.Where(q => resultIds.Contains(q.ResourcesId)).ToList();
            }

            //循环生成绑定账号的账单
            foreach (var subjectHouseItem in subjectHouseList)
            {
                //查询计算资源属性
                ICalculateProperty calculateProperty = propertyList
                                                        .Where(p => p.HouseDeptID == subjectHouseItem.HouseDeptId
                                                        && p.ResourcesId == subjectHouseItem.ResourcesId)
                                                        .FirstOrDefault();
                //资源找不到跳过生成账单 如果房屋状态为 未交房 不计费 修改：2016-11-22
                if (calculateProperty == null || calculateProperty.HouseStatus == HouseStatusEnum.NotSubmit)
                {
                    continue;
                }

                //自动生成 只能生成当月的账单 修改：2016-11-21
                //手动生成 需要生成连续账单 修改：2016-12-16
                DateTime? beginDate = BillCommonService.Instance.GetBillBeginDate(pmUnitWork, chargeSubject, subjectHouseItem, IsManual);
                if (!beginDate.HasValue)
                {
                    continue;
                }
                //确定账单生成开始时间
                DateTime billBeginDate = beginDate.Value;
                //确定结束时间，默认为当前月
                DateTime billEndDate = (new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)).AddMonths(1).AddDays(-1);
                if (IsManual && EndDate.HasValue)
                {
                    billEndDate = EndDate.Value;// new DateTime(EndDate.Value.Year, EndDate.Value.Month, 1);
                }
                //时间范围列表
                IList<BillDateRange> dateRangeList = GetBillDateRangeList(billBeginDate, (BillPeriodEnum)chargeSubject.BillPeriod, billEndDate);
                //循环生成该账号下的月账单
                foreach (var ditem in dateRangeList)
                {
                    //未售房：若绑定收费项目，则费用计入开发商（根据配置是否计入开发商) 修改：2016-11-15
                    if (calculateProperty.HouseStatus == HouseStatusEnum.Unsold
                    && calculateProperty.UnsoldIsBindDeveloper == true)
                    {
                        ditem.IsDevPay = true;
                        GenerateSingleChargBill(chargeSubject, ditem, calculateProperty, subjectHouseItem, config);
                    }
                    //未收房、已收房：若绑定收费项目，则费用计入业主 修改：2016-11-15
                    else if (calculateProperty.HouseStatus == HouseStatusEnum.NotReceive
                        || calculateProperty.HouseStatus == HouseStatusEnum.Received)
                    {
                        //开发商代缴
                        ditem.IsDevPay = BillCommonService.Instance.GetIsDevPay(subjectHouseItem, ditem);
                        if (ditem.IsDevPay && chargeSubject.BillPeriod == BillPeriodEnum.DailyCharge.GetHashCode())
                        {
                            IList<BillDateRange> bdRange = SplitBill(subjectHouseItem, ditem);
                            foreach (var bditem in bdRange)
                            {
                                //v2.9 如果允许自动预存抵扣 且 允许账单合并 2017-9-13
                                if (config.IsPreAutomaticDeduction.Value && config.IsPreMergeChargeRecord.Value && config.ActionType == ActionTypeEnum.ManualGenerationBill)
                                {
                                    GenerateSingleChargBill(chargeSubject, bditem, calculateProperty, subjectHouseItem, config, pmUnitWork, false);//外面统一生成收费记录再提交
                                }
                                else
                                {
                                    GenerateSingleChargBill(chargeSubject, bditem, calculateProperty, subjectHouseItem, config);
                                }
                            }
                        }
                        else
                        {
                            //v2.9 如果允许自动预存抵扣 且 允许账单合并 2017-9-13
                            if (config.IsPreAutomaticDeduction.Value && config.IsPreMergeChargeRecord.Value && config.ActionType == ActionTypeEnum.ManualGenerationBill)
                            {
                                GenerateSingleChargBill(chargeSubject, ditem, calculateProperty, subjectHouseItem, config, pmUnitWork, false);
                            }
                            else
                            {
                                GenerateSingleChargBill(chargeSubject, ditem, calculateProperty, subjectHouseItem, config);
                            }    
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 生成收费项目账单 
        /// 对于车位管理费
        /// </summary>
        /// <param name="pmUnitWork">工作单元</param>
        /// <param name="chargeSubject">收费项目</param>
        /// <param name="propertyList">计算属性列表</param>
        private void GenerateParkingSpaceSubjectChargBill(IPropertyMgrUnitOfWork pmUnitWork, ChargeSubject chargeSubject, IList<CalculateProperty> propertyList, CommunityConfigDTO config, 
            bool IsManual, int?[] resultIds = null, DateTime? EndDate = null)
        {
            var query = pmUnitWork.SubjectHouseRefRepository.GetAll()
               .Where(s => s.ChargeSubjecId == chargeSubject.Id && s.SubjectType == chargeSubject.SubjectType
               && s.IsDel == false);

            IList<SubjectHouseRef> subjectHouseList = new List<SubjectHouseRef>();
            if (resultIds == null)
            {
                subjectHouseList = query.ToList();
            }
            else
            {
                subjectHouseList = query.Where(q => resultIds.Contains(q.ResourcesId)).ToList();
            }

            //循环生成绑定账号的账单
            foreach (var subjectHouseItem in subjectHouseList)
            {
                //查询计算资源属性
                ICalculateProperty calculateProperty = propertyList
                                                        .Where(p =>
                                                        //p.HouseDeptID == subjectHouseItem.HouseDeptId && //修改：2016-11-15 只根据资源查找
                                                        p.ResourcesId == subjectHouseItem.ResourcesId)
                                                        .FirstOrDefault();
                //资源找不到跳过生成账单
                if (calculateProperty == null)
                {
                    continue;
                }

                //只能生成当月的账单 修改：2016-11-21
                DateTime? beginDate = BillCommonService.Instance.GetBillBeginDate(pmUnitWork, chargeSubject, subjectHouseItem, IsManual);
                if (!beginDate.HasValue)
                {
                    continue;
                }
                //确定账单生成开始时间
                DateTime billBeginDate = beginDate.Value;
                //确定结束时间，默认为当前月
                DateTime billEndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1);
                if (IsManual && EndDate.HasValue)
                {
                    billEndDate = EndDate.Value;//new DateTime(EndDate.Value.Year, EndDate.Value.Month, 1);
                }

                //时间范围列表
                IList<BillDateRange> dateRangeList = GetBillDateRangeList(billBeginDate, (BillPeriodEnum)chargeSubject.BillPeriod, billEndDate);
                //循环生成该账号下的月账单
                foreach (var ditem in dateRangeList)
                {
                    //开发商代缴
                    ditem.IsDevPay = BillCommonService.Instance.GetIsDevPay(subjectHouseItem, ditem);
                    //如果是每日计费 需要拆分账单
                    if (ditem.IsDevPay && chargeSubject.BillPeriod == BillPeriodEnum.DailyCharge.GetHashCode())
                    {
                        IList<BillDateRange> bdRange = SplitBill(subjectHouseItem, ditem);
                        foreach (var bditem in bdRange)
                        {
                            //v2.9 如果允许自动预存抵扣 且 允许账单合并 2017-9-13
                            if (config.IsPreAutomaticDeduction.Value && config.IsPreMergeChargeRecord.Value && config.ActionType == ActionTypeEnum.ManualGenerationBill)
                            {
                                GenerateSingleChargBill(chargeSubject, bditem, calculateProperty, subjectHouseItem, config, pmUnitWork, false);
                            }
                            else
                            {
                                GenerateSingleChargBill(chargeSubject, bditem, calculateProperty, subjectHouseItem, config);
                            }
                        }
                    }
                    else
                    {
                        //v2.9 如果允许自动预存抵扣 且 允许账单合并 2017-9-13
                        if (config.IsPreAutomaticDeduction.Value && config.IsPreMergeChargeRecord.Value && config.ActionType == ActionTypeEnum.ManualGenerationBill)
                        {
                            GenerateSingleChargBill(chargeSubject, ditem, calculateProperty, subjectHouseItem, config, pmUnitWork, false);
                        }
                        else
                        {
                            GenerateSingleChargBill(chargeSubject, ditem, calculateProperty, subjectHouseItem, config);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 生成收费项目账单 
        /// 对于其它周期收费项目
        /// </summary>
        /// <param name="pmUnitWork">工作单元</param>
        /// <param name="chargeSubject">收费项目</param>
        /// <param name="propertyList">计算属性列表</param>
        private void GenerateOtherSubjectChargBill(IPropertyMgrUnitOfWork pmUnitWork, IList<CalculateProperty> hmpList, ChargeSubject chargeSubject, CommunityConfigDTO config,
            bool IsManual, int?[] resultIds = null, DateTime? EndDate = null)
        {
            var query = pmUnitWork.SubjectHouseRefRepository.GetAll()
               .Where(s => s.ChargeSubjecId == chargeSubject.Id && s.SubjectType == chargeSubject.SubjectType
               && s.IsDel == false);

            IList<SubjectHouseRef> subjectHouseList = new List<SubjectHouseRef>();
            if (resultIds == null)
            {
                subjectHouseList = query.ToList();
            }
            else
            {
                subjectHouseList = query.Where(q => resultIds.Contains(q.ResourcesId)).ToList();
            }

            //循环生成绑定账号的账单
            foreach (var subjectHouseItem in subjectHouseList)
            {               
                //只能生成当月的账单 修改：2016-11-21
                DateTime? beginDate = BillCommonService.Instance.GetBillBeginDate(pmUnitWork, chargeSubject, subjectHouseItem, IsManual);
                if (!beginDate.HasValue)
                {
                    continue;
                }
                //确定账单生成开始时间
                DateTime billBeginDate = beginDate.Value;

                var property = hmpList.Where(h => h.HouseDeptID == subjectHouseItem.HouseDeptId).FirstOrDefault();
                if (property == null)
                {
                    property = new CalculateProperty()
                    {
                        ResourcesId = subjectHouseItem.ResourcesId.Value,
                        ComDeptId = chargeSubject.ComDeptId.Value,
                        HouseDeptID = subjectHouseItem.HouseDeptId.Value,
                        ResourcesName = subjectHouseItem.ResourceName
                    };
                }

                //确定结束时间，默认为当前月
                DateTime billEndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1);
                if (IsManual && EndDate.HasValue)
                {
                    billEndDate = EndDate.Value; //new DateTime(EndDate.Value.Year, EndDate.Value.Month, 1);
                }

                //时间范围列表
                IList<BillDateRange> dateRangeList = GetBillDateRangeList(billBeginDate, (BillPeriodEnum)chargeSubject.BillPeriod, billEndDate);
                //循环生成该账号下的月账单
                foreach (var ditem in dateRangeList)
                {
                    //开发商代缴
                    ditem.IsDevPay = BillCommonService.Instance.GetIsDevPay(subjectHouseItem, ditem);
                    //如果是每日计费 需要拆分账单
                    if (ditem.IsDevPay && chargeSubject.BillPeriod == BillPeriodEnum.DailyCharge.GetHashCode())
                    {
                        IList<BillDateRange> bdRange = SplitBill(subjectHouseItem, ditem);
                        foreach (var bditem in bdRange)
                        {
                            //v2.9 如果允许自动预存抵扣 且 允许账单合并 2017-9-13
                            if (config.IsPreAutomaticDeduction.Value && config.IsPreMergeChargeRecord.Value && config.ActionType == ActionTypeEnum.ManualGenerationBill)
                            {
                                GenerateSingleChargBill(chargeSubject, bditem, property, subjectHouseItem, config, pmUnitWork, false);
                            }
                            else
                            {
                                GenerateSingleChargBill(chargeSubject, bditem, property, subjectHouseItem, config);
                            }
                        }
                    }
                    else
                    {
                        //v2.9 如果允许自动预存抵扣 且 允许账单合并 2017-9-13
                        if (config.IsPreAutomaticDeduction.Value && config.IsPreMergeChargeRecord.Value && config.ActionType == ActionTypeEnum.ManualGenerationBill)
                        {
                            GenerateSingleChargBill(chargeSubject, ditem, property, subjectHouseItem, config, pmUnitWork, false);
                        }
                        else
                        {
                            GenerateSingleChargBill(chargeSubject, ditem, property, subjectHouseItem, config);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 生成三表收费项目账单
        /// 若本次抄表读数小于上次计费读数：
        ///1、本次读数+最大读数-上次读数 大于 最大读数/2，则账单为0；
        ///2、本次读数+最大读数-上次读数 小于 最大读数/2，则账单为 本次读数+最大读数-上次读数 对应的账单
        ///（如，最大读数9999.9，上次读数9010。  
        ///本次读数9000，9000+9999.9-9010 大于 9999.9/2，账单为0；
        ///本次读数100，100+9999.9-9010 小于 9999.9/2，账单为100+9999.9-9010对应的费用）
        /// </summary>
        /// <param name="pmUnitWork">工作单元</param>
        /// <param name="chargeSubject">收费项目</param>
        /// <param name="CommunityID">小区ID</param>
        private void GenerateMeterSubjectChargBill(IPropertyMgrUnitOfWork pmUnitWork, IList<CalculateProperty> hmpList, ChargeSubject chargeSubject, CommunityConfigDTO config,
            int CommunityID, int?[] resultIds = null)
        {
            var query = pmUnitWork.SubjectHouseRefRepository.GetAll()
               .Where(s => s.ChargeSubjecId == chargeSubject.Id && s.SubjectType == chargeSubject.SubjectType
               && s.IsDel == false);

            IList<SubjectHouseRef> subjectHouseList = new List<SubjectHouseRef>();
            if (resultIds == null)
            {
                subjectHouseList = query.ToList();
            }
            else
            {
                subjectHouseList = query.Where(q => resultIds.Contains(q.ResourcesId)).ToList();
            }

            //循环生成绑定账号的账单
            foreach (var subjectHouseItem in subjectHouseList)
            {
                //查询该账号下的三表
                var meterList = pmUnitWork
                    .MeterRepository
                    .GetAll()
                    .Where(m => m.IsEnabled == true && m.Id == subjectHouseItem.ResourcesId)
                    .ToList();

                foreach (var meterItem in meterList)
                {
                    using (var mrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                    {
                        //查询当前收费项目对应的最后一条记录
                        var lastReadRecord = mrUnitOfWork
                            .MeterReadRecordRepository
                            .GetAll()
                            .Where(r => r.MeterId == meterItem.Id)
                            .OrderByDescending(r => r.ReadDate)
                            .FirstOrDefault();
                        //如果为null继续循环
                        if (lastReadRecord == null)
                        {
                            continue;
                        }
                        //如果抄表日期相等 不需要生成账单
                        if (lastReadRecord.ReadDate.Value >= meterItem.ReadDate.Value)
                        {
                            continue;
                        }
                        BillDateRange bdr = new BillDateRange();
                        bdr.BeginDate = lastReadRecord.ReadDate.Value.AddDays(1);
                        bdr.EndDate = meterItem.ReadDate.Value;
                        bdr.IsDevPay = BillCommonService.Instance.GetIsDevPay(subjectHouseItem, bdr);
                        //读数值 = 抄表 - 上一次记录
                        decimal meterVal = meterItem.MeterValue.Value - lastReadRecord.MeterValue.Value;
                        //小于0表示超过最大值
                        if (meterVal < 0)
                        {
                            //读数值 = 本次读数+最大读数-上次读数
                            meterVal = meterItem.MeterValue.Value + meterItem.MaxValue.Value - lastReadRecord.MeterValue.Value;
                            //本次读数+最大读数-上次读数 大于 最大读数/2，则账单为0
                            if (meterVal > (meterItem.MaxValue / 2))
                            {
                                meterVal = 0;
                            }
                        }

                        CalculateProperty cp = new CalculateProperty();
                        cp.ComDeptId = CommunityID;
                        cp.HouseDeptID = meterItem.HouseDeptID??0;
                        cp.ResourcesId = meterItem.Id.Value;
                        cp.ResourcesName = meterItem.MeterNum;
                        cp.HouseDoorNo = CalculatePropertyHelper.GetHouseDoorNo(hmpList, meterItem.HouseDeptID);
                        MeterTypeEnum meterType = (MeterTypeEnum)meterItem.MeterType;
                        cp.Properties = CalculatePropertyHelper.GetMeterProperties(meterType, meterVal);
                        cp.ExtendProperty = "读数：" + lastReadRecord.MeterValue + "-" + meterItem.MeterValue;
                        //添加公区表属性 2017-7-24
                        if (meterItem.IsPublicArea.HasValue && meterItem.IsPublicArea.Value && !string.IsNullOrEmpty(meterItem.PublicAreaHouseDeptIDs))
                        {
                            cp.PublicAreaProperty.IsPublicAreaMeter = meterItem.IsPublicArea;
                            cp.PublicAreaProperty.AllocationType = (AllocationTypeEnum)meterItem.AllocationType;
                            int?[] houseDeptIds = Array.ConvertAll(meterItem.PublicAreaHouseDeptIDs.Split(','), t => (int?)int.Parse(t));
                            var publicAreaHouseList = hmpList.Where(h => houseDeptIds.Contains(h.HouseDeptID)).ToList();
                            //分摊房屋建筑面积合计
                            cp.PublicAreaProperty.TotalBuildArea = CalculatePropertyHelper.GetTotalHouseBuildArea(publicAreaHouseList);
                            //分摊房屋数
                            cp.PublicAreaProperty.HouseNumber = publicAreaHouseList.Count();
                            //生成到账单到分摊房屋下面
                            string billID = string.Empty;
                            //公区表分摊房屋列表
                            foreach (var item in publicAreaHouseList)
                            {
                                cp.HouseDeptID = item.HouseDeptID;
                                cp.HouseDoorNo = item.HouseDoorNo;
                                //分摊房屋建筑面积
                                var bArea = item.Properties[ChargeFormulaEnum.BuildArea];
                                if (!string.IsNullOrEmpty(bArea))
                                {
                                    cp.PublicAreaProperty.BuildArea = decimal.Parse(bArea);
                                }

                                //v2.9 如果允许自动预存抵扣 且 允许账单合并 2017-9-13
                                if (config.IsPreAutomaticDeduction.Value && config.IsPreMergeChargeRecord.Value && config.ActionType == ActionTypeEnum.ManualGenerationBill)
                                {
                                    //生成单个账单到分摊房屋
                                    billID += GenerateSingleChargBill(chargeSubject, bdr, cp, subjectHouseItem, config, pmUnitWork, false) + ";";
                                }
                                else
                                {
                                    //生成单个账单到分摊房屋
                                    billID += GenerateSingleChargBill(chargeSubject, bdr, cp, subjectHouseItem, config, mrUnitOfWork, false) + ";";
                                }
                                     
                            }

                            //v2.9 如果允许自动预存抵扣 且 允许账单合并 2017-9-13
                            if (config.IsPreAutomaticDeduction.Value && config.IsPreMergeChargeRecord.Value && config.ActionType == ActionTypeEnum.ManualGenerationBill)
                            {
                                //新增读数记录
                                BillCommonService.Instance.AddMeterReadRecord(pmUnitWork, meterItem, lastReadRecord, meterVal, billID);
                            }
                            else
                            {
                                //新增读数记录
                                BillCommonService.Instance.AddMeterReadRecord(mrUnitOfWork, meterItem, lastReadRecord, meterVal, billID);
                                //不等于空，生成账单成功
                                if (!string.IsNullOrEmpty(billID))
                                {
                                    //提交
                                    mrUnitOfWork.Commit();
                                }
                            }  
                        }
                        else
                        {
                            string billID = string.Empty;
                            //v2.9 如果允许自动预存抵扣 且 允许账单合并 2017-9-13
                            if (config.IsPreAutomaticDeduction.Value && config.IsPreMergeChargeRecord.Value && config.ActionType == ActionTypeEnum.ManualGenerationBill)
                            {
                                billID = GenerateSingleChargBill(chargeSubject, bdr, cp, subjectHouseItem, config, pmUnitWork, false);
                                //新增读数记录
                                BillCommonService.Instance.AddMeterReadRecord(pmUnitWork, meterItem, lastReadRecord, meterVal, billID);
                            }
                            else
                            {
                                billID = GenerateSingleChargBill(chargeSubject, bdr, cp, subjectHouseItem, config, mrUnitOfWork, false);
                                //新增读数记录
                                BillCommonService.Instance.AddMeterReadRecord(mrUnitOfWork, meterItem, lastReadRecord, meterVal, billID);
                                //不等于空，生成账单成功
                                if (!string.IsNullOrEmpty(billID))
                                {
                                    //提交
                                    mrUnitOfWork.Commit();
                                }
                            }
                        } 
                    }
                }
            }
        }

        #endregion

        #region 获取小区ID

        /// <summary>
        /// 获取需要生成账单的小区ID
        /// </summary>
        /// <param name="BillDay">账单日</param>
        /// <returns>小区ID集合</returns>
        private int?[] GetBillCommunityID(int BillDay = 0)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork
                        .ChargeSubjectRepository
                        .GetAll()
                        .Where(c => c.IsDel == false
                            && c.AutomaticBill == 1//判断是否配置为自动生成 1为自动生成
                                                   //&& (c.BillDay == BillDay || BillDay == 0)
                                                   //修改：2016-11-21 添加检查机制 如当天大于账单日的都需要检查生成
                            && (BillDay == 0 || c.BillDay == BillDay || DateTime.Today.Day > c.BillDay)
                            && (c.BillPeriod == EnumHelper.BillPeriod.DailyCharge
                            || c.BillPeriod == EnumHelper.BillPeriod.MonthlyCharge
                            || c.BillPeriod == EnumHelper.BillPeriod.MeterCharge
                            )
                            && c.BeginDate <= DateTime.Today
                            && c.SubjectType != EnumHelper.SubjectType.SystemPreset)
                        .Select(c => c.ComDeptId)
                        .Distinct()
                        .ToArray();
            }
        }

        #endregion

        #endregion

        #region IGenerateChargBill 接口实现

        #region 循环账单

        /// <summary>
        /// 生成指定小区账单
        /// 指定账单日：生成账单日收费项目账单 包括之前未生成的账单
        /// 不指定账单日：生成所有未生成的账单，所有账单日
        /// </summary>
        /// <param name="CommunityID">小区ID</param>
        /// <param name="BillDay">账单日 账单日=0时，为所有账单</param>
        public virtual void GenerateCommunityChargBill(int CommunityID, int BillDay = 0, int houseDeptId = 0)
        {
            if (BillDay < 0 || BillDay > 31)
            {
                return;
            }
            LogProperty.WriteLoginToFile(string.Format("[小区账单] CommunityID:{0} BillDay:{1} houseDeptId:{2}", CommunityID, BillDay, houseDeptId), "GenerateCommunityChargBill", FileLogType.Info);
            //获取计算数量，如房屋面积、停车位面积
            IList<CalculateProperty> hmpList = CalculatePropertyHelper.GetHouseCalculateProperty(CommunityID, houseDeptId);
            IList<CalculateProperty> pmpList = CalculatePropertyHelper.GetParkingSpaceCalculateProperty(CommunityID, hmpList, houseDeptId);

            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //获取满足条件的收费对象 三表和按月收费项目
                var chargeSubjectList = pmUnitWork
                    .ChargeSubjectRepository
                    .GetAll()
                    .Where(c => c.IsDel == false
                                && c.AutomaticBill == 1//判断是否配置为自动生成 1为自动生成
                                                       //等于当前账单日 或 所有账单日
                                                       //&& (c.BillDay == BillDay || BillDay == 0)
                                                       //修改：2016-11-21 添加检查机制 如当天大于账单日的都需要检查生成
                                && (BillDay == 0 || c.BillDay == BillDay || DateTime.Today.Day > c.BillDay)
                                && c.ComDeptId == CommunityID
                                && (c.BillPeriod == EnumHelper.BillPeriod.DailyCharge
                                    || c.BillPeriod == EnumHelper.BillPeriod.MonthlyCharge
                                    || c.BillPeriod == EnumHelper.BillPeriod.MeterCharge
                                )
                                && c.BeginDate <= DateTime.Today
                                && c.SubjectType != EnumHelper.SubjectType.SystemPreset)//排除预交费账单
                                .ToList();
                //获取物业小区个性化配置 2017-9-13
                var config = CommunityConfigMappers.ChangeCommunityConfigToDTO(BillCommonService.Instance.GetCommunityConfig(pmUnitWork, CommunityID));

                //循环生成账单
                foreach (var item in chargeSubjectList)
                {
                    SubjectTypeEnum subjectType = (SubjectTypeEnum)item.SubjectType;
                    switch (subjectType)
                    {
                        case SubjectTypeEnum.House://房屋相关
                            {
                                int?[] resultIds = null;
                                if (houseDeptId != 0)
                                {
                                    resultIds = new int?[] { houseDeptId };
                                }
                                GenerateHouseSubjectChargBill(pmUnitWork, item, hmpList, config, false, resultIds);
                            }
                            break;
                        case SubjectTypeEnum.ParkingSpace://车位相关
                            {
                                int?[] resultIds = null;
                                if (houseDeptId != 0)
                                {
                                    resultIds = pmpList.Where(p => p.HouseDeptID == houseDeptId).Select(p => (int?)p.ResourcesId).ToArray();
                                }
                                GenerateParkingSpaceSubjectChargBill(pmUnitWork, item, pmpList, config, false, resultIds);
                            }
                            break;
                        case SubjectTypeEnum.Meter://三表相关
                            {
                                int?[] resultIds = null;
                                if (houseDeptId != 0)
                                {
                                    resultIds = pmUnitWork.MeterRepository.GetAll()
                                        .Where(m => m.IsEnabled == true && m.HouseDeptID == houseDeptId).Select(m => m.Id).ToArray();
                                }
                                GenerateMeterSubjectChargBill(pmUnitWork, hmpList, item, config, CommunityID, resultIds);
                            }
                            break;
                        case SubjectTypeEnum.Other://其它 不需要其它计费参数
                            {
                                int?[] resultIds = null;
                                if (houseDeptId != 0)
                                {
                                    resultIds = new int?[] { houseDeptId };
                                }
                                GenerateOtherSubjectChargBill(pmUnitWork, hmpList, item, config, false, resultIds);
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 生成所有账单
        /// </summary>
        /// <param name="BillDay">账单日</param>
        public virtual void GenerateAllChargBill(int BillDay = 0, bool IsTaskRun = true)
        {
            //查询需要生成账单的小区
            int?[] cArray = GetBillCommunityID(BillDay);
            foreach (var item in cArray)
            {
                if (IsTaskRun)
                {
                    Task.Run(() =>
                    {
                        RegistService();//异步任务需要重新注册服务
                        GenerateCommunityChargBill(item.Value, BillDay);
                    });
                }
                else
                {
                    GenerateCommunityChargBill(item.Value, BillDay);
                }
            }
        }


        /// <summary>
        /// 自动循环生成账单
        /// 默认每天循环 时间指定
        /// </summary>
        /// <param name="StartDate">开始日期</param>
        /// <param name="Hour">小时</param>
        /// <param name="Minute">分钟</param>
        public virtual void AutomaticCycleGenerationBill(DateTime StartDate, int Hour, int Minute, bool IsTaskRun)
        {
            if (Hour < 0 || Hour >= 24)
            {
                Hour = 0;
            }

            if (Minute < 0 || Minute >= 60)
            {
                Minute = 0;
            }

            this.Hour = Hour;
            this.Minute = Minute;
            this.StartDate = StartDate;
            this.IsTaskRun = IsTaskRun;

            // 在应用程序启动时运行的代码  
            if (myTimer == null)
            {
                myTimer = new Timer();
                myTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                myTimer.Interval = 300000; //间隔5分钟循环 
                //myTimer.Interval = 60000;
                myTimer.Enabled = true;
                myTimer.AutoReset = true;
            }
            else
            {
                myTimer.Enabled = true;
            }
            LogProperty.WriteLoginToFile("账单循环开始", "CycleGenerationBill", FileLogType.Info);
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //大于等于当前时间 并大于等于执行时间
            if (StartDate <= DateTime.Now && LastExecutionTime != DateTime.Today
                && DateTime.Now.Hour == this.Hour && DateTime.Now.Minute >= this.Minute)
            {
                //确保每天只执行一次
                LastExecutionTime = DateTime.Today;
                LogProperty.WriteLoginToFile("账单循环最后日期：" + LastExecutionTime.ToShortDateString(), "CycleGenerationBill", FileLogType.Info);
                //生成今天所有账单
                GenerateAllChargBill(DateTime.Today.Day, this.IsTaskRun);
            }
            //LogProperty.WriteLoginToFile("Test 账单循环", "CycleGenerationBill", FileLogType.Info);
        }

        public virtual void CloseCycle()
        {
            if (myTimer != null)
            {
                myTimer.Enabled = false;
            }
        }

        #endregion

        #region 预付款

        /// <summary>
        /// 生成预付款账单
        /// </summary>
        /// <param name="ComDeptId">小区ID</param>
        /// <param name="HouseDeptId">房屋ID</param>
        /// <param name="ResourcesId">资源ID 注：房屋资源Id和房屋的DeptId一致</param>
        /// <param name="Amount">预付款金额</param>
        /// <param name="Remark">备注</param>
        /// <returns></returns>
        //public ResultModel GeneratePrepaymentBill(int ComDeptId, int HouseDeptId, int ResourcesId, decimal Amount, string Remark, int Operator, string OperatorName)
        //{
        //    if (Amount <= 0)
        //    {
        //        return new ResultModel() { IsSuccess = false, ErrorCode = "810", Msg = "预付款金额不能小于等于零" };
        //    }
        //    try
        //    {
        //        if (ComDeptId == 0)
        //        {
        //            ComDeptId = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCommunityDeptIdByHouseDeptId(HouseDeptId);
        //        }
        //        using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
        //        {
        //            var bill = BillCommonService.Instance.GeneratePrepaymentBill(propertyMgrUnitOfWork, ComDeptId, HouseDeptId, ResourcesId, Amount, BillStatusEnum.NoPayment, Remark);
        //            propertyMgrUnitOfWork.Commit();
        //            return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "添加预付款成功", Data = bill };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogProperty.WriteLoginToFile(string.Format("[预付款]Code:900 HouseDeptId:{0} ErrorMsg:{1}", HouseDeptId, ex.Message), "GeneratePrepaymentBill", FileLogType.Exception);
        //        return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "添加预付款失败" };
        //    }
        //}

        // <summary>
        /// 生成预付款临时账单
        /// </summary>
        /// <param name="ComDeptId">小区ID</param>
        /// <param name="HouseDeptId">房屋ID</param>
        /// <param name="ResourcesId">资源ID 注：房屋资源Id和房屋的DeptId一致</param>
        /// <param name="Amount">预付款金额</param>
        /// <param name="Remark">备注</param>
        /// <returns></returns>
        public ResultModel GenerateTempPrepaymentBill(int ComDeptId, int HouseDeptId, int ResourcesId
            , decimal Amount, string Remark, int Operator, string OperatorName, DateTime? BeginDate, DateTime? EndDate)
        {
            if (Amount <= 0)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "810", Msg = "预付款金额不能小于等于零" };
            }
            try
            {
                if (ComDeptId == 0)
                {
                    ComDeptId = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCommunityDeptIdByHouseDeptId(HouseDeptId);
                }
                string resourcesName = string.Empty;
                var deptInfo = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoById(ResourcesId.ToString());
                if (deptInfo != null)
                {
                    resourcesName = deptInfo.Name;
                }
                var bill = BillCommonService.Instance.GenerateTempPrepaymentBill(ComDeptId, HouseDeptId, ResourcesId, resourcesName, Amount, BillStatusEnum.NoPayment, Remark, BeginDate, EndDate);
                return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "添加预付款成功", Data = bill };
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[临时预付款]Code:900 HouseDeptId:{0} ErrorMsg:{1}", HouseDeptId, ex.Message), "GeneratePrepaymentBill", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "添加预付款失败" };
            }
        }

        public string GetSubjectNameById(int? subjectId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var subject = propertyMgrUnitOfWork.ChargeSubjectRepository.GetByKey(subjectId);
                if (subject != null)
                {
                    return subject.Name;
                }
                return string.Empty;
            }
        }

        // <summary>
        /// 生成预付款临时账单 添加2017-04-07
        /// </summary>
        /// <returns></returns>
        public ResultModel GenerateTempPrepaymentBill(int ComDeptId, int HouseDeptId, int ResourcesId
            , decimal Amount, string Remark, int Operator, string OperatorName, int? PreChargeSubjectId, double? Months)
        {
            if (Amount <= 0)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "810", Msg = "预付款金额不能小于等于零" };
            }
            try
            {
                if (ComDeptId == 0)
                {
                    ComDeptId = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCommunityDeptIdByHouseDeptId(HouseDeptId);
                }
                string resourcesName = string.Empty;
                var deptInfo = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoById(ResourcesId.ToString());
                if (deptInfo != null)
                {
                    resourcesName = deptInfo.Name;
                }
                if (PreChargeSubjectId == 0)
                {
                    Remark = (Months.HasValue ? string.Format("所有收费项目 预存{0}个月 ", Months) : "所有收费项目 预存费") + Remark;
                }
                else
                {
                    Remark = (Months.HasValue ? string.Format("{0} 预存{1}个月 ", GetSubjectNameById(PreChargeSubjectId), Months) : GetSubjectNameById(PreChargeSubjectId) + " 预存费") + Remark;
                }
                var bill = BillCommonService.Instance.GenerateTempPrepaymentBill(ComDeptId, HouseDeptId, ResourcesId, resourcesName, Amount, BillStatusEnum.NoPayment, Remark, null, null);
                return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "添加预付款成功", Data = bill };
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[临时预付款]Code:900 HouseDeptId:{0} ErrorMsg:{1}", HouseDeptId, ex.Message), "GeneratePrepaymentBill", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "添加预付款失败" };
            }
        }

        #endregion

        #region 临时账单


        /// <summary>
        /// 生成临时账单
        /// </summary>
        public ChargBill GenerateTemporaryBill(int ComDeptId, int HouseDeptId, int ResourcesId, int ChargeSubjectId, DateTime? BeginDateTime, DateTime? EndDateTime, decimal Amount, string Remark)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var chargeSubject = propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll().Where(c => c.Id == ChargeSubjectId).FirstOrDefault();
                if (chargeSubject != null)
                {
                    string resourcesName = string.Empty;
                    var deptInfo = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoById(ResourcesId.ToString());
                    if (deptInfo != null)
                    {
                        resourcesName = deptInfo.Name;
                    }
                    var bill = BillCommonService.Instance.GenerateTemporaryBill(propertyMgrUnitOfWork, chargeSubject, ComDeptId, HouseDeptId, ResourcesId, resourcesName, BeginDateTime, EndDateTime, Amount, BillStatusEnum.NoPayment, Remark);
                    propertyMgrUnitOfWork.Commit();
                    return bill;
                }
                return null;
            }
        }

        //GenerateTempTemporaryBill
        public ChargBill GenerateTempTemporaryBill(int ComDeptId, int HouseDeptId, int ResourcesId, int ChargeSubjectId, DateTime? BeginDateTime, DateTime? EndDateTime, decimal Amount, string Remark, int ReourceType = (int)ReourceTypeEnum.House)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var chargeSubject = propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll().Where(c => c.Id == ChargeSubjectId).FirstOrDefault();
                if (chargeSubject != null)
                {

                    string resourcesName = string.Empty;
                    if (ReourceType == (int)ReourceTypeEnum.House)
                    {
                        var deptInfo = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoById(ResourcesId.ToString());
                        if (deptInfo != null)
                        {
                            resourcesName = deptInfo.Name;
                        }
                    }
                    else if (ReourceType == (int)ReourceTypeEnum.CarPark)
                    {
                        var carport = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCarPortById(ResourcesId);
                        if (carport != null)
                        {
                            resourcesName = carport.CarportNum;
                        }
                    }
                    var bill = BillCommonService.Instance.GenerateTempTemporaryBill(chargeSubject, ComDeptId, HouseDeptId, ResourcesId, resourcesName, BeginDateTime, EndDateTime, Amount, BillStatusEnum.NoPayment, Remark);
                    return bill;
                }
                return null;
            }
        }

        #endregion

        #region 账单拆分

        public ResultModel SplitBill(string billId, DateTime splitDate, string remark, int Operator, string OperatorName)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var bill = propertyMgrUnitOfWork.ChargBillRepository.GetByKey(billId);


                var checkmodel = CheckSplitBill(billId, splitDate);


                if (!checkmodel.IsSuccess)
                {
                    return checkmodel;
                }
                try
                {
                    var newBill = BillCommonService.Instance.SplitBill(propertyMgrUnitOfWork, bill, splitDate, remark, Operator, OperatorName);
                    propertyMgrUnitOfWork.Commit();
                    return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "拆分账单成功" };
                }
                catch (Exception ex)
                {
                    LogProperty.WriteLoginToFile(string.Format("[拆分账单]Code:900 BillId:{0} ErrorMsg:{1}", billId, ex.Message), "SplitBill", FileLogType.Exception);
                    return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "拆分账单失败" };
                }
            }
        }


        public ResultModel CheckSplitBill(string billId, DateTime splitDate)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var bill = propertyMgrUnitOfWork.ChargBillRepository.GetByKey(billId);
                if (bill.ChargeSubject.BillPeriod != BillPeriodEnum.DailyCharge.GetHashCode())
                {
                    return new ResultModel() { IsSuccess = false, ErrorCode = "810", Msg = "账单收费周期必须是每日计费按月收取" };
                }
                if (!(bill.BeginDate < splitDate && splitDate < bill.EndDate))
                {
                    return new ResultModel() { IsSuccess = false, ErrorCode = "820", Msg = "账单开始日期 < 拆分日期 < 账单结束日期" };
                }
                return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "检测通过" };
            }

        }

        public ResultModel SplitTempBill(string billId, DateTime splitDate, string remark, int Operator, string OperatorName, ChargBill ebill)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //var bill = propertyMgrUnitOfWork.ChargBillRepository.GetByKey(billId);
                //if (bill == null)
                //{
                //    bill = ebill;
                //    bill.ChargeSubject = propertyMgrUnitOfWork.ChargeSubjectRepository.GetByKey(bill.ChargeSubjectId);
                //}
                var bill = ebill;
                if (!(bill.BeginDate < splitDate && splitDate < bill.EndDate))
                {
                    return new ResultModel() { IsSuccess = false, ErrorCode = "820", Msg = "账单开始日期 < 拆分日期 < 账单结束日期" };
                }

                var checkmodel = CheckSplitTempBill(ebill, splitDate);
                bill.ChargeSubject = propertyMgrUnitOfWork.ChargeSubjectRepository.GetByKey(bill.ChargeSubjectId);
                if (!checkmodel.IsSuccess)
                {
                    return checkmodel;
                }
                try
                {
                    var newBill = BillCommonService.Instance.SplitTempBill(bill, splitDate, remark, Operator, OperatorName);
                    //propertyMgrUnitOfWork.Commit();
                    return new ResultModel()
                    {
                        IsSuccess = true,
                        ErrorCode = "0",
                        Msg = "拆分账单成功",
                        Data = new ChargBill[] { newBill, bill }
                    };
                }
                catch (Exception ex)
                {
                    LogProperty.WriteLoginToFile(string.Format("[拆分账单]Code:900 BillId:{0} ErrorMsg:{1}", billId, ex.Message), "SplitBill", FileLogType.Exception);
                    return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "拆分账单失败" };
                }
            }
        }


        public ResultModel CheckSplitTempBill(ChargBill bill, DateTime splitDate)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //   var bill = propertyMgrUnitOfWork.ChargBillRepository.GetByKey(billId);

                bill.ChargeSubject = propertyMgrUnitOfWork.ChargeSubjectRepository.GetByKey(bill.ChargeSubjectId);
                if (bill.ChargeSubject.SubjectType == (int)SubjectTypeEnum.Meter)
                {
                    return new ResultModel() { IsSuccess = false, ErrorCode = "801", Msg = "三表账单不能拆分" };
                }

                if (bill.ChargeSubject.BillPeriod != BillPeriodEnum.DailyCharge.GetHashCode())
                {
                    return new ResultModel() { IsSuccess = false, ErrorCode = "810", Msg = "账单收费周期必须是每日计费按月收取" };
                }

                //if (!(bill.BeginDate < splitDate && splitDate < bill.EndDate))
                //{
                //    return new ResultModel() { IsSuccess = false, ErrorCode = "820", Msg = "账单开始日期 < 拆分日期 < 账单结束日期" };
                //}
                if (bill.ReceivedAmount > 0)
                {
                    return new ResultModel() { IsSuccess = false, ErrorCode = "830", Msg = "该账单已缴纳部分费用，暂不支持拆分" };
                }
                return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "检测通过" };
            }

        }

        #endregion

        #region 刷新账单 后改为手动生成本月账单

        public ResultModel RefreshChargBill(int HouseDeptID)
        {
            try
            {
                int CommunityID = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCommunityDeptIdByHouseDeptId(HouseDeptID);
                GenerateCommunityChargBill(CommunityID, 0, HouseDeptID);
                return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "手动生成本月账单成功" };
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[刷新账单]Code:900 HouseDeptID:{0} ErrorMsg:{1}", HouseDeptID, ex.Message), "RefreshChargBill", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "手动生成本月账单失败" };
            }

        }

        #endregion

        #region  作废账单
        public ResultModel DeleteBill(string billId, string remark, DailyChargBillDTO deleteModel)
        {


            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var bill = propertyMgrUnitOfWork.ChargBillRepository.GetByKey(billId);

                var checkmodel = CheckDeleteBill(billId, remark, deleteModel, bill);
                if (!checkmodel.IsSuccess)
                {
                    //检查
                    return checkmodel;
                }

                try
                {
                    if (deleteModel.NewBillList != null && deleteModel.NewBillList.Count > 1)
                    {
                        //拆分账单  
                        foreach (var deleobj in deleteModel.NewBillList)
                        {

                            var chargesubject = propertyMgrUnitOfWork.ChargeSubjectRepository.GetByKey(deleobj.ChargeSubjectId);
                            if (deleobj.ActionStatus == ActionStatusEnum.New)
                            {//新增
                                var insertbill = ChargBillMappers.ChangeDTOToChargBillNew(deleobj);
                                insertbill.CreateTime = DateTime.Now;
                                insertbill.UpdateTime = DateTime.Now;
                                if (insertbill.Id == billId)
                                {
                                    insertbill.IsDel = true;
                                    insertbill.Remark += "  " + remark;
                                }

                                propertyMgrUnitOfWork.ChargBillRepository.Add(insertbill);
                                insertbill.ChargeSubject = propertyMgrUnitOfWork.ChargeSubjectRepository.GetByKey(insertbill.ChargeSubjectId);
                                BillCommonService.Instance.GenerateChargeSubjectSna(propertyMgrUnitOfWork, insertbill, insertbill.ChargeSubject);             //生成快照
                            }
                            else if (deleobj.ActionStatus == ActionStatusEnum.Update)
                            {//修改


                                if (deleobj.Id == billId)
                                {
                                    bill.BillAmount = deleobj.BillAmount;
                                    bill.BeginDate = deleobj.BeginDate;
                                    bill.EndDate = deleobj.EndDate;
                                    //propertyMgrUnitOfWork.ChargBillRepository.Update(bill);
                                }
                                else
                                {
                                    var updatebill = propertyMgrUnitOfWork.ChargBillRepository.GetByKey(deleobj.Id); //ChargBillMappers.ChangeDTOToChargBillNew(deleobj);
                                    updatebill.BillAmount = deleobj.BillAmount;
                                    updatebill.BeginDate = deleobj.BeginDate;
                                    updatebill.EndDate = deleobj.EndDate;
                                    updatebill.UpdateTime = DateTime.Now;
                                    propertyMgrUnitOfWork.ChargBillRepository.Update(updatebill);
                                }
                            }
                        }
                    }

                    if (bill != null)
                    {
                        var chargesubject = propertyMgrUnitOfWork.ChargeSubjectRepository.GetByKey(bill.ChargeSubjectId);
                        if (chargesubject.SubjectType == (int)SubjectTypeEnum.Meter)
                        {
                            //作废三表
                            string ErrorStr = string.Empty;
                            if (!DeleteThreeMeter(bill.ResourcesId, propertyMgrUnitOfWork, bill.Id, out ErrorStr, chargesubject.Name))
                            {
                                return new ResultModel() { IsSuccess = false, ErrorCode = "780", Msg = ErrorStr };
                            }
                        }
                        //处理三表
                        bill.UpdateTime = DateTime.Now;
                        bill.IsDel = true;
                        bill.Remark += "  " + remark;
                        propertyMgrUnitOfWork.ChargBillRepository.Update(bill);
                    }
                    propertyMgrUnitOfWork.Commit();
                    return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "作废账单成功" };
                }
                catch (Exception ex)
                {
                    LogProperty.WriteLoginToFile(string.Format("[作废账单]Code:900 BillId:{0} ErrorMsg:{1}", billId, ex.Message), "DeleteBill", FileLogType.Exception);
                    return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "作废账单失败" };
                }
            }
        }

        /// <summary>
        /// 传入处理三表记录
        /// </summary>
        /// <param name="resouceId"></param>
        /// <param name="uow"></param>
        public bool DeleteThreeMeter(int? resouceId, IPropertyMgrUnitOfWork uow, string billid, out string ErrorStr, string chargesubjectname, bool IsCheck = false)
        {

            var meter = uow.MeterRepository.GetByKey(resouceId);
            if (meter != null && meter.Id > 0)
            {
                //取出三表记录

                var meterList = uow.MeterReadRecordRepository.GetAll().Where(o => o.MeterId == resouceId).OrderByDescending(o => o.ReadDate).ToList();
                if (meterList.Count() > 0)
                {

                    var firstmeter = meterList[0];
                    if (string.IsNullOrEmpty(firstmeter.BillID))
                    {
                        ErrorStr = string.Empty;
                        return true;
                    }

                    if (firstmeter.BillID.Contains(";"))
                    {
                        ErrorStr = "本次读表包含多个账单，无法作废";
                        return false;
                    }

                    if (firstmeter.BillID != billid)
                    {//不成功
                        ErrorStr = "请先将最新的【" + chargesubjectname + "】账单作废再进行本次操作";
                        return false;
                    }
                    else
                    {
                        if (IsCheck == true)
                        {
                            ErrorStr = string.Empty;
                            return true;
                        }
                        uow.MeterReadRecordRepository.Remove(firstmeter);// (firstmeter.Id);
                        ErrorStr = string.Empty;
                        return true;
                    }
                }
            }

            ErrorStr = string.Empty;
            return true;
        }




        public ResultModel CheckDeleteBill(string billId, string remark, DailyChargBillDTO deleteModel)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var bill = propertyMgrUnitOfWork.ChargBillRepository.GetByKey(billId);
                return CheckDeleteBill(billId, remark, deleteModel, bill);
            }
        }

        public ResultModel CheckDeleteBill(List<ChargBillDTO> billList, string remark, DailyChargBillDTO deleteModel)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                bool IsResult = true;
                string ErrorMessage = string.Empty;
                var RowList=  billList.Where(o => o.RowType == RowTypeEnum.ChildRow).ToList();
               foreach (var checkmodel in RowList)
                {
                    var bill = propertyMgrUnitOfWork.ChargBillRepository.GetByKey(checkmodel.Id);
                    var CheckResult= CheckDeleteBill(bill.Id, remark, deleteModel, bill);
                    if (!CheckResult.IsSuccess)
                    {
                        IsResult = false;
                        ErrorMessage += " 科目：" + checkmodel.ChargeSubjectName + " 时间范围:" + checkmodel.BeginDate.Value.ToShortDateString() + "-" + checkmodel.EndDate.Value.ToShortDateString() + " 原因:" + CheckResult.Msg;
                    }
                }

                return new ResultModel() { IsSuccess = IsResult, Msg = ErrorMessage };
            }
        }







        public ResultModel CheckDeleteBill(string billId, string remark, DailyChargBillDTO deleteModel, ChargBill bill)
        {
            //修改检查作废
            //如果是拆分账单头，需要更新账单开始时间、结束时间和金额
            if (deleteModel.NewBillList != null && bill != null)
            {
                var entity = deleteModel.NewBillList.Where(n => n.Id == bill.Id).FirstOrDefault();
                if (entity != null)
                {
                    bill.BillAmount = entity.BillAmount;
                    bill.BeginDate = entity.BeginDate;
                    bill.EndDate = entity.EndDate;
                }
            }

            if (bill == null)
            {
                var entity = deleteModel.NewBillList.Where(n => n.Id == billId).FirstOrDefault();
                if (entity != null)
                {
                    bill = ChargBillMappers.ChangeDTOToChargBillNew(entity);
                }
            }

            if (string.IsNullOrEmpty(remark))
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "701", Msg = "请填写备注" };
            }

            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //处理三表
                var chargesubject = propertyMgrUnitOfWork.ChargeSubjectRepository.GetByKey(bill.ChargeSubjectId);
                if (bill != null)
                {
                    if (bill.ReceivedAmount > 0)
                    {
                        return new ResultModel() { IsSuccess = false, ErrorCode = "702", Msg = "已有交款金额的账单不能作废" };
                    }

                    if (chargesubject != null && chargesubject.SubjectType == (int)SubjectTypeEnum.SystemPreset)
                    {
                        return new ResultModel() { IsSuccess = false, ErrorCode = "703", Msg = "预存费用不允许作废" };
                    }
                }

                try
                {
                    if (deleteModel.NewBillList != null && deleteModel.NewBillList.Count > 1)
                    {
                        //拆分账单  
                        //foreach (var deleobj in deleteModel.NewBillList)
                        //{

                        //var chargesubject = propertyMgrUnitOfWork.ChargeSubjectRepository.GetByKey(deleobj.ChargeSubjectId);

                        if (chargesubject != null && chargesubject.SubjectType == (int)SubjectTypeEnum.Meter)
                        {
                            return new ResultModel() { IsSuccess = false, ErrorCode = "710", Msg = "三表类型账单拆分后无法作废" };
                        }
                        //}
                    }

                    if (bill != null)
                    {
                        //未来的周期性账单只能作废该收费项目结束时间最晚的账单 修改：2017-01-10
                        DateTime currentMonthEnd = (new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)).AddMonths(1).AddDays(-1);
                        if (bill.BeginDate.HasValue && bill.BeginDate.Value > currentMonthEnd)
                        {
                            //先判断拆分账单
                            //if (deleteModel.NewBillList != null && deleteModel.NewBillList.Any(n => n.EndDate > bill.EndDate))
                            //{
                            //    return new ResultModel() { IsSuccess = false, ErrorCode = "720", Msg = "未来的周期性账单只能作废该收费项目结束时间最晚的账单" };
                            //}
                            ////再判断数据库
                            //if (propertyMgrUnitOfWork.ChargBillRepository.GetAll().Any(c => c.ResourcesId == bill.ResourcesId && c.ChargeSubjectId == bill.ChargeSubjectId && c.IsDel == false && c.EndDate > bill.EndDate))
                            //{
                            //    return new ResultModel() { IsSuccess = false, ErrorCode = "730", Msg = "未来的周期性账单只能作废该收费项目结束时间最晚的账单" };
                            //}
                        }


                        if (chargesubject != null && chargesubject.SubjectType == (int)SubjectTypeEnum.Meter)
                        {
                            string ErrorStr = string.Empty;
                            if (!DeleteThreeMeter(bill.ResourcesId, propertyMgrUnitOfWork, bill.Id, out ErrorStr, chargesubject.Name, true))
                            {
                                return new ResultModel() { IsSuccess = false, ErrorCode = "780", Msg = ErrorStr };
                            }
                        }

                    }

                    return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "检查无误" };
                }
                catch (Exception ex)
                {
                    LogProperty.WriteLoginToFile(string.Format("[检查作废账单]Code:900 BillId:{0} ErrorMsg:{1}", billId, ex.Message), "DeleteBill", FileLogType.Exception);
                    return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "作废账单检查失败" };
                }
            }
        }

        #endregion

        #region 手动生成账单

        /// <summary>
        /// 房屋手动生成账单
        /// </summary>
        public ResultModel ManualGenerationHouseBill(int ResourcesId, string[] SubjectResourceIds, DateTime EndDate, int DeptType)
        {
            //拆分收费项目Id 和 资源Id
            Dictionary<int?, List<int?>> dsrlist = new Dictionary<int?, List<int?>>();
            foreach (var item in SubjectResourceIds)
            {
                var iarry = item.Split('_');
                if (iarry.Length == 2)
                {
                    int sid = int.Parse(iarry[0]);
                    int rid = int.Parse(iarry[1]);
                    if (dsrlist.Keys.Contains(sid))
                    {
                        dsrlist[sid].Add(rid);
                    }
                    else
                    {
                        var ridList = new List<int?>();
                        ridList.Add(rid);
                        dsrlist.Add(sid, ridList);
                    }
                }
            }
            try
            {
                int CommunityID = 0;
                if (DeptType == (int )EDeptType.FangWu)
                {
                    CommunityID = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCommunityDeptIdByHouseDeptId(ResourcesId);
                }
                else if(DeptType == (int)EDeptType.CheWei)
                {
                    CommunityID = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCommunityDeptIdByCarPortId(ResourcesId);
                }

                IList<CalculateProperty> pmpList = new List<CalculateProperty>();
                int?[] subjectIds = dsrlist.Keys.ToArray();
                using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    var chargeSubjectList = new List<ChargeSubject>();
                    //获取物业小区 个性化配置 v2.9 2017-9-13
                    var config = CommunityConfigMappers.ChangeCommunityConfigToDTO(BillCommonService.Instance.GetCommunityConfig(pmUnitWork, CommunityID));
                    config.ActionType = ActionTypeEnum.ManualGenerationBill;
                    config.ChargeRecordId = Guid.NewGuid().ToString();
                    switch (DeptType)
                    {
                        case (int)EDeptType.FangWu:
                            {
                                IList<CalculateProperty> hmpList = CalculatePropertyHelper.GetHouseCalculateProperty(CommunityID, ResourcesId);
                                pmpList = CalculatePropertyHelper.GetParkingSpaceCalculateProperty(CommunityID, hmpList, ResourcesId);

                                chargeSubjectList = pmUnitWork
                               .ChargeSubjectRepository
                               .GetAll()
                               .Where(c => c.IsDel == false
                                           && subjectIds.Contains(c.Id) //符合选择条件
                                           && c.ComDeptId == CommunityID
                                           && (c.BillPeriod == EnumHelper.BillPeriod.DailyCharge
                                               || c.BillPeriod == EnumHelper.BillPeriod.MonthlyCharge
                                               || c.BillPeriod == EnumHelper.BillPeriod.MeterCharge
                                           )
                                           //&& c.BeginDate <= DateTime.Today
                                           && c.SubjectType != EnumHelper.SubjectType.SystemPreset)//排除预交费账单
                                           .ToList();

                                //获取满足条件的收费对象 三表和按月收费项目

                                //循环生成账单
                                foreach (var item in chargeSubjectList)
                                {
                                    SubjectTypeEnum subjectType = (SubjectTypeEnum)item.SubjectType;
                                    DateTime edate = EndDate;
                                    //非每日计划 取月末
                                    if (item.BillPeriod != (int)BillPeriodEnum.DailyCharge)
                                    {
                                        edate = (new DateTime(EndDate.Year, EndDate.Month, 1)).AddMonths(1).AddDays(-1);
                                    }
                                    switch (subjectType)
                                    {
                                        case SubjectTypeEnum.House://房屋相关
                                            {
                                                int?[] resultIds = null;
                                                if (ResourcesId != 0 && DeptType == (int)EDeptType.FangWu)
                                                {
                                                    resultIds = new int?[] { ResourcesId };
                                                }
                                                GenerateHouseSubjectChargBill(pmUnitWork, item, hmpList, config, true, resultIds, edate);
                                            }
                                            break;
                                        case SubjectTypeEnum.ParkingSpace://车位相关
                                            {
                                                int?[] resultIds = null;
                                                if (ResourcesId != 0)
                                                {
                                                    resultIds = dsrlist[item.Id].ToArray();
                                                }
                                                GenerateParkingSpaceSubjectChargBill(pmUnitWork, item, pmpList, config, true, resultIds, edate);
                                            }
                                            break;
                                        case SubjectTypeEnum.Meter://三表相关
                                            {
                                                int?[] resultIds = null;
                                                if (ResourcesId != 0)
                                                {
                                                    resultIds = dsrlist[item.Id].ToArray();
                                                }
                                                GenerateMeterSubjectChargBill(pmUnitWork, hmpList, item, config, CommunityID, resultIds);
                                            }
                                            break;
                                        case SubjectTypeEnum.Other://其它 不需要其它计费参数
                                            {
                                                int?[] resultIds = null;
                                                if (ResourcesId != 0)
                                                {
                                                    resultIds = new int?[] { ResourcesId };
                                                }
                                                GenerateOtherSubjectChargBill(pmUnitWork, hmpList, item, config, true, resultIds, edate);
                                            }
                                            break;
                                    }
                                }
                            }
                            break;

                        case (int)EDeptType.CheWei:
                            {
                                //修改bug #4609 2017-07-07 tdz
                                int houseDeptId = 0;
                                var carport = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCarPortById(ResourcesId);
                                if (carport != null)
                                {
                                    houseDeptId = carport.HouseDeptID??0;
                                }
                                IList<CalculateProperty> hmpList = CalculatePropertyHelper.GetHouseCalculateProperty(CommunityID, houseDeptId);
                                CommunityID = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCommunityDeptIdByCarPortId(ResourcesId);
                                pmpList = CalculatePropertyHelper.GetParkingSpaceCalculateProperty(CommunityID, hmpList, 0, ResourcesId, (int)ReourceTypeEnum.CarPark);
                                chargeSubjectList = pmUnitWork
                               .ChargeSubjectRepository
                               .GetAll()
                               .Where(c => c.IsDel == false
                                           && subjectIds.Contains(c.Id) //符合选择条件
                                           && c.ComDeptId == CommunityID
                                           && (c.BillPeriod == EnumHelper.BillPeriod.DailyCharge
                                               || c.BillPeriod == EnumHelper.BillPeriod.MonthlyCharge
                                               || c.BillPeriod == EnumHelper.BillPeriod.MeterCharge
                                           )
                                           //&& c.BeginDate <= DateTime.Today
                                           && c.SubjectType != EnumHelper.SubjectType.SystemPreset)//排除预交费账单
                                           .ToList();

                                foreach (var item in chargeSubjectList)
                                {
                                    SubjectTypeEnum subjectType = (SubjectTypeEnum)item.SubjectType;
                                    DateTime edate = EndDate;
                                    //非每日计划 取月末
                                    if (item.BillPeriod != (int)BillPeriodEnum.DailyCharge)
                                    {
                                        edate = (new DateTime(EndDate.Year, EndDate.Month, 1)).AddMonths(1).AddDays(-1);
                                    }
                                    switch (subjectType)
                                    {
                                        case SubjectTypeEnum.ParkingSpace://车位相关
                                            {
                                                int?[] resultIds = null;
                                                if (ResourcesId != 0)
                                                {
                                                    resultIds = dsrlist[item.Id].ToArray();
                                                    //resultIds = pmpList.Where(p => p.ResourcesId == ResourcesId).Select(p => (int?)p.ResourcesId).ToArray();
                                                }
                                                GenerateParkingSpaceSubjectChargBill(pmUnitWork, item, pmpList, config, true, resultIds, edate);
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                    }
                    //配置允许预存抵扣 且 合并收费记录
                    if (config.IsPreAutomaticDeduction.Value && config.IsPreMergeChargeRecord.Value)
                    {
                        if (config.ChargBillList.Count() > 0)
                        {
                            var billDictionary = config.ChargBillList.ToDictionary(key => key.Id, value => value.ReceivedAmount.Value);
                            BillCommonService.Instance.GenerateChargeRecordByBillList(pmUnitWork, config.ChargBillList, config.ChargeRecordId, billDictionary, "手动生成账单收费记录合并");
                        }
                        pmUnitWork.Commit();
                    }
                }

                return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "手动生成账单成功" };
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[手动生成账单]Code:900 HouseDeptID:{0} ErrorMsg:{1}", ResourcesId, ex.Message), "ManualGenerationHouseBill", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "手动生成账单失败" };
            }
        }

        /// <summary>
        /// 批量生成账单
        /// </summary>
        public ResultModel ManualBatchGenerationByComDeptId(int ComDeptId, int SubjectId, int?[] ResultIds, DateTime EndDate)
        {
            try
            {
                using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    //获取满足条件的收费对象 三表和按月收费项目
                    var chargeSubject = pmUnitWork
                        .ChargeSubjectRepository
                        .GetAll()
                        .Where(c => c.IsDel == false
                                    && c.Id == SubjectId //符合选择条件
                                    && c.ComDeptId == ComDeptId
                                    && (c.BillPeriod == EnumHelper.BillPeriod.DailyCharge
                                        || c.BillPeriod == EnumHelper.BillPeriod.MonthlyCharge
                                        || c.BillPeriod == EnumHelper.BillPeriod.MeterCharge
                                    )
                                    //&& c.BeginDate <= DateTime.Today
                                    && c.SubjectType != EnumHelper.SubjectType.SystemPreset)//排除预交费账单
                                    .FirstOrDefault();
                    if (chargeSubject == null)
                    {
                        return new ResultModel() { IsSuccess = false, ErrorCode = "710", Msg = "选择的收费项目不存在或不符合条件" };
                    }

                    //获取物业小区 个性化配置 v2.9 2017-9-13
                    var config = CommunityConfigMappers.ChangeCommunityConfigToDTO(BillCommonService.Instance.GetCommunityConfig(pmUnitWork, ComDeptId));

                    SubjectTypeEnum subjectType = (SubjectTypeEnum)chargeSubject.SubjectType;
                    IList<CalculateProperty> hmpList = CalculatePropertyHelper.GetHouseCalculateProperty(ComDeptId);
                    switch (subjectType)
                    {
                        case SubjectTypeEnum.House://房屋相关
                            {
                                GenerateHouseSubjectChargBill(pmUnitWork, chargeSubject, hmpList, config, true, ResultIds, EndDate);
                            }
                            break;
                        case SubjectTypeEnum.ParkingSpace://车位相关
                            {
                                IList<CalculateProperty> pmpList = CalculatePropertyHelper.GetParkingSpaceCalculateProperty(ComDeptId, hmpList);
                                GenerateParkingSpaceSubjectChargBill(pmUnitWork, chargeSubject, pmpList, config, true, ResultIds, EndDate);
                            }
                            break;
                        case SubjectTypeEnum.Meter://三表相关
                            {
                                GenerateMeterSubjectChargBill(pmUnitWork, hmpList, chargeSubject, config, ComDeptId, ResultIds);
                            }
                            break;
                        case SubjectTypeEnum.Other://其它 不需要其它计费参数
                            {
                                //IList<CalculateProperty> hmpList = CalculatePropertyHelper.GetHouseCalculateProperty(ComDeptId);
                                GenerateOtherSubjectChargBill(pmUnitWork, hmpList, chargeSubject, config, true, ResultIds, EndDate);
                            }
                            break;
                    }
                }
                return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "批量生成账单成功" };
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[批量生成账单]Code:900 ComDeptId:{0} ErrorMsg:{1}", ComDeptId, ex.Message), "ManualBatchGenerationByComDeptId", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "批量生成账单失败" };
            }
        }

        #endregion

        #region 解绑手动生成账单

        public IList<ResultModel> ManualUnbundlingGenerationBill(int ComDeptId, Dictionary<int, IEnumerable<UnbundlingDto>> UnbundlingList)
        {
            IList<ResultModel> resultList = new List<ResultModel>();
            int[] subjectIds = UnbundlingList.Keys.ToArray();
            DateTime endDate = DateTime.Today;// (new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)).AddMonths(1).AddDays(-1);
            foreach (var subjectId in subjectIds)
            {
                int?[] resultIds = UnbundlingList[subjectId].Select(s => (int?)s.ResultId).ToArray();
                var result = ManualBatchGenerationByComDeptId(ComDeptId, subjectId, resultIds, endDate);
                result.Data = new { SubjectId = subjectId, ResultIds = resultIds };
                resultList.Add(result);
            }
            if (resultList.Count() == 1)
            {
                if (resultList[0].IsSuccess)
                {
                    resultList[0].Msg = "生成账单成功";
                }
                else
                {
                    resultList[0].Msg = "生成账单失败";
                }
            }
            return resultList;
        }

        #endregion

        #endregion

        #region 注册服务

        /// <summary>
        /// 注册服务
        /// </summary>
        private void RegistService()
        {
            PresentationServiceHelper.Register<IPropertyService>(new PropertyAppService());
            RegistDomainService();
        }

        /// <summary>
        /// 领域层服务
        /// </summary>
        private void RegistDomainService()
        {
            DomainInterfaceHelper.Register<IPropertyDomainService>(new PropertyDomainService());
        }

        #endregion

    }
}
