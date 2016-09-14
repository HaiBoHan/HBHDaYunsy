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
	public class CompleteRptUpdated : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
					CompleteRpt rpt = key.GetEntity() as CompleteRpt;
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
                                            if (rpt.DocState == CompleteRptStateEnum.Approved && rpt.OriginalData.DocState == CompleteRptStateEnum.Approving)
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
                                                    dto.nodeStatus = "4";
                                                    dto.oldVin = string.Empty;
                                                    dto.flowingCode = ((rpt.DescFlexField.PubDescSeg12.Length >= 8) ? rpt.DescFlexField.PubDescSeg12.Substring(rpt.DescFlexField.PubDescSeg12.Length - 8, 8) : rpt.DescFlexField.PubDescSeg12);
                                                    vehicleInfoDto resultdto = service.Do(dto);
                                                    if (resultdto != null && resultdto.flag == 0)
                                                    {
                                                        throw new System.ApplicationException(resultdto.errMsg);
                                                    }
                                                }
                                                catch (System.Exception e)
                                                {
                                                    throw new System.ApplicationException("调用DMS接口错误：" + e.Message);
                                                }
                                            }
                                            else if (rpt.DescFlexField.PrivateDescSeg21 != rpt.OriginalData.DescFlexField.PrivateDescSeg21)
                                            //// 新增是不是要写？？？
                                            //if (rpt.DocState == CompleteRptStateEnum.Opened
                                            //    || rpt.DocState == CompleteRptStateEnum.Approving
                                            //    )
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
                                                    dto.nodeStatus = "4";
                                                    dto.oldVin = rpt.DescFlexField.PrivateDescSeg21;
                                                    dto.flowingCode = ((rpt.DescFlexField.PrivateDescSeg21.Length >= 8) ? rpt.DescFlexField.PrivateDescSeg21.Substring(rpt.DescFlexField.PrivateDescSeg21.Length - 8, 8) : rpt.DescFlexField.PrivateDescSeg21);
                                                    vehicleInfoDto resultdto = service.Do(dto);
                                                    if (resultdto != null && resultdto.flag == 0)
                                                    {
                                                        throw new System.ApplicationException(resultdto.errMsg);
                                                    }
                                                }
                                                catch (System.Exception e)
                                                {
                                                    throw new System.ApplicationException("调用DMS接口错误：" + e.Message);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            System.DateTime arg_42B_0 = rpt.ActualRcvTime;
                                            if (rpt.MOKey != null && rpt.MO.MODocType.Code == "MO02")
                                            {
                                                try
                                                {
                                                    SI09ImplService service2 = new SI09ImplService();
                                                    service2.Url = PubHelper.GetAddress(service2.Url);
                                                    //vehicleChangeInfoDto d = service2.receive(new vehicleChangeInfoDto
                                                    //{
                                                    //    vin = rpt.DescFlexField.PubDescSeg12,
                                                    //    docStatus = 7
                                                    //});
                                                    vehicleChangeInfoDto dto = new vehicleChangeInfoDto();
                                                    // 等待上线0,上线1,下线滞留2,下线调试3,最终检验4,总装入库5,调试检验6,车辆整改7
                                                    dto.vin = rpt.DescFlexField.PubDescSeg12;
                                                    dto.docStatus = 7;
                                                    vehicleChangeInfoDto d = service2.Do(dto);
                                                    if (d != null && d.flag == 0)
                                                    {
                                                        throw new System.ApplicationException(d.errMsg);
                                                    }
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
			}
		}
	}
}
