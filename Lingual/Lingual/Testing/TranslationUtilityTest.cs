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
		
		public void TestHelloKeyExcpectedToPassTranslation(string key)
		{
			Assert.AreEqual("Hello!", translator.Translate(key), "The English translation failed for this key: " + key);
			Assert.AreEqual( "¡Hola!", translator.Translate(key, LocaleEnum.ES), "The Spanish translation failed for this key: " + key);
			Assert.AreEqual("Hallo!", translator.Translate(key, LocaleEnum.DE), "The German translation failed for this key: " + key);
		}

		[TestCase("")]
		[TestCase("account.invalid.message")]
		public void TestHelloKeyExpectedToFailTranslation(string key)
		{
			Assert.AreNotEqual("Hello!", translator.Translate(key), "English: these should not be equal!");
			Assert.AreNotEqual("¡Hola!", translator.Translate(key, LocaleEnum.ES), "Spanish: these should not be equal!");
			Assert.AreNotEqual("Hallo!", translator.Translate(key, LocaleEnum.DE), "German: these should not be equal!");
		}

		
		[TestCase("loggedin.user.day", "Billy Bob", "Tuesday")]
		public void TestAcceptedInterpolatedStrings(string key, string userName, string day)
		{
			Assert.AreEqual("Hello Billy Bob! Today is Tuesday", translator.Translate(key, LocaleEnum.EN, userName, day), "English Translation Failed");
			Assert.AreEqual("Hallo, Billy Bob. Heute ist Tuesday", translator.Translate(key, LocaleEnum.DE, userName, day), "German Translation Failed");
		}

		[TestCase("login.hello")]
		[TestCase("")]
		public void TestExcpectedToFailInterpolatedStrings(string key)
		{
			Assert.AreNotEqual("Hello Billy Bob! Today is Tuesday", translator.Translate(key));
			Assert.AreNotEqual("Hallo, Billy Bob. Heute ist Tuesday", translator.Translate(key));
		}

		[TestCase(0.0, "$0.00")]
		[TestCase(10.0, "$10.00")]
		[TestCase(100.0, "$100.00")]
		[TestCase(10000000000.0, "$10,000,000,000.00")]
		public void TestCurrencyValuesEn(double value, string answer)
		{
			Assert.AreEqual(answer, translator.TranslateCurrency(value));
		}


		[TestCase(0.0, "0,00 €")]
		[TestCase(10.0, "10,00 €")]
		[TestCase(100.0, "100,00 €")]
		[TestCase(10000000000.0, "10.000.000.000,00 €")]
		public void TestCurrencyValuesEs(double value, string answer)
		{
			Assert.AreEqual(answer, translator.TranslateCurrency(value, LocaleEnum.ES));
		}


		[TestCase(0.0, "0,00 €")]
		[TestCase(10.0, "10,00 €")]
		[TestCase(100.0, "100,00 €")]
		[TestCase(10000000000.0, "10.000.000.000,00 €")]
		public void TestCurrencyValuesDe(double value, string answer)
		{
			Assert.AreEqual(answer, translator.TranslateCurrency(value, LocaleEnum.DE));
		}
	}
}
