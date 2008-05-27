/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 14:23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace sharpMSN
{
	/// <summary>
	/// Description of Owner.
	/// </summary>
	public class Owner : Contact
	{
      // Methods
		public Owner(string pMail, string pPassword)
		{
		      this.privacy = MSNPrivacy.Unknown;
		      this.notifyPrivacy = MSNNotifyPrivacy.Unknown;
		      this.mail = pMail;
		      this.password = pPassword;
		}
 
        // Properties
		public string Name
		{
		      get
		      {
		            return this.name;
		      }
		      set
		      {
		            if (this.messenger != null)
		            {
		                  this.messenger.ChangeScreenName(value);
		            }
		      }
		}
		 
		public MSNNotifyPrivacy NotifyPrivacy
		{
		      get
		      {
		            return this.notifyPrivacy;
		      }
		      set
		      {
		            if (this.messenger != null)
		            {
		                  this.messenger.SetNotifyPrivacy(value);
		            }
		      }
		}
		 
		public string Password
		{
		      get
		      {
		            return this.password;
		      }
		      set
		      {
		            this.password = value;
		      }
		}
		
		public MSNPrivacy Privacy
		{
		      get
		      {
		            return this.privacy;
		      }
		      set
		      {
		            if (this.messenger != null)
		            {
		                  this.messenger.SetPrivacy(value);
		            }
		      }
		}
		 
		public MSNStatus Status
		{
		      get
		      {
		            return this.status;
		      }
		      set
		      {
		            if (this.messenger != null)
		            {
		                  this.messenger.SetStatus(value);
		            }
		      }
		}
		 
      // Fields
      internal MSNNotifyPrivacy notifyPrivacy;
      private string password;
      internal MSNPrivacy privacy;

	}
}
