using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBH.DoNet.DevPlatform.EntityMapping;
using UFIDA.U9.ISV.MiscRcvISV.Proxy;
using UFIDA.U9.ISV.MiscRcvISV;
using UFIDA.U9.CBO.Pub.Controller;
using UFIDA.U9.CBO.SCM.Item;
using UFSoft.UBF.PL;
using UFIDA.U9.Base;
using UFIDA.U9.CBO.SCM.Warehouse;

namespace U9.VOB.Cus.HBHDaYunSY.EntityMapping
{
    public partial class ReturnOrderDto : BaseEntity
    {
        // 单据类型: 三包旧件入库
        //public const string Const_MiscRcvDocTypeCode = "MiscRcv001";
        public const string Const_MiscRcvDocTypeCode = "ZS-SB";
        // 默认旧件库
        public const string Const_MiscRcvWhCode = "SHJJ";

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
            bool isAllLineZero = true;
            if (this.ReturnOrderDtlDto != null
                && this.ReturnOrderDtlDto.Length > 0
                )
            {
                for (int i = this.ReturnOrderDtlDto.Length - 1; i >= 0; i--)
                {
                    ReturnOrderDtlDto line = this.ReturnOrderDtlDto[i];

                    if (line != null)
                    {
                        //bool isDel = false;

                        {
                            decimal qty = PubClass.GetDecimal(line.AlreadyIn);

                            //if (qty == 0)
                            //{
                            //    isDel = true;
                            //}
                            if (qty > 0)
                            {
                                isAllLineZero = false;
                                break;
                            }
                        }

                        //if (isDel)
                        //{
                        //    this.ReturnOrderDtlDto.RemoveAt(i);
                        //    continue;
                        //}
                    }
                }
            }

            if (isAllLineZero)
            {
                result.Sucessfull = false;
                result.Message = string.Format("回运单[{0}]没有行、或者行数量全部为0，不可生成ERP单据！"
                    , this.ReturnNo
                    );

                return result;
            }

            //result.Sucessfull = true;
            //result.Message = string.Format("调用U9服务[{0}]成功，不过服务未实现!"
            //    , className
            //    );

            CommonCreateMiscRcvProxy proxy = new CommonCreateMiscRcvProxy();
            proxy.MiscRcvDTOList = new List<UFIDA.U9.ISV.MiscRcvISV.IC_MiscRcvDTOData>();

            if(this.ReturnOrderDtlDto != null
                && this.ReturnOrderDtlDto.Length > 0
                )
            {
                ReturnOrderDtlDto firstLine = this.ReturnOrderDtlDto[0];

                IC_MiscRcvDTOData miscHead = new IC_MiscRcvDTOData();

                miscHead.MiscRcvDocType = new UFIDA.U9.CBO.Pub.Controller.CommonArchiveDataDTOData();
                miscHead.MiscRcvDocType.Code = Const_MiscRcvDocTypeCode;

                //miscHead.BusinessDate = DateTime.Today;
                miscHead.BusinessDate = this.ReturnDate.GetDateTime(DateTime.Today);
                miscHead.Memo = this.ReMark;

                if (miscHead.DescFlexField == null)
                {
                    miscHead.DescFlexField = new UFIDA.U9.Base.FlexField.DescFlexField.DescFlexSegmentsData();
                }
                //miscHead.DescFlexField.PrivateDescSeg1 = firstLine.DmsSaleNo;
                //miscHead.DescFlexField.PrivateDescSeg2 = firstLine.DMSShipNo;
                miscHead.DescFlexField.PrivateDescSeg1 = this.ReturnNo;

                int lineNo = 0;
                miscHead.MiscRcvTransLs = new List<IC_MiscRcvTransLsDTOData>();
                foreach (ReturnOrderDtlDto lineDTO in this.ReturnOrderDtlDto)
                {
                    IC_MiscRcvTransLsDTOData miscLine = new IC_MiscRcvTransLsDTOData();

                    lineNo += 10;
                    miscLine.DocLineNo = lineNo;
                    miscLine.ItemInfo = new UFIDA.U9.CBO.SCM.Item.ItemInfoData();
                    //miscLine.ItemInfo.ItemCode = lineDTO.ErpMaterialCode;
                    miscLine.ItemInfo.ItemCode = lineDTO.PartCode;
                    //ItemMaster item = ItemMaster.Finder.Find("Org=@Org and Code=@Code"
                    //    , new OqlParam(Context.LoginOrg.ID)
                    //    , new OqlParam(lineDTO.PartCode)
                    //    );
                    //if (item != null)
                    //{
                    //    miscLine.ItemInfo.ItemID = item.ID;
                    //}

                    //miscLine.StoreUOMQty = lineDTO.AlreadyIn;
                    miscLine.StoreUOMQty = lineDTO.AlreadyIn;
                    //miscLine.CostUOMQty = miscLine.StoreUOMQty;
                    miscLine.CostPrice = lineDTO.PartFee;
                    miscLine.CostMny = miscLine.CostPrice * miscLine.StoreUOMQty;
                    if (miscLine.CostPrice == 0)
                    {
                        miscLine.IsZeroCost = true;
                    }
                    miscLine.Wh = new UFIDA.U9.CBO.Pub.Controller.CommonArchiveDataDTOData();
                    //miscLine.Wh.Code = lineDTO.Warehouse;
                    miscLine.Wh.Code = Const_MiscRcvWhCode;
                    //Warehouse wh = Warehouse.Finder.Find("Org=@Org and Code=@Code"
                    //    , new OqlParam(Context.LoginOrg.ID)
                    //    , new OqlParam(Const_MiscRcvWhCode)
                    //    );
                    //if (wh != null)
                    //{
                    //    miscLine.Wh.ID = wh.ID;
                    //}

                    //miscLine.LotInfo = new UFIDA.U9.CBO.SCM.PropertyTypes.LotInfoData();
                    //miscLine.LotInfo.LotCode = lineDTO.LotCode;

                    miscLine.SupplierInfo = new UFIDA.U9.CBO.SCM.Supplier.SupplierMISCInfoData();
                    //miscLine.SupplierInfo.Code = lineDTO.DealerCode;
                    miscLine.SupplierInfo.Code = this.DealerCode;


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
