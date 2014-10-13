using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lingual
{
	public class TranslationUtility
	{
		//private static string LOCALE_PATH = "/locale/";

		private List<TranslationHash> translationHashes; 

		private static TranslationUtility _instance;

		private TranslationUtility()
		{
			setUpTranslationHashes();
		}

		public static TranslationUtility Instance
		{
			get
			{
				return _instance ?? (_instance = new TranslationUtility());
			}
		}

		public string Translate(string key)
		{
			throw new NotImplementedException();
		}

		public string Translate(string key, string locale)
		{
			throw new NotImplementedException();
		}

		public string Translate(string key, params string[] arguments)
		{
			throw new NotImplementedException();
		}

		public string Translate(string key, string locale, params string[] arguments)
		{
			throw new NotImplementedException();
		}

		private void setUpTranslationHashes()
		{
			translationHashes = new List<TranslationHash>();
			translationHashes.Add(new TranslationHash(){TranslationLocale = LocaleEnum.En});
			translationHashes.Add(new TranslationHash() { TranslationLocale = LocaleEnum.De });
			translationHashes.Add(new TranslationHash() { TranslationLocale = LocaleEnum.Es });
			translationHashes.Add(new TranslationHash() { TranslationLocale = LocaleEnum.Pt });
		}
	}
}
