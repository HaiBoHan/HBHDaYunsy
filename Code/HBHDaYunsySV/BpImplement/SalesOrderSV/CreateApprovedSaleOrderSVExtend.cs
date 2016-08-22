namespace UFIDA.U9.Cust.HBDY.API
{
	using System;
	using System.Collections.Generic;
	using System.Text; 
	using UFSoft.UBF.AopFrame;	
	using UFSoft.UBF.Util.Context;
    using UFIDA.U9.ISV.SM;
    using UFIDA.U9.CBO.Pub.Controller;
    using UFIDA.U9.SM.SO.Proxy;
    using UFIDA.U9.SM.SO;
    using UFIDA.U9.CBO.SCM.Customer;
    using UFSoft.UBF.PL;
    using UFIDA.U9.Base;
    using UFIDA.U9.CBO.SCM.Item;
    using UFIDA.U9.Base.FlexField.DescFlexField;
    using System.Collections;
    using HBH.DoNet.DevPlatform.EntityMapping;
    using UFSoft.UBF.Transactions;
    using UFSoft.UBF.Business;
    using UFIDA.U9.CBO.SCM.ProjectTask;
    using UFIDA.U9.Base.PropertyTypes;
    using UFIDA.U9.CBO.Pub.Controller.Proxy;
    using UFIDA.U9.Base.Organization;
    using UFIDA.U9.ISV.SM.Proxy;
    using HBH.DoNet.DevPlatform.U9Mapping;

	/// <summary>
	/// CreateApprovedSaleOrderSV partial 
	/// </summary>	
	public partial class CreateApprovedSaleOrderSV 
	{	
		internal BaseStrategy Select()
		{
			return new CreateApprovedSaleOrderSVImpementStrategy();	
		}		
	}
	
    //#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class CreateApprovedSaleOrderSVImpementStrategy : BaseStrategy
	{
        public const bool isLog = true;

		public CreateApprovedSaleOrderSVImpementStrategy() { }

		public override object Do(object obj)
        {
            CreateApprovedSaleOrderSV bpObj = (CreateApprovedSaleOrderSV)obj;
			
			//get business operation context is as follows
			//IContext context = ContextManager.Context	
			
			//auto generating code end,underside is user custom code
            //and if you Implement replace this Exception Code...


            //long svID = HBHCommon.HBHCommonSVBefore(bpObj);

            List<SoBackDTO> result2 = CreateSO(bpObj);

            //if (result2 != null
            //    && result2.Count > 0
            //    )
            //{
            //    SoBackDTO first = PubClass.GetFirst<SoBackDTO>(result2); 

            //    if (first != null)
            //    {
            //        HBHCommon.HBHCommonSVAfter(svID, result2, first.IsSuccess, first.ErrorInfo, first.ERPDocNo);
            //    }
            //}

            return result2;
        }

        // 创建SO
        /// <summary>
        /// 创建SO
        /// </summary>
        /// <param name="bpObj"></param>
        /// <returns></returns>
        private List<SoBackDTO> CreateSO(CreateApprovedSaleOrderSV bpObj)
        {

            System.Collections.Generic.List<SoBackDTO> results = new System.Collections.Generic.List<SoBackDTO>();
            SoBackDTO result = new SoBackDTO();
            //object result2;
            try
            {
                if (bpObj.SoLineDto == null || bpObj.SoLineDto.Count == 0)
                {
                    result.IsSuccess = false;
                    result.ErrorInfo = "传入参数不可为空";
                    result.Timestamp = System.DateTime.Now.ToString();
                    results.Add(result);
                    //result2 = results;
                }
                else
                {
                    string usercode = bpObj.SoLineDto[0].UserCode;
                    string enterprise = bpObj.SoLineDto[0].EnterpriseCode;
                    string errormessage = this.ValidateParamNullOrEmpty(bpObj);
                    if (!string.IsNullOrEmpty(errormessage))
                    {
                        result.IsSuccess = false;
                        result.ErrorInfo = errormessage;
                        result.Timestamp = System.DateTime.Now.ToString();
                        results.Add(result);
                        //result2 = results;
                    }
                    else
                    {
                        System.Collections.Generic.List<SOStatusDTOData> statusDTOs = null;
                        using (UBFTransactionScope trans = new UBFTransactionScope(TransactionOption.Required))
                        {
                            using (ISession session = Session.Open())
                            {
                                foreach (SoLineDTO line in bpObj.SoLineDto)
                                {
                                    if (!string.IsNullOrEmpty(line.DmsSaleNo))
                                    {
                                        Project project = Project.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID, line.DmsSaleNo), new OqlParam[0]);
                                        if (project == null)
                                        {
                                            Project p = Project.Create();
                                            p.Org = (Context.LoginOrg);
                                            p.StartDate = (System.DateTime.Now);
                                            p.EndDate = (System.DateTime.Now);
                                            p.Code = (line.DmsSaleNo);
                                            p.Name = (line.DmsSaleNo);
                                            p.Effective = (new Effective());
                                            p.Effective.IsEffective = (true);
                                            p.Effective.EffectiveDate = (System.Convert.ToDateTime("2000.01.01 00:00:00"));
                                            p.Effective.DisableDate = (System.Convert.ToDateTime("9999.12.31"));
                                        }
                                    }
                                }
                                session.Commit();
                            }
                            Project project2 = Project.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID, bpObj.SoLineDto[0].DmsSaleNo), new OqlParam[0]);
                            if (project2 != null)
                            {
                                try
                                {
                                    OnlineSendObjsProxy onlineSendObjsProxy = new OnlineSendObjsProxy();
                                    onlineSendObjsProxy.FullName = ("UFIDA.U9.CBO.SCM.ProjectTask.Project");
                                    onlineSendObjsProxy.IDs = (new System.Collections.Generic.List<long>());
                                    onlineSendObjsProxy.IDs.Add(project2.ID);
                                    onlineSendObjsProxy.FromOrg = (Context.LoginOrg.ID);
                                    onlineSendObjsProxy.ToOrgList = (new System.Collections.Generic.List<long>());
                                    //Organization org = Organization.Finder.Find("Code='10'", new OqlParam[0]);
                                    //if (org != null)
                                    //{
                                    //    onlineSendObjsProxy.ToOrgList.Add(org.ID);
                                    //}
                                    //Organization org2 = Organization.Finder.Find("Code='30'", new OqlParam[0]);
                                    //if (org2 != null)
                                    //{
                                    //    onlineSendObjsProxy.ToOrgList.Add(org2.ID);
                                    //}
                                    if (HBHCommon.ProjectSendOrgCode.Count > 0)
                                    {
                                        foreach (string orgCode in HBHCommon.ProjectSendOrgCode)
                                        {
                                            Organization org = Organization.Finder.Find("Code=@OrgCode"
                                                , new OqlParam(orgCode)
                                                );
                                            if (org != null)
                                            {
                                                onlineSendObjsProxy.ToOrgList.Add(org.ID);
                                            }
                                        }
                                    }
                                    onlineSendObjsProxy.Do();
                                }
                                catch (System.Exception e)
                                {
                                    //throw new System.ApplicationException(string.Format("{0}项目下发失败：{1}", project2.Code, ex.Message));
                                    result.IsSuccess = false;
                                    result.ErrorInfo = e.Message;
                                    result.Timestamp = System.DateTime.Now.ToString();
                                    results.Add(result);
                                    //result2 = results;
                                    //return result2;
                                    return results;
                                }
                            }
                            try
                            {
                                CommonCreateSOSRVProxy proxy = new CommonCreateSOSRVProxy();
                                proxy.SOs = (this.GetSaleOrderDTODataList(bpObj));
                                proxy.ContextDTO = (new ContextDTOData());
                                proxy.ContextDTO.OrgID = (Context.LoginOrg.ID);
                                proxy.ContextDTO.OrgCode = (Context.LoginOrg.Code);
                                proxy.ContextDTO.EntCode = (enterprise);
                                proxy.ContextDTO.UserCode = (usercode);
                                proxy.ContextDTO.CultureName = (Context.LoginLanguageCode);
                                System.Collections.Generic.List<CommonArchiveDataDTOData> resultsolist = proxy.Do();
                                if (resultsolist == null || resultsolist.Count == 0)
                                {
                                    result.IsSuccess = false;
                                    result.ErrorInfo = "没有生成销售订单";
                                    result.Timestamp = System.DateTime.Now.ToString();
                                    results.Add(result);
                                    //result2 = results;
                                    return results;
                                }
                                SOStatusTransferBPProxy bp = new SOStatusTransferBPProxy();
                                bp.SOKeyDTOList = new System.Collections.Generic.List<SM.SO.SOKeyDTOData>();
                                foreach (CommonArchiveDataDTOData d in resultsolist)
                                {
                                    SM.SO.SOKeyDTOData dto = new SM.SO.SOKeyDTOData();
                                    dto.SOkey = (d.ID);
                                    dto.TargetStatus = (2);
                                    bp.SOKeyDTOList.Add(dto);
                                }
                                statusDTOs = bp.Do();
                                bp = new SOStatusTransferBPProxy();
                                bp.SOKeyDTOList = (new System.Collections.Generic.List<SM.SO.SOKeyDTOData>());
                                foreach (SOStatusDTOData dt in statusDTOs)
                                {
                                    SM.SO.SOKeyDTOData dto = new SM.SO.SOKeyDTOData();
                                    dto.SOkey = (dt.SOID);
                                    dto.SOSysVersion = (dt.SysVersion);
                                    dto.TargetStatus = (3);
                                    bp.SOKeyDTOList.Add(dto);
                                }
                                statusDTOs = bp.Do();
                                trans.Commit();
                            }
                            catch (System.Exception e)
                            {
                                trans.Rollback();
                                result.IsSuccess = false;
                                result.ErrorInfo = e.Message;
                                result.Timestamp = System.DateTime.Now.ToString();
                                results.Add(result);
                                //result2 = results;
                                //return result2;
                                return results;
                            }
                        }
                        if (statusDTOs != null && statusDTOs.Count > 0)
                        {
                            foreach (SOStatusDTOData dt in statusDTOs)
                            {
                                SO so = SO.Finder.FindByID(dt.SOID);
                                if (so != null)
                                {
                                    result.IsSuccess = true;
                                    result.ErrorInfo = "生单成功";
                                    result.DMSDocNo = so.DescFlexField.PubDescSeg5;
                                    result.ERPDocNo = so.DocNo;
                                    results.Add(result);
                                }
                            }
                        }
                        //result2 = results;
                    }
                }
            }
            catch (System.Exception e)
            {
                result.IsSuccess = false;
                result.ErrorInfo = e.Message;
                result.Timestamp = System.DateTime.Now.ToString();
                results.Add(result);
                //result2 = results;
            }
            //return result2;
            return results;
        }

        // 传入参数非空校验
        /// <summary>
        /// 传入参数非空校验
        /// </summary>
        /// <param name="bpObj"></param>
        private string ValidateParamNullOrEmpty(CreateApprovedSaleOrderSV bpObj)
        {
            string errormessage = string.Empty;
            foreach (SoLineDTO solinedto in bpObj.SoLineDto)
            {
                if (string.IsNullOrEmpty(solinedto.DealerCode))
                {
                    errormessage += string.Format("[{0}]DMS销售订单的[经销商代码]不可为空,", solinedto.DmsSaleNo);
                }
                else
                {
                    Customer customer = Customer.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID, solinedto.DealerCode), new OqlParam[0]);
                    if (customer == null)
                    {
                        errormessage += string.Format("[{0}]DMS销售订单的[经销商代码({1})]在U9系统中找不到对应的客户档案,请同步,", solinedto.DmsSaleNo, solinedto.DealerCode);
                    }
                }
                if (string.IsNullOrEmpty(solinedto.OrderType))
                {
                    errormessage += string.Format("[{0}]DMS销售订单的[订单类型]不可为空,", solinedto.DmsSaleNo);
                }
                else
                {
                    SODocType doctype = SODocType.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID, solinedto.OrderType), new OqlParam[0]);
                    if (doctype == null)
                    {
                        errormessage += string.Format("[{0}]DMS销售订单的[订单类型]在U9系统中找不到对应的[销售单据类型({1})],请同步,", solinedto.DmsSaleNo, solinedto.OrderType);
                    }
                }
                if (string.IsNullOrEmpty(solinedto.ErpMaterialCode))
                {
                    errormessage += string.Format("[{0}]DMS销售订单的参数SOLines的[ERP物料编号]不可为空,", solinedto.DmsSaleNo);
                }
                else
                {
                    ItemMaster item = ItemMaster.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID, solinedto.ErpMaterialCode), new OqlParam[0]);
                    if (item == null)
                    {
                        errormessage += string.Format("[{0}]DMS销售订单的参数SOLines的[ERP物料编号({1})]在U9系统中找不到对应的料品档案,请同步,", solinedto.DmsSaleNo, solinedto.ErpMaterialCode);
                    }
                }
                SO so = SO.Finder.Find(string.Format("Org={0} and DescFlexField.PubDescSeg5='{1}'", Context.LoginOrg.ID, solinedto.DmsSaleNo), new OqlParam[0]);
                if (so != null)
                {
                    errormessage += string.Format("DMS订单[{0}]已经生成U9标准销售订单{1},不能重复生成,", solinedto.DmsSaleNo, so.DocNo);
                }
            }
            return errormessage;
        }

        // 得到销售订单dto
        /// <summary>
        /// 得到销售订单dto
        /// </summary>
        /// <param name="bpObj"></param>
        /// <returns></returns>
        private System.Collections.Generic.List<SaleOrderDTOData> GetSaleOrderDTODataList(CreateApprovedSaleOrderSV bpObj)
        {
            System.Collections.Generic.List<SaleOrderDTOData> list = new System.Collections.Generic.List<SaleOrderDTOData>();
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<SoLineDTO>> dic = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<SoLineDTO>>();
            foreach (SoLineDTO solinedto in bpObj.SoLineDto)
            {
                if (!dic.ContainsKey(solinedto.SpitOrderFlag))
                {
                    dic.Add(solinedto.SpitOrderFlag, new System.Collections.Generic.List<SoLineDTO>());
                }
                dic[solinedto.SpitOrderFlag].Add(solinedto);
            }
            foreach (string key in dic.Keys)
            {
                List<SoLineDTO> listLineDTO = dic[key];

                if (listLineDTO != null
                    && listLineDTO.Count > 0
                    )
                {
                    SoLineDTO firstDTO = listLineDTO.GetFirst<SoLineDTO>();

                    SaleOrderDTOData sodto = new SaleOrderDTOData();
                    sodto.DocumentType = (new CommonArchiveDataDTOData());
                    sodto.DocumentType.Code = (firstDTO.OrderType);
                    sodto.OrderBy = (new CustomerMISCInfoData());
                    sodto.OrderBy.Code = (firstDTO.DealerCode);
                    sodto.SOSrcType = (SOSourceTypeEnum.Manual.Value);
                    sodto.TC = (new CommonArchiveDataDTOData());
                    //sodto.TC.Code = (string.IsNullOrEmpty(firstDTO.Currency) ? "C001" : firstDTO.Currency);
                    sodto.TC.Code = (string.IsNullOrEmpty(firstDTO.Currency) ? HBHCommon.DefaultCurrencyCode : firstDTO.Currency);
                    sodto.DescFlexField = (new DescFlexSegmentsData());
                    sodto.DescFlexField.PubDescSeg5 = (firstDTO.DmsSaleNo);
                    //string RecTerm = "01";
                    string RecTerm = HBHCommon.DefaultRecTermCode;
                    Customer customer = Customer.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID, firstDTO.DealerCode), new OqlParam[0]);
                    if (customer != null)
                    {
                        sodto.ConfirmTerm = (new CommonArchiveDataDTOData());
                        if (customer.ARConfirmTerm != null)
                        {
                            sodto.ConfirmTerm.Code = (customer.ARConfirmTerm.Code);
                        }
                        else
                        {
                            sodto.ConfirmTerm.Code = HBHCommon.DefaultConfirmTermCode;  // ("01");
                        }
                        sodto.BargainMode = (customer.Bargain.Value);
                        sodto.ShipRule = (new CommonArchiveDataDTOData());
                        if (customer.ShippmentRuleKey != null)
                        {
                            sodto.ShipRule.Code = (customer.ShippmentRule.Code);
                        }
                        else
                        {
                            sodto.ShipRule.Code = HBHCommon.DefaultShipRuleCode;    // ("C001");
                        }
                        if (customer.RecervalTermKey != null)
                        {
                            RecTerm = customer.RecervalTerm.Code;
                        }
                    }
                    else
                    {
                        sodto.ShipRule.Code = HBHCommon.DefaultShipRuleCode;    // ("C001");
                        sodto.BargainMode = HBHCommon.DefaultBargainMode;    // (0);
                        sodto.ConfirmTerm = (new CommonArchiveDataDTOData());
                        sodto.ConfirmTerm.Code = HBHCommon.DefaultConfirmTermCode;  // ("01");
                    }
                    sodto.SOLines = new List<ISV.SM.SOLineDTOData>();
                    foreach (SoLineDTO srcsolinedto in dic[key])
                    {
                        UFIDA.U9.ISV.SM.SOLineDTOData solinedto2 = new UFIDA.U9.ISV.SM.SOLineDTOData();
                        solinedto2.ItemInfo = (new ItemInfoData());
                        solinedto2.ItemInfo.ItemCode = (srcsolinedto.ErpMaterialCode);
                        if (!string.IsNullOrEmpty(srcsolinedto.FinalPrice))
                        {
                            solinedto2.FinallyPriceTC = (decimal.Parse(srcsolinedto.FinalPrice));
                        }
                        if (!string.IsNullOrEmpty(srcsolinedto.Number))
                        {
                            solinedto2.OrderByQtyPU = (decimal.Parse(srcsolinedto.Number));
                        }
                        else
                        {
                            solinedto2.OrderByQtyPU = (1m);
                        }
                        if (!string.IsNullOrEmpty(srcsolinedto.Money))
                        {
                            solinedto2.TotalMoneyTC = (decimal.Parse(srcsolinedto.Money));
                        }
                        solinedto2.Project = (new CommonArchiveDataDTOData());
                        solinedto2.Project.Code = (srcsolinedto.DmsSaleNo);
                        solinedto2.SrcDocType = (SOSourceTypeEnum.Manual.Value);
                        solinedto2.RecTerm = (new CommonArchiveDataDTOData());
                        solinedto2.RecTerm.Code = (RecTerm);
                        solinedto2.DescFlexField = (new DescFlexSegmentsData());
                        solinedto2.DescFlexField.PrivateDescSeg1 = (srcsolinedto.MaterialCode);
                        solinedto2.SOShiplines = (new System.Collections.Generic.List<SOShipLineDTOData>());
                        SOShipLineDTOData soshipliendto = new SOShipLineDTOData();
                        soshipliendto.Project = (new CommonArchiveDataDTOData());
                        soshipliendto.Project.Code = (srcsolinedto.DmsSaleNo);
                        soshipliendto.ItemInfo = (new ItemInfoData());
                        soshipliendto.ItemInfo.ItemCode = (srcsolinedto.ErpMaterialCode);
                        soshipliendto.IsMRPRequire = (true);
                        soshipliendto.RequireDate = (string.IsNullOrEmpty(srcsolinedto.DeliveryDate) ? System.DateTime.Now : System.Convert.ToDateTime(srcsolinedto.DeliveryDate));
                        solinedto2.SOShiplines.Add(soshipliendto);
                        sodto.SOLines.Add(solinedto2);
                    }
                    list.Add(sodto);
                }
            }
            return list;
        }
    }
}