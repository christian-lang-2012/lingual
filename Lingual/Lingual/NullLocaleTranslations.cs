using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lingual
{
    public class NullLocaleTranslations : ILocaleTranslation
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
        /// <returns><c>true</c>, if exists was keyed, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        public bool ContainsKey(string key)
        {
            return false;
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
    }
}
