using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Lingual.TranslationUtilities
{
    public class TranslationDictionary
    {
        #region Properties and Variables

        public Locale Locale { get; private set; }

        private readonly Dictionary<string, string> _translationDictionary = new Dictionary<string, string>();

        #endregion

        #region Core Methods

        /// <summary>
        /// Constructor for the TranslationDictionary. Must take in a Locale
        /// </summary>
        /// <param name="locale"></param>
        public TranslationDictionary(Locale locale)
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
            if (!KeyExists(key))
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
        public string GetValue(string key, PluralDegree? plurality = null)
        {
            var translation = KeyExists(key) ? _translationDictionary[key.ToLower()] : key;
            if (plurality.HasValue)
            {
                translation = PluralKeyFinder(JObject.Parse(translation), plurality.Value);
            }
            return translation;

        }

        /// <summary>
        /// Checks to see if the translation dictionary is empty
        /// </summary>
        /// <returns></returns>
        public bool IsTranslationDictionaryEmpty()
        {
            return _translationDictionary.Any();
        }

        public bool KeyExists(string key)
        {
            return _translationDictionary.ContainsKey(key.ToLower());
        }
        #endregion

        #region Private Helper Methods

        private string PluralKeyFinder(JObject pluralKeys, PluralDegree plurality)
        {
            var jobjectDictionary = pluralKeys.ToObject<Dictionary<PluralDegree, string>>();
            return jobjectDictionary.Where(t => t.Key == plurality).Select(t => t.Value).First();
        }
        #endregion
    }
}
