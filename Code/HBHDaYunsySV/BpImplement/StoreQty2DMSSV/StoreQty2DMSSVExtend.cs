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


            UpdateDMS();

		}

        private void UpdateDMS()
        {
            WhQoh.EntityList lst = WhQoh.Finder.FindAll("(Wh.DescFlexField.PrivateDescSeg3 = 'True') and (SupplierInfo.Supplier is not null and SupplierInfo.Supplier > 0 and SupplierInfo.Supplier.Category.DescFlexField.PrivateDescSeg1 = 'True') and exists( select 1 from UFIDA::U9::CBO::SCM::Supplier::SupplierItem supItem where supItem.SupplierInfo.Supplier = UFIDA::U9::InvTrans::WhQoh::WhQoh.SupplierInfo.Supplier and supItem.ItemInfo.ItemID = UFIDA::U9::InvTrans::WhQoh::WhQoh.ItemInfo.ItemID ) ");

            if (lst != null
                && lst.Count > 0
                )
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
                        stockDTO dto = new stockDTO();
                        dto.itemMaster = transline.ItemInfo.ItemID.Code;
                        if (transline.SupplierInfo != null && transline.SupplierInfo.Supplier != null)
                        {
                            dto.supplier = transline.SupplierInfo.Supplier.Code;
                        }
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

                    stockDTO[] dtolist = service.Do(list.ToArray());
                }
                catch (System.Exception e)
                {
                    throw new System.ApplicationException("调用DMS接口错误：" + e.Message);
                }
            }
        }
	}

    //#endregion
	
	
}