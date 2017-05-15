using System;
using UFIDA.U9.CBO.SCM.ProjectTask;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI03;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI09;
using UFIDA.U9.MO.Complete;
using UFIDA.U9.MO.Enums;
using UFIDA.U9.SM.SO;
using UFSoft.UBF.Business;
using UFSoft.UBF.Eventing;
using UFSoft.UBF.PL;
using UFIDA.U9.Base;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI01;
using HBH.DoNet.DevPlatform.EntityMapping;
using System.Collections.Generic;
using COM.DaYun.MES.CJDBE;
using System.Text;
using UFIDA.U9.InvDoc.MiscRcv;
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
    // http://123.234.34.236:8034/U9/erp/display.aspx?lnk=SCM.INV.INV2010_10&sId=3017nid&__dm=true
    // EntityName	DisplayName	DefaultTableName	AssemblyName	UIClassName	UIAssemblyName
    // UFIDA.U9.InvDoc.MiscRcv.MiscRcvTrans	库存杂收单	InvDoc_MiscRcvTrans	UFIDA.U9.InvDoc.InvDocBE	UFIDA.U9.SCM.INV.MiscRcvUIModel.MiscRcvUIMainFormWebPart	UFIDA.U9.SCM.INV.MiscRcvUI.WebPart
    // UFIDA.U9.InvDoc.MiscRcv.MiscRcvTransL	库存杂收单行	InvDoc_MiscRcvTransL	UFIDA.U9.InvDoc.InvDocBE	UFIDA.U9.SCM.INV.MiscRcvUIModel.MiscRcvUIMainFormWebPart	UFIDA.U9.SCM.INV.MiscRcvUI.WebPart
    public class MiscRcvTrans_BeforeDefaultValue : IEventSubscriber
    {
        public void Notify(params object[] args)
        {
            if (args != null && args.Length != 0 && args[0] is EntityEvent)
            {
                BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
                if ((key != null))
                {
                    MiscRcvTrans entity = key.GetEntity() as MiscRcvTrans;

                    if (entity == null)
                        return;

                    // 回运单号有值，则DMS传进来的
                    if (entity.DescFlexField != null
                        && entity.DescFlexField.PrivateDescSeg3.IsNotNullOrWhiteSpace()
                        )
                    {
                        foreach (MiscRcvTransL line in entity.MiscRcvTransLs)
                        {
                            if (line != null)
                            {
                                if (line.IsZeroCost
                                    && line.Price_Sources.IsNull()
                                    )
                                {
                                    line.Price_Sources = "零成本";
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
