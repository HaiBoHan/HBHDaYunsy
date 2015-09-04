using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UFSoft.UBF.UI.Custom;
using UFSoft.UBF.UI.IView;
using UFSoft.UBF.UI.ControlModel;
using UFSoft.UBF.UI.WebControlAdapter;
using UFIDA.U9.Cust.HBH.Common.CommonHelper;
using System.Collections.Specialized;
using UFSoft.UBF.UI.ActionProcess;
using UserDefAllocRateUIModel;
using UFIDA.U9.Cust.HBH.Common.CommonLibary;
using UFSoft.UBF.UI.MD.Runtime;
using U9.VOB.Cus.HBHDaYunsy.Proxy;

namespace U9.VOB.Cus.HBHDaYunsy.UIPlugIn
{
    public class UserDefAllocRateUIPlugin: ExtendedPartBase
    {
        private const string CostAccountant = "ID";
        private IPart CostAccountantPart;
        private IUFButton btnRefresh = new UFWebButtonAdapter();
        public override void AfterInit(IPart part, EventArgs e)
        {
            this.CostAccountantPart = part;
            IUFCard card = (IUFCard)part.GetUFControlByName(part.TopLevelContainer, "Card0");
            IUFButton btnNavS = new UFWebButtonAdapter();
            btnNavS.Text = ("自动分配");
            btnNavS.ID = ("btnNavS");
            btnNavS.AutoPostBack = (true);
            this.btnRefresh.Text = ("刷新");
            this.btnRefresh.ID = ("btnRefresh");
            this.btnRefresh.AutoPostBack = (true);
            this.btnRefresh.Visible = (false);
            card.Controls.Add(btnNavS);
            card.Controls.Add(this.btnRefresh);
            UIPlugHelper.Layout(card, btnNavS, 2, 0);
            btnNavS.Click += (new EventHandler(this.btnNavS_Click));
            this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
            IUFCard carddataGrid = (IUFCard)part.GetUFControlByName(part.TopLevelContainer, "Card4");
            IUFDataGrid dataGrid = (IUFDataGrid)part.GetUFControlByName(carddataGrid, "DataGrid5");
            dataGrid.Columns[7].Point = 4;
        }
        public void btnNavS_Click(object sender, EventArgs e)
        {
            CostAccountantPart.CurrentState[UserDefAllocRateAccountUIFormWebPart.Const_UserDef_AccountPeriod] = null;
            CostAccountantPart.CurrentState[UserDefAllocRateAccountUIFormWebPart.Const_UserDef_AccountPeriod_Code] = null;
            CostAccountantPart.CurrentState[UserDefAllocRateAccountUIFormWebPart.Const_UserDef_AccountPeriod_Name] = null;

            this.CostAccountantPart.Model.ClearErrorMessage();
            IUIRecord record = this.CostAccountantPart.Model.Views["UserDefAllocRate"].FocusedRecord;
            if (record != null && record["Code"] != null)
            {
                NameValueCollection nvs = new NameValueCollection();
                //if (this.CostAccountantPart != null && this.CostAccountantPart.Model != null && this.CostAccountantPart.Model.Views.Count != 0 && this.CostAccountantPart.Model.Views != null)
                //{
                //    nvs.Add("UserID", this.CostAccountantPart.Model.Views["UserDefAllocRate"].FocusedRecord["ID"].ToString());
                //    nvs.Add("UserCode", this.CostAccountantPart.Model.Views["UserDefAllocRate"].FocusedRecord["Code"].ToString());
                //    nvs.Add("UserName", this.CostAccountantPart.Model.Views["UserDefAllocRate"].FocusedRecord["Name"].ToString());
                //}
                this.CostAccountantPart.ShowAtlasModalDialog(this.btnRefresh, "22da09ae-62d2-409d-a05b-83264d122583", "查询", "400", "200", "", nvs, false, false, false);
            }
            else
            {
                this.CostAccountantPart.Model.ErrorMessage.Message = ("编码不能为空！");
            }
        }
        public void btnRefresh_Click(object sender, EventArgs e)
        {
            IUIRecord record = this.CostAccountantPart.Model.Views["UserDefAllocRate"].FocusedRecord;
            if (record != null)
            {
                long userRateID = PubClass.GetLong(record["ID"]);
                string strUserRateCode = PubClass.GetString(record["Code"]);

                if (!PubClass.IsNull(strUserRateCode))
                {
                    long accID = PubClass.GetLong(CostAccountantPart.CurrentState[UserDefAllocRateAccountUIFormWebPart.Const_UserDef_AccountPeriod]);
                    string strAccCode = PubClass.GetString(CostAccountantPart.CurrentState[UserDefAllocRateAccountUIFormWebPart.Const_UserDef_AccountPeriod_Code]);
                    string strAccName = PubClass.GetString(CostAccountantPart.CurrentState[UserDefAllocRateAccountUIFormWebPart.Const_UserDef_AccountPeriod_Name]);

                    //GetNumAllocationDTOData getnumBP = new GetNumAllocationDTOData();
                    //if (userCode == "001")
                    //{
                    //    getnumBP.IsCarLoad = true;
                    //}
                    //else
                    //{
                    //    getnumBP.IsCarLoad = false;
                    //}
                    //getnumBP.AccountingPeriodID = this.CurrentModel.AccountingPeriodView.FocusedRecord.AccountingPeriod.GetValueOrDefault();
                    //getnumBP.UserDefAllocRateID = Convert.ToInt64(userID);

                    //new GetNumAllocationBPProxy
                    //{
                    //    InDTO = getnumBP
                    //}.Do();

                    GetNumAllocationBPProxy proxy = new GetNumAllocationBPProxy();
                    proxy.AccountPeriod = accID;
                    proxy.UserDefAllocRateID = userRateID;
                    proxy.UserRateCode = strUserRateCode;

                    proxy.Do();


                    ((BaseAction)this.CostAccountantPart.Action).NavigateAction.Refresh(null);
                }
            }
        }
    }
}
