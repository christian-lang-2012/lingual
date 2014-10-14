using Lingual.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

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

		private LocaleEnum _currentLocaleEnum;

		private static TranslationUtility _instance;
		#endregion

		#region Poperties

		public LocaleEnum CurrentLocale
		{
			get { return _currentLocaleEnum; }
			set
			{
				_currentLocaleEnum = value;
				SetTranslationNodes(_currentLocaleEnum);
			}
		}

		private TranslationUtility()
		{
			SetUpTranslationHashes();
		}
		#endregion

		#region Singleton Instance

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

		#endregion

		#region Translation Utilities
		/// <summary>
		/// Translates the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Returns the value associated with the passed in key</returns>
		public string Translate(string key)
		{
			var translation = "No translation";
			foreach (var translationNode in _translationHashes.Where(t => t.TranslationLocale == CurrentLocale).SelectMany(t => t.TranslationNodes.Where(tn => tn.Key == key)))
			{
				translation = translationNode.Value;
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
			var translation = "No translation";
			var translationHash = _translationHashes.FirstOrDefault(x => x.TranslationLocale == locale);

			if (translationHash != null)
			{
				if (translationHash.IsTranslationHashEmpty())
				{
					SetTranslationNodes(locale);
				}

				foreach (TranslationNode translationNode in translationHash.TranslationNodes.Where(tn => tn.Key == key))
				{
					translation = translationNode.Value;
				}
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
		/// <summary>
		/// Sets up translation hashes.
		/// </summary>
		private void SetUpTranslationHashes()
		{
			_translationHashes = new List<TranslationHash>
			{
				new TranslationHash(LocaleEnum.EN),
				new TranslationHash(LocaleEnum.DE),
				new TranslationHash(LocaleEnum.ES),
				new TranslationHash(LocaleEnum.PT)
			};
		}

		/// <summary>
		/// Sets the translation nodes for the specific locale.
		/// </summary>
		/// <param name="localeEnum">The locale enum.</param>
		public void SetTranslationNodes(LocaleEnum localeEnum)
		{
			var localeJsonObj = LocaleFileHandler.GetLocaleFile(localeEnum);
			TranslationHash currentHash = null;
			foreach (TranslationHash translationHash in _translationHashes.Where(translationHash => translationHash.TranslationLocale == localeEnum))
			{
				currentHash = translationHash;
			}

			if (currentHash != null)
			{
				foreach (KeyValuePair<String, JToken> prop in localeJsonObj)
				{
					currentHash.AddTranslationNode(prop.Key, prop.Value.ToString());
				}

			}
		}

		#endregion
	}
}
