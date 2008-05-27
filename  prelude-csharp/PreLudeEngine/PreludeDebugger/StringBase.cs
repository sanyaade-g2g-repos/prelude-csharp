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

namespace preLude
{
	/// <summary>
	/// Description of StringBase.
	/// </summary>
	public class StringBase
	{
		private int isWordGroup = -1;
		private string invalidWordList = " i, am, are, is, he, she, it," +
			"we, they, them, their, in, of, at, from, to, too";
		private bool usesInvalidWordList = false;
		
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
				if(usesInvalidWordList)
				{
					if(invalidWordList.IndexOf(cc) != -1)
						continue;
				}
				
				IEnumerator m = memory.GetEnumerator();
				int x = 0;
				while(m.MoveNext())
				{
					bb = (string)m.Current;
					bb = bb.ToLower();
					//optimization over word list:
					if(usesInvalidWordList)
					{
						if(invalidWordList.IndexOf(bb) != -1)
							continue;
					}
						
					x++;
					if(cc == bb)
					{
						matchRate++;
						//optimization over word groups
						if(isWordGroup == x)
							matchRate++; //bonus
						isWordGroup = x+1;
					}
					else
						isWordGroup = -1;
				}
				
			}
			return matchRate;
		}
		
		
		public bool UsesInvalidWordList {
			set {
				usesInvalidWordList = value;
			}
		}
		
		public string InvalidWordList {
			set {
				invalidWordList = value;
			}
		}
		
		
	}
}
