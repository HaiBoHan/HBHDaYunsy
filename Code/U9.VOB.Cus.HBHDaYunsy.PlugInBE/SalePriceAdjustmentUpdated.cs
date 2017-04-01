using System;
using System.Collections.Generic;
using UFIDA.U9.Base;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_PI06;
using UFIDA.U9.SPR.SalePriceAdjustment;
using UFSoft.UBF.Business;
using UFSoft.UBF.Eventing;
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
	public class SalePriceAdjustmentUpdated : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
                    SalePriceAdjustment SalepriceAdjustment = key.GetEntity() as SalePriceAdjustment;
                    if (PubHelper.IsOrg_SalePriceList2DMS(SalepriceAdjustment))
					{
						bool flag = PubHelper.IsUsedDMSAPI();
						if (flag)
						{
							PI06ImplService service = new PI06ImplService();
							// service.Url = PubHelper.GetAddress(service.Url);
							System.Collections.Generic.List<partBaseDto> lines = new System.Collections.Generic.List<partBaseDto>();
							foreach (SalePriceAdjustLine line in SalepriceAdjustment.SalePriceAdjustLines)
							{
								if (
                                    //line.SysState == 2 
                                    line.SysState == UFSoft.UBF.PL.Engine.ObjectState.Inserted
                                    //|| (line.SysState == 4 
                                    || (line.SysState == UFSoft.UBF.PL.Engine.ObjectState.Updated
                                        && line.NewPrice != line.OriginalData.NewPrice
                                        ))
								{
									partBaseDto linedto = new partBaseDto();
									linedto.suptCode = string.Empty;
									if (line.ItemInfo != null && line.ItemInfo.ItemIDKey != null)
									{
										linedto.partCode = line.ItemInfo.ItemID.Code;
										linedto.partName = line.ItemInfo.ItemID.Name;
										if (line.ItemInfo.ItemID.InventoryUOM != null)
										{
											linedto.unit = line.ItemInfo.ItemID.InventoryUOM.Name;
										}
										if (line.ItemInfo.ItemID.PurchaseInfo != null)
										{
											linedto.miniPack = ((line.ItemInfo.ItemID.PurchaseInfo.MinRcvQty > 0) ? System.Convert.ToInt32(line.ItemInfo.ItemID.PurchaseInfo.MinRcvQty) : 1);
										}
									}
									linedto.salePrice = float.Parse(line.NewPrice.ToString());
									linedto.unitPrace = linedto.salePrice;
									linedto.isDanger = "0";
									linedto.isReturn = "1";
									linedto.isSale = "1";
									linedto.isFlag = "1";
									linedto.isEffective = line.Lapse.ToString();
									linedto.actionType = 1;
									lines.Add(linedto);
								}
							}
							try
							{
								if (lines.Count > 0)
								{
									partBaseDto d = service.Do(lines.ToArray());
									if (d != null && d.flag == 0)
									{
										throw new BusinessException(d.errMsg);
									}
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
