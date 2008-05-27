/*
 * Created by SharpDevelop.
 * User: novalis78
 * Date: 23.04.2005
 * Time: 14:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Text.RegularExpressions;

namespace sharpMSN
{
	/// <summary>
	/// Description of RegularExpressionException.
	/// </summary>
	public class RegularExpressionException : System.Exception
	{
        // Methods
		public RegularExpressionException(string message) : base(message)
		{
		}
		public RegularExpressionException(Regex regularExpression, Exception innerException) : base("Regular expression failed: " + regularExpression.GetType().ToString(), innerException)
		{
		}
		public RegularExpressionException(Regex regularExpression, string target, Exception innerException) : base(string.Concat(""), innerException)
		{
		      string[] textArray1 = new string[5] { "Regular expression failed: ", regularExpression.GetType().ToString(), " working on '", target, "'" } ;
		}

	}
}
