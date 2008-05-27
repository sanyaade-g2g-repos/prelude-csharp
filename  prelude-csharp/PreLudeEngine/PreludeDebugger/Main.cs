/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 18.12.2004
 * Time: 10:19
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using PreLudeEngine;

namespace PreludeDebugger
{
	class MainClass
	{
		private static string ind = "";
		public static void Main(string[] args)
		{
			Console.WriteLine("Prelude@# (0.5.0) command line version, welcome user!");
			Console.WriteLine("if you want to stop chatting, enter: 'exit'");
			//initialize interface
			PreLudeInterface pi = new PreLudeInterface();
			//define path to mind file
			pi.loadedMind = "mind.mdu";	
			pi.usesInvalidWordList = false;
			//start your engine ...
			pi.initializeEngine();
			pi.maxMatchesAllowed = 3;
			pi.setProactiveMode(true);
			//here we go:
			while(!ind.StartsWith("exit"))
			{
				Console.Write("You say: ");
				ind = Console.ReadLine();
				Console.WriteLine("Prelude says: " + pi.chatWithPrelude(ind));
			}
			pi.stopPreludeEngine();
		}
	}
}
