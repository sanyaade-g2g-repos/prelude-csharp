/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 14:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace sharpMSN
{
	/// <summary>
	/// Description of MessageEventArgs.
	/// </summary>
	public class MessageEventArgs
	{
      // Methods
		public MessageEventArgs(Message message, Contact sender)
		{
		      this.Message = message;
		      this.Sender = sender;
		}
 
      // Fields
      public Message Message;
      public Contact Sender;

	}
}
