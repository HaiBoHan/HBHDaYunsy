using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBH.DoNet.DevPlatform.EntityMapping;
using UFIDA.U9.ISV.MiscRcvISV.Proxy;
using UFIDA.U9.ISV.MiscRcvISV;
using UFIDA.U9.CBO.Pub.Controller;

namespace U9.VOB.Cus.HBHDaYunSY.EntityMapping
{
    public partial class CreateMiscReceive : BaseEntity
    {
        public const string Const_MiscRcvDocTypeCode = "01";

        // CommonCreateMiscRcv

        public override HBH.DoNet.DevPlatform.EntityMapping.EntityResult Do()
        {
            //return base.Do();
            EntityResult result = new EntityResult();

            //result.Sucessfull = true;
            //result.Message = "调用【出库单】服务成功，但服务未实现!";
            //return result;

            string className = this.GetType().FullName;

            // BPM用单号做唯一键，不用ID
            // 我们实体的ID，一般要求传入 第三方系统ID，如果第三方系统ID不是long(可能为字符串、或GUID)，那么要求第三方系统，此ID强制赋值 为 1 ;
            //long id = this.ID;
            //if (id < 0)
            //{
            //    result.Sucessfull = false;
            //    result.Message = string.Format("[{0}]执行失败,ID[{1}]不可转化为长整数或小于0 !"
            //        , className
            //        , this.ID
            //        );
            //    return result;
            //}

            // 删除数量为空的行
            if (this.MiscRcvLines != null
                && this.MiscRcvLines.Count > 0
                )
            {
                for (int i = this.MiscRcvLines.Count - 1; i >= 0; i--)
                {
                    MiscRcvLine line = this.MiscRcvLines[i];

                    if (line != null)
                    {
                        bool isDel = false;

                        {
                            decimal qty = PubClass.GetDecimal(line.Number);

                            if (qty == 0)
                            {
                                isDel = true;
                            }
                        }

                        if (isDel)
                        {
                            this.MiscRcvLines.RemoveAt(i);
                            continue;
                        }
                    }
                }
            }

            //result.Sucessfull = true;
            //result.Message = string.Format("调用U9服务[{0}]成功，不过服务未实现!"
            //    , className
            //    );

            CommonCreateMiscRcvProxy proxy = new CommonCreateMiscRcvProxy();
            proxy.MiscRcvDTOList = new List<UFIDA.U9.ISV.MiscRcvISV.IC_MiscRcvDTOData>();

            if(this.MiscRcvLines != null
                && this.MiscRcvLines.Count > 0
                )
            {
                MiscRcvLine firstLine = this.MiscRcvLines[0];

                IC_MiscRcvDTOData miscHead = new IC_MiscRcvDTOData();

                miscHead.MiscRcvDocType = new UFIDA.U9.CBO.Pub.Controller.CommonArchiveDataDTOData();
                miscHead.MiscRcvDocType.Code = Const_MiscRcvDocTypeCode;

                miscHead.BusinessDate = DateTime.Today;

                if (miscHead.DescFlexField == null)
                {
                    miscHead.DescFlexField = new UFIDA.U9.Base.FlexField.DescFlexField.DescFlexSegmentsData();
                }
                miscHead.DescFlexField.PrivateDescSeg1 = firstLine.DmsSaleNo;
                miscHead.DescFlexField.PrivateDescSeg2 = firstLine.DMSShipNo;

                miscHead.MiscRcvTransLs = new List<IC_MiscRcvTransLsDTOData>();
                foreach (MiscRcvLine lineDTO in this.MiscRcvLines)
                {
                    IC_MiscRcvTransLsDTOData miscLine = new IC_MiscRcvTransLsDTOData();

                    miscLine.ItemInfo = new UFIDA.U9.CBO.SCM.Item.ItemInfoData();
                    miscLine.ItemInfo.ItemCode = lineDTO.ErpMaterialCode;

                    miscLine.StoreUOMQty = lineDTO.Number;
                    miscLine.CostPrice = lineDTO.Price;
                    miscLine.CostMny = lineDTO.Money;
                    
                    miscLine.Wh = new UFIDA.U9.CBO.Pub.Controller.CommonArchiveDataDTOData();
                    miscLine.Wh.Code = lineDTO.Warehouse;

                    miscLine.LotInfo = new UFIDA.U9.CBO.SCM.PropertyTypes.LotInfoData();
                    miscLine.LotInfo.LotCode = lineDTO.LotCode;

                    miscLine.SupplierInfo = new UFIDA.U9.CBO.SCM.Supplier.SupplierMISCInfoData();
                    miscLine.SupplierInfo.Code = lineDTO.DealerCode;


                    miscHead.MiscRcvTransLs.Add(miscLine);
                }

                proxy.MiscRcvDTOList.Add(miscHead);
            }

            if (proxy.MiscRcvDTOList.Count > 0)
            {
                try
                {
                    List<CommonArchiveDataDTOData> lstResult = proxy.Do();

                    if (lstResult != null
                        && lstResult.Count > 0
                        && lstResult[0] != null
                        )
                    {
                        result.Sucessfull = true;
                        result.StringValue = lstResult[0].Code;
                    }
                    else
                    {
                        result.Sucessfull = false;
                        result.Message = "U9服务执行异常，无返回值!";
                    }
                }
                catch (Exception ex)
                {
                    result.Sucessfull = false;
                    result.Message = ex.Message;
                    result.Trace = ex.StackTrace;
                }
            }
            else
            {
                result.Sucessfull = false;
                result.Message = string.Format("收货有效行为空!");
            }

            return result;
        }
    }
}
