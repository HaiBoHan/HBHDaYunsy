using System;
using System.Collections.Generic;
using UFIDA.U9.Base;
using UFIDA.U9.CBO.SCM.Supplier;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_PI08;
using UFSoft.UBF.Business;
using UFSoft.UBF.Eventing;
using HBH.DoNet.DevPlatform.EntityMapping;

namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
	public class SupplierUpdated : IEventSubscriber
	{
		public void Notify(params object[] args)
		{
			if (args != null && args.Length != 0 && args[0] is EntityEvent)
			{
				BusinessEntity.EntityKey key = ((EntityEvent)args[0]).EntityKey;
				if (!(key == null))
				{
					Supplier supplier = key.GetEntity() as Supplier;
                    if (PubHelper.IsOrg_Supplier2DMS(supplier))
					{
						bool flag = PubHelper.IsUsedDMSAPI();
                        if (flag)
                        {
                            if (
                                PubHelper.IsUpdateDMS(supplier)
                                )
                            {
                                try
                                {
                                    PI08ImplService service = new PI08ImplService();
                                    service.Url = PubHelper.GetAddress(service.Url);
                                    System.Collections.Generic.List<supplierDto> list = new System.Collections.Generic.List<supplierDto>();

                                    supplierDto dto = GetUpdateDMSDTO(supplier);

                                    list.Add(dto);
                                    supplierDto s = service.Do(list.ToArray());
                                    if (s != null && s.flag == 0)
                                    {
                                        throw new BusinessException(s.errMsg);
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

        public static supplierDto GetUpdateDMSDTO(Supplier supplier)
        {
            supplierDto dto = new supplierDto();
            dto.suptCode = supplier.Code;
            dto.suptName = supplier.Name;
            dto.supShortName = supplier.ShortName;
            if (supplier.ContactObjectKey != null)
            {
                if (supplier.ContactObject.PersonName != null)
                {
                    dto.linkMan = supplier.ContactObject.PersonName.DisplayName;
                }
                dto.phone = supplier.ContactObject.DefaultPhoneNum;
                dto.fax = supplier.ContactObject.DefaultFaxNum;
                if (supplier.ContactObject.DefaultLocation != null && supplier.ContactObject.DefaultLocation.PostalCode != null)
                {
                    dto.zipCode = supplier.ContactObject.DefaultLocation.PostalCode.PostalCode;
                }
                if (supplier.ContactObject.DefaultLocation != null)
                {
                    dto.address = supplier.ContactObject.DefaultLocation.Address1;
                }
            }
            dto.actionType = 2;
            // status  100201 有效 100202 无效
            dto.status = (supplier.Effective != null && supplier.Effective.IsEffective) ? "100201" : "100202";
            return dto;
        }
	}
}
