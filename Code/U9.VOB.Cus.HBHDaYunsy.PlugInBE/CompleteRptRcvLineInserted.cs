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
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
    public class CompleteRptRcvLineInserted : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
                    CompleteRptRcvLine entity = key.GetEntity() as CompleteRptRcvLine;
                    CompleteRpt rpt = entity.CompleteRpt;

                    if (entity == null
                        || rpt == null
                        )
                        return;

					bool flag = PubHelper.IsUsedDMSAPI();
                    if (flag)
                    {
                        // 制造组织(成都)的才做完工报告
                        if (Context.LoginOrg.Code == PubHelper.Const_OrgCode_Chengdu)
                        {
                            if (rpt.ProjectKey != null)
                            {
                                Project project = Project.Finder.Find("Code='" + rpt.Project.Code.ToString() + "' and org=" + rpt.Project.MasterOrg.ID.ToString(), new OqlParam[0]);
                                if (project != null)
                                {
                                    SOLine soline = SOLine.Finder.Find(string.Format("Project={0}", project.ID.ToString()), new OqlParam[0]);
                                    if (soline != null && !string.IsNullOrEmpty(soline.SO.DescFlexField.PubDescSeg5))
                                    {
                                        if (((rpt.MOKey != null && rpt.MO.MODocType.Code != "MO02")
                                                || rpt.PLSKey != null
                                                )
                                            // 物料一样，说明是整车完工
                                            && soline.ItemInfo.ItemID.Code == rpt.Item.Code
                                            )
                                        {
                                            //if (entity.DocState == CompleteRptStateEnum.Approved && entity.OriginalData.DocState == CompleteRptStateEnum.Approving)
                                            {
                                                try
                                                {
                                                    SI03ImplService service = new SI03ImplService();
                                                    // service.Url = PubHelper.GetAddress(service.Url);
                                                    vehicleInfoDto dto = new vehicleInfoDto();
                                                    if (rpt.ProjectKey != null)
                                                    {
                                                        dto.dmsSaleNo = rpt.Project.Code;
                                                    }
                                                    dto.vin = rpt.DescFlexField.PubDescSeg12;
                                                    dto.erpMaterialCode = rpt.Item.Code;
                                                    // 等待上线0,上线1,下线滞留2,下线调试3,最终检验4,总装入库5,调试检验6,车辆整改7
                                                    dto.nodeStatus = "5";
                                                    dto.oldVin = string.Empty;
                                                    dto.flowingCode = ((rpt.DescFlexField.PubDescSeg12.Length >= 8) ? rpt.DescFlexField.PubDescSeg12.Substring(rpt.DescFlexField.PubDescSeg12.Length - 8, 8) : rpt.DescFlexField.PubDescSeg12);
                                                    vehicleInfoDto resultdto = service.Do(dto);
                                                    if (resultdto != null && resultdto.flag == 0)
                                                    {
                                                        throw new BusinessException(resultdto.errMsg);
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
                    }
				}
			}
		}
	}
}
