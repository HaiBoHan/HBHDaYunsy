﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.36366
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace U9.VOB.Cus.HBHDaYunsy.PlugInBE.Test.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://testserver/U9/hbhservices/U9.VOB.HBHCommon.IHBHCommonSVForJava.svc")]
        public string U9_VOB_Cus_HBHDaYunsy_PlugInBE_Test_U9CommonSV_HBHCommonSVForJavaStub {
            get {
                return ((string)(this["U9_VOB_Cus_HBHDaYunsy_PlugInBE_Test_U9CommonSV_HBHCommonSVForJavaStub"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://scisoft.eicp.net:9080/dms/ws/PI06")]
        public string U9_VOB_Cus_HBHDaYunsy_PlugInBE_Test_DMS_PI06_PI06ImplService {
            get {
                return ((string)(this["U9_VOB_Cus_HBHDaYunsy_PlugInBE_Test_DMS_PI06_PI06ImplService"]));
            }
        }
    }
}