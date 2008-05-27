/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 14:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace sharpMSN
{
	/// <summary>
	/// Description of NewMailReceivedEventArgs.
	/// </summary>
	public class NewMailReceivedEventArgs : System.EventArgs
	{
		public NewMailReceivedEventArgs(string from, string messageURL, string subject, string destinationFolder, string fromEmail, int id)
		{
		      this.From = from;
		      this.MessageURL = messageURL;
		      this.Subject = subject;
		      this.DestinationFolder = destinationFolder;
		      this.FromEmail = fromEmail;
		      this.ID = id;
		}
 	
	   // Fields
      public string DestinationFolder;
      public string From;
      public string FromEmail;
      public int ID;
      public string MessageURL;
      public string Subject;

	}
}
