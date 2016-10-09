using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UFSoft.UBF.Util.Log;
using UFIDA.U9.ISV.SM.Proxy;
using UFIDA.U9.ISV.SM;
using UFSoft.UBF.Transactions;
using UFIDA.U9.Base;

namespace UFIDA.U9.Cust.HBDY.API
{
    internal class HBHCommon
    {
        // 新能源组织
        /// <summary>
        /// 新能源组织
        /// </summary>
        public const string Const_OrgCode_Electric = "70";
        // 湖北大运汽车有限公司
        /// <summary>
        /// 湖北大运汽车有限公司
        /// </summary>
        public const string Const_OrgCode_Hubei = "20";
        // 成都大运十堰分公司
        /// <summary>
        /// 成都大运十堰分公司
        /// </summary>
        public const string Const_OrgCode_Chengdu = "10";

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
        /// 默认杂收单据类型编码    ZF03
        /// </summary>
        public const string DefaultMiscDocTypeCode = "ZF03";
        /// <summary>
        /// 默认杂收收益部门编码    "101404"
        /// </summary>
        public const string DefaultBenefitDeptCode = "101404";
        /// <summary>
        /// 默认分隔符(为空时赋值这个分隔符)
        /// </summary>
        public const string DefaultSplitFlag = "---------------------------------------------------------";

        // 默认出货业务员      "DMSTESTUSER"
        /// <summary>
        /// 默认出货业务员 
        /// </summary>
        public const string DefaultShipOperatorCode = "DMS";

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

        // 湖北大运,配件,价表编码
        /// <summary>
        /// 湖北大运,配件,价表编码
        /// </summary>
        public const string Const_HuBeiPartPriceListCode = "SPL2015070003";

        // 获得价表编码
        /// <summary>
        /// 获得价表编码
        /// </summary>
        /// <returns></returns>
        public static string GetPartPriceListCode()
        {
            if (Context.LoginOrg.Code == Const_OrgCode_Electric)
            {
                return Const_ElectricPartPriceListCode;
            }
            else if (Context.LoginOrg.Code == Const_OrgCode_Hubei)
            {
                return Const_HuBeiPartPriceListCode;
            }

            return string.Empty;
        }

        // 现金
        /// <summary>
        /// 现金
        /// </summary>
        public const string Const_ShipDocType_XJ = "CK-XJ"; 
        // 三包
        /// <summary>
        /// 三包
        /// </summary>
        public const string Const_ShipDocType_SB = "CK-SB";
        
        // 出货(储备订单)
        /// <summary>
        /// 出货(储备订单)
        /// </summary>
        public const string Const_ShipDocType_PH = "CK-PH";

        // 生产组织转DMS
        private static List<string> lstDMSShipDocType = new List<string>();
        /// <summary>
        /// 生产组织转DMS
        /// </summary>
        public static List<string> DMSShipDocType
        {
            get
            {
                if (lstDMSShipDocType.Count == 0)
                {
                    lstDMSShipDocType.Add(Const_ShipDocType_XJ);
                    lstDMSShipDocType.Add(Const_ShipDocType_XJ);
                }

                return lstDMSShipDocType;
            }
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="msg"></param>
        public static void LoggerError(string msg)
        {
            ILogger logger = LoggerManager.GetLogger("DMS接口日志(HBH)");
            logger.Error(msg, new object[0]);
        }




        public static void ApproveShipments(System.Collections.Generic.List<DocKeyDTOData> shipKeyList)
        {
            using (UBFTransactionScope aTransact = new UBFTransactionScope(TransactionOption.Required))
            {
                try
                {
                    AuditShipSVProxy approveproxy = new AuditShipSVProxy();
                    approveproxy.ShipKeys = shipKeyList;
                    approveproxy.Do();

                    aTransact.Commit();
                }
                catch (Exception ex)
                {
                    aTransact.Rollback();

                    throw ex;
                }
            }
        }
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
