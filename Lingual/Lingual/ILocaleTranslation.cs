using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lingual
{
    public interface ILocaleTranslation
    {
        void AddTranslation(string key, string value);
        bool ContainsKey(string key);
        string GetValue(string key, Plurality? plurality = null);
        bool HasFallbackLocale();
        bool IsTranslationDictionaryEmpty();
    }
}
