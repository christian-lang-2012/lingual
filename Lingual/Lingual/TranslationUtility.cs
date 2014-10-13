using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lingual
{
	public class TranslationUtility : Translator
	{

		private static File _translationFile;

		public static File TranslationFile
		{
			get
			{
				
			}
		}

		public string translate(string key)
		{
			throw new NotImplementedException();
		}

		public string translate(string key, string locale)
		{
			throw new NotImplementedException();
		}

		public string translate(string key, params string[] arguments)
		{
			throw new NotImplementedException();
		}

		public string translate(string key, string locale, params string[] arguments)
		{
			throw new NotImplementedException();
		}
	}
}
