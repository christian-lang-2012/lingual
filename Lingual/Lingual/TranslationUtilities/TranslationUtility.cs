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
        // TODO can we make this readonly
        // FIXME make this a dictionary keyed by locale
        // FIXME maybe rename to locales?
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
            // FIXME lookup in dictionary instead of list
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
            // FIXME lookup in dictionary instead of list
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

		/// <summary>
		/// Returns a locale formatted string of the specified Datetime object
		/// </summary>
		/// <param name="key">The DateTime to localize</param>
		/// <param name="locale">The locale</param>
		/// <returns></returns>
		public string TranslateDate(DateTime key, LocaleEnum locale = LocaleEnum.EN)
		{
			var culture = new CultureInfo(locale.ToString());
            // FIXME extract "d" into static const string
			return key.ToString("d", culture);
		}

		/// <summary>
		/// Plural translations; returns properly pluralized translation
		/// </summary>
		/// <param name="key">Translation key</param>
		/// <param name="pluralDegree">Degree of plurality</param>
		/// <param name="locale">Interpolated Data</param>
		/// <returns></returns>
		public string TranslatePlural(string key, String pluralDegree, LocaleEnum locale = LocaleEnum.EN)
		{
            // FIXME lookup in dictionary instead of list
			var requestedDictionary = _translationDictionaries[0];
			foreach (var translationDictionary in _translationDictionaries.Where(translationDictionary => translationDictionary.TranslationLocale == locale))
			{
				requestedDictionary = translationDictionary;
			}
			return requestedDictionary.GetValue(key, pluralDegree);
		}

		/// <summary>
		/// Interpolated plural translations; returns properly pluralized translation with interpolated data.
		/// </summary>
		/// <param name="key">Translation key</param>
		/// <param name="pluralDegree">Degree of plurality</param>
		/// <param name="locale">Specified locale</param>
		/// <param name="interpolatedData">Interpolated data</param>
		/// <returns></returns>
		public string TranslatePlural(string key, String pluralDegree, LocaleEnum locale, params string[] interpolatedData)
		{
			var interpolStringVal = TranslatePlural(key, pluralDegree, locale);
			if (interpolatedData.Any())
			{
				interpolStringVal = string.Format(interpolStringVal, interpolatedData);
			}
			return interpolStringVal;
		}

		/// <summary>
		/// Translates the amount passed in to the locale that's passed in
		/// </summary>
		/// <param name="value"></param>
		/// <param name="locale"></param>
		/// <returns></returns>
        // FIXME rename value to something other than a keyword
		public string TranslateCurrency(double value, LocaleEnum locale = LocaleEnum.EN)
		{
            // FIXME extract "C2" into static const string
			return value.ToString("C2", CultureInfo.CreateSpecificCulture(locale.ToString()));
		}

		#endregion

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

            // FIXME rename translationDictionary
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
            // FIXME use var since we seee cast on rhs
            // FIXME rename token to something that describes the object.
            // FIXME cast to JOBject, remove top-level array brackets in each json file
			JArray token = (JArray)localeJsonObj[localeEnum.ToString().ToLower()];

            // FIXME remove outer foreach since we are iterating object properties, not array items.
			foreach (JObject content in token.Children<JObject>())
			{
                // FIXME rename prop to property
				foreach (JProperty prop in content.Properties())
				{
                    // FIXME use .Where(t => ...).FirstOrDefault()
                    // FIXME rename to translationDictionary
                    // FIXME Move above of double nested foreaches to avoid re-computing var
					var localeHash = _translationDictionaries.FirstOrDefault(t => t.TranslationLocale == localeEnum);
					if (localeHash != null)
					{
						localeHash.AddTranslation(prop.Name, prop.Value.ToString());
					}
				}
			}
		}

		#endregion
	}
}
