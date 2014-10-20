using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lingual.Handlers;
using Lingual.Enums;
using Newtonsoft.Json.Linq;

namespace Lingual.TranslationUtilities
{
	/// <summary>
	/// A translation utility class
	/// </summary>
	public class TranslationUtility
	{
		#region Private Attributes
		private List<TranslationDictionary> _translationDictionaries;


		private static TranslationUtility _instance;
		#endregion

		#region Properties

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
			var requestedDictionary = _translationDictionaries[0];
			foreach (var translationDictionary in _translationDictionaries.Where(translationDictionary => translationDictionary.TranslationLocale == locale))
			{
				requestedDictionary = translationDictionary;
			}

			return requestedDictionary.GetValue(key);
		}

		/// <summary>
		/// Translates the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="locale">The locale.</param>
		/// <param name="arguments">The arguments.</param>
		/// <returns>Returns the value associated with the passed in key, locale, and passes in the arguements to the string. Parameter locale takes precedence over current locale</returns>

		public string Translate(string key, LocaleEnum locale = LocaleEnum.EN, params string[] arguments)
		{
			TranslationDictionary requestedTranslationDictionary = null;
			foreach (var translationDictionary in _translationDictionaries.Where(translationHash => translationHash.TranslationLocale == locale))
			{
				requestedTranslationDictionary = translationDictionary;
			}
			return requestedTranslationDictionary.GetValue(key, arguments);
		}
		#endregion


		/// <summary>
		/// Translates the amount passed in to the locale that's passed in
		/// </summary>
		/// <param name="value"></param>
		/// <param name="locale"></param>
		/// <returns></returns>
		public string TranslateCurrency(double value, LocaleEnum locale = LocaleEnum.EN)
		{
			return value.ToString("C3", CultureInfo.CreateSpecificCulture(locale.ToString()));
		}

		#region Helper Methods
		/// <summary>
		/// Sets up translation hashes.
		/// </summary>
		private void SetUpTranslationHashes()
		{
			_translationDictionaries = new List<TranslationDictionary>
			{
				new TranslationDictionary(LocaleEnum.EN),
				new TranslationDictionary(LocaleEnum.DE),
				new TranslationDictionary(LocaleEnum.ES)
			};

			foreach (var translationHash in _translationDictionaries)
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
			JArray token = (JArray)localeJsonObj[localeEnum.ToString().ToLower()];

			foreach (JObject content in token.Children<JObject>())
			{
				foreach (JProperty prop in content.Properties())
				{
					var localeHash = _translationDictionaries.Where(t => t.TranslationLocale == localeEnum).FirstOrDefault();
					localeHash.AddTranslation(prop.Name, prop.Value.ToString());
				}
			}
		}

		#endregion
	}
}
