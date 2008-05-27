/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 14:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace sharpMSN
{
	/// <summary>
	/// Description of ConversationEventArgs.
	/// </summary>
	public class ConversationEventArgs : System.EventArgs
	{
      // Methods
		public ConversationEventArgs(Conversation conversation)
		{
		      this.Conversation = conversation;
		}
 
      // Fields
      public Conversation Conversation;

	}
}
