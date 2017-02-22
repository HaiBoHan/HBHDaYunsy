using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UFSoft.UBF.Eventing;
using UFSoft.UBF.Business;
using HBH.DoNet.DevPlatform.EntityMapping;
using UFIDA.U9.Base;
using UFSoft.UBF.PL;
using UFIDA.U9.CBO.SCM.Enums;
using UFIDA.U9.PM.PO;
using UFIDA.U9.CBO.SCM.Supplier;
using UFIDA.U9.HHKK.Kkpm;

namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
    /*
  UFIDA.U9.HHKK.Kkpm.ArrivalUI
  UFIDA.U9.HHKK.Kkpm.ArrivalDoc	到货单	Kkpm_ArrivalDoc	UFIDA.U9.HHKK.KkpmBE	ArrivalDoc	UFIDA.U9.HHKK.Kkpm.ArrivalUIModel.ArrivalMainUIFormWebPart	UFIDA.U9.HHKK.Kkpm.ArrivalUI.WebPart   
     */
    public class ArrivalDoc_BeforeDefaultValue : IEventSubscriber
    {
        public void Notify(params object[] args)
        {
            if (args != null && args.Length != 0 && args[0] is EntityEvent)
            {
                BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
                if (!(key == null))
                {
                    ArrivalDoc entity = key.GetEntity() as ArrivalDoc;

                    if (entity != null
                        )
                    {
                        DateTime docDate = entity.BusinessDate;
                        string oldSuptCode = string.Empty;
                        string newSuptCode = string.Empty;

                        if (entity.SysState != UFSoft.UBF.PL.Engine.ObjectState.Inserted
                            && entity.OriginalData != null
                            && entity.OriginalData.Supplier != null
                            )
                        {
                            oldSuptCode = entity.OriginalData.Supplier.Code;
                        }

                        if (entity.Supplier != null)
                        {
                            newSuptCode = entity.Supplier.Code;
                        }

                        foreach (ArrivalDocLine docline in entity.ArrivalDocLines)
                        {
                            string oldItemCode = string.Empty;
                            string newItemCode = string.Empty;

                            if (docline.SysState != UFSoft.UBF.PL.Engine.ObjectState.Inserted
                                && docline.OriginalData != null
                                && docline.OriginalData.ItemInfo != null
                                && docline.OriginalData.ItemInfo.ItemID != null
                                )
                            {
                                oldItemCode = docline.OriginalData.ItemInfo.ItemID.Code;
                            }

                            if (docline.ItemInfo != null 
                                && docline.ItemInfo.ItemID != null
                                )
                            {
                                newItemCode = docline.ItemInfo.ItemID.Code;
                            }

                            if (newSuptCode.IsNotNullOrWhiteSpace()
                                && newItemCode.IsNotNullOrWhiteSpace()
                                // 新旧不一致，才重取
                                && (oldSuptCode != newSuptCode
                                    || oldItemCode != newItemCode
                                    )
                                )
                            {
                                SupplySource suptSource = PurchaseOrder_BeforeDefaultValue.GetSupplySource(docDate, newSuptCode, newItemCode);
                                if (suptSource != null)
                                {
                                    /*
                                    货源表  1
                                     * 
                                    标准采购：9
                                    标准收货：2
                                    到货：2
                                    质检单：6
                                     */
                                    docline.DescFlexField.PrivateDescSeg2 = suptSource.DescFlexField.PrivateDescSeg1;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
