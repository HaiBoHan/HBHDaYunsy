namespace U9.VOB.Cus.HBHDaYunsy
{
	using System;
	using System.Collections.Generic;
	using System.Text; 
	using UFSoft.UBF.AopFrame;	
	using UFSoft.UBF.Util.Context;
    //using UFIDA.U9.SPR.SalePriceList;
    using UFSoft.UBF.PL;

	/// <summary>
	/// PriceList2DMSSV partial 
	/// </summary>	
	public partial class PriceList2DMSSV 
	{	
		internal BaseStrategy Select()
		{
			return new PriceList2DMSSVImpementStrategy();	
		}		
	}
	
	#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class PriceList2DMSSVImpementStrategy : BaseStrategy
	{
		public PriceList2DMSSVImpementStrategy() { }

		public override object Do(object obj)
		{						
			PriceList2DMSSV bpObj = (PriceList2DMSSV)obj;
			
			//get business operation context is as follows
			//IContext context = ContextManager.Context	
			
			//auto generating code end,underside is user custom code
			//and if you Implement replace this Exception Code...
            //throw new NotImplementedException();


            //SalePriceList pricelist = SalePriceList.Finder.Find("Code=@Code"
            //    , new OqlParam(UFIDA.U9.Cust.HBDY.API.HBHCommon.Const_ElectricPartPriceListCode)
            //    );

            //if (pricelist != null)
            //{ 
            //    // PI06
            //}

            return null;
		}		
	}

	#endregion
	
	
}