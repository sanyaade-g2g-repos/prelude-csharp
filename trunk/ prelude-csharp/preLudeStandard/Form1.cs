using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace WindowsApplication1
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private AxAgentObjects.AxAgent axAgent1;
		private AgentObjects.IAgentCtlCharacter speaker;
		private System.Windows.Forms.Button speak;
		private System.Windows.Forms.RichTextBox talk;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.axAgent1 = new AxAgentObjects.AxAgent();
			this.speak = new System.Windows.Forms.Button();
			this.talk = new System.Windows.Forms.RichTextBox();
			((System.ComponentModel.ISupportInitialize)(this.axAgent1)).BeginInit();
			this.SuspendLayout();
			// 
			// axAgent1
			// 
			this.axAgent1.Enabled = true;
			this.axAgent1.Location = new System.Drawing.Point(0, 232);
			this.axAgent1.Name = "axAgent1";
			this.axAgent1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAgent1.OcxState")));
			this.axAgent1.Size = new System.Drawing.Size(32, 32);
			this.axAgent1.TabIndex = 0;
			// 
			// speak
			// 
			this.speak.Location = new System.Drawing.Point(192, 176);
			this.speak.Name = "speak";
			this.speak.Size = new System.Drawing.Size(168, 48);
			this.speak.TabIndex = 1;
			this.speak.Text = "speak";
			this.speak.Click += new System.EventHandler(this.speak_Click);
			// 
			// talk
			// 
			this.talk.Location = new System.Drawing.Point(24, 24);
			this.talk.Name = "talk";
			this.talk.Size = new System.Drawing.Size(328, 136);
			this.talk.TabIndex = 2;
			this.talk.Text = "";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(376, 270);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.talk,
																		  this.speak,
																		  this.axAgent1});
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.axAgent1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			try
			{				
			 this.axAgent1.Characters.Load("Sarah" , "SarahJane.acs");    //load the character  in the axAgent1 object  -- axAgent1 can load more than one character

				this.speaker = this.axAgent1.Characters["Sarah"];     //give the speaker object the character to show it
				this.speaker.Show(0);
			}
			catch(FileNotFoundException)   //if the charater not found  // using IO 
			{
				MessageBox.Show("Invalid charater location");
			}
		}

		private void speak_Click(object sender, System.EventArgs e)
		{
			if(this.talk.Text != "")
		      this.speaker.Speak(this.talk.Text , null);

			else 
				this.speaker.Speak("what should i say", null);




		}
	}
}

/*In the speaker objects you will find some useful function that help you to control the character such as :
  MoveTo( int x , int y , objectspeed speed);

  Play(string animation);   // this function make the character to play some animation 
                            //The possible animations that the character can play it are
/******************
RestPose, Wave, DontRecognize, Uncertain, Decline,Sad, StopListening, GetAttention, GetAttentionReturn,Blink, Idle3_2, Surprised,Congratulate_2,Reading,Announce,Read ,ReadReturn,Idle2_2  ,Writing ,Write ,WriteReturn ,Congratulate ,Confused ,Suggest ,MoveRight, MoveLeft,Idle2_1,  MoveUp, MoveDown, StartListening, WriteContinued, DoMagic1, DoMagic2,Idle1_1,  LookDown, LookDownBlink, LookDownReturn, LookLeft, LookLeftBlink, LookLeftReturn,Idle1_3,  LookRight, LookRightBlink, LookRightReturn, LookUp, LookUpBlink,LookUpReturn,Idle1_2,ReadContinued, Pleased, GetAttentionContinued, Process, Search, Think,Idle1_4,Greet,Idle3_1,GestureUp,GestureDown,GestureLeft,GestureRight,Show,Hide,Hearing_4,Hearing_1,Hearing_2,Hearin,
Alert,Explain,Processing,Thinking,Searching,Acknowledge
*********************/


