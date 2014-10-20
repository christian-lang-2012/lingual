using System;
using System.Collections.Generic;
using System.Linq;
using Lingual.Handlers;
using Lingual.Enums;
using Newtonsoft.Json.Linq;
using System.Globalization;

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
            var interpolStringVal = requestedTranslationDictionary.GetValue(key);
            if (arguments.Any())
            {
                interpolStringVal = string.Format(interpolStringVal, arguments);
            }
            return interpolStringVal;
		}
		#endregion

        /// <summary>
        /// Returns a locale formatted string of the specified Datetime object
        /// </summary>
        /// <param name="key">The DateTime to localize</param>
        /// <param name="locale">The locale</param>
        /// <returns></returns>
        public string TranslateDate(DateTime key, LocaleEnum locale = LocaleEnum.EN)
        {
            CultureInfo culture = new CultureInfo(locale.ToString());
            return key.ToString("d", culture);
        }

        public string TranslatePlural(string key, String pluralDegree, LocaleEnum locale = LocaleEnum.EN)
        {
            var requestedDictionary = _translationDictionaries[0];
            foreach (var translationDictionary in _translationDictionaries.Where(translationDictionary => translationDictionary.TranslationLocale == locale))
            {
                requestedDictionary = translationDictionary;
            }
            return requestedDictionary.GetValue(key, pluralDegree);
        }

        public string TranslatePlural(string key, String pluralDegree, LocaleEnum locale, params string[] arguments)
        {
            var interpolStringVal = TranslatePlural(key, pluralDegree, locale);
            if (arguments.Any())
            {
                interpolStringVal = string.Format(interpolStringVal, arguments);
            }
            return interpolStringVal;
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
