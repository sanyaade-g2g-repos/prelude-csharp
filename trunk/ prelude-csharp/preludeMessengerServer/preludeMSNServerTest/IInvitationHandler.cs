/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 14:10
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace sharpMSN
{
	/// <summary>
	/// Description of IInvitationHandler.
	/// </summary>
	internal interface IInvitationHandler
	{
	      // Methods
	      void HandleMessage(Conversation conversation, Contact sender, string applicationName, int cookie, string header, string body);
	}
 
}
