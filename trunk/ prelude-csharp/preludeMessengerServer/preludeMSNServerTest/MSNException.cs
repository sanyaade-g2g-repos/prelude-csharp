/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 14:19
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace sharpMSN
{
	/// <summary>
	/// Description of MSNException.
	/// </summary>
	public class MSNException : System.Exception
	{
      // Methods
		public MSNException(string message) : base(message)
		{
		      this.errorID = -1;
		}
		 
		public MSNException(string message, Exception innerException) : base(message, innerException)
		{
		      this.errorID = -1;
		}
 
		public MSNException(string message, int ID) : base(message)
		{
		      this.errorID = -1;
		      this.errorID = ID;
		}
 

      // Fields
      private int errorID;

	}
}
