using System;
using UFIDA.U9.Base;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI09;
using UFIDA.U9.InvDoc.TransferOut;
using UFSoft.UBF.Business;
using UFSoft.UBF.Eventing;
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
	internal class TransferOutPlugInUpdated : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
					TransferOut Transferout = key.GetEntity() as TransferOut;
					bool flag = PubHelper.IsUsedDMSAPI();
					if (flag)
					{
						if (PubHelper.SaleOrg2DMS.Contains(Context.LoginOrg.Code))
						{
							if (Transferout.Status == Status.Approved && Transferout.OriginalData.Status == Status.Approving)
							{
								try
								{
									string errormessage = string.Empty;
									foreach (TransOutLine line in Transferout.TransOutLines)
									{
										if (!string.IsNullOrEmpty(line.DescFlexSegments.PubDescSeg12))
										{
											SI09ImplService service = new SI09ImplService();
											// service.Url = PubHelper.GetAddress(service.Url);
											vehicleChangeInfoDto d = service.Do(new vehicleChangeInfoDto
											{
												vin = line.DescFlexSegments.PubDescSeg12,
												docStatus = 5
											});
											if (d != null && d.flag == 0)
											{
												errormessage += string.Format("Vin:{0},错误信息：{1}  ", line.DescFlexSegments.PubDescSeg12, d.errMsg);
											}
										}
									}
									if (!string.IsNullOrEmpty(errormessage))
									{
										throw new System.ApplicationException(errormessage);
									}
								}
								catch (System.Exception e)
								{
									throw new System.ApplicationException("调用DMS接口错误：" + e.Message);
								}
							}
						}
						if (Context.LoginOrg.Code == "10")
						{
							if (Transferout.Status == Status.Approved && Transferout.OriginalData.Status == Status.Approving)
							{
								try
								{
									string errormessage = string.Empty;
									foreach (TransOutLine line in Transferout.TransOutLines)
									{
										if (!string.IsNullOrEmpty(line.DescFlexSegments.PubDescSeg12))
										{
											if (line.TransOutOrg.Code == "10" && line.TransOutSubLines[0].TransInOrg.Code == "20")
											{
												SI09ImplService service = new SI09ImplService();
												// service.Url = PubHelper.GetAddress(service.Url);
												vehicleChangeInfoDto d = service.Do(new vehicleChangeInfoDto
												{
													vin = line.DescFlexSegments.PubDescSeg12,
													docStatus = 7
												});
												if (d != null && d.flag == 0)
												{
													errormessage += string.Format("Vin:{0},错误信息：{1}  ", line.DescFlexSegments.PubDescSeg12, d.errMsg);
												}
											}
										}
									}
									if (!string.IsNullOrEmpty(errormessage))
									{
										throw new System.ApplicationException(errormessage);
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
