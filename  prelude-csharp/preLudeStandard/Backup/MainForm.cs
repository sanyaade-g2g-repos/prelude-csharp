/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 17.11.2004
 * Time: 20:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using SpeechLib;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using AMS.Profile;
using PreludeEngine;

namespace preLude
{
  		
	/// <summary>
	/// Description of MainForm.	
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MenuItem menuVoiceTraining;
		private System.Windows.Forms.MenuItem menuToolsCount;
		private System.Windows.Forms.MenuItem menuHelpAbout;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuToolsEdit;
		private System.Windows.Forms.MenuItem menuFileSave;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel1;
		private AxAgentObjects.AxAgent axAgent1;
		private System.Windows.Forms.MenuItem menuNew;
		private System.Windows.Forms.MenuItem menuHelp;
		private System.Windows.Forms.Panel panelBottom;
		private System.Windows.Forms.MenuItem menuToolAgent;
		private System.Windows.Forms.MenuItem menuFileOpen;
		private System.Windows.Forms.Timer idleControl;
		private System.Windows.Forms.MenuItem menuFileClose;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.MenuItem menuListen;
		private System.Windows.Forms.MenuItem menuFileSaveAs;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.MenuItem menuHelpUpdate;
		private System.Windows.Forms.MenuItem menuSettingsContribute;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.Timer agentSpeakingChecker;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.Button sendButton;
		private System.Windows.Forms.RichTextBox outputBox;
		private System.Windows.Forms.MenuItem menu_autoSpeak;
		private System.Windows.Forms.RichTextBox inputBox;
		const  string strFilter = "Prelude memory files(*.mdu)|*.mdu";
		private PreLudeInterface pi = null;
		private static int presentLine = 0;
		private string loadedMind = "mind.mdu";	
		private bool enabledAgent = false;
		private AgentObjects.IAgentCtlCharacterEx Character;
		private AgentObjects.IAgentCtlRequest charReq;
		private string lastAgent = "";
		
		public  bool autoSpeaking = false;  
		public  bool voiceRecognition = false;
		//SR
		private const int grammarId = 10;
		private bool speechInitialized = false;
		private SpeechLib.SpSharedRecoContext   objRecoContext;
		private SpeechLib.ISpeechRecoGrammar    grammar;
		//private bool SpeechRuleState = false;
		
		//coords
		private int bottom = 0;
		private int right  = 0;
		
		/// <summary>
		/// TODO: wenn eingabe in animationsliste, play animation - wenn nicht, random
		/// TODO: save last agent in registry
		/// TODO: if found melissa, start melissa, else merlin
		/// </summary>
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			StartPosition = FormStartPosition.CenterScreen;
			
			InitializeComponent();
		      
			pi = new PreLudeInterface();
			pi.loadedMind = loadedMind;
			pi.isContributable = false;
			pi.isSpeaking = false;
			pi.proactiveMode = true;
			//pi.reportBoredom += new PreLudeInterface.AutoSpeakHandler(iamBored);			
			pi.initializeEngine();
			menuSettingsContribute.Checked = false;
			axAgent1.Hide();
			lastAgent = LoadLastAgent();
			inputBox.Focus();
			inputBox.Clear();
			if(lastAgent != "")
			{
				LoadAgent(lastAgent);
			}
			idleControl.Start();
			agentSpeakingChecker.Start();
		}
		
		[STAThread]
		public static void Main(string[] args)
		{
			try
			{
				Application.Run(new MainForm());
				//MainForm.ActiveForm.TransparencyKey = SystemColors.Control;
			}
			catch(System.Exception ex)
			{
				MessageBox.Show("Uups! An error occured - " + ex.ToString());
			}
		}
		
		#region Windows Forms Designer generated code
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.inputBox = new System.Windows.Forms.RichTextBox();
			this.menu_autoSpeak = new System.Windows.Forms.MenuItem();
			this.outputBox = new System.Windows.Forms.RichTextBox();
			this.sendButton = new System.Windows.Forms.Button();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.agentSpeakingChecker = new System.Windows.Forms.Timer(this.components);
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuSettingsContribute = new System.Windows.Forms.MenuItem();
			this.menuHelpUpdate = new System.Windows.Forms.MenuItem();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.menuFileSaveAs = new System.Windows.Forms.MenuItem();
			this.menuListen = new System.Windows.Forms.MenuItem();
			this.panel4 = new System.Windows.Forms.Panel();
			this.menuFileClose = new System.Windows.Forms.MenuItem();
			this.idleControl = new System.Windows.Forms.Timer(this.components);
			this.menuFileOpen = new System.Windows.Forms.MenuItem();
			this.menuToolAgent = new System.Windows.Forms.MenuItem();
			this.panelBottom = new System.Windows.Forms.Panel();
			this.menuHelp = new System.Windows.Forms.MenuItem();
			this.menuNew = new System.Windows.Forms.MenuItem();
			this.axAgent1 = new AxAgentObjects.AxAgent();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuFileSave = new System.Windows.Forms.MenuItem();
			this.menuToolsEdit = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuHelpAbout = new System.Windows.Forms.MenuItem();
			this.menuToolsCount = new System.Windows.Forms.MenuItem();
			this.menuVoiceTraining = new System.Windows.Forms.MenuItem();
			this.panel4.SuspendLayout();
			this.panelBottom.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.axAgent1)).BeginInit();
			this.panel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// inputBox
			// 
			this.inputBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.inputBox.Location = new System.Drawing.Point(0, 0);
			this.inputBox.Name = "inputBox";
			this.inputBox.Size = new System.Drawing.Size(365, 76);
			this.inputBox.TabIndex = 1;
			this.inputBox.Text = "";
			this.inputBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputBoxKeyDown);
			this.inputBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.InputBoxKeyPress);
			// 
			// menu_autoSpeak
			// 
			this.menu_autoSpeak.Index = 2;
			this.menu_autoSpeak.RadioCheck = true;
			this.menu_autoSpeak.Text = "Auto Speaking";
			this.menu_autoSpeak.Click += new System.EventHandler(this.Menu_autoSpeakClick);
			// 
			// outputBox
			// 
			this.outputBox.BackColor = System.Drawing.Color.White;
			this.outputBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.outputBox.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World);
			this.outputBox.Location = new System.Drawing.Point(0, 0);
			this.outputBox.Name = "outputBox";
			this.outputBox.Size = new System.Drawing.Size(445, 171);
			this.outputBox.TabIndex = 9;
			this.outputBox.Text = "";
			// 
			// sendButton
			// 
			this.sendButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.sendButton.Location = new System.Drawing.Point(5, 0);
			this.sendButton.Name = "sendButton";
			this.sendButton.Size = new System.Drawing.Size(64, 74);
			this.sendButton.TabIndex = 2;
			this.sendButton.Text = "&Send";
			this.sendButton.Click += new System.EventHandler(this.SendButtonClick);
			// 
			// agentSpeakingChecker
			// 
			this.agentSpeakingChecker.Interval = 50;
			this.agentSpeakingChecker.Tick += new System.EventHandler(this.AgentSpeakingCheckerTick);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
						this.menuToolsCount,
						this.menuToolsEdit,
						this.menuToolAgent});
			this.menuItem2.Text = "Tools";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 2;
			this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
						this.menuListen,
						this.menuVoiceTraining,
						this.menu_autoSpeak,
						this.menuSettingsContribute});
			this.menuItem3.Text = "Settings";
			// 
			// menuSettingsContribute
			// 
			this.menuSettingsContribute.Index = 3;
			this.menuSettingsContribute.Text = "Sync Mind ...";
			this.menuSettingsContribute.Click += new System.EventHandler(this.MenuSettingsContributeClick);
			// 
			// menuHelpUpdate
			// 
			this.menuHelpUpdate.Index = 2;
			this.menuHelpUpdate.Text = "Update";
			this.menuHelpUpdate.Click += new System.EventHandler(this.MenuHelpUpdateClick);
			// 
			// menuFileSaveAs
			// 
			this.menuFileSaveAs.Index = 3;
			this.menuFileSaveAs.Text = "Save as...";
			this.menuFileSaveAs.Click += new System.EventHandler(this.MenuFileSaveAsClick);
			// 
			// menuListen
			// 
			this.menuListen.Index = 0;
			this.menuListen.Text = "Listen!";
			this.menuListen.Click += new System.EventHandler(this.MenuListenClick);
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.inputBox);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(8, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(365, 76);
			this.panel4.TabIndex = 4;
			// 
			// menuFileClose
			// 
			this.menuFileClose.Index = 4;
			this.menuFileClose.Text = "Close";
			this.menuFileClose.Click += new System.EventHandler(this.MenuFileCloseClick);
			// 
			// idleControl
			// 
			this.idleControl.Interval = 50000;
			this.idleControl.Tick += new System.EventHandler(this.IdleControlTick);
			// 
			// menuFileOpen
			// 
			this.menuFileOpen.Index = 1;
			this.menuFileOpen.Text = "Open";
			this.menuFileOpen.Click += new System.EventHandler(this.MenuFileOpenClick);
			// 
			// menuToolAgent
			// 
			this.menuToolAgent.Index = 2;
			this.menuToolAgent.Text = "Load Agent";
			this.menuToolAgent.Click += new System.EventHandler(this.MenuToolAgentClick);
			// 
			// panelBottom
			// 
			this.panelBottom.Controls.Add(this.panel4);
			this.panelBottom.Controls.Add(this.panel3);
			this.panelBottom.Controls.Add(this.panel2);
			this.panelBottom.Controls.Add(this.statusBar);
			this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelBottom.Location = new System.Drawing.Point(0, 171);
			this.panelBottom.Name = "panelBottom";
			this.panelBottom.Size = new System.Drawing.Size(445, 97);
			this.panelBottom.TabIndex = 5;
			// 
			// menuHelp
			// 
			this.menuHelp.Index = 0;
			this.menuHelp.Text = "Help";
			this.menuHelp.Click += new System.EventHandler(this.MenuHelpClick);
			// 
			// menuNew
			// 
			this.menuNew.Index = 0;
			this.menuNew.Text = "New";
			this.menuNew.Click += new System.EventHandler(this.MenuNewClick);
			// 
			// axAgent1
			// 
			this.axAgent1.ContainingControl = this;
			this.axAgent1.Enabled = true;
			this.axAgent1.Location = new System.Drawing.Point(32, 97);
			this.axAgent1.Name = "axAgent1";
			this.axAgent1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAgent1.OcxState")));
			this.axAgent1.Size = new System.Drawing.Size(32, 32);
			this.axAgent1.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.axAgent1);
			this.panel1.Location = new System.Drawing.Point(448, 22);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(104, 141);
			this.panel1.TabIndex = 2;
			// 
			// panel2
			// 
			this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(8, 76);
			this.panel2.TabIndex = 2;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.sendButton);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel3.Location = new System.Drawing.Point(373, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(72, 76);
			this.panel3.TabIndex = 3;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
						this.menuItem1,
						this.menuItem2,
						this.menuItem3,
						this.menuItem4});
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 76);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(445, 21);
			this.statusBar.TabIndex = 0;
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 3;
			this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
						this.menuHelp,
						this.menuHelpUpdate,
						this.menuHelpAbout});
			this.menuItem4.Text = "?";
			// 
			// menuFileSave
			// 
			this.menuFileSave.Index = 2;
			this.menuFileSave.Text = "Save";
			this.menuFileSave.Click += new System.EventHandler(this.MenuFileSaveClick);
			// 
			// menuToolsEdit
			// 
			this.menuToolsEdit.Index = 1;
			this.menuToolsEdit.Text = "Edit memory";
			this.menuToolsEdit.Click += new System.EventHandler(this.MenuToolsEditClick);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
						this.menuNew,
						this.menuFileOpen,
						this.menuFileSave,
						this.menuFileSaveAs,
						this.menuFileClose});
			this.menuItem1.Text = "File";
			// 
			// menuHelpAbout
			// 
			this.menuHelpAbout.Index = 1;
			this.menuHelpAbout.Text = "About";
			this.menuHelpAbout.Click += new System.EventHandler(this.MenuHelpAboutClick);
			// 
			// menuToolsCount
			// 
			this.menuToolsCount.Index = 0;
			this.menuToolsCount.Text = "Count memory";
			this.menuToolsCount.Click += new System.EventHandler(this.MenuToolsCountClick);
			// 
			// menuVoiceTraining
			// 
			this.menuVoiceTraining.Index = 1;
			this.menuVoiceTraining.Text = "Voice training ...";
			this.menuVoiceTraining.Click += new System.EventHandler(this.MenuVoiceTrainingClick);
			// 
			// MainForm
			// 
			this.AccessibleName = " ";
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(445, 268);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.outputBox);
			this.Controls.Add(this.panelBottom);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.Name = "MainForm";
			this.Text = "preLude @ #";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.MainFormClosing);
			this.panel4.ResumeLayout(false);
			this.panelBottom.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.axAgent1)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion
		
		void InputBoxKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			
		}
		
		void InputBoxKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{			
			if(e.KeyChar == (char) 13)
			{	
				doChatting();
			}
		}
		
		#region speech support
		void MenuSettingsSpeakClick(object sender, System.EventArgs e)
		{
//			if(menuSettingsSpeak.Checked)
//			{
//				menuSettingsSpeak.Checked = false;
//				pi.isSpeaking = false;
//			}
//			else
//			{
//				menuSettingsSpeak.Checked = true;
//				pi.isSpeaking = true;
//			}
		}
		#endregion  
		
		
		void MainFormClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			pi.stopPreludeEngine();
			if(menuSettingsContribute.Checked)
			   pi.forcedContribution();
		}
		
		#region Menu Item Handling
		
		void MenuHelpAboutClick(object sender, System.EventArgs e)
		{
			DefaultNamespace.AboutBox ab = new DefaultNamespace.AboutBox();
			ab.ShowDialog();
		}
		
		void MenuToolsCountClick(object sender, System.EventArgs e)
		{
			string u = Convert.ToString(pi.countMindMemory());
			outputBox.AppendText("Bot's current memory size: " + u + " \n");
		    presentLine++;
		}
		
		void MenuToolsEditClick(object sender, System.EventArgs e)
		{
		    string currDir  = Directory.GetCurrentDirectory();
			string filePath = currDir + "\\" + loadedMind;
			Process myProcess = new Process();
			myProcess.StartInfo.FileName = "Notepad";
			myProcess.StartInfo.Arguments = loadedMind;
			myProcess.Start();
		}
		
		void MenuFileSaveClick(object sender, System.EventArgs e)
		{
			pi.forcedSaveMindFile();
		}
		
		void MenuFileSaveAsClick(object sender, System.EventArgs e)
		{
			saveFileDialog1.Filter = strFilter;
			saveFileDialog1.ShowDialog();
			if(saveFileDialog1.FileName != "")
			{
				pi.forcedSaveMindFile(saveFileDialog1.FileName);
			}
		}
		
		void MenuFileOpenClick(object sender, System.EventArgs e)
		{
			openFileDialog1.Filter = strFilter;
			openFileDialog1.ShowDialog();
			if(openFileDialog1.FileName != "")
			{
				pi = new PreLudeInterface();
				loadedMind = openFileDialog1.FileName;
				pi.initializeEngine();
			}
		}
		
		void MenuFileCloseClick(object sender, System.EventArgs e)
		{
			this.Close();
		}
		
		void MenuSettingsContributeClick(object sender, System.EventArgs e)
		{
			
			if(menuSettingsContribute.Checked)
			{
				menuSettingsContribute.Checked = false;
				pi.isContributable = false;
			}
			else
			{
				menuSettingsContribute.Checked = true;			
				pi.isContributable = true;
			}
		}
		#endregion
		
	     private void GoToLineAndColumn(RichTextBox RTB, int Line, int Column)
	     {
	          int offset = 0;
	          for (int i = 0; i < Line - 1 && i < RTB.Lines.Length; i++)
	          {
	               offset += RTB.Lines[i].Length + 1;
	          }
	          RTB.Focus();
	          if(Character != null && lastAgent != "")
	          	RTB.Select(offset + Column, Character.Name.Length);
	          else
	          	RTB.Select(offset + Column, 4);
	    }		
	    
	     
	     
	     void MenuToolAgentClick(object sender, System.EventArgs e)
	     {
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.AddExtension = true;
			openFileDialog.Filter = "Microsoft Agent Characters (*.acs)|*.acs";
			openFileDialog.FilterIndex = 1 ;
			openFileDialog.RestoreDirectory = true ;
			
			if(openFileDialog.ShowDialog() != DialogResult.OK)
				return;
			
			LoadAgent(openFileDialog.FileName);
	     }
	    	     
	     void SendButtonClick(object sender, System.EventArgs e)
	     {
	     	doChatting();
	     }
	     
	     private void doChatting()
	     {
	     	idleControl.Stop();
	     	string output = "";
	     	presentLine++;
			string cleanInputText = inputBox.Text.Trim('\n');
			output = pi.chatWithPrelude(cleanInputText);
			outputBox.AppendText("You: \t" + cleanInputText);
			GoToLineAndColumn(outputBox, presentLine, 0);
			outputBox.SelectionColor = Color.Orange;
			outputBox.AppendText("\n");
			presentLine++;
			if(Character != null && lastAgent != "")
				outputBox.AppendText(Character.Name + ": \t" + output);
			else
				outputBox.AppendText("CPU: \t" + output);
			GoToLineAndColumn(outputBox, presentLine, 0);
			outputBox.SelectionColor = Color.Blue;
			outputBox.AppendText("\n");
			outputBox.Focus();
			inputBox.Text   = "";
			inputBox.Focus();
			try
			{
				if(this.Character != null && enabledAgent == true)
				{
					charReq = this.Character.Speak(output, null);
				}
			}
			catch(System.Exception e)
			{
				MessageBox.Show("Error occured: " + e.ToString());
			}
			idleControl.Start();
	     }
	     
	     void Menu_autoSpeakClick(object sender, System.EventArgs e)
	     {
	     	if(autoSpeaking)
	     	{
	     		autoSpeaking = false;
	     		menu_autoSpeak.Checked = false;
	     		statusBar.Text = "AutoSpeaking disabled";
	     	}
	     	else
	     	{
	     		autoSpeaking = true;
	     		menu_autoSpeak.Checked = true;
	     		statusBar.Text = "AutoSpeaking enabled";
	     	}
	     }
	     
		private void iamBored(string a)
		{
			if(autoSpeaking)
			{
				pi.speak(a);
				outputBox.AppendText("Prelude to herself: " + a + "\n");
				presentLine++;
				outputBox.AppendText("Prelude bored: " + pi.chatWithPrelude(a) + "\n");
				presentLine++;
			}
		}	
		
	     
		void MenuNewClick(object sender, System.EventArgs e)
		{
			//Create new mind file - start from zero
		}
		
		void MenuListenClick(object sender, System.EventArgs e)
		{
			//activate Voice recognition
			if(voiceRecognition)
	     	{
	     		voiceRecognition = false;
	     		menuListen.Checked = false;
	     		if(grammar != null)
	     			grammar.DictationSetState(SpeechLib.SpeechRuleState.SGDSInactive);
	     		statusBar.Text = "Speech recognition disabled";
	     	}
	     	else
	     	{
	     		EnableSpeech();
	     		voiceRecognition = true;
	     		menuListen.Checked = true;
	     		statusBar.Text = "Speech recognition enabled";
	     	}
			
		}
		
		private bool EnableSpeech()
		{
			
			if (speechInitialized == false)
			{
				InitializeSpeech();
			}
	
			objRecoContext.State = SpeechRecoContextState.SRCS_Enabled;
			return true;
		}
		
		private  void InitializeSpeech()
		{
			try
			{
				// First of all, let's create the main reco context object. 
				objRecoContext = new SpeechLib.SpSharedRecoContext();
				
				// Then, let's set up the event handler. We only care about
				// Hypothesis and Recognition events in this sample.
				
				objRecoContext.Recognition += new 
					_ISpeechRecoContextEvents_RecognitionEventHandler(
					RecoContext_Recognition);

				grammar = objRecoContext.CreateGrammar(grammarId);
				grammar.DictationLoad("", SpeechLoadOption.SLOStatic);
				grammar.DictationSetState(SpeechLib.SpeechRuleState.SGDSActive);

				speechInitialized = true;
				statusBar.Text = "Speech recognition initialized";
			}
			catch(Exception e)
			{
				MessageBox.Show(
					"Exception caught when initializing SAPI." 
					+ " This application may not run correctly.\r\n\r\n"
					+ e.ToString(),
					"Error");
			}
		}

		public  void RecoContext_Recognition(int StreamNumber, object StreamPosition, SpeechRecognitionType RecognitionType, ISpeechRecoResult Result)
		{
			//MessageBox.Show(charReq.Status.ToString());
			if(charReq.Status == 0)
			{
				grammar.DictationSetState(SpeechRuleState.SGDSInactive);
				
				inputBox.Text = Result.PhraseInfo.GetText(0, -1, true);
				
				doChatting();
				
				grammar.DictationSetState(SpeechLib.SpeechRuleState.SGDSActive);
			}
		}
		
		void MenuVoiceTrainingClick(object sender, System.EventArgs e)
		{
			if(objRecoContext == null)
			   objRecoContext = new SpeechLib.SpSharedRecoContext();	
			object obstr = null;
			objRecoContext.Recognizer.DisplayUI(this.Handle.ToInt32(), "Prelude@# Voice training", "UserTraining", ref obstr); 
				
		}
		
		void MenuHelpClick(object sender, System.EventArgs e)
		{
			//Help HTML page
		}
		
		private void SetLastAgent(string characterName)
		{
			Xml config = new Xml();
			config.SetValue("CharacterParams", "LastCharacter", characterName);
		}
		
		private string LoadLastAgent()
		{
			string name = "";
			Xml config = new Xml();
			name = (String)config.GetValue("CharacterParams", "LastCharacter");
			return name;
		}
		
		private void LoadAgent(string pathToAcsFile)
		{
			try { axAgent1.Characters.Unload("CharacterID"); }
			catch { }
			try
			{
				SetLastAgent(pathToAcsFile);
				axAgent1.Characters.Load("CharacterID", (object)pathToAcsFile);
				Character = axAgent1.Characters["CharacterID"];
				Character.Show(null); 
				Character.Balloon.Style = 0;
				Character.Left = Convert.ToInt16(this.Right);
				//Character.Left = Convert.ToInt16(right - Character.Width);
				Character.Top = Convert.ToInt16(this.bottom);
				//Character.Top = Convert.ToInt16(bottom + Character.Height);
				//
				Character.Speak("Hello, i wanna chat with you", null);
				enabledAgent = true;
				if(pi != null)
					pi.isSpeaking = false;
				//menuSettingsSpeak.Checked = false;
				statusBar.Text = "Loaded MsAgent: " + Character.Name;
				
			}
			catch(System.Exception ex)
			{
				MessageBox.Show("Uh uh, i encountered an error: " + ex.ToString());
			}
		}
		
		void IdleControlTick(object sender, System.EventArgs e)
		{
			//if nothing happens for some time, start to play random
			//animations, to draw back attention
			if(Character == null)
				return;
			idleControl.Stop();
			IEnumerator ie = Character.AnimationNames.GetEnumerator();
			while(ie.MoveNext())
			{
				Character.Play(ie.Current.ToString());
					idleControl.Stop();
			}
			
	//		if(Character != null)
			/*	Character.Play("Smile");
				Character.Play("Blink");
				Character.Play("Restpose");
				Character.Play("Show");
				Character.Play("Hide");
	*/
	
	
	
			idleControl.Start();
		}
		
		void AgentSpeakingCheckerTick(object sender, System.EventArgs e)
		{
			if(charReq != null)
			{
				statusBar.Text = "Agent status: " + charReq.Status.ToString();
				statusBar.Text += " current mind size: " + pi.countMindMemory().ToString();
				if((charReq.Status == 4))
				{
					if(grammar != null)
						grammar.DictationSetState(SpeechLib.SpeechRuleState.SGDSInactive);
					//SpeechRuleState = false;
				}
				else if((charReq.Status == 0))
				{
					if(grammar != null)
						grammar.DictationSetState(SpeechLib.SpeechRuleState.SGDSActive);
					//SpeechRuleState = false;
				}
			}
		}
		
		void MenuHelpUpdateClick(object sender, System.EventArgs e)
		{
				System.Diagnostics.Process.Start("IExplore"," http://prelude.lennart-lopin.de/download.htm");
		}
		
	}	
}
/*
 * Blink
Cry
Furious
Furious2
Hide
LookLeft
LookRight
Meditate
Perplex
Pleased
Restpose
Sad
Scared
Show
Smile
Surprised
Think
 * */
