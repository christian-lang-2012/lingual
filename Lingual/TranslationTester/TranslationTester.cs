using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lingual;
using Lingual.TranslationUtilities;

namespace TranslationTester
{
	public class TranslationTester
	{
		public static void Main(string[] args)
		{
			TranslationUtility tu = TranslationUtility.Instance;
			string output = tu.Translate("en");
			Console.WriteLine(output);
		}
	}
}
