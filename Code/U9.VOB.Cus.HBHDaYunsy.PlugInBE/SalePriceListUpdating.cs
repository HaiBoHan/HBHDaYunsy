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
	public class SalePriceListUpdating : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
                    SalePriceList SalepriceList = key.GetEntity() as SalePriceList;
                    if (PubHelper.IsOrg_SalePriceList2DMS(SalepriceList))
					{
						bool flag = PubHelper.IsUsedDMSAPI();
                        if (flag)
                        {
                            // 电动车配件价表
                            //if (PubHelper.PriceList2DMS.Contains(SalepriceList.Code))
                            if (SalePriceListInserted.IsUpdateDMS(SalepriceList))
                            {
                                PI06ImplService service = new PI06ImplService();
                                // service.Url = PubHelper.GetAddress(service.Url);
                                System.Collections.Generic.List<partBaseDto> lines = new System.Collections.Generic.List<partBaseDto>();
                                foreach (SalePriceLine line in SalepriceList.SalePriceLines)
                                {
                                    //if (line.SysState != 8)
                                    if (line.SysState != UFSoft.UBF.PL.Engine.ObjectState.Deleted)
                                    {
                                        //if (line.SysState != 2)
                                        if (line.SysState != UFSoft.UBF.PL.Engine.ObjectState.Inserted)
                                        {
                                            if (line.OriginalData.Price != line.Price)
                                            {
                                                if (line.Active && System.DateTime.Now >= line.FromDate && (System.DateTime.Now < line.ToDate || line.ToDate.ToString() == "9999.12.31"))
                                                {
                                                    //SupplierItem.EntityList supitemlist = SupplierItem.Finder.FindAll(string.Format("Org={0} and ItemInfo.ItemID={1} and Effective.IsEffective=1 and '{2}' between Effective.EffectiveDate and Effective.DisableDate", Context.LoginOrg.ID.ToString(), line.ItemInfo.ItemID.ID.ToString(), System.DateTime.Now.ToString()), new OqlParam[0]);
                                                    SupplySource.EntityList supitemlist = SupplySource.Finder.FindAll("ItemInfo.ItemCode=@ItemCode and Effective.IsEffective=1 and @Now between Effective.EffectiveDate and Effective.DisableDate"
                                                        , new OqlParam(line.ItemInfo.ItemCode)
                                                        , new OqlParam(System.DateTime.Today)
                                                        );
                                                    if (supitemlist != null && supitemlist.Count > 0)
                                                    {
                                                        foreach (SupplySource i in supitemlist)
                                                        {
                                                            partBaseDto linedto = new partBaseDto();
                                                            linedto.suptCode = i.SupplierInfo.Supplier.Code;
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
                                                            linedto.salePrice = float.Parse(line.Price.ToString("G0"));
                                                            linedto.unitPrace = linedto.salePrice;
                                                            linedto.isDanger = "0";
                                                            linedto.isReturn = "1";
                                                            linedto.isSale = "1";
                                                            linedto.isFlag = "1";
                                                            linedto.isEffective = line.Active.ToString();
                                                            linedto.actionType = 2;
                                                            lines.Add(linedto);
                                                        }
                                                    }
                                                    else
                                                    {
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
                                                        linedto.actionType = 2;
                                                        lines.Add(linedto);
                                                    }
                                                }
                                            }
                                        }
                                        else if (line.Active && System.DateTime.Now >= line.FromDate && (System.DateTime.Now < line.ToDate || line.ToDate.ToString() == "9999.12.31"))
                                        {
                                            //SupplierItem.EntityList supitemlist = SupplierItem.Finder.FindAll(string.Format("Org={0} and ItemInfo.ItemID={1} and Effective.IsEffective=1 and '{2}' between Effective.EffectiveDate and Effective.DisableDate", Context.LoginOrg.ID.ToString(), line.ItemInfo.ItemID.ID.ToString(), System.DateTime.Now.ToString()), new OqlParam[0]);
                                            //SupplySource.EntityList supitemlist = SupplySource.Finder.FindAll(string.Format("Org={0} and ItemInfo.ItemID={1} and Effective.IsEffective=1 and '{2}' between Effective.EffectiveDate and Effective.DisableDate", Context.LoginOrg.ID.ToString(), line.ItemInfo.ItemID.ID.ToString(), System.DateTime.Now.ToString()), new OqlParam[0]);
                                            SupplySource.EntityList supitemlist = SupplySource.Finder.FindAll("ItemInfo.ItemCode=@ItemCode and Effective.IsEffective=1 and @Now between Effective.EffectiveDate and Effective.DisableDate"
                                                , new OqlParam(line.ItemInfo.ItemCode)
                                                , new OqlParam(System.DateTime.Today)
                                                );
                                            if (supitemlist != null && supitemlist.Count > 0)
                                            {
                                                foreach (SupplySource i in supitemlist)
                                                {
                                                    partBaseDto linedto = new partBaseDto();
                                                    linedto.suptCode = i.SupplierInfo.Supplier.Code;
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
                                                    linedto.actionType = 2;
                                                    lines.Add(linedto);
                                                }
                                            }
                                            else
                                            {
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
                                                linedto.actionType = 2;
                                                lines.Add(linedto);
                                            }
                                        }
                                    }
                                }
                                // 不处理删除了，只处理生效就好了
                                using (System.Collections.Generic.IEnumerator<IPersistableObject> enumerator3 = SalepriceList.SalePriceLines.DelLists.GetEnumerator())
                                {
                                    while (enumerator3.MoveNext())
                                    {
                                        //SalePriceLine line = (SalePriceLine)enumerator3.Current;
                                        //partBaseDto linedto = new partBaseDto();
                                        //linedto.partCode = line.ItemInfo.ItemID.Code;
                                        //linedto.partName = line.ItemInfo.ItemID.Name;
                                        //if (line.ItemInfo.ItemID.InventoryUOM != null)
                                        //{
                                        //    linedto.unit = line.ItemInfo.ItemID.InventoryUOM.Name;
                                        //}
                                        //if (line.ItemInfo.ItemID.PurchaseInfo != null)
                                        //{
                                        //    linedto.miniPack = ((line.ItemInfo.ItemID.PurchaseInfo.MinRcvQty > 0) ? System.Convert.ToInt32(line.ItemInfo.ItemID.PurchaseInfo.MinRcvQty) : 1);
                                        //}
                                        //linedto.salePrice = float.Parse(line.Price.ToString());
                                        //linedto.unitPrace = linedto.salePrice;
                                        //linedto.isDanger = "0";
                                        //linedto.isReturn = "1";
                                        //linedto.isSale = "1";
                                        //linedto.isFlag = "1";
                                        //linedto.isEffective = line.Active.ToString();
                                        //linedto.actionType = 3;
                                        //lines.Add(linedto);
                                    }
                                }
                                try
                                {
                                    if (lines.Count > 0)
                                    {
                                        partBaseDto t = service.Do(lines.ToArray());
                                        if (t != null && t.flag == 0)
                                        {
                                            throw new System.ApplicationException(t.errMsg);
                                        }
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
