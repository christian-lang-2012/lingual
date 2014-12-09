using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lingual.Handlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Lingual.LocaleTranslation;

namespace Lingual
{

    /// <summary>
    /// A translation utility class
    /// </summary>
    public class Translator

    {
        
        public const Locale DefaultLocale = Locale.en;
        private readonly Dictionary<Locale, LocaleTranslations> _locales = new Dictionary<Locale, LocaleTranslations>();
        private readonly ILocaleTranslation _nullTranslationDictionary = new NullLocaleTranslations();
        private LocaleFileHandler localeHandler;
        private const string DateFormatter = "d";
        private const string CurrencyFormatter = "C2";

        /// <summary>
        /// Initializes a new instance of the Translator class.
        /// </summary>
        public Translator(string filePath)
        {
            localeHandler = new LocaleFileHandler(filePath);
            localeHandler.CheckLocaleFolderExists();
            CreateTranslationDictionaries();
        }
        
        /// <summary>
        /// Get the translation dictionary for the locale passed in
        /// </summary>
        /// <param name="locale">The locale.</param>
        /// <returns>
        /// Returns the locale tranlastion for the locale passed in
        /// </returns>
        public ILocaleTranslation GetTranslationDictionaryForLocale(Locale locale)
        {
            ILocaleTranslation translationDictionary = null;

            if (_locales.ContainsKey(locale))
            {
                translationDictionary = _locales[locale];
            }
            else if (locale == Locale.en)
            {
                throw new Exception("No english translation dictionary found");
            }
            else
            {
                translationDictionary = _nullTranslationDictionary;
            }

            return translationDictionary;
        }

        /// <summary>
        /// Get the fallback locale of the locale passed in
        /// </summary>
        /// <param name="locale">The locale.</param>
        /// <returns>
        /// Returns the fallback locale of the one that was requested
        /// </returns>
        public Locale GetFallbackLocale(Locale locale)
        {
            Locale fallbackLocale = DefaultLocale;

            if (locale != DefaultLocale)
            {
                Locale proposedFallbackLocale = LocaleMapper.LanguageToCultureMappings[locale];
                if (_locales.ContainsKey(proposedFallbackLocale))
                {
                    fallbackLocale = proposedFallbackLocale;
                }
            }

            return fallbackLocale;
        }

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
            if (key == null)
            {
                return string.Empty;
            }

            string translation;

            ILocaleTranslation translationDictionary = GetTranslationDictionaryForLocale(locale);
            if (!translationDictionary.ContainsKey(key) && translationDictionary.HasFallbackLocale())
            {
                Locale fallbackLocale = this.GetFallbackLocale(locale);
                translation = Translate(key, fallbackLocale);
            }
            else
            {
                translation = translationDictionary.GetValue(key);
            }

            return translation;
        }

        /// <summary>
        /// Plural translations; returns properly pluralized translation
        /// </summary>
        /// <param name="key">Translation key</param>
        /// <param name="plurality">Degree of plurality</param>
        /// <param name="locale">Interpolated Data</param>
        /// <returns></returns>
        public string TranslatePlural(string key, Plurality plurality, Locale locale = DefaultLocale)
        {
            string translation;

            ILocaleTranslation translationDictionary = GetTranslationDictionaryForLocale(locale);
            if (!translationDictionary.ContainsKey(key) && translationDictionary.HasFallbackLocale())
            {
                Locale fallbackLocale = this.GetFallbackLocale(locale);
                translation = TranslatePlural(key, plurality, fallbackLocale);
            }
            else
            {
                translation = translationDictionary.GetValue(key, plurality);
            }

            return translation;
        }

        /// <summary>
        /// Translates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="locale">The locale.</param>
        /// <param name="tokens">An object of tokens to replace in translation value.</param>
        /// <returns>
        /// Returns the value associated with the passed in key, locale, and passes in the arguements to the string. Parameter locale takes precedence over current locale
        /// </returns>
        public string Translate(string key, object tokens = null, Locale locale = DefaultLocale)
        {
            return this.Translate(key, locale, tokens);
        }
        /// <summary>
        /// Translates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="locale">The locale.</param>
        /// <param name="tokens">An object of tokens to replace in translation value.</param>
        /// <returns>
        /// Returns the value associated with the passed in key, locale, and passes in the arguements to the string. Parameter locale takes precedence over current locale
        /// </returns>
        public string Translate(string key, Locale locale = DefaultLocale, object tokens = null)
        {
            var translation = Translate(key, locale);

            return InterpolateTranslation(translation, tokens);
        }

        /// <summary>
        /// Returns a locale formatted string of the specified Datetime object
        /// </summary>
        /// <param name="date">The DateTime to localize</param>
        /// <param name="locale">The locale</param>
        /// <returns></returns>
        public string Localize(DateTime date, Locale locale = DefaultLocale)
        {
            var cultureName = locale.ToString().Split('_')[0];
            var culture = new CultureInfo(cultureName);
            return date.ToString(DateFormatter, culture);
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
        /// Translate a key using the passed in Dictionary tokens
        /// </summary>
        /// <param name="sourceTranslation"></param>
        /// <param name="tokens">An object of tokens to replace in translation value.</param>
        /// <returns>The string translated from the interpolation if it exists. If not, it returns the key
        /// </returns>
        public string InterpolateTranslation(string sourceTranslation, object tokens)
        {
            if (string.IsNullOrWhiteSpace(sourceTranslation) || tokens == null)
            {
                return string.Empty;
            }

            var properties = tokens.GetType().GetProperties();
            foreach(var property in properties)
            {
                var key = string.Format("__{0}__", property.Name.ToUpper());
                if (sourceTranslation.Contains(key))
                {
                    var value = (string)property.GetValue(tokens, null);
                    sourceTranslation = sourceTranslation.Replace(key, value);
                }
            }

            return sourceTranslation;
        }

        /// <summary>
        /// Interpolated plural translations; returns properly pluralized translation with interpolated data.
        /// </summary>
        /// <param name="key">Translation key</param>
        /// <param name="plurality">Degree of plurality</param>
        /// <param name="tokens">An object of tokens to replace in translation value.</param>
        /// <param name="locale">Specified locale</param>
        /// <returns></returns>
        public string TranslatePlural(string key, Plurality plurality, Locale locale = DefaultLocale, object tokens = null)
        {
            var translation = TranslatePlural(key, plurality, locale);

            return InterpolateTranslation(translation, tokens);
        }

        /// <summary>
        /// Interpolated plural translations; returns properly pluralized translation with interpolated data.
        /// </summary>
        /// <param name="key">Translation key</param>
        /// <param name="plurality">Degree of plurality</param>
        /// <param name="tokens">An object of tokens to replace in translation value.</param>
        /// <param name="locale">Specified locale</param>
        /// <returns></returns>
        public string TranslatePlural(string key, Plurality plurality, object tokens, Locale locale = DefaultLocale)
        {
            var translation = TranslatePlural(key, plurality, locale);

            return InterpolateTranslation(translation, tokens);
        }

        /// <summary>
        /// Loads translation dictionaries from the file system.
        /// </summary>
        private void CreateTranslationDictionaries()
        {
            foreach (Locale locale in Enum.GetValues(typeof(Locale)))
            {
                if (localeHandler.LocaleFileExists(locale))
                {
                    var translationDictionary = new LocaleTranslations(locale);
                    SetTranslationNodesFromFile(translationDictionary);
                    _locales.Add(locale, translationDictionary);
                }
            }
        }

        /// <summary>
        /// Sets the translation nodes for the specific locale by loading the
        /// translation file.
        /// </summary>
        /// <param name="translationDictionary"></param>
        private void SetTranslationNodesFromFile(LocaleTranslations translationDictionary)
        {
            var localeTranslations = localeHandler.GetLocaleFile(translationDictionary.Locale);

            foreach (var jsonObjProperties in localeTranslations)
            {
                if (jsonObjProperties.Key != "null")
                {
                    // TODO maybe not ToString for objects?
                    translationDictionary.AddTranslation(jsonObjProperties.Key, jsonObjProperties.Value.ToString());
                }
            }
        }
    }
}
