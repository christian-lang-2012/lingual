using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lingual.Enums;
using Lingual.TranslationUtilities;

namespace Testing
{
	class Program
	{
		static void Main(string[] args)
		{
			TranslationUtility tu = TranslationUtility.Instance;

			double value = 123.43;

			Console.WriteLine(tu.TranslateCurrency(value, LocaleEnum.ES));
		}
	}
}
