﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.34003
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08 {
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://server.sales.ws.org.com/")]
    public partial class Exception : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string messageField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
                this.RaisePropertyChanged("message");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(dealerInfoDto))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://server.sales.ws.org.com/")]
    public partial class baseDto : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int actionTypeField;
        
        private string errMsgField;
        
        private int flagField;
        
        private System.DateTime invokeTimeField;
        
        private bool invokeTimeFieldSpecified;
        
        private string passwordField;
        
        private string usernameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public int actionType {
            get {
                return this.actionTypeField;
            }
            set {
                this.actionTypeField = value;
                this.RaisePropertyChanged("actionType");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string errMsg {
            get {
                return this.errMsgField;
            }
            set {
                this.errMsgField = value;
                this.RaisePropertyChanged("errMsg");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public int flag {
            get {
                return this.flagField;
            }
            set {
                this.flagField = value;
                this.RaisePropertyChanged("flag");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public System.DateTime invokeTime {
            get {
                return this.invokeTimeField;
            }
            set {
                this.invokeTimeField = value;
                this.RaisePropertyChanged("invokeTime");
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
                this.RaisePropertyChanged("invokeTimeSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
                this.RaisePropertyChanged("password");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string username {
            get {
                return this.usernameField;
            }
            set {
                this.usernameField = value;
                this.RaisePropertyChanged("username");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://server.sales.ws.org.com/")]
    public partial class dealerInfoDto : baseDto {
        
        private string companyCodeField;
        
        private string companyNameField;
        
        private string companyShortNameField;
        
        private string dealerCodeField;
        
        private string dealerNameField;
        
        private string dealerShortNameField;
        
        private int dealerTypeField;
        
        private string statusField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string companyCode {
            get {
                return this.companyCodeField;
            }
            set {
                this.companyCodeField = value;
                this.RaisePropertyChanged("companyCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string companyName {
            get {
                return this.companyNameField;
            }
            set {
                this.companyNameField = value;
                this.RaisePropertyChanged("companyName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string companyShortName {
            get {
                return this.companyShortNameField;
            }
            set {
                this.companyShortNameField = value;
                this.RaisePropertyChanged("companyShortName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string dealerCode {
            get {
                return this.dealerCodeField;
            }
            set {
                this.dealerCodeField = value;
                this.RaisePropertyChanged("dealerCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string dealerName {
            get {
                return this.dealerNameField;
            }
            set {
                this.dealerNameField = value;
                this.RaisePropertyChanged("dealerName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string dealerShortName {
            get {
                return this.dealerShortNameField;
            }
            set {
                this.dealerShortNameField = value;
                this.RaisePropertyChanged("dealerShortName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public int dealerType {
            get {
                return this.dealerTypeField;
            }
            set {
                this.dealerTypeField = value;
                this.RaisePropertyChanged("dealerType");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
                this.RaisePropertyChanged("status");
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://server.sales.ws.org.com/", ConfigurationName="DMSAsync_SI08.SI08")]
    public interface SI08 {
        
        // CODEGEN: 参数“return”需要其他方案信息，使用参数模式无法捕获这些信息。特定特性为“System.Xml.Serialization.XmlElementAttribute”。
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.Exception), Action="", Name="Exception")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(baseDto))]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.receiveResponse receive(U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.receive request);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="", ReplyAction="*")]
        System.IAsyncResult Beginreceive(U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.receive request, System.AsyncCallback callback, object asyncState);
        
        U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.receiveResponse Endreceive(System.IAsyncResult result);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="receive", WrapperNamespace="http://server.sales.ws.org.com/", IsWrapped=true)]
    public partial class receive {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://server.sales.ws.org.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("arg0", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.dealerInfoDto[] arg0;
        
        public receive() {
        }
        
        public receive(U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.dealerInfoDto[] arg0) {
            this.arg0 = arg0;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="receiveResponse", WrapperNamespace="http://server.sales.ws.org.com/", IsWrapped=true)]
    public partial class receiveResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://server.sales.ws.org.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.dealerInfoDto @return;
        
        public receiveResponse() {
        }
        
        public receiveResponse(U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.dealerInfoDto @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface SI08Channel : U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.SI08, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class receiveCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public receiveCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.dealerInfoDto Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.dealerInfoDto)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SI08Client : System.ServiceModel.ClientBase<U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.SI08>, U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.SI08 {
        
        private BeginOperationDelegate onBeginreceiveDelegate;
        
        private EndOperationDelegate onEndreceiveDelegate;
        
        private System.Threading.SendOrPostCallback onreceiveCompletedDelegate;
        
        public SI08Client() {
        }
        
        public SI08Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SI08Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SI08Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SI08Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<receiveCompletedEventArgs> receiveCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.receiveResponse U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.SI08.receive(U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.receive request) {
            return base.Channel.receive(request);
        }
        
        public U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.dealerInfoDto receive(U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.dealerInfoDto[] arg0) {
            U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.receive inValue = new U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.receive();
            inValue.arg0 = arg0;
            U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.receiveResponse retVal = ((U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.SI08)(this)).receive(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.SI08.Beginreceive(U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.receive request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.Beginreceive(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult Beginreceive(U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.dealerInfoDto[] arg0, System.AsyncCallback callback, object asyncState) {
            U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.receive inValue = new U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.receive();
            inValue.arg0 = arg0;
            return ((U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.SI08)(this)).Beginreceive(inValue, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.receiveResponse U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.SI08.Endreceive(System.IAsyncResult result) {
            return base.Channel.Endreceive(result);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.dealerInfoDto Endreceive(System.IAsyncResult result) {
            U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.receiveResponse retVal = ((U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.SI08)(this)).Endreceive(result);
            return retVal.@return;
        }
        
        private System.IAsyncResult OnBeginreceive(object[] inValues, System.AsyncCallback callback, object asyncState) {
            U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.dealerInfoDto[] arg0 = ((U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.dealerInfoDto[])(inValues[0]));
            return this.Beginreceive(arg0, callback, asyncState);
        }
        
        private object[] OnEndreceive(System.IAsyncResult result) {
            U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.dealerInfoDto retVal = this.Endreceive(result);
            return new object[] {
                    retVal};
        }
        
        private void OnreceiveCompleted(object state) {
            if ((this.receiveCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.receiveCompleted(this, new receiveCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void receiveAsync(U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.dealerInfoDto[] arg0) {
            this.receiveAsync(arg0, null);
        }
        
        public void receiveAsync(U9.VOB.Cus.HBHDaYunsy.PlugInBE.DMSAsync_SI08.dealerInfoDto[] arg0, object userState) {
            if ((this.onBeginreceiveDelegate == null)) {
                this.onBeginreceiveDelegate = new BeginOperationDelegate(this.OnBeginreceive);
            }
            if ((this.onEndreceiveDelegate == null)) {
                this.onEndreceiveDelegate = new EndOperationDelegate(this.OnEndreceive);
            }
            if ((this.onreceiveCompletedDelegate == null)) {
                this.onreceiveCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnreceiveCompleted);
            }
            base.InvokeAsync(this.onBeginreceiveDelegate, new object[] {
                        arg0}, this.onEndreceiveDelegate, this.onreceiveCompletedDelegate, userState);
        }
    }
}