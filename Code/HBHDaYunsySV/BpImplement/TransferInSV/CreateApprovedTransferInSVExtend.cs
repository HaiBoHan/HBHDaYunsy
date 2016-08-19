namespace UFIDA.U9.Cust.HBDY.API
{
	using System;
	using System.Collections.Generic;
	using System.Text; 
	using UFSoft.UBF.AopFrame;	
	using UFSoft.UBF.Util.Context;

	/// <summary>
	/// CreateApprovedTransferInSV partial 
	/// </summary>	
	public partial class CreateApprovedTransferInSV 
	{	
		internal BaseStrategy Select()
		{
			return new CreateApprovedTransferInSVImpementStrategy();	
		}		
	}
	
	#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class CreateApprovedTransferInSVImpementStrategy : BaseStrategy
	{
		public CreateApprovedTransferInSVImpementStrategy() { }

		public override object Do(object obj)
		{						
			CreateApprovedTransferInSV bpObj = (CreateApprovedTransferInSV)obj;
			
			//get business operation context is as follows
			//IContext context = ContextManager.Context	
			
			//auto generating code end,underside is user custom code
			//and if you Implement replace this Exception Code...
			throw new NotImplementedException();
		}		
	}

	#endregion
	
	
}