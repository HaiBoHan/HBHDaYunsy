using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBH.DoNet.DevPlatform.U9Mapping;
using HBH.DoNet.DevPlatform.EntityMapping;
using UFIDA.U9.Cust.HBDY.API;

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

    }
}
