using COM.DaYun.MFG.SingleWorkPlanBE;
using System;
using UFIDA.U9.Base;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI03;
using UFIDA.U9.SM.SO;
using UFSoft.UBF.Business;
using UFSoft.UBF.Eventing;
using UFSoft.UBF.PL;
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
	public class SingleWorkPlanCreated : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
					SingleWorkPlan Singleworkplan = key.GetEntity() as SingleWorkPlan;
					if (Context.LoginOrg.Code == "10")
					{
						bool flag = PubHelper.IsUsedDMSAPI();
						if (flag)
						{
							string dmssaleno = string.Empty;
							if (Singleworkplan.SrcDoc != null && Singleworkplan.SrcDoc.PLSKey != null && Singleworkplan.SrcDoc.PLS.ProjectKey != null)
							{
								SOLine soline = SOLine.Finder.Find(string.Format("SO.Org={0} and Project={1}", Context.LoginOrg.ID.ToString(), Singleworkplan.SrcDoc.PLS.ProjectKey.ID.ToString()), new OqlParam[0]);
								if (soline != null)
								{
									dmssaleno = soline.SO.DescFlexField.PubDescSeg5;
								}
							}
							if (!string.IsNullOrEmpty(dmssaleno))
							{
								try
								{
									SI03ImplService service = new SI03ImplService();
									// service.Url = PubHelper.GetAddress(service.Url);
									vehicleInfoDto dto = new vehicleInfoDto();
									if (Singleworkplan.SrcDoc != null && Singleworkplan.SrcDoc.PLSKey != null && Singleworkplan.SrcDoc.PLS.ProjectKey != null)
									{
										SOLine soline = SOLine.Finder.Find(string.Format("SO.Org={0} and Project={1}", Context.LoginOrg.ID.ToString(), Singleworkplan.SrcDoc.PLS.ProjectKey.ID.ToString()), new OqlParam[0]);
										if (soline != null)
										{
											dto.dmsSaleNo = soline.SO.DescFlexField.PubDescSeg5;
										}
									}
									dto.commissionNo = Singleworkplan.WorkCode;
									dto.vin = Singleworkplan.VINFinal;
									if (Singleworkplan.ItemMasterKey != null)
									{
										dto.erpMaterialCode = Singleworkplan.ItemMaster.Code;
									}
									dto.flowingCode = Singleworkplan.VIN;
									dto.oldVin = string.Empty;
									dto.nodeStatus = Singleworkplan.PlanStatus.Value.ToString();
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
