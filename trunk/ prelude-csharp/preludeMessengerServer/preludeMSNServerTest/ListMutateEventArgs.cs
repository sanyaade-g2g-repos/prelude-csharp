/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 14:11
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace sharpMSN
{
	/// <summary>
	/// Description of ListMutateEventArgs.
	/// </summary>
	public class ListMutateEventArgs : System.EventArgs
	{
      // Methods
		public ListMutateEventArgs(Contact contact, MSNList affectedList)
		{
		      this.Subject = contact;
		      this.AffectedList = affectedList;
		}


      // Fields
      public MSNList AffectedList;
      public Contact Subject;

	}
}
