namespace UFIDA.U9.Cust.HBDY.API
{
	using System;
	using System.Collections.Generic;
	using System.Text; 
	using UFSoft.UBF.AopFrame;	
	using UFSoft.UBF.Util.Context;

	/// <summary>
	/// CreateShipSV partial 
	/// </summary>	
	public partial class CreateShipSV 
	{	
		internal BaseStrategy Select()
		{
			return new CreateShipSVImpementStrategy();	
		}		
	}
	
	#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class CreateShipSVImpementStrategy : BaseStrategy
	{
		public CreateShipSVImpementStrategy() { }

		public override object Do(object obj)
		{						
			CreateShipSV bpObj = (CreateShipSV)obj;
			
			//get business operation context is as follows
			//IContext context = ContextManager.Context	
			
			//auto generating code end,underside is user custom code
			//and if you Implement replace this Exception Code...
			throw new NotImplementedException();
		}		
	}

	#endregion
	
	
}