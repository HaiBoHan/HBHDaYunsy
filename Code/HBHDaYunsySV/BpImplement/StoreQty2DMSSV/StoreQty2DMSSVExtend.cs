using U9.VOB.Cus.HBHDaYunsy.HBHDaYunsySV;
namespace U9.VOB.Cus.HBHDaYunsy
{
	using System;
	using System.Collections.Generic;
	using System.Text; 
	using UFSoft.UBF.AopFrame;	
	using UFSoft.UBF.Util.Context;
    using U9.VOB.Cus.HBHDaYunsy.PlugInBE;
    using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_PI07;
    using UFIDA.U9.InvTrans.WhQoh;
    using HBH.DoNet.DevPlatform.EntityMapping;
    using UFIDA.U9.CBO.SCM.Supplier;
    using UFSoft.UBF.PL;
    using UFIDA.U9.Cust.HBDY.API;
    using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_PI06;
    using UFIDA.U9.SPR.SalePriceList;
    using UFIDA.U9.CBO.SCM.Customer;
    using UFIDA.U9.Base;
    using UFIDA.U9.SPR.SalePriceAdjustment;

	/// <summary>
	/// StoreQty2DMSSV partial 
	/// </summary>	
	public partial class StoreQty2DMSSV 
	{	
		internal BaseStrategy Select()
		{
			return new StoreQty2DMSSVImpementStrategy();	
		}		
	}
	
    //#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class StoreQty2DMSSVImpementStrategy : BaseStrategy
	{
		public StoreQty2DMSSVImpementStrategy() { }

		public override object Do(object obj)
		{						
			StoreQty2DMSSV bpObj = (StoreQty2DMSSV)obj;
			
			//get business operation context is as follows
			//IContext context = ContextManager.Context	
			
			//auto generating code end,underside is user custom code
			//and if you Implement replace this Exception Code...
            //throw new NotImplementedException();

            // 更新所有库存数据


            if (bpObj != null)
            {
                switch (bpObj.TransferType)
                {
                    case (int)DaYun2DMSTransferTypeEnum.Whqoh:
                        {
                            UpdateDMS_Whqoh(bpObj);
                        }
                        break;
                    case (int)DaYun2DMSTransferTypeEnum.SupplySource:
                        {
                            UpdateDMS_SupplySource(bpObj);
                        }
                        break;
                    case (int)DaYun2DMSTransferTypeEnum.Supplier:
                        {
                            UpdateDMS_Supplier(bpObj);
                        }
                        break;
                    case (int)DaYun2DMSTransferTypeEnum.Customer:
                        {
                            UpdateDMS_Customer(bpObj);
                        }
                        break;
                    case (int)DaYun2DMSTransferTypeEnum.PriceList:
                        {
                            UpdateDMS_PriceList(bpObj);
                        }
                        break;
                    case (int)DaYun2DMSTransferTypeEnum.SalePriceAdjustment:
                        {
                            UpdateDMS_SalePriceAdjustment(bpObj);
                        }
                        break;
                }
            }

            return null;

		}

        private void UpdateDMS_Whqoh(StoreQty2DMSSV bpObj)
        {
            //string opath = "(Wh.DescFlexField.PrivateDescSeg3 = 'True') and (SupplierInfo.Supplier is not null and SupplierInfo.Supplier > 0 and SupplierInfo.Supplier.Category.DescFlexField.PrivateDescSeg1 = 'True') and exists( select 1 from UFIDA::U9::CBO::SCM::Supplier::SupplierItem supItem where supItem.SupplierInfo.Supplier = UFIDA::U9::InvTrans::WhQoh::WhQoh.SupplierInfo.Supplier and supItem.ItemInfo.ItemID = UFIDA::U9::InvTrans::WhQoh::WhQoh.ItemInfo.ItemID ) @AddSuptOpath ";
            // SupplierInfo.Supplier is not null and SupplierInfo.Supplier > 0 and SupplierInfo.Supplier.Category.DescFlexField.PrivateDescSeg1 = 'True'
            //string opath = "(Wh.DescFlexField.PrivateDescSeg3 is not null and Wh.DescFlexField.PrivateDescSeg3 = 'True') and (LotInfo.LotMaster_EntityID is not null and LotInfo.LotMaster_EntityID > 0 and LotInfo.LotMaster_EntityID.DescFlexSegments.PrivateDescSeg1 is not null and LotInfo.LotMaster_EntityID.DescFlexSegments.PrivateDescSeg1 != '' ) and (LotInfo.LotMaster_EntityID.DescFlexSegments.PrivateDescSeg1 in (select supt.Code from UFIDA::U9::CBO::SCM::Supplier::Supplier supt where supt.Category.DescFlexField.PrivateDescSeg1 = 'True'))  @AddSuptOpath ";
            // 由交叉档 改为 货源表
            string opath = "Wh.Org=@Org and (Wh.DescFlexField.PrivateDescSeg3 is not null and Wh.DescFlexField.PrivateDescSeg3 = 'True') and (LotInfo.LotMaster_EntityID is not null and LotInfo.LotMaster_EntityID > 0 and LotInfo.LotMaster_EntityID.DescFlexSegments.PrivateDescSeg1 is not null and LotInfo.LotMaster_EntityID.DescFlexSegments.PrivateDescSeg1 != '' ) and (LotInfo.LotMaster_EntityID.DescFlexSegments.PrivateDescSeg1 in (select supt.Code from UFIDA::U9::CBO::SCM::Supplier::Supplier supt where supt.Category.DescFlexField.PrivateDescSeg1 = 'True'))  @AddSuptOpath ";

            if (bpObj.SupItems != null
                && bpObj.SupItems.Count > 0
                )
            {
                string ids = bpObj.SupItems.GetOpathFromList();

                opath = opath.Replace("@AddSuptOpath"
                    , string.Format(" and ItemInfo.ItemID in ({0})", ids)
                    );
            }
            else
            {
                opath = opath.Replace("@AddSuptOpath"
                    , string.Empty
                    );
            }

            WhQoh.EntityList lst = WhQoh.Finder.FindAll(opath, new OqlParam(Context.LoginOrg.ID));

            if (lst != null
                && lst.Count > 0
                )
            {
                //SendWhqoh2DMS_Async(lst);
                List<List<WhQoh>> pageList = PageList<WhQoh>(lst);

                if (pageList != null
                    && pageList.Count > 0
                    )
                {
                    foreach (List<WhQoh> page in pageList)
                    {
                        //PubExtend.BatchSend2DMS_Async(lst, 2);
                        BatchSend2DMS_Async(page);
                    }
                }
            }
        }
        
        private void UpdateDMS_SupplySource(StoreQty2DMSSV bpObj)
        {             
            string opath = "Org=@Org @AddSuptOpath ";
            
            if (bpObj.SupItems != null
                && bpObj.SupItems.Count > 0
                )
            {
                string ids = bpObj.SupItems.GetOpathFromList();

                opath = opath.Replace("@AddSuptOpath"
                    , string.Format(" and ID in ({0})", ids)
                    );
            }
            else
            {
                opath = opath.Replace("@AddSuptOpath"
                    , string.Empty
                    );
            }

            SupplySource.EntityList lst = SupplySource.Finder.FindAll(opath,new OqlParam(Context.LoginOrg.ID));
            
            if (lst != null
                && lst.Count > 0
                )
            {
                List<List<SupplySource>> pageList = PageList<SupplySource>(lst);

                if (pageList != null
                    && pageList.Count > 0
                    )
                {
                    foreach (List<SupplySource> page in pageList)
                    {
                        //PubExtend.BatchSend2DMS_Async(lst, 2);
                        PubExtend.BatchSend2DMS_Async(page, 2);
                    }
                }
            }
        }


        private void UpdateDMS_Supplier(StoreQty2DMSSV bpObj)
        {
            string opath = "1=1 @AddSuptOpath ";

            if (bpObj.SupItems != null
                && bpObj.SupItems.Count > 0
                )
            {
                string ids = bpObj.SupItems.GetOpathFromList();

                opath = opath.Replace("@AddSuptOpath"
                    , string.Format(" and ID in ({0})", ids)
                    );
            }
            else
            {
                opath = opath.Replace("@AddSuptOpath"
                    , string.Empty
                    );
            }

            Supplier.EntityList lst = Supplier.Finder.FindAll(opath, new OqlParam(Context.LoginOrg.ID));

            if (lst != null
                && lst.Count > 0
                )
            {
                List<List<Supplier>> pageList = PageList<Supplier>(lst);

                if (pageList != null
                    && pageList.Count > 0
                    )
                {
                    foreach (List<Supplier> page in pageList)
                    {
                        //PubExtend.BatchSend2DMS_Async(lst, 2);
                        PubExtend.BatchSend2DMS_Async(page, 2);
                    }
                }
            }
        }

        private void UpdateDMS_Customer(StoreQty2DMSSV bpObj)
        {
            string opath = "1=1 @AddSuptOpath ";

            if (bpObj.SupItems != null
                && bpObj.SupItems.Count > 0
                )
            {
                string ids = bpObj.SupItems.GetOpathFromList();

                opath = opath.Replace("@AddSuptOpath"
                    , string.Format(" and ID in ({0})", ids)
                    );
            }
            else
            {
                opath = opath.Replace("@AddSuptOpath"
                    , string.Empty
                    );
            }

            Customer.EntityList lst = Customer.Finder.FindAll(opath, new OqlParam(Context.LoginOrg.ID));

            if (lst != null
                && lst.Count > 0
                )
            {
                List<List<Customer>> pageList = PageList<Customer>(lst);

                if (pageList != null
                    && pageList.Count > 0
                    )
                {
                    foreach (List<Customer> page in pageList)
                    {
                        //PubExtend.BatchSend2DMS_Async(lst, 2);
                        PubExtend.BatchSend2DMS_Async(page, 2);
                    }
                }
            }
        }

        private void UpdateDMS_PriceList(StoreQty2DMSSV bpObj)
        {
            string opath = "1=1 and SalePriceList.Code = @Code and SalePriceList.Org=@Org  @AddSuptOpath ";

            if (bpObj.SupItems != null
                && bpObj.SupItems.Count > 0
                )
            {
                string ids = bpObj.SupItems.GetOpathFromList();

                opath = opath.Replace("@AddSuptOpath"
                    , string.Format(" and ID in ({0})", ids)
                    );
            }
            else
            {
                // 不选，就不同步
                opath = opath.Replace("@AddSuptOpath"
                    , " and 1=0 "
                    );
            }

            string priceListCode = HBHCommon.GetPartPriceListCode();

            if (priceListCode.IsNotNullOrWhiteSpace())
            {
                SalePriceLine.EntityList lst = SalePriceLine.Finder.FindAll(opath
                    , new OqlParam(priceListCode)
                    , new OqlParam(Context.LoginOrg.ID)
                    );

                if (lst != null
                    && lst.Count > 0
                    )
                {
                    List<List<SalePriceLine>> pageList = PageList<SalePriceLine>(lst);

                    if (pageList != null
                        && pageList.Count > 0
                        )
                    {
                        foreach (List<SalePriceLine> page in pageList)
                        {
                            //PubExtend.BatchSend2DMS_Async(lst, 2);
                            PubExtend.BatchSend2DMS_Async(page, 2);
                        }
                    }
                }
            }
        }

        private void UpdateDMS_SalePriceAdjustment(StoreQty2DMSSV bpObj)
        {
            string opath = "1=1 and SalePriceAdjustment.PriceList.Code = @Code and SalePriceAdjustment.Org=@Org  @AddSuptOpath ";

            if (bpObj.SupItems != null
                && bpObj.SupItems.Count > 0
                )
            {
                string ids = bpObj.SupItems.GetOpathFromList();

                opath = opath.Replace("@AddSuptOpath"
                    , string.Format(" and ID in ({0})", ids)
                    );
            }
            else
            {
                // 不选，就不同步
                opath = opath.Replace("@AddSuptOpath"
                    , " and 1=0 "
                    );
            }

            string priceListCode = HBHCommon.GetPartPriceListCode();

            if (priceListCode.IsNotNullOrWhiteSpace())
            {
                SalePriceAdjustLine.EntityList lst = SalePriceAdjustLine.Finder.FindAll(opath
                    , new OqlParam(priceListCode)
                    , new OqlParam(Context.LoginOrg.ID)
                    );
                
                if (lst != null
                    && lst.Count > 0
                    )
                {
                    List<List<SalePriceAdjustLine>> pageList = PageList<SalePriceAdjustLine>(lst);

                    if (pageList != null
                        && pageList.Count > 0
                        )
                    {
                        foreach (List<SalePriceAdjustLine> page in pageList)
                        {
                            //PubExtend.BatchSend2DMS_Async(lst, 2);
                            PubExtend.BatchSend2DMS_Async(page, 2);
                        }
                    }
                }
            }
        }



        private static void BatchSend2DMS_Async(IList<WhQoh> lst)
        {
            //PI07ImplService service = new PI07ImplService();

            //WSHttpBinding binding = new WSHttpBinding();
            //binding.MaxReceivedMessageSize = int.MaxValue;
            //binding.MaxBufferPoolSize = int.MaxValue;
            ////  http://10.3.11.227:9081/dms/ws/PI07?wsdl
            ////  http://10.3.21.115:8080/hbfcdms/ws/PI07?wsdl
            //System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(new Uri("http://ip/dms/ws/PI07?wsdl")); ;

            //DMSAsync_PI07.PI07Client service = new DMSAsync_PI07.PI07Client(binding, remoteAddress);
            PI07Client service = PubExtend.GetPI07Async();
            // service.Url = PubHelper.GetAddress(service.Url);
            System.Collections.Generic.List<stockDTO> list = new System.Collections.Generic.List<stockDTO>();

            try
            {
                foreach (WhQoh transline in lst)
                {
                    if (transline.LotInfo != null
                        //&& transline.LotInfo.LotMaster_EntityID != null
                        //&& transline.LotInfo.LotMaster_EntityID.DescFlexSegments != null
                        )
                    {
                        //string suptCode = transline.LotInfo.LotMaster_EntityID.DescFlexSegments.PrivateDescSeg1;

                        //if (suptCode.IsNotNullOrWhiteSpace())
                        {
                            //Supplier supt = Supplier.Finder.Find("Code=@Code"
                            //    , new OqlParam(suptCode)
                            //    );

                            //if (PubHelper.IsUpdateDMS(supt))

                            Supplier supt;
                            if (PubHelper.IsUpdateDMS(transline.LotInfo, out supt))
                            {
                                stockDTO dto = new stockDTO();
                                dto.itemMaster = transline.ItemInfo.ItemID.Code;
                                //if (transline.SupplierInfo != null && transline.SupplierInfo.Supplier != null)
                                //{
                                //    dto.supplier = transline.SupplierInfo.Supplier.Code;
                                //}
                                dto.supplier = supt != null ? supt.Code : string.Empty;
                                //else if (transline.DocLine.EntityType == "UFIDA.U9.PM.Rcv.RcvLine")
                                //{
                                //    RcvLine rcvline = RcvLine.Finder.FindByID(transline.DocLine.EntityID);
                                //    if (rcvline != null && rcvline.Receivement.ReceivementType == ReceivementTypeEnum.SaleReturn && rcvline.SrcDocType == RcvSrcDocTypeEnum.RMA && rcvline.SrcDoc != null && rcvline.SrcDoc.SrcDocLine != null && rcvline.SrcDoc.SrcDocLine.EntityID > 0)
                                //    {
                                //        RMALine rmaline = RMALine.Finder.FindByID(rcvline.SrcDoc.SrcDocLine.EntityID);
                                //        if (rmaline != null && rmaline.SrcDocType == RMASrcDocTypeEnum.Ship && rmaline.SrcDocLine != null && rmaline.SrcDocLine.ID > 0)
                                //        {
                                //            ShipLine shipline = ShipLine.Finder.FindByID(rmaline.SrcDocLine.ID);
                                //            if (shipline != null && shipline.Supplier != null && shipline.Supplier.Supplier != null)
                                //            {
                                //                dto.supplier = shipline.Supplier.Supplier.Code;
                                //            }
                                //        }
                                //    }
                                //}
                                dto.number = transline.StoreQty.GetInt();
                                if (transline.LotInfo != null)
                                {
                                    dto.lot = transline.LotInfo.LotCode;
                                }
                                // DMS仓库编码按照U9的建立，因为出库时还要传回来仓库编码；
                                //dto.WH = "SHBJ";
                                if (transline.Wh != null)
                                {
                                    dto.WH = transline.Wh.Code;
                                }
                                //if (transline.Direction == TransDirectionEnum.Rcv)
                                //{
                                //    // 0，入库；1，出库；2，盘点（库存全部更新）
                                //    dto.actionType = 0;
                                //}
                                //else
                                //{
                                //    dto.actionType = 1;
                                //}
                                // actionType   0，入库；1，出库；2，盘点（库存全部更新）
                                // Direction	异动方向   0,入/收；1，出/发
                                dto.actionType = 2;

                                list.Add(dto);
                                //stockDTO[] dtolist = service.Do(list.ToArray());
                                // 改为异步调用了
                                //if (dtolist != null && dtolist.Length > 0 && dtolist[0].flag == 0)
                                //{
                                //    throw new System.ApplicationException(dtolist[0].errMsg);
                                //}
                            }
                        }
                    }
                }

                stockDTO[] dtolist = service.Do(list.ToArray());
            }
            catch (System.Exception e)
            {
                throw new System.ApplicationException("调用DMS接口错误：" + e.Message);
            }
        }

        private static List<List<T>> PageList<T>(IList<T> lst)
        {
            List<List<T>> pageList = new List<List<T>>();
            List<T> onePage = new List<T>();
            //int pageSize = 100;
            int pageSize = 10;
            int currentPage = 0;
            int currentIndex = 0;
            while (currentIndex + currentPage * pageSize < lst.Count
                )
            {
                onePage.Add(lst[currentIndex + currentPage * pageSize]);

                currentIndex++;

                if (currentIndex == pageSize)
                {
                    currentPage++;
                    currentIndex = 0;

                    List<T> currentPageList = new List<T>();
                    currentPageList.AddRange(onePage);
                    pageList.Add(currentPageList);

                    onePage.Clear();
                }
            }

            // 最后一页没有满，仍有数据，添加到集合里;
            if (onePage.Count > 0)
            {
                pageList.Add(onePage);
            }

            return pageList;
        }
	}

    //#endregion
	
	
}