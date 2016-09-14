using System;
using System.Collections.Generic;
using UFIDA.U9.Base;
using UFIDA.U9.CBO.SCM.Customer;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI08;
using UFSoft.UBF.Business;
using UFSoft.UBF.Eventing;
using HBH.DoNet.DevPlatform.EntityMapping;

namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
	public class CustomerDeleted : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
					Customer customer = key.GetEntity() as Customer;
                    if (PubHelper.IsOrg_Customer2DMS(customer))
					{
						bool flag = PubHelper.IsUsedDMSAPI();
						if (flag)
                        {
                            //if ((customer.CustomerCategoryKey != null && (customer.CustomerCategory.Code == "101007" || customer.CustomerCategory.Code == "101006"))
                            //    || (customer.CustomerCategory != null
                            //        && customer.CustomerCategory.DescFlexField != null
                            //        && customer.CustomerCategory.DescFlexField.PrivateDescSeg1.GetBool()
                            //        )
                            //    )
                            if(
                                PubHelper.IsUpdateDMS(customer)
                                )
							{
								try
								{
									SI08ImplService service = new SI08ImplService();
									// service.Url = PubHelper.GetAddress(service.Url);
									System.Collections.Generic.List<dealerInfoDto> list = new System.Collections.Generic.List<dealerInfoDto>();
									dealerInfoDto dto = new dealerInfoDto();
									dto.dealerCode = customer.Code;
									dto.dealerName = customer.Name;
									dto.dealerShortName = customer.ShortName;
									dto.companyCode = customer.Code;
									dto.companyName = customer.Name;
									dto.companyShortName = customer.ShortName;
									if (customer.CustomerCategoryKey != null)
									{
										dto.dealerType = int.Parse(customer.CustomerCategory.Code);
									}
                                    dto.actionType = 3;
                                    // status  100201 有效 100202 无效
                                    dto.status = (customer.Effective != null && customer.Effective.IsEffective) ? "100201" : "100202";

									list.Add(dto);
									dealerInfoDto d = service.Do(list.ToArray());
									if (d != null && d.flag == 0)
									{
										throw new System.ApplicationException(d.errMsg);
									}
								}
								catch (System.Exception e)
								{
									throw new System.ApplicationException("调用DMS接口错误：" + e.Message);
								}
							}
						}
					}
				}
			}
		}
	}
}
