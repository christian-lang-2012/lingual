using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Lingual
{
    public class LocaleTranslations : ILocaleTranslation
    {
        public Locale Locale { get; private set; }

        private readonly Dictionary<string, string> _translationDictionary = new Dictionary<string, string>();

        /// <summary>
        /// Constructor for the TranslationDictionary. Must take in a Locale
        /// </summary>
        /// <param name="locale"></param>
        public LocaleTranslations(Locale locale)
        {
            Locale = locale;
        }

        /// <summary>
        /// Creates a translation node and addes it to the list of nodes using the key and the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void AddTranslation(string key, string value)
        {
            if (!ContainsKey(key))
            {
                _translationDictionary.Add(key.ToLower(), value);
            }
        }

        /// <summary>
        /// Gets the value corresponding to the passed in key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="plurality"></param>
        /// <returns></returns>
        public string GetValue(string key, Plurality? plurality = null)
        {
            var translation = ContainsKey(key) ? _translationDictionary[key.ToLower()] : key;

            if (plurality.HasValue)
            {
                translation = PluralKeyFinder(translation, plurality.Value);
                translation = translation ?? key;
            }

            return translation;
        }

        /// <summary>
        /// Returns whether the current locale is the same as the default locale
        /// </summary>
        /// <returns><c>true</c> if this instance has fallback locale; otherwise, <c>false</c>.</returns>
        public bool HasFallbackLocale()
        {
            return Locale != Translator.DefaultLocale;
        }

        /// <summary>
        /// Checks to see if the translation dictionary is empty
        /// </summary>
        /// <returns></returns>
        public bool IsTranslationDictionaryEmpty()
        {
            return _translationDictionary.Any();
        }

        /// <summary>
        /// Checks to see if the key exists in the dictionary
        /// </summary>
        /// <returns><c>true</c>, if exists was keyed, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        public bool ContainsKey(string key)
        {
            return _translationDictionary.ContainsKey(key.ToLower());
        }

        /// <summary>
        /// Finds each plurality in the key
        /// </summary>
        /// <returns>The key finder.</returns>
        /// <param name="pluralKeys">Plural keys.</param>
        /// <param name="plurality">Plurality.</param>
        private string PluralKeyFinder(string translation, Plurality plurality)
        {
            var pluralityDictionary = JObject.Parse(translation).ToObject<Dictionary<Plurality, string>>();
            return pluralityDictionary.Where(t => t.Key == plurality).Select(t => t.Value).First();
        }
    }
}
