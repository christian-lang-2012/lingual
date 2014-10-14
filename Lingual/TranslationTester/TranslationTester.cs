using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lingual;

namespace TranslationTester
{
	public class TranslationTester
	{
		public static void Main(string[] args)
		{
			TranslationUtility tu = TranslationUtility.Instance;
			tu.CurrentLocale = LocaleEnum.EN;
			string output = tu.Translate("account.invalid.message", LocaleEnum.EN);
			Console.WriteLine(output);
		}
	}
}
