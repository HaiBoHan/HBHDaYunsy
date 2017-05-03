using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBH.DoNet.DevPlatform.EntityMapping;

namespace U9.VOB.Cus.HBHDaYunSY.EntityMapping
{
    public partial class ReturnOrderDto : BaseEntity
    {

        // 回运单号
        /// <summary>
        /// 回运单号
        /// </summary>
        public string ReturnNo { get; set; }

        // 经销商编号
        /// <summary>
        /// 经销商编号
        /// </summary>
        public string DealerCode { get; set; }

        // 旧件产生日期
        /// <summary>
        /// 旧件产生日期
        /// </summary>
        public string ReturnProdate { get; set; }

        // 回运日期
        /// <summary>
        /// 回运日期
        /// </summary>
        public string ReturnDate { get; set; }

        // 备注
        /// <summary>
        /// 备注
        /// </summary>
        public string ReMark { get; set; }
        


        // 行
        /// <summary>
        /// 行
        /// </summary>
        public ReturnOrderDtlDto[] ReturnOrderDtlDto { get; set; }
    }
}
