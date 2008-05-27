/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 14:04
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace sharpMSN
{
	/// <summary>
	/// Description of ContactGroupEventArgs.
	/// </summary>
	public class ContactGroupEventArgs : System.EventArgs
	{
		public ContactGroupEventArgs(ContactGroup contactGroup)
		{
		      this.ContactGroup = contactGroup;
		}

      // Fields
      public ContactGroup ContactGroup;

	}
}
