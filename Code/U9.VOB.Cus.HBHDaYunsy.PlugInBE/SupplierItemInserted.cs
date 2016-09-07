using System;
using System.Collections.Generic;
using UFIDA.U9.Base;
using UFIDA.U9.CBO.SCM.Supplier;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_PI06;
using UFIDA.U9.SPR.SalePriceList;
using UFSoft.UBF.Business;
using UFSoft.UBF.Eventing;
using UFSoft.UBF.PL;
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
	public class SupplierItemInserted : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
					SupplierItem supplierItem = key.GetEntity() as SupplierItem;
                    //if (Context.LoginOrg.Code == "10")
                    if (PubHelper.IsOrg_SupplierItem2DMS())
					{
						bool flag = PubHelper.IsUsedDMSAPI();
                        if (flag)
                        {
                            if (IsUpdateDMS(supplierItem)
                                )
                            {
                                try
                                {
                                    PI06ImplService service = new PI06ImplService();
                                    // service.Url = PubHelper.GetAddress(service.Url);
                                    partBaseDto linedto = new partBaseDto();
                                    System.Collections.Generic.List<partBaseDto> lines = new System.Collections.Generic.List<partBaseDto>();
                                    if (supplierItem.SupplierInfo != null && supplierItem.SupplierInfo.SupplierKey != null)
                                    {
                                        linedto.suptCode = supplierItem.SupplierInfo.Supplier.Code;
                                    }
                                    if (supplierItem.ItemInfo != null && supplierItem.ItemInfo.ItemIDKey != null)
                                    {
                                        linedto.partCode = supplierItem.ItemInfo.ItemID.Code;
                                        linedto.partName = supplierItem.ItemInfo.ItemID.Name;
                                    }
                                    if (supplierItem.ItemInfo.ItemID.InventoryUOM != null)
                                    {
                                        linedto.unit = supplierItem.ItemInfo.ItemID.InventoryUOM.Name;
                                    }
                                    if (supplierItem.ItemInfo.ItemID.PurchaseInfo != null)
                                    {
                                        linedto.miniPack = ((supplierItem.ItemInfo.ItemID.PurchaseInfo.MinRcvQty > 0) ? System.Convert.ToInt32(supplierItem.ItemInfo.ItemID.PurchaseInfo.MinRcvQty) : 1);
                                    }
                                    linedto.isFlag = "2";
                                    //SalePriceLine line = SalePriceLine.Finder.Find(string.Format("SalePriceList.Org={0} and ItemInfo.ItemID={1} and Active=1 and '{2}' between FromDate and ToDate", Context.LoginOrg.ID.ToString(), supplierItem.ItemInfo.ItemID.ID.ToString(), System.DateTime.Now.ToString()), new OqlParam[0]);
                                    SalePriceLine line = PubHelper.GetSalePriceList(supplierItem);
                                    if (line != null)
                                    {
                                        linedto.salePrice = float.Parse(line.Price.ToString("G0"));
                                        linedto.unitPrace = linedto.salePrice;
                                    }
                                    else
                                    {
                                        linedto.salePrice = 0f;
                                        linedto.unitPrace = 0f;
                                    }
                                    linedto.isDanger = "0";
                                    linedto.isReturn = "1";
                                    linedto.isSale = "1";
                                    linedto.isEffective = supplierItem.Effective.IsEffective.ToString();
                                    linedto.actionType = 1;
                                    lines.Add(linedto);
                                    partBaseDto d = service.Do(lines.ToArray());
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

        public static bool IsUpdateDMS(SupplierItem supplierItem)
        {
            return supplierItem != null
                                            && supplierItem.SupplierInfo != null
                                            && supplierItem.SupplierInfo.Supplier != null
                                            && SupplierInserted.IsUpdateDMS(supplierItem.SupplierInfo.Supplier);
        }
	}
}
