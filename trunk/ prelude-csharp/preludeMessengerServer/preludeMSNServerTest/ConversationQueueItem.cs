/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 14:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace sharpMSN
{
	/// <summary>
	/// Description of ConversationQueueItem.
	/// </summary>
	internal class ConversationQueueItem
	{
      // Methods
		public ConversationQueueItem(string mail, object clientData)
		{
		      this.Mail = mail;
		      this.ClientData = clientData;
		}
 
      // Fields
      public object ClientData;
      public string Mail;

	}
}
