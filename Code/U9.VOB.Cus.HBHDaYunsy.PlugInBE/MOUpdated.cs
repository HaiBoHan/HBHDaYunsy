using System;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI09;
using UFIDA.U9.MO.Enums;
using UFIDA.U9.MO.MO;
using UFSoft.UBF.Business;
using UFSoft.UBF.Eventing;
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
	public class MOUpdated : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
					MO mo = key.GetEntity() as MO;
					bool flag = PubHelper.IsUsedDMSAPI();
					if (flag)
					{
						if (mo.MODocType != null && mo.MODocType.Code == "MO02" && mo.OriginalData.DocState == MOStateEnum.Approved && mo.DocState == MOStateEnum.Released)
						{
							try
							{
								SI09ImplService service = new SI09ImplService();
								// service.Url = PubHelper.GetAddress(service.Url);
                                //vehicleChangeInfoDto d = service.Do(new vehicleChangeInfoDto
                                //{
                                //    vin = mo.DescFlexField.PubDescSeg12,
                                //    docStatus = 5
                                //});

                                vehicleChangeInfoDto vehicleDTO = new vehicleChangeInfoDto();
                                //单台作业计划状态;  等待上线0,上线1,下线滞留2,下线调试3,最终检验4,总装入库5,调试检验6, 车辆整改7
                                vehicleDTO.docStatus = 5;
                                vehicleDTO.vin = mo.DescFlexField.PubDescSeg12;

                                vehicleChangeInfoDto d = service.Do(vehicleDTO);
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
