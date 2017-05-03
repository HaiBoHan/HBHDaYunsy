package com.org.ws.part.client;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Collection;
import java.util.GregorianCalendar;
import java.util.Iterator;
import java.util.List;
import java.util.ListIterator;

import javax.xml.bind.JAXBElement;
import javax.xml.datatype.DatatypeFactory;
import javax.xml.datatype.XMLGregorianCalendar;
import javax.xml.ws.Holder;

import net.sf.json.JSONObject;

import org.apache.cxf.endpoint.Client;
import org.apache.cxf.frontend.ClientProxy;
import org.apache.cxf.transport.http.HTTPConduit;
import org.apache.cxf.transports.http.configuration.HTTPClientPolicy;
import org.apache.log4j.Logger;
import org.tempuri.CreateShipSVStub;
import org.tempuri.HBHCommonSVForJavaStub;
import org.ufida.entitydata.U9VOBHBHCommonSVResultDTOData;
import org.ufida11.U9VOBHBHCommonIHBHCommonSVForJava;
import org.ufidapi03.UFIDAU9CustHBDYAPIShipSVICreateShipSV;
import org.ufidapi03.entitydata.ArrayOfUFIDAU9CustHBDYAPIShipSVShipBackDTOData;
import org.ufidapi03.entitydata.ArrayOfUFIDAU9CustHBDYAPIShipSVShipLineDTOData;
import org.ufidapi03.entitydata.UFIDAU9CustHBDYAPIShipSVShipLineDTOData;

import com.org.framework.common.DBFactory;
import com.org.framework.common.QuerySet;
import com.org.framework.util.Pub;
import com.org.mvc.context.ActionContext;
import com.org.ws.common.CommonHandleUtil;
import com.org.ws.common.WsConstant;
import com.org.ws.part.dao.PartReturnDao;
import com.org.ws.part.dto.ReturnOrderDtlDto;
import com.org.ws.part.dto.ReturnOrderDto;


/**
 * @Title: 旧件杂发接口(同步)
 * @Description:(DMS发送旧件杂发信息到U9) 
 * @throws
 */
public class PI11 implements WsConstant{
    private CommonHandleUtil util = new CommonHandleUtil();
    private PartReturnDao dao = new PartReturnDao();
    private Logger log = Logger.getLogger(this.getClass().getName());
    /**
     * @Title: send 
     * @param orderId 订单ID
     * @throws
     */
    public void send(DBFactory factory,String orderId) {
        try {
            //获取订单编号
            QuerySet qs = dao.getOrderNo(orderId,factory);
            if (qs.getRowCount() <= 0) {
                handleErr("没有找到此旧件发运单 旧件编号为 " + orderId);
            }
            //回运单号
            String returnNo = qs.getString(1, "ORDER_NO");
           
            // 渠道代码
            String dealerCode = qs.getString(1, "CODE");
            String returnProdate =qs.getString(1, "RETURN_PRODATE");
            String returnDate =qs.getString(1, "RETURN_DATE");
            String returnRemark =qs.getString(1,"REMARK");
            ReturnOrderDto dto =new ReturnOrderDto();
            dto.setReturnNo(returnNo);
            dto.setDealerCode(dealerCode);
            dto.setReturnProdate(returnProdate);
            dto.setReturnDate(returnDate);
            dto.setReMark(returnRemark);
            List  dlldto= new ArrayList() ;
            QuerySet qsDtl = dao.queryReturnPartsInfo(orderId,factory);
             for (int i=0;i<qsDtl.getRowCount();i++) {
            	 ReturnOrderDtlDto dtldto =new ReturnOrderDtlDto();
            	 dtldto.setClaimNo(qsDtl.getString(i, "CLAIM_NO"));
                 dtldto.setOrderNo(qsDtl.getString(i, "ORDER_NO"));
                 dtldto.setPartCode(qsDtl.getString(i, "PART_CODE"));
                 dtldto.setOldSuptCode(qsDtl.getString(i, "OLD_SUPT_CODE"));
                 dtldto.setDutySuptCode(qsDtl.getString(i, "DUTY_SUPT_CODE"));
                 dtldto.setPartTroubleDesc(qsDtl.getString(i, "PART_TROUBLE_DESC"));
                 dtldto.setReturnCount(qsDtl.getString(i, "RETURN_COUNT"));
                 dtldto.setActualCount(qsDtl.getString(i, "ACTUAL_COUNT"));
                 dtldto.setAlreadyIn(qsDtl.getString(i, "ALREADY_IN"));
                 dtldto.setRemark(qsDtl.getString(1, "REMARK"));
                 dlldto.add(dtldto);
             }
             dto.setReturnOrderDtlDto(dlldto);
            
             JSONObject jsonObject = JSONObject.fromObject(dto);
             
            //将dto传递到ERP 调用接口
             HBHCommonSVForJavaStub stub = new HBHCommonSVForJavaStub();
             U9VOBHBHCommonIHBHCommonSVForJava createsv =  stub.getBasicHttpBindingU9VOBHBHCommonIHBHCommonSVForJava();
            // 设置连接时间
            Client client = ClientProxy.getClient(createsv);
            HTTPConduit http = (HTTPConduit) client.getConduit();
            HTTPClientPolicy httpClientPolicy =  new  HTTPClientPolicy();
            httpClientPolicy.setConnectionTimeout( 30000 );
            httpClientPolicy.setAllowChunking( false );
            httpClientPolicy.setReceiveTimeout( 120000 );
            http.setClient(httpClientPolicy);
            Holder<exceptions.ubf.ufsoft.ArrayOfMessageBase> outMessages = new Holder<exceptions.ubf.ufsoft.ArrayOfMessageBase>();
            
    		Holder<U9VOBHBHCommonSVResultDTOData> doResult = new Holder<U9VOBHBHCommonSVResultDTOData>();
            createsv._do(util.createContext(1), "CreateMiscReceive", jsonObject.toString(), doResult, outMessages);
            
            // 接口返回值
    		boolean bl = false;
			long erpID = -1;
			if(doResult != null && doResult.value != null){
				bl = doResult.value.isMSucess();
				JAXBElement<String> msg = doResult.value.getMMessage();
				if(bl==false){
					handleErr(msg.getValue());
				}
			}
        } catch (Exception e) {
            log.error(e, e);
            handleErr(e.getMessage());
        }
    }
    
    /** 
     * @Title: handleErr 
     * @Description:(出错处理) 
     * @param @param errMsg 出错信息
     * @return void 返回类型 
     * @throws
     */
    private void handleErr(String errMsg) {
        util.handleErr("配件发运失败 " + errMsg);
    }
    
    public static void main(String[] args) {
        DBFactory factory = new DBFactory();
        ActionContext atx = ActionContext.getContext();
        atx.setDBFactory(factory);
        PI11 si = new PI11();
        si.send(factory,"2012122003351924");
    }
}
