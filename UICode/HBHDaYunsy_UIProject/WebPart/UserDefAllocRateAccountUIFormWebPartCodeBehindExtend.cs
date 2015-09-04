using System;
using System.Text;
using System.Collections;
using System.Xml;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Threading;

using Telerik.WebControls;
using UFSoft.UBF.UI.WebControls;
using UFSoft.UBF.UI.Controls;
using UFSoft.UBF.Util.Log;
using UFSoft.UBF.Util.Globalization;

using UFSoft.UBF.UI.IView;
using UFSoft.UBF.UI.Engine;
using UFSoft.UBF.UI.MD.Runtime;
using UFSoft.UBF.UI.ActionProcess;
using UFSoft.UBF.UI.WebControls.ClientCallBack;



/***********************************************************************************************
 * Form ID: 
 * UIFactory Auto Generator 
 ***********************************************************************************************/
namespace UserDefAllocRateUIModel
{
    public partial class UserDefAllocRateAccountUIFormWebPart
    {
        public const string Const_UserDef_AccountPeriod = "UserDef_AccountPeriod";
        public const string Const_UserDef_AccountPeriod_Code = "UserDef_AccountPeriod_Code";
        public const string Const_UserDef_AccountPeriod_Name = "UserDef_AccountPeriod_Name";

        #region Custome eventBind
	
		 
				//BtnClose_Click...
		private void BtnClose_Click_Extend(object sender, EventArgs  e)
		{
			//调用模版提供的默认实现.--默认实现可能会调用相应的Action.


            //BtnClose_Click_DefaultImpl(sender, e);

            this.CloseDialog(false);
		}	
		 
				//BtnOk_Click...
		private void BtnOk_Click_Extend(object sender, EventArgs  e)
		{
			//调用模版提供的默认实现.--默认实现可能会调用相应的Action.
            //BtnOk_Click_DefaultImpl(sender,e);

            this.CurrentState[Const_UserDef_AccountPeriod] = null;
            this.CurrentState[Const_UserDef_AccountPeriod_Code] = null;
            this.CurrentState[Const_UserDef_AccountPeriod_Name] = null;

            MainViewRecord record = this.Model.MainView.FocusedRecord;

            if (record != null)
            {
                long accID = record.AccountPeriod.GetValueOrDefault(-1);

                if (accID > 0)
                {
                    this.CurrentState[Const_UserDef_AccountPeriod] = accID;
                    this.CurrentState[Const_UserDef_AccountPeriod_Code] = record.AccountPeriod_Code;
                    this.CurrentState[Const_UserDef_AccountPeriod_Name] = record.AccountPeriod_Name;

                    this.CloseDialog(true);
                }
            }
        }

        #endregion

		
            
            

		#region 自定义数据初始化加载和数据收集
		private void OnLoadData_Extend(object sender)
		{	
			OnLoadData_DefaultImpl(sender);
		}
		private void OnDataCollect_Extend(object sender)
		{	
			OnDataCollect_DefaultImpl(sender);
		}
		#endregion  

        #region 自己扩展 Extended Event handler 
		public void AfterOnLoad()
		{

		}

        public void AfterCreateChildControls()
        {


		
        }
        
        public void AfterEventBind()
        {
        }
        
		public void BeforeUIModelBinding()
		{

		}

		public void AfterUIModelBinding()
		{


		}


        #endregion
		
    }
}