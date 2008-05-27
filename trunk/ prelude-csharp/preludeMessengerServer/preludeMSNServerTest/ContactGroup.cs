/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 13:52
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace sharpMSN
{
	/// <summary>
	/// Description of ContactGroup.
	/// </summary>
	public class ContactGroup
	{
      // Fields
      public object ClientData;
      private int id;
      internal Messenger messenger;
      private string name;		
		
		  // Methods
		internal ContactGroup(string name, int id, Messenger messenger)
		{
		      this.name = name;
		      this.id = id;
		      this.messenger = messenger;
		}
		
		
		public override int GetHashCode()
		{
		      return this.id;
		}
	 


        // Properties
        public int ID
		{
		      get
		      {
		            return this.id;
		      }
		}
 

        public Messenger Messenger
		{
		      get
		      {
		            return this.messenger;
		      }
		}
		 

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
		                  this.messenger.RenameGroup(this, value);
		            }
		      }
		}
 	
	}
}
