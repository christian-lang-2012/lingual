using Lingual.Enums;
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
		public string GetValue(string key, params string[] arguments)
		{
			var returnValue = "Key doesn't exist";
			if (KeyExists(key))
			{
				returnValue = translationDictionary[key];
			}
            if (arguments.Any())
            {
                returnValue = string.Format(returnValue, arguments);
            }
			return returnValue;
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
		#endregion
	}
}
