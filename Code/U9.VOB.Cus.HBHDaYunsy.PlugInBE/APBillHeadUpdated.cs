using System;
using UFIDA.U9.AP.APBill;
using UFIDA.U9.CBO.FI.Enums;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI05;
using UFIDA.U9.SM.RMA;
using UFSoft.UBF.Business;
using UFSoft.UBF.Eventing;
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
	public class APBillHeadUpdated : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
					APBillHead APbillhead = key.GetEntity() as APBillHead;
					if (APbillhead.OriginalData.DocStatus == BillStatusEnum.Approving && APbillhead.DocStatus == BillStatusEnum.Approved)
					{
						SI05ImplService service = new SI05ImplService();
						// service.Url = PubHelper.GetAddress(service.Url);
						foreach (APBillLine line in APbillhead.APBillLines)
						{
							if (line.SrcDocType == APBillSrcDocTypeEnum.RMA && line.SrcBillLineID > 0)
							{
								RMALine srcline = RMALine.Finder.FindByID(line.SrcBillLineID);
								if (srcline != null)
								{
									accountReturnDto dto = new accountReturnDto();
									dto.dealerCode = srcline.RMA.Customer.Customer.Code;
									dto.DMSShipNo = srcline.RMA.DescFlexField.PrivateDescSeg1;
									dto.dmsSaleNo = srcline.RMA.DescFlexField.PubDescSeg5;
									dto.earnestMoney = srcline.RMA.DescFlexField.PubDescSeg13;
									dto.deposit = srcline.RMA.DescFlexField.PubDescSeg21;
									dto.shipMoney = srcline.RMA.DescFlexField.PubDescSeg14;
									if (srcline.RMA.Customer.Customer.CustomerCategoryKey != null)
									{
										dto.customerType = srcline.RMA.Customer.Customer.CustomerCategory.Code;
									}
									dto.vin = srcline.RMA.DescFlexField.PubDescSeg12;
									dto.amount = double.Parse((line.APOCMoney.NonTax + line.APOCMoney.GoodsTax).ToString());
									dto.operaTionType = "1";
									try
									{
										accountReturnDto c = service.Do(dto);
										if (c != null && c.flag == 0)
										{
											throw new System.ApplicationException(c.errMsg);
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
}
