using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Lingual.Infrastructure;
using System.Text.RegularExpressions;

namespace Lingual
{
    public interface ITranslator
    {
        string Localize(string key, CultureInfo culture, object tokens = null);
        string Localize(DateTime date, CultureInfo culture);
        string Localize(double currencyAmount, CultureInfo culture);
        string Localize(decimal currencyAmount, CultureInfo culture);
        string Localize(string key, Plurality plurality, CultureInfo culture, object tokens = null);
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
        public Translator(CultureInfo rootFallbackCulture, string directoryPath, ILocaleDirectoryLoader fileLoader)
        {
        	this.CultureTranslators = fileLoader.ParseCultureTranslators(directoryPath);
            this.RootFallbackCulture = rootFallbackCulture;
        }

        public ICultureTranslator Get(CultureInfo culture)
        {
            if (culture.Let(this.CultureTranslators.ContainsKey, false))
            {
                return this.CultureTranslators[culture];
            }   
            return null;
        }
                
        public string Localize(string key, CultureInfo startCulture, object tokens = null)
        {
            return GracefulFallback(key, startCulture, (k,c) => 
            {
                var localized = this.Get(c).Let(translator => translator.Get(k));
                if (tokens == null)
                {
                    return localized;
                }   
                return ApplyInterpolatedItems(localized, tokens);
            });

        }

        public string Localize(DateTime date, CultureInfo startCulture)
        {
            return GracefulFallback(date, startCulture, (k,c) => k.ToString("d", c));
        }

        public string Localize(double currencyAmount, CultureInfo startCulture)
        {
            return GracefulFallback(currencyAmount, startCulture, (k,c) => k.ToString("C2", c));
        }
        public string Localize(decimal currencyAmount, CultureInfo startCulture)
        {
            return GracefulFallback(currencyAmount, startCulture, (k,c) => k.ToString("C2", c));
        }

        public string Localize(string key, Plurality plurality, CultureInfo startCulture, object tokens = null)
        {
            return this.Localize(string.Join(".", key, plurality), startCulture, tokens);
        }
        
        public string GracefulFallback<T>(T key, CultureInfo startCulture, Func<T, CultureInfo, string> transform)
        {
            var hasParentCulture = startCulture.Let(i => i.Parent) != null;
            return transform(key, startCulture)
                .Recover(() => hasParentCulture 
                    ? transform(key, startCulture.Parent) 
                    : null)
                .Recover(() => transform(key, this.RootFallbackCulture))
                .Recover(() => key.Let(i => i.ToString()) ?? string.Empty);
        }

        public string ApplyInterpolatedItems(string str, object tokens)
        {
            return tokens.GetType().GetProperties()
                .Aggregate(str, (accumulator, prop) => 
                {
                    var pattern = string.Format("__{0}__", prop.Name);
                    var replaceValue = prop.GetValue(tokens, null).ToString();
                    return Regex.Replace(accumulator, pattern, replaceValue,RegexOptions.IgnoreCase);
                });
        }   
    }
}

