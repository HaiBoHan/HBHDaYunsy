﻿using System;
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
using UFIDA.U9.SPR.SalePriceAdjustmentBListUI;

namespace U9.VOB.Cus.HBHDaYunsy.UIPlugIn
{
    // CBO.SCM.MF5040_20
    // UFIDA.U9.SPR.SalePriceAdjustmentBListUI.SalePriceAdjustmentBListUIFormWebPart	UFIDA.U9.SCM.SD.SalePriceAdjustmentUI.WebPart
    public class SalePriceAdjustmentBList_UIPlugin : ExtendedPartBase
    {
        public UFSoft.UBF.UI.IView.IPart part;
        SalePriceAdjustmentBListUIFormWebPart _strongPart;


        public override void AfterInit(IPart Part, EventArgs e)
        {
            part = Part;
            _strongPart = Part as SalePriceAdjustmentBListUIFormWebPart;


            // Card0
            // 3
            IUFCard card0 = (IUFCard)part.GetUFControlByName(part.TopLevelContainer, "Card0");
            if (card0 != null)
            {
                IUFButton btn2DMS = new UFWebButtonAdapter();
                btn2DMS.Text = "下发DMS";
                btn2DMS.ID = "btn2DMS";
                btn2DMS.AutoPostBack = true;
                btn2DMS.Click += new EventHandler(btn2DMS_Click);

                card0.Controls.Add(btn2DMS);
                HBHCommon.HBHCommonUI.UICommonHelper.Layout(card0, btn2DMS, 8, 0);

                // 确认对话框
                UFIDA.U9.UI.PDHelper.PDFormMessage.ShowConfirmDialog(_strongPart.Page, "确认同步DMS？", "确认同步DMS", btn2DMS);
            }
        }

        public void btn2DMS_Click(object sender, EventArgs e)
        {
            part.Model.ClearErrorMessage();
            part.DataCollect();
            part.IsConsuming = false;
            part.IsDataBinding = true;

            StoreQty2DMSSVProxy proxy = new StoreQty2DMSSVProxy();
            proxy.TransferType = (int)DaYun2DMSTransferTypeEnum.SalePriceAdjustment;

            long[] selected = _strongPart.Model.SalePriceAdjustLine.GetSelectedRecordIDs();

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
