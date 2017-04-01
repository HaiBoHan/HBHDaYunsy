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
    // CBO.Pub.Supplier.SupplierItem
    // SupplierItemCrossUIModel.SupplierItemCrossUIFormWebPart	UFIDA.U9.CBO.Pub.SupplierItemUI.WebPart

    // CBO.Pub.Supplier.SupplierItemList
    // SupplierItemCrossBList.SupplierItemCrossBListWebPart	UFIDA.U9.CBO.Pub.SupplierItemUI.WebPart

    // CBO.Pub.Item.ItemList
    // ItemBListUIModel.ItemBListUIFormWebPart	UFIDA.U9.CBO.Pub.Item.ItemUI.WebPart
    public class ItemList_UIPlugin: ExtendedPartBase
    {
        public UFSoft.UBF.UI.IView.IPart part;
        private ItemBListUIModel.ItemBListUIFormWebPart _strongPart;


        public override void AfterInit(IPart Part, EventArgs e)
        {
            part = Part;
            _strongPart = Part as ItemBListUIModel.ItemBListUIFormWebPart;


            // Card0
            // 3
            IUFCard card0 = (IUFCard)part.GetUFControlByName(part.TopLevelContainer, "Card0");
            if (card0 != null)
            {
                IUFButton btn2DMSWhqoh = new UFWebButtonAdapter();
                btn2DMSWhqoh.Text = "下发DMS库存";
                btn2DMSWhqoh.ID = "btn2DMSWhqoh";
                btn2DMSWhqoh.AutoPostBack = true;
                btn2DMSWhqoh.Click += new EventHandler(btn2DMSWhqoh_Click);

                card0.Controls.Add(btn2DMSWhqoh);
                HBHCommon.HBHCommonUI.UICommonHelper.Layout(card0, btn2DMSWhqoh, 8, 0);

                // 确认对话框
                UFIDA.U9.UI.PDHelper.PDFormMessage.ShowConfirmDialog(_strongPart.Page, "确认同步DMS库存？", "确认同步DMS库存", btn2DMSWhqoh);
            }
        }

        public void btn2DMSWhqoh_Click(object sender, EventArgs e)
        {
            part.Model.ClearErrorMessage();
            part.DataCollect();
            part.IsConsuming = false;
            part.IsDataBinding = true;

            StoreQty2DMSSVProxy proxy = new StoreQty2DMSSVProxy();
            proxy.TransferType = (int)DaYun2DMSTransferTypeEnum.Whqoh;

            long[] selected = _strongPart.Model.ItemMaster.GetSelectedRecordIDs();

            if (selected != null
                && selected.Length > 0
                )
            {
                proxy.SupItems = selected.ToList<long>();
            }

            proxy.Do();

            HBHCommon.HBHCommonUI.UICommonHelper.ShowStatusMessage(_strongPart, "已更新DMS成功!");
        }
    }
}
