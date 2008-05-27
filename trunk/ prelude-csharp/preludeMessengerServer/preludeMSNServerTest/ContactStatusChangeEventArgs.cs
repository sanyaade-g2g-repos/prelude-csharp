/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 14:05
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace sharpMSN
{
	/// <summary>
	/// Description of ContactStatusChangeEventArgs.
	/// </summary>
	public class ContactStatusChangeEventArgs : System.EventArgs
	{
      // Methods
		public ContactStatusChangeEventArgs(Contact contact, MSNStatus oldStatus)
		{
		      this.Contact = contact;
		      this.OldStatus = oldStatus;
		}
 
      // Fields
      public Contact Contact;
      public MSNStatus OldStatus;

	}
}
