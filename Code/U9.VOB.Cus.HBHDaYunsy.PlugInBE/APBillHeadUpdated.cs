using System;
using UFIDA.U9.AP.APBill;
using UFIDA.U9.CBO.FI.Enums;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI05;
using UFIDA.U9.SM.RMA;
using UFSoft.UBF.Business;
using UFSoft.UBF.Eventing;
using UFIDA.U9.Base;
using UFIDA.U9.SM.Ship;
using HBH.DoNet.DevPlatform.EntityMapping;
using System.Collections.Generic;
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
	public class APBillHeadUpdated : IEventSubscriber
    {
        Dictionary<long, accountReturnDto> dicAccountDTO = new Dictionary<long, accountReturnDto>();

		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
                if (!(key == null))
                {
                    dicAccountDTO = new Dictionary<long, accountReturnDto>();

                    APBillHead APbillhead = key.GetEntity() as APBillHead;
                    //if (voucher.Org.Code == "20")
                    if (PubHelper.IsOrg_Finance2DMS())
                    {
                        if (APbillhead.OriginalData.DocStatus == BillStatusEnum.Approving && APbillhead.DocStatus == BillStatusEnum.Approved)
                        {
                            //SI05ImplService service = new SI05ImplService();
                            // service.Url = PubHelper.GetAddress(service.Url);
                            foreach (APBillLine line in APbillhead.APBillLines)
                            {
                                if (line.SrcDocType == APBillSrcDocTypeEnum.RMA && line.SrcBillLineID > 0)
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
                                                //accountReturnDto dto = new accountReturnDto();
                                                dto.dealerCode = srcline.RMA.Customer.Customer.Code;
                                                dto.DMSShipNo = srcline.RMA.DescFlexField.PrivateDescSeg1;
                                                dto.dmsSaleNo = srcline.RMA.DescFlexField.PubDescSeg5;
                                                dto.earnestMoney = srcline.RMA.DescFlexField.PubDescSeg13;
                                                dto.deposit = srcline.RMA.DescFlexField.PubDescSeg21;
                                                dto.shipMoney = srcline.RMA.DescFlexField.PubDescSeg14;
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
                                                dto.amount = double.Parse((line.APOCMoney.NonTax + line.APOCMoney.GoodsTax).ToString());
                                                dto.operaTionType = "1";
                                            }
                                        }
                                    }
                                }
                            }

                            try
                            {
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
                                            SI05ImplService service = new SI05ImplService();
                                            accountReturnDto c = service.Do(dto);
                                            if (c != null && c.flag == 0)
                                            {
                                                throw new System.ApplicationException(c.errMsg);
                                            }
                                        }
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
	}
}
