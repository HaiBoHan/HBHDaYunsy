using System;
using System.Collections.Generic;
using UFIDA.U9.Base;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_PI07;
using UFIDA.U9.InvDoc.PrdEndCheck;
using UFSoft.UBF.Business;
using UFSoft.UBF.Eventing;
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
    // 库存盘点单
    /// <summary>
    /// 库存盘点单
    /// </summary>
	public class PrdEndChkBillUpdated : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
					PrdEndChkBill prdendChkBill = key.GetEntity() as PrdEndChkBill;
					if (PubHelper.SaleOrg2DMS.Contains(Context.LoginOrg.Code))
					{
						bool flag = PubHelper.IsUsedDMSAPI();
						if (flag)
						{
							if (prdendChkBill.OriginalData.State == CheckStatus.Approving && prdendChkBill.State == CheckStatus.Approved && prdendChkBill.ItemInfo.ItemID.StockCategory.Code == "26")
							{
								try
                                {
                                    //PI07ImplService service = new PI07ImplService();
                                    //DMSAsync_PI07.PI07Client service = new DMSAsync_PI07.PI07Client();
                                    //DMSAsync_PI07.PI07Client service = PubExtend.GetPI07Async();
                                    // 异步总会有返回结果丢失，改为同步了
                                    PI07ImplService service = new PI07ImplService();
                                    // service.Url = PubHelper.GetAddress(service.Url);
                                    //System.Collections.Generic.List<DMSAsync_PI07.stockDTO> list = new System.Collections.Generic.List<DMSAsync_PI07.stockDTO>();
                                    //DMSAsync_PI07.stockDTO dto = new DMSAsync_PI07.stockDTO();
                                    System.Collections.Generic.List<stockDTO> list = new System.Collections.Generic.List<stockDTO>();
                                    stockDTO dto = new stockDTO();
									dto.itemMaster = prdendChkBill.ItemInfo.ItemID.Code;
									if (prdendChkBill.Supplier != null && prdendChkBill.Supplier.Supplier != null)
									{
										dto.supplier = prdendChkBill.Supplier.Supplier.Code;
									}
									dto.number = ((prdendChkBill.AdjQtySU == 0) ? 0 : System.Convert.ToInt32(prdendChkBill.AdjQtySU));
									if (prdendChkBill.Lot != null)
									{
										dto.lot = prdendChkBill.Lot.LotCode;
									}
									if (prdendChkBill.Wh != null)
									{
										dto.WH = prdendChkBill.Wh.Code;
									}
									dto.actionType = 2;
									list.Add(dto);
                                    //DMSAsync_PI07.stockDTO[] t = service.Do(list.ToArray());
                                    stockDTO[] t = service.Do(list.ToArray());
                                    // 改为异步调用了
                                    //if (t != null && t.Length > 0 && t[0].flag == 0)
                                    //{
                                    //    throw new BusinessException(t[0].errMsg);
                                    //}
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
