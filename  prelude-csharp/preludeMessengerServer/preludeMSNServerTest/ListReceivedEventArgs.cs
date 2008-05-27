/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 14:12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace sharpMSN
{
	/// <summary>
	/// Description of ListReceivedEventArgs.
	/// </summary>
	public class ListReceivedEventArgs : System.EventArgs
	{
      // Methods
		public ListReceivedEventArgs(MSNList affectedList)
		{
		      this.AffectedList = affectedList;
		}
		 
      // Fields
      public MSNList AffectedList;

	}
}
