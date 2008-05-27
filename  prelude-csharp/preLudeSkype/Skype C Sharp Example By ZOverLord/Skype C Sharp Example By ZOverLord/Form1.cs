using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SKYPE4COMLib;
using PreludeEngine;

// Copyright by ZOverLord 2006. Example of Skype4COM status messages. Includes:
//
// Attachment Status
// Connection Status
// User Status
// Call Status
// Message Status
// Silent Mode
//
// As you can see the release version of this example program is very small, 24KB to be
// exact, not including the Interop and other .NET files. Checkbox settings are stored
// without the use of any registry data by using the .NET Application Settings User data.
//
// Any of the links listed here can be opened by simply holding down the CTRL key and
// clicking on the link.
//
// NOTE: This example program does NOT include the Skype4COMLib .dll. If you are using the
//       Skype 3.0 for Windows release, the Skype4COMLib .dll should be already installed
//       on your system. If needed you can download the latest Skype4COMLib .dll and 
//       then register it from here: https://developer.skype.com/Docs/Skype4COM
//
// Also includes CheckBoxes to stop at user logoff and Exit as well as Start Skype if needed.
// Added a Silent Mode Button as well. You can clear the text in the window at anytime by
// using the "Clear" button".
//
// Ways to use this Example program:
//
// 1. Start The Example program with Skype not running and the "Start Skype if not running"
//    checkbox is checked. This will show you how this example program starts Skype and 
//    attaches to the Skype client. You can then change User status, make a test call to
//    Echo123, send a chat message to Echo123 and see that the Example program is processing
//    all the events as it should.
//
// 2. Stop this Example program while Skype is running. Start the Example program and do the
//    same tests as above to see the events processed.
//
// 3. Uncheck the "Start Skype if not running" checkbox. Stop Skype. Stop this example
//    program, then start this example program, and then start Skype. Do the same tests as
//    above and see that the Example program is processing all the events as it should.
//
// 4. Leaving the "Start Skype if not running" checkbox unchecked. Start Skype then start
//    this program, so you can see what happens when Skype is already running vs stopped
//    as in the above example.
//
// 5. By checking the "Stop at Skype Logoff or Exit" you can test to see that this
//    example program goes away when that checkbox is checked when the Skype User logs off
//    or the Skype client is stopped. If you uncheck this checkbox, and the Skype user logs
//    off or the Skype client is stopped you can see how this example program links back up
//    to the Skype client when another logon is tried, or the Skype client is re-started.
//
// 6. By unchecking the "Stop at Skype Logoff or Exit" checkbox, you can see what status
//    messages are sent when a Skype User logs off or exits Skype. You can also see how
//    to re-attach when a Skype User logs back in, or the Skype client is restarted.
//
// 7. Additional logic could be added to this example program if needed to help you isolate
//    and locate problems you may have in another program, and this can be done quickly.
//
// 8. By removing the authorization in the Skype client via Tools -> Options -> Privacy ->
//    Manage other programs Access to Skype, you can use the same tests above to act as if
//    it was a new installation in each case using different checkbox settings and even
//    using the "Silent Mode" button as well.
//
// 9. The "Silent Mode" button can be used in four ways:
//
//    a. If Skype is NOT running and the "Start Skype if not running" checkbox is checked
//       and you click on the "Silent Mode" button. Skype will be started and as soon as
//       this example program attaches to Skype. The Silent Mode request will be processed.
//
//    b. If Skype is NOT running and the "Start Skype if not running" checkbox is NOT
//       checked, the Silent Mode request will be queued until Skype is started and this
//       example program attaches to Skype and then the Silent mode request will be 
//       processed.
//
//    c. If Skype is running, and this example program is attached to Skype then the
//       Silent mode request will be processed immediately.
//
//    d. If Skype is running, but no Skype User is logged in, the Silent Mode request will
//       be queued and processed as soon as this example program is attached to the Skype
//       client ("Once a Skype User Logs On").
//
//    This also shows how you can integrate a Skype interface into your application that
//    can spawn Skype at some later time, not just at your application startup, but based on
//    a button or link click. So as you can see, you can start Skype at anytime, not just
//    at your application startup. If Skype was not running, and your application does other
//    things that does not require the Skype interface, this can be useful to invoke Skype
//    when needed, if it was not running already.  
//
//    NOTE: The Silent Mode request response does timeout, however it does complete. This
//          causes a GUI hang at the moment, a bug report has been filed for this.
//
//10. Try all these above examples and any more you can think of using Auto-Logon and also
//    without automatically logging in when Skype starts. You can see that this example
//    program handles all these situations without throwing any exceptions ("minus the one
//    listed above"). It is important to plan for all the possible situations that your
//    application could encounter while interfacing to Skype and this example program is a
//    way to show you how to deal with these situations gracefully. You can run this example
//    program in debug if needed to see this.     
//
// The intent of this example program was to show methods to handle all the initialization
// possibilities in a C# programming environment using Microsoft Visual C# 2005 Express
// Edition when interfacing to Skype4COMLib. Which is FREE and can be located at:
// 
// http://msdn.microsoft.com/vstudio/express/visualcsharp/
//
// Documentation on Skype4COMLib can be found here:
//
// https://developer.skype.com/Docs/Skype4COM
//
// There are many other status messages and events that Skype4COMLib supports but most of
// the popular ones are used here as examples to get you started with learning how to 
// interface to the Skype4COMLib using C# and at no cost by using the Microsoft Express
// Edition.
//
// Things can get a little tricky when using C# to interface to Skype4COMLib. This is because
// there are some ambiguous references for Attachment and Connection events. This means that
// in order to process these events you really need two attach statements, one for normal
// processing of the other events and one for the Skype class that handles Attachment and
// Connection events. 
//
// Hopefully by looking at this code, and playing with it a bit, you can 
// see why things are done here as they are, any questions just shout.
//
// ZOverLord AKA on Skype as TheUberOverLord
//

namespace SkypePrelude
{
    public partial class Form1 : Form
    {
        // Some flags to check our status information
        public bool Attached = false;
        public bool IsOnline = false;
        public bool WasAttached = false;
        public bool PendingSilentModeStartup = false;
        private string loadedMind = "mind.mdu";
        private PreLudeInterface pi = null;

        // Uses the normal Skype4COMLib using statement
        public Skype skype = new Skype();

        // Use this for ambiguous reference situations like
        // Attachment Status, Connection Status and others
        public SKYPE4COMLib.SkypeClass cSkype;

        public Form1()
        {          
            InitializeComponent();
            
            // Note: These use the Skype using reference
            skype.UserStatus += new _ISkypeEvents_UserStatusEventHandler(Skype_UserStatus);
            skype.CallStatus += new _ISkypeEvents_CallStatusEventHandler(Skype_CallStatus);
            skype.MessageStatus += new _ISkypeEvents_MessageStatusEventHandler(Skype_MessageStatus);

            // Note: These use the Skype class reference due to ambiguous reference issues
            cSkype = new SkypeClass();
            cSkype._ISkypeEvents_Event_AttachmentStatus += new _ISkypeEvents_AttachmentStatusEventHandler(OurAttachmentStatus);
            cSkype._ISkypeEvents_Event_ConnectionStatus += new _ISkypeEvents_ConnectionStatusEventHandler(OurConnectionStatus);

            // Used to catch our form closing so we can save checkbox information
            FormClosing += ByeBye;
            pi = new PreLudeInterface();
            pi.loadedMind = loadedMind;
            pi.isContributable = false;
            pi.isSpeaking = false;
            pi.proactiveMode = true;
            pi.initializeEngine();
        }

        public void ByeBye(object sender, FormClosingEventArgs e)
        {
            // User Settings are saved here (Check Boxes)
            Properties.Settings.Default.Save();
        }

        public void OurAttachmentStatus(TAttachmentStatus status)
        {
            Attached = false;

            // Write Attachment Status to Window
            this.textBox1.AppendText("Attachment Status: " + cSkype.Convert.AttachmentStatusToText(status));
            this.textBox1.AppendText(" - " + status.ToString() + Environment.NewLine);
            this.textBox1.ScrollToCaret();

            if (status == TAttachmentStatus.apiAttachAvailable)
            {
                try
                {
                    // This attaches to the Skype4COM class statement
                    cSkype.Attach(7, true);
                }
                catch (Exception)
                {
                    // All Skype Logic uses TRY for safety
                }
            }
            else
                if (status == TAttachmentStatus.apiAttachSuccess)
                {
                    try
                    {
                        // This attaches to the Skype using reference. We already sent an attach request
                        // for the Skype class in the initialization procedure, which is how this
                        // Attachment success was created. So now that we know we are connected
                        // to the Skype Client, we need to Attach to the Skype Class as well.

                        // If we don't attach to both the Skype using reference and the Skype class
                        // reference we will never get User, Call, or Message events. This can best
                        // be seen by starting Skype and then this Example application. You can see
                        // that when you change your User status, Make a Call, Send or receive chat
                        // Messages, that none of those events will be processed if this was commented
                        // out.

                        // It should be noted that this does cause a slight delay with the GUI when
                        // you start this example program without the DoEvents below.

                        System.Windows.Forms.Application.DoEvents();
                        skype.Attach(7, false);
                    }
                    catch (Exception)
                    {
                        // All Skype Logic uses TRY for safety
                    }

                    Attached = true;
                    WasAttached = true;

                    // If we have a queued Silent Mode request, We are attached, process it now
                    if (PendingSilentModeStartup)
                    {
                        PendingSilentModeStartup = false;
                        try
                        {
                            if (!skype.SilentMode) skype.SilentMode = true;
                        }
                        catch (Exception)
                        {
                            // All Skype Logic uses TRY for safety
                        }
                    }
                }
                else
                    if ((status == TAttachmentStatus.apiAttachNotAvailable) && (Properties.Settings.Default.StopAtLogoffOrExit) && (WasAttached))
                    {
                        this.Close();
                    }                                  
        }

        public void OurConnectionStatus(TConnectionStatus status)
        {
            IsOnline = false;

            // Write Connection Status to Window
            this.textBox1.AppendText("Connection Status: " + cSkype.Convert.ConnectionStatusToText(status));
            this.textBox1.AppendText(" - " + status.ToString() + Environment.NewLine);
            this.textBox1.ScrollToCaret();

            if (status == TConnectionStatus.conOnline)
            {
                IsOnline = true;
            }
        }

        public void Skype_UserStatus(TUserStatus status)
        {
            // Write User Status to Window
            this.textBox1.AppendText("User Status: " + skype.Convert.UserStatusToText(status));
            this.textBox1.AppendText(" - " + status.ToString() + Environment.NewLine);
            this.textBox1.ScrollToCaret();

            if ((status == TUserStatus.cusLoggedOut) && (Properties.Settings.Default.StopAtLogoffOrExit))
            {
                this.Close();
            }
        }

        public void Skype_CallStatus(Call call, TCallStatus status)
        {
            // Write Call Status to Window
            this.textBox1.AppendText("Call Status: " + skype.Convert.CallStatusToText(status));
            this.textBox1.AppendText( " - " + status.ToString() + Environment.NewLine);
            this.textBox1.ScrollToCaret();

            if ((status > TCallStatus.clsUnplaced) && (status < TCallStatus.clsOnHold) && (status != TCallStatus.clsFailed))
            {
                // Could Start or Stop something when a call starts here
            }
            else
            {
                // Could Stop or Resume something when a call stops here
            }
        }

        public void Skype_MessageStatus(IChatMessage ichatmessage, TChatMessageStatus Status)
        {
            // Write Message Status to Window
            //if (ichatmessage.Type  == TChatMessageType.cmeSaid)
            //    return;
            if ((ichatmessage.Status != TChatMessageStatus.cmsReceived))
                return;

            this.textBox1.AppendText("Message Status: " + skype.Convert.ChatMessageStatusToText(Status));
            this.textBox1.AppendText(" - " + Status.ToString() + Environment.NewLine);
            string botsAnswer = pi.chatWithPrelude(ichatmessage.Body);
            this.textBox1.AppendText("Prelude: " + botsAnswer);
            ichatmessage.Chat.SendMessage(botsAnswer);
            this.textBox1.ScrollToCaret();
        }

        public void Example_Initialize(object sender, System.EventArgs e)
        {
            // Initialize Checkboxes from Saved User Settings
            this.checkBox1.Checked = Properties.Settings.Default.StartSkype;
            this.checkBox2.Checked = Properties.Settings.Default.StopAtLogoffOrExit;

            try
            {
                // This attaches to the Skype4COM class statement
                cSkype.Attach(7, false);

                // We don't want to attach to the Skype using statement here otherwise we
                // could prompt twice for authorization, so we wait to do that once we get
                // a success on this. I left this here so that if you wish to uncomment it
                // you can see what happens if you do the attach here.

                // Example Simply go to Tools -> Options -> Privacy ->
                // Manage other programs access to Skype and remove the entry for this
                // example program, click ok, then save. If you uncomment the attach statement
                // below, and recompile, and run this example program you will see that you
                // are prompted twice to allow this to run. If you select the this time only
                // option, you would be prompted twice again, until you allowed it to always
                // run. That is why this attach statement is done when the first one above
                // completes with a success in the Attachment status event handler above that
                // was assigned.

                // This attaches to the Skype using reference
                // skype.Attach(7, false);
            }
            catch (Exception)
            {
                // All Skype Logic uses TRY for safety
            }

            // Check to See if we are going to start Skype based on CheckBox setting

            if (Properties.Settings.Default.StartSkype)
            {
                try
                {
                    if (!skype.Client.IsRunning)
                    {
                        skype.Client.Start(false, true);
                    }
                }
                catch (Exception)
                {
                    // All Skype Logic uses TRY for safety
                }
            }
        }

        public void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                Properties.Settings.Default.StartSkype = true;
            }
            else
            {
                Properties.Settings.Default.StartSkype = false;
            }
        }

        public void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox2.Checked)
            {
                Properties.Settings.Default.StopAtLogoffOrExit = true;
            }
            else
            {
                Properties.Settings.Default.StopAtLogoffOrExit = false;
            }
        }

        public void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://developer.skype.com/Docs/Skype4COM/Example?action=show");
            }
            catch (Exception)
            {
                // Just In case User is offline with no internet connection
            }
        }

        public void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://testing.OnlyTheRightAnswers.com");
            }
            catch (Exception)
            {
                // Just In case User is offline with no internet connection
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if ((skype.Client.IsRunning) && (Attached))
                {
                    if (!skype.SilentMode)
                    {
                        skype.SilentMode = true;
                    }
                }
                else
                    if (!Attached)
                    {
                        PendingSilentModeStartup = true;
                        if ((!skype.Client.IsRunning) && (Properties.Settings.Default.StartSkype)) skype.Client.Start(false, true);
                    }
            }
            catch (Exception)
            {
                // All Skype Logic uses TRY for safety
            }
        }

        public void button2_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "";
        }

    }
}