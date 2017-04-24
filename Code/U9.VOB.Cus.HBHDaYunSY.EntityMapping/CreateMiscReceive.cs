using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBH.DoNet.DevPlatform.EntityMapping;

namespace U9.VOB.Cus.HBHDaYunSY.EntityMapping
{
    public partial class CreateMiscReceive : BaseEntity
    {



        // 行
        /// <summary>
        /// 行
        /// </summary>
        public List<MiscRcvLine> MiscRcvLines { get; set; }
    }
}
