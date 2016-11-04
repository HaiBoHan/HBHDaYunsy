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
using System.Collections.Generic;
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
	public class ARBillHeadUpdated : IEventSubscriber
    {
        Dictionary<long, accountReturnDto> dicAccountDTO = new Dictionary<long, accountReturnDto>();

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
                            //if (ARbillhead.OriginalData.DocStatus == BillStatusEnum.Approving && ARbillhead.DocStatus == BillStatusEnum.Approved)
                            int approveStatus = GetOperaTionType(ARbillhead);
                            if (approveStatus > -1)
                            {
                                //SI05ImplService service = new SI05ImplService();
                                // service.Url = PubHelper.GetAddress(service.Url);
                                bool flag2 = false;
                                //accountReturnDto dto = new accountReturnDto();
                                foreach (ARBillLine line in ARbillhead.ARBillLines)
                                {
                                    if (line.SrcDocType == ARBillSrcDocTypeEnum.ShipmentBill && line.SrcBillLineID > 0)
                                    {
                                        ShipLine shipline = ShipLine.Finder.FindByID(line.SrcBillLineID);
                                        if (PubHelper.IsUpdateDMS(shipline))
                                        {
                                            accountReturnDto dto = GetAccountDTO(shipline);

                                            if (dto != null)
                                            {
                                                dto.dealerCode = ARbillhead.AccrueCust.Customer.Code;
                                                //if (ARbillhead.AccrueCust.Customer.CustomerCategoryKey != null)
                                                //{
                                                //    dto.customerType = ARbillhead.AccrueCust.Customer.CustomerCategory.Code;
                                                //}

                                                if (Context.LoginOrg.Code == PubHelper.Const_OrgCode_Electric)
                                                {
                                                    // 电动车只有服务站
                                                    dto.customerType = "101006";
                                                }
                                                else
                                                {
                                                    if (ARbillhead.AccrueCust.Customer.CustomerCategoryKey != null)
                                                    {
                                                        dto.customerType = ARbillhead.AccrueCust.Customer.CustomerCategory.Code;
                                                    }
                                                }
                                                dto.DMSShipNo = shipline.Ship.DescFlexField.PrivateDescSeg1;
                                                dto.dmsSaleNo = shipline.DescFlexField.PubDescSeg5;
                                                dto.earnestMoney = shipline.DescFlexField.PubDescSeg13;
                                                dto.vin = line.DescFlexField.PubDescSeg12;
                                                dto.deposit = shipline.DescFlexField.PubDescSeg21;
                                                dto.shipMoney = shipline.DescFlexField.PubDescSeg14;
                                                dto.UNineCreateUser = ARbillhead.CreatedBy;
                                                dto.amount += double.Parse((line.AROCMoney.NonTax + line.AROCMoney.GoodsTax).ToString());
                                                // 0是发运扣款、1是退回退款
                                                //dto.operaTionType = "0";
                                                dto.operaTionType = approveStatus.ToString();
                                                flag2 = true;
                                            }
                                        }
                                    }
                                    else if (line.SrcDocType == ARBillSrcDocTypeEnum.RMA && line.SrcBillLineID > 0)
                                    {
                                        RMALine srcline = RMALine.Finder.FindByID(line.SrcBillLineID);
                                        if (srcline != null)
                                        {
                                            ShipLine shipline = null;
                                            if (srcline.SrcShipLine != null
                                                )
                                            {
                                                shipline = srcline.SrcShipLine;
                                            }

                                            if ((shipline == null
                                                && srcline.RMA.DescFlexField.PubDescSeg5.IsNotNullOrWhiteSpace()
                                                )
                                                || (shipline != null
                                                    && PubHelper.IsUpdateDMS(shipline)
                                                    )
                                                )
                                            {
                                                accountReturnDto dto = GetAccountDTO(shipline);

                                                if (dto != null)
                                                {
                                                    dto.dealerCode = srcline.RMA.Customer.Customer.Code;
                                                    dto.DMSShipNo = shipline != null ? shipline.Ship.DescFlexField.PrivateDescSeg1
                                                        : srcline.RMA.DescFlexField.PrivateDescSeg1;
                                                    dto.dmsSaleNo = srcline.RMA.DescFlexField.PubDescSeg5;
                                                    dto.earnestMoney = srcline.RMA.DescFlexField.PubDescSeg13;
                                                    dto.deposit = srcline.RMA.DescFlexField.PubDescSeg21;
                                                    dto.shipMoney = srcline.RMA.DescFlexField.PubDescSeg14;
                                                    dto.UNineCreateUser = ARbillhead.CreatedBy;
                                                    //if (srcline.RMA.Customer.Customer.CustomerCategoryKey != null)
                                                    //{
                                                    //    dto.customerType = srcline.RMA.Customer.Customer.CustomerCategory.Code;
                                                    //}

                                                    if (Context.LoginOrg.Code == PubHelper.Const_OrgCode_Electric)
                                                    {
                                                        // 电动车只有服务站
                                                        dto.customerType = "101006";
                                                    }
                                                    else
                                                    {
                                                        if (srcline.RMA.Customer.Customer.CustomerCategoryKey != null)
                                                        {
                                                            dto.customerType = srcline.RMA.Customer.Customer.CustomerCategory.Code;
                                                        }
                                                    }
                                                    dto.vin = srcline.RMA.DescFlexField.PubDescSeg12;
                                                    dto.amount += double.Parse((line.AROCMoney.NonTax + line.AROCMoney.GoodsTax).ToString());
                                                    // 0是发运扣款、1是退回退款
                                                    //dto.operaTionType = "1";
                                                    // 退货，取反
                                                    dto.operaTionType = (approveStatus == 1 ? 0 : (approveStatus == 0 ? 1 : -1)).ToString();
                                                    flag2 = true;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (dicAccountDTO != null
                                    && dicAccountDTO.Count > 0
                                    )
                                {
                                    foreach (accountReturnDto dto in dicAccountDTO.Values)
                                    {
                                        if (dto != null
                                            && dto.amount != 0.0
                                            )
                                        {
                                            dto.amount = System.Math.Round(dto.amount, 2);

                                            // 有DMS单号
                                            if (dto.DMSShipNo.IsNotNullOrWhiteSpace())
                                            {
                                                try
                                                {
                                                    if (flag2)
                                                    {
                                                        SI05ImplService service = new SI05ImplService();
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
					}
				}
			}
		}

        private accountReturnDto GetAccountDTO(ShipLine shipline)
        {
            if (shipline != null
                && shipline.Ship != null
                )
            {
                long shipID = shipline.Ship.ID;

                if (dicAccountDTO.ContainsKey(shipID))
                {
                    return dicAccountDTO[shipID];
                }
                else
                {
                    accountReturnDto accDTO = new accountReturnDto();

                    dicAccountDTO.Add(shipID, accDTO);

                    return accDTO;
                }
            }

            return null;
        }

        private int GetOperaTionType(ARBillHead ARbillhead)
        {
            // 0是发运扣款、1是退回退款
            int approveType = -1;

            if (ARbillhead.OriginalData.DocStatus == BillStatusEnum.Approving 
                && ARbillhead.DocStatus == BillStatusEnum.Approved
                )
            {
                approveType = 0;
            }
            else if (ARbillhead.OriginalData.DocStatus == BillStatusEnum.Approved 
                && ARbillhead.DocStatus == BillStatusEnum.Opened
                )
            {
                approveType = 1;
            }

            return approveType;
        }
	}
}
