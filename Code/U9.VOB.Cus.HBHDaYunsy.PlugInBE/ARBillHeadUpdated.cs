using System;
using UFIDA.U9.AR.ARBill;
using UFIDA.U9.Base;
using UFIDA.U9.CBO.FI.Enums;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI05;
using UFIDA.U9.SM.RMA;
using UFIDA.U9.SM.Ship;
using UFSoft.UBF.Business;
using UFSoft.UBF.Eventing;
using HBH.DoNet.DevPlatform.EntityMapping;
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
	public class ARBillHeadUpdated : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
                    ARBillHead ARbillhead = key.GetEntity() as ARBillHead;
                    //if (voucher.Org.Code == "20")
                    if (PubHelper.IsOrg_Finance2DMS())
					{
						bool flag = PubHelper.IsUsedDMSAPI();
						if (flag)
						{
							if (ARbillhead.OriginalData.DocStatus == BillStatusEnum.Approving && ARbillhead.DocStatus == BillStatusEnum.Approved)
							{
								SI05ImplService service = new SI05ImplService();
								// service.Url = PubHelper.GetAddress(service.Url);
								bool flag2 = false;
								accountReturnDto dto = new accountReturnDto();
								foreach (ARBillLine line in ARbillhead.ARBillLines)
								{
									if (line.SrcDocType == ARBillSrcDocTypeEnum.ShipmentBill && line.SrcBillLineID > 0)
									{
										ShipLine shipline = ShipLine.Finder.FindByID(line.SrcBillLineID);
                                        if (IsUpdateDMS(shipline))
										{
											dto.dealerCode = ARbillhead.AccrueCust.Customer.Code;
											if (ARbillhead.AccrueCust.Customer.CustomerCategoryKey != null)
											{
												dto.customerType = ARbillhead.AccrueCust.Customer.CustomerCategory.Code;
											}
											dto.DMSShipNo = shipline.Ship.DescFlexField.PrivateDescSeg1;
											dto.dmsSaleNo = shipline.DescFlexField.PubDescSeg5;
											dto.earnestMoney = shipline.DescFlexField.PubDescSeg13;
											dto.vin = line.DescFlexField.PubDescSeg12;
											dto.deposit = shipline.DescFlexField.PubDescSeg21;
											dto.shipMoney = shipline.DescFlexField.PubDescSeg14;
											dto.UNineCreateUser = ARbillhead.CreatedBy;
											dto.amount += double.Parse((line.AROCMoney.NonTax + line.AROCMoney.GoodsTax).ToString());
											dto.operaTionType = "0";
											flag2 = true;
										}
									}
									else if (line.SrcDocType == ARBillSrcDocTypeEnum.RMA && line.SrcBillLineID > 0)
									{
                                        RMALine srcline = RMALine.Finder.FindByID(line.SrcBillLineID);
										if (srcline != null)
                                        {
                                            ShipLine shipline = null;
                                            if (srcline.SrcDocLine != null
                                                && srcline.SrcDocLine.ID > 0
                                                )
                                            {
                                                shipline = ShipLine.Finder.FindByID(srcline.SrcDocLine.ID);
                                            }

                                            if ((shipline == null
                                                && srcline.RMA.DescFlexField.PubDescSeg5.IsNotNullOrWhiteSpace()
                                                )
                                                || (shipline != null
                                                    && IsUpdateDMS(shipline)
                                                    )
                                                )
                                            {
                                                dto.dealerCode = srcline.RMA.Customer.Customer.Code;
                                                dto.DMSShipNo = shipline != null ? shipline.Ship.DescFlexField.PrivateDescSeg1
                                                    : srcline.RMA.DescFlexField.PrivateDescSeg1;
                                                dto.dmsSaleNo = srcline.RMA.DescFlexField.PubDescSeg5;
                                                dto.earnestMoney = srcline.RMA.DescFlexField.PubDescSeg13;
                                                dto.deposit = srcline.RMA.DescFlexField.PubDescSeg21;
                                                dto.shipMoney = srcline.RMA.DescFlexField.PubDescSeg14;
                                                dto.UNineCreateUser = ARbillhead.CreatedBy;
                                                if (srcline.RMA.Customer.Customer.CustomerCategoryKey != null)
                                                {
                                                    dto.customerType = srcline.RMA.Customer.Customer.CustomerCategory.Code;
                                                }
                                                dto.vin = srcline.RMA.DescFlexField.PubDescSeg12;
                                                dto.amount += double.Parse((line.AROCMoney.NonTax + line.AROCMoney.GoodsTax).ToString());
                                                dto.operaTionType = "1";
                                                flag2 = true;
                                            }
										}
									}
								}
								if (dto.amount != 0.0)
								{
									dto.amount = System.Math.Round(dto.amount, 2);
								}
								try
								{
									if (flag2)
									{
										accountReturnDto c = service.Do(dto);
										if (c != null && c.flag == 0)
										{
											throw new System.ApplicationException(c.errMsg);
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

        private static bool IsUpdateDMS(ShipLine shipline)
        {
            if (Context.LoginOrg.Code == PubHelper.Const_OrgCode_Electric)
            {
                if (shipline != null
                    && shipline.Ship != null
                    && shipline.Ship.DocumentType != null
                    && PubHelper.IsUpdateDMS_Electric(shipline.Ship.DocumentType)
                    )
                {
                    return true;
                }
            }
            else if (Context.LoginOrg.Code == PubHelper.Const_OrgCode_Chengdu
                || Context.LoginOrg.Code == PubHelper.Const_OrgCode_Hubei
                )
            {
                if (shipline != null && (shipline.Ship.DocumentType.Code == "XM10" || shipline.Ship.DocumentType.Code == "XM12" || shipline.Ship.DocumentType.Code == "XM1" || shipline.Ship.DocumentType.Code == "XM7" || shipline.Ship.DocumentType.Code == "XM4")
                    )
                {
                    return true;
                }
            }
            return false;
        }
	}
}
