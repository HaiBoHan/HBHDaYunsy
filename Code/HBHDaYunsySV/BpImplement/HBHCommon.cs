using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UFSoft.UBF.Util.Log;

namespace UFIDA.U9.Cust.HBDY.API
{
    internal class HBHCommon
    {
        /// <summary>
        /// 默认币种编码
        /// </summary>
        public const string DefaultCurrencyCode = "C001";
        /// <summary>
        /// 默认收款条件编码
        /// </summary>
        public const string DefaultRecTermCode = "01";
        /// <summary>
        /// 默认出货原则编码
        /// </summary>
        public const string DefaultShipRuleCode = "C001";
        /// <summary>
        /// 默认立账条件编码
        /// </summary>
        public const string DefaultConfirmTermCode = "01";
        /// <summary>
        /// 默认成交方式编码
        /// </summary>
        public const int DefaultBargainMode = 0;
        /// <summary>
        /// 默认分隔符(为空时赋值这个分隔符)
        /// </summary>
        public const string DefaultSplitFlag = "---------------------------------------------------------";


        public const string DefaultShipOperatorCode = "DMSTESTUSER";

        public static readonly List<string> ProjectSendOrgCode = new List<string>
		{
			"10",
			"20"
		};

        // 新能源,配件,价表编码
        /// <summary>
        /// 新能源,配件,价表编码
        /// </summary>
        public const string Const_ElectricPartPriceListCode = "SPL2016050007";


        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="msg"></param>
        public static void LoggerError(string msg)
        {
            ILogger logger = LoggerManager.GetLogger("DMS接口日志(HBH)");
            logger.Error(msg, new object[0]);
        }

        public const string Const_ElectricOrgCode = "70";
    }
}
