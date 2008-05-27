/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 17.11.2004
 * Time: 22:56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using Ader.Text;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace PreludeEngine
{
	/// <summary>
	/// Description of StringBase.
	/// </summary>
	public class StringBase
	{
		public StringBase()
		{
		}
		//break up string into words
		protected ArrayList tokenizeString(string a)
		{
			ArrayList al = new ArrayList();
			StringTokenizer tok = new StringTokenizer(a);
			tok.IgnoreWhiteSpace = true;
			tok.SymbolChars = new char[]{',', ':', '!', '?', ';', '-'};
		
			Token token;
			do
			{
    			token = tok.Next();
    			
    			if(token.Value != "." && token.Value != "," &&
    			   token.Value != "!" && token.Value != "?" &&
    			   token.Value != "")
    				al.Add(token.Value);
    			
			} while (token.Kind != TokenKind.EOF);
			return al;
		}
		//return how closely an input string resembles a single memory entry
		//break because dont want to count same word re-occurences
		protected int calculateMatchRate(ArrayList input, ArrayList memory)
		{
			int matchRate = 0;
			string cc     = "";
			string bb     = "";
			
			IEnumerator i = input.GetEnumerator();
			while(i.MoveNext())
			{
				cc = (string)i.Current;
				cc = cc.ToLower();
				
				IEnumerator m = memory.GetEnumerator();
				while(m.MoveNext())
				{
					bb = (string)m.Current;
					bb = bb.ToLower();
					if(cc == bb)
					{
						matchRate++; break;
					} 
				}
				
			}
			return matchRate;
		}
		
		protected ArrayList removeRedundantEntries(ArrayList a)
		{
			IEnumerator i = a.GetEnumerator();
			while(i.MoveNext())
			{
				;
			}
			return a;
		}
		
	}
}
