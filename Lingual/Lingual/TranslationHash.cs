using Lingual.Handlers;
using Newtonsoft.Json.Linq;
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
            TranslationLocale = LocaleEnum.Es;
		}

        public void setTranslationNodes()
        {
            var localeJsonObj = LocaleFileHandler.GetLocaleFile(TranslationLocale);
            foreach (JToken prop in localeJsonObj )
            {

            }
        }

		public void AddTranslationNode(string key, string value)
		{
			TranslationNodes.Add(new TranslationNode() { Key = key, Value = value });
		}

		public TranslationNode GetTranslationNode(string key)
		{
			TranslationNode tn = new TranslationNode()
			{
				Key = "Key Not Found",
				Value = "No translation available"
			};

			foreach (TranslationNode t in TranslationNodes)
			{
				if (t.Key == key)
					tn = t;
			}

			return tn;
		}

	}
}
