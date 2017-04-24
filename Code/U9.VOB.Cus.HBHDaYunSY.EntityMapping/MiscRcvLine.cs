using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBH.DoNet.DevPlatform.EntityMapping;

namespace U9.VOB.Cus.HBHDaYunSY.EntityMapping
{
    public class MiscRcvLine 
    {
        // 数量
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Number { get; set; }

        // ERP物料编号
        /// <summary>
        /// ERP物料编号
        /// </summary>
        public string ErpMaterialCode { get; set; }

        // DMS物料编号
        /// <summary>
        /// DMS物料编号
        /// </summary>
        public string MaterialCode { get; set; }

        // DMS销售订单编号
        /// <summary>
        /// DMS销售订单编号
        /// </summary>
        public string DmsSaleNo { get; set; }

        // DMS销售出库单号
        /// <summary>
        /// DMS销售出库单号
        /// </summary>
        public string DMSShipNo { get; set; }

        // 经销商代码
        /// <summary>
        /// 经销商代码
        /// </summary>
        public string DealerCode { get; set; }

        // 批号
        /// <summary>
        /// 批号
        /// </summary>
        public string LotCode { get; set; }

        // 存储地点
        /// <summary>
        /// 存储地点
        /// </summary>
        public string Warehouse { get; set; }

        // 价格
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        // 金额
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }


    }
}
