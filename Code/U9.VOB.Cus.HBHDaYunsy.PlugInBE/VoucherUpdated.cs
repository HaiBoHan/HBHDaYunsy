using System;
using UFIDA.U9.Base;
using UFIDA.U9.CBO.SCM.Customer;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI04;
using UFIDA.U9.GL.Voucher;
using UFSoft.UBF.Business;
using UFSoft.UBF.Eventing;
using UFSoft.UBF.PL;
using UFSoft.UBF.Util.Log;
using HBH.DoNet.DevPlatform.EntityMapping;
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
	public class VoucherUpdated : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
					Voucher voucher = key.GetEntity() as Voucher;
                    //if (voucher.Org.Code == "20")
                    if (PubHelper.IsOrg_Finance2DMS())
					{
						bool flag = PubHelper.IsUsedDMSAPI();
						if (flag)
						{
							if ((voucher.OriginalData.VoucherStatus == VoucherStatus.Approving
                                || voucher.OriginalData.VoucherStatus == VoucherStatus.Open 
                                )
                                && voucher.VoucherStatus == VoucherStatus.Approved)
							{
								SI04ImplService service = new SI04ImplService();
								// service.Url = PubHelper.GetAddress(service.Url);
								foreach (Entry entry in voucher.Entries)
								{
                                    //if(entry.AccountKey != null && (entry.Account.Segment1.StartsWith("22410401") || entry.Account.Segment1.StartsWith("1122010303") || entry.Account.Segment1.StartsWith("1122010101") || entry.Account.Segment1.StartsWith("1122010301") || entry.Account.Segment1.StartsWith("2241030801") || entry.Account.Segment1 == "2241030803" || entry.Account.Segment1 == "1122010302" || entry.Account.Segment1 == "2241030901"))

                                    // 这里只处理打款(回款) 业务(即，账号增加；减少 在应收应付里做)

                                    // operaTionType账户类型；现金、三包
                                    // 现金，DJ(定金)；三包，CBXY(三包信用)
                                    string dmsOperationType = GetDMSOperationType(entry);

                                    //if (IsUpdateDMS(entry))
                                    if(dmsOperationType.IsNotNullOrWhiteSpace())
									{
                                        //if (entry.AccountedCr != 0)
                                        // Cr贷、Dr借
                                        if (entry.AccountedCr != 0
                                            // || entry.AccountedDr != 0
                                            )
                                        {
                                            Customer cust = Customer.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), entry.Account.Segment3), new OqlParam[0]);

                                            if (PubHelper.IsUpdateDMS(cust))
                                            {
                                                try
                                                {
                                                    accountInfoDto dto = new accountInfoDto();
                                                    dto.dealerCode = entry.Account.Segment3;
                                                    dto.creadNo = voucher.VoucherDisplayCode;
                                                    dto.invokeTime = System.DateTime.Now;
                                                    dto.UNineCreateUser = voucher.CreatedBy;
                                                    dto.remark = entry.Abstracts;
                                                    dto.changeType = ((entry.AccountedCr > 0) ? 1 : 0);
                                                    // operaTionType账户类型；现金、三包
                                                    // 现金，DJ(定金)；三包，SBXY(三包信用)
                                                    //if (entry.Account.Segment1.StartsWith("22410401") || entry.Account.Segment1 == "1122010302")
                                                    //{
                                                    //    dto.operaTionType = "DJ";
                                                    //}
                                                    //else if (entry.Account.Segment1.StartsWith("1122010101"))
                                                    //{
                                                    //    dto.operaTionType = "FCK";
                                                    //}
                                                    //else if (entry.Account.Segment1 == "1122010301")
                                                    //{
                                                    //    dto.operaTionType = "CBXY";
                                                    //}
                                                    //else if (entry.Account.Segment1 == "2241030801" || entry.Account.Segment1 == "2241030803")
                                                    //{
                                                    //    dto.operaTionType = "BZJ";
                                                    //}
                                                    //else if (entry.Account.Segment1 == "12210203")
                                                    //{
                                                    //    dto.operaTionType = "SBXY";
                                                    //}
                                                    //else
                                                    //{
                                                    //    dto.operaTionType = "FL";
                                                    //}
                                                    dto.operaTionType = dmsOperationType;

                                                    dto.amount = double.Parse(System.Math.Abs(entry.AccountedCr).ToString());

                                                    if (Context.LoginOrg.Code == PubHelper.Const_OrgCode_Electric)
                                                    {
                                                        // 电动车只有服务站
                                                        dto.customerType = "101006";
                                                    }
                                                    else
                                                    {
                                                        if (cust != null && cust.CustomerCategoryKey != null)
                                                        {
                                                            dto.customerType = cust.CustomerCategory.Code;
                                                        }
                                                    }
                                                    ILogger logger = LoggerManager.GetLogger(typeof(Voucher));
                                                    logger.Info(string.Format("审核 dealerCode={0},creadNo={1},invokeTime={2},UNineCreateUser={3},remark={4},changeType={5},operaTionType={6},amount={7},customerType={8}", new object[]
												{
													dto.dealerCode,
													dto.creadNo,
													dto.invokeTime,
													dto.UNineCreateUser,
													dto.remark,
													dto.changeType,
													dto.operaTionType,
													dto.amount.ToString(),
													dto.customerType
												}), new object[0]);
                                                    accountInfoDto c = service.Do(dto);
                                                    if (c != null && c.flag == 0)
                                                    {
                                                        throw new BusinessException(c.errMsg);
                                                    }
                                                }
                                                catch (System.Exception e)
                                                {
                                                    throw new BusinessException("调用DMS接口错误：" + e.Message);
                                                }
                                            }
                                        }
									}
								}
							}
							else if (voucher.OriginalData.VoucherStatus == VoucherStatus.Approved 
                                && (voucher.VoucherStatus == VoucherStatus.Approving
                                    || voucher.VoucherStatus == VoucherStatus.Open
                                    )
                                )
							{
								SI04ImplService service = new SI04ImplService();
								// service.Url = PubHelper.GetAddress(service.Url);
								foreach (Entry entry in voucher.Entries)
								{
                                    //if (entry.AccountKey != null && (entry.Account.Segment1.StartsWith("22410401") || entry.Account.Segment1.StartsWith("1122010303") || entry.Account.Segment1.StartsWith("1122010101") || entry.Account.Segment1.StartsWith("1122010301") || entry.Account.Segment1.StartsWith("2241030801") || entry.Account.Segment1 == "2241030803" || entry.Account.Segment1 == "1122010302" || entry.Account.Segment1 == "2241030901"))

                                    // 这里只处理打款(回款) 业务(即，账号增加；减少 在应收应付里做)

                                    // operaTionType账户类型；现金、三包
                                    // 现金，DJ(定金)；三包，CBXY(三包信用)
                                    string dmsOperationType = GetDMSOperationType(entry);

                                    //if (IsUpdateDMS(entry))
                                    if (dmsOperationType.IsNotNullOrWhiteSpace())
									{
                                        if (entry.AccountedCr != 0)
                                        {
                                            Customer cust = Customer.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), entry.Account.Segment3), new OqlParam[0]);

                                            if (PubHelper.IsUpdateDMS(cust))
                                            {
                                                try
                                                {
                                                    accountInfoDto dto = new accountInfoDto();
                                                    dto.dealerCode = entry.Account.Segment3;
                                                    dto.creadNo = voucher.VoucherDisplayCode;
                                                    dto.invokeTime = System.DateTime.Now;
                                                    dto.UNineCreateUser = voucher.CreatedBy;
                                                    dto.remark = entry.Abstracts;
                                                    dto.changeType = ((entry.AccountedCr > 0) ? 0 : 1);
                                                    //if (entry.Account.Segment1.StartsWith("22410401") || entry.Account.Segment1 == "1122010302")
                                                    //{
                                                    //    dto.operaTionType = "DJ";
                                                    //}
                                                    //else if (entry.Account.Segment1.StartsWith("1122010101"))
                                                    //{
                                                    //    dto.operaTionType = "FCK";
                                                    //}
                                                    //else if (entry.Account.Segment1 == "1122010301")
                                                    //{
                                                    //    dto.operaTionType = "CBXY";
                                                    //}
                                                    //else if (entry.Account.Segment1 == "2241030801" || entry.Account.Segment1 == "2241030803")
                                                    //{
                                                    //    dto.operaTionType = "BZJ";
                                                    //}
                                                    //else if (entry.Account.Segment1 == "12210203")
                                                    //{
                                                    //    dto.operaTionType = "SBXY";
                                                    //}
                                                    //else
                                                    //{
                                                    //    dto.operaTionType = "FL";
                                                    //}
                                                    dto.operaTionType = dmsOperationType;
                                                    dto.amount = double.Parse(System.Math.Abs(entry.AccountedCr).ToString());
                                                    //Customer cust = Customer.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), entry.Account.Segment3), new OqlParam[0]);
                                                    //if (cust != null && cust.CustomerCategoryKey != null)
                                                    //{
                                                    //    dto.customerType = cust.CustomerCategory.Code;
                                                    //}

                                                    if (Context.LoginOrg.Code == PubHelper.Const_OrgCode_Electric)
                                                    {
                                                        // 电动车只有服务站
                                                        dto.customerType = "101006";
                                                    }
                                                    else
                                                    {
                                                        if (cust != null && cust.CustomerCategoryKey != null)
                                                        {
                                                            dto.customerType = cust.CustomerCategory.Code;
                                                        }
                                                    }
                                                    ILogger logger = LoggerManager.GetLogger(typeof(Voucher));
                                                    logger.Info(string.Format("弃审 dealerCode={0},creadNo={1},invokeTime={2},UNineCreateUser={3},remark={4},changeType={5},operaTionType={6},amount={7},customerType={8}", new object[]
												{
													dto.dealerCode,
													dto.creadNo,
													dto.invokeTime,
													dto.UNineCreateUser,
													dto.remark,
													dto.changeType,
													dto.operaTionType,
													dto.amount.ToString(),
													dto.customerType
												}), new object[0]);
                                                    accountInfoDto c = service.Do(dto);
                                                    if (c != null && c.flag == 0)
                                                    {
                                                        throw new BusinessException(c.errMsg);
                                                    }
                                                }
                                                catch (System.Exception e)
                                                {
                                                    throw new BusinessException("调用DMS接口错误：" + e.Message);
                                                }
                                            }
                                        }
									}
								}
							}
						}
					}
				}
			}
		}

        // 账户类型
        /// <summary>
        /// 账户类型
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private static string GetDMSOperationType(Entry entry)
        {
            if (entry != null
                && entry.Account != null
                && !entry.Account.Segment1.IsNull()
                )
            {
                if (Context.LoginOrg.Code == PubHelper.Const_OrgCode_Electric)
                {
                    /*
                    科目名称	科目编码	说明	DMS类型
                    保证金账户	22410306	备件	BZJ
                    现金配件	1122020301	备件	DJ
                    三包配件	1122020302	备件	SBXY
                    铺货配件	1122020303	备件	CBXY
                     */
                    // 1、现金销售业务
                    // 扣款，借     现金配件款   1122020301
                    // 打款，借     库存现金或者银行存款    1001/1002
                    //       贷     现金配件款    1122020301
                    // 2、三包销售业务
                    // 扣款，借     配件款   1122020302
                    // 打款，借     库存现金、银行存款、其他应付款/客户返利/三包服务费    1001/1002/22411001
                    //       贷     三包配件款    1122020302

                    // operaTionType账户类型；现金、三包
                    // 现金，DJ(定金)；三包，SBXY(三包信用)
                    if (entry.Account.Segment1.StartsWith("1122020301")
                        )
                    {
                        return "DJ";
                    }
                    // 三包配件
                    else if (entry.Account.Segment1.StartsWith("1122020302"))
                    {
                        return "SBXY";
                    }
                    // 保证金账户
                    else if (entry.Account.Segment1.StartsWith("22410306"))
                    {
                        return "BZJ";
                    }
                    // 铺货配件
                    else if (entry.Account.Segment1.StartsWith("1122020303"))
                    {
                        return "CBXY";
                    }
                }
                else if (Context.LoginOrg.Code == PubHelper.Const_OrgCode_Chengdu
                    || Context.LoginOrg.Code == PubHelper.Const_OrgCode_Hubei
                    )
                {
                    /*
                    定金账户	22410401	通用车
                    发车款账户	1122010101	通用车
                    监控车信用账户	待定	通用车
                    保证金账户	2241030801	通用车
                    授信保证金账户	2241030901	通用车
                    保证金账户	2241030803	备件
                    现金配件	1122010302	备件
                    三包配件	1122010303	备件
                    铺货配件	1122010301	备件
                     */
                    // operaTionType账户类型；现金、三包
                    // 现金，DJ(定金)；三包，CBXY(三包信用)
                    if (entry.Account.Segment1.StartsWith("22410401") 
                        || entry.Account.Segment1.StartsWith("1122010302")
                        )
                    {
                        return "DJ";
                    }
                    // 发车款账户
                    else if (entry.Account.Segment1.StartsWith("1122010101"))
                    {
                        return "FCK";
                    }
                    // 三包配件
                    else if (entry.Account.Segment1 == "1122010301")
                    {
                        return "CBXY";
                    }
                    // 保证金账户
                    else if (entry.Account.Segment1 == "2241030801" || entry.Account.Segment1 == "2241030803")
                    {
                        return "BZJ";
                    }
                    // 三包配件
                    // 2017-01-16 wf 按熊彬给的DMS科目 修改
                    //else if (entry.Account.Segment1 == "12210203")
                    else if (entry.Account.Segment1 == "1122010303")
                    {
                        return "SBXY";
                    }
                    // 授信保证金账户
                    else if (entry.Account.Segment1 == "2241030901")
                    {
                        return "FL";
                    }
                    else
                    {
                        //return "FL";

                        return string.Empty;
                    }
                }
            }
            return string.Empty;
        }

        //private static bool IsUpdateDMS(Entry entry)
        //{
        //    if (Context.LoginOrg.Code == PubHelper.Const_OrgCode_Electric)
        //    {
        //        // 1、现金销售业务
        //        // 扣款，借     现金配件款   1122020301
        //        // 打款，借     库存现金或者银行存款    1001/1002
        //        //       贷     现金配件款    1122020301
        //        // 2、三包销售业务
        //        // 扣款，借     配件款   1122020302
        //        // 打款，借     库存现金、银行存款、其他应付款/客户返利/三包服务费    1001/1002/22411001
        //        //       贷     三包配件款    1122020302
        //    }
        //    else if (Context.LoginOrg.Code == PubHelper.Const_OrgCode_Chengdu
        //        || Context.LoginOrg.Code == PubHelper.Const_OrgCode_Hubei
        //        )
        //    {
        //        return entry.AccountKey != null && (entry.Account.Segment1.StartsWith("22410401") || entry.Account.Segment1.StartsWith("1122010303") || entry.Account.Segment1.StartsWith("1122010101") || entry.Account.Segment1.StartsWith("1122010301") || entry.Account.Segment1.StartsWith("2241030801") || entry.Account.Segment1 == "2241030803" || entry.Account.Segment1 == "1122010302" || entry.Account.Segment1 == "2241030901");
        //    }

        //    return false;
        //}
	}
}
