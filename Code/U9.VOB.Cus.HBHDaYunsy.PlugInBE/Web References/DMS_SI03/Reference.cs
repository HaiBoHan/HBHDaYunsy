﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.34209
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.34209 版自动生成。
// 
#pragma warning disable 1591

namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMS_SI03 {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="SI03ImplServiceSoapBinding", Namespace="http://server.sales.ws.org.com/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(baseDto))]
    public partial class SI03ImplService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback receiveOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public SI03ImplService() {
            this.Url = global::U9.VOB.Cus.HBHDaYunsy.PlugInBE.Properties.Settings.Default.U9_VOB_Cus_HBHDaYunsy_PlugInBE_DMS_SI03_SI03ImplService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event receiveCompletedEventHandler receiveCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://server.sales.ws.org.com/", ResponseNamespace="http://server.sales.ws.org.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public vehicleInfoDto receive([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] vehicleInfoDto arg0) {
            object[] results = this.Invoke("receive", new object[] {
                        arg0});
            return ((vehicleInfoDto)(results[0]));
        }
        
        /// <remarks/>
        public void receiveAsync(vehicleInfoDto arg0) {
            this.receiveAsync(arg0, null);
        }
        
        /// <remarks/>
        public void receiveAsync(vehicleInfoDto arg0, object userState) {
            if ((this.receiveOperationCompleted == null)) {
                this.receiveOperationCompleted = new System.Threading.SendOrPostCallback(this.OnreceiveOperationCompleted);
            }
            this.InvokeAsync("receive", new object[] {
                        arg0}, this.receiveOperationCompleted, userState);
        }
        
        private void OnreceiveOperationCompleted(object arg) {
            if ((this.receiveCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.receiveCompleted(this, new receiveCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://server.sales.ws.org.com/")]
    public partial class vehicleInfoDto : baseDto {
        
        private System.DateTime actualOfflineDateField;
        
        private bool actualOfflineDateFieldSpecified;
        
        private long actualTypeField;
        
        private bool actualTypeFieldSpecified;
        
        private string adjustDescField;
        
        private string commissionNoField;
        
        private string dmsSaleNoField;
        
        private string engineNoField;
        
        private string erpMaterialCodeField;
        
        private string flowingCodeField;
        
        private string materialCodeField;
        
        private string nodeStatusField;
        
        private string oldVinField;
        
        private System.DateTime predictOfflineDateField;
        
        private bool predictOfflineDateFieldSpecified;
        
        private int productStatusField;
        
        private string remarkField;
        
        private string requestTypeField;
        
        private string saddleTypeField;
        
        private string tireCodeField;
        
        private string vinField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime actualOfflineDate {
            get {
                return this.actualOfflineDateField;
            }
            set {
                this.actualOfflineDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool actualOfflineDateSpecified {
            get {
                return this.actualOfflineDateFieldSpecified;
            }
            set {
                this.actualOfflineDateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long actualType {
            get {
                return this.actualTypeField;
            }
            set {
                this.actualTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool actualTypeSpecified {
            get {
                return this.actualTypeFieldSpecified;
            }
            set {
                this.actualTypeFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string adjustDesc {
            get {
                return this.adjustDescField;
            }
            set {
                this.adjustDescField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string commissionNo {
            get {
                return this.commissionNoField;
            }
            set {
                this.commissionNoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dmsSaleNo {
            get {
                return this.dmsSaleNoField;
            }
            set {
                this.dmsSaleNoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string engineNo {
            get {
                return this.engineNoField;
            }
            set {
                this.engineNoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string erpMaterialCode {
            get {
                return this.erpMaterialCodeField;
            }
            set {
                this.erpMaterialCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string flowingCode {
            get {
                return this.flowingCodeField;
            }
            set {
                this.flowingCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string materialCode {
            get {
                return this.materialCodeField;
            }
            set {
                this.materialCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string nodeStatus {
            get {
                return this.nodeStatusField;
            }
            set {
                this.nodeStatusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string oldVin {
            get {
                return this.oldVinField;
            }
            set {
                this.oldVinField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime predictOfflineDate {
            get {
                return this.predictOfflineDateField;
            }
            set {
                this.predictOfflineDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool predictOfflineDateSpecified {
            get {
                return this.predictOfflineDateFieldSpecified;
            }
            set {
                this.predictOfflineDateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int productStatus {
            get {
                return this.productStatusField;
            }
            set {
                this.productStatusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string remark {
            get {
                return this.remarkField;
            }
            set {
                this.remarkField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string requestType {
            get {
                return this.requestTypeField;
            }
            set {
                this.requestTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string saddleType {
            get {
                return this.saddleTypeField;
            }
            set {
                this.saddleTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string tireCode {
            get {
                return this.tireCodeField;
            }
            set {
                this.tireCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string vin {
            get {
                return this.vinField;
            }
            set {
                this.vinField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(vehicleInfoDto))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://server.sales.ws.org.com/")]
    public partial class baseDto {
        
        private int actionTypeField;
        
        private string errMsgField;
        
        private int flagField;
        
        private System.DateTime invokeTimeField;
        
        private bool invokeTimeFieldSpecified;
        
        private string passwordField;
        
        private string usernameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int actionType {
            get {
                return this.actionTypeField;
            }
            set {
                this.actionTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string errMsg {
            get {
                return this.errMsgField;
            }
            set {
                this.errMsgField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int flag {
            get {
                return this.flagField;
            }
            set {
                this.flagField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime invokeTime {
            get {
                return this.invokeTimeField;
            }
            set {
                this.invokeTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool invokeTimeSpecified {
            get {
                return this.invokeTimeFieldSpecified;
            }
            set {
                this.invokeTimeFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string username {
            get {
                return this.usernameField;
            }
            set {
                this.usernameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void receiveCompletedEventHandler(object sender, receiveCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class receiveCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal receiveCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public vehicleInfoDto Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((vehicleInfoDto)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591