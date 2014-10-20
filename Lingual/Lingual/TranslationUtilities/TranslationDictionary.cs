using Lingual.Enums;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Lingual.TranslationUtilities
{
	public class TranslationDictionary
	{
		#region Properties and Variables

		private Dictionary<string, string> translationDictionary;

		public LocaleEnum TranslationLocale { get; private set; }
		#endregion

		#region Core Methods
		public TranslationDictionary(LocaleEnum locale)
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
			var returnValue = "Key doesn't exist";
			if (KeyExists(key))
			{
				returnValue = translationDictionary[key];
			}
			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="isPlural"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public string GetValue(string key, string pluralDegree)
		{
			//Default and params don't work together. Only way to get it to work is method overload
			var nonPluralTrans = GetValue(key);
			var jarrayParse = JArray.Parse(nonPluralTrans);
			return PluralKeyFinder(jarrayParse, pluralDegree);
		}

		public bool IsTranslationHashEmpty()
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
			string pluralVal = "No translation";
			foreach (JObject content in pluralKeys.Children<JObject>())
			{
				foreach (JProperty prop in content.Properties().Where(t => t.Name == pluralCount))
				{
					pluralVal = prop.Value.ToString();
				}
			}
			return pluralVal;
		}
		#endregion
	}
}
