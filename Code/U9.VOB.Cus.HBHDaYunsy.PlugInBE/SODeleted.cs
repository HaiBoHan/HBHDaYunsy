using System;
using System.Collections.Generic;
using UFIDA.U9.Base;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI02;
using UFIDA.U9.SM.SO;
using UFSoft.UBF.Business;
using UFSoft.UBF.Eventing;
namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
	public class SODeleted : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
					SO so = key.GetEntity() as SO;
					if (PubHelper.SaleOrg2DMS.Contains(Context.LoginOrg.Code))
					{
						bool flag = PubHelper.IsUsedDMSAPI();
						if (flag)
						{
							if (!string.IsNullOrEmpty(so.DescFlexField.PubDescSeg5))
							{
								try
								{
									System.Collections.Generic.List<orderInfoDto> list = new System.Collections.Generic.List<orderInfoDto>();
									SI02ImplService service = new SI02ImplService();
									// service.Url = PubHelper.GetAddress(service.Url);
									list.Add(new orderInfoDto
									{
										docStatus = "2",
										dmsSaleNo = so.DescFlexField.PubDescSeg5
									});
									service.Do(list.ToArray());
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
