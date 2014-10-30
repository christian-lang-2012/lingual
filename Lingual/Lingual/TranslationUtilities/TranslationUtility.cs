using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lingual.Handlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Lingual.TranslationUtilities
{
    /// <summary>
    /// A translation utility class
    /// </summary>
    public class TranslationUtility
    {
        #region Public Attributes

        public const Locale DefaultLocale = Locale.en;

        #endregion

        #region Private Attributes

        private readonly Dictionary<Locale, TranslationDictionary> _locales = new Dictionary<Locale, TranslationDictionary>();

        private const string DateFormatter = "d";
        private const string CurrencyFormatter = "C2";

        private static TranslationUtility _instance;

        #endregion

        #region Singleton Instance

        /// <summary>
        /// Gets the instance. Adhereing to the singleton pattern so that way there isn't a huge instance of the Translation Utility everyhere
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

        private TranslationUtility()
        {
            CreateTranslationDictionaries();
        }

        #endregion

        #region Translation Utilities

        /// <summary>
        /// Translates the specified key using the locale passed in
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="locale">The locale.</param>
        /// <returns>
        /// Returns the value associated with the passed in key and locale. Parameter locale takes precedence over current locale
        /// </returns>
        public string Translate(string key, Locale locale = DefaultLocale)
        {
            TranslationDictionary translationDictionary = _locales[locale];
            if (!translationDictionary.KeyExists(key))
            {
                translationDictionary = _locales[LocaleMapper.LanguageToCultureMappings[locale]];
                if (!translationDictionary.KeyExists(key))
                {
                    translationDictionary = _locales[DefaultLocale];
                }
            }

            return translationDictionary.GetValue(key);
        }

        /// <summary>
        /// Translates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="locale">The locale.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>
        /// Returns the value associated with the passed in key, locale, and passes in the arguements to the string. Parameter locale takes precedence over current locale
        /// </returns>
        public string Translate(string key, Locale? locale, Dictionary<string, string> argsDictionary)
        {
            var translatedString = locale.HasValue ? Translate(key, locale.Value) : Translate(key);

            if (argsDictionary.Any())
            {
                translatedString = InterpolationDictionaryTokenizer(translatedString, argsDictionary);
            }
            return translatedString;
        }

        /// <summary>
        /// Returns a locale formatted string of the specified Datetime object
        /// </summary>
        /// <param name="key">The DateTime to localize</param>
        /// <param name="locale">The locale</param>
        /// <returns></returns>
        public string Localize(DateTime key, Locale locale = DefaultLocale)
        {
            var cultureName = locale.ToString().Split('_')[0];
            var culture = new CultureInfo(cultureName);
            return key.ToString(DateFormatter, culture);
        }

        /// <summary>
        /// Translates the amount passed in to the locale that's passed in
        /// </summary>
        /// <param name="currencyAmount"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public string Localize(double currencyAmount, Locale locale = DefaultLocale)
        {
            var cultureName = locale.ToString().Replace('_', '-');
            return currencyAmount.ToString(CurrencyFormatter, CultureInfo.CreateSpecificCulture(cultureName));
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="interpolString"></param>
        /// <param name="argsDictionary"></param>
        /// <returns></returns>
        public String InterpolationDictionaryTokenizer(String interpolString, Dictionary<string, string> argsDictionary)
        {
            var newInterpolatedString = interpolString;
            foreach (var dictKVPair in argsDictionary)
            {
                if (newInterpolatedString.Contains(dictKVPair.Key))
                {
                    newInterpolatedString = newInterpolatedString.Replace(dictKVPair.Key, dictKVPair.Value);
                }
            }
            return newInterpolatedString;
        }

        /// <summary>
        /// Plural translations; returns properly pluralized translation
        /// </summary>
        /// <param name="key">Translation key</param>
        /// <param name="plurality">Degree of plurality</param>
        /// <param name="locale">Interpolated Data</param>
        /// <returns></returns>
        public string TranslatePlural(string key, PluralDegree plurality, Locale locale = DefaultLocale)
        {
            TranslationDictionary requestedTranslationDictionary = _locales[locale];
            if (!requestedTranslationDictionary.KeyExists(key))
            {
                requestedTranslationDictionary = _locales[LocaleMapper.LanguageToCultureMappings[locale]];
                if (!requestedTranslationDictionary.KeyExists(key))
                {
                    requestedTranslationDictionary = _locales[DefaultLocale];
                }
            }

            var translation = requestedTranslationDictionary.GetValue(key, plurality);

            return translation;
        }

        /// <summary>
        /// Interpolated plural translations; returns properly pluralized translation with interpolated data.
        /// </summary>
        /// <param name="key">Translation key</param>
        /// <param name="pluralDegree">Degree of plurality</param>
        /// <param name="locale">Specified locale</param>
        /// <param name="interpolatedData">Interpolated data</param>
        /// <returns></returns>
        public string TranslatePlural(string key, PluralDegree pluralDegree, Locale? locale, Dictionary<string, string> argsDictionary)
        {
            var interpolStringVal = locale.HasValue
                ? TranslatePlural(key, pluralDegree, locale.Value)
                : TranslatePlural(key, pluralDegree);

            if (argsDictionary.Any())
            {
                interpolStringVal = InterpolationDictionaryTokenizer(interpolStringVal, argsDictionary);
            }
            return interpolStringVal;
        }

        #endregion

        #region Helper Methods
        /// <summary>
        /// Sets up translation hashes.
        /// </summary>
        private void SetUpTranslationHashes()
        {
            foreach (var locale in Enum.GetValues(typeof(Locale)))
            {
                _locales.Add((Locale)locale, new TranslationDictionary((Locale)locale));
            }

            foreach (var translationDictionary in _locales.Values)
            {
                SetTranslationNodes(translationDictionary);
            }
        }

        /// <summary>
        /// Sets the translation nodes for the specific locale.
        /// </summary>
        /// <param name="translationDictionary"></param>
        public void SetTranslationNodes(TranslationDictionary translationDictionary)
        {
            var localeJsonObj = LocaleFileHandler.GetLocaleFile(translationDictionary.Locale);

            foreach (var jsonObjProperties in localeJsonObj)           
            {
                if (jsonObjProperties.Key != "null")
                {
                    translationDictionary.AddTranslation(jsonObjProperties.Key, jsonObjProperties.Value.ToString());
                }

            }
        }
        #endregion
    }
}
