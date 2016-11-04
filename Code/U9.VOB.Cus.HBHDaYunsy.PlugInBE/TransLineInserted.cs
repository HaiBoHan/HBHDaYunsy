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
using HBH.DoNet.DevPlatform.EntityMapping;
using UFIDA.U9.CBO.SCM.Supplier;
using UFSoft.UBF.PL;
using UFIDA.U9.Base.Doc;
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
                            Supplier supt;
                            if (IsUpdateDMS(transline, out supt))
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
                                            //dto.number = System.Convert.ToInt32(transline.StoreUOMQty);
                                            dto.number = transline.StoreUOMQty.GetInt();
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

        public static bool IsUpdateDMS(TransLine transline, out Supplier supt)
        {
            supt = null;

            //return transline.Wh != null && transline.Wh.Code.StartsWith("SHBJ");
            if (transline.Wh != null
                && PubHelper.IsWarehouse2DMS(transline.Wh)
                //&& PubHelper.IsUpdateDMS(transline.SupplierInfo)
                )
            {
                //// 批次供应商
                //string suptCode = transline.LotInfo.LotMaster_EntityID.DescFlexSegments.PrivateDescSeg1;
                //if (suptCode.IsNotNullOrWhiteSpace())
                //{
                //    Supplier supt = Supplier.Finder.Find("Code=@Code"
                //        , new OqlParam(suptCode)
                //        );

                //    if (PubHelper.IsUpdateDMS(supt))
                //    {
                //        return true;
                //    }
                //}

                // 刨除VMI结算
                if (IsUpdateWhqoh(transline))
                {
                    // 如果不是DMS的出货单，才更新DMS；
                    if (!IsDMSShipment(transline))
                    {
                        // 如果存在货源表
                        bool bl = PubHelper.IsUpdateDMS(transline.LotInfo, out supt);

                        if (bl)
                        {
                            SupplySource supplySource = SupplySource.Finder.Find("ItemInfo.ItemID=@ItemID and SupplierInfo.Supplier=@SuptID"
                                , new OqlParam(transline.ItemInfo.ItemIDKey.ID)
                                , new OqlParam(supt.ID)
                                );

                            if (supplySource != null)
                            {
                                return SupplySourceInserted.IsUpdateDMS(supplySource);
                            }
                        }
                    }
                }
            }

            return false;
        }

        // 是否更新库存(刨除VMI结算)
        /// <summary>
        /// 是否更新库存(刨除VMI结算)
        /// </summary>
        /// <param name="transline"></param>
        /// <returns></returns>
        public static bool IsUpdateWhqoh(TransLine transline)
        {
            // QtyPriceDealFlg 数量价值处理       (消耗汇总,为4；不知道价值数量怎么取的，所以没用这个判断)
            /*
            NotWorthAndQty	不处理数量和价值	3
            Qty	只处理数量	1
            QtyAndWorth	处理数量和价值	0
            Worth	只处理价值	2
            WorthAndWorthQty	处理价值及价值数量	4
             */

            /* UFIDA.U9.InvTrans.Trans.TransLine   UFIDA.U9.InvTrans.InvTransBE.dll            
		public bool IsAffectQoh
		{
			get
			{
				if ((this.QtyPriceDealFlg == QtyPriceDealFlgEnum.QtyAndWorth && !this.IsIPVChanged()) || this.QtyPriceDealFlg == QtyPriceDealFlgEnum.Qty)
				{
					this.isAffectQoh = true;
				}
				return this.isAffectQoh;
			}
			set
			{
				this.isAffectQoh = value;
			}
		}
             */
            if (transline != null
                //// PM035	VMI采购结算	322
                //&& transline.DocBizType != BusinessTypeEnum.PM035
                // 改成用 异动行公共方法
                && transline.IsAffectQoh
                )
            {
                return true;
            }

            return false;
        }

        private static bool IsDMSShipment(TransLine transline)
        {
            if (transline.DocLine != null
                && transline.DocLine.EntityID > 0
                && transline.DocLine.EntityType == typeof(ShipLine).FullName
                )
            {
                ShipLine shipline = ShipLine.Finder.FindByID(transline.DocLine.EntityID);

                if (PubHelper.IsUpdateDMS(shipline))
                {
                    return true;
                }
            }

            return false;
        }
	}
}
