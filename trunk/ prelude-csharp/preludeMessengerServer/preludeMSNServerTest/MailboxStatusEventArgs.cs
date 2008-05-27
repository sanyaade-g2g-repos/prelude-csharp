/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 14:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace sharpMSN
{
	/// <summary>
	/// Description of MailboxStatusEventArgs.
	/// </summary>
	public class MailboxStatusEventArgs : System.EventArgs
	{
        // Methods
		public MailboxStatusEventArgs(int inboxUnread, int foldersUnread, string inboxURL, string foldersURL, string postURL)
		{
		      this.InboxUnread = inboxUnread;
		      this.FoldersUnread = foldersUnread;
		      this.InboxURL = inboxURL;
		      this.FoldersURL = foldersURL;
		      this.PostURL = postURL;
		}

      // Fields
      public int FoldersUnread;
      public string FoldersURL;
      public int InboxUnread;
      public string InboxURL;
      public string PostURL;

	}
}
