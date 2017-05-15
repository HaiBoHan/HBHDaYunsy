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


                            //return;

                            // MES推送DMS接口
                            /* // 生产线采集点
                            EntityName	DisplayName	DefaultTableName	AssemblyName	ViewName	UIClassName	UIAssemblyName	IsMainView	UID	FilterOriginalOPath	URI	Container[装配assemblyID]	ClassName
COM.DaYun.MES.CJDBE.CarTestCQRecord	车辆调试过程检验质量问题记录	dayun_cartestcqrecord	COM.DaYun.MES.DaYunCJDBE	CJDHead_CarTestCQRecord	DaYunCJDUIModel.DaYunCJDUIMainFromWebPart	COM.DaYun.MES.UI.DaYunCJDUI.WebPart	0	CAB27503-D7D5-4E05-84FB-A084202EE8C3		mes.cjd	66be41ba-d206-4bae-8f52-379345be90ee	DaYunCJDUIModel.DaYunCJDUIMainFromWebPart
COM.DaYun.MES.CJDBE.CarTestQRecord	车辆调试过程质量问题记录	dayun_cartestqrecord	COM.DaYun.MES.DaYunCJDBE	CJDHead_CarTestQRecord	DaYunCJDUIModel.DaYunCJDUIMainFromWebPart	COM.DaYun.MES.UI.DaYunCJDUI.WebPart	0	CAB27503-D7D5-4E05-84FB-A084202EE8C3		mes.cjd	66be41ba-d206-4bae-8f52-379345be90ee	DaYunCJDUIModel.DaYunCJDUIMainFromWebPart
COM.DaYun.MES.CJDBE.CJDHead	生产采集点	dayun_cjd	COM.DaYun.MES.DaYunCJDBE	CJDHead	DaYunCJDUIModel.DaYunCJDUIMainFromWebPart	COM.DaYun.MES.UI.DaYunCJDUI.WebPart	1	CAB27503-D7D5-4E05-84FB-A084202EE8C3		mes.cjd	66be41ba-d206-4bae-8f52-379345be90ee	DaYunCJDUIModel.DaYunCJDUIMainFromWebPart
COM.DaYun.MES.CJDBE.CarAssemblyQRecord	车辆装配过程质量问题记录	dayun_carassemblyqrecord	COM.DaYun.MES.DaYunCJDBE	CJDHead_CarAssemblyQRecord	DaYunCJDUIModel.DaYunCJDUIMainFromWebPart	COM.DaYun.MES.UI.DaYunCJDUI.WebPart	0	CAB27503-D7D5-4E05-84FB-A084202EE8C3		mes.cjd	66be41ba-d206-4bae-8f52-379345be90ee	DaYunCJDUIModel.DaYunCJDUIMainFromWebPart
COM.DaYun.MES.CJDBE.CarCQRecord	车辆最终检测质量问题记录	dayun_carcqrecord	COM.DaYun.MES.DaYunCJDBE	CJDHead_CarCQRecord	DaYunCJDUIModel.DaYunCJDUIMainFromWebPart	COM.DaYun.MES.UI.DaYunCJDUI.WebPart	0	CAB27503-D7D5-4E05-84FB-A084202EE8C3		mes.cjd	66be41ba-d206-4bae-8f52-379345be90ee	DaYunCJDUIModel.DaYunCJDUIMainFromWebPart
COM.DaYun.MES.CJDBE.CJDLine	关重件绑定	danyun_cjdline	COM.DaYun.MES.DaYunCJDBE	CJDHead_CJDLine	DaYunCJDUIModel.DaYunCJDUIMainFromWebPart	COM.DaYun.MES.UI.DaYunCJDUI.WebPart	0	CAB27503-D7D5-4E05-84FB-A084202EE8C3		mes.cjd	66be41ba-d206-4bae-8f52-379345be90ee	DaYunCJDUIModel.DaYunCJDUIMainFromWebPart
COM.DaYun.MES.CJDBE.CarAssemblyCQRecord	车辆装配过程检验质量问题记录	dayun_carassemblycqrecord	COM.DaYun.MES.DaYunCJDBE	CJDHead_CarAssemblyCQRecord	DaYunCJDUIModel.DaYunCJDUIMainFromWebPart	COM.DaYun.MES.UI.DaYunCJDUI.WebPart	0	CAB27503-D7D5-4E05-84FB-A084202EE8C3		mes.cjd	66be41ba-d206-4bae-8f52-379345be90ee	DaYunCJDUIModel.DaYunCJDUIMainFromWebPart
                             */
                            /* // CJDHead_CarTestCQRecord	DaYunCJDUIModel.DaYunCJDUIMainFromWebPart	COM.DaYun.MES.UI.DaYunCJDUI.WebPart
                             * OnPush56
                            		private void OnPush56_Click_Extend(object sender, EventArgs e)
		{
			this.OnPush56_Click_DefaultImpl(sender, e);
			this.Push();
			this.SaveClick0_Click_Extend(sender, e);
		}
		private void Push()
		{
			if (this.Model.CJDHead.FocusedRecord != null && this.Model.CJDHead.FocusedRecord.Dtzyplan_PlanStatus.HasValue)
			{
				switch (this.Model.CJDHead.FocusedRecord.Dtzyplan_PlanStatus.Value)
				{
				case 1:
					this.Model.CJDHead.FocusedRecord.Status = new int?(3);
					break;
				case 3:
					this.Model.CJDHead.FocusedRecord.Status = new int?(6);
					break;
				case 4:
					this.Model.CJDHead.FocusedRecord.Status = new int?(5);
					break;
				case 6:
					this.Model.CJDHead.FocusedRecord.Status = new int?(4);
					break;
				}
			}
		}
                             */
                            if (entity.DocType != null
                                // 底盘完工报告（单据类型 = 总装完工报告）
                                && entity.DocType.Code == Const_MesDocTypeCode
                                // VIN码有值
                                && entity.DescFlexField.PubDescSeg12.IsNotNullOrWhiteSpace()
                                )
                            {
                                // 审核
                                if (entity.DocState == CompleteRptStateEnum.Approved && entity.OriginalData.DocState == CompleteRptStateEnum.Approving)
                                {
                                    CJDHead cjdHead = GetCJDHead(entity);

                                    if (cjdHead != null
                                        && cjdHead.CJDLine != null
                                        && cjdHead.CJDLine.Count > 0
                                        )
                                    {
                                        try
                                        {
                                            SI01ImplService service = new SI01ImplService();

                                            List<mesDataTmpDto> lstMesDTO = new List<mesDataTmpDto>();
                                            string strVin = entity.DescFlexField.PubDescSeg12;

                                            foreach (CJDLine cjdLine in cjdHead.CJDLine)
                                            {
                                                if (cjdLine.ItemMaster != null)
                                                {
                                                    // service.Url = PubHelper.GetAddress(service.Url);
                                                    mesDataTmpDto dto = new mesDataTmpDto();
                                                    // vin底盘号 ,bomdm 配件图号,   gysdm供应商代码  pjtm 配件条码   pch 批次号,
                                                    dto.vin = strVin;
                                                    // bomdm 配件图号
                                                    dto.bomdm = cjdLine.ItemMaster.Code;
                                                    // gysdm供应商代码
                                                    if (cjdLine.Supplyer != null)
                                                    {
                                                        dto.gysdm = cjdLine.Supplyer.Code;
                                                    }
                                                    // pjtm 配件条码 
                                                    dto.pjtm = cjdLine.SN;
                                                    // pch 批次号  采集点没有(从SN条码解析出来)
                                                    // *110377*f0701006*j2914n1h-010*
                                                    // *供应商*批次号*料号*
                                                    dto.pch = GetLotCode(cjdLine);

                                                    lstMesDTO.Add(dto);
                                                }
                                            }
                                            
                                            if (lstMesDTO.Count > 0)
                                            {
                                                mesDataTmpDto resultdto = service.Do(lstMesDTO.ToArray());
                                                if (resultdto != null && resultdto.flag == 0)
                                                {
                                                    throw new BusinessException(resultdto.errMsg);
                                                }
                                            }
                                        }
                                        catch (System.Exception e)
                                        {
                                            throw new BusinessException("调用DMS接口SI01[MES接口]错误：" + e.Message);
                                        }
                                    }
                                }
                            }



                        }
                    }
				}
			}
		}

        public static string GetLotCode(CJDLine cjdLine)
        {
            if (cjdLine != null
                && cjdLine.SN.IsNotNullOrWhiteSpace()
                )
            {
                // *110377*f0701006*j2914n1h-010*
                // *供应商*批次号*料号*
                string[] arr = cjdLine.SN.Split(new char[] { '*' }, StringSplitOptions.None);

                if (arr.Length >= 3)
                {
                    return arr[2];
                }
            }

            return string.Empty;
        }

        private CJDHead GetCJDHead(CompleteRpt entity)
        {
            // 全局段23 
            if (entity.DescFlexField.PrivateDescSeg23.IsNotNullOrWhiteSpace())
            {
                if (entity.DescFlexField.PrivateDescSeg23.Contains(","))
                {
                    throw new BusinessException(string.Format("完工报告[{0}]的来源采集点ID为多个[{1}]，无法给DMS发送条码数据，请检查！"
                        , entity.DocNo
                        , entity.DescFlexField.PrivateDescSeg23
                        ));
                }
                else
                {
                    CJDHead cjdHead = CJDHead.Finder.FindByID(entity.DescFlexField.PrivateDescSeg23);

                    return cjdHead;
                }
            }

            return null;
        }
	}
}
