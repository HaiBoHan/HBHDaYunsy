﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI04;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_PI06;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_PI07;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_PI08;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI02;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI03;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI05;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI08;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI09;
using U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI11;
using HBH.DoNet.DevPlatform.U9Mapping;
using System.ServiceModel;
using UFIDA.U9.Base;
using HBH.DoNet.DevPlatform.EntityMapping;
using HBH.DoNet.DevPlatform.ErpSvProxy;
using UFIDA.U9.CBO.SCM.Supplier;
using UFIDA.U9.SPR.SalePriceList;

namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE
{
    public static class PubExtend
    {
        public const bool IsLog = true;
        public const string Const_ResultNullMessage = "已发送DMS，但DMS返回结果为空!";

        // 配件主数据接口
        /// <summary>
        /// 配件主数据接口
        /// </summary>
        /// <param name="service"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static partBaseDto Do(this PI06ImplService service, partBaseDto[] param)
        {
            service.Url = PubHelper.GetAddress(service.Url);

            string entityName = "配件主数据接口";
            long svID = -1;
            if (IsLog)
            {
                svID = ProxyLogger.CreateTransferSV(entityName
                    //, EntitySerialization.EntitySerial(bpObj)
                    , Newtonsoft.Json.JsonConvert.SerializeObject(param)
                    , service.GetType().FullName,Newtonsoft.Json.JsonConvert.SerializeObject(service));
            }

            try
            {
                var result = service.receive(param);

                if (svID > 0)
                {
                    if (result != null
                        )
                    {
                        //string resultXml = EntitySerialization.EntitySerial(result);
                        string resultXml = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                        ProxyLogger.UpdateTransferSV(svID, resultXml, result.flag == 1, result.errMsg, string.Empty, string.Empty);
                    }
                    else
                    {
                        ProxyLogger.UpdateTransferSV(svID, string.Empty, false, Const_ResultNullMessage, string.Empty, string.Empty);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                if (svID > 0)
                {
                    ProxyLogger.UpdateTransferSV(svID, string.Empty, false, ex.Message, string.Empty, ex.StackTrace);
                }

                throw ex;
            }

            return null;
        }

        #region 配件主数据(异步)

        public static DMSAsync_PI06.PI06Client GetPI06Async()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferPoolSize = int.MaxValue;
            //  http://10.3.11.227:9081/dms/ws/PI07
            //  http://10.3.21.115:8080/hbfcdms/ws/PI07
            System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(new Uri("http://ip/dms/ws/PI07")); ;

            DMSAsync_PI06.PI06Client service = new DMSAsync_PI06.PI06Client(binding, remoteAddress);

            return service;
        }

        // 配件主数据接口(异步)
        /// <summary>
        /// 配件主数据接口(异步)
        /// </summary>
        /// <param name="service"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DMSAsync_PI06.partBaseDto Do(this DMSAsync_PI06.PI06Client service, DMSAsync_PI06.partBaseDto[] param)
        {
            service.Endpoint.Address = new System.ServiceModel.EndpointAddress(PubHelper.GetAddress(service.Endpoint.Address.Uri.ToString()));

            string entityName = "配件主数据接口(异步)";
            long svID = -1;
            if (IsLog)
            {
                svID = ProxyLogger.CreateTransferSV(entityName
                    //, EntitySerialization.EntitySerial(bpObj)
                    , Newtonsoft.Json.JsonConvert.SerializeObject(param)
                    , service.GetType().FullName, Newtonsoft.Json.JsonConvert.SerializeObject(service.Endpoint.Address));
            }

            //UFIDA.U9.CBO.Pub.Controller.ContextDTO contextDTO = new UFIDA.U9.CBO.Pub.Controller.ContextDTO();
            //contextDTO.EntCode = PubClass.GetString(UFIDA.U9.Base.Context.GetAttribute("EnterpriseID"));
            //contextDTO.OrgCode = Context.LoginOrg.Code;
            //contextDTO.UserCode = Context.LoginUser;

            U9Context context = GetHBHU9Context();

            try
            {
                //var result = service.receive(param);

                //service.receiveCompleted += new EventHandler<DMSAsync_PI07.receiveCompletedEventArgs>(service_receiveCompleted);
                //service.receiveAsync(param, svID);
                service.Beginreceive(param
                    , delegate(IAsyncResult asyncResult)
                    {
                        if (asyncResult != null
                            )
                        {
                            //long svID = (long)asyncResult.AsyncState;
                            svID = (long)asyncResult.AsyncState;

                            if (svID > 0)
                            {
                                EntityResult logResult = new EntityResult();

                                DMSAsync_PI06.partBaseDto result = null;
                                try
                                {
                                    result = service.Endreceive(asyncResult);
                                    //contextDTO.WriteToContext();
                                }
                                catch (Exception ex)
                                {
                                    //ProxyLogger.UpdateTransferSV(svID, string.Empty, false, ex.Message, "异步获取返回值异常!", ex.StackTrace);
                                    logResult.Sucessfull = false;
                                    logResult.Message = ex.Message;
                                    logResult.Trace = ex.StackTrace;
                                    logResult.StringValue = "异步获取返回值异常!";
                                    UpdateU9LogProxy(context, logResult, svID);
                                }

                                if (result != null
                                    )
                                {
                                    //string resultXml = EntitySerialization.EntitySerial(result);
                                    string resultXml = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                                    bool flag = result.flag == 1;
                                    string msg = result.errMsg;
                                    //try
                                    //{
                                    //    ProxyLogger.UpdateTransferSV(svID, resultXml, flag, msg, string.Empty, string.Empty);
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //    throw ex;
                                    //}

                                    logResult.Sucessfull = flag;
                                    logResult.Message = msg;
                                    logResult.StringValue = resultXml;
                                    logResult.Trace = string.Empty;

                                    UpdateU9LogProxy(context, logResult, svID);
                                }
                                else
                                {
                                    //ProxyLogger.UpdateTransferSV(svID, string.Empty, false, Const_ResultNullMessage, string.Empty, string.Empty);

                                    logResult.Sucessfull = false;
                                    logResult.Message = Const_ResultNullMessage;
                                    logResult.Trace = string.Empty;
                                    logResult.StringValue = "异步返回值为空!";
                                    UpdateU9LogProxy(context, logResult, svID);
                                }
                            }
                            //return result;
                        }
                    }
                    , svID);

            }
            catch (Exception ex)
            {
                if (svID > 0)
                {
                    ProxyLogger.UpdateTransferSV(svID, string.Empty, false, ex.Message, string.Empty, ex.StackTrace);
                }

                throw ex;
            }

            return null;
        }

        public static void SendSupplyItem2DMS_Async(IList<SupplySource> lstErpData, int actionType)
        {
            if (lstErpData != null
                && lstErpData.Count > 0
                )
            {
                DMSAsync_PI06.PI06Client service = PubExtend.GetPI06Async();

                System.Collections.Generic.List<DMSAsync_PI06.partBaseDto> lines = new System.Collections.Generic.List<DMSAsync_PI06.partBaseDto>();
                foreach (SupplySource supplierItem in lstErpData)
                {
                    DMSAsync_PI06.partBaseDto linedto = new DMSAsync_PI06.partBaseDto();
                    if (supplierItem.OriginalData.SupplierInfo != null && supplierItem.OriginalData.SupplierInfo.SupplierKey != null)
                    {
                        linedto.suptCode = supplierItem.OriginalData.SupplierInfo.Supplier.Code;
                    }
                    if (supplierItem.OriginalData.ItemInfo != null && supplierItem.OriginalData.ItemInfo.ItemIDKey != null)
                    {
                        linedto.partCode = supplierItem.OriginalData.ItemInfo.ItemID.Code;
                        linedto.partName = supplierItem.OriginalData.ItemInfo.ItemID.Name;
                    }
                    if (supplierItem.OriginalData.ItemInfo.ItemID.InventoryUOM != null)
                    {
                        linedto.unit = supplierItem.OriginalData.ItemInfo.ItemID.InventoryUOM.Name;
                    }
                    if (supplierItem.OriginalData.ItemInfo.ItemID.PurchaseInfo != null)
                    {
                        linedto.miniPack = ((supplierItem.ItemInfo.ItemID.PurchaseInfo.MinRcvQty > 0) ? System.Convert.ToInt32(supplierItem.ItemInfo.ItemID.PurchaseInfo.MinRcvQty) : 1);
                    }
                    //SalePriceLine line = SalePriceLine.Finder.Find(string.Format("SalePriceList.Org={0} and ItemInfo.ItemID={1} and Active=1 and '{2}' between FromDate and ToDate", Context.LoginOrg.ID.ToString(), supplierItem.OriginalData.ItemInfo.ItemID.ID.ToString(), System.DateTime.Now.ToString()), new OqlParam[0]);
                    SalePriceLine line = PubHelper.GetSalePriceList(supplierItem);
                    if (line != null)
                    {
                        linedto.salePrice = float.Parse(line.Price.ToString());
                        linedto.unitPrace = linedto.salePrice;
                    }
                    else
                    {
                        linedto.salePrice = 0f;
                        linedto.unitPrace = 0f;
                    }
                    linedto.isFlag = "2";
                    linedto.isDanger = "0";
                    linedto.isReturn = "1";
                    linedto.isSale = "1";
                    linedto.isEffective = supplierItem.OriginalData.Effective.IsEffective.ToString();
                    linedto.actionType = actionType;
                    lines.Add(linedto);
                }

                // service.Do(lines);

                try
                {
                    PubExtend.Do(service, lines.ToArray());
                }
                catch (System.Exception e)
                {
                    throw new System.ApplicationException("调用DMS接口错误：" + e.Message);
                }
            }
        }

        #endregion

        // 库存同步接口(同步)
        /// <summary>
        /// 库存同步接口(同步)
        /// </summary>
        /// <param name="service"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static stockDTO[] Do(this PI07ImplService service, stockDTO[] param)
        {
            service.Url = PubHelper.GetAddress(service.Url);

            string entityName = "库存同步接口";
            long svID = -1;
            if (IsLog)
            {
                svID = ProxyLogger.CreateTransferSV(entityName
                    //, EntitySerialization.EntitySerial(bpObj)
                    , Newtonsoft.Json.JsonConvert.SerializeObject(param)
                    , service.GetType().FullName,Newtonsoft.Json.JsonConvert.SerializeObject(service));
            }

            try
            {
                var result = service.receive(param);
                
                //service.receiveCompleted += new DMS_PI07.receiveCompletedEventHandler(service_receiveCompleted);
                //service.receiveAsync(param, svID);

                if (svID > 0)
                {
                    if (result != null
                        && result.Length > 0
                        )
                    {
                        //string resultXml = EntitySerialization.EntitySerial(result);
                        string resultXml = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                        bool flag = result[0] != null ? result[0].flag == 1 : false;
                        string msg = result[0] != null ? result[0].errMsg : string.Empty;
                        ProxyLogger.UpdateTransferSV(svID, resultXml, flag, msg, string.Empty, string.Empty);
                    }
                    else
                    {
                        ProxyLogger.UpdateTransferSV(svID, string.Empty, false, Const_ResultNullMessage, string.Empty, string.Empty);
                    }
                }

            }
            catch (Exception ex)
            {
                if (svID > 0)
                {
                    ProxyLogger.UpdateTransferSV(svID, string.Empty, false, ex.Message, string.Empty, ex.StackTrace);
                }

                throw ex;
            }

            return null;
        }


        public static DMSAsync_PI07.PI07Client GetPI07Async()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferPoolSize = int.MaxValue;
            //  http://10.3.11.227:9081/dms/ws/PI07
            //  http://10.3.21.115:8080/hbfcdms/ws/PI07
            System.ServiceModel.EndpointAddress remoteAddress = new System.ServiceModel.EndpointAddress(new Uri("http://ip/dms/ws/PI07")); ;

            DMSAsync_PI07.PI07Client service = new DMSAsync_PI07.PI07Client(binding, remoteAddress);

            return service;
        }


        // 库存同步接口(异步)
        /// <summary>
        /// 库存同步接口(异步)
        /// </summary>
        /// <param name="service"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DMSAsync_PI07.stockDTO[] Do(this DMSAsync_PI07.PI07Client service, DMSAsync_PI07.stockDTO[] param)
        {
            service.Endpoint.Address = new System.ServiceModel.EndpointAddress(PubHelper.GetAddress(service.Endpoint.Address.Uri.ToString()));

            string entityName = "库存同步接口(异步)";
            long svID = -1;
            if (IsLog)
            {
                svID = ProxyLogger.CreateTransferSV(entityName
                    //, EntitySerialization.EntitySerial(bpObj)
                    , Newtonsoft.Json.JsonConvert.SerializeObject(param)
                    , service.GetType().FullName, Newtonsoft.Json.JsonConvert.SerializeObject(service.Endpoint.Address));
            }

            //UFIDA.U9.CBO.Pub.Controller.ContextDTO contextDTO = new UFIDA.U9.CBO.Pub.Controller.ContextDTO();
            //contextDTO.EntCode = PubClass.GetString(UFIDA.U9.Base.Context.GetAttribute("EnterpriseID"));
            //contextDTO.OrgCode = Context.LoginOrg.Code;
            //contextDTO.UserCode = Context.LoginUser;

            //U9Context context = new U9Context();
            //string enCode = PubClass.GetString(UFIDA.U9.Base.Context.GetAttribute("EnterpriseID"));
            //context.EnterpriseCode = enCode;
            //context.EnterpriseID = enCode;
            //context.OrgCode = Context.LoginOrg.Code;
            //context.OrgID = Context.LoginOrg.ID.ToString();
            //context.CultureName = Context.LoginLanguageCode;
            //context.UserID = Context.LoginUserID;
            //context.UserCode = Context.LoginUser;
            //context.Url = "http://localhost/U9/HBHServices/U9.VOB.HBHCommon.IU9CommonSV.svc";
            U9Context context = GetHBHU9Context();

            try
            {
                //var result = service.receive(param);

                //service.receiveCompleted += new EventHandler<DMSAsync_PI07.receiveCompletedEventArgs>(service_receiveCompleted);
                //service.receiveAsync(param, svID);
                service.Beginreceive(param
                    , delegate(IAsyncResult asyncResult)
                      {
                          if (asyncResult != null
                              )
                          {
                              //long svID = (long)asyncResult.AsyncState;
                              svID = (long)asyncResult.AsyncState;

                              if (svID > 0)
                              {
                                  EntityResult logResult = new EntityResult();

                                  DMSAsync_PI07.stockDTO[] result = null;
                                  try
                                  {
                                      result = service.Endreceive(asyncResult);
                                      //contextDTO.WriteToContext();
                                  }
                                  catch (Exception ex)
                                  {
                                      //ProxyLogger.UpdateTransferSV(svID, string.Empty, false, ex.Message, "异步获取返回值异常!", ex.StackTrace);
                                      logResult.Sucessfull = false;
                                      logResult.Message = ex.Message;
                                      logResult.Trace = ex.StackTrace;
                                      logResult.StringValue = "异步获取返回值异常!";
                                      UpdateU9LogProxy(context, logResult, svID);
                                  }

                                  if (result != null
                                      && result.Length > 0
                                      )
                                  {
                                      //string resultXml = EntitySerialization.EntitySerial(result);
                                      string resultXml = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                                      bool flag = result[0] != null ? result[0].flag == 1 : false;
                                      string msg = result[0] != null ? result[0].errMsg : string.Empty;
                                      //try
                                      //{
                                      //    ProxyLogger.UpdateTransferSV(svID, resultXml, flag, msg, string.Empty, string.Empty);
                                      //}
                                      //catch (Exception ex)
                                      //{
                                      //    throw ex;
                                      //}

                                      logResult.Sucessfull = flag;
                                      logResult.Message = msg;
                                      logResult.StringValue = resultXml;
                                      logResult.Trace = string.Empty;

                                      UpdateU9LogProxy(context, logResult,svID);
                                  }
                                  else
                                  {
                                      //ProxyLogger.UpdateTransferSV(svID, string.Empty, false, Const_ResultNullMessage, string.Empty, string.Empty);

                                      logResult.Sucessfull = false;
                                      logResult.Message = Const_ResultNullMessage;
                                      logResult.Trace = string.Empty;
                                      logResult.StringValue = "异步返回值为空!";
                                      UpdateU9LogProxy(context, logResult,svID);
                                  }
                              }
                              //return result;
                          }
                      }
                    , svID);

            }
            catch (Exception ex)
            {
                if (svID > 0)
                {
                    ProxyLogger.UpdateTransferSV(svID, string.Empty, false, ex.Message, string.Empty, ex.StackTrace);
                }

                throw ex;
            }

            return null;
        }

        static void UpdateU9LogProxy(U9Context context,EntityResult result,long svID)
        {
            ServiceProxy svProxy = new ServiceProxy();

            U9ProxyLogger logger = new U9ProxyLogger();
            logger.ID = svID;
            logger.Result = result;

            svProxy.Do(context, logger);
            
        }

        static void service_receiveCompleted(object sender, DMSAsync_PI07.receiveCompletedEventArgs e)
        {
            if (e != null
                )
            {
                long svID = (long)e.UserState;

                if (svID > 0)
                {
                    DMSAsync_PI07.stockDTO[] result = e.Result;
                    if (result != null
                        && result.Length > 0
                        )
                    {
                        //string resultXml = EntitySerialization.EntitySerial(result);
                        string resultXml = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                        bool flag = result[0] != null ? result[0].flag == 1 : false;
                        string msg = result[0] != null ? result[0].errMsg : string.Empty;
                        ProxyLogger.UpdateTransferSV(svID, resultXml, flag, msg, string.Empty, string.Empty);
                    }
                    else
                    {
                        ProxyLogger.UpdateTransferSV(svID, string.Empty, false, Const_ResultNullMessage, string.Empty, string.Empty);
                    }
                }
                //return result;
            }
        }

        // 供应商同步接口
        /// <summary>
        /// 供应商同步接口
        /// </summary>
        /// <param name="service"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static supplierDto Do(this PI08ImplService service, supplierDto[] param)
        {
            service.Url = PubHelper.GetAddress(service.Url);

            string entityName = "供应商同步接口";
            long svID = -1;
            if (IsLog)
            {
                svID = ProxyLogger.CreateTransferSV(entityName
                    //, EntitySerialization.EntitySerial(bpObj)
                    , Newtonsoft.Json.JsonConvert.SerializeObject(param)
                    , service.GetType().FullName,Newtonsoft.Json.JsonConvert.SerializeObject(service));
            }

            try
            {
                var result = service.receive(param);

                if (svID > 0)
                {
                    if (result != null
                        )
                    {
                        //string resultXml = EntitySerialization.EntitySerial(result);
                        string resultXml = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                        ProxyLogger.UpdateTransferSV(svID, resultXml, result.flag == 1, result.errMsg, string.Empty, string.Empty);
                    }
                    else
                    {
                        ProxyLogger.UpdateTransferSV(svID, string.Empty, false, Const_ResultNullMessage, string.Empty, string.Empty);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                if (svID > 0)
                {
                    ProxyLogger.UpdateTransferSV(svID, string.Empty, false, ex.Message, string.Empty, ex.StackTrace);
                }

                throw ex;
            }

            return null;
        }

        // 整车订单同步接口
        /// <summary>
        /// 整车订单同步接口
        /// </summary>
        /// <param name="service"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static orderInfoDto[] Do(this SI02ImplService service, orderInfoDto[] param)
        {
            service.Url = PubHelper.GetAddress(service.Url);

            string entityName = "整车订单同步接口";
            long svID = -1;
            if (IsLog)
            {
                svID = ProxyLogger.CreateTransferSV(entityName
                    //, EntitySerialization.EntitySerial(bpObj)
                    , Newtonsoft.Json.JsonConvert.SerializeObject(param)
                    , service.GetType().FullName,Newtonsoft.Json.JsonConvert.SerializeObject(service));
            }

            try
            {
                var result = service.receive(param);

                if (svID > 0)
                {
                    if (result != null
                        && result.Length > 0
                        && result[0] != null
                        )
                    {
                        //string resultXml = EntitySerialization.EntitySerial(result);
                        string resultXml = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                        ProxyLogger.UpdateTransferSV(svID, resultXml, result[0].flag == 1, result[0].errMsg, string.Empty, string.Empty);
                    }
                    else
                    {
                        ProxyLogger.UpdateTransferSV(svID, string.Empty, false, Const_ResultNullMessage, string.Empty, string.Empty);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                if (svID > 0)
                {
                    ProxyLogger.UpdateTransferSV(svID, string.Empty, false, ex.Message, string.Empty, ex.StackTrace);
                }

                throw ex;
            }

            return null;
        }

        // 整车订单同步接口
        /// <summary>
        /// 整车订单同步接口
        /// </summary>
        /// <param name="service"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static vehicleInfoDto Do(this SI03ImplService service, vehicleInfoDto param)
        {
            service.Url = PubHelper.GetAddress(service.Url);

            string entityName = "整车订单同步接口";
            long svID = -1;
            if (IsLog)
            {
                svID = ProxyLogger.CreateTransferSV(entityName
                    //, EntitySerialization.EntitySerial(bpObj)
                    , Newtonsoft.Json.JsonConvert.SerializeObject(param)
                    , service.GetType().FullName,Newtonsoft.Json.JsonConvert.SerializeObject(service));
            }

            try
            {
                var result = service.receive(param);

                if (svID > 0)
                {
                    if (result != null
                        )
                    {
                        //string resultXml = EntitySerialization.EntitySerial(result);
                        string resultXml = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                        ProxyLogger.UpdateTransferSV(svID, resultXml, result.flag == 1, result.errMsg, string.Empty, string.Empty);
                    }
                    else
                    {
                        ProxyLogger.UpdateTransferSV(svID, string.Empty, false, Const_ResultNullMessage, string.Empty, string.Empty);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                if (svID > 0)
                {
                    ProxyLogger.UpdateTransferSV(svID, string.Empty, false, ex.Message, string.Empty, ex.StackTrace);
                }

                throw ex;
            }

            return null;
        }

        // 打款接口
        /// <summary>
        /// 打款接口
        /// </summary>
        /// <param name="service"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static accountInfoDto Do(this SI04ImplService service, accountInfoDto param)
        {
            service.Url = PubHelper.GetAddress(service.Url);

            string entityName = "资金同步接口";
            long svID = -1;
            if (IsLog)
            {
                svID = ProxyLogger.CreateTransferSV(entityName
                    //, EntitySerialization.EntitySerial(bpObj)
                    , Newtonsoft.Json.JsonConvert.SerializeObject(param)
                    , service.GetType().FullName,Newtonsoft.Json.JsonConvert.SerializeObject(service));
            }

            try
            {
                var result = service.receive(param);

                if (svID > 0)
                {
                    if (result != null
                        )
                    {
                        //string resultXml = EntitySerialization.EntitySerial(result);
                        string resultXml = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                        ProxyLogger.UpdateTransferSV(svID, resultXml, result.flag == 1, result.errMsg, string.Empty, string.Empty);
                    }
                    else
                    {
                        ProxyLogger.UpdateTransferSV(svID, string.Empty, false, Const_ResultNullMessage, string.Empty, string.Empty);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                if (svID > 0)
                {
                    ProxyLogger.UpdateTransferSV(svID, string.Empty, false, ex.Message, string.Empty, ex.StackTrace);
                }

                throw ex;
            }

            return null;
        }

        // 资金同步接口(好像已弃用，与资金同步一起了)
        /// <summary>
        /// 资金同步接口(好像已弃用，与资金同步一起了)
        /// </summary>
        /// <param name="service"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static accountReturnDto Do(this SI05ImplService service, accountReturnDto param)
        {
            service.Url = PubHelper.GetAddress(service.Url);

            string entityName = "打款同步接口";
            long svID = -1;
            if (IsLog)
            {
                svID = ProxyLogger.CreateTransferSV(entityName
                    //, EntitySerialization.EntitySerial(bpObj)
                    , Newtonsoft.Json.JsonConvert.SerializeObject(param)
                    , service.GetType().FullName,Newtonsoft.Json.JsonConvert.SerializeObject(service));
            }

            try
            {
                var result = service.receive(param);

                if (svID > 0)
                {
                    if (result != null
                        )
                    {
                        //string resultXml = EntitySerialization.EntitySerial(result);
                        string resultXml = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                        ProxyLogger.UpdateTransferSV(svID, resultXml, result.flag == 1, result.errMsg, string.Empty, string.Empty);
                    }
                    else
                    {
                        ProxyLogger.UpdateTransferSV(svID, string.Empty, false, Const_ResultNullMessage, string.Empty, string.Empty);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                if (svID > 0)
                {
                    ProxyLogger.UpdateTransferSV(svID, string.Empty, false, ex.Message, string.Empty, ex.StackTrace);
                }

                throw ex;
            }

            return null;
        }

        // 经销商主数据接口
        /// <summary>
        /// 经销商主数据接口
        /// </summary>
        /// <param name="service"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static dealerInfoDto Do(this SI08ImplService service, dealerInfoDto[] param)
        {
            service.Url = PubHelper.GetAddress(service.Url);

            string entityName = "经销商主数据接口";
            long svID = -1;
            if (IsLog)
            {
                svID = ProxyLogger.CreateTransferSV(entityName
                    //, EntitySerialization.EntitySerial(bpObj)
                    , Newtonsoft.Json.JsonConvert.SerializeObject(param)
                    , service.GetType().FullName,Newtonsoft.Json.JsonConvert.SerializeObject(service));
            }

            try
            {
                var result = service.receive(param);

                if (svID > 0)
                {
                    if (result != null
                        )
                    {
                        //string resultXml = EntitySerialization.EntitySerial(result);
                        string resultXml = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                        ProxyLogger.UpdateTransferSV(svID, resultXml, result.flag == 1, result.errMsg, string.Empty, string.Empty);
                    }
                    else
                    {
                        ProxyLogger.UpdateTransferSV(svID, string.Empty, false, Const_ResultNullMessage, string.Empty, string.Empty);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                if (svID > 0)
                {
                    ProxyLogger.UpdateTransferSV(svID, string.Empty, false, ex.Message, string.Empty, ex.StackTrace);
                }

                throw ex;
            }

            return null;
        }

        // 车辆整改接口
        /// <summary>
        /// 车辆整改接口
        /// </summary>
        /// <param name="service"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static vehicleChangeInfoDto Do(this SI09ImplService service, vehicleChangeInfoDto param)
        {
            service.Url = PubHelper.GetAddress(service.Url);

            string entityName = "车辆整改接口";
            long svID = -1;
            if (IsLog)
            {
                svID = ProxyLogger.CreateTransferSV(entityName
                    //, EntitySerialization.EntitySerial(bpObj)
                    , Newtonsoft.Json.JsonConvert.SerializeObject(param)
                    , service.GetType().FullName,Newtonsoft.Json.JsonConvert.SerializeObject(service));
            }

            try
            {
                var result = service.receive(param);

                if (svID > 0)
                {
                    if (result != null
                        )
                    {
                        //string resultXml = EntitySerialization.EntitySerial(result);
                        string resultXml = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                        ProxyLogger.UpdateTransferSV(svID, resultXml, result.flag == 1, result.errMsg, string.Empty, string.Empty);
                    }
                    else
                    {
                        ProxyLogger.UpdateTransferSV(svID, string.Empty, false, Const_ResultNullMessage, string.Empty, string.Empty);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                if (svID > 0)
                {
                    ProxyLogger.UpdateTransferSV(svID, string.Empty, false, ex.Message, string.Empty, ex.StackTrace);
                }

                throw ex;
            }

            return null;
        }

        // 移库接口
        /// <summary>
        /// 移库接口
        /// </summary>
        /// <param name="service"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static vehicleMoveInfoDto Do(this SI11ImplService service, vehicleMoveInfoDto[] param)
        {
            service.Url = PubHelper.GetAddress(service.Url);

            string entityName = "移库接口";
            long svID = -1;
            if (IsLog)
            {
                svID = ProxyLogger.CreateTransferSV(entityName
                    //, EntitySerialization.EntitySerial(bpObj)
                    , Newtonsoft.Json.JsonConvert.SerializeObject(param)
                    , service.GetType().FullName,Newtonsoft.Json.JsonConvert.SerializeObject(service));
            }

            try
            {
                var result = service.receive(param);

                if (svID > 0)
                {
                    if (result != null
                        )
                    {
                        //string resultXml = EntitySerialization.EntitySerial(result);
                        string resultXml = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                        ProxyLogger.UpdateTransferSV(svID, resultXml, result.flag == 1, result.errMsg, string.Empty, string.Empty);
                    }
                    else
                    {
                        ProxyLogger.UpdateTransferSV(svID, string.Empty, false, Const_ResultNullMessage, string.Empty, string.Empty);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                if (svID > 0)
                {
                    ProxyLogger.UpdateTransferSV(svID, string.Empty, false, ex.Message, string.Empty, ex.StackTrace);
                }

                throw ex;
            }

            return null;
        }





        private static U9Context GetHBHU9Context()
        {
            U9Context context = new U9Context();
            string enCode = PubClass.GetString(UFIDA.U9.Base.Context.GetAttribute("EnterpriseID"));
            context.EnterpriseCode = enCode;
            context.EnterpriseID = enCode;
            context.OrgCode = Context.LoginOrg.Code;
            context.OrgID = Context.LoginOrg.ID.ToString();
            context.CultureName = Context.LoginLanguageCode;
            context.UserID = Context.LoginUserID;
            context.UserCode = Context.LoginUser;
            context.Url = "http://localhost/U9/HBHServices/U9.VOB.HBHCommon.IU9CommonSV.svc";
            return context;
        }
    }
}