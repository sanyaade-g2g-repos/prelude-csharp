/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 14:29
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace sharpMSN
{
	/// <summary>
	/// Description of StatusChangeEventArgs.
	/// </summary>
	public class StatusChangeEventArgs : System.EventArgs
	{
      // Methods
		public StatusChangeEventArgs(MSNStatus oldStatus)
		{
		      this.OldStatus = oldStatus;
		}
 
      // Fields
      public MSNStatus OldStatus;

	}
}
