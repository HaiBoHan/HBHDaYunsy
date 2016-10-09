using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace U9.VOB.Cus.HBHDaYunsy.UIPlugIn
{
    public class UIHelper
    {
    }

    internal enum DaYun2DMSTransferTypeEnum
    {
        // 库存
        /// <summary>
        /// 库存
        /// </summary>
        Whqoh = 0,
        // 配件主数据
        /// <summary>
        /// 配件主数据
        /// </summary>
        SupplySource = 1,
        // 供应商
        /// <summary>
        /// 供应商
        /// </summary>
        Supplier = 2,
        // 客户(经销商)
        /// <summary>
        /// 客户(经销商)
        /// </summary>
        Customer = 3,
        // 价表
        /// <summary>
        /// 价表
        /// </summary>
        PriceList = 4,
    }
}
