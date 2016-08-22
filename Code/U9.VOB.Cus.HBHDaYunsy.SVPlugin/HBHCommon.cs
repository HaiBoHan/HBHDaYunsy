using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBH.DoNet.DevPlatform.U9Mapping;
using HBH.DoNet.DevPlatform.EntityMapping;

namespace U9.VOB.Cus.HBHDaYunsy.SVPlugin
{
    internal class HBHCommon
    {
        public const bool IsLog = true;

        public static long HBHCommonSVBefore(object bpObj)
        {
            long svID = -1;
            if (IsLog)
            {
                svID = ProxyLogger.CreateTransferSV(bpObj.GetType().FullName
                    //, EntitySerialization.EntitySerial(bpObj)
                    , Newtonsoft.Json.JsonConvert.SerializeObject(bpObj)
                    , bpObj.GetType().FullName, string.Empty);
            }

            // 重置上下文
            U9Helper.SetDefaultContext();
            return svID;
        }

        public static void HBHCommonSVAfter(long svID, object result, bool isSucess, string message, string resultValue)
        {
            if (svID > 0)
            {
                if (result != null
                    )
                {
                    //string resultXml = EntitySerialization.EntitySerial(result);
                    string resultXml = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                    ProxyLogger.UpdateTransferSV(svID, resultXml, isSucess, message, resultValue, string.Empty);
                }
            }
        }

        #region 接口常量

        public readonly static List<string> ProjectSendOrgCode = new List<string>() { "10", "20" };

        // 默认币种编码
        /// <summary>
        /// 默认币种编码
        /// </summary>
        public const string DefaultCurrencyCode = "C001";

        // 默认收款条件编码
        /// <summary>
        /// 默认收款条件编码
        /// </summary>
        public const string DefaultRecTermCode = "01";

        // 默认出货原则编码
        /// <summary>
        /// 默认出货原则编码
        /// </summary>
        public const string DefaultShipRuleCode = "C001";

        // 默认立账条件编码
        /// <summary>
        /// 默认立账条件编码
        /// </summary>
        public const string DefaultConfirmTermCode = "01";

        // 默认成交方式编码
        /// <summary>
        /// 默认成交方式编码
        /// </summary>
        public const int DefaultBargainMode = 0;

        #endregion

    }
}
