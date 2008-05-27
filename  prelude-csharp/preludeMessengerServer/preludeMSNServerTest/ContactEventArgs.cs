/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 14:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace sharpMSN
{
	/// <summary>
	/// Description of ContactEventArgs.
	/// </summary>
	public class ContactEventArgs : System.EventArgs
	{
      // Methods
		public ContactEventArgs(Contact contact)
		{
		      this.Contact = contact;
		}
 


      // Fields
      public Contact Contact;

	}
}
