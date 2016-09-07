namespace UFIDA.U9.Cust.HBDY.API.DipatchOutWhAPISV
{
	using System;
	using System.Collections.Generic;
    using System.Text;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.Util.Context;
    using UFIDA.U9.ISV.SM;
    using UFIDA.U9.CBO.Pub.Controller;
    using UFSoft.UBF.Transactions;
    using UFIDA.U9.ISV.SM.Proxy;
    using UFIDA.U9.ISV.MiscShipISV.Proxy;
    using UFIDA.U9.SM.Ship;
    using UFIDA.U9.InvDoc.MiscShip;
    using UFIDA.U9.CBO.SCM.Customer;
    using UFSoft.UBF.PL;
    using UFIDA.U9.CBO.SCM.Item;
    using UFIDA.U9.Base;
    using UFIDA.U9.Lot;
    using UFIDA.U9.CBO.SCM.Supplier;
    using HBH.DoNet.DevPlatform.EntityMapping;
    using UFIDA.U9.CBO.HR.Operator;
    using UFIDA.U9.Base.FlexField.DescFlexField;
    using UFIDA.U9.CBO.SCM.Warehouse;
    using UFIDA.U9.CBO.SCM.PropertyTypes;
    using UFIDA.U9.Base.PropertyTypes;
    using UFIDA.U9.ISV.MiscShipISV;
    using UFIDA.U9.InvTrans.WhQoh.Proxy;
    using UFIDA.U9.InvTrans.WhQoh;
    using UFIDA.U9.InvDoc.TransferIn.Proxy;
    using UFIDA.U9.ISV.TransferInISV.Proxy;
    using UFIDA.U9.InvDoc.TransferIn;
    using HBH.DoNet.DevPlatform.U9Mapping;
    using UFIDA.U9.Cust.HBDY.API.ShipSV;
    using UFIDA.U9.Cust.HBDY.API;

	/// <summary>
	/// DispatchOutWhCarSV partial 
	/// </summary>	
    public partial class DispatchOutWhCarSV // : HBHTransferSV
	{	
		internal BaseStrategy Select()
		{
			return new DispatchOutWhCarSVImpementStrategy();	
		}		
	}
	
    //#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class DispatchOutWhCarSVImpementStrategy : BaseStrategy
	{
		public DispatchOutWhCarSVImpementStrategy() { }

		public override object Do(object obj)
		{						
			DispatchOutWhCarSV bpObj = (DispatchOutWhCarSV)obj;
			
			//get business operation context is as follows
			//IContext context = ContextManager.Context	
			
			//auto generating code end,underside is user custom code
            //and if you Implement replace this Exception Code...

            //long svID = HBHCommon.HBHCommonSVBefore(bpObj);

            List<ShipBackDTO> result2 = CreateDispatch(bpObj);

            //if (result2 != null
            //    && result2.Count > 0
            //    )
            //{
            //    ShipBackDTO first = PubClass.GetFirst<ShipBackDTO>(result2);

            //    if (first != null)
            //    {
            //        HBHCommon.HBHCommonSVAfter(svID, result2, first.IsSuccess, first.ErrorInfo, first.ERPDocNo);
            //    }
            //}

            return result2;
        }

        private System.Collections.Generic.List<ShipBackDTO> CreateDispatch(DispatchOutWhCarSV bpObj)
        {
            System.Collections.Generic.List<ShipBackDTO> result = new System.Collections.Generic.List<ShipBackDTO>();
            //object result2;
            try
            {
                if (bpObj.CarShipLineDTOs == null || bpObj.CarShipLineDTOs.Count == 0)
                {
                    string msg = "传入参数不可为空";
                    result.Add(new ShipBackDTO
                    {
                        IsSuccess = false,
                        ErrorInfo = msg,
                        Timestamp = System.DateTime.Now
                    });
                    //result2 = result;
                    HBHCommon.LoggerError(msg);
                }
                else
                {
                    System.Collections.Generic.List<CarShipLineDTO> shiplist = new System.Collections.Generic.List<CarShipLineDTO>();
                    System.Collections.Generic.List<CarShipLineDTO> transferinlist = new System.Collections.Generic.List<CarShipLineDTO>();
                    string errormessage = this.ValidateParamNullOrEmpty(bpObj, ref shiplist, ref transferinlist);
                    if (!string.IsNullOrEmpty(errormessage))
                    {
                        string msg = "请检查传入参数";
                        result.Add(new ShipBackDTO
                        {
                            IsSuccess = false,
                            ErrorInfo = errormessage + "请检查传入参数",
                            Timestamp = System.DateTime.Now
                        });
                        //result2 = result;
                        HBHCommon.LoggerError(msg);
                    }
                    else
                    {
                        System.Collections.Generic.List<DocKeyDTOData> shipidlist = new System.Collections.Generic.List<DocKeyDTOData>();
                        System.Collections.Generic.List<CommonArchiveDataDTOData> transinidlist = new System.Collections.Generic.List<CommonArchiveDataDTOData>();
                        if (shiplist != null && shiplist.Count > 0)
                        {
                            try
                            {
                                CreateShipSVProxy proxy = new CreateShipSVProxy();
                                proxy.ShipDTOs = (this.GetShipDTOList(shiplist));
                                shipidlist = proxy.Do();
                            }
                            catch (System.Exception e)
                            {
                                //result.Add(new ShipBackDTO
                                //{
                                //    IsSuccess = false,
                                //    ErrorInfo = "生成出货单失败：" + e.Message,
                                //    Timestamp = System.DateTime.Now
                                //});
                                //result2 = result;
                                //return result2;
                                ShipBackDTO backDTO = new ShipBackDTO();
                                backDTO.IsSuccess = false;
                                backDTO.ErrorInfo = "生成出货单失败：" + e.Message;
                                backDTO.Timestamp = System.DateTime.Now;
                                HBHCommon.LoggerError(backDTO.ErrorInfo + "/r/n" + e.StackTrace);
                                result.Add(backDTO);
                                return result;
                            }
                            if (shipidlist == null || shipidlist.Count <= 0)
                            {
                                //result.Add(new ShipBackDTO
                                //{
                                //    IsSuccess = false,
                                //    ErrorInfo = "生单失败：没有生成出货单",
                                //    Timestamp = System.DateTime.Now
                                //});
                                //result2 = result;
                                //return result2;

                                ShipBackDTO backDTO = new ShipBackDTO();
                                backDTO.IsSuccess = false;
                                backDTO.ErrorInfo = "生单失败：没有生成出货单";
                                backDTO.Timestamp = System.DateTime.Now;
                                HBHCommon.LoggerError(backDTO.ErrorInfo );
                                result.Add(backDTO);
                                return result;
                            }
                        }
                        if (transferinlist != null && transferinlist.Count > 0)
                        {
                            //using (UBFTransactionScope trans = new UBFTransactionScope(TransactionOption.Required))
                            {
                                try
                                {
                                    UFIDA.U9.ISV.TransferInISV.Proxy.CommonCreateTransferInSVProxy proxy2 = new UFIDA.U9.ISV.TransferInISV.Proxy.CommonCreateTransferInSVProxy();
                                    proxy2.TransferInDTOList = (this.GetTransferInDTOList(transferinlist));
                                    transinidlist = proxy2.Do();
                                    if (transinidlist == null || transinidlist.Count <= 0)
                                    {
                                        //result.Add(new ShipBackDTO
                                        //{
                                        //    IsSuccess = false,
                                        //    ErrorInfo = "生单失败：没有生成调入单",
                                        //    Timestamp = System.DateTime.Now
                                        //});
                                        //result2 = result;
                                        //return result2;

                                        ShipBackDTO backDTO = new ShipBackDTO();
                                        backDTO.IsSuccess = false;
                                        backDTO.ErrorInfo = "生单失败：没有生成调入单";
                                        backDTO.Timestamp = System.DateTime.Now;
                                        HBHCommon.LoggerError(backDTO.ErrorInfo);
                                        result.Add(backDTO);
                                        return result;
                                    }
                                    TransferInBatchApproveSRVProxy approveproxy = new TransferInBatchApproveSRVProxy();
                                    approveproxy.DocList = (transinidlist);
                                    approveproxy.ApprovedBy = (Context.LoginUser);
                                    approveproxy.ApprovedOn = (System.DateTime.Now);
                                    approveproxy.Do();
                                    //trans.Commit();
                                }
                                catch (System.Exception e)
                                {
                                    //trans.Rollback();
                                    //result.Add(new ShipBackDTO
                                    //{
                                    //    IsSuccess = false,
                                    //    ErrorInfo = "生成调入单失败：" + e.Message,
                                    //    Timestamp = System.DateTime.Now
                                    //});
                                    //result2 = result;
                                    //return result2;
                                    ShipBackDTO backDTO = new ShipBackDTO();
                                    backDTO.IsSuccess = false;
                                    backDTO.ErrorInfo = "生成调入单失败：" + e.Message;
                                    backDTO.Timestamp = System.DateTime.Now;
                                    HBHCommon.LoggerError(backDTO.ErrorInfo + "/r/n" + e.StackTrace);
                                    result.Add(backDTO);
                                    return result;
                                }
                            }
                        }
                        foreach (DocKeyDTOData shipid in shipidlist)
                        {
                            Ship ship = Ship.Finder.FindByID(shipid.DocID);
                            if (ship != null)
                            {
                                result.Add(new ShipBackDTO
                                {
                                    IsSuccess = true,
                                    ErrorInfo = "生单出货单成功",
                                    Timestamp = System.DateTime.Now,
                                    ERPDocNo = shipid.DocNO,
                                    DMSDocNo = ship.DescFlexField.PubDescSeg7
                                });
                            }
                        }
                        foreach (CommonArchiveDataDTOData transin in transinidlist)
                        {
                            TransferIn t = TransferIn.Finder.FindByID(transin.ID);
                            if (t != null)
                            {
                                result.Add(new ShipBackDTO
                                {
                                    IsSuccess = true,
                                    ErrorInfo = "生单调入单成功",
                                    Timestamp = System.DateTime.Now,
                                    ERPDocNo = transin.Code,
                                    DMSDocNo = t.TransInLines[0].DescFlexSegments.PubDescSeg5
                                });
                            }
                        }
                        //result2 = result;
                    }
                }
            }
            catch (System.Exception e)
            {
                //result.Add(new ShipBackDTO
                //{
                //    IsSuccess = false,
                //    ErrorInfo = e.Message,
                //    Timestamp = System.DateTime.Now
                //});
                //result2 = result;

                ShipBackDTO backDTO = new ShipBackDTO();
                backDTO.IsSuccess = false;
                backDTO.ErrorInfo = e.Message;
                backDTO.Timestamp = System.DateTime.Now;
                HBHCommon.LoggerError(backDTO.ErrorInfo + "/r/n" + e.StackTrace);
                result.Add(backDTO);
            }
            //return result2;
            return result;
        }
        /// <summary>
        /// 传入参数非空校验
        /// </summary>
        /// <param name="bpObj"></param>
        private string ValidateParamNullOrEmpty(DispatchOutWhCarSV bpObj, ref System.Collections.Generic.List<CarShipLineDTO> shiplist, ref System.Collections.Generic.List<CarShipLineDTO> transferinlist)
        {
            string errormessage = string.Empty;
            foreach (CarShipLineDTO linedto in bpObj.CarShipLineDTOs)
            {
                if (linedto.OrderType == 401103 && !linedto.IsSale)
                {
                    if (string.IsNullOrEmpty(linedto.WhOut))
                    {
                        errormessage += string.Format("[{0}]DMS销售出库单的[调出存储地点({1})]不可为空,", linedto.DMSShipNo, linedto.WhOut);
                    }
                    else
                    {
                        Warehouse whout = Warehouse.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), linedto.WhOut), new OqlParam[0]);
                        if (whout == null)
                        {
                            errormessage += string.Format("[{0}]DMS销售出库单的[调出存储地点({1})]在U9系统中找不到对应的存储地点档案,请同步,", linedto.DMSShipNo, linedto.WhOut);
                        }
                    }
                    if (string.IsNullOrEmpty(linedto.WhIn))
                    {
                        errormessage += string.Format("[{0}]DMS销售出库单的[调入存储地点({1})]不可为空,", linedto.DMSShipNo, linedto.WhIn);
                    }
                    else
                    {
                        Warehouse whin = Warehouse.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), linedto.WhIn), new OqlParam[0]);
                        if (whin == null)
                        {
                            errormessage += string.Format("[{0}]DMS销售出库单的[调入存储地点({1})]在U9系统中找不到对应的存储地点档案,请同步,", linedto.DMSShipNo, linedto.WhIn);
                        }
                    }
                    transferinlist.Add(linedto);
                }
                else
                {
                    if (linedto.OrderType != 401103 || !linedto.IsSale)
                    {
                        if (linedto.VehicleOrChassis <= 0)
                        {
                            errormessage += string.Format("[{0}]DMS销售出库单的[整车或底盘]不可为空,", linedto.DMSShipNo);
                        }
                    }
                    if (string.IsNullOrEmpty(linedto.DealerCode))
                    {
                        errormessage += string.Format("[{0}]DMS销售出库单的[经销商代码({1})]不可为空,", linedto.DMSShipNo, linedto.DealerCode);
                    }
                    else
                    {
                        Customer customer = Customer.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), linedto.DealerCode), new OqlParam[0]);
                        if (customer == null)
                        {
                            errormessage += string.Format("[{0}]DMS销售出库单的[经销商代码({1})]在U9系统中找不到对应的客户档案,请同步,", linedto.DMSShipNo, linedto.DealerCode);
                        }
                    }
                    shiplist.Add(linedto);
                }
                if (string.IsNullOrEmpty(linedto.ErpMaterialCode))
                {
                    errormessage += string.Format("[{0}]DMS销售出库单的[ERP料号]不可为空,", linedto.DMSShipNo);
                }
                else
                {
                    ItemMaster item = ItemMaster.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), linedto.ErpMaterialCode), new OqlParam[0]);
                    if (item == null)
                    {
                        errormessage += string.Format("[{0}]DMS销售出库单的[ERP料号][{1}]在U9系统中找不到对应的料品档案,请同步,", linedto.DMSShipNo, linedto.ErpMaterialCode);
                    }
                }
                if (linedto.Number <= 0m)
                {
                    errormessage += string.Format("[{0}]DMS销售出库单的[数量]必须大于0,", linedto.DMSShipNo);
                }

                if (linedto.SpitOrderFlag.IsNull())
                {
                    linedto.SpitOrderFlag = HBHCommon.DefaultSplitFlag;
                }
            }
            return errormessage;
        }
        /// <summary>
        /// 得到出货单dto
        /// </summary>
        /// <param name="bpObj"></param>
        /// <returns></returns>
        private System.Collections.Generic.List<UFIDA.U9.ISV.SM.ShipDTOForIndustryChainData> GetShipDTOList(System.Collections.Generic.List<CarShipLineDTO> shiplist)
        {
            System.Collections.Generic.List<UFIDA.U9.ISV.SM.ShipDTOForIndustryChainData> list = new System.Collections.Generic.List<UFIDA.U9.ISV.SM.ShipDTOForIndustryChainData>();
            string opeatorstr = "DMSTESTUSER";
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<CarShipLineDTO>> dic = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<CarShipLineDTO>>();
            foreach (CarShipLineDTO dtoline in shiplist)
            {
                if (!dic.ContainsKey(dtoline.SpitOrderFlag))
                {
                    dic.Add(dtoline.SpitOrderFlag, new System.Collections.Generic.List<CarShipLineDTO>());
                }
                dic[dtoline.SpitOrderFlag].Add(dtoline);
            }
            foreach (string key in dic.Keys)
            {
                List<CarShipLineDTO> listLineDTO = dic[key];

                if (listLineDTO != null
                    && listLineDTO.Count > 0
                    )
                {
                    CarShipLineDTO firstDTO = listLineDTO.GetFirst<CarShipLineDTO>();

                    UFIDA.U9.ISV.SM.ShipDTOForIndustryChainData shipdto = new UFIDA.U9.ISV.SM.ShipDTOForIndustryChainData();
                    shipdto.CreatedBy = (Context.LoginUser);
                    shipdto.ModifiedBy = (Context.LoginUser);
                    string doctypecode = string.Empty;
                    shipdto.DocumentType = (new CommonArchiveDataDTOData());
                    if (firstDTO.OrderType == 401103 && firstDTO.IsSale)
                    {
                        doctypecode = "XM7";
                    }
                    else if (firstDTO.VehicleOrChassis == 400102)
                    {
                        doctypecode = "XM4";
                    }
                    else
                    {
                        doctypecode = "XM1";
                    }
                    shipdto.DocumentType.Code = (doctypecode);
                    long ConfirmAccording = -1L;
                    int ConfirmMode = -1;
                    long ConfirmTerm = -1L;
                    long InvoiceAccording = -1L;
                    long ReceivableTerm = -1L;
                    ShipDocType doctype = ShipDocType.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), doctypecode), new OqlParam[0]);
                    if (doctype != null)
                    {
                        if (doctype.ConfirmAccordingKey != null)
                        {
                            ConfirmAccording = doctype.ConfirmAccordingKey.ID;
                        }
                        if (doctype.ConfirmMode.Value >= 0)
                        {
                            ConfirmMode = doctype.ConfirmMode.Value;
                        }
                        if (doctype.InvoiceAccordingKey != null)
                        {
                            InvoiceAccording = doctype.InvoiceAccordingKey.ID;
                        }
                    }
                    Customer customer = Customer.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), firstDTO.DealerCode), new OqlParam[0]);
                    if (customer != null)
                    {
                        if (customer.ARConfirmTermKey != null)
                        {
                            ConfirmTerm = customer.ARConfirmTermKey.ID;
                        }
                        if (customer.RecervalTermKey != null)
                        {
                            ReceivableTerm = customer.RecervalTermKey.ID;
                        }
                        shipdto.BargainMode = (customer.Bargain.Value);
                        shipdto.ShipmentRule = (new CommonArchiveDataDTOData());
                        if (customer.ShippmentRule != null)
                        {
                            shipdto.ShipmentRule.Code = (customer.ShippmentRule.Code);
                        }
                        else
                        {
                            shipdto.ShipmentRule.Code = ("C001");
                        }
                        if (customer.RecervalTerm != null)
                        {
                            string RecTerm = customer.RecervalTerm.Code;
                        }
                    }
                    else
                    {
                        shipdto.ShipmentRule.Code = ("C001");
                        shipdto.BargainMode = (0);
                    }
                    shipdto.SrcDocType = (0);
                    shipdto.ReceivableTerm = (new CommonArchiveDataDTOData());
                    if (ReceivableTerm > 0L)
                    {
                        shipdto.ReceivableTerm.ID = (ReceivableTerm);
                    }
                    else
                    {
                        shipdto.ReceivableTerm.Code = ("01");
                    }
                    shipdto.ConfirmTerm = (new CommonArchiveDataDTOData());
                    if (ConfirmTerm > 0L)
                    {
                        shipdto.ConfirmTerm.ID = (ConfirmTerm);
                    }
                    else
                    {
                        shipdto.ConfirmTerm.Code = ("01");
                    }
                    shipdto.ConfirmAccording = (new CommonArchiveDataDTOData());
                    shipdto.ConfirmAccording.ID = (ConfirmAccording);
                    shipdto.ConfirmMode = ((ConfirmMode < 0) ? 0 : ConfirmMode);
                    shipdto.InvoiceAccording = (new CommonArchiveDataDTOData());
                    shipdto.InvoiceAccording.ID = (InvoiceAccording);
                    shipdto.Seller = (new CommonArchiveDataDTOData());
                    shipdto.Seller.Code = (opeatorstr);
                    Operators opeator = Operators.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), opeatorstr), new OqlParam[0]);
                    if (opeator != null)
                    {
                        shipdto.SaleDept = (new CommonArchiveDataDTOData());
                        shipdto.SaleDept.ID = (opeator.DeptKey.ID);
                    }
                    shipdto.CreatedBy = (Context.LoginUser);
                    shipdto.CreatedOn = (System.DateTime.Now);
                    shipdto.OrderBy = (new CommonArchiveDataDTOData());
                    shipdto.OrderBy.Code = (firstDTO.DealerCode);
                    shipdto.AC = (new CommonArchiveDataDTOData());
                    shipdto.AC.Code = (string.IsNullOrEmpty(firstDTO.Currency) ? "C001" : firstDTO.Currency);
                    shipdto.TC = (new CommonArchiveDataDTOData());
                    shipdto.TC.Code = (string.IsNullOrEmpty(firstDTO.Currency) ? "C001" : firstDTO.Currency);
                    shipdto.DescFlexField = (new DescFlexSegmentsData());
                    shipdto.DescFlexField.PubDescSeg5 = (firstDTO.DmsSaleNo);
                    shipdto.DescFlexField.PrivateDescSeg1 = (firstDTO.DMSShipNo);
                    System.DateTime arg_5CD_0 = firstDTO.ShipDate;
                    if (firstDTO.ShipDate != System.DateTime.MinValue && firstDTO.ShipDate > System.DateTime.Now)
                    {
                        shipdto.BusinessDate = (firstDTO.ShipDate);
                    }
                    else
                    {
                        shipdto.BusinessDate = (System.DateTime.Now);
                    }
                    shipdto.ShipLines = (new System.Collections.Generic.List<UFIDA.U9.ISV.SM.ShipLineDTOForIndustryChainData>());
                    foreach (CarShipLineDTO linedto in listLineDTO)
                    {
                        UFIDA.U9.ISV.SM.ShipLineDTOForIndustryChainData shiplinedto = new UFIDA.U9.ISV.SM.ShipLineDTOForIndustryChainData();
                        shiplinedto.ItemInfo = (new ItemInfoData());
                        shiplinedto.ItemInfo.ItemCode = (linedto.ErpMaterialCode);
                        shiplinedto.ShipQtyTUAmount = (linedto.Number);
                        shiplinedto.TotalMoneyTC = (linedto.Money);
                        shiplinedto.TotalNetMoneyTC = (linedto.Money);
                        shiplinedto.Project = (new CommonArchiveDataDTOData());
                        shiplinedto.Project.Code = (linedto.DmsSaleNo);
                        shiplinedto.ConfirmAccording = (new CommonArchiveDataDTOData());
                        shiplinedto.ConfirmAccording.ID = (ConfirmAccording);
                        shiplinedto.ConfirmMode = ((ConfirmMode < 0) ? 0 : ConfirmMode);
                        shiplinedto.ConfirmTerm = (new CommonArchiveDataDTOData());
                        if (ConfirmTerm > 0L)
                        {
                            shiplinedto.ConfirmTerm.ID = (ConfirmTerm);
                        }
                        else
                        {
                            shiplinedto.ConfirmTerm.Code = ("01");
                        }
                        shiplinedto.InvoiceAccording = (new CommonArchiveDataDTOData());
                        shiplinedto.InvoiceAccording.ID = (InvoiceAccording);
                        shiplinedto.ReceivableTerm = (new CommonArchiveDataDTOData());
                        if (ReceivableTerm > 0L)
                        {
                            shiplinedto.ReceivableTerm.ID = (ReceivableTerm);
                        }
                        else
                        {
                            shiplinedto.ReceivableTerm.Code = ("01");
                        }
                        shiplinedto.WH = (new CommonArchiveDataDTOData());
                        string whcode = string.Empty;
                        if (firstDTO.OrderType == 401103 && firstDTO.IsSale)
                        {
                            whcode = "0080";
                        }
                        else if (firstDTO.VehicleOrChassis == 400102)
                        {
                            whcode = "0090";
                        }
                        else
                        {
                            whcode = "0080";
                        }
                        shiplinedto.WH.Code = (whcode);
                        Warehouse whout = Warehouse.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), whcode), new OqlParam[0]);
                        if (whout != null && whout.DepositType == DepositTypeEnum.VMI)
                        {
                            shiplinedto.VMI = (true);
                            if (whout.Supplier != null)
                            {
                                shiplinedto.Supplier = (new CommonArchiveDataDTOData());
                                shiplinedto.Supplier.Code = (whout.Supplier.Code);
                            }
                        }
                        shiplinedto.DescFlexField = (new DescFlexSegmentsData());
                        shiplinedto.DescFlexField.PubDescSeg5 = (linedto.DmsSaleNo);
                        shiplinedto.Project = (new CommonArchiveDataDTOData());
                        shiplinedto.Project.Code = (linedto.DmsSaleNo);
                        shiplinedto.DescFlexField.PubDescSeg13 = (linedto.EarnestMoney.ToString());
                        shiplinedto.DescFlexField.PubDescSeg14 = ((linedto.ShipMoney <= 0m) ? (linedto.Money - linedto.EarnestMoney).ToString() : linedto.ShipMoney.ToString());
                        shiplinedto.DescFlexField.PubDescSeg21 = (linedto.Deposit.ToString());
                        shiplinedto.DescFlexField.PubDescSeg12 = (linedto.VIN);
                        shipdto.ShipLines.Add(shiplinedto);
                    }
                    list.Add(shipdto);
                }
            }
            return list;
        }
        /// <summary>
        /// 得到调入单dto
        /// </summary>
        /// <param name="bpObj"></param>
        /// <returns></returns>
        private System.Collections.Generic.List<UFIDA.U9.ISV.TransferInISV.IC_TransferInDTOData> GetTransferInDTOList(System.Collections.Generic.List<CarShipLineDTO> transferinlist)
        {
            System.Collections.Generic.List<UFIDA.U9.ISV.TransferInISV.IC_TransferInDTOData> list = new System.Collections.Generic.List<UFIDA.U9.ISV.TransferInISV.IC_TransferInDTOData>();
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<CarShipLineDTO>> dic = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<CarShipLineDTO>>();
            foreach (CarShipLineDTO dtoline in transferinlist)
            {
                if (!dic.ContainsKey(dtoline.SpitOrderFlag))
                {
                    dic.Add(dtoline.SpitOrderFlag, new System.Collections.Generic.List<CarShipLineDTO>());
                }
                dic[dtoline.SpitOrderFlag].Add(dtoline);
            }
            foreach (string key in dic.Keys)
            {
                List<CarShipLineDTO> listLineDTO = dic[key];

                if (listLineDTO != null
                    && listLineDTO.Count > 0
                    )
                {
                    CarShipLineDTO firstDTO = listLineDTO.GetFirst<CarShipLineDTO>();

                    UFIDA.U9.ISV.TransferInISV.IC_TransferInDTOData transindto = new UFIDA.U9.ISV.TransferInISV.IC_TransferInDTOData();
                    transindto.TransInDocType = (new CommonArchiveDataDTOData());
                    transindto.TransInDocType.Code = ("CarOutWH");
                    transindto.CreatedBy = (Context.LoginUser);
                    transindto.CreatedOn = (System.DateTime.Now);
                    transindto.ModifiedBy = (Context.LoginUser);
                    transindto.ModifiedOn = (System.DateTime.Now);
                    transindto.TransInLines = (new System.Collections.Generic.List<UFIDA.U9.ISV.TransferInISV.IC_TransInLineDTOData>());
                    foreach (CarShipLineDTO linedto in listLineDTO)
                    {
                        UFIDA.U9.ISV.TransferInISV.IC_TransInLineDTOData transinlinedto = new UFIDA.U9.ISV.TransferInISV.IC_TransInLineDTOData();
                        transinlinedto.ItemInfo = (new ItemInfoData());
                        transinlinedto.ItemInfo.ItemCode = (linedto.ErpMaterialCode);
                        transinlinedto.TransInWh = (new CommonArchiveDataDTOData());
                        transinlinedto.TransInWh.Code = (linedto.WhIn);
                        Warehouse whin = Warehouse.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), linedto.WhIn), new OqlParam[0]);
                        if (whin != null)
                        {
                            transinlinedto.StorageType = (whin.StorageType.Value);
                        }
                        else
                        {
                            transinlinedto.StorageType = (4);
                        }
                        transinlinedto.StoreUOMQty = (linedto.Number);
                        transinlinedto.CostCurrency = (new CommonArchiveDataDTOData());
                        transinlinedto.CostCurrency.Code = (linedto.Currency);
                        transinlinedto.Project = (new CommonArchiveDataDTOData());
                        transinlinedto.Project.Code = (linedto.DmsSaleNo);
                        transinlinedto.DescFlexSegments = (new DescFlexSegmentsData());
                        transinlinedto.DescFlexSegments.PubDescSeg5 = (linedto.DmsSaleNo);
                        transinlinedto.TransInSubLines = (new System.Collections.Generic.List<UFIDA.U9.ISV.TransferInISV.IC_TransInSubLineDTOData>());
                        UFIDA.U9.ISV.TransferInISV.IC_TransInSubLineDTOData sublinedto = new UFIDA.U9.ISV.TransferInISV.IC_TransInSubLineDTOData();
                        sublinedto.TransOutWh = (new CommonArchiveDataDTOData());
                        sublinedto.TransOutWh.Code = (linedto.WhOut);
                        Warehouse whout = Warehouse.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), linedto.WhOut), new OqlParam[0]);
                        if (whout != null)
                        {
                            sublinedto.StorageType = (whout.StorageType.Value);
                        }
                        else
                        {
                            sublinedto.StorageType = (4);
                        }
                        sublinedto.Project = (new CommonArchiveDataDTOData());
                        sublinedto.Project.Code = (linedto.DmsSaleNo);
                        transinlinedto.TransInSubLines.Add(sublinedto);
                        transindto.TransInLines.Add(transinlinedto);
                    }
                    list.Add(transindto);
                }
            }
            return list;
        }
    }
}