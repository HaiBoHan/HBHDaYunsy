using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            DMS_PI06Aysn();

            Console.ReadLine();
            Console.Read();
        }

        private static void DMS_PI06Aysn()
        {
            //DMSAsync_PI06.PI06Client service = new DMSAsync_PI06.PI06Client();

            //System.Collections.Generic.List<DMSAsync_PI06.partBaseDto> lines = new System.Collections.Generic.List<DMSAsync_PI06.partBaseDto>();

            DMS_PI06.PI06ImplService service = new DMS_PI06.PI06ImplService();
            System.Collections.Generic.List<DMS_PI06.partBaseDto> lines = new System.Collections.Generic.List<DMS_PI06.partBaseDto>();

            {
                //DMSAsync_PI06.partBaseDto linedto = new DMSAsync_PI06.partBaseDto();
                DMS_PI06.partBaseDto linedto = new DMS_PI06.partBaseDto();
                linedto.suptCode = "供应商001";
                linedto.partCode = "PartCode001";
                linedto.partName = "配件001";
                //if (item.InventoryUOM != null)
                {
                    linedto.unit = "个";
                }
                //if (item.PurchaseInfo != null)
                {
                    linedto.miniPack = 1;
                }
                linedto.salePrice = 10;
                linedto.unitPrace = 10;
                linedto.isDanger = "0";
                linedto.isReturn = "1";
                linedto.isSale = "1";
                linedto.isFlag = "1";
                linedto.isEffective = "true";
                linedto.actionType = 2;
                lines.Add(linedto);
            }

            // service.Do(lines);

            if (lines != null
                && lines.Count > 0
                )
            {
                try
                {
                    PI06AysnDo(service, lines.ToArray());
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(("调用DMS接口错误：" + e.Message));
                }
            }

        }

        //private static void PI06AysnDo(DMSAsync_PI06.PI06Client service, DMSAsync_PI06.partBaseDto[] param)
        private static void PI06AysnDo(DMS_PI06.PI06ImplService service, DMS_PI06.partBaseDto[] param)
        {
            //string uri = PubHelper.GetAddress(service.Endpoint.Address.Uri.ToString());

            //string oldurl = service.Endpoint.Address.Uri.ToString();
            //string newurl = "http://scisoft.eicp.net:9080/dms/ws";

            //int index = oldurl.LastIndexOf("/");
            //string svName = oldurl.Substring(index);

            //newurl += svName;

            //service.Endpoint.Address = new System.ServiceModel.EndpointAddress(newurl);

            string entityName = "配件主数据接口(异步)";
            long svID = -1;
            
            try
            {
                //var result = service.receive(param);

                //service.receiveCompleted += new EventHandler<DMSAsync_PI07.receiveCompletedEventArgs>(service_receiveCompleted);
                //service.receiveAsync(param, svID);

                service.receive(param);

                //object userState = 1;
                //service.receiveCompleted += new DMS_PI06.receiveCompletedEventHandler(service_receiveCompleted);
                //service.receiveAsync(param, userState);

                return;

                //service.Beginreceive(param
                //    , delegate(IAsyncResult asyncResult)
                //    {
                //        if (asyncResult != null
                //            )
                //        {
                //            //long svID = (long)asyncResult.AsyncState;
                //            svID = (long)asyncResult.AsyncState;

                //            if (svID > 0)
                //            {
                //                //EntityResult logResult = new EntityResult();

                //                DMSAsync_PI06.partBaseDto result = null;
                //                try
                //                {
                //                    result = service.Endreceive(asyncResult);
                //                    //contextDTO.WriteToContext();
                //                }
                //                catch (Exception ex)
                //                {
                //                    //ProxyLogger.UpdateTransferSV(svID, string.Empty, false, ex.Message, "异步获取返回值异常!", ex.StackTrace);
                //                    //logResult.Sucessfull = false;
                //                    //logResult.Message = ex.Message;
                //                    //logResult.Trace = ex.StackTrace;
                //                    //logResult.StringValue = "异步获取返回值异常!";
                //                    //UpdateU9LogProxy(context, logResult, svID);
                //                }

                //                if (result != null
                //                    )
                //                {
                //                    //string resultXml = EntitySerialization.EntitySerial(result);
                //                    //string resultXml = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                //                    //bool flag = result.flag == 1;
                //                    //string msg = result.errMsg;
                //                    ////try
                //                    ////{
                //                    ////    ProxyLogger.UpdateTransferSV(svID, resultXml, flag, msg, string.Empty, string.Empty);
                //                    ////}
                //                    ////catch (Exception ex)
                //                    ////{
                //                    ////    throw ex;
                //                    ////}

                //                    //logResult.Sucessfull = flag;
                //                    //logResult.Message = msg;
                //                    //logResult.StringValue = resultXml;
                //                    //logResult.Trace = string.Empty;

                //                    //UpdateU9LogProxy(context, logResult, svID);
                //                }
                //                else
                //                {
                //                    //ProxyLogger.UpdateTransferSV(svID, string.Empty, false, Const_ResultNullMessage, string.Empty, string.Empty);

                //                    //logResult.Sucessfull = false;
                //                    //logResult.Message = Const_ResultNullMessage;
                //                    //logResult.Trace = string.Empty;
                //                    //logResult.StringValue = "异步返回值为空!";
                //                    //UpdateU9LogProxy(context, logResult, svID);
                //                }
                //            }
                //            //return result;
                //        }
                //    }
                //    , svID);

            }
            catch (Exception e)
            {
                //if (svID > 0)
                //{
                //    ProxyLogger.UpdateTransferSV(svID, string.Empty, false, ex.Message, string.Empty, ex.StackTrace);
                //}

                Console.WriteLine(("调用DMS接口错误：" + e.Message));

                //throw ex;
            }
        }

        static void service_receiveCompleted(object sender, DMS_PI06.receiveCompletedEventArgs e)
        {
            if (e != null)
            {
                Console.WriteLine(string.Format("UserState:{0} ;Cancelled:{1} ;Error:{2} ; Result.flag:{3} ; Result.errMsg:{4}"
                    , e.UserState
                    , e.Cancelled
                    , e.Error
                    , e.Result != null ? e.Result.flag : -1
                    , e.Result != null ? e.Result.errMsg : string.Empty
                    ));
            }
        }

        private static void U9CommonAysnDo(U9CommonSV.HBHCommonSVForJavaStub service, string entityName , string entityContext)
        {
            //string uri = PubHelper.GetAddress(service.Endpoint.Address.Uri.ToString());

            //string oldurl = service.Endpoint.Address.Uri.ToString();
            //string newurl = "http://scisoft.eicp.net:9080/dms/ws";

            //int index = oldurl.LastIndexOf("/");
            //string svName = oldurl.Substring(index);

            //newurl += svName;

            //service.Endpoint.Address = new System.ServiceModel.EndpointAddress(newurl);

            //string entityName = "配件主数据接口(异步)";
            //long svID = -1;

            try
            {
                //var result = service.receive(param);

                //service.receiveCompleted += new EventHandler<DMSAsync_PI07.receiveCompletedEventArgs>(service_receiveCompleted);
                //service.receiveAsync(param, svID);

                service.DoCompleted += new U9CommonSV.DoCompletedEventHandler(service_DoCompleted);

                //service.Do(
            }
            catch (Exception e)
            {
                //if (svID > 0)
                //{
                //    ProxyLogger.UpdateTransferSV(svID, string.Empty, false, ex.Message, string.Empty, ex.StackTrace);
                //}

                Console.WriteLine(("调用DMS接口错误：" + e.Message));
            }
        }

        static void service_DoCompleted(object sender, U9CommonSV.DoCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
