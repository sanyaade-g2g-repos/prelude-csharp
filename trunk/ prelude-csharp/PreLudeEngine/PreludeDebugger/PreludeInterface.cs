/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 05.12.2004
 * Time: 11:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using SpeechLib;
using DefaultNamespace;
using System.Windows.Forms;

namespace PreLudeEngine
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public class PreLudeInterface
	{
		private preLude.Mind mindInstance = null;
		public string loadedMind = "mind.mdu";
		public bool isContributable = false;
		public bool isSpeaking      = false;
		public bool proactiveMode   = true;
		public bool usesInvalidWordList = false;
		public string invalidWordList = "";
		public int maxMatchesAllowed = 5;
		
		public void initializeEngine()
		{
			mindInstance = new preLude.Mind(loadedMind, false);
			mindInstance.analyzeShortTermMemory();
			mindInstance.proactiveMode = proactiveMode;
			mindInstance.UsesInvalidWordList = this.usesInvalidWordList;
			mindInstance.InvalidWordList = this.invalidWordList;
			mindInstance.MaxMatchesAllowed = this.maxMatchesAllowed;
		}
		
		public string chatWithPrelude(string input)
		{
			if(mindInstance == null) return "Error: Mind not initialized";
			string output = "";
			output = mindInstance.listenToInput(input);
			if(isSpeaking)
				speak(output);
			return output;	
		}
		
		private void speak(string a)
		{
			if(mindInstance == null) return;
			try
            {
            	SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
                SpVoice speech = new SpVoice();
                if (isSpeaking)
                {
                    speech.Speak(a, SpFlags);    
                }
        	}
        	catch
            {
            	MessageBox.Show("Speech Engine error");
            }
		}
		
		public void stopPreludeEngine()
		{
			if(mindInstance == null) return;
			mindInstance.prepareCurrentMemoryForDisc();
			if(isContributable)
			   mindInstance.contributeClientMind();
		}
		
		public void forcedContribution()
		{	
			if(mindInstance == null) return;
			if(isContributable)
			   mindInstance.contributeClientMind();
		}
		//save current mind to disc
		public void forcedSaveMindFile()
		{
			if(mindInstance == null) return;
			mindInstance.prepareCurrentMemoryForDisc();
		}
		
		//save current mind to disc there is another way too!
		public void forcedSaveMindFile(string a)
		{
			if(mindInstance == null) return;
			mindInstance.prepareCurrentMemoryForDisc(a);
		}		
		
		//count currently loaded bot memory
		public int countMindMemory()
		{
			if(mindInstance == null) return -1;
			int i = 0;
			i = mindInstance.MemorySize;
			return i;
		}
		
		public string getVersionInfo()
		{
			return "Prelude@# Engine, version 0.4.0, 2004-05(c) by Lennart Lopin ";
		}
		
		public bool setPreludeClient(string server, int port)
		{
			if(mindInstance != null) return false;
			DefaultNamespace.PreLudeClient.port = port;
			DefaultNamespace.PreLudeClient.server = server;
			return true;
		}
		
		public void setProactiveMode(bool a)
		{
			if(a == true)
				mindInstance.startProactiveMode();
			else
				mindInstance.stopProactiveMode();
			return;
		}
	}
}
