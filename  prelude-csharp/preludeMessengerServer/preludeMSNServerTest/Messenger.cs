/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 14:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using System.Collections;
using System.Net;
using System.Web;
using System.Text;
using System.Threading;
using System.Security;
using System.Security.Cryptography;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace sharpMSN
{
	/// <summary>
	/// Description of Messenger.
	/// </summary>
	public class Messenger
	{

      // Fields
      private static Regex ADDRe;
      private static Regex ADG;
      private static Regex BLPRe;
      private static Regex BPRRe7;
      private static Regex BPRRe8;
      private static Regex CHLRe;
      private IPEndPoint clientAddress;
      public object ClientData;
      private Connection connection;
     // private ConnectionFailureHandler ConnectionFailure;
     // private ContactAddedHandler ContactAdded;
     // private ContactGroupAddedHandler ContactGroupAdded;
     // private ContactGroupChangedHandler ContactGroupChanged;
     // private ContactGroupRemovedHandler ContactGroupRemoved;
      private Hashtable contactGroups;
     // private ContactOfflineHandler ContactOffline;
     // private ContactOnlineHandler ContactOnline;
     // private ContactRemovedHandler ContactRemoved;
      private ContactList contacts;
     // private ContactStatusChangeHandler ContactStatusChange;
     // private ConversationCreatedHandler ConversationCreated;
      private ArrayList conversationList;
      private Queue conversationQueue;
      private int currentTransaction;
     // private ErrorReceivedHandler ErrorReceived;
      private static Regex FLNRe;
      private static Regex GTCRe;
      private static Regex ILNRe;
      public MSNStatus InitialStatus;
      private Contact lastContactSynced;
      protected int lastSync;
     // private ListReceivedHandler ListReceived;
      private ArrayList log;
      private static Regex LSGRe;
      private static Regex LSTRe7;
      private static Regex LSTRe8;
     // private MailboxStatusHandler MailboxStatus;
      private static Regex messageRe;
     // private MessageReceivedHandler MessageReceived;
      private IPAddress messengerServer;
      private static Regex MSGRe;
      private bool networkConnected;
      private static Regex NLNRe;
      private Owner owner;
      private static Regex REARe;
      private static Regex REG;
      private static Regex REMRe;
     // private ReverseAddedHandler ReverseAdded;
     // private ReverseRemovedHandler ReverseRemoved;
      private static Regex RMG;
      private static Regex RNGRe;
      internal Socket socket;
      private byte[] socketBuffer;
      private static Regex splitRe;
      private int syncContactsCount;
     // private SynchronizationCompletedHandler SynchronizationCompleted;
      private static Regex SYNRe;
      protected bool synSended;
      internal UTF8Encoding TextEncoding;
      private string totalMessage;
      private static Regex XFRRe;
		

   	  public event ConnectionFailureHandler ConnectionFailure;
      public event ContactAddedHandler ContactAdded;
      public event ContactGroupAddedHandler ContactGroupAdded;
      public event ContactGroupChangedHandler ContactGroupChanged;
      public event ContactGroupRemovedHandler ContactGroupRemoved;
      public event ContactOfflineHandler ContactOffline;
      public event ContactOnlineHandler ContactOnline;
      public event ContactRemovedHandler ContactRemoved;
      public event ContactStatusChangeHandler ContactStatusChange;
      public event ConversationCreatedHandler ConversationCreated;
      public event ErrorReceivedHandler ErrorReceived;
      public event ListReceivedHandler ListReceived;
      public event MailboxStatusHandler MailboxStatus;
      public event MessageReceivedHandler MessageReceived;
      public event ReverseAddedHandler ReverseAdded;
      public event ReverseRemovedHandler ReverseRemoved;
      public event SynchronizationCompletedHandler SynchronizationCompleted;
      
      
      // Methods
static Messenger()
{
      Messenger.messageRe = new Regex(@"^(?<Command>[A-Z0-9]{3})\s+(?<Message>.*?)$", RegexOptions.Compiled);
      Messenger.CHLRe = new Regex(@"(?<Transaction>[0-9]+)\s+(?<Hash>\d+)", RegexOptions.Compiled);
      Messenger.ILNRe = new Regex(@"(?<Transaction>[0-9]+)\s+(?<Status>[A-Z]{3})\s+(?<Mail>\S+)\s+(?<Name>\S+)", RegexOptions.Compiled);
      Messenger.NLNRe = new Regex(@"(?<Status>[A-Z]{3})\s+(?<Mail>\S+)\s+(?<Name>\S+)", RegexOptions.Compiled);
      Messenger.FLNRe = new Regex(@"(?<Mail>\S+)", RegexOptions.Compiled);
      Messenger.LSTRe8 = new Regex(@"(?<Mail>\S+)\s+(?<Name>.*?)\s+(?<Lists>[0-9]+)\s?(?<Groups>[\S]+)?$", RegexOptions.Compiled);
      Messenger.LSTRe7 = new Regex(@"(?<Transaction>[0-9]+)\s+(?<Type>\w{2})\s+(?<Version>[0-9]+)\s+(?<Nr>[0-9]+)\s+(?<Total>[0-9]+)\s+(?<Mail>\S+)\s+(?<Name>.*?)\s?(?<GroupID>[0-9]+)?$", RegexOptions.Compiled);
      Messenger.XFRRe = new Regex(@"(?<Transaction>[0-9]+)\s+SB\s+(?<IP>[0-9\.]+):(?<Port>[0-9]+)\s+([A-Z]+)\s+(?<Hash>[0-9\.]+)$", RegexOptions.Compiled);
      Messenger.RNGRe = new Regex(@"(?<Session>[0-9]+)\s+(?<IP>[0-9\.]+):(?<Port>[0-9]+)\s+CKI\s+(?<Hash>[0-9\.]+)\s+(?<Mail>\S+)\s+(?<Name>.*?)$", RegexOptions.Compiled);
      Messenger.MSGRe = new Regex(@"(?<ServiceName>\w+)\s+(?<ServiceName2>\w+)\s+(?<Length>[0-9]+)$", RegexOptions.Compiled);
      Messenger.splitRe = new Regex("\r\n");
      Messenger.BPRRe7 = new Regex(@"(?<SyncID>[0-9]+)\s+(?<Mail>\S+)\s+(?<Type>[\w]+)\s?(?<Value>.*?)$", RegexOptions.Compiled);
      Messenger.BPRRe8 = new Regex(@"(?<Type>[\w]+)\s+(?<Value>.*?)$", RegexOptions.Compiled);
      Messenger.LSGRe = new Regex(@"(?<GroupID>[0-9]+)\s+(?<Name>[\w\._@]+)\s+[0-9]+$", RegexOptions.Compiled);
      Messenger.REARe = new Regex(@"(?<TransID>[0-9]+)\s+(?<PassportID>[0-9]+)\s+(?<Mail>\S+)\s+(?<Name>\S+)$", RegexOptions.Compiled);
      Messenger.ADDRe = new Regex(@"(?<TransID>[0-9]+)\s+(?<Type>[\w]+)\s+(?<ListVersion>[0-9]+)\s+(?<Mail>\S+)\s+(?<Name>\S+)\s?(?<Group>[0-9]+)?", RegexOptions.Compiled);
      Messenger.REMRe = new Regex(@"(?<TransID>[0-9]+)\s+(?<Type>[\w]+)\s+(?<ListVersion>[0-9]+)\s+(?<Mail>\S+)\s?(?<Group>[0-9]+)?", RegexOptions.Compiled);
      Messenger.SYNRe = new Regex(@"(?<TransID>[0-9]+)\s+(?<SyncID>[0-9]+)\s?(?<UsersCount>[0-9]+)?\s?(?<GroupsCount>[0-9]+)?", RegexOptions.Compiled);
      Messenger.BLPRe = new Regex(@"(?<TransID>[0-9]+)\s+(?<SyncID>[0-9]+)?\s?(?<Mode>[\w]+)", RegexOptions.Compiled);
      Messenger.GTCRe = new Regex(@"(?<TransID>[0-9]+)\s+(?<SyncID>[0-9]+)?\s?(?<Mode>[\w]+)", RegexOptions.Compiled);
      Messenger.ADG = new Regex(@"(?<TransID>[0-9]+)\s+(?<ListSync>\d+)\s+(?<Name>\S+)\s+(?<GroupID>\d+)", RegexOptions.Compiled);
      Messenger.RMG = new Regex(@"(?<TransID>[0-9]+)\s+(?<ListSync>\d+)\s+(?<GroupID>\d+)", RegexOptions.Compiled);
      Messenger.REG = new Regex(@"(?<TransID>[0-9]+)\s+(?<ListSync>\d+)\s+(?<Name>\S+)\s+(?<GroupID>\d+)", RegexOptions.Compiled);
}
 

public Messenger()
{
      this.messengerServer = IPAddress.Parse("64.4.13.17");
      this.lastContactSynced = null;
      this.syncContactsCount = 0;
      this.networkConnected = false;
      this.InitialStatus = MSNStatus.Online;
      this.log = new ArrayList();
      this.conversationQueue = new Queue();
      this.conversationList = new ArrayList();
      this.contacts = new Messenger.ContactList();
      this.contactGroups = new Hashtable();
      this.currentTransaction = 0;
      this.TextEncoding = new UTF8Encoding();
      this.socketBuffer = new byte[0x8000];
      this.synSended = false;
      this.totalMessage = "";
      IPHostEntry entry1 = Dns.Resolve("messenger.hotmail.com");
      IPAddress address1 = entry1.AddressList[0];
      this.MessengerServer = address1;
}
 

public void AddContact(string mail)
{
      object[] objArray1 = new object[7] { "ADD ", this.NewTrans(), " AL ", mail, " ", mail, "\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
      Thread.Sleep(100);
      objArray1 = new object[7] { "ADD ", this.NewTrans(), " FL ", mail, " ", mail, "\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
}
 
public void AddContact(string mail, ContactGroup group)
{
      object[] objArray1 = new object[7] { "ADD ", this.NewTrans(), " AL ", mail, " ", mail, "\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
      objArray1 = new object[9] { "ADD ", this.NewTrans(), " FL ", mail, " ", mail, " ", group.ID, "\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
}
 
public void AddGroup(string groupName)
{
      object[] objArray1 = new object[5] { "ADG ", this.NewTrans(), " ", Messenger.URLEncode(groupName), " 0\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
}
 
internal void AddToList(Contact contact, MSNList list)
{
      object[] objArray1 = new object[9] { "ADD ", this.NewTrans(), " ", this.GetMSNList(list), " ", contact.Mail, " ", contact.Mail, "\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
}
 
private string AuthenticatePassport(string twnString)
{
      string text4;
      try
      {
            WebRequest request1 = WebRequest.Create("https://nexus.passport.com/rdr/pprdr.asp");
            WebResponse response1 = request1.GetResponse();
            string text1 = response1.Headers.Get("PassportURLs");
            Regex regex1 = new Regex("DALogin=([^,]+)");
            Match match1 = regex1.Match(text1);
            if (!match1.Success)
            {
                  throw new MSNException("Regular expression failed; no DALogin (messenger login server) could be extracted");
            }
            string text2 = match1.Groups[1].ToString();
            Uri uri1 = new Uri("https://" + text2);
            response1.Close();
            response1 = this.PassportServerLogin(uri1, twnString);
            string text3 = response1.Headers.Get("Authentication-Info");
            regex1 = new Regex("from-PP='([^']+)'");
            match1 = regex1.Match(text3);
            if (!match1.Success)
            {
                  throw new MSNException("Regular expression failed; no ticket could be extracted");
            }
            text3 = match1.Groups[1].ToString();
            response1.Close();
            text4 = text3;
      }
      catch (Exception exception1)
      {
            throw new MSNException("Authenticating with passport.com failed : " + exception1.ToString(), exception1);
      }
      return text4;
}
 
public void BlockContact(Contact contact)
{
      object[] objArray1 = new object[5] { "REM ", this.NewTrans(), " AL ", contact.mail, "\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
      Thread.Sleep(150);
      objArray1 = new object[7] { "ADD ", this.NewTrans(), " BL ", contact.mail, " ", contact.name, "\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
}
 
public void ChangeGroup(Contact contact, ContactGroup group)
{
      object[] objArray1 = new object[9] { "ADD ", this.NewTrans(), " FL ", contact.mail, " ", contact.name, " ", group.ID, "\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
      objArray1 = new object[7] { "REM ", this.NewTrans(), " FL ", contact.mail, " ", contact.contactGroup, "\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
}

internal void ChangeScreenName(string NewName)
{
      if (this.owner == null)
      {
            throw new MSNException("Not a valid owner");
      }
      if (!this.socket.Connected)
      {
            throw new MSNException("Not connected to the messenger network");
      }
      object[] objArray1 = new object[7] { "REA ", this.NewTrans(), " ", this.owner.Mail, " ", Messenger.URLEncode(NewName), "\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
}
 
private string CheckedSendAndReceive(string text)
{
      string text1 = this.SendAndReceive(text);
      if (text1.IndexOf("XFR", 0, 3) > -1)
      {
            this.SwitchNameserver(text1);
      }
      return text1;
}
 

public void CloseConnection()
{
      if ((this.socket != null) && this.socket.Connected)
      {
            this.socket.Shutdown(SocketShutdown.Both);
            this.socket.Close();
      }
      this.networkConnected = false;
      this.conversationList.Clear();
      this.conversationQueue.Clear();
      this.contacts.Clear();
}
 

public void Connect(Connection connection, Owner owner)
{
      this.connection = connection;
      this.owner = owner;
      this.owner.messenger = this;
      this.DoConnect();
}
 
public void Connect(string user, string password)
{
      this.Connect(new Connection("messenger.hotmail.com", 0x747), new Owner(user, password));
}
 
private void conversation_ConnectionClosed(Conversation sender, EventArgs e)
{
      this.conversationList.Remove(sender);
}
 
private void DataReceivedCallback(IAsyncResult ar)
{
      if ((this.socket != null) && this.socket.Connected)
      {
            int num2;
            int num1 = 0;
            try
            {
                  num1 = this.socket.EndReceive(ar);
                  if (num1 <= 0)
                  {
                        this.CloseConnection();
                  }
            }
            catch (ObjectDisposedException)
            {
                  return;
            }
            this.totalMessage = this.totalMessage + this.TextEncoding.GetString(this.socketBuffer, 0, num1);
            while ((num2 = this.totalMessage.IndexOf("\r\n")) > 0)
            {
                  Contact contact6;
                  Contact contact8;
                  object[] objArray1;
                  string text6;
                  string text1 = this.totalMessage.Substring(0, num2);
                  this.totalMessage = this.totalMessage.Remove(0, num2 + 2);
                  if (this.MessageReceived != null)
                  {
                        this.MessageReceived(this, text1);
                  }
                  Match match1 = Messenger.messageRe.Match(text1);
                  if (!match1.Success)
                  {
                        continue;
                  }
                  text1 = match1.Groups["Message"].ToString();
                  switch (match1.Groups[1].ToString())
                  {
                        case "CHL":
                        {
                              match1 = Messenger.RunRegularExpression(Messenger.CHLRe, text1);
                              if (match1.Success)
                              {
                                    string text2 = this.HashMD5(match1.Groups["Hash"].ToString() + "Q1P7W2E4J9R8U3S5");
                                    objArray1 = new object[4] { "QRY ", this.NewTrans(), " msmsgs@msnmsgr.com 32\r\n", text2 } ;
                                    this.SocketSend(string.Concat(objArray1));
                              }
                              continue;
                        }
                        case "ILN":
                        {
                              goto Label_033D;
                        }
                        case "NLN":
                        {
                              goto Label_0401;
                        }
                        case "FLN":
                        {
                              goto Label_04D2;
                        }
                        case "LST":
                        {
                              goto Label_0557;
                        }
                        case "XFR":
                        {
                              goto Label_07CE;
                        }
                        case "RNG":
                        {
                              goto Label_092F;
                        }
                        case "BPR":
                        {
                              match1 = Messenger.RunRegularExpression(Messenger.BPRRe7, text1);
                              if (!match1.Success)
                              {
                                    goto Label_0BCA;
                              }
                              contact6 = this.GetContact(match1.Groups["Mail"].ToString());
                              goto Label_0AE3;
                        }
                        case "LSG":
                        {
                              goto Label_0CF8;
                        }
                        case "SYN":
                        {
                              goto Label_0D79;
                        }
                        case "REA":
                        {
                              match1 = Messenger.RunRegularExpression(Messenger.REARe, text1);
                              if (match1.Success)
                              {
                                    if ((this.owner == null) || (match1.Groups["Mail"].ToString() != this.owner.mail))
                                    {
                                          goto Label_0E81;
                                    }
                                    this.owner.SetName(match1.Groups["Name"].ToString());
                              }
                              continue;
                        }
                        case "ADD":
                        {
                              goto Label_0EBF;
                        }
                        case "REM":
                        {
                              goto Label_0FC4;
                        }
                        case "BLP":
                        {
                              match1 = Messenger.RunRegularExpression(Messenger.BLPRe, text1);
                              if (!match1.Success || (this.owner == null))
                              {
                                    continue;
                              }
                              goto Label_10DB;
                        }
                        case "GTC":
                        {
                              match1 = Messenger.RunRegularExpression(Messenger.GTCRe, text1);
                              if (!match1.Success || (this.owner == null))
                              {
                                    continue;
                              }
                              goto Label_1168;
                        }
                        case "ADG":
                        {
                              goto Label_11C7;
                        }
                        case "RMG":
                        {
                              goto Label_1269;
                        }
                        case "REG":
                        {
                              goto Label_12ED;
                        }
                        case "MSG":
                        {
                              goto Label_1395;
                        }
                        default:
                        {
                              goto Label_140D;
                        }
                  }
            Label_033D:
                  match1 = Messenger.RunRegularExpression(Messenger.ILNRe, text1);
                  if (!match1.Success)
                  {
                        continue;
                  }
                  Contact contact1 = this.GetContact(match1.Groups["Mail"].ToString());
                  contact1.SetName(match1.Groups["Name"].ToString());
                  MSNStatus status1 = contact1.Status;
                  contact1.SetStatus(this.ParseStatus(match1.Groups["Status"].ToString()));
                  if (((status1 == MSNStatus.Unknown) || (status1 == MSNStatus.Offline)) && (this.ContactOnline != null))
                  {
                        this.ContactOnline(this, new ContactEventArgs(contact1));
                  }
                  if (this.ContactStatusChange != null)
                  {
                        this.ContactStatusChange(this, new ContactStatusChangeEventArgs(contact1, status1));
                  }
                  continue;
            Label_0401:
                  match1 = Messenger.RunRegularExpression(Messenger.NLNRe, text1);
                  if (!match1.Success)
                  {
                        continue;
                  }
                  Contact contact2 = this.GetContact(match1.Groups["Mail"].ToString());
                  contact2.SetName(match1.Groups["Name"].ToString());
                  MSNStatus status2 = contact2.status;
                  contact2.SetStatus(this.ParseStatus(match1.Groups["Status"].ToString()));
                  if (this.ContactStatusChange != null)
                  {
                        this.ContactStatusChange(this, new ContactStatusChangeEventArgs(contact2, status2));
                  }
                  match1.Groups["Name"].ToString();
                  if (this.ContactOnline != null)
                  {
                        this.ContactOnline(this, new ContactEventArgs(contact2));
                  }
                  continue;
            Label_04D2:
                  if (!(match1 = Messenger.FLNRe.Match(text1)).Success)
                  {
                        continue;
                  }
                  Contact contact3 = this.GetContact(match1.Groups["Mail"].ToString());
                  MSNStatus status3 = contact3.status;
                  contact3.SetStatus(MSNStatus.Offline);
                  if (this.ContactStatusChange != null)
                  {
                        this.ContactStatusChange(this, new ContactStatusChangeEventArgs(contact3, status3));
                  }
                  if (this.ContactOffline != null)
                  {
                        this.ContactOffline(this, new ContactEventArgs(contact3));
                  }
                  continue;
            Label_0557:
                  this.syncContactsCount--;
                  match1 = Messenger.RunRegularExpression(Messenger.LSTRe7, text1);
                  if (match1.Success)
                  {
                        Contact contact4 = this.GetContact(match1.Groups["Mail"].ToString());
                        MSNList list1 = this.GetMSNList(match1.Groups["Type"].ToString());
                        contact4.lists |= list1;
                        contact4.SetName(match1.Groups["Name"].ToString());
                        if (match1.Groups["GroupID"].ToString().Length > 0)
                        {
                              contact4.SetContactGroup(int.Parse(match1.Groups["GroupID"].ToString()));
                        }
                        contact4.updateVersion = int.Parse(match1.Groups["Version"].ToString());
                        if (match1.Groups["Nr"].ToString() == match1.Groups["Total"].ToString())
                        {
                              MSNList list2 = this.GetMSNList(match1.Groups["Type"].ToString());
                              if (this.ListReceived != null)
                              {
                                    this.ListReceived(this, new ListReceivedEventArgs(list2));
                              }
                        }
                        continue;
                  }
                  match1 = Messenger.RunRegularExpression(Messenger.LSTRe8, text1);
                  if (!match1.Success)
                  {
                        continue;
                  }
                  Contact contact5 = this.GetContact(match1.Groups["Mail"].ToString());
                  this.lastContactSynced = contact5;
                  contact5.lists = (MSNList) int.Parse(match1.Groups["Lists"].ToString());
                  contact5.updateVersion = this.lastSync;
                  contact5.SetName(match1.Groups["Name"].ToString());
                  char[] chArray1 = new char[1] { ',' } ;
                  string[] textArray1 = match1.Groups["Groups"].ToString().Split(chArray1);
                  string[] textArray2 = textArray1;
                  for (int num11 = 0; num11 < textArray2.Length; num11++)
                  {
                        string text3 = textArray2[num11];
                        if (text3 != "")
                        {
                              contact5.SetContactGroup(int.Parse(text3));
                        }
                  }
                  if ((this.syncContactsCount <= 0) && (this.SynchronizationCompleted != null))
                  {
                        this.SynchronizationCompleted(this, new EventArgs());
                  }
                  continue;
            Label_07CE:
                  match1 = Messenger.RunRegularExpression(Messenger.XFRRe, text1);
                  if (!match1.Success)
                  {
                        continue;
                  }
                  IPEndPoint point1 = new IPEndPoint(IPAddress.Parse(match1.Groups["IP"].ToString()), int.Parse(match1.Groups["Port"].ToString()));
                  Socket socket1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                  socket1.Connect(point1);
                  if (!socket1.Connected)
                  {
                        this.log.Add("Could not connect");
                        objArray1 = new object[4] { "Could not connect to switchboard server @ ", match1.Groups["IP"], ":", match1.Groups["Port"] } ;
                        throw new MSNException(string.Concat(objArray1));
                  }
                  if (this.conversationQueue.Count <= 0)
                  {
                        throw new MSNException("Server sends chatanswer but there are no conversations left in the queue");
                  }
                  Conversation conversation1 = (Conversation) this.conversationQueue.Dequeue();
                  conversation1.SetConnection(socket1, match1.Groups["Hash"].ToString());
                  this.conversationList.Add(conversation1);
                  conversation1.ConnectionClosed += new Conversation.ConnectionClosedHandler(this.conversation_ConnectionClosed);
                  if (this.ConversationCreated != null)
                  {
                        this.ConversationCreated(this, new ConversationEventArgs(conversation1));
                  }
                  conversation1.InitiateRequest();
                  continue;
            Label_092F:
                  match1 = Messenger.RunRegularExpression(Messenger.RNGRe, text1);
                  if (!match1.Success)
                  {
                        continue;
                  }
                  IPEndPoint point2 = new IPEndPoint(IPAddress.Parse(match1.Groups["IP"].ToString()), int.Parse(match1.Groups["Port"].ToString()));
                  Socket socket2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                  socket2.Connect(point2);
                  if (!socket2.Connected)
                  {
                        this.log.Add("Could not connect");
                        objArray1 = new object[4] { "Could not connect to switchboard server @ ", match1.Groups["IP"], ":", match1.Groups["Port"] } ;
                        throw new MSNException(string.Concat(objArray1));
                  }
                  Conversation conversation2 = new Conversation(socket2, match1.Groups["Hash"].ToString(), this.GetContact(match1.Groups["Mail"].ToString()), this, int.Parse(match1.Groups["Session"].ToString()));
                  this.conversationList.Add(conversation2);
                  conversation2.ConnectionClosed += new Conversation.ConnectionClosedHandler(this.conversation_ConnectionClosed);
                  if (this.ConversationCreated != null)
                  {
                        this.ConversationCreated(this, new ConversationEventArgs(conversation2));
                  }
                  conversation2.InitiateAnswer();
                  continue;
            Label_0AE3:
                  if ((text6 = match1.Groups["Type"].ToString()) == null)
                  {
                        continue;
                  }
                  text6 = string.IsInterned(text6);
                  if (text6 != "PHH")
                  {
                        if (text6 == "PHW")
                        {
                              goto Label_0B58;
                        }
                        if (text6 == "PHM")
                        {
                              goto Label_0B7E;
                        }
                        if (text6 == "MOB")
                        {
                              goto Label_0BA4;
                        }
                        continue;
                  }
                  contact6.homePhone = HttpUtility.UrlDecode(match1.Groups["Value"].ToString());
                  continue;
            Label_0B58:
                  contact6.workPhone = HttpUtility.UrlDecode(match1.Groups["Value"].ToString());
                  continue;
            Label_0B7E:
                  contact6.mobilePhone = HttpUtility.UrlDecode(match1.Groups["Value"].ToString());
                  continue;
            Label_0BA4:
                  contact6.mobilePager = HttpUtility.UrlDecode(match1.Groups["Value"].ToString());
                  continue;
            Label_0BCA:
                  match1 = Messenger.RunRegularExpression(Messenger.BPRRe8, text1);
                  if (!match1.Success)
                  {
                        continue;
                  }
                  Contact contact7 = this.lastContactSynced;
                  if (contact7 == null)
                  {
                        throw new MSNException("Phone numbers are sent but lastContact == null");
                  }
                  if ((text6 = match1.Groups["Type"].ToString()) == null)
                  {
                        continue;
                  }
                  text6 = string.IsInterned(text6);
                  if (text6 != "PHH")
                  {
                        if (text6 == "PHW")
                        {
                              goto Label_0C7B;
                        }
                        if (text6 == "PHM")
                        {
                              goto Label_0CA1;
                        }
                        if (text6 == "MOB")
                        {
                              goto Label_0CC7;
                        }
                        continue;
                  }
                  contact7.homePhone = HttpUtility.UrlDecode(match1.Groups["Value"].ToString());
                  continue;
            Label_0C7B:
                  contact7.workPhone = HttpUtility.UrlDecode(match1.Groups["Value"].ToString());
                  continue;
            Label_0CA1:
                  contact7.mobilePhone = HttpUtility.UrlDecode(match1.Groups["Value"].ToString());
                  continue;
            Label_0CC7:
                  contact7.mobilePager = HttpUtility.UrlDecode(match1.Groups["Value"].ToString());
                  continue;
            Label_0CF8:
                  match1 = Messenger.RunRegularExpression(Messenger.LSGRe, text1);
                  if (!match1.Success)
                  {
                        continue;
                  }
                  try
                  {
                        this.contactGroups.Add(int.Parse(match1.Groups["GroupID"].ToString()), new ContactGroup(match1.Groups["Name"].ToString(), int.Parse(match1.Groups["GroupID"].ToString()), this));
                        continue;
                  }
                  catch (FormatException)
                  {
                        continue;
                  }
            Label_0D79:
                  match1 = Messenger.RunRegularExpression(Messenger.SYNRe, text1);
                  if (!match1.Success)
                  {
                        continue;
                  }
                  int num3 = int.Parse(match1.Groups["SyncID"].ToString());
                  if (this.lastSync == num3)
                  {
                        this.syncContactsCount = 0;
                        if (this.SynchronizationCompleted != null)
                        {
                              this.SynchronizationCompleted(this, new EventArgs());
                        }
                        continue;
                  }
                  this.lastSync = num3;
                  this.lastContactSynced = null;
                  string text4 = match1.Groups["UsersCount"].Value;
                  this.syncContactsCount = int.Parse(text4);
                  continue;
            Label_0E81:
                  contact8 = this.GetContact(match1.Groups["Mail"].ToString());
                  contact8.SetName(match1.Groups["Name"].ToString());
                  continue;
            Label_0EBF:
                  match1 = Messenger.RunRegularExpression(Messenger.ADDRe, text1);
                  if (!match1.Success)
                  {
                        continue;
                  }
                  MSNList list3 = this.GetMSNList(match1.Groups["Type"].ToString());
                  Contact contact9 = this.GetContact(match1.Groups["Mail"].ToString());
                  contact9.SetName(match1.Groups["Name"].ToString());
                  int num4 = 0;
                  if (match1.Groups["Group"].ToString().Length > 0)
                  {
                        num4 = int.Parse(match1.Groups["Group"].ToString());
                  }
                  contact9.AddToList(list3);
                  if (num4 > 0)
                  {
                        contact9.SetContactGroup(num4);
                  }
                  if ((list3 == MSNList.ReverseList) && (this.ReverseAdded != null))
                  {
                        this.ReverseAdded(this, new ContactEventArgs(contact9));
                  }
                  if (this.ContactAdded != null)
                  {
                        this.ContactAdded(this, new ListMutateEventArgs(contact9, list3));
                  }
                  continue;
            Label_0FC4:
                  match1 = Messenger.RunRegularExpression(Messenger.REMRe, text1);
                  if (!match1.Success)
                  {
                        continue;
                  }
                  MSNList list4 = this.GetMSNList(match1.Groups["Type"].ToString());
                  Contact contact10 = this.GetContact(match1.Groups["Mail"].ToString());
                  int num5 = 0;
                  if (match1.Groups["Group"].ToString().Length > 0)
                  {
                        num5 = int.Parse(match1.Groups["Group"].ToString());
                  }
                  contact10.RemoveFromList(list4);
                  if (num5 > 0)
                  {
                        contact10.SetContactGroup(num5);
                  }
                  if ((list4 == MSNList.ReverseList) && (this.ReverseRemoved != null))
                  {
                        this.ReverseRemoved(this, new ContactEventArgs(contact10));
                  }
                  if (this.ContactRemoved != null)
                  {
                        this.ContactRemoved(this, new ListMutateEventArgs(contact10, list4));
                  }
                  continue;
            Label_10DB:
                  if ((text6 = match1.Groups["Mode"].ToString()) == null)
                  {
                        continue;
                  }
                  text6 = string.IsInterned(text6);
                  if (text6 != "AL")
                  {
                        if (text6 == "BL")
                        {
                              goto Label_1129;
                        }
                        continue;
                  }
                  this.owner.privacy = MSNPrivacy.AllExceptBlocked;
                  continue;
            Label_1129:
                  this.owner.privacy = MSNPrivacy.NoneButAllowed;
                  continue;
            Label_1168:
                  if ((text6 = match1.Groups["Mode"].ToString()) == null)
                  {
                        continue;
                  }
                  text6 = string.IsInterned(text6);
                  if (text6 != "A")
                  {
                        if (text6 == "N")
                        {
                              goto Label_11B6;
                        }
                        continue;
                  }
                  this.owner.notifyPrivacy = MSNNotifyPrivacy.PromptOnAdd;
                  continue;
            Label_11B6:
                  this.owner.notifyPrivacy = MSNNotifyPrivacy.AutomaticAdd;
                  continue;
            Label_11C7:
                  match1 = Messenger.RunRegularExpression(Messenger.ADG, text1);
                  if (!match1.Success)
                  {
                        continue;
                  }
                  try
                  {
                        int num6 = int.Parse(match1.Groups["GroupID"].ToString());
                        this.contactGroups[num6] = new ContactGroup(Messenger.URLDecode(match1.Groups["Name"].ToString()), num6, this);
                        if (this.ContactGroupAdded != null)
                        {
                              this.ContactGroupAdded(this, new ContactGroupEventArgs((ContactGroup) this.contactGroups[num6]));
                        }
                        continue;
                  }
                  catch (Exception)
                  {
                        continue;
                  }
            Label_1269:
                  match1 = Messenger.RunRegularExpression(Messenger.RMG, text1);
                  if (!match1.Success)
                  {
                        continue;
                  }
                  try
                  {
                        int num7 = int.Parse(match1.Groups["GroupID"].ToString());
                        ContactGroup group1 = (ContactGroup) this.contactGroups[num7];
                        this.contactGroups.Remove(num7);
                        if (this.ContactGroupRemoved != null)
                        {
                              this.ContactGroupRemoved(this, new ContactGroupEventArgs(group1));
                        }
                        continue;
                  }
                  catch (Exception)
                  {
                        continue;
                  }
            Label_12ED:
                  match1 = Messenger.RunRegularExpression(Messenger.REG, text1);
                  if (!match1.Success)
                  {
                        continue;
                  }
                  try
                  {
                        int num8 = int.Parse(match1.Groups["GroupID"].ToString());
                        ContactGroup group2 = (ContactGroup) this.contactGroups[num8];
                        group2.Name = Messenger.URLDecode(match1.Groups["Name"].ToString());
                        if (this.ContactGroupChanged != null)
                        {
                              this.ContactGroupChanged(this, new ContactGroupEventArgs((ContactGroup) this.contactGroups[num8]));
                        }
                        continue;
                  }
                  catch (Exception)
                  {
                        continue;
                  }
            Label_1395:
                  if (!(match1 = Messenger.MSGRe.Match(match1.Groups["Message"].ToString())).Success)
                  {
                        continue;
                  }
                  try
                  {
                        int num9 = int.Parse(match1.Groups["Length"].ToString());
                        string text5 = this.totalMessage.Substring(0, num9);
                        this.totalMessage = this.totalMessage.Remove(0, num9);
                        this.ParseHotmailMessages(text5);
                        continue;
                  }
                  catch (FormatException)
                  {
                        continue;
                  }
            Label_140D:
                  try
                  {
                        int num10 = int.Parse(match1.Groups[1].ToString());
                        if (this.ErrorReceived != null)
                        {
                              this.ErrorReceived(this, new MSNErrorEventArgs((MSNError) Enum.ToObject(typeof(MSNError), num10)));
                        }
                        continue;
                  }
                  catch (FormatException)
                  {
                        continue;
                  }
            }
            this.socketBuffer = new byte[this.socketBuffer.Length];
            if (this.socket.Connected)
            {
                  try
                  {
                        this.socket.BeginReceive(this.socketBuffer, 0, this.socketBuffer.Length, SocketFlags.None, new AsyncCallback(this.DataReceivedCallback), new object());
                  }
                  catch (SocketException exception1)
                  {
                        if (this.ConnectionFailure != null)
                        {
                              this.ConnectionFailure(this, new ConnectionErrorEventArgs(exception1));
                        }
                        return;
                  }
            }
      }
}

protected void DoConnect()
{
      this.log.Add("Starting connection");
      if (this.Connected)
      {
            this.CloseConnection();
      }
      Encoding encoding1 = Encoding.ASCII;
      byte[] buffer1 = new byte[0x400];
      IPHostEntry entry1 = Dns.GetHostByName("messenger.hotmail.com");
      if (entry1.AddressList.Length <= 0)
      {
            throw new MSNException("Could not resolve IP adress for messenger.hotmail.com. Check your DNS settings.");
      }
      this.MessengerServer = entry1.AddressList[0];
      IPEndPoint point1 = new IPEndPoint(this.MessengerServer, 0x747);
      this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      this.socket.Connect(point1);
      if (!this.socket.Connected)
      {
            this.log.Add("Could not connect");
            throw new MSNException("Could not connect");
      }
      this.SendAndReceive("VER " + this.NewTrans() + " MSNP8 CVR0");
      object[] objArray1 = new object[4] { "CVR ", this.NewTrans(), " 0x0409 win 4.10 i386 MSNMSGR 5.0.0544 MSMSGS ", this.Owner.Mail } ;
      string text1 = this.SendAndReceive(string.Concat(objArray1));
      objArray1 = new object[4] { "USR ", this.NewTrans(), " TWN I ", this.owner.Mail } ;
      text1 = this.SendAndReceive(string.Concat(objArray1));
      if (text1.IndexOf("NS") >= 0)
      {
            this.SwitchNameserver(text1);
            objArray1 = new object[4] { "CVR ", this.NewTrans(), " 0x0409 win 4.10 i386 MSNMSGR 5.0.0544 MSMSGS ", this.Owner.Mail } ;
            this.CheckedSendAndReceive(string.Concat(objArray1));
            objArray1 = new object[4] { "USR ", this.NewTrans(), " TWN I ", this.owner.Mail } ;
            text1 = this.CheckedSendAndReceive(string.Concat(objArray1));
            Regex regex1 = new Regex(@"USR\s+[0-9]+\s+TWN\s+S\s+(\S+)\r\n");
            Match match1 = regex1.Match(text1);
            if (!match1.Success)
            {
                  throw new MSNException("Regular expression failed; no TWN string could be extracted");
            }
            string text2 = match1.Groups[1].ToString();
            string text3 = this.AuthenticatePassport(text2);
            objArray1 = new object[4] { "USR ", this.NewTrans(), " TWN S ", text3 } ;
            this.CheckedSendAndReceive(string.Concat(objArray1));
      }
      this.networkConnected = true;
      this.socket.BeginReceive(this.socketBuffer, 0, this.socketBuffer.Length, SocketFlags.None, new AsyncCallback(this.DataReceivedCallback), new object());
}
 
~Messenger()
{
      this.CloseConnection();
}
 

public Contact GetContact(string mail)
{
      mail = mail.ToLower();
      if (!this.contacts.ContainsKey(mail))
      {
            Contact contact1 = new Contact();
            contact1.mail = mail;
            contact1.name = mail;
            contact1.messenger = this;
            contact1.contactGroup = 0;
            this.contacts.Add(mail, contact1);
            return (Contact) this.contacts[mail];
      }
      return (Contact) this.contacts[mail];
}
 
public ContactGroup GetContactGroup(string groupName)
{
      foreach (ContactGroup group1 in this.contactGroups.Values)
      {
            if (group1.Name == groupName)
            {
                  return group1;
            }
      }
      return null;
}
 public Messenger.ContactList.ListEnumerator GetListEnumerator(MSNList type)
{
      switch (type)
      {
            case MSNList.ForwardList:
            {
                  return this.ForwardList;
            }
            case MSNList.AllowedList:
            {
                  return this.AllowedList;
            }
            case MSNList.BlockedList:
            {
                  return this.BlockedList;
            }
            case MSNList.ReverseList:
            {
                  return this.ReverseList;
            }
      }
      return this.AllList;
}
 

public ArrayList GetLog()
{
      return this.log;
}
 

protected string GetMSNList(MSNList list)
{
      switch (list)
      {
            case MSNList.ForwardList:
            {
                  return "FL";
            }
            case MSNList.AllowedList:
            {
                  return "AL";
            }
            case MSNList.BlockedList:
            {
                  return "BL";
            }
            case MSNList.ReverseList:
            {
                  return "RL";
            }
      }
      throw new MSNException("Unknown MSNList type");
}
 

protected MSNList GetMSNList(string name)
{
      string text1;
      if ((text1 = name) != null)
      {
            text1 = string.IsInterned(text1);
            if (text1 != "AL")
            {
                  if (text1 == "FL")
                  {
                        return MSNList.ForwardList;
                  }
                  if (text1 == "BL")
                  {
                        return MSNList.BlockedList;
                  }
                  if (text1 == "RL")
                  {
                        return MSNList.ReverseList;
                  }
            }
            else
            {
                  return MSNList.AllowedList;
            }
      }
      throw new MSNException("Unknown MSNList type");
}
 

private string[] GetWords(string line)
{
      return Regex.Split(line, @"\s+");
}
 

protected string HashMD5(string input)
{
      byte[] buffer1 = ((HashAlgorithm) CryptoConfig.CreateFromName("MD5")).ComputeHash(this.TextEncoding.GetBytes(input));
      string text1 = "";
      for (int num1 = 0; num1 < buffer1.Length; num1++)
      {
            text1 = text1 + buffer1[num1].ToString("x2");
      }
      return text1;
}
 
protected int NewTrans()
{
      return this.currentTransaction++;
}
 
private void ParseHotmailMessages(string body)
{
      Match match1;
      Regex regex1 = new Regex(@"Content-Type:\s+(?<ContentType>[\w/\-0-9]+)", RegexOptions.Compiled | RegexOptions.Multiline);
      if ((match1 = regex1.Match(body)).Success)
      {
            string text5;
            match1.Groups["ContentType"].ToString();
            if ((text5 = match1.Groups["ContentType"].ToString()) == null)
            {
                  return;
            }
            text5 = string.IsInterned(text5);
            if (((text5 != "text/x-msmsgsinitialemailnotification") && (text5 != "text/x-msmsgsemailnotification")) && ((text5 != "application/x-msmsgsemailnotification") && (text5 != "application/x-msmsgsinitialemailnotification")))
            {
                  if (text5 == "text/x-msmsgsprofile")
                  {
                        string text4 = Regex.Match(body, @"ClientIP:\s+(?<IP>\S+)").Groups[1].ToString();
                        int num3 = int.Parse(Regex.Match(body, @"ClientPort:\s+(?<Port>\d+)").Groups[1].ToString());
                        num3 = ((num3 & 0xff) * 0x100) + ((num3 & 65280) / 0x100);
                        this.clientAddress = new IPEndPoint(IPAddress.Parse(text4), num3);
                  }
            }
            else
            {
                  try
                  {
                        int num1 = int.Parse(Regex.Match(body, @"Inbox-Unread:\s+(?<InboxUnread>[0-9]+)").Groups[1].ToString());
                        int num2 = int.Parse(Regex.Match(body, @"Folders-Unread:\s+(?<FoldersUnread>[0-9]+)").Groups[1].ToString());
                        string text1 = Regex.Match(body, @"Inbox-URL:\s+(?<InboxURL>\S+)").Groups[1].ToString();
                        string text2 = Regex.Match(body, @"Folders-URL:\s+(?<FoldersURL>\S+)").Groups[1].ToString();
                        string text3 = Regex.Match(body, @"Post-URL:\s+(?<PostURL>\S+)").Groups[1].ToString();
                        if (this.MailboxStatus != null)
                        {
                              this.MailboxStatus(this, new MailboxStatusEventArgs(num1, num2, text1, text2, text3));
                        }
                  }
                  catch (Exception)
                  {
                  }
            }
      }
}
 
private string ParseMessage(string line)
{
      Regex regex1 = new Regex(@"^[A-Z0-9]{3}\s+[0-9]+\s+(.*?)$", RegexOptions.Compiled);
      Match match1 = regex1.Match(line);
      if (!match1.Success)
      {
            throw new MSNException("Could not parse transaction-based message in line: " + line);
      }
      return match1.Groups[1].ToString();
}
 
protected string ParseStatus(MSNStatus status)
{
      switch (status)
      {
            case MSNStatus.Offline:
            {
                  return "FLN";
            }
            case MSNStatus.Hidden:
            {
                  return "HDN";
            }
            case MSNStatus.Online:
            {
                  return "NLN";
            }
            case MSNStatus.Away:
            {
                  return "AWY";
            }
            case MSNStatus.Busy:
            {
                  return "BSY";
            }
            case MSNStatus.BRB:
            {
                  return "BRB";
            }
            case MSNStatus.Lunch:
            {
                  return "LUN";
            }
            case MSNStatus.Phone:
            {
                  return "PHN";
            }
            case MSNStatus.Idle:
            {
                  return "IDL";
            }
      }
      return "Unknown status";
}
 
protected MSNStatus ParseStatus(string status)
{
      switch (status)
      {
            case "NLN":
            {
                  return MSNStatus.Online;
            }
            case "BSY":
            {
                  return MSNStatus.Busy;
            }
            case "IDL":
            {
                  return MSNStatus.Idle;
            }
            case "BRB":
            {
                  return MSNStatus.BRB;
            }
            case "AWY":
            {
                  return MSNStatus.Away;
            }
            case "PHN":
            {
                  return MSNStatus.Phone;
            }
            case "LUN":
            {
                  return MSNStatus.Lunch;
            }
            case "FLN":
            {
                  return MSNStatus.Offline;
            }
            case "HDN":
            {
                  return MSNStatus.Hidden;
            }
      }
      return MSNStatus.Unknown;
}
 

private string ParseTransaction(string line)
{
      Regex regex1 = new Regex(@"^[A-Z0-9]{3}\s+([0-9]+)", RegexOptions.Compiled);
      Match match1 = regex1.Match(line);
      if (!match1.Success)
      {
            throw new MSNException("Could not parse transaction ID line: " + line);
      }
      return match1.Groups[1].ToString();
}
 
private WebResponse PassportServerLogin(Uri serverUri, string twnString)
{
      HttpWebRequest request1 = (HttpWebRequest) WebRequest.Create(serverUri);
      request1.Headers.Clear();
      string[] textArray1 = new string[6] { "Authorization: Passport1.4 OrgVerb=GET,OrgURL=http%3A%2F%2Fmessenger%2Emsn%2Ecom,sign-in=", HttpUtility.UrlEncode(this.Owner.Mail), ",pwd=", HttpUtility.UrlEncode(this.Owner.Password), ",", twnString } ;
      string text1 = string.Concat(textArray1);
      request1.Headers.Add(text1);
      request1.Headers.ToString();
      request1.AllowAutoRedirect = false;
      request1.PreAuthenticate = false;
      HttpWebResponse response1 = (HttpWebResponse) request1.GetResponse();
      if (response1.StatusCode == HttpStatusCode.OK)
      {
            return response1;
      }
      if (response1.StatusCode == HttpStatusCode.Found)
      {
            string text2 = response1.Headers.Get("Location");
            response1.Close();
            return this.PassportServerLogin(new Uri(text2), twnString);
      }
      if (response1.StatusCode == HttpStatusCode.Unauthorized)
      {
            throw new MSNException("Failed to login. Response of passport server: " + response1.Headers[0]);
      }
      throw new MSNException("Passport server responded with an unknown header");
}
 
public void RemoveContact(Contact contact)
{
      object[] objArray1 = new object[5] { "REM ", this.NewTrans(), " FL ", contact.mail, "\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
      Thread.Sleep(150);
      objArray1 = new object[5] { "REM ", this.NewTrans(), " AL ", contact.mail, "\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
}
 
internal void RemoveFromList(Contact contact, MSNList list)
{
      object[] objArray1 = new object[9] { "REM ", this.NewTrans(), " ", this.GetMSNList(list), " ", contact.Mail, " ", contact.Mail, "\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
}
 

public void RemoveGroup(ContactGroup group)
{
      object[] objArray1 = new object[5] { "RMG ", this.NewTrans(), " ", group.ID, "\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
}

public void RemoveGroup(string groupName)
{
      ContactGroup group1 = this.GetContactGroup(groupName);
      if (group1 == null)
      {
            throw new MSNException("Groupname was not found");
      }
      this.RemoveGroup(group1);
}

internal void RenameGroup(ContactGroup group, string newGroupName)
{
      object[] objArray1 = new object[7] { "REG ", this.NewTrans(), " ", group.ID, " ", HttpUtility.UrlEncode(newGroupName), " 0\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
}
 

public Conversation RequestConversation(string mail)
{
      return this.RequestConversation(mail, null);
}
 

public Conversation RequestConversation(string mail, object clientData)
{
      Conversation conversation1 = new Conversation(this, this.GetContact(mail), clientData);
      this.conversationQueue.Enqueue(conversation1);
      this.SocketSend("XFR " + this.NewTrans() + " SB\r\n");
      return conversation1;
}
 
internal void RequestScreenName(Contact contact)
{
      object[] objArray1 = new object[7] { "REA ", this.NewTrans(), " ", contact.Mail, " ", contact.Name, "\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
}
 

private static Match RunRegularExpression(Regex process, string target)
{
      Match match1;
      try
      {
            match1 = process.Match(target);
      }
      catch (FormatException exception1)
      {
            throw new RegularExpressionException(process, target, exception1);
      }
      return match1;
}
 
public void SendAllowedListRequest()
{
      this.SendListRequest("AL");
}
 

private string SendAndReceive(string text)
{
      this.log.Add(">" + text);
      if (this.socket.Send(this.TextEncoding.GetBytes(text + "\r\n")) == 0)
      {
            throw new MSNException("Send failed");
      }
      byte[] buffer1 = new byte[0x400];
      int num1 = 0;
      if (!this.socket.Connected)
      {
            throw new MSNException("Connection dropped");
      }
      num1 = this.socket.Receive(buffer1, buffer1.Length, SocketFlags.None);
      char[] chArray1 = new char[num1];
      Decoder decoder1 = Encoding.UTF8.GetDecoder();
      decoder1.GetChars(buffer1, 0, num1, chArray1, 0);
      return new string(chArray1);
}
 
public void SendBlockedListRequest()
{
      this.SendListRequest("BL");
}

 public void SendForwardListRequest()
{
      this.SendListRequest("FL");
}
 

private void SendListRequest(string type)
{
      if (this.socket == null)
      {
            throw new MSNException("Socket is null");
      }
      object[] objArray1 = new object[5] { "LST ", this.NewTrans(), " ", type, "\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
}
 

public void SendReverseListRequest()
{
      this.SendListRequest("RL");
}
 
internal void SetNotifyPrivacy(MSNNotifyPrivacy privacy)
{
      if (privacy == MSNNotifyPrivacy.AutomaticAdd)
      {
            this.SocketSend("GTC " + this.NewTrans() + " N\r\n");
      }
      if (privacy == MSNNotifyPrivacy.PromptOnAdd)
      {
            this.SocketSend("GTC " + this.NewTrans() + " A\r\n");
      }
}
 

internal void SetPrivacy(MSNPrivacy privacy)
{
      if (privacy == MSNPrivacy.AllExceptBlocked)
      {
            this.SocketSend("BLP " + this.NewTrans() + " AL\r\n");
      }
      if (privacy == MSNPrivacy.NoneButAllowed)
      {
            this.SocketSend("BLP " + this.NewTrans() + " BL\r\n");
      }
}
 

public void SetStatus(MSNStatus status)
{
      if (!this.synSended)
      {
            throw new MSNException("Can't set status. You must call SynchronizeList() before you can set an initial status.");
      }
      object[] objArray1 = new object[5] { "CHG ", this.NewTrans(), " ", this.ParseStatus(status), " 0\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
}
 

protected void SocketSend(string text)
{
      if (this.socket.Send(this.TextEncoding.GetBytes(text)) == 0)
      {
            throw new MSNException("Send failed");
      }
}

private void SwitchNameserver(string serverString)
{
      Regex regex1 = new Regex(@"([0-9\.]+):([0-9]+)");
      Match match1 = regex1.Match(serverString);
      if (!match1.Success)
      {
            throw new MSNException("Regular expression failed; no Name server could be extracted");
      }
      IPAddress address1 = IPAddress.Parse(match1.Groups[1].ToString());
      IPEndPoint point1 = new IPEndPoint(address1, int.Parse(match1.Groups[2].ToString()));
      this.socket.Shutdown(SocketShutdown.Both);
      this.socket.Close();
      this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      this.socket.Connect(point1);
      if (!this.socket.Connected)
      {
            this.log.Add("Could not connect");
            throw new MSNException("Could not connect");
      }
      this.log.Add("Changing nameserver: " + address1);
      this.SendAndReceive("VER " + this.NewTrans() + " MSNP8 CVR0");
}
 

public void SynchronizeList()
{
      this.SocketSend("SYN " + this.NewTrans() + " 0\r\n");
      this.synSended = true;
}
 
public void UnBlockContact(Contact contact)
{
      object[] objArray1 = new object[5] { "REM ", this.NewTrans(), " BL ", contact.mail, "\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
      Thread.Sleep(150);
      objArray1 = new object[7] { "ADD ", this.NewTrans(), " AL ", contact.mail, " ", contact.name, "\r\n" } ;
      this.SocketSend(string.Concat(objArray1));
}
 
public static string URLDecode(string text)
{
      return HttpUtility.UrlDecode(text);
}
 
public static string URLEncode(string text)
{
      return Regex.Replace(HttpUtility.UrlEncode(text), @"\+", "%20");
}
 


      // Properties
public Messenger.ContactList.ListEnumerator AllList
{
      get
      {
            return new Messenger.ContactList.ListEnumerator(this.contacts.GetEnumerator());
      }
}
 
public Messenger.ContactList.AllowedListEnumerator AllowedList
{
      get
      {
            return new Messenger.ContactList.AllowedListEnumerator(this.contacts.GetEnumerator());
      }
}
 
public Messenger.ContactList.BlockedListEnumerator BlockedList
{
      get
      {
            return new Messenger.ContactList.BlockedListEnumerator(this.contacts.GetEnumerator());
      }
}
 public IPEndPoint ClientAddress
{
      get
      {
            return this.clientAddress;
      }
}
 
public bool Connected
{
      get
      {
            if ((this.socket != null) && this.socket.Connected)
            {
                  return this.networkConnected;
            }
            return false;
      }
}
 

public Hashtable ContactGroups
{
      get
      {
            return this.contactGroups;
      }
}
 
public ArrayList Conversations
{
      get
      {
            return this.conversationList;
      }
}
 
public Messenger.ContactList.ForwardListEnumerator ForwardList
{
      get
      {
            return new Messenger.ContactList.ForwardListEnumerator(this.contacts.GetEnumerator());
      }
}
 
public IPAddress MessengerServer
{
      get
      {
            return this.messengerServer;
      }
      set
      {
            this.messengerServer = value;
      }
}
 

public Owner Owner
{
      get
      {
            return this.owner;
      }
}
 
public Messenger.ContactList.ReverseListEnumerator ReverseList
{
      get
      {
            return new Messenger.ContactList.ReverseListEnumerator(this.contacts.GetEnumerator());
      }
}
 


      // Nested Types
      public delegate void ConnectionFailureHandler(Messenger sender, ConnectionErrorEventArgs e);
      public delegate void ContactAddedHandler(Messenger sender, ListMutateEventArgs e);
      public delegate void ContactGroupAddedHandler(Messenger sender, ContactGroupEventArgs e);
      public delegate void ContactGroupChangedHandler(Messenger sender, ContactGroupEventArgs e);
      public delegate void ContactGroupRemovedHandler(Messenger sender, ContactGroupEventArgs e);
      public delegate void ContactOfflineHandler(Messenger sender, ContactEventArgs e);
      public delegate void ContactOnlineHandler(Messenger sender, ContactEventArgs e);
      public delegate void ContactRemovedHandler(Messenger sender, ListMutateEventArgs e);
      public delegate void ContactStatusChangeHandler(Messenger sender, ContactStatusChangeEventArgs e);
      public delegate void ConversationCreatedHandler(Messenger sender, ConversationEventArgs e);
      public delegate void ErrorReceivedHandler(Messenger sender, MSNErrorEventArgs e);
      public delegate void ListReceivedHandler(Messenger sender, ListReceivedEventArgs e);
      public delegate void MailboxStatusHandler(Messenger sender, MailboxStatusEventArgs e);
      public delegate void MessageReceivedHandler(Messenger sender, string Message);
      public delegate void OnConnectionClosed(Messenger sender, EventArgs e);
      public delegate void OnConnectionEstablished(Messenger sender, EventArgs e);
      public delegate void ReverseAddedHandler(Messenger sender, ContactEventArgs e);
      public delegate void ReverseRemovedHandler(Messenger sender, ContactEventArgs e);
      public delegate void SynchronizationCompletedHandler(Messenger sender, EventArgs e);


      public class ContactList : Hashtable
      {
      	public ContactList()
		{
		}
	
	    // Nested Types
	      public class AllowedListEnumerator : Messenger.ContactList.ListEnumerator
	      {
	            // Methods
	            public AllowedListEnumerator(IDictionaryEnumerator listEnum) : base(listEnum)
				{
				}

				public override bool MoveNext()
				{
				      while (this.baseEnum.MoveNext())
				      {
				            if ((((Contact) this.baseEnum.Value).lists & MSNList.AllowedList) > ((MSNList) 0))
				            {
				                  return true;
				            }
				      }
				      return false;
				}
	      }
	
	      public class BlockedListEnumerator : Messenger.ContactList.ListEnumerator
	      {
	            // Methods
	            public BlockedListEnumerator(IDictionaryEnumerator listEnum) : base(listEnum)
				{
				}

	            public override bool MoveNext()
				{
				      while (this.baseEnum.MoveNext())
				      {
				            if (((Contact) this.baseEnum.Value).Blocked)
				            {
				                  return true;
				            }
				      }
				      return false;
				}
	      }
	
	      public class ForwardListEnumerator : Messenger.ContactList.ListEnumerator
	      {
	            // Methods
	            public ForwardListEnumerator(IDictionaryEnumerator listEnum) : base(listEnum)
				{
				}
	            public override bool MoveNext()
				{
				      while (this.baseEnum.MoveNext())
				      {
				            if ((((Contact) this.baseEnum.Value).lists & MSNList.ForwardList) > ((MSNList) 0))
				            {
				                  return true;
				            }
				      }
				      return false;
				}
	      }
	
	      public class ListEnumerator : IEnumerator
	      {
	            // Methods
	            public ListEnumerator(IDictionaryEnumerator listEnum)
				{
				      this.baseEnum = listEnum;
				}
				 
	            public IEnumerator GetEnumerator()
				{
				      return this;
				}
	            
				public virtual bool MoveNext()
				{
				      return this.baseEnum.MoveNext();
				}
 
				public void Reset()
				{
				      this.baseEnum.Reset();
				}
				 
	            // Properties
				public object Current
				{
				      get
				      {
				            return this.baseEnum.Value;
				      }
				}
				
	            // Fields
	            protected IDictionaryEnumerator baseEnum;
	      }
	
	      public class ReverseListEnumerator : Messenger.ContactList.ListEnumerator
	      {
	            // Methods
	            public ReverseListEnumerator(IDictionaryEnumerator listEnum) : base(listEnum)
				{
				}

				public override bool MoveNext()
				{
				      while (this.baseEnum.MoveNext())
				      {
				            if ((((Contact) this.baseEnum.Value).lists & MSNList.ReverseList) > ((MSNList) 0))
				            {
				                  return true;
				            }
				      }
				      return false;
				}

	      }
      	
      	

      }
      
      
	}
}
