using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Lingual.TranslationUtilities
{
	public class TranslationDictionary
	{
		#region Properties and Variables

		private Dictionary<string, string> translationDictionary;

		public Locales TranslationLocale { get; private set; }
		#endregion

		#region Core Methods

		/// <summary>
		/// Constructor for the TranslationDictionary. Must take in a LocaleEnum
		/// </summary>
		/// <param name="locale"></param>
		public TranslationDictionary(Locales locale)
		{
			TranslationLocale = locale;
			translationDictionary = new Dictionary<string, string>();
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
				translationDictionary.Add(key.ToLower(), value);
			}
		}

		/// <summary>
		/// Gets the value corresponding to the passed in key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public string GetValue(string key)
		{
			var returnValue = KeyExists(key) ? translationDictionary[key] : key;

			return returnValue;
		}

		/// <summary>
		/// Get's the Translation of the key with the plural degree
		/// </summary>
		/// <param name="key"></param>
		/// <param name="pluralDegree"></param>
		/// <returns></returns>
		public string GetValue(string key, string pluralDegree)
		{
			//Default and params don't work together. Only way to get it to work is method overload
			var nonPluralTrans = GetValue(key);
			var jarrayParse = JArray.Parse(nonPluralTrans);
			return PluralKeyFinder(jarrayParse, pluralDegree);
		}

		/// <summary>
		/// Checks to see if the translation dictionary is empty
		/// </summary>
		/// <returns></returns>
		public bool IsTranslationDictionaryEmpty()
		{
			return translationDictionary.Any();
		}

		#endregion

		#region Private Helper Methods
		private bool KeyExists(string key)
		{
			return translationDictionary.ContainsKey(key.ToLower());
		}

		private string PluralKeyFinder(JArray pluralKeys, string pluralCount)
		{
			var pluralVal = "No translation";
			foreach (JProperty property in pluralKeys.Children<JObject>().SelectMany(content => content.Properties().Where(t => t.Name == pluralCount)))
			{
				pluralVal = property.Value.ToString();
			}
			return pluralVal;
		}
		#endregion
	}
}
