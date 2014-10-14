using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lingual
{
	/// <summary>
	/// A translation utility class
	/// </summary>
	public class TranslationUtility
	{
		//private static string LOCALE_PATH = "/locale/";
		#region Private Attributes

		private List<TranslationHash> _translationHashes; 

		private static TranslationUtility _instance;
		#endregion

		public LocaleEnum CurrentLocale { get; set; }

		private TranslationUtility()
		{
			SetUpTranslationHashes();
		}

		#region Translation Utilities

		/// <summary>
		/// Gets the instance. Adhereing to the singleton pattern so that way there aren't huge instance of the Translation Utility everyhere
		/// </summary>
		/// <value>
		/// The instance.
		/// </value>
		public static TranslationUtility Instance
		{
			get
			{
				return _instance ?? (_instance = new TranslationUtility());
			}
		}

		/// <summary>
		/// Translates the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Returns the value associated with the passed in key</returns>
		public string Translate(string key)
		{
			string translation = "No translation";
			foreach (var tn in _translationHashes.Where(t => t.TranslationLocale == CurrentLocale).SelectMany(t => t.TranslationNodes.Where(tn => tn.Key == key)))
			{
				translation = tn.Value;
			}

			return translation;
		}

		/// <summary>
		/// Translates the specified key using the locale passed in
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="locale">The locale.</param>
		/// <returns>Returns the value associated with the passed in key and locale. Parameter locale takes precedence over current locale</returns>
		public string Translate(string key, LocaleEnum locale)
		{
			string translation = "No translation";
			foreach (var tn in _translationHashes.Where(t => t.TranslationLocale == locale).SelectMany(t => t.TranslationNodes.Where(tn => tn.Key == key)))
			{
				translation = tn.Value;
			}

			return translation;
		}

		/// <summary>
		/// Translates the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="arguments">The arguments.</param>
		/// <returns>Returns the value associated with the passed in key and passes in the arguements to the string</returns>
		public string Translate(string key, params string[] arguments)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Translates the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="locale">The locale.</param>
		/// <param name="arguments">The arguments.</param>
		/// <returns>Returns the value associated with the passed in key, locale, and passes in the arguements to the string. Parameter locale takes precedence over current locale</returns>
		public string Translate(string key, string locale, params string[] arguments)
		{
			throw new NotImplementedException();
		}
		#endregion


		#region Helper Methods
		private void SetUpTranslationHashes()
		{
			_translationHashes = new List<TranslationHash>();
			_translationHashes.Add(new TranslationHash(LocaleEnum.EN));
			_translationHashes.Add(new TranslationHash(LocaleEnum.DE));
			_translationHashes.Add(new TranslationHash(LocaleEnum.ES));
			_translationHashes.Add(new TranslationHash(LocaleEnum.PT));
		}
		#endregion
	}
}
