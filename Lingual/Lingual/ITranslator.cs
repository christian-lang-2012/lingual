using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lingual
{
    public interface ITranslator
    {
        string InterpolateTranslation(string sourceTranslation, object tokens);

        string Translate(string key, Locale locale);

        string Translate(string key, object tokens, Locale locale);

        string Translate(string key, Locale locale, object tokens);

        string Localize(DateTime date, Locale locale);

        string Localize(double currencyAmount, Locale locale);

        string TranslatePlural(string key, Plurality plurality, Locale locale, object tokens);

        string TranslatePlural(string key, Plurality plurality, object tokens, Locale locale);

        string TranslatePlural(string key, Plurality plurality, Locale locale);
    }
}
