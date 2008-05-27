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

namespace preLude
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
			tok.SymbolChars = new char[]{',', ':', '!', '?'};

			Token token;
			do
			{
    			token = tok.Next();
    			al.Add(token.Value);
    			
			} while (token.Kind != TokenKind.EOF);
			return al;
		}
		//return how closely an input string resembles a single memory entry
		protected int calculateMatchRate(ArrayList input, ArrayList memory)
		{
			int matchRate = 0;
			IEnumerator i = input.GetEnumerator();
			IEnumerator m = memory.GetEnumerator();
			while(i.MoveNext())
			{
				while(m.MoveNext())
				{
					SortedList isNewWord = new SortedList();
					string cc = (string)i.Current;
					string bb = (string)m.Current;
					cc.ToLower();
					bb.ToLower();
					//mehrfachwertung für ein wort vermeiden z.b. eine 3 für 3x "ich"
					if(!isNewWord.Contains(bb))
					{
						isNewWord.Add(bb, bb);
						if(cc == bb)
						{
							matchRate++;
						}
					}
					isNewWord.Clear();
				}
			}
			return matchRate;
		}
		
		
	}
}
