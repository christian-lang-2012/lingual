using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lingual.LocaleTranslation
{
    public class NullLocaleTranslations : ILocaleTranslation
    {
        /// <summary>
        /// Doesn't add a translation for the null dictionary
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void AddTranslation(string key, string value)
        {
            return;
        }

        /// <summary>
        /// Returns false because this is the null locale translation
        /// </summary>
        /// <returns><c>true</c>, if exists was keyed, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        public bool ContainsKey(string key)
        {
            return false;
        }

        /// <summary>
        /// returns the key back for null translation locale
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="key">Key.</param>
        /// <param name="plurality">Plurality.</param>
        public string GetValue(string key, Plurality? plurality = null)
        {
            return key;
        }

        /// <summary>
        /// Returns true that there is a fallback locale
        /// </summary>
        /// <returns><c>true</c> if this instance has fallback locale; otherwise, <c>false</c>.</returns>
        public bool HasFallbackLocale()
        {
            return true;
        }

        /// <summary>
        /// returns true that the dictionary is empty
        /// </summary>
        /// <returns><c>true</c> if this instance is translation dictionary empty; otherwise, <c>false</c>.</returns>
        public bool IsTranslationDictionaryEmpty()
        {
            return true;
        }
    }
}
