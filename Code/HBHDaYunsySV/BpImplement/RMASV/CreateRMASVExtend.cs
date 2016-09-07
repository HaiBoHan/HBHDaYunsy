namespace UFIDA.U9.Cust.HBDY.API.RMASV
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
    using UFIDA.U9.SM.RMA;
    using UFIDA.U9.Base.DTOs;
    using HBH.DoNet.DevPlatform.U9Mapping;
    using UFIDA.U9.Cust.HBDY.API.ShipSV;
    using UFIDA.U9.Cust.HBDY.API;

	/// <summary>
	/// CreateRMASV partial 
	/// </summary>	
    public partial class CreateRMASV // : HBHTransferSV
	{	
		internal BaseStrategy Select()
		{
			return new CreateRMASVImpementStrategy();	
		}		
	}
	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class CreateRMASVImpementStrategy : BaseStrategy
	{
		public CreateRMASVImpementStrategy() { }

		public override object Do(object obj)
		{						
			CreateRMASV bpObj = (CreateRMASV)obj;
			
			//get business operation context is as follows
			//IContext context = ContextManager.Context	
			
			//auto generating code end,underside is user custom code
            //and if you Implement replace this Exception Code...


            //long svID = HBHCommon.HBHCommonSVBefore(bpObj);

            List<ShipBackDTO> result2 = CreateRMA(bpObj);

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

        private List<ShipBackDTO> CreateRMA(CreateRMASV bpObj)
        {
            System.Collections.Generic.List<ShipBackDTO> result = new System.Collections.Generic.List<ShipBackDTO>();
            //object result2;
            try
            {
                if (bpObj.RMALineDTOs == null || bpObj.RMALineDTOs.Count == 0)
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
                    HBHCommon.LoggerError(backDTO.ErrorInfo);
                    result.Add(backDTO);
                }
                else
                {
                    string errormessage = this.ValidateParamNullOrEmpty(bpObj);
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
                        backDTO.ErrorInfo = errormessage + "请检查传入参数" ;
                        backDTO.Timestamp = System.DateTime.Now;
                        HBHCommon.LoggerError(backDTO.ErrorInfo);
                        result.Add(backDTO);
                    }
                    else
                    {
                        System.Collections.Generic.List<DocKeyDTOData> rmaidlist = null;
                        try
                        {
                            CreateRMASRVProxy proxy = new CreateRMASRVProxy();
                            proxy.RMADTOs = (this.GetRMADTOList(bpObj));
                            //proxy.ContextDTO = (new ContextDTOData());
                            //proxy.ContextDTO.OrgID = (Context.LoginOrg.ID);
                            //proxy.ContextDTO.OrgCode = (Context.LoginOrg.Code);
                            //proxy.ContextDTO.EntCode = (bpObj.RMALineDTOs[0].EnterpriseCode);
                            //proxy.ContextDTO.UserID = (long.Parse(Context.LoginUserID));
                            //proxy.ContextDTO.UserCode = (Context.LoginUser);
                            //proxy.ContextDTO.CultureName = (Context.LoginLanguageCode);
                            rmaidlist = proxy.Do();
                        }
                        catch (System.Exception e)
                        {
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
                        if (rmaidlist == null || rmaidlist.Count <= 0)
                        {
                            //result.Add(new ShipBackDTO
                            //{
                            //    IsSuccess = false,
                            //    ErrorInfo = "生单失败：没有生成退回处理单",
                            //    Timestamp = System.DateTime.Now
                            //});
                            //result2 = result;

                            ShipBackDTO backDTO = new ShipBackDTO();
                            backDTO.IsSuccess = false;
                            backDTO.ErrorInfo = "生单失败：没有生成退回处理单";
                            backDTO.Timestamp = System.DateTime.Now;
                            HBHCommon.LoggerError(backDTO.ErrorInfo);
                            result.Add(backDTO);
                        }
                        else
                        {
                            foreach (DocKeyDTOData rmaid in rmaidlist)
                            {
                                result.Add(new ShipBackDTO
                                {
                                    IsSuccess = true,
                                    ErrorInfo = "生单成功",
                                    Timestamp = System.DateTime.Now,
                                    ERPDocNo = rmaid.DocNO
                                });
                            }
                            //result2 = result;
                        }
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
        private string ValidateParamNullOrEmpty(CreateRMASV bpObj)
        {
            string errormessage = string.Empty;
            foreach (RMALineDTO linedto in bpObj.RMALineDTOs)
            {
                if (string.IsNullOrEmpty(linedto.DealerCode))
                {
                    errormessage += string.Format("[{0}]DMS销售出库单的[经销商代码]不可为空,", linedto.DMSShipNo);
                }
                else
                {
                    Customer customer = Customer.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), linedto.DealerCode), new OqlParam[0]);
                    if (customer == null)
                    {
                        errormessage += string.Format("[{0}]DMS销售出库单的[经销商代码({1})]在U9系统中找不到对应的客户档案,请同步,", linedto.DMSShipNo, linedto.DealerCode);
                    }
                }
                if (string.IsNullOrEmpty(linedto.ErpMaterialCode))
                {
                    errormessage += string.Format("[{0}]DMS销售出库单的参数RMALineDTOs的[ERP料号]不可为空,", linedto.DMSShipNo);
                }
                else
                {
                    ItemMaster item = ItemMaster.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), linedto.ErpMaterialCode), new OqlParam[0]);
                    if (item == null)
                    {
                        errormessage += string.Format("[{0}]DMS销售出库单的参数RMALineDTOs的[ERP料号][{1}]在U9系统中找不到对应的料品档案,请同步,", linedto.DMSShipNo, linedto.ErpMaterialCode);
                    }
                }
                if (string.IsNullOrEmpty(linedto.WHIn))
                {
                    errormessage += string.Format("[{0}]DMS销售出库单的参数RMALineDTOs的[存储地点]不可为空,", linedto.DMSShipNo);
                }
                else
                {
                    Warehouse whout = Warehouse.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), linedto.WHIn), new OqlParam[0]);
                    if (whout == null)
                    {
                        errormessage += string.Format("[{0}]DMS销售出库单的参数RMALineDTOs的[存储地点({1})]在U9系统中找不到对应的存储地点档案,请同步,", linedto.DMSShipNo, linedto.WHIn);
                    }
                }
                if (linedto.Number <= 0m)
                {
                    errormessage += string.Format("[{0}]DMS销售出库单的参数RMALineDTOs的[退回数量]必须大于0,", linedto.DMSShipNo);
                }

                if (linedto.SpitOrderFlag.IsNull())
                {
                    linedto.SpitOrderFlag = HBHCommon.DefaultSplitFlag;
                }
            }
            return errormessage;
        }

        // 得到调入单dto
        /// <summary>
        /// 得到调入单dto
        /// </summary>
        /// <param name="bpObj"></param>
        /// <returns></returns>
        private System.Collections.Generic.List<UFIDA.U9.ISV.SM.RMADTOData> GetRMADTOList(CreateRMASV bpObj)
        {
            System.Collections.Generic.List<UFIDA.U9.ISV.SM.RMADTOData> list = new System.Collections.Generic.List<UFIDA.U9.ISV.SM.RMADTOData>();
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<RMALineDTO>> dic = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<RMALineDTO>>();
            foreach (RMALineDTO dtoline in bpObj.RMALineDTOs)
            {
                if (!dic.ContainsKey(dtoline.SpitOrderFlag))
                {
                    dic.Add(dtoline.SpitOrderFlag, new System.Collections.Generic.List<RMALineDTO>());
                }
                dic[dtoline.SpitOrderFlag].Add(dtoline);
            }
            foreach (string key in dic.Keys)
            {
                List<RMALineDTO> listLineDTO = dic[key];

                if (listLineDTO != null
                    && listLineDTO.Count > 0
                    )
                {
                    RMALineDTO firstDTO = listLineDTO.GetFirst<RMALineDTO>();

                    UFIDA.U9.ISV.SM.RMADTOData rmadto = new UFIDA.U9.ISV.SM.RMADTOData();
                    RMALineDTO dto = firstDTO;
                    rmadto.DocumentTypeDTO = (new IDCodeNameDTOData());
                    rmadto.DocumentTypeDTO.Code = ("H0001");
                    rmadto.KeepAccountsPeriodDTO = (new IDCodeNameDTOData());
                    RMADocType doctype = RMADocType.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), "H0001"), new OqlParam[0]);
                    if (doctype != null)
                    {
                        if (doctype.AccountAccordingKey != null)
                        {
                            rmadto.ConfirmAccordingDTO = (new IDCodeNameDTOData());
                            rmadto.ConfirmAccordingDTO.Code = (doctype.AccountAccording.Code);
                        }
                        if (doctype.BillingMode.Value >= 0)
                        {
                            rmadto.BillingMode = (doctype.BillingMode.Value);
                        }
                        else
                        {
                            rmadto.BillingMode = (0);
                        }
                        if (doctype.InvoiceAccordingKey != null)
                        {
                            rmadto.InvoiceAccordingDTO = (new IDCodeNameDTOData());
                            rmadto.InvoiceAccordingDTO.Code = (doctype.InvoiceAccording.Code);
                        }
                    }
                    else
                    {
                        rmadto.BillingMode = (0);
                    }
                    rmadto.AccrueTermDTO = (new IDCodeNameDTOData());
                    rmadto.AccrueTermDTO.Code = ("01");
                    rmadto.CustomerDTO = (new IDCodeNameDTOData());
                    rmadto.CustomerDTO.Code = (dto.DealerCode);
                    rmadto.ACDTO = (new IDCodeNameDTOData());
                    rmadto.ACDTO.Code = (string.IsNullOrEmpty(dto.Currency) ? "C001" : dto.Currency);
                    rmadto.TCDTO = (new IDCodeNameDTOData());
                    rmadto.TCDTO.Code = (string.IsNullOrEmpty(dto.Currency) ? "C001" : dto.Currency);
                    rmadto.DescFlexField = (new DescFlexSegmentsData());
                    rmadto.DescFlexField.PubDescSeg5 = (dto.DmsSaleNo);
                    rmadto.DescFlexField.PrivateDescSeg1 = (dto.DMSShipNo);
                    rmadto.DescFlexField.PubDescSeg12 = (dto.VIN);
                    rmadto.DescFlexField.PubDescSeg13 = (dto.EarnestMoney.ToString());
                    rmadto.DescFlexField.PubDescSeg14 = ((dto.ShipMoney <= 0m) ? (dto.Money - dto.EarnestMoney).ToString() : dto.ShipMoney.ToString());
                    rmadto.DescFlexField.PubDescSeg21 = (dto.Deposit.ToString());
                    System.DateTime arg_354_0 = dto.ShipDate;
                    if (dto.ShipDate != System.DateTime.MinValue && dto.ShipDate > System.DateTime.Now)
                    {
                        rmadto.BusinessDate = (dto.ShipDate);
                    }
                    else
                    {
                        rmadto.BusinessDate = (System.DateTime.Now);
                    }
                    rmadto.RMALines = (new System.Collections.Generic.List<ISV.SM.RMALineDTOData>());
                    foreach (RMALineDTO linedto in listLineDTO)
                    {
                        ISV.SM.RMALineDTOData rmalinedto = new ISV.SM.RMALineDTOData();
                        rmalinedto.ItemInfoDTO = (new IDCodeNameDTOData());
                        rmalinedto.ItemInfoDTO.Code = (linedto.ErpMaterialCode);
                        rmalinedto.ApplyQtyTU1 = (linedto.Number);
                        rmalinedto.RtnQtyTU1 = (linedto.Number);
                        rmalinedto.RtnQtyPU = (linedto.Number);
                        rmalinedto.ApplyMoneyTC = (linedto.Money);
                        rmalinedto.ApplyNetMoneyTC = (linedto.Money);
                        rmalinedto.ProjectDTO = (new IDCodeNameDTOData());
                        rmalinedto.ProjectDTO.Code = (linedto.DmsSaleNo);
                        rmalinedto.WarehouseDTO = (new IDCodeNameDTOData());
                        rmalinedto.WarehouseDTO.Code = (linedto.WHIn);
                        rmadto.RMALines.Add(rmalinedto);
                    }
                    list.Add(rmadto);
                }
            }
            return list;
        }
    }
}
