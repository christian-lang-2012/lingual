using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lingual.Enums;
using Lingual.TranslationUtilities;
using NUnit.Framework;

namespace Lingual.Testing
{
	public class TranslationUtilityTest
	{
		private TranslationUtility translator = TranslationUtility.Instance;

		[TestCase("login.hello")]
		public void TestHelloKeyTranslation(string key)
		{
			Assert.AreEqual(translator.Translate(key), "Hello!", "The English translation failed for this key: " + key);
			Assert.AreEqual(translator.Translate(key, LocaleEnum.ES), "�Hola!", "The Spanish translation failed for this key: " + key);
			Assert.AreEqual(translator.Translate(key, LocaleEnum.DE), "Hallo!", "The German translation failed for this key: " + key);
		}
	}
}
