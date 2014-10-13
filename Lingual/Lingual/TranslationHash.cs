using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lingual
{
	public class TranslationHash
	{
		public List<TranslationNode> TranslationNodes { get; set; }

		public LocaleEnum TranslationLocale { get; set; }


		public TranslationHash()
		{
			TranslationNodes = new List<TranslationNode>();
		}

		/// <summary>
		/// Creates a translation node and addes it to the list of nodes using the key and the value.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public void AddTranslationNode(string key, string value)
		{
			TranslationNodes.Add(new TranslationNode() { Key = key, Value = value });
		}

		/// <summary>
		/// Gets the translation node using the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public TranslationNode GetTranslationNode(string key)
		{
			var tn = new TranslationNode()
			{
				Key = "Key Not Found",
				Value = "No translation available"
			};

			foreach (var t in TranslationNodes.Where(t => t.Key == key))
			{
				tn = t;
			}

			return tn;
		}

	}
}
