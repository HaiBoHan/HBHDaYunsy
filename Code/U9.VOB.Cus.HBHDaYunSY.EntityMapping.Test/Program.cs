using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBH.DoNet.DevPlatform.EntityMapping;

namespace U9.VOB.Cus.HBHDaYunSY.EntityMapping.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ReturnOrderDto rcv = new ReturnOrderDto();

            rcv.MiscRcvLines = new List<MiscRcvLine>();

            {
                MiscRcvLine line = new MiscRcvLine();
                line.DealerCode = "供应商编码";
                line.DmsSaleNo = "DMS订单号";
                line.DMSShipNo = "DMS发运单号";
                line.ErpMaterialCode = "ERP物料编码";
                line.LotCode = "批号";
                line.MaterialCode = "DMS料号";
                line.Money = 600;
                line.Number = 40;
                line.Price = 15;
                line.Warehouse = "仓库编码";

                rcv.MiscRcvLines.Add(line);
            }
            {
                MiscRcvLine line = new MiscRcvLine();
                line.DealerCode = "供应商编码2";
                line.DmsSaleNo = "DMS订单号";
                line.DMSShipNo = "DMS发运单号";
                line.ErpMaterialCode = "ERP物料编码2";
                line.LotCode = "批号2";
                line.MaterialCode = "DMS料号2";
                line.Money = 50;
                line.Number = 10;
                line.Price = 5;
                line.Warehouse = "仓库编码";

                rcv.MiscRcvLines.Add(line);
            }

            string str = EntitySerialization.EntitySerial(rcv);
            EntitySerialization.OutPutToFile(rcv.GetType().FullName + "-" + DateTime.Today.ToString("yyyyMMdd"), str);

            string strJson = EntitySerialization.EntitySerialJson(rcv);
            EntitySerialization.OutPutToFile(rcv.GetType().FullName + "-Json-" + DateTime.Today.ToString("yyyyMMdd"), strJson);

            Console.ReadLine();


        }
    }
}
