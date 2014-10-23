using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using Lingual.Handlers;
using Newtonsoft.Json.Linq;

namespace Lingual.TranslationUtilities
{
	/// <summary>
	/// A translation utility class
	/// </summary>
	public class TranslationUtility
	{
		#region Private Attributes

		private readonly Dictionary<Locales, TranslationDictionary> _locales =
			new Dictionary<Locales, TranslationDictionary>();

		private const string DateFormatter = "d";
		private const string CurrencyFormatter = "C2";

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
			get { return _instance ?? (_instance = new TranslationUtility()); }
		}

		#endregion

		#region Translation Utilities

		/// <summary>
		/// Translates the specified key using the locale passed in
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="locale">The locale.</param>
		/// <returns>Returns the value associated with the passed in key and locale. Parameter locale takes precedence over current locale</returns>
		public string Translate(string key, Locales locale = Locales.EN)
		{
			var requestedTranslationDictionary = _locales[locale];
			return requestedTranslationDictionary.GetValue(key);
		}

		/// <summary>
		/// Translates the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="locale">The locale.</param>
		/// <param name="arguments">The arguments.</param>
		/// <returns>Returns the value associated with the passed in key, locale, and passes in the arguements to the string. Parameter locale takes precedence over current locale</returns>
		public string Translate(string key, Locales? locale, params string[] arguments)
		{
			var translatedString = locale.HasValue ? Translate(key, locale.Value) : Translate(key);

			if (arguments.Any())
			{
				translatedString = string.Format(translatedString, arguments);
			}
			return translatedString;
		}

		/// <summary>
		/// Returns a locale formatted string of the specified Datetime object
		/// </summary>
		/// <param name="key">The DateTime to localize</param>
		/// <param name="locale">The locale</param>
		/// <returns></returns>
		public string TranslateDate(DateTime key, Locales locale = Locales.EN)
		{
			var culture = new CultureInfo(locale.ToString());
			return key.ToString(DateFormatter, culture);
		}

		/// <summary>
		/// Plural translations; returns properly pluralized translation
		/// </summary>
		/// <param name="key">Translation key</param>
		/// <param name="pluralDegree">Degree of plurality</param>
		/// <param name="locale">Interpolated Data</param>
		/// <returns></returns>
		public string TranslatePlural(string key, PluralDegree plurality, Locales locale = Locales.EN)
		{
			var requestedTranslationDictionary = _locales[locale];
			return requestedTranslationDictionary.GetValue(key, plurality);
		}

		/// <summary>
		/// Interpolated plural translations; returns properly pluralized translation with interpolated data.
		/// </summary>
		/// <param name="key">Translation key</param>
		/// <param name="pluralDegree">Degree of plurality</param>
		/// <param name="locale">Specified locale</param>
		/// <param name="interpolatedData">Interpolated data</param>
		/// <returns></returns>
		public string TranslatePlural(string key, PluralDegree pluralDegree, Locales? locale, params string[] interpolatedData)
		{
			var interpolStringVal = locale.HasValue
				? TranslatePlural(key, pluralDegree, locale.Value)
				: TranslatePlural(key, pluralDegree);
			if (interpolatedData.Any())
			{
				interpolStringVal = string.Format(interpolStringVal, interpolatedData);
			}
			return interpolStringVal;
		}

		/// <summary>
		/// Translates the amount passed in to the locale that's passed in
		/// </summary>
		/// <param name="currencyAmount"></param>
		/// <param name="locale"></param>
		/// <returns></returns>
		public string TranslateCurrency(double currencyAmount, Locales locale = Locales.EN)
		{
			return currencyAmount.ToString(CurrencyFormatter, CultureInfo.CreateSpecificCulture(locale.ToString()));
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// Sets up translation hashes.
		/// </summary>
		private void SetUpTranslationHashes()
		{

			_locales.Add(Locales.EN, new TranslationDictionary(Locales.EN));
			_locales.Add(Locales.ES, new TranslationDictionary(Locales.ES));
			_locales.Add(Locales.DE, new TranslationDictionary(Locales.DE));

			foreach (var translationDictionary in _locales.Values)
			{
				SetTranslationNodes(translationDictionary.TranslationLocale);
			}
		}

		/// <summary>
		/// Sets the translation nodes for the specific locale.
		/// </summary>
		/// <param name="locale">The locale enum.</param>
		public void SetTranslationNodes(Locales locale)
		{
			var localeJsonObj = LocaleFileHandler.GetLocaleFile(locale);
			var localeHash = _locales.Where(t => t.Key == locale).FirstOrDefault();

			foreach (var jsonKVPair in localeJsonObj)
			{
				localeHash.Value.AddTranslation(jsonKVPair.Key, jsonKVPair.Value.ToString());
			}
		}

		#endregion
	}
}
