using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UFSoft.UBF.Eventing;
using UFSoft.UBF.Business;
using UFIDA.U9.SM.Ship;
using HBH.DoNet.DevPlatform.EntityMapping;
using UFIDA.U9.Base;
using UFIDA.U9.SPR.SalePriceList;
using UFSoft.UBF.PL;
using UFIDA.U9.CBO.SCM.Enums;

namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
    public class Ship_BeforeDefaultValue : IEventSubscriber
    {
        public void Notify(params object[] args)
        {
            if (args != null && args.Length != 0 && args[0] is EntityEvent)
            {
                BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
                if (!(key == null))
                {
                    Ship entity = key.GetEntity() as Ship;

                    // DMS配件订单赋值价表
                    if (entity != null
                        && entity.DescFlexField != null
                        && entity.DescFlexField.PrivateDescSeg1.IsNotNullOrWhiteSpace()
                        )
                    {
                        //if (entity.SysState == UFSoft.UBF.PL.Engine.ObjectState.Inserted)
                        //{
                        //    if (Context.LoginOrg.Code == PubHelper.Const_OrgCode_Electric)
                        //    {
                        //        if (entity.DocumentType != null)
                        //        {
                        //            if (entity.DocumentType.Code == PubHelper.Const_ShipDocType_XJ
                        //                || entity.DocumentType.Code == PubHelper.Const_ShipDocType_SB
                        //                )
                        //            {
                        //                if (entity.PriceList <= 0)
                        //                {
                        //                    SalePriceList priceList = SalePriceList.Finder.Find("Code=@Code"
                        //                        , new OqlParam(PubHelper.Const_ElectricPartPriceListCode)
                        //                        );

                        //                    if (priceList != null)
                        //                    {
                        //                        entity.PriceList = priceList.ID;
                        //                        entity.PriceListNo = priceList.Code;
                        //                        entity.PriceListName = priceList.Name;

                        //                        foreach (ShipLine line in entity.ShipLines)
                        //                        {
                        //                            line.PriceList = priceList.ID;
                        //                            line.PriceListCode = priceList.Code;
                        //                            line.PriceListName = priceList.Name;

                        //                            line.PriceSource = PriceSourceEnum.PriceList;
                        //                        }

                        //                    }
                        //                }

                        //            }
                        //        }
                        //    }
                        //}
                    }
                }
            }
        }
    }
}
