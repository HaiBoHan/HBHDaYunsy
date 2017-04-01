using System;
using System.Collections.Generic;
using UFIDA.U9.Base;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_PI06;
using UFIDA.U9.SPR.SalePriceList;
using UFSoft.UBF.Business;
using UFSoft.UBF.Eventing;
using UFSoft.UBF.PL;
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
	public class SalePriceListDeleted : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
					SalePriceList SalepriceList = key.GetEntity() as SalePriceList;


                    // 不处理删除了，只处理生效就好了
                    return;

                    if (PubHelper.IsOrg_SalePriceList2DMS(SalepriceList))
					{
						bool flag = PubHelper.IsUsedDMSAPI();
                        if (flag)
                        {
                            // 电动车配件价表
                            //if (PubHelper.PriceList2DMS.Contains(SalepriceList.Code))
                            if(SalePriceListInserted.IsUpdateDMS(SalepriceList))
                            {
                                PI06ImplService service = new PI06ImplService();
                                // service.Url = PubHelper.GetAddress(service.Url);
                                System.Collections.Generic.List<partBaseDto> lines = new System.Collections.Generic.List<partBaseDto>();
                                using (System.Collections.Generic.IEnumerator<IPersistableObject> enumerator = SalepriceList.SalePriceLines.DelLists.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        SalePriceLine line = (SalePriceLine)enumerator.Current;
                                        partBaseDto linedto = new partBaseDto();
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
                                        linedto.salePrice = float.Parse(line.Price.ToString());
                                        linedto.unitPrace = linedto.salePrice;
                                        linedto.isDanger = "0";
                                        linedto.isReturn = "1";
                                        linedto.isSale = "1";
                                        linedto.isFlag = "1";
                                        linedto.isEffective = line.Active.ToString();
                                        linedto.actionType = 3;
                                        lines.Add(linedto);
                                    }
                                }
                                try
                                {
                                    if (lines.Count > 0)
                                    {
                                        partBaseDto t = service.Do(lines.ToArray());
                                        if (t != null && t.flag == 0)
                                        {
                                            throw new BusinessException(t.errMsg);
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
}
