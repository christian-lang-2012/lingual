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
		[TestCase("account.invalid.message")]
		public void TestHelloKeyTranslation(string key)
		{
			Assert.AreEqual(translator.Translate(key), "Hello!", "The English translation failed for this key: " + key);
			Assert.AreEqual(translator.Translate(key, LocaleEnum.ES), "¡Hola!", "The Spanish translation failed for this key: " + key);
			Assert.AreEqual(translator.Translate(key, LocaleEnum.DE), "Hallo!", "The German translation failed for this key: " + key);
		}


		[TestCase("loggedin.user.day", "Billy Bob", "Tuesday")]
		public void TestInterpolatedStrings(string key, string userName, string day)
		{
			Assert.AreEqual(translator.Translate(key, LocaleEnum.EN, userName, day), "Hello Billy Bob! Today is Tuesday", "English Translation Failed");
			Assert.AreEqual(translator.Translate(key, LocaleEnum.DE, userName, day), "Hallo, Billy Bob. Heute ist Tuesday", "German Translation Failed");
		}
	}
}
