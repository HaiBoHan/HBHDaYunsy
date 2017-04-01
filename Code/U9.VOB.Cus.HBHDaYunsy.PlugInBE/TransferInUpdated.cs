using System;
using System.Collections.Generic;
using UFIDA.U9.Base;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI11;
using UFIDA.U9.InvDoc.TransferIn;
using UFSoft.UBF.Business;
using UFSoft.UBF.Eventing;
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
	public class TransferInUpdated : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
					TransferIn transferin = key.GetEntity() as TransferIn;
					if (PubHelper.SaleOrg2DMS.Contains(Context.LoginOrg.Code))
					{
						bool flag = PubHelper.IsUsedDMSAPI();
						if (flag)
						{
							if (transferin.DocType.Code == "MoveWH")
							{
								if (transferin.Status == TransInStatus.Approved && transferin.OriginalData.Status == TransInStatus.Approving)
								{
									SI11ImplService service = new SI11ImplService();
									// service.Url = PubHelper.GetAddress(service.Url);
									System.Collections.Generic.List<vehicleMoveInfoDto> list = new System.Collections.Generic.List<vehicleMoveInfoDto>();
									foreach (TransInLine line in transferin.TransInLines)
									{
										foreach (TransInSubLine subline in line.TransInSubLines)
										{
											vehicleMoveInfoDto dto = new vehicleMoveInfoDto();
											dto.flowCode = ((subline.TransInLine.DescFlexSegments.PubDescSeg12.Length >= 8) ? subline.TransInLine.DescFlexSegments.PubDescSeg12.Substring(subline.TransInLine.DescFlexSegments.PubDescSeg12.Length - 8, 8) : subline.TransInLine.DescFlexSegments.PubDescSeg12);
											if (subline.TransInLine.TransInWhKey != null)
											{
												dto.toWarehose = subline.TransInLine.TransInWh.Code;
											}
											if (subline.TransOutWhKey != null)
											{
												dto.fromWarehose = subline.TransOutWh.Code;
											}
											list.Add(dto);
										}
									}
									try
									{
										vehicleMoveInfoDto dto2 = service.Do(list.ToArray());
										if (dto2 != null && dto2.flag == 0)
										{
											throw new BusinessException(dto2.errMsg);
										}
									}
									catch (System.Exception e)
									{
										throw new BusinessException("调用DMS接口错误：" + e.Message);
									}
								}
								else if (transferin.Status == TransInStatus.Opening && transferin.OriginalData.Status == TransInStatus.Approved)
								{
									SI11ImplService service = new SI11ImplService();
									// service.Url = PubHelper.GetAddress(service.Url);
									System.Collections.Generic.List<vehicleMoveInfoDto> list = new System.Collections.Generic.List<vehicleMoveInfoDto>();
									foreach (TransInLine line in transferin.TransInLines)
									{
										foreach (TransInSubLine subline in line.TransInSubLines)
										{
											vehicleMoveInfoDto dto = new vehicleMoveInfoDto();
											dto.flowCode = ((subline.TransInLine.DescFlexSegments.PubDescSeg12.Length >= 8) ? subline.TransInLine.DescFlexSegments.PubDescSeg12.Substring(subline.TransInLine.DescFlexSegments.PubDescSeg12.Length - 8, 8) : subline.TransInLine.DescFlexSegments.PubDescSeg12);
											if (subline.TransInLine.TransInWhKey != null)
											{
												dto.fromWarehose = subline.TransInLine.TransInWh.Code;
											}
											if (subline.TransOutWhKey != null)
											{
												dto.toWarehose = subline.TransOutWh.Code;
											}
											list.Add(dto);
										}
									}
									try
									{
										vehicleMoveInfoDto dto2 = service.Do(list.ToArray());
										if (dto2 != null && dto2.flag == 0)
										{
											throw new BusinessException(dto2.errMsg);
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
