namespace UFIDA.U9.Cust.HBDY.API
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
    using HBH.DoNet.DevPlatform.U9Mapping;

	/// <summary>
	/// CreateShipSV partial 
	/// </summary>	
    public partial class CreateShipSV // : HBHTransferSV
	{	
		internal BaseStrategy Select()
		{
			return new CreateShipSVImpementStrategy();	
		}		
	}
	

	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class CreateShipSVImpementStrategy : BaseStrategy
	{
		public CreateShipSVImpementStrategy() { }

		public override object Do(object obj)
		{						
			CreateShipSV bpObj = (CreateShipSV)obj;
			
			//get business operation context is as follows
			//IContext context = ContextManager.Context	
			
			//auto generating code end,underside is user custom code
            //and if you Implement replace this Exception Code...


            //long svID = HBHCommon.HBHCommonSVBefore(bpObj);

            List<ShipBackDTO> result2 = CreateShip(bpObj);

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

        private List<ShipBackDTO> CreateShip(CreateShipSV bpObj)
        {
            System.Collections.Generic.List<ShipBackDTO> result = new System.Collections.Generic.List<ShipBackDTO>();
            //object result2;
            try
            {
                if (bpObj.ShipLineDTOs == null || bpObj.ShipLineDTOs.Count == 0)
                {
                    //result.Add(new ShipBackDTO
                    //{
                    //    IsSuccess = false,
                    //    ErrorInfo = "传入参数不可为空",
                    //    Timestamp = System.DateTime.Now
                    //});
                    //result2 = result;

                    ShipBackDTO backDTO = new ShipBackDTO();
                    backDTO.IsSuccess = false;
                    backDTO.ErrorInfo = "传入参数不可为空";
                    backDTO.Timestamp = System.DateTime.Now;
                    HBHCommon.LoggerError(backDTO.ErrorInfo );
                    result.Add(backDTO);
                }
                else
                {
                    System.Collections.Generic.List<ShipLineDTO> shiplinelist = new System.Collections.Generic.List<ShipLineDTO>();
                    System.Collections.Generic.List<ShipLineDTO> MiscShipmentLinelist = new System.Collections.Generic.List<ShipLineDTO>();
                    string errormessage = this.ValidateParamNullOrEmpty(bpObj, ref shiplinelist, ref MiscShipmentLinelist);
                    if (!string.IsNullOrEmpty(errormessage))
                    {
                        //result.Add(new ShipBackDTO
                        //{
                        //    IsSuccess = false,
                        //    ErrorInfo = errormessage + "请检查传入参数",
                        //    Timestamp = System.DateTime.Now
                        //});
                        //result2 = result;

                        ShipBackDTO backDTO = new ShipBackDTO();
                        backDTO.IsSuccess = false;
                        backDTO.ErrorInfo = errormessage + "请检查传入参数";
                        backDTO.Timestamp = System.DateTime.Now;
                        HBHCommon.LoggerError(backDTO.ErrorInfo);
                        result.Add(backDTO);
                    }
                    else
                    {
                        System.Collections.Generic.List<DocKeyDTOData> shipidlist = new System.Collections.Generic.List<DocKeyDTOData>();
                        System.Collections.Generic.List<CommonArchiveDataDTOData> miscshiplist = new System.Collections.Generic.List<CommonArchiveDataDTOData>();
                        //using (UBFTransactionScope trans = new UBFTransactionScope(TransactionOption.Required))
                        {
                            try
                            {
                                if (shiplinelist != null && shiplinelist.Count > 0)
                                {
                                    CreateShipSVProxy proxy = new CreateShipSVProxy();
                                    proxy.ShipDTOs = (this.GetShipDTOList(shiplinelist));
                                    shipidlist = proxy.Do();
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
                                    AuditShipSVProxy approveproxy = new AuditShipSVProxy();
                                    approveproxy.ShipKeys = (shipidlist);
                                    approveproxy.Do();
                                }
                                if (MiscShipmentLinelist != null && MiscShipmentLinelist.Count > 0)
                                {
                                    CommonCreateMiscShipProxy proxy2 = new CommonCreateMiscShipProxy();
                                    proxy2.MiscShipmentDTOList = (this.GetMiscShipmentDTOList(MiscShipmentLinelist));
                                    miscshiplist = proxy2.Do();
                                    if (miscshiplist == null || miscshiplist.Count <= 0)
                                    {
                                        //result.Add(new ShipBackDTO
                                        //{
                                        //    IsSuccess = false,
                                        //    ErrorInfo = "生单失败：没有生成杂发单",
                                        //    Timestamp = System.DateTime.Now
                                        //});
                                        //result2 = result;
                                        //return result2;

                                        ShipBackDTO backDTO = new ShipBackDTO();
                                        backDTO.IsSuccess = false;
                                        backDTO.ErrorInfo = "生单失败：没有生成杂发单";
                                        backDTO.Timestamp = System.DateTime.Now;
                                        HBHCommon.LoggerError(backDTO.ErrorInfo);
                                        result.Add(backDTO);
                                        return result;
                                    }
                                    CommonApproveMiscShipSVProxy approveproxy2 = new CommonApproveMiscShipSVProxy();
                                    approveproxy2.MiscShipmentKeyList = (miscshiplist);
                                    approveproxy2.Do();
                                }
                                //trans.Commit();
                            }
                            catch (System.Exception e)
                            {
                                //trans.Rollback();
                                //result.Add(new ShipBackDTO
                                //{
                                //    IsSuccess = false,
                                //    ErrorInfo = "生单失败：" + e.Message,
                                //    Timestamp = System.DateTime.Now
                                //});
                                //result2 = result;
                                //return result2;

                                ShipBackDTO backDTO = new ShipBackDTO();
                                backDTO.IsSuccess = false;
                                backDTO.ErrorInfo = "生单失败：" + e.Message;
                                backDTO.Timestamp = System.DateTime.Now;
                                HBHCommon.LoggerError(backDTO.ErrorInfo + "/r/n" + e.StackTrace);
                                result.Add(backDTO);
                                return result;
                            }
                        }
                        if (shipidlist != null && shipidlist.Count > 0)
                        {
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
                                        DMSDocNo = ship.DescFlexField.PubDescSeg5
                                    });
                                }
                            }
                        }
                        if (miscshiplist != null && miscshiplist.Count > 0)
                        {
                            foreach (CommonArchiveDataDTOData miscshipid in miscshiplist)
                            {
                                MiscShipment miscship = MiscShipment.Finder.FindByID(miscshipid.ID);
                                if (miscship != null)
                                {
                                    result.Add(new ShipBackDTO
                                    {
                                        IsSuccess = true,
                                        ErrorInfo = "生单杂发单成功",
                                        Timestamp = System.DateTime.Now,
                                        ERPDocNo = miscship.DocNo,
                                        DMSDocNo = miscship.DescFlexField.PubDescSeg5
                                    });
                                }
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

        // 传入参数非空校验
        /// <summary>
        /// 传入参数非空校验
        /// </summary>
        /// <param name="bpObj"></param>
        private string ValidateParamNullOrEmpty(CreateShipSV bpObj, ref System.Collections.Generic.List<ShipLineDTO> shiplinelist, ref System.Collections.Generic.List<ShipLineDTO> MiscShipmentLinelist)
        {
            string errormessage = string.Empty;
            foreach (ShipLineDTO linedto in bpObj.ShipLineDTOs)
            {
                if (string.IsNullOrEmpty(linedto.OrderType))
                {
                    errormessage += string.Format("[{0}]DMS销售订单的[订单类型]不可为空,", linedto.DmsSaleNo);
                }
                if (linedto.OrderType == "3")
                {
                    MiscShipmentLinelist.Add(linedto);
                }
                else
                {
                    if (string.IsNullOrEmpty(linedto.DealerCode))
                    {
                        errormessage += string.Format("[{0}]DMS销售订单的[经销商代码]不可为空,", linedto.DmsSaleNo);
                    }
                    else
                    {
                        Customer customer = Customer.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), linedto.DealerCode), new OqlParam[0]);
                        if (customer == null)
                        {
                            errormessage += string.Format("[{0}]DMS销售订单的[经销商代码({1})]在U9系统中找不到对应的客户档案,请同步,", linedto.DmsSaleNo, linedto.DealerCode);
                        }
                    }
                    shiplinelist.Add(linedto);
                }
                if (string.IsNullOrEmpty(linedto.ErpMaterialCode))
                {
                    errormessage += string.Format("[{0}]DMS销售出库单的参数ShipLines的[ERP料号]不可为空,", linedto.DMSShipNo);
                }
                else
                {
                    ItemMaster item = ItemMaster.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), linedto.ErpMaterialCode), new OqlParam[0]);
                    if (item == null)
                    {
                        errormessage += string.Format("[{0}]DMS销售出库单的参数ShipLines的[ERP料号][{1}]在U9系统中找不到对应的料品档案,请同步,", linedto.DMSShipNo, linedto.ErpMaterialCode);
                    }
                }
                if (string.IsNullOrEmpty(linedto.Lot))
                {
                    errormessage += string.Format("[{0}]DMS销售出库单的[批号]不可为空,", linedto.DMSShipNo);
                }
                else
                {
                    LotMaster Lot = LotMaster.Finder.Find(string.Format("LotCode='{0}'", linedto.Lot), new OqlParam[0]);
                    if (Lot == null)
                    {
                        errormessage += string.Format("[{0}]DMS销售出库单的批号[{1}]在U9系统中找不到对应的存储地点档案,请同步,", linedto.DMSShipNo, linedto.Lot);
                    }
                }
                if (string.IsNullOrEmpty(linedto.MaterialCode))
                {
                    errormessage += string.Format("[{0}]DMS销售出库单的[供应商编码]不可为空,", linedto.DMSShipNo);
                }
                else
                {
                    Supplier supplier = Supplier.Finder.Find(string.Format("Code='{0}' and Org={1}", linedto.MaterialCode, Context.LoginOrg.ID.ToString()), new OqlParam[0]);
                    if (supplier == null)
                    {
                        errormessage += string.Format("[{0}]DMS销售出库单的供应商[{1}]在U9系统中找不到对应的存储地点档案,请同步,", linedto.DMSShipNo, linedto.MaterialCode);
                    }
                }
                if (linedto.Number <= 0m)
                {
                    errormessage += string.Format("[{0}]DMS销售出库单的参数ShipLines的[数量]必须大于0,", linedto.DMSShipNo);
                }

                if (linedto.SpitOrderFlag.IsNull())
                {
                    linedto.SpitOrderFlag = HBHCommon.DefaultSplitFlag;
                }
            }
            return errormessage;
        }

        // 得到Ship单dto
        /// <summary>
        /// 得到Ship单dto
        /// </summary>
        /// <param name="bpObj"></param>
        /// <returns></returns>
        private System.Collections.Generic.List<UFIDA.U9.ISV.SM.ShipDTOForIndustryChainData> GetShipDTOList(System.Collections.Generic.List<ShipLineDTO> shiplinelist)
        {
            System.Collections.Generic.List<UFIDA.U9.ISV.SM.ShipDTOForIndustryChainData> list = new System.Collections.Generic.List<UFIDA.U9.ISV.SM.ShipDTOForIndustryChainData>();
            string opeatorstr = "DMSTESTUSER";
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ShipLineDTO>> dic = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ShipLineDTO>>();
            foreach (ShipLineDTO dtoline in shiplinelist)
            {
                if (!dic.ContainsKey(dtoline.SpitOrderFlag))
                {
                    dic.Add(dtoline.SpitOrderFlag, new System.Collections.Generic.List<ShipLineDTO>());
                }
                dic[dtoline.SpitOrderFlag].Add(dtoline);
            }
            foreach (string key in dic.Keys)
            {
                List<ShipLineDTO> listLineDTO = dic[key];

                if (listLineDTO != null
                    && listLineDTO.Count > 0
                    )
                {
                    ShipLineDTO firstDTO = listLineDTO.GetFirst<ShipLineDTO>();

                    UFIDA.U9.ISV.SM.ShipDTOForIndustryChainData shipdto = new UFIDA.U9.ISV.SM.ShipDTOForIndustryChainData();
                    string doctypecode = string.Empty;
                    shipdto.DocumentType = (new CommonArchiveDataDTOData());
                    if (firstDTO.OrderType == "0")
                    {
                        doctypecode = "XM10";
                    }
                    else if (firstDTO.OrderType == "1")
                    {
                        doctypecode = "XM5";
                    }
                    else if (firstDTO.OrderType == "2")
                    {
                        doctypecode = "XM12";
                    }
                    shipdto.DocumentType.Code = (doctypecode);
                    shipdto.CreatedBy = (Context.LoginUser);
                    shipdto.ModifiedBy = (Context.LoginUser);
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
                    //System.DateTime arg_5E1_0 = firstDTO.ShipDate;
                    if (firstDTO.ShipDate != System.DateTime.MinValue && firstDTO.ShipDate > System.DateTime.Now)
                    {
                        shipdto.BusinessDate = (firstDTO.ShipDate);
                    }
                    else
                    {
                        shipdto.BusinessDate = (System.DateTime.Now);
                    }
                    shipdto.ShipLines = (new System.Collections.Generic.List<UFIDA.U9.ISV.SM.ShipLineDTOForIndustryChainData>());
                    foreach (ShipLineDTO linedto in listLineDTO)
                    {
                        UFIDA.U9.ISV.SM.ShipLineDTOForIndustryChainData shiplinedto = new UFIDA.U9.ISV.SM.ShipLineDTOForIndustryChainData();
                        shiplinedto.ItemInfo = (new ItemInfoData());
                        shiplinedto.ItemInfo.ItemCode = (linedto.ErpMaterialCode);
                        shiplinedto.ShipQtyTUAmount = (linedto.Number);
                        shiplinedto.TotalMoneyTC = (linedto.Money);
                        shiplinedto.TotalNetMoneyTC = (linedto.Money);
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
                        shiplinedto.WH.Code = ("SHBJ");
                        Warehouse whout = Warehouse.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), "SHBJ"), new OqlParam[0]);
                        if (whout != null && whout.DepositType == DepositTypeEnum.VMI)
                        {
                            shiplinedto.VMI = (true);
                        }
                        shiplinedto.Supplier = (new CommonArchiveDataDTOData());
                        shiplinedto.Supplier.Code = (linedto.MaterialCode);
                        LotMaster Lot = LotMaster.Finder.Find(string.Format("LotCode='{0}'", linedto.Lot), new OqlParam[0]);
                        shiplinedto.LotInfo = new LotInfoData();
                        shiplinedto.LotInfo.LotMaster = (new BizEntityKeyData());
                        shiplinedto.LotInfo.LotMaster.EntityID = (Lot.ID);
                        shiplinedto.DescFlexField = (new DescFlexSegmentsData());
                        shiplinedto.DescFlexField.PubDescSeg5 = (linedto.DmsSaleNo);
                        shiplinedto.DescFlexField.PubDescSeg13 = (linedto.EarnestMoney.ToString("G0"));
                        shiplinedto.DescFlexField.PubDescSeg14 = ((linedto.ShipMoney <= 0m) ? (linedto.Money - linedto.EarnestMoney).ToString("G0") : linedto.ShipMoney.ToString("G0"));
                        shiplinedto.DescFlexField.PubDescSeg21 = (linedto.Deposit.ToString("G0"));
                        shiplinedto.DescFlexField.PubDescSeg12 = (linedto.VIN);
                        shipdto.ShipLines.Add(shiplinedto);
                    }
                    list.Add(shipdto);
                }
            }
            return list;
        }
        /// <summary>
        /// 校验库存可用量
        /// </summary>
        /// <param name="bpObj"></param>
        /// <returns></returns>
        private static string ValidateWhqoh(CreateShipSV bpObj)
        {
            System.Collections.Generic.Dictionary<string, decimal> HaveWhqohqty = new System.Collections.Generic.Dictionary<string, decimal>();
            System.Text.StringBuilder errors = new System.Text.StringBuilder();
            Warehouse whout = Warehouse.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), "SHBJ"), new OqlParam[0]);
            string result;
            if (whout == null)
            {
                errors.Append("售后配件库U9系统中不存在，请同步");
                result = errors.ToString();
            }
            else
            {
                QueryBinAvailableQohPubNoramlProxy proxy = new QueryBinAvailableQohPubNoramlProxy();
                System.Collections.Generic.List<BinAvailableDTOForNormalData> list = new System.Collections.Generic.List<BinAvailableDTOForNormalData>();
                foreach (ShipLineDTO dto in bpObj.ShipLineDTOs)
                {
                    BinAvailableDTOForNormalData dto2 = new BinAvailableDTOForNormalData();
                    BinAvailableDTODetailForNormalData d = new BinAvailableDTODetailForNormalData();
                    System.Collections.Generic.List<int> grouplist = new System.Collections.Generic.List<int>();
                    d.ItemCode = (dto.ErpMaterialCode);
                    grouplist.Add(QohFieldEnum.ItemMaster.Value);
                    grouplist.Add(QohFieldEnum.Wh.Value);
                    d.Wh = (whout.ID);
                    if (!string.IsNullOrEmpty(dto.MaterialCode))
                    {
                        Supplier supplier = Supplier.Finder.Find(string.Format("Code='{0}' and Org={1}", dto.MaterialCode, Context.LoginOrg.ID.ToString()), new OqlParam[0]);
                        grouplist.Add(QohFieldEnum.Supplier.Value);
                        d.Supplier = (supplier.ID);
                    }
                    if (!string.IsNullOrEmpty(dto.Lot))
                    {
                        LotMaster Lot = LotMaster.Finder.Find(string.Format("LotCode='{0}'", dto.Lot), new OqlParam[0]);
                        grouplist.Add(QohFieldEnum.Lot.Value);
                        d.Lot = (Lot.Key);
                    }
                    d.Org = (Context.LoginOrg.ID);
                    dto2.GroupList = (grouplist);
                    dto2.BinAvailableDTODetail = (d);
                    list.Add(dto2);
                }
                proxy.BinAvailableDTOList = (list);
                System.Collections.Generic.List<BinAvailableRtnDTOWithNoData> whqohlist = proxy.Do();
                if (whqohlist.Count <= 0)
                {
                    errors.Append("料品的库存可用量小于等于0");
                    result = errors.ToString();
                }
                else
                {
                    foreach (BinAvailableRtnDTOWithNoData whqoh in whqohlist)
                    {
                        if (whqoh.BinAvailableRthDTOForNormalList != null && whqoh.BinAvailableRthDTOForNormalList.Count > 0)
                        {
                            string whcode = string.Empty;
                            string lotcode = string.Empty;
                            string itemcode = string.Empty;
                            string customercode = string.Empty;
                            string suppliercode = string.Empty;
                            foreach (BinAvailableRthDTOForNormalData dto3 in whqoh.BinAvailableRthDTOForNormalList)
                            {
                                if (!(dto3.QohQty <= 0m))
                                {
                                    whcode = ((dto3.DescribeOnWhqoh.Wh <= 0L) ? string.Empty : ((Warehouse)dto3.DescribeOnWhqoh.Wh_SKey.GetEntity()).Code);
                                    lotcode = ((dto3.DescribeOnWhqoh.Lot.ID <= 0L) ? string.Empty : ((LotMaster)dto3.DescribeOnWhqoh.Lot.GetEntity()).LotCode);
                                    itemcode = ((dto3.DescribeOnWhqoh.ItemMaster <= 0L) ? string.Empty : ((ItemMaster)dto3.DescribeOnWhqoh.ItemMaster_SKey.GetEntity()).Code);
                                    suppliercode = ((dto3.DescribeOnWhqoh.Supplier <= 0L) ? string.Empty : ((Supplier)dto3.DescribeOnWhqoh.Supplier_SKey.GetEntity()).Code);
                                    if (!HaveWhqohqty.ContainsKey(itemcode + whcode + lotcode + suppliercode))
                                    {
                                        HaveWhqohqty.Add(itemcode + whcode + lotcode + suppliercode, dto3.QohQty);
                                    }
                                    else
                                    {
                                        System.Collections.Generic.Dictionary<string, decimal> dictionary;
                                        string key;
                                        (dictionary = HaveWhqohqty)[key = itemcode + whcode + lotcode + suppliercode] = dictionary[key] + dto3.QohQty;
                                    }
                                }
                            }
                        }
                    }
                    foreach (ShipLineDTO dto in bpObj.ShipLineDTOs)
                    {
                        if (!HaveWhqohqty.ContainsKey(dto.ErpMaterialCode + "SHBJ" + dto.Lot + dto.MaterialCode) || !(HaveWhqohqty[dto.ErpMaterialCode + "SHBJ" + dto.Lot + dto.MaterialCode] >= dto.Number))
                        {
                            errors.Append("料品【" + dto.ErpMaterialCode + "】");
                            errors.Append("在存储地点【售后配件仓】");
                            if (HaveWhqohqty.ContainsKey(dto.ErpMaterialCode + "SHBJ" + dto.Lot + dto.MaterialCode) && HaveWhqohqty[dto.ErpMaterialCode + "SHBJ" + dto.Lot + dto.MaterialCode] < dto.Number)
                            {
                                errors.Append(string.Concat(new string[]
								{
									"的出库数量【",
									dto.Number.ToString("G0"),
									"】不可大于库存可用量【",
									HaveWhqohqty[dto.ErpMaterialCode + "SHBJ" + dto.Lot + dto.MaterialCode].ToString(),
									"】"
								}));
                            }
                            else
                            {
                                errors.Append("的库存可用量等于0");
                            }
                        }
                    }
                    result = errors.ToString();
                }
            }
            return result;
        }
        /// <summary>
        /// 得到杂发单dto
        /// </summary>
        /// <param name="bpObj"></param>
        /// <returns></returns>
        private System.Collections.Generic.List<IC_MiscShipmentDTOData> GetMiscShipmentDTOList(System.Collections.Generic.List<ShipLineDTO> MiscShipmentLinelist)
        {
            System.Collections.Generic.List<IC_MiscShipmentDTOData> list = new System.Collections.Generic.List<IC_MiscShipmentDTOData>();
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ShipLineDTO>> dic = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ShipLineDTO>>();
            foreach (ShipLineDTO dtoline in MiscShipmentLinelist)
            {
                if (!dic.ContainsKey(dtoline.SpitOrderFlag))
                {
                    dic.Add(dtoline.SpitOrderFlag, new System.Collections.Generic.List<ShipLineDTO>());
                }
                dic[dtoline.SpitOrderFlag].Add(dtoline);
            }
            foreach (string key in dic.Keys)
            {
                List<ShipLineDTO> listLineDTO = dic[key];

                if (listLineDTO != null
                    && listLineDTO.Count > 0
                    )
                {
                    ShipLineDTO firstDTO = listLineDTO.GetFirst<ShipLineDTO>();

                    IC_MiscShipmentDTOData misshipdto = new IC_MiscShipmentDTOData();
                    misshipdto.MiscShipDocType = (new CommonArchiveDataDTOData());
                    misshipdto.MiscShipDocType.Code = ("ZF03");
                    misshipdto.DescFlexField = (new DescFlexSegmentsData());
                    misshipdto.DescFlexField.PubDescSeg5 = (firstDTO.DmsSaleNo);
                    misshipdto.DescFlexField.PrivateDescSeg3 = (firstDTO.DMSShipNo);
                    misshipdto.BusinessDate = (System.DateTime.Now);
                    misshipdto.MiscShipLs = (new System.Collections.Generic.List<IC_MiscShipmentLDTOData>());
                    foreach (ShipLineDTO linedto in listLineDTO)
                    {
                        IC_MiscShipmentLDTOData misshiplinedto = new IC_MiscShipmentLDTOData();
                        misshiplinedto.ItemInfo = (new ItemInfoData());
                        misshiplinedto.ItemInfo.ItemCode = (linedto.ErpMaterialCode);
                        misshiplinedto.StoreUOMQty = (linedto.Number);
                        misshiplinedto.BenefitDept = (new CommonArchiveDataDTOData());
                        misshiplinedto.BenefitDept.Code = ("101404");
                        misshiplinedto.Wh = (new CommonArchiveDataDTOData());
                        misshiplinedto.Wh.Code = ("SHBJ");
                        Warehouse whout = Warehouse.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), "SHBJ"), new OqlParam[0]);
                        if (whout != null && whout.DepositType == DepositTypeEnum.VMI)
                        {
                            misshiplinedto.IsVMI = (true);
                        }
                        misshiplinedto.SupplierInfo = (new SupplierMISCInfoData());
                        misshiplinedto.SupplierInfo.Code = (linedto.MaterialCode);
                        misshiplinedto.StoreType = (4);
                        misshiplinedto.LotInfo = (new LotInfoData());
                        misshiplinedto.LotInfo.LotCode = (linedto.Lot);
                        misshiplinedto.DescFlexSegments = (new DescFlexSegmentsData());
                        misshiplinedto.DescFlexSegments.PubDescSeg5 = (linedto.DmsSaleNo);
                        misshiplinedto.DescFlexSegments.PubDescSeg12 = (linedto.VIN);
                        misshipdto.MiscShipLs.Add(misshiplinedto);
                    }
                    list.Add(misshipdto);
                }
            }
            return list;
        }
    }
}