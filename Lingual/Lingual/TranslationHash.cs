using Lingual.Handlers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lingual.Exceptions;

namespace Lingual
{
	public class TranslationHash
	{
		#region Properties
		public List<TranslationNode> TranslationNodes { get; private set; }

		public LocaleEnum TranslationLocale { get; private set; }
		#endregion

		#region Core Methods
		public TranslationHash(LocaleEnum locale)
		{
			TranslationLocale = locale;
			TranslationNodes = new List<TranslationNode>();
		}

		/// <summary>
		/// Creates a translation node and addes it to the list of nodes using the key and the value.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public void AddTranslationNode(string key, string value)
		{
			if (!CheckToIfKeyAlreadyExistsInHash(key))
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

		public bool IsTranslationHashEmpty()
		{
			return TranslationNodes.Any();
		}

		#endregion

		#region Private Helper Methods
		private bool CheckToIfKeyAlreadyExistsInHash(string key)
		{
			bool exists = false;
			foreach (TranslationNode tn in TranslationNodes.Where(tn => tn.Key == key))
			{
				exists = true;
			}

			return exists;
		}
		#endregion
	}
}
