/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 14:01
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace sharpMSN
{
	/// <summary>
	/// Description of ContactChangeEventArgs.
	/// </summary>
	public class ContactChangeEventArgs : System.EventArgs
	{
      // Methods
		public ContactChangeEventArgs(Contact contact)
		{
		      this.Contact = contact;
		}
 
      // Fields
      public Contact Contact;
	}
}
