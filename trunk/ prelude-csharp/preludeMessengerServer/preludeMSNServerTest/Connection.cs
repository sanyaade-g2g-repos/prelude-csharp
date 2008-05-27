/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 13:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
namespace sharpMSN
{
	
	public enum MSNCharset
	{
	      // Fields
	      ANSI = 0,
	      Arabic = 0xb2,
	      Baltic = 0xba,
	      ChineseBig5 = 0x88,
	      Default = 1,
	      EastEurope = 0xee,
	      GB2312 = 0x86,
	      Greek = 0xa1,
	      HANGEUL = 0x81,
	      Hebrew = 0xb1,
	      JOHAB = 130,
	      MAC = 0x4d,
	      OEM = 0xff,
	      Russian = 0xcc,
	      SHIFTJIS = 0x80,
	      Symbol = 2,
	      Thai = 0xde,
	      Turkish = 0xa2,
	      Vietnamese = 0xa3
	}
 
	public enum MSNError
	{
	      // Fields
	      AlreadyInMode = 0xda,
	      AlreadyLoggedIn = 0xcf,
	      AuthenticationFailed = 0x38f,
	      BadFriendFile = 0x2cd,
	      CouldNotCreateConnection = 0x2c3,
	      DatabaseConnectionFailed = 0x25b,
	      DatabaseServerError = 0x1f5,
	      FileOperationFailed = 510,
	      InternalServerError = 500,
	      InvalidFullUsername = 0xd1,
	      InvalidParameter = 0xc9,
	      InvalidUser = 0xcd,
	      InvalidUsername = 0xd0,
	      MemoryAllocFailed = 520,
	      MissingDomain = 0xce,
	      MissingRequiredField = 300,
	      NameServerDown = 0x25a,
	      NotAcceptingNewUsers = 920,
	      NotAllowedWhenOffline = 0x391,
	      NotExpected = 0x2cb,
	      NotLoggedIn = 0x12e,
	      ServerGoingDown = 0x25c,
	      ServerIsBusy = 600,
	      ServerIsUnavailable = 0x259,
	      SessionIsOverloaded = 0x2c8,
	      SwitchboardFailed = 280,
	      SwitchboardTransferFailed = 0x119,
	      SyntaxError = 200,
	      TooManyActiveUsers = 0x2c9,
	      TooManySessions = 0x2ca,
	      UserAlreadyOnList = 0xd8,
	      UserAlreadyThere = 0xd7,
	      UserInOppositeList = 0xdb,
	      UserListFull = 210,
	      UserNotOnline = 0xd9,
	      WriteIsBlocking = 0x2c7
	}

	public enum MSNStatus
	{
	      // Fields
	      Away = 4,
	      BRB = 6,
	      Busy = 5,
	      Hidden = 2,
	      Idle = 9,
	      Lunch = 7,
	      Offline = 1,
	      Online = 3,
	      Phone = 8,
	      Unknown = 0
	}
 
	public enum MSNFileTransferCancelCode
	{
	      // Fields
	      ConnectionBlocked = 0x9ca6f3e,
	      GeneralError = 4,
	      OutOfDiskSpace = 0x8d16c05,
	      ReceiverCancelled = 0x9ca6f32,
	      SenderCancelled = 0x9ca6f33,
	      TimeOut = 2
	}
	
	public enum MSNInvitationCancelCode
	{
	      // Fields
	      FAIL = 1,
	      OUTBANDCANCEL = 2,
	      REJECT = 4,
	      REJECT_NOT_INSTALLED = 8,
	      TIMEOUT = 0x10
	}
 
	public enum MSNList
	{
	      // Fields
	      AllowedList = 2,
	      BlockedList = 4,
	      ForwardList = 1,
	      ReverseList = 8
	}
 
	public enum MSNNotifyPrivacy
	{
	      // Fields
	      AutomaticAdd = 2,
	      PromptOnAdd = 1,
	      Unknown = 0
	}
 
	public enum MSNPrivacy
	{
	      // Fields
	      AllExceptBlocked = 1,
	      NoneButAllowed = 2,
	      Unknown = 0
	}

	public enum MSNTextDecorations
	{
	      // Fields
	      Bold = 1,
	      Italic = 2,
	      None = 0,
	      Strike = 8,
	      Underline = 4
	}
 

	
	
	
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public class Connection
	{
	  	// Fields
      	private string host;
      	private int port;

		
		public Connection(string pHost, int pPort)
		{
      		this.host = pHost;
      		this.port = pPort;
		}
		 
		public string Host
		{
		      get
		      {
		            return this.host;
		      }
		      set
		      {
		            this.host = value;
		      }
		}

		public int Port
		{
		      get
		      {
		            return this.port;
		      }
		      set
		      {
		            this.port = value;
		      }
		}
 

		

	}
}
