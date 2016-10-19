using System;
using System.Collections.Generic;
using UFIDA.U9.Base;
using UFIDA.U9.CBO.SCM.Supplier;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_PI06;
using UFIDA.U9.SPR.SalePriceList;
using UFSoft.UBF.Business;
using UFSoft.UBF.Eventing;
using UFSoft.UBF.PL;
using UFIDA.U9.CBO.SCM.Enums;
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
    public class SupplySourceUpdated : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
                    SupplySource supplierItem = key.GetEntity() as SupplySource;
                    //if (Context.LoginOrg.Code == "10")
                    if (PubHelper.IsOrg_SupplierItem2DMS())
					{
						bool flag = PubHelper.IsUsedDMSAPI();
                        if (flag)
                        {
                            if (SupplySourceInserted.IsUpdateDMS(supplierItem))
                            {
                                //if (IsKeyChanged(supplierItem))
                                {
                                    PI06ImplService service = new PI06ImplService();
                                    // service.Url = PubHelper.GetAddress(service.Url);

                                    System.Collections.Generic.List<partBaseDto> lines = GetUpdateDMSDTO(supplierItem);

                                    if (lines != null
                                        && lines.Count > 0
                                        )
                                    {
                                        try
                                        {
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
			}
		}

        private static System.Collections.Generic.List<partBaseDto> GetUpdateDMSDTO(SupplySource supplierItem)
        {
            //// 如果修改供应商、和料品，那么删除旧的，新增新的；如果这两个不修改，就不需要更新DMS了；
            System.Collections.Generic.List<partBaseDto> lines = new System.Collections.Generic.List<partBaseDto>();
            // 关键字段改变，先删除旧的、再新增新的；
            if (IsKeyChanged(supplierItem))
            {
                partBaseDto linedto = new partBaseDto();
                if (supplierItem.OriginalData.SupplierInfo != null && supplierItem.OriginalData.SupplierInfo.SupplierKey != null)
                {
                    linedto.suptCode = supplierItem.OriginalData.SupplierInfo.Supplier.Code;
                }
                if (supplierItem.OriginalData.ItemInfo != null && supplierItem.OriginalData.ItemInfo.ItemIDKey != null)
                {
                    linedto.partCode = supplierItem.OriginalData.ItemInfo.ItemID.Code;
                    linedto.partName = supplierItem.OriginalData.ItemInfo.ItemID.Name;
                }
                if (supplierItem.OriginalData.ItemInfo.ItemID.InventoryUOM != null)
                {
                    linedto.unit = supplierItem.OriginalData.ItemInfo.ItemID.InventoryUOM.Name;
                }
                if (supplierItem.OriginalData.ItemInfo.ItemID.PurchaseInfo != null)
                {
                    linedto.miniPack = ((supplierItem.ItemInfo.ItemID.PurchaseInfo.MinRcvQty > 0) ? System.Convert.ToInt32(supplierItem.ItemInfo.ItemID.PurchaseInfo.MinRcvQty) : 1);
                }
                //SalePriceLine line = SalePriceLine.Finder.Find(string.Format("SalePriceList.Org={0} and ItemInfo.ItemID={1} and Active=1 and '{2}' between FromDate and ToDate", Context.LoginOrg.ID.ToString(), supplierItem.OriginalData.ItemInfo.ItemID.ID.ToString(), System.DateTime.Now.ToString()), new OqlParam[0]);
                SalePriceLine line = PubHelper.GetSalePriceList(supplierItem);
                if (line != null)
                {
                    linedto.salePrice = float.Parse(line.Price.ToString());
                    linedto.unitPrace = linedto.salePrice;
                }
                else
                {
                    linedto.salePrice = 0f;
                    linedto.unitPrace = 0f;
                }
                linedto.isFlag = "2";
                linedto.isDanger = "0";
                linedto.isReturn = "1";
                linedto.isSale = "1";
                linedto.isEffective = supplierItem.OriginalData.Effective.IsEffective.ToString();
                linedto.actionType = 3;
                lines.Add(linedto);
                linedto = new partBaseDto();
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
                SalePriceLine line2 = SalePriceLine.Finder.Find(string.Format("SalePriceList.Org={0} and ItemInfo.ItemID={1} and Active=1 and '{2}' between FromDate and ToDate", supplierItem.MasterOrg.ID.ToString(), supplierItem.ItemInfo.ItemID.ID.ToString(), System.DateTime.Now.ToString()), new OqlParam[0]);
                if (line2 != null)
                {
                    linedto.salePrice = float.Parse(line2.Price.ToString());
                    linedto.unitPrace = linedto.salePrice;
                }
                else
                {
                    linedto.salePrice = 0f;
                    linedto.unitPrace = 0f;
                }
                linedto.isFlag = "2";
                linedto.isDanger = "0";
                linedto.isReturn = "1";
                linedto.isSale = "1";
                linedto.isEffective = supplierItem.Effective.IsEffective.ToString();
                linedto.actionType = 1;
                lines.Add(linedto);
            }
            // UI更新，不管是否关键字段，都更新DMS（熊彬想实现手动同步dms操作）
            else // if(supplierItem.ActiveType == ActivityTypeEnum.SrvUpdate)
            {
                partBaseDto linedto = new partBaseDto();
                if (supplierItem.OriginalData.SupplierInfo != null && supplierItem.OriginalData.SupplierInfo.SupplierKey != null)
                {
                    linedto.suptCode = supplierItem.OriginalData.SupplierInfo.Supplier.Code;
                }
                if (supplierItem.OriginalData.ItemInfo != null && supplierItem.OriginalData.ItemInfo.ItemIDKey != null)
                {
                    linedto.partCode = supplierItem.OriginalData.ItemInfo.ItemID.Code;
                    linedto.partName = supplierItem.OriginalData.ItemInfo.ItemID.Name;
                }
                if (supplierItem.OriginalData.ItemInfo.ItemID.InventoryUOM != null)
                {
                    linedto.unit = supplierItem.OriginalData.ItemInfo.ItemID.InventoryUOM.Name;
                }
                if (supplierItem.OriginalData.ItemInfo.ItemID.PurchaseInfo != null)
                {
                    linedto.miniPack = ((supplierItem.ItemInfo.ItemID.PurchaseInfo.MinRcvQty > 0) ? System.Convert.ToInt32(supplierItem.ItemInfo.ItemID.PurchaseInfo.MinRcvQty) : 1);
                }
                //SalePriceLine line = SalePriceLine.Finder.Find(string.Format("SalePriceList.Org={0} and ItemInfo.ItemID={1} and Active=1 and '{2}' between FromDate and ToDate", Context.LoginOrg.ID.ToString(), supplierItem.OriginalData.ItemInfo.ItemID.ID.ToString(), System.DateTime.Now.ToString()), new OqlParam[0]);
                SalePriceLine line = PubHelper.GetSalePriceList(supplierItem);
                if (line != null)
                {
                    linedto.salePrice = float.Parse(line.Price.ToString());
                    linedto.unitPrace = linedto.salePrice;
                }
                else
                {
                    linedto.salePrice = 0f;
                    linedto.unitPrace = 0f;
                }
                linedto.isFlag = "2";
                linedto.isDanger = "0";
                linedto.isReturn = "1";
                linedto.isSale = "1";
                linedto.isEffective = supplierItem.OriginalData.Effective.IsEffective.ToString();
                linedto.actionType = 2;
                lines.Add(linedto);
            }
            return lines;
        }

        private static bool IsKeyChanged(SupplySource supplierItem)
        {
            return supplierItem.OriginalData.ItemInfo.ItemID != supplierItem.ItemInfo.ItemID || supplierItem.OriginalData.SupplierInfo.SupplierKey != supplierItem.SupplierInfo.SupplierKey;
        }
	}
}
