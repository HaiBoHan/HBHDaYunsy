using System;
using System.Collections.Generic;
using UFIDA.U9.Base;
using UFIDA.U9.CBO.SCM.Customer;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI04;
using UFIDA.U9.GL.Voucher;
using UFSoft.UBF.Business;
using UFSoft.UBF.Eventing;
using UFSoft.UBF.PL;
using UFSoft.UBF.Util.Log;
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
	public class VoucherDeleted : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;

                // 删除不作处理，审核、弃审才处理
                return;
				if (!(key == null))
				{
                    Voucher voucher = key.GetEntity() as Voucher;
                    //if (voucher.Org.Code == "20")
                    if (PubHelper.IsOrg_Finance2DMS())
					{
						bool flag = PubHelper.IsUsedDMSAPI();
                        if (flag)
                        {
                            // 不审核的，都不更新DMS，删除时肯定也不应该修改;
                            if (voucher.VoucherStatus == VoucherStatus.Approved)
                            {
                                SI04ImplService service = new SI04ImplService();
                                // service.Url = PubHelper.GetAddress(service.Url);
                                using (System.Collections.Generic.IEnumerator<IPersistableObject> enumerator = voucher.Entries.DelLists.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        Entry entry = (Entry)enumerator.Current;
                                        if (entry.AccountKey != null && (entry.Account.Segment1.StartsWith("22410401") || entry.Account.Segment1.StartsWith("1122010303") || entry.Account.Segment1.StartsWith("1122010101") || entry.Account.Segment1.StartsWith("1122010301") || entry.Account.Segment1.StartsWith("2241030801") || entry.Account.Segment1 == "2241030803" || entry.Account.Segment1 == "1122010302" || entry.Account.Segment1 == "2241030901"))
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
                                                        if (entry.Account.Segment1.StartsWith("22410401") || entry.Account.Segment1 == "1122010302")
                                                        {
                                                            dto.operaTionType = "DJ";
                                                        }
                                                        else if (entry.Account.Segment1.StartsWith("1122010101"))
                                                        {
                                                            dto.operaTionType = "FCK";
                                                        }
                                                        else if (entry.Account.Segment1 == "1122010301")
                                                        {
                                                            dto.operaTionType = "CBXY";
                                                        }
                                                        else if (entry.Account.Segment1 == "2241030801" || entry.Account.Segment1 == "2241030803")
                                                        {
                                                            dto.operaTionType = "BZJ";
                                                        }
                                                        else if (entry.Account.Segment1 == "12210203")
                                                        {
                                                            dto.operaTionType = "SBXY";
                                                        }
                                                        else
                                                        {
                                                            dto.operaTionType = "FL";
                                                        }
                                                        dto.amount = double.Parse(System.Math.Abs(entry.AccountedCr).ToString());
                                                        //Customer cust = Customer.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), entry.Account.Segment3), new OqlParam[0]);

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
                                                        logger.Info(string.Format("删除 dealerCode={0},creadNo={1},invokeTime={2},UNineCreateUser={3},remark={4},changeType={5},operaTionType={6},amount={7},customerType={8}", new object[]
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
		}
	}
}
