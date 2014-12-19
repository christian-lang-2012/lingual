using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Lingual.Infrastructure;

namespace Lingual
{
    public interface ITranslator
    {
        string Localize(string key, CultureInfo culture, object tokens = null);
        string Localize(DateTime date, CultureInfo locale);
        string Localize(double currencyAmount, CultureInfo locale);
        string Localize(string key, Plurality plurality, CultureInfo locale, object tokens = null);
    }

    public class Translator : ITranslator
    {
        public CultureInfo RootFallbackCulture { get; set; }
        public Dictionary<CultureInfo, ICultureTranslator> CultureTranslators { get; set; }

        public Translator(CultureInfo rootFallbackCulture, Dictionary<CultureInfo, ICultureTranslator> cultureTranslators)
        {
            this.CultureTranslators = cultureTranslators;
            this.RootFallbackCulture = rootFallbackCulture;
        }
        public Translator(CultureInfo rootFallbackCulture, string directoryPath, ILocaleFileLoader fileLoader)
        {
        	this.CultureTranslators = fileLoader.ParseCultureTranslators(directoryPath);
            this.RootFallbackCulture = rootFallbackCulture;
        }

        public ICultureTranslator Get(CultureInfo culture)
        {
            if (this.CultureTranslators.ContainsKey(culture))
            {
                return this.CultureTranslators[culture];
            }   
            return null;
        }
                
        public string Localize(string key, CultureInfo culture, object tokens = null)
        {
            return GracefulFallback(key, culture, (k,c) => 
            {
                var localized = this.Get(c).Let(i => i.Get(k));
                if (tokens == null)
                {
                    return localized;
                }   
                return ApplyInterpolatedItems(localized, tokens);
            });

        }

        public string Localize(DateTime date, CultureInfo culture)
        {
            return GracefulFallback(date, culture, (k,c) => k.ToString("d", c));
        }

        public string Localize(double currencyAmount, CultureInfo culture)
        {
            return GracefulFallback(currencyAmount, culture, (k,c) => k.ToString("C2", c));
        }

        public string Localize(string key, Plurality plurality, CultureInfo locale, object tokens = null)
        {
            return this.Localize(string.Join(".", key, plurality), locale, tokens);
        }
        
        public string GracefulFallback<T>(T key, CultureInfo startCulture, Func<T, CultureInfo, string> transform)
        {
            var hasParentCulture = startCulture.Parent != null;
            return transform(key, startCulture)
                .Recover(() => hasParentCulture 
                    ? transform(key, startCulture.Parent) 
                    : null)
                .Recover(() => transform(key, this.RootFallbackCulture))
                .Recover(() => key.ToString());
        }

        public string ApplyInterpolatedItems(string str, object tokens)
        {
            foreach (var prop in tokens.GetType().GetProperties())
            {
                var replaceKey = string.Format("__{0}__", prop.Name.ToUpper());
                var replaceValue = (string)prop.GetValue(tokens, null);
                str = str.Replace(replaceKey, replaceValue);
            }
            return str;
        }   
    }
}

