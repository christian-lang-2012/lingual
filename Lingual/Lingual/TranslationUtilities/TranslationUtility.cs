using System;
using System.Collections.Generic;
using System.Linq;
using Lingual.Handlers;
using Newtonsoft.Json.Linq;
using Lingual.Enums;

namespace Lingual.TranslationUtilities
{
	/// <summary>
	/// A translation utility class
	/// </summary>
	public class TranslationUtility
	{
		#region Private Attributes

		private List<TranslationHash> _translationHashes;


		private static TranslationUtility _instance;
		#endregion

		#region Poperties

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
		/// Translates the specified key using the locale passed in
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="locale">The locale.</param>
		/// <returns>Returns the value associated with the passed in key and locale. Parameter locale takes precedence over current locale</returns>
		public string Translate(string key, LocaleEnum locale = LocaleEnum.EN)
		{
			var requestedLanguageHash = _translationHashes[0];
			foreach (var translationHash in _translationHashes.Where(translationHash => translationHash.TranslationLocale == locale))
			{
				requestedLanguageHash = translationHash;
			}

			return requestedLanguageHash.GetValue(key);
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
				new TranslationHash(LocaleEnum.ES)
			};

			foreach (var translationHash in _translationHashes)
			{
				SetTranslationNodes(translationHash.TranslationLocale);
			}
		}

		/// <summary>
		/// Sets the translation nodes for the specific locale.
		/// </summary>
		/// <param name="localeEnum">The locale enum.</param>
		public void SetTranslationNodes(LocaleEnum localeEnum)
		{
			var localeJsonObj = LocaleFileHandler.GetLocaleFile(localeEnum);
			TranslationHash currentHash = null;
			foreach (var translationHash in _translationHashes.Where(translationHash => translationHash.TranslationLocale == localeEnum))
			{
				currentHash = translationHash;
			}

			if (currentHash != null)
			{
				foreach (var prop in localeJsonObj)
				{
					currentHash.AddTranslation(prop.Key, prop.Value.ToString());
				}

			}
		}

		#endregion
	}
}
