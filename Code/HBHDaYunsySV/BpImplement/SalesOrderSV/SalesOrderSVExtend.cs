namespace UFIDA.U9.Cust.HBDY.API
{
	using System;
	using System.Collections.Generic;
	using System.Text; 
	using UFSoft.UBF.AopFrame;	
	using UFSoft.UBF.Util.Context;

	/// <summary>
	/// SalesOrderSV partial 
	/// </summary>	
	public partial class SalesOrderSV 
	{	
		internal BaseStrategy Select()
		{
			return new SalesOrderSVImpementStrategy();	
		}		
	}
	
	#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class SalesOrderSVImpementStrategy : BaseStrategy
	{
		public SalesOrderSVImpementStrategy() { }

		public override object Do(object obj)
		{						
			SalesOrderSV bpObj = (SalesOrderSV)obj;
			
			//get business operation context is as follows
			//IContext context = ContextManager.Context	
			
			//auto generating code end,underside is user custom code
			//and if you Implement replace this Exception Code...
			throw new NotImplementedException();
		}		
	}

	#endregion
	
	
}