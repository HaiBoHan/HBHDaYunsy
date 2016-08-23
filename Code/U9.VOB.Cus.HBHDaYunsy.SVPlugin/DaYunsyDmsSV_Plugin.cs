using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UFSoft.UBF.Service;
using UFIDA.U9.Cust.HBDY.API;
using HBH.DoNet.DevPlatform.EntityMapping;
using HBH.DoNet.DevPlatform.U9Mapping;

namespace U9.VOB.Cus.HBHDaYunsy.SVPlugin
{
    public class DaYunsyDmsSV_Plugin : BPSVExtendBase
    {
        private long svID = -1;

        public override void BeforeDo(object bp)
        {
            //CreateApprovedSaleOrderSV bpObj = bp as CreateApprovedSaleOrderSV;

            svID = HBHCommon.HBHCommonSVBefore(bp);

            //HBHTransferSV transSV = bp as HBHTransferSV;
            //if (transSV != null)
            //{
            //    transSV.ID = svID;
            //}
        }

        public override void AfterDo(object bp, ref object result)
        {
            if (result != null
                )
            {
                //HBHTransferSV transSV = bp as HBHTransferSV;

                //if (transSV != null)
                //{
                //    svID = transSV.ID;
                //}

                List<TransferInResultDTO> resultTransfer = result as List<TransferInResultDTO>;
                List<ShipBackDTO> resultShipBack = result as List<ShipBackDTO>;
                List<SoBackDTO> resultSOBack = result as List<SoBackDTO>;

                if (resultTransfer != null)
                {
                    TransferInResultDTO first = resultTransfer.GetFirst();

                    if (first != null)
                    {
                        HBHCommon.HBHCommonSVAfter(svID, resultTransfer, first.IsSuccess, first.ErrorInfo, first.ERPDocNo);
                    }
                }
                else if (resultShipBack != null)
                {
                    if (resultShipBack != null)
                    {
                        ShipBackDTO first = resultShipBack.GetFirst();

                        if (first != null)
                        {
                            HBHCommon.HBHCommonSVAfter(svID, result, first.IsSuccess, first.ErrorInfo, first.ERPDocNo);
                        }
                    }
                }
                else if (resultSOBack != null)
                {
                    if (resultSOBack != null)
                    {
                        SoBackDTO first = resultSOBack.GetFirst();

                        if (first != null)
                        {
                            HBHCommon.HBHCommonSVAfter(svID, result, first.IsSuccess, first.ErrorInfo, first.ERPDocNo);
                        }
                    }
                }
                else
                {
                    HBHCommon.HBHCommonSVAfter(svID, result, true, string.Empty, string.Empty);
                }
            }
        }
    }
}
