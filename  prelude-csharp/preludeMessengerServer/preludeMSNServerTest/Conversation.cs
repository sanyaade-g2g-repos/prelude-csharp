/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 14:33
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using System.Web;
using System.Collections;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace sharpMSN
{
	/// <summary>
	/// Description of Conversation.
	/// </summary>
	public class Conversation
	{
		
// Fields
     // private AllContactsLeftHandler AllContactsLeft;
      private static Regex APPRe;
      private static Regex BYERe;
      private static Regex CALRe;
      public object ClientData;
      private static Regex commandRe;
     // public ConnectionClosedHandler ConnectionClosed;
     // private ConnectionEstablishedHandler ConnectionEstablished;
     // private ContactJoinHandler ContactJoin;
     // private ContactLeaveHandler ContactLeave;
      private Contact contactToCall;
      private static Regex CookieRe;
      private static Regex CTRe;
      private string hash;
      private static Regex IRORe;
      private static Regex JOIRe;
      private static Regex messageRe;
     // private MessageReceivedHandler MessageReceived;
      private Messenger messenger;
      private static Regex MSGRe;
      private int sessionID;
      private byte[] socketBuffer;
      private Socket socketSB;
      private int transaction;
      private static Regex TypingRe;
      private Hashtable users;
     // private UserTypingHandler UserTyping;		
// Events
      public event AllContactsLeftHandler AllContactsLeft;
      public event ConnectionClosedHandler ConnectionClosed;
      public event ConnectionEstablishedHandler ConnectionEstablished;
      public event ContactJoinHandler ContactJoin;
      public event ContactLeaveHandler ContactLeave;
      public event MessageReceivedHandler MessageReceived;
      public event UserTypingHandler UserTyping;

      // Methods
		static Conversation()
		{
		      Conversation.commandRe = new Regex(@"^(?<Command>[A-Z0-9]{3})\s?(?<Transaction>[0-9]+)?\s?(?<Message>.*?)$", RegexOptions.Compiled | RegexOptions.Multiline);
		      Conversation.messageRe = new Regex(@"^MSG\s+(?<Mail>\S+)\s+(?<Name>.*?)\s+(?<Length>[0-9]+)$", RegexOptions.Compiled | RegexOptions.Multiline);
		      Conversation.CALRe = new Regex(@"^RINGING\s+(?<sessionID>[0-9]+)$");
		      Conversation.MSGRe = new Regex(@"^(?<Mail>[\S]+)(?<Name>.*?)(?<Length>[0-9]+)$", RegexOptions.Compiled | RegexOptions.Multiline);
		      Conversation.JOIRe = new Regex(@"^JOI\s+(?<Mail>\S+)\s+(?<Name>\S+)$", RegexOptions.Compiled);
		      Conversation.IRORe = new Regex(@"^IRO\s+\d+\s+\d+\s+\d+\s+(?<Mail>\S+)\s+(?<Name>\S+)$", RegexOptions.Compiled);
		      Conversation.BYERe = new Regex(@"^BYE\s+(?<Mail>\S+)$", RegexOptions.Compiled);
		      Conversation.TypingRe = new Regex(@"TypingUser:\s+(?<Mail>[\w_\.@]+)", RegexOptions.Compiled | RegexOptions.Multiline);
		      Conversation.CTRe = new Regex("Content-Type: text/x-msmsgsinvite", RegexOptions.Compiled | RegexOptions.Multiline);
		      Conversation.APPRe = new Regex("Application-Name: (?<Application>.*?)", RegexOptions.Compiled | RegexOptions.Multiline);
		      Conversation.CookieRe = new Regex(@"Invitation-Cookie: (?<Cookie>\d+)", RegexOptions.Compiled | RegexOptions.Multiline);
		}
		 
		internal Conversation(Messenger msn, Contact otherUser, object clientData)
		{
		      this.users = new Hashtable();
		      this.transaction = 0;
		      this.sessionID = 0;
		      this.ClientData = null;
		      this.socketBuffer = new byte[0x1000];
		      this.messenger = msn;
		      this.contactToCall = otherUser;
		      this.ClientData = clientData;
		}
		
		internal Conversation(Socket switchBoard, string sbHash, Contact otherUser, Messenger msn, int session)
		{
		      this.users = new Hashtable();
		      this.transaction = 0;
		      this.sessionID = 0;
		      this.ClientData = null;
		      this.socketBuffer = new byte[0x1000];
		      this.contactToCall = otherUser;
		      this.hash = sbHash;
		      this.socketSB = switchBoard;
		      this.messenger = msn;
		      this.sessionID = session;
		}
		
		public void Close()
		{
		      if (this.socketSB.Connected)
		      {
		            this.socketSB.Shutdown(SocketShutdown.Both);
		            this.socketSB.Close();
		            if (this.ConnectionClosed != null)
		            {
		                  this.ConnectionClosed(this, new EventArgs());
		            }
		      }
		}
		 
		private void DataReceivedCallback(IAsyncResult ar)
		{
		      int num1 = this.socketSB.EndReceive(ar);
		      if (num1 <= 0)
		      {
		            this.Close();
		      }
		      else
		      {
		            int num2;
		            string text1 = this.messenger.TextEncoding.GetString(this.socketBuffer);
		            string text2 = "";
		            while ((num2 = text1.IndexOf("\r\n")) > 0)
		            {
		                  Match match1;
		                  string text5;
		                  text2 = text1.Substring(0, num2);
		                  text1 = text1.Substring(num2 + 2);
		                  if (!(match1 = Conversation.commandRe.Match(text2)).Success)
		                  {
		                        continue;
		                  }
		                  if ((text5 = match1.Groups["Command"].ToString()) == null)
		                  {
		                        continue;
		                  }
		                  text5 = string.IsInterned(text5);
		                  if (text5 != "USR")
		                  {
		                        if (text5 == "CAL")
		                        {
		                              goto Label_0145;
		                        }
		                        if (text5 == "JOI")
		                        {
		                              goto Label_0195;
		                        }
		                        if (text5 == "BYE")
		                        {
		                              goto Label_0235;
		                        }
		                        if (text5 == "IRO")
		                        {
		                              goto Label_02F6;
		                        }
		                        if (text5 == "MSG")
		                        {
		                              goto Label_0396;
		                        }
		                        continue;
		                  }
		                  if (match1.Groups["Message"].ToString().IndexOf("OK") < 0)
		                  {
		                        continue;
		                  }
		                  if (this.ConnectionEstablished != null)
		                  {
		                        this.ConnectionEstablished(this, new EventArgs());
		                  }
		                  this.Invite(this.contactToCall);
		                  continue;
		            Label_0145:
		                  if ((match1 = Conversation.CALRe.Match(match1.Groups["Message"].ToString())).Success)
		                  {
		                        this.sessionID = int.Parse(match1.Groups["sessionID"].ToString());
		                  }
		                  continue;
		            Label_0195:
		                  if (!(match1 = Conversation.JOIRe.Match(text2)).Success)
		                  {
		                        continue;
		                  }
		                  Contact contact1 = this.Messenger.GetContact(match1.Groups["Mail"].ToString());
		                  contact1.SetName(match1.Groups["Name"].ToString());
		                  if (!this.users.Contains(contact1.Mail))
		                  {
		                        this.users.Add(contact1.Mail, contact1);
		                  }
		                  if (this.ContactJoin != null)
		                  {
		                        this.ContactJoin(this, new ContactEventArgs(contact1));
		                  }
		                  continue;
		            Label_0235:
		                  if (!(match1 = Conversation.BYERe.Match(text2)).Success || !this.users.ContainsKey(match1.Groups["Mail"].ToString()))
		                  {
		                        continue;
		                  }
		                  Contact contact2 = (Contact) this.users[match1.Groups["Mail"].ToString()];
		                  this.users.Remove(contact2.Mail);
		                  if (this.ContactLeave != null)
		                  {
		                        this.ContactLeave(this, new ContactEventArgs(contact2));
		                  }
		                  if ((this.users.Count == 0) && (this.AllContactsLeft != null))
		                  {
		                        this.AllContactsLeft(this, new EventArgs());
		                  }
		                  continue;
		            Label_02F6:
		                  if (!(match1 = Conversation.IRORe.Match(text2)).Success)
		                  {
		                        continue;
		                  }
		                  Contact contact3 = this.Messenger.GetContact(match1.Groups["Mail"].ToString());
		                  contact3.SetName(match1.Groups["Name"].ToString());
		                  if (!this.users.Contains(contact3.Mail))
		                  {
		                        this.users.Add(contact3.Mail, contact3);
		                  }
		                  if (this.ContactJoin != null)
		                  {
		                        this.ContactJoin(this, new ContactEventArgs(contact3));
		                  }
		                  continue;
		            Label_0396:
		                  if ((match1 = Conversation.messageRe.Match(text2)).Success)
		                  {
		                        string text4;
		                        int num3 = int.Parse(match1.Groups["Length"].ToString());
		                        text2 = text1.Substring(0, num3);
		                        text1 = text1.Substring(num3);
		                        if (Conversation.TypingRe.Match(text2).Success)
		                        {
		                              if (this.UserTyping != null)
		                              {
		                                    this.UserTyping(this, new ContactEventArgs(this.messenger.GetContact(match1.Groups["Mail"].ToString())));
		                              }
		                              continue;
		                        }
		                        int num4 = text2.IndexOf("\r\n\r\n");
		                        string text3 = text2.Substring(0, num4);
		                        if (num4 > 0)
		                        {
		                              text4 = text2.Substring(num4 + 4);
		                        }
		                        else
		                        {
		                              text4 = "";
		                        }
		                        if (Conversation.CTRe.Match(text3).Success)
		                        {
		                              Conversation.APPRe.Match(text4);
		                              int num5 = int.Parse(Conversation.CookieRe.Match(text4).Groups["Cookie"].ToString());
		                              continue;
		                        }
		                        Message message1 = new Message(text4, text3);
		                        message1.ParseHeader();
		                        if (this.MessageReceived != null)
		                        {
		                              this.MessageReceived(this, new MessageEventArgs(message1, this.messenger.GetContact(match1.Groups["Mail"].ToString())));
		                        }
		                  }
		            }
		            this.socketBuffer = new byte[this.socketBuffer.Length];
		            if (this.socketSB.Connected)
		            {
		                  this.socketSB.BeginReceive(this.socketBuffer, 0, this.socketBuffer.Length, SocketFlags.None, new AsyncCallback(this.DataReceivedCallback), new object());
		            }
		      }
		}
      
		~Conversation()
		{
		      if (this.socketSB.Connected)
		      {
		            this.socketSB.Shutdown(SocketShutdown.Both);
		            this.socketSB.Close();
		      }
		}
 

		public override int GetHashCode()
		{
		      return this.hash.GetHashCode();
		}
		 
		internal void InitiateAnswer()
		{
		      int num1;
		      if (!this.socketSB.Connected)
		      {
		            throw new MSNException("Switchboard socket not connected when setting up chat connection");
		      }
		      object[] objArray1 = new object[9];
		      objArray1[0] = "ANS ";
		      this.transaction = num1 = this.transaction + 1;
		      objArray1[1] = num1;
		      objArray1[2] = " ";
		      objArray1[3] = this.messenger.Owner.Mail;
		      objArray1[4] = " ";
		      objArray1[5] = this.hash;
		      objArray1[6] = " ";
		      objArray1[7] = this.sessionID;
		      objArray1[8] = "\r\n";
		      this.socketSB.Send(this.messenger.TextEncoding.GetBytes(string.Concat(objArray1)));
		      this.socketSB.BeginReceive(this.socketBuffer, 0, this.socketBuffer.Length, SocketFlags.None, new AsyncCallback(this.DataReceivedCallback), new object());
		}
		 
		internal void InitiateRequest()
		{
		      int num1;
		      if (!this.socketSB.Connected)
		      {
		            throw new MSNException("Switchboard socket not connected when setting up chat connection");
		      }
		      object[] objArray1 = new object[7];
		      objArray1[0] = "USR ";
		      this.transaction = num1 = this.transaction + 1;
		      objArray1[1] = num1;
		      objArray1[2] = " ";
		      objArray1[3] = this.messenger.Owner.Mail;
		      objArray1[4] = " ";
		      objArray1[5] = this.hash;
		      objArray1[6] = "\r\n";
		      this.socketSB.Send(this.messenger.TextEncoding.GetBytes(string.Concat(objArray1)));
		      this.socketSB.BeginReceive(this.socketBuffer, 0, this.socketBuffer.Length, SocketFlags.None, new AsyncCallback(this.DataReceivedCallback), new object());
		}
		 
		public void Invite(Contact contact)
		{
		      int num1;
		      object[] objArray1 = new object[5];
		      objArray1[0] = "CAL ";
		      this.transaction = num1 = this.transaction + 1;
		      objArray1[1] = num1;
		      objArray1[2] = " ";
		      objArray1[3] = contact.Mail;
		      objArray1[4] = "\r\n";
		      this.socketSB.Send(this.messenger.TextEncoding.GetBytes(string.Concat(objArray1)));
		}
		 
		internal void SendCommand(string message)
		{
		      this.socketSB.Send(this.messenger.TextEncoding.GetBytes(message));
		}
		 
		public void SendMessage(string body)
		{
		      this.SendMessage(body, "MIME-Version: 1.0\r\nContent-Type: text/plain; charset=UTF-8\r\nX-MMS-IM-Format: FN=Microsoft%20Sans%20Serif; EF=; CO=0; CS=0; PF=22");
		}
		 
		public void SendMessage(string body, MSNCharset charset)
		{
		      this.SendMessage(body, "MIME-Version: 1.0\r\nContent-Type: text/plain; charset=UTF-8\r\nX-MMS-IM-Format: FN=Microsoft%20Sans%20Serif; EF=; CO=0; CS=" + ((int) charset) + "; PF=22");
		}
		 
		public void SendMessage(string body, string header)
		{
		      int num2;
		      if (!this.socketSB.Connected)
		      {
		            throw new MSNException("Failed to send message: connection already closed");
		      }
		      byte[] buffer1 = this.messenger.TextEncoding.GetBytes(header + body);
		      int num1 = buffer1.Length + 4;
		      object[] objArray1 = new object[8];
		      objArray1[0] = "MSG ";
		      this.transaction = num2 = this.transaction + 1;
		      objArray1[1] = num2;
		      objArray1[2] = " N ";
		      objArray1[3] = num1;
		      objArray1[4] = "\r\n";
		      objArray1[5] = header;
		      objArray1[6] = "\r\n\r\n";
		      objArray1[7] = body;
		      this.socketSB.Send(this.messenger.TextEncoding.GetBytes(string.Concat(objArray1)));
		}
		 		
		public void SendMessage(string body, string header, string commandHeader)
		{
		      if (!this.socketSB.Connected)
		      {
		            throw new MSNException("Failed to send message: connection already closed");
		      }
		      this.socketSB.Send(this.messenger.TextEncoding.GetBytes(commandHeader + header + body));
		}
		
		internal void SetConnection(Socket switchBoard, string sbHash)
		{
		      this.hash = sbHash;
		      this.socketSB = switchBoard;
		}
 
		
		
		      // Properties
		public bool Connected
		{
		      get
		      {
		            if (this.socketSB != null)
		            {
		                  return this.socketSB.Connected;
		            }
		            return false;
		      }
		}
		
		public bool Invited
		{
		      get
		      {
		            return (this.sessionID != 0);
		      }
		}
		
		public Messenger Messenger
		{
		      get
		      {
		            return this.messenger;
		      }
		}
		 
		public Hashtable Users
		{
		      get
		      {
		            return this.users;
		      }
		}

      // Nested Types
      public delegate void AllContactsLeftHandler(Conversation sender, EventArgs e);
      public delegate void ConnectionClosedHandler(Conversation sender, EventArgs e);
      public delegate void ConnectionEstablishedHandler(Conversation sender, EventArgs e);
      public delegate void ContactJoinHandler(Conversation sender, ContactEventArgs e);
      public delegate void ContactLeaveHandler(Conversation sender, ContactEventArgs e);
      public delegate void MessageReceivedHandler(Conversation sender, MessageEventArgs e);
      public delegate void UserTypingHandler(Conversation sender, ContactEventArgs e);


	}
}
