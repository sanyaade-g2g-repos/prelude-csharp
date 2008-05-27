/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 12.12.2004
 * Time: 21:59
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using preLude;
using System.Runtime.InteropServices;
using DefaultNamespace;
using System.Reflection;

[assembly:AssemblyKeyFile("prelude.snk")]
namespace PreludeLib 
{

	public interface IPreludeServer 
	{
		String LoadedMind{get; set;}
		bool IsContributable{get; set;}
		bool IsSpeaking{get; set;}		
      
		void initializeEngine();
		String chatWithPrelude(string a);
		void stopPreludeEngine();
    }

	[ClassInterface(ClassInterfaceType.AutoDual)]
	[Guid("CAD5F914-C8CB-4657-B89A-2D1D9342D963")]
	public class PreludeServer : IPreludeServer
	{
		private Mind mindInstance;
		private String loadedMind = "mind.mdu";
		private bool isContributable;
		private bool isSpeaking;
		public  String blub = "hello";
		
		public PreludeServer(){}
		public String LoadedMind{
            get { return loadedMind; }
            set { loadedMind = value; }
        }
		public bool IsContributable{
			get { return isContributable; }
			set { isContributable = value; }
		}
		public bool IsSpeaking {
			get { return isSpeaking; }
			set { isSpeaking = value; }
		}
			
		public void initializeEngine()
		{
			mindInstance = new preLude.Mind(loadedMind, false);
			mindInstance.analyzeShortTermMemory();
			return;
		}
		
		public String chatWithPrelude(String  input)
		{
			if(mindInstance == null) return "Error: Mind not initialized";
			string output = "";
			output = mindInstance.listenToInput(input);
			//if(isSpeaking)
			//	speak(output);
			return output;
		}
		
		public void stopPreludeEngine()
		{
			if(mindInstance == null) return;
			mindInstance.prepareCurrentMemoryForDisc();
			//if(isContributable)
			//   mindInstance.contributeClientMind();
			return;
		}
		
	}
}
