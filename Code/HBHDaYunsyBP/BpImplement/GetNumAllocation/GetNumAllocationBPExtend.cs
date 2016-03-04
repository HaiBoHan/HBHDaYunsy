
namespace U9.VOB.Cus.HBHDaYunsy
{
	using System;
	using System.Collections.Generic;
	using System.Text; 
	using UFSoft.UBF.AopFrame;	
	using UFSoft.UBF.Util.Context;
    using UFIDA.U9.CA.DateData;
    using UFSoft.UBF.PL;
    using UFSoft.UBF.Business;
    using UFIDA.U9.Base.Account;
    using UFIDA.U9.CA.BaseData;
    using UFIDA.U9.CBO.HR.Department;
    using UFIDA.U9.CBO.MFG.ProductionLine;
    using UFIDA.U9.CBO.MFG.WorkCenter;
    using UFIDA.U9.Base;

	/// <summary>
	/// GetNumAllocationBP partial 
	/// </summary>	
	public partial class GetNumAllocationBP 
	{	
		internal BaseStrategy Select()
		{
			return new GetNumAllocationBPImpementStrategy();	
		}		
	}
	
    //#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class GetNumAllocationBPImpementStrategy : BaseStrategy
	{
		public GetNumAllocationBPImpementStrategy() { }

		public override object Do(object obj)
		{						
			GetNumAllocationBP bpObj = (GetNumAllocationBP)obj;
			
			//get business operation context is as follows
			//IContext context = ContextManager.Context	
			
			//auto generating code end,underside is user custom code
			//and if you Implement replace this Exception Code...
            //throw new NotImplementedException();

            if (bpObj != null)
            {
                OutputQty.EntityList qtylist = OutputQty.Finder.FindAll(" Org = @Org and  AccountingPeriod = " + bpObj.AccountPeriod + "", new OqlParam(Context.LoginOrg.ID));
                UserDefAllocRate rate = UserDefAllocRate.Finder.Find("ID = " + bpObj.UserDefAllocRateID + "", new OqlParam[0]);
                UserDefAllocRatebyobject.EntityList userobj = UserDefAllocRatebyobject.Finder.FindAll("UserDefAllocRate = " + bpObj.UserDefAllocRateID + "", new OqlParam[0]);
                AccountPeriod period = AccountPeriod.Finder.Find("ID = " + bpObj.AccountPeriod, new OqlParam[0]);
                if (rate != null)
                {
                    using (Session session = Session.Open())
                    {
                        if (userobj != null && userobj.Count != 0)
                        {
                            for (int a = userobj.Count - 1; a >= 0; a--)
                            {
                                userobj[a].Remove();
                            }
                        }
                        if (qtylist != null && qtylist.Count != 0)
                        {
                            foreach (OutputQty qty in qtylist)
                            {
                                if (!(qty.ReceiptQty == 0) && qty.CostField != null)
                                {
                                    /*
                                    if (userCode == "001")
                                    {
                                        getnumBP.IsCarLoad = true;
                                    }
                                    else
                                    {
                                        getnumBP.IsCarLoad = false;
                                    }
                                     */
                                    //if (!bpObj.IsCarLoad)
                                    if (bpObj.UserRateCode != "001")
                                    {
                                        StdWorkingHours.EntityList lststd = StdWorkingHours.Finder.FindAll(" Org = @Org and ItemMaster = " + qty.ItemMaster.ID,  new OqlParam(Context.LoginOrg.ID));
                                        if (lststd.Count != 0)
                                        {
                                            foreach (StdWorkingHours std in lststd)
                                            {
                                                if (!(std.RunLaborHours == 0))
                                                {
                                                    if (std.ToDate > period.ToDate)
                                                    {
                                                        //if (qty.Department.Code == "1030")
                                                        //{
                                                        //    UserDefAllocRatebyobject ject = UserDefAllocRatebyobject.Create(rate);
                                                        //    //ject.Department = (new Department());
                                                        //    if (qty.DepartmentKey != null)
                                                        //    {
                                                        //        ject.DepartmentKey = (qty.DepartmentKey);
                                                        //    }
                                                        //    ject.CostFieldKey = (qty.CostFieldKey);
                                                        //    ject.ProductionLine = (new ProductionLine());
                                                        //    if (qty.ProductionLineKey != null)
                                                        //    {
                                                        //        ject.ProductionLineKey = (qty.ProductionLineKey);
                                                        //    }
                                                        //    ject.WorkCenter = (new WorkCenter());
                                                        //    if (qty.WorkCenterKey != null)
                                                        //    {
                                                        //        ject.WorkCenterKey = (qty.WorkCenterKey);
                                                        //    }
                                                        //    ject.MO = (qty.MO);
                                                        //    ject.PLS = (qty.PLS);
                                                        //    ject.ProductKey = (qty.ProductKey);
                                                        //    ject.DriverValue = (std.RunLaborHours * qty.ReceiptQty);
                                                        //}
                                                        //else 
                                                        if (qty.MODocType.Value != 4 && qty.MODocType.Value != 3)
                                                        {
                                                            UserDefAllocRatebyobject ject = UserDefAllocRatebyobject.Create(rate);
                                                            //ject.Department = (new Department());
                                                            if (qty.DepartmentKey != null)
                                                            {
                                                                ject.DepartmentKey = (qty.DepartmentKey);
                                                            }
                                                            ject.CostFieldKey = (qty.CostFieldKey);
                                                            //ject.ProductionLine = (new ProductionLine());
                                                            if (qty.ProductionLineKey != null)
                                                            {
                                                                ject.ProductionLineKey = (qty.ProductionLineKey);
                                                            }
                                                            ject.WorkCenter = (new WorkCenter());
                                                            if (qty.WorkCenterKey != null)
                                                            {
                                                                ject.WorkCenterKey = (qty.WorkCenterKey);
                                                            }
                                                            ject.MO = (qty.MO);
                                                            ject.PLS = (qty.PLS);
                                                            ject.ProductKey = (qty.ProductKey);
                                                            ject.DriverValue = (std.RunLaborHours * qty.ReceiptQty);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (qty.MODocType.Value != 4 && qty.MODocType.Value != 3)
                                    {
                                        UserDefAllocRatebyobject ject = UserDefAllocRatebyobject.Create(rate);
                                        //ject.Department = (new Department());
                                        if (qty.DepartmentKey != null)
                                        {
                                            ject.DepartmentKey = (qty.DepartmentKey);
                                        }
                                        ject.CostFieldKey = (qty.CostFieldKey);
                                        //ject.ProductionLine = (new ProductionLine());
                                        if (qty.ProductionLineKey != null)
                                        {
                                            ject.ProductionLineKey = (qty.ProductionLineKey);
                                        }
                                        //ject.WorkCenter = (new WorkCenter());
                                        if (qty.WorkCenterKey != null)
                                        {
                                            ject.WorkCenterKey = (qty.WorkCenterKey);
                                        }
                                        ject.MO = (qty.MO);
                                        ject.PLS = (qty.PLS);
                                        ject.ProductKey = (qty.ProductKey);
                                        ject.DriverValue = (qty.ReceiptQty);
                                    }
                                }
                            }
                        }
                        session.Commit();
                    }
                }
            }
            return null;
		}		
	}

    //#endregion
	
	
}