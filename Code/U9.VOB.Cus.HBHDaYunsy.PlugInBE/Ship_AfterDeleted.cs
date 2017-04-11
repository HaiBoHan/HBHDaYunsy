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
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_PI09;

namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
    public class Ship_AfterDeleted : IEventSubscriber
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
                        partOrderInfoDto dmsOrderDTO = new partOrderInfoDto();
                        //// 订单号
                        //dmsOrderDTO.dmsSaleNo = entity.DescFlexField.PubDescSeg5;
                        // 出货单号
                        dmsOrderDTO.dmsSaleNo = entity.DescFlexField.PrivateDescSeg1;
                        //dmsOrderDTO.isDel = true.ToString();
                        dmsOrderDTO.isDel = "1";

                        PI09ImplService proxy = new PI09ImplService();

                        try
                        {
                            partOrderInfoDto result = proxy.Do(dmsOrderDTO);
                            if (result != null && result.flag == 0)
                            {
                                throw new BusinessException(result.errMsg);
                            }
                        }
                        catch (System.Exception e)
                        {
                            throw new BusinessException("调用DMS接口错误：" + e.Message);
                        }
                    }
                }
            }
        }
    }
}
