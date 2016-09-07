using System;
using System.Collections.Generic;
using UFIDA.U9.Base;
using UFIDA.U9.CBO.SCM.Enums;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_PI07;
using UFIDA.U9.InvDoc.MiscShip;
using UFIDA.U9.InvTrans.Trans;
using UFIDA.U9.PM.Enums;
using UFIDA.U9.PM.Rcv;
using UFIDA.U9.SM.RMA;
using UFIDA.U9.SM.Ship;
using UFSoft.UBF.Business;
using UFSoft.UBF.Eventing;
using System.ServiceModel;
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
	public class TransLineInserted : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
					TransLine transline = key.GetEntity() as TransLine;
					if (PubHelper.SaleOrg2DMS.Contains(Context.LoginOrg.Code))
					{
						bool flag = PubHelper.IsUsedDMSAPI();
						if (flag)
						{
                            if (IsUpdateDMS(transline))
							{
                                //if (transline.DocLine != null && transline.DocLine.EntityType != "UFIDA.U9.InvDoc.PrdEndCheck.PrdEndChkBill" && transline.DocLine.EntityType != "UFIDA.U9.SM.Ship.ShipLine")
								{
                                    //string ismiscship = string.Empty;
                                    //if (transline.DocLine.EntityType == "UFIDA.U9.InvDoc.MiscShip.MiscShipmentL" && transline.DocLine.EntityID > 0)
                                    //{
                                    //    MiscShipmentL misshipline = MiscShipmentL.Finder.FindByID(transline.DocLine.EntityID);
                                    //    if (misshipline != null)
                                    //    {
                                    //        ismiscship = misshipline.MiscShip.DescFlexField.PubDescSeg5;
                                    //    }
                                    //}
                                    //if (string.IsNullOrEmpty(ismiscship))
									{
										try
                                        {

                                            //PI07ImplService service = new PI07ImplService();

                                            //WSHttpBinding binding = new WSHttpBinding();
                                            //binding.MaxReceivedMessageSize = int.MaxValue;
                                            //binding.MaxBufferPoolSize = int.MaxValue;
                                            ////  http://10.3.11.227:9081/dms/ws/PI07?wsdl
                                            ////  http://10.3.21.115:8080/hbfcdms/ws/PI07?wsdl
                                            //System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(new Uri("http://ip/dms/ws/PI07?wsdl")); ;

                                            //DMSAsync_PI07.PI07Client service = new DMSAsync_PI07.PI07Client(binding, remoteAddress);
                                            DMSAsync_PI07.PI07Client service = PubExtend.GetPI07Async();
											// service.Url = PubHelper.GetAddress(service.Url);
                                            System.Collections.Generic.List<DMSAsync_PI07.stockDTO> list = new System.Collections.Generic.List<DMSAsync_PI07.stockDTO>();
                                            DMSAsync_PI07.stockDTO dto = new DMSAsync_PI07.stockDTO();
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
											dto.number = System.Convert.ToInt32(transline.StoreUOMQty);
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
                                            dto.actionType = transline.Direction.Value;

											list.Add(dto);
                                            DMSAsync_PI07.stockDTO[] dtolist = service.Do(list.ToArray());
                                            //stockDTO[] dtolist = service.Do(list.ToArray());
                                            // 改为异步调用了
                                            //if (dtolist != null && dtolist.Length > 0 && dtolist[0].flag == 0)
                                            //{
                                            //    throw new System.ApplicationException(dtolist[0].errMsg);
                                            //}
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

        public static bool IsUpdateDMS(TransLine transline)
        {
            //return transline.Wh != null && transline.Wh.Code.StartsWith("SHBJ");
            return transline.Wh != null 
                && PubHelper.IsWarehouse2DMS(transline.Wh)
                ;
        }
	}
}
