/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 13:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Web;

namespace sharpMSN
{
	/// <summary>
	/// Description of Contact.
	/// </summary>
	public class Contact
	{
		
      // Fields
      public object ClientData;
     // private ContactBlockedHandler ContactBlocked;
      internal int contactGroup;
     // private ContactGroupChangedHandler ContactGroupChanged;
     // private ContactOfflineHandler ContactOffline;
     // private ContactOnlineHandler ContactOnline;
     // private ContactUnBlockedHandler ContactUnBlocked;
      internal string homePhone;
      public bool inList;
      internal MSNList lists;
      internal string mail;
      internal Messenger messenger;
      internal string mobilePager;
      internal string mobilePhone;
      internal string name;
     // private ScreenNameChangedHandler ScreenNameChanged;
      internal MSNStatus status;
     // private StatusChangedHandler StatusChanged;
      internal int updateVersion;
      internal string workPhone;
	      
	      
	  // Events
      public event ContactBlockedHandler ContactBlocked;
      public event ContactGroupChangedHandler ContactGroupChanged;
      public event ContactOfflineHandler ContactOffline;
      public event ContactOnlineHandler ContactOnline;
      public event ContactUnBlockedHandler ContactUnBlocked;
      public event ScreenNameChangedHandler ScreenNameChanged;
      public event StatusChangedHandler StatusChanged;
      
	// Nested Types
      public delegate void ContactBlockedHandler(Contact sender, EventArgs e);
      public delegate void ContactGroupChangedHandler(Contact sender, EventArgs e);
      public delegate void ContactOfflineHandler(Contact sender, StatusChangeEventArgs e);
      public delegate void ContactOnlineHandler(Contact sender, EventArgs e);
      public delegate void ContactUnBlockedHandler(Contact sender, EventArgs e);
      public delegate void ScreenNameChangedHandler(Contact sender, EventArgs e);
      public delegate void StatusChangedHandler(Contact sender, StatusChangeEventArgs e);
	      
	      
		public Contact()
		{	
		      this.status = MSNStatus.Offline;
		      this.updateVersion = 0;
		      this.inList = false;
			
		}
		
		internal void AddToList(MSNList list)
		{
		      if ((list == MSNList.BlockedList) && !this.Blocked)
		      {
		            this.lists |= MSNList.BlockedList;
		            this.ContactBlocked(this, new EventArgs());
		      }
		      else
		      {
		            this.lists |= list;
		      }
		}
		
		public override int GetHashCode()
		{
		      return this.mail.GetHashCode();
		}

		public void RemoveFromList()
		{
		      if (this.messenger != null)
		      {
		            this.messenger.RemoveContact(this);
		      }
		}
		
		internal void RemoveFromList(MSNList list)
		{
		      if ((list == MSNList.BlockedList) && this.Blocked)
		      {
		            this.lists ^= MSNList.BlockedList;
		            this.ContactUnBlocked(this, new EventArgs());
		      }
		      else
		      {
		            this.lists ^= list;
		      }
		}
 
		internal void SetContactGroup(int newGroup)
		{
		      if (this.contactGroup != newGroup)
		      {
		            this.contactGroup = newGroup;
		            if (this.ContactGroupChanged != null)
		            {
		                  this.ContactGroupChanged(this, new EventArgs());
		            }
		      }
		}
		
		internal void SetName(string newName)
		{
		      if (this.name != newName)
		      {
		            this.name = HttpUtility.UrlDecode(newName);
		            if (this.ScreenNameChanged != null)
		            {
		                  this.ScreenNameChanged(this, new EventArgs());
		            }
		      }
		}
		
		internal void SetStatus(MSNStatus newStatus)
		{
		      if (this.status != newStatus)
		      {
		            MSNStatus status1 = this.status;
		            this.status = newStatus;
		            if (this.StatusChanged != null)
		            {
		                  this.StatusChanged(this, new StatusChangeEventArgs(status1));
		            }
		            if ((status1 == MSNStatus.Offline) && (this.ContactOnline != null))
		            {
		                  this.ContactOnline(this, new EventArgs());
		            }
		            if ((newStatus == MSNStatus.Offline) && (this.ContactOffline != null))
		            {
		                  this.ContactOffline(this, new StatusChangeEventArgs(status1));
		            }
		      }
		}
		
		
		public void UpdateScreenName()
		{
		      if (this.messenger == null)
		      {
		            throw new MSNException("No valid messenger object");
		      }
		      this.messenger.RequestScreenName(this);
		}
		
		
	  // Properties
      public bool Blocked
		{
		      get
		      {
		            return ((this.lists & MSNList.BlockedList) > ((MSNList) 0));
		      }
		      set
		      {
		            if (this.messenger != null)
		            {
		                  if (value)
		                  {
		                        this.messenger.BlockContact(this);
		                  }
		                  else
		                  {
		                        this.messenger.UnBlockContact(this);
		                  }
		            }
		      }
		}

	  public ContactGroup ContactGroup
		{
		      get
		      {
		            if (this.messenger != null)
		            {
		                  return (ContactGroup) this.messenger.ContactGroups[this.contactGroup];
		            }
		            return null;
		      }
		      set
		      {
		            if (this.messenger != null)
		            {
		                  this.messenger.ChangeGroup(this, value);
		            }
		      }
		}
				 
	  public string HomePhone
	  {
		      get
		      {
		            return this.homePhone;
		      }
	  }
		
		public string Mail
		{
		      get
		      {
		            return this.mail;
		      }
		}
 
		public string MobilePager
		{
		      get
		      {
		            return this.mobilePager;
		      }
		}

		public string MobilePhone
		{
		      get
		      {
		            return this.mobilePhone;
		      }
		}
 
		public string Name
		{
		      get
		      {
		            return this.name;
		      }
		}
 
		public bool OnAllowedList
		{
		      get
		      {
		            return ((this.lists & MSNList.AllowedList) > ((MSNList) 0));
		      }
		      set
		      {
		            if (value != this.OnAllowedList)
		            {
		                  if (value)
		                  {
		                        this.messenger.AddToList(this, MSNList.AllowedList);
		                  }
		                  else
		                  {
		                        this.messenger.RemoveFromList(this, MSNList.AllowedList);
		                  }
		            }
		      }
		}
				 
		public bool OnBlockedList
		{
		      get
		      {
		            return ((this.lists & MSNList.BlockedList) > ((MSNList) 0));
		      }
		      set
		      {
		            if (value != this.OnBlockedList)
		            {
		                  if (value)
		                  {
		                        this.messenger.AddToList(this, MSNList.BlockedList);
		                  }
		                  else
		                  {
		                        this.messenger.RemoveFromList(this, MSNList.BlockedList);
		                  }
		            }
		      }
		}
		 
		public bool OnForwardList
		{
		      get
		      {
		            return ((this.lists & MSNList.ForwardList) > ((MSNList) 0));
		      }
		      set
		      {
		            if (value != this.OnForwardList)
		            {
		                  if (value)
		                  {
		                        this.messenger.AddToList(this, MSNList.ForwardList);
		                  }
		                  else
		                  {
		                        this.messenger.RemoveFromList(this, MSNList.ForwardList);
		                  }
		            }
		      }
		}
 
		public bool OnReverseList
		{
		      get
		      {
		            return ((this.lists & MSNList.ReverseList) > ((MSNList) 0));
		      }
		}		 

		public MSNStatus Status
		{
		      get
		      {
		            return this.status;
		      }
		}
 
		public string WorkPhone
		{
		      get
		      {
		            return this.workPhone;
		      }
		}
 

	}
}
