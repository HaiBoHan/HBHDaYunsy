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
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
    public class CompleteRpt_AfterValidate : IEventSubscriber
    {
        public void Notify(params object[] args)
        {
            if (args != null && args.Length != 0 && args[0] is EntityEvent)
            {
                BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
                if ((key != null))
                {
                    CompleteRpt entity = key.GetEntity() as CompleteRpt;

                    if (entity == null)
                        return;

                    // 新增时，设置来源采集点单据ID
                    if (entity.SysState == UFSoft.UBF.PL.Engine.ObjectState.Inserted)
                    {
                        if (entity.DescFlexField != null
                            && entity.DescFlexField.PubDescSeg12.IsNotNullOrWhiteSpace()
                            )
                        {
                            string strVin = entity.DescFlexField.PubDescSeg12;

                            CJDHead.EntityList lstCJD = CJDHead.Finder.FindAll("Org=@Org and Dtzyplan.VIN = @Vin"
                                , new OqlParam(Context.LoginOrg.ID)
                                , new OqlParam(strVin)
                                );

                            if (lstCJD != null
                                && lstCJD.Count > 0
                                )
                            {
                                StringBuilder sbIDs = new StringBuilder();
                                foreach (CJDHead cjdHead in lstCJD)
                                {
                                    if (cjdHead != null
                                        && cjdHead.ID > 0
                                        )
                                    {
                                        sbIDs.Append(cjdHead.ID).Append(",");
                                    }
                                }

                                entity.DescFlexField.PrivateDescSeg23 = sbIDs.GetStringRemoveLastSplit();
                            }
                        }
                    }
                }
            }
        }
    }
}
