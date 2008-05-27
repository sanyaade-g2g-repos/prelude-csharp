/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 14:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace sharpMSN
{
	/// <summary>
	/// Description of MSNErrorEventArgs.
	/// </summary>
	public class MSNErrorEventArgs
	{

		public MSNErrorEventArgs(MSNError error)
		{
		      this.Error = error;
		}
 
      // Fields
      public MSNError Error;

	}
}
