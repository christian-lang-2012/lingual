using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Lingual.TranslationUtilities
{

    public interface ITranslationDictionary
    {
        void AddTranslation(string key, string value);
        string GetValue(string key, Plurality? plurality = null);
        bool HasFallbackLocale();
        bool IsTranslationDictionaryEmpty();
        bool KeyExists(string key);
    }

    public class NullTranslationDictionary : ITranslationDictionary
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void AddTranslation(string key, string value)
        {
            return;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="key">Key.</param>
        /// <param name="plurality">Plurality.</param>
        public string GetValue(string key, Plurality? plurality = null)
        {
            return key;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns><c>true</c> if this instance has fallback locale; otherwise, <c>false</c>.</returns>
        public bool HasFallbackLocale()
        {
            return true;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns><c>true</c> if this instance is translation dictionary empty; otherwise, <c>false</c>.</returns>
        public bool IsTranslationDictionaryEmpty()
        {
            return true;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns><c>true</c>, if exists was keyed, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        public bool KeyExists(string key)
        {
            return false;
        }
    }

    public class TranslationDictionary : ITranslationDictionary
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
        public string GetValue(string key, Plurality? plurality = null)
        {
            var translation = KeyExists(key) ? _translationDictionary[key.ToLower()] : key;

            if (plurality.HasValue)
            {
                translation = PluralKeyFinder(JObject.Parse(translation), plurality.Value);
            }

            return translation;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns><c>true</c> if this instance has fallback locale; otherwise, <c>false</c>.</returns>
        public bool HasFallbackLocale()
        {
            return Locale != TranslationUtility.DefaultLocale;
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
        /// TODO
        /// </summary>
        /// <returns><c>true</c>, if exists was keyed, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        public bool KeyExists(string key)
        {
            return _translationDictionary.ContainsKey(key.ToLower());
        }
        #endregion

        #region Private Helper Methods

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns>The key finder.</returns>
        /// <param name="pluralKeys">Plural keys.</param>
        /// <param name="plurality">Plurality.</param>
        private string PluralKeyFinder(JObject pluralKeys, Plurality plurality)
        {
            var jobjectDictionary = pluralKeys.ToObject<Dictionary<Plurality, string>>();
            return jobjectDictionary.Where(t => t.Key == plurality).Select(t => t.Value).First();
        }
        #endregion
    }
}
