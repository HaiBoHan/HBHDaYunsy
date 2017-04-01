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
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
	public class CompleteRptUpdated : IEventSubscriber
	{
        private const string Const_MesDocTypeCode = "2";

		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
					CompleteRpt entity = key.GetEntity() as CompleteRpt;
					bool flag = PubHelper.IsUsedDMSAPI();
                    if (flag)
                    {
                        // 制造组织(成都)的才做完工报告
                        if (Context.LoginOrg.Code == PubHelper.Const_OrgCode_Chengdu)
                        {
                            if (entity.ProjectKey != null)
                            {
                                Project project = Project.Finder.Find("Code='" + entity.Project.Code.ToString() + "' and org=" + entity.Project.MasterOrg.ID.ToString(), new OqlParam[0]);
                                if (project != null)
                                {
                                    SOLine soline = SOLine.Finder.Find(string.Format("Project={0}", project.ID.ToString()), new OqlParam[0]);
                                    if (soline != null && !string.IsNullOrEmpty(soline.SO.DescFlexField.PubDescSeg5))
                                    {
                                        if (((entity.MOKey != null && entity.MO.MODocType.Code != "MO02")
                                                || entity.PLSKey != null
                                                )
                                            // 物料一样，说明是整车完工
                                            && soline.ItemInfo.ItemID.Code == entity.Item.Code
                                            )
                                        {
                                            if (entity.DocState == CompleteRptStateEnum.Approved && entity.OriginalData.DocState == CompleteRptStateEnum.Approving)
                                            {
                                                try
                                                {
                                                    SI03ImplService service = new SI03ImplService();
                                                    // service.Url = PubHelper.GetAddress(service.Url);
                                                    vehicleInfoDto dto = new vehicleInfoDto();
                                                    if (entity.ProjectKey != null)
                                                    {
                                                        dto.dmsSaleNo = entity.Project.Code;
                                                    }
                                                    dto.vin = entity.DescFlexField.PubDescSeg12;
                                                    dto.erpMaterialCode = entity.Item.Code;
                                                    // 等待上线0,上线1,下线滞留2,下线调试3,最终检验4,总装入库5,调试检验6,车辆整改7
                                                    dto.nodeStatus = "4";
                                                    dto.oldVin = string.Empty;
                                                    dto.flowingCode = ((entity.DescFlexField.PubDescSeg12.Length >= 8) ? entity.DescFlexField.PubDescSeg12.Substring(entity.DescFlexField.PubDescSeg12.Length - 8, 8) : entity.DescFlexField.PubDescSeg12);
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
                                            else if (entity.DescFlexField.PrivateDescSeg21 != entity.OriginalData.DescFlexField.PrivateDescSeg21)
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
                                                    if (entity.ProjectKey != null)
                                                    {
                                                        dto.dmsSaleNo = entity.Project.Code;
                                                    }
                                                    dto.vin = entity.DescFlexField.PubDescSeg12;
                                                    dto.erpMaterialCode = entity.Item.Code;
                                                    // 等待上线0,上线1,下线滞留2,下线调试3,最终检验4,总装入库5,调试检验6,车辆整改7
                                                    dto.nodeStatus = "4";
                                                    dto.oldVin = entity.DescFlexField.PrivateDescSeg21;
                                                    dto.flowingCode = ((entity.DescFlexField.PrivateDescSeg21.Length >= 8) ? entity.DescFlexField.PrivateDescSeg21.Substring(entity.DescFlexField.PrivateDescSeg21.Length - 8, 8) : entity.DescFlexField.PrivateDescSeg21);
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
                                        else
                                        {
                                            System.DateTime arg_42B_0 = entity.ActualRcvTime;
                                            if (entity.MOKey != null && entity.MO.MODocType.Code == "MO02")
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
                                                    dto.vin = entity.DescFlexField.PubDescSeg12;
                                                    dto.docStatus = 7;
                                                    vehicleChangeInfoDto d = service2.Do(dto);
                                                    if (d != null && d.flag == 0)
                                                    {
                                                        throw new BusinessException(d.errMsg);
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


                            //// MES推送DMS接口
                            //if (entity.DocType != null
                            //    // 底盘完工报告（单据类型 = 总装完工报告）
                            //    && entity.DocType.Code == Const_MesDocTypeCode
                            //    // VIN码有值
                            //    && entity.DescFlexField.PubDescSeg12.IsNotNullOrWhiteSpace()
                            //    )
                            //{
                            //    // 审核
                            //    if (entity.DocState == CompleteRptStateEnum.Approved && entity.OriginalData.DocState == CompleteRptStateEnum.Approving)
                            //    {
                            //        try
                            //        {
                            //            SI01ImplService service = new SI01ImplService();
                            //            // service.Url = PubHelper.GetAddress(service.Url);
                            //            mesDataTmpDto dto = new mesDataTmpDto();
                            //            // vin底盘号 ,bomdm 配件图号,   gysdm供应商代码  pjtm 配件条码   pch 批次号,
                            //            dto.vin = entity.DescFlexField.PubDescSeg12;
                            //            // bomdm 配件图号
                            //            dto.bomdm = string.Empty;
                            //            // gysdm供应商代码
                            //            dto.gysdm = string.Empty;
                            //            // pjtm 配件条码 
                            //            dto.pjtm = string.Empty;
                            //            // pch 批次号
                            //            dto.pch = string.Empty;

                            //            mesDataTmpDto resultdto = service.Do(dto);
                            //            if (resultdto != null && resultdto.flag == 0)
                            //            {
                            //                throw new BusinessException(resultdto.errMsg);
                            //            }
                            //        }
                            //        catch (System.Exception e)
                            //        {
                            //            throw new BusinessException("调用DMS接口SI01[MES接口]错误：" + e.Message);
                            //        }
                            //    }
                            //}



                        }
                    }
				}
			}
		}
	}
}
