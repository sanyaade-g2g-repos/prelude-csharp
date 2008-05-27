/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 13:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Net.Sockets;

namespace sharpMSN
{
	/// <summary>
	/// Description of ConnectionErrorEventArgs.
	/// </summary>
	public class ConnectionErrorEventArgs : System.EventArgs
	{
		public SocketException Error;
 

		public ConnectionErrorEventArgs()
		{
		}
		
		public ConnectionErrorEventArgs(SocketException error)
		{
		      this.Error = error;
		}

	}
}
