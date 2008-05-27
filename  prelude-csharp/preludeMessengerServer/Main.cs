/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 22.04.2005
 * Time: 21:47
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
//using DotMSN;
using sharpMSN;
using PreLudeEngine;
using System.Timers;
using System.Threading;

namespace preludeMessengerServer
{
	class MainClass
	{
		
		// this object will be the interface to the dotMSN library
		//private static DotMSN.Messenger messenger = new Messenger();
		private static sharpMSN.Messenger messenger = new Messenger();
		private static string ind = "";
		public  static PreLudeInterface pi;
		private string emailAdress = "preludine78@hotmail.com";
		private string userPassword = "ganesh00";
		private static System.Timers.Timer timer = null;
		
		public static void Main(string[] args)
		{
			Console.WriteLine("# worldz first prelude im server rulez!!\n");
			timer = new System.Timers.Timer();
			timer.Elapsed += new System.Timers.ElapsedEventHandler(updateContacts);			
			MainClass mc = new MainClass();
			//initialize interface
			pi = new PreLudeInterface();
			pi.proactiveMode = false;			
			//define path to mind file
			pi.loadedMind = "mind.mdu";			
			//start your engine ...
			pi.initializeEngine();
			
			mc.startPreludeMSNServer();
			Console.ReadLine();
		}
		
		public void startPreludeMSNServer()
		{
			//messenger = new Messenger();
			try
			{				
				// make sure we don't use the default settings, since they're invalid
				if(emailAdress == "yourmail@hotmail.com")
					Console.WriteLine("Fill in your own passport details to connect to the messenger service");
				else
				{
				    //messenger.Owner.Name = "Mirabelle";
				    
				    messenger.ContactAdded += new Messenger.ContactAddedHandler(OnContactAdded);
				    	
					// setup the callbacks
					// we log when someone goes online
					messenger.ContactOnline += new Messenger.ContactOnlineHandler(OnContactOnline);
					messenger.ContactOffline += new Messenger.ContactOfflineHandler(OnContactOffline);
					//messenger.ContactStatusChange += new Messenger.StatusChangedHandler(OnStatusChanged);

					// we want to do something when we have a conversation
					messenger.ConversationCreated += new Messenger.ConversationCreatedHandler(ConversationCreated);

					// notify us when synchronization is completed
					messenger.SynchronizationCompleted += new Messenger.SynchronizationCompletedHandler(OnSynchronizationCompleted);
					
					messenger.MessageReceived += new Messenger.MessageReceivedHandler(OnReceivingMessages);

					// everything is setup, now connect to the messenger service
					messenger.Connect(emailAdress, userPassword);					
					Console.WriteLine("Connected!\r\n");

					// synchronize the whole list.
					// remember you can only do this once per session!
					// after synchronizing the initial status will be set.
					messenger.SynchronizeList();
					messenger.SetStatus(MSNStatus.Online);
					timer.Interval = 1000 * 60 * 60 * 3; // 3 hours
					timer.Start();
				}
			}
			catch(MSNException e)
			{
				// in case of an error, report this to the user (or developer)
				Console.WriteLine("Connecting failed: " + e.ToString());
			}			

		}

		
		private void ConversationCreated(Messenger sender, ConversationEventArgs e)
		{
			// we request a conversation or were asked one. Now log this
			Console.WriteLine( "Conversation object created\r\n");

			// remember there are not yet users in the conversation (except ourselves)
			// they will join _after_ this event. We create another callback to handle this.
			// When user(s) have joined we can start sending messages.
			e.Conversation.ContactJoin += new Conversation.ContactJoinHandler(ContactJoined);			

			// log the event when the two clients are connected
			e.Conversation.ConnectionEstablished += new Conversation.ConnectionEstablishedHandler(ConnectionEstablished);

			// notify us when the other contact is typing something
			e.Conversation.UserTyping  += new Conversation.UserTypingHandler(ContactTyping);			
			
			//exchange messages
			e.Conversation.MessageReceived += new Conversation.MessageReceivedHandler(OnConversationReceived);
		}
		
		private void ConnectionEstablished(Conversation sender, EventArgs e)
		{
			Console.WriteLine("connection established.\r\n");
		}

		private void ContactTyping(Conversation sender, ContactEventArgs e)
		{
			//Console.WriteLine(e.Contact.Name + " is typing");
		}

		private void OnContactOnline(Messenger sender, ContactEventArgs e)
		{
			Console.WriteLine(e.Contact.Name + " went online\r\n");
			//messenger.SynchronizeList();
			//messenger.SetStatus(MSNStatus.Online);					
			messenger.RequestConversation(e.Contact.Mail);			
		}
		
		private void OnContactOffline(Messenger sender, ContactEventArgs e)
		{
			Console.WriteLine(e.Contact.Name + " went offline\r\n");
		}

		private void ContactJoined(Conversation sender, ContactEventArgs e)
		{
			// someone joined our conversation! remember that this also occurs when you are
			// only talking to 1 other person. Log this event.
			Console.WriteLine(e.Contact.Name + " joined the conversation.\r\n");

			// now say something back. You can send messages using the Conversation object.
			sender.SendMessage("Hi!");
		}
			
		private void OnSynchronizationCompleted(Messenger sender, EventArgs e)
		{
			// first show all people who are on our forwardlist. This is the 'main' contactlist
			// a normal person would see when logging in.
			// if you want to get all 'online' people enumerate trough this list and extract
			// all contacts with the right DotMSN.MSNStatus  (eg online/away/busy)
			foreach(Contact contact in messenger.GetListEnumerator(MSNList.ForwardList))
			{					
				Console.WriteLine( "FL > " + contact.Name + " (" + contact.Status + ")\r\n");
			}

			// now get the reverse list. This list shows all people who have you on their
			// contactlist.
			foreach(Contact contact in messenger.ReverseList)
			{
				Console.WriteLine( "RL > " + contact.Name + " (" + contact.Status + ")\r\n");
			}

			// we follow with the blocked list. this shows all the people who are blocked
			// by you
			foreach(Contact contact in messenger.BlockedList)
			{
				Console.WriteLine(  "BL > " + contact.Name + " (" + contact.Status + ")\r\n");
			}

			// when the privacy of the client is set to MSNPrivacy.NoneButAllowed then only
			// the contacts on the allowedlist are able to see your status
			foreach(Contact contact in messenger.AllowedList)
			{
				Console.WriteLine( "AL > " + contact.Name + " (" + contact.Status + ")\r\n");
			}

			// now set our initial status !
			// we must set an initial status otherwise 
			messenger.SetStatus(MSNStatus.Online);					
			Console.WriteLine( "Status set to online!\r\n");

			/* uncomment this when you want to automatically add
			* people who have added you to their contactlist on your own
			* contactlist. (remember the pop-up dialog in MSN Messenger client when someone adds you, this is the 'automatic' method)*/
			foreach(Contact contact in messenger.GetListEnumerator(MSNList.ReverseList))
			{	
				Thread.Sleep(5000);
				messenger.AddContact(contact.Mail);
				Thread.Sleep(5000);
				messenger.RequestConversation(contact.Mail);
			}
		}
		
		private void OnReceivingMessages(Messenger sender, string a)
		{
			Console.WriteLine("Hotmail MSG: " + a);
			return;
		}
	
		private void OnConversationReceived(Conversation sender, MessageEventArgs e)
		{
			if(e.Message.Text == "exit__") 
				pi.stopPreludeEngine();
			else
			{
				Console.WriteLine(sender.Messenger.Owner.Name + "User said: " + e.Message.Text);
				ind = pi.chatWithPrelude(e.Message.Text);
				Console.WriteLine("I, Prelude, say: " + ind );
				sender.SendMessage(ind);
			}
		}
		
			
		private void OnContactAdded(Messenger sender, ListMutateEventArgs e)
		{
			Console.WriteLine("received contact");
			return;
		}
			
		private static void updateContacts(object sender, System.Timers.ElapsedEventArgs e)
		{
			if(timer.Enabled != false)
			{
				//stopPreludeMSNServer();
				//startPreludeMSNServer();
			}		
		}	
		
		private static void stopPreludeMSNServer()
		{
			messenger.CloseConnection();
		}


	}
}
