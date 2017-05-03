using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBH.DoNet.DevPlatform.EntityMapping;

namespace U9.VOB.Cus.HBHDaYunSY.EntityMapping
{
    public class ReturnOrderDtlDto 
    {
        // 索赔单号
        /// <summary>
        /// 索赔单号
        /// </summary>
        public decimal ClaimNo { get; set; }

        //// ERP物料编号
        ///// <summary>
        ///// ERP物料编号
        ///// </summary>
        //public string ErpMaterialCode { get; set; }

        // DMS物料编号
        /// <summary>
        /// DMS物料编号
        /// </summary>
        public string PartCode { get; set; }

        // DMS订单编号
        /// <summary>
        /// DMS订单编号
        /// </summary>
        public string OrderNo { get; set; }

        // 旧供应商编号
        /// <summary>
        /// 旧供应商编号
        /// </summary>
        public string OldSuptCode { get; set; }

        // 责任供应商
        /// <summary>
        /// 责任供应商
        /// </summary>
        public string DutySuptCode { get; set; }

        // 车辆故障描述
        /// <summary>
        /// 车辆故障描述
        /// </summary>
        public string PartTroubleDesc { get; set; }

        // 回运数量
        /// <summary>
        /// 回运数量
        /// </summary>
        public decimal ReturnCount { get; set; }

        // 实际数量
        /// <summary>
        /// 实际数量
        /// </summary>
        public decimal ActualCount { get; set; }

        // 审批入库数量
        /// <summary>
        /// 审批入库数量
        /// </summary>
        public decimal AlreadyIn { get; set; }

        // 备注
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        // 价格
        /// <summary>
        /// 价格
        /// </summary>
        public decimal PartFee { get; set; }

        //// 存储地点
        ///// <summary>
        ///// 存储地点
        ///// </summary>
        //public string Warehouse { get; set; }

        //// 价格
        ///// <summary>
        ///// 价格
        ///// </summary>
        //public decimal Price { get; set; }

        //// 金额
        ///// <summary>
        ///// 金额
        ///// </summary>
        //public decimal Money { get; set; }


    }
}
