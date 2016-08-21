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
    using UFIDA.U9.InvDoc.TransferIn.Proxy;
    using UFIDA.U9.ISV.TransferInISV.Proxy;
    using UFIDA.U9.InvDoc.TransferIn;

	/// <summary>
	/// CreateApprovedTransferInSV partial 
	/// </summary>	
	public partial class CreateApprovedTransferInSV 
	{	
		internal BaseStrategy Select()
		{
			return new CreateApprovedTransferInSVImpementStrategy();	
		}		
	}
	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class CreateApprovedTransferInSVImpementStrategy : BaseStrategy
	{
		public CreateApprovedTransferInSVImpementStrategy() { }

		public override object Do(object obj)
		{						
			CreateApprovedTransferInSV bpObj = (CreateApprovedTransferInSV)obj;
			
			//get business operation context is as follows
			//IContext context = ContextManager.Context	
			
			//auto generating code end,underside is user custom code
			//and if you Implement replace this Exception Code...

            System.Collections.Generic.List<TransferInResultDTO> result = new System.Collections.Generic.List<TransferInResultDTO>();
            object result2;
            try
            {
                if (bpObj.TransferInLineDTOList == null || bpObj.TransferInLineDTOList.Count == 0)
                {
                    result.Add(new TransferInResultDTO
                    {
                        IsSuccess = false,
                        ErrorInfo = "传入参数不可为空",
                        Timestamp = System.DateTime.Now
                    });
                    result2 = result;
                }
                else
                {
                    string errormessage = this.ValidateParamNullOrEmpty(bpObj);
                    if (!string.IsNullOrEmpty(errormessage))
                    {
                        result.Add(new TransferInResultDTO
                        {
                            IsSuccess = false,
                            ErrorInfo = errormessage + "请检查传入参数",
                            Timestamp = System.DateTime.Now
                        });
                        result2 = result;
                    }
                    else
                    {
                        System.Collections.Generic.List<CommonArchiveDataDTOData> transinidlist;
                        using (UBFTransactionScope trans = new UBFTransactionScope(TransactionOption.Required))
                        {
                            try
                            {
                                UFIDA.U9.ISV.TransferInISV.Proxy.CommonCreateTransferInSVProxy proxy = new UFIDA.U9.ISV.TransferInISV.Proxy.CommonCreateTransferInSVProxy();
                                proxy.TransferInDTOList = (this.GetTransferInDTOList(bpObj));
                                transinidlist = proxy.Do();
                                if (transinidlist == null || transinidlist.Count <= 0)
                                {
                                    result.Add(new TransferInResultDTO
                                    {
                                        IsSuccess = false,
                                        ErrorInfo = "生单失败：没有生成调入单",
                                        Timestamp = System.DateTime.Now
                                    });
                                    result2 = result;
                                    return result2;
                                }
                                TransferInBatchApproveSRVProxy approveproxy = new TransferInBatchApproveSRVProxy();
                                approveproxy.DocList = (transinidlist);
                                approveproxy.ApprovedBy = (Context.LoginUser);
                                approveproxy.ApprovedOn = (System.DateTime.Now);
                                approveproxy.Do();
                                trans.Commit();
                            }
                            catch (System.Exception e)
                            {
                                trans.Rollback();
                                result.Add(new TransferInResultDTO
                                {
                                    IsSuccess = false,
                                    ErrorInfo = "生单失败：" + e.Message,
                                    Timestamp = System.DateTime.Now
                                });
                                result2 = result;
                                return result2;
                            }
                        }
                        foreach (CommonArchiveDataDTOData transin in transinidlist)
                        {
                            TransferIn t = TransferIn.Finder.FindByID(transin.ID);
                            if (t != null)
                            {
                                result.Add(new TransferInResultDTO
                                {
                                    IsSuccess = true,
                                    ErrorInfo = "生单成功",
                                    Timestamp = System.DateTime.Now,
                                    ERPDocNo = transin.Code,
                                    TransDocNo = t.DescFlexField.PrivateDescSeg4
                                });
                            }
                        }
                        result2 = result;
                    }
                }
            }
            catch (System.Exception e)
            {
                result.Add(new TransferInResultDTO
                {
                    IsSuccess = false,
                    ErrorInfo = e.Message,
                    Timestamp = System.DateTime.Now
                });
                result2 = result;
            }
            return result2;
        }

        // 传入参数非空校验
        /// <summary>
        /// 传入参数非空校验
        /// </summary>
        /// <param name="bpObj"></param>
        private string ValidateParamNullOrEmpty(CreateApprovedTransferInSV bpObj)
        {
            string errormessage = string.Empty;
            foreach (TransInLineDTO linedto in bpObj.TransferInLineDTOList)
            {
                if (linedto.OperationType <= 0)
                {
                    errormessage += string.Format("[{0}]DMS移库单的[操作类型]不可为空,", linedto.TransDocNo);
                }
                if (string.IsNullOrEmpty(linedto.ItemMaster))
                {
                    errormessage += string.Format("[{0}]DMS移库单的参数TtransInLines的[ERP料号]不可为空,", linedto.TransDocNo);
                }
                else
                {
                    ItemMaster item = ItemMaster.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), linedto.ItemMaster), new OqlParam[0]);
                    if (item == null)
                    {
                        errormessage += string.Format("[{0}]DMS移库单的参数TtransInLines的[ERP料号({1})]在U9系统中找不到对应的料品档案,请同步,", linedto.TransDocNo, linedto.ItemMaster);
                    }
                }
                if (string.IsNullOrEmpty(linedto.WhOut))
                {
                    errormessage += string.Format("[{0}]DMS移库单的参数TtransInLines的[调出存储地点]不可为空,", linedto.TransDocNo);
                }
                else
                {
                    Warehouse whout = Warehouse.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), linedto.WhOut), new OqlParam[0]);
                    if (whout == null)
                    {
                        errormessage += string.Format("[{0}]DMS移库单的参数TtransInLines的[调出存储地点({1})]在U9系统中找不到对应的存储地点档案,请同步,", linedto.TransDocNo, linedto.WhOut);
                    }
                }
                if (string.IsNullOrEmpty(linedto.WhIn))
                {
                    errormessage += string.Format("[{0}]DMS移库单的参数TtransInLines的[调入存储地点]不可为空,", linedto.TransDocNo);
                }
                else
                {
                    Warehouse whin = Warehouse.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), linedto.WhIn), new OqlParam[0]);
                    if (whin == null)
                    {
                        errormessage += string.Format("[{0}]DMS移库单的参数TtransInLines的[调入存储地点({1})]在U9系统中找不到对应的存储地点档案,请同步,", linedto.TransDocNo, linedto.WhIn);
                    }
                }
                if (linedto.Number <= 0)
                {
                    errormessage += string.Format("[{0}]DMS移库单的参数TtransInLines的[数量]必须大于0,", linedto.TransDocNo);
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
        private System.Collections.Generic.List<ISV.TransferInISV.IC_TransferInDTOData> GetTransferInDTOList(CreateApprovedTransferInSV bpObj)
        {
            System.Collections.Generic.List<ISV.TransferInISV.IC_TransferInDTOData> list = new System.Collections.Generic.List<ISV.TransferInISV.IC_TransferInDTOData>();
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<TransInLineDTO>> dic = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<TransInLineDTO>>();
            foreach (TransInLineDTO dtoline in bpObj.TransferInLineDTOList)
            {
                if (!dic.ContainsKey(dtoline.SpitOrderFlag))
                {
                    dic.Add(dtoline.SpitOrderFlag, new System.Collections.Generic.List<TransInLineDTO>());
                }
                dic[dtoline.SpitOrderFlag].Add(dtoline);
            }
            foreach (string key in dic.Keys)
            {
                List<TransInLineDTO> listLineDTO = dic[key];

                if (listLineDTO != null
                    && listLineDTO.Count > 0
                    )
                {
                    TransInLineDTO firstDTO = listLineDTO.GetFirst<TransInLineDTO>();

                    ISV.TransferInISV.IC_TransferInDTOData transindto = new ISV.TransferInISV.IC_TransferInDTOData();
                    transindto.TransInDocType = (new CommonArchiveDataDTOData());
                    if (firstDTO.OperationType == 1)
                    {
                        transindto.TransInDocType.Code = ("MoveWH");
                    }
                    else if (firstDTO.OperationType == 2)
                    {
                        transindto.TransInDocType.Code = ("CarOutWH");
                    }
                    transindto.CreatedBy = (Context.LoginUser);
                    transindto.CreatedOn = (System.DateTime.Now);
                    transindto.ModifiedBy = (Context.LoginUser);
                    transindto.ModifiedOn = (System.DateTime.Now);
                    transindto.DescFlexField = (new DescFlexSegmentsData());
                    transindto.DescFlexField.PrivateDescSeg4 = (firstDTO.TransDocNo);
                    transindto.TransInLines = (new System.Collections.Generic.List<ISV.TransferInISV.IC_TransInLineDTOData>());
                    foreach (TransInLineDTO linedto in listLineDTO)
                    {
                        ISV.TransferInISV.IC_TransInLineDTOData transinlinedto = new ISV.TransferInISV.IC_TransInLineDTOData();
                        transinlinedto.ItemInfo = (new ItemInfoData());
                        transinlinedto.ItemInfo.ItemCode = (linedto.ItemMaster);
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
                        transinlinedto.DescFlexSegments = (new DescFlexSegmentsData());
                        transinlinedto.DescFlexSegments.PubDescSeg12 = (linedto.VIN);
                        transinlinedto.Project = (new CommonArchiveDataDTOData());
                        transinlinedto.Project.Code = (linedto.TransDocNo);
                        transinlinedto.TransInSubLines = (new System.Collections.Generic.List<ISV.TransferInISV.IC_TransInSubLineDTOData>());
                        ISV.TransferInISV.IC_TransInSubLineDTOData sublinedto = new ISV.TransferInISV.IC_TransInSubLineDTOData();
                        sublinedto.TransOutWh = (new CommonArchiveDataDTOData());
                        sublinedto.TransOutWh.Code = (linedto.WhOut);
                        sublinedto.Project = (new CommonArchiveDataDTOData());
                        sublinedto.Project.Code = (linedto.TransDocNo);
                        Warehouse whout = Warehouse.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID.ToString(), linedto.WhOut), new OqlParam[0]);
                        if (whout != null)
                        {
                            sublinedto.StorageType = (whout.StorageType.Value);
                        }
                        else
                        {
                            sublinedto.StorageType = (4);
                        }
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