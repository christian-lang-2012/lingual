using System;
using NUnit.Framework;
using Lingual.TranslationUtilities;
using Lingual.Enums;


namespace Lingual.Tests
{
	[TestFixture]
	public class UTest
	{
		private readonly TranslationUtility _translator = TranslationUtility.Instance;
		
		[Test]
		[TestCase("login.hello", "Hello!", LocaleEnum.EN)]
		[TestCase("account.invalid.message", "The user name or password provided is incorrect.", LocaleEnum.EN)]
		[TestCase("account.invalid.username", "The username provided is incorrect.", LocaleEnum.EN)]
		[TestCase("login.hello", "Hallo!", LocaleEnum.DE)]
		[TestCase("account.invalid.message", "The user name or password provided is incorrect.", LocaleEnum.DE)]
		[TestCase("account.invalid.username", "Die zur Verfügung gestellten Benutzernamen ist falsch.", LocaleEnum.DE)]
		[TestCase("login.hello", "¡Hola!", LocaleEnum.ES)]
		[TestCase("account.invalid.message", "El nombre de usuario o la contraseña facilitada no es la correcta.", LocaleEnum.ES)]
		[TestCase("account.invalid.username", "El nombre de usuario proporcionado es incorrecta.", LocaleEnum.ES)]
		public void TestTranslation(String key, String answer, LocaleEnum locale)
		{
			Assert.AreEqual(answer, _translator.Translate(key, locale));
		}

		[Test]
		[TestCase("loggedin.user.day", "Hello Cid! Today is Tuesday", LocaleEnum.EN, "Cid", "Tuesday")]
		[TestCase("loggedin.user.day", "Hallo, Cid. Heute ist Dienstag", LocaleEnum.DE, "Cid", "Dienstag")]
		[TestCase("loggedin.user.day", "¡Hola Cid! Hoy es Martes", LocaleEnum.ES, "Cid", "Martes")]
		public void TestInterpolationTranslation(String key, String answer, LocaleEnum locale, params string[] args)
		{
			Assert.AreEqual(answer, _translator.Translate(key, locale, args));
		}

		[Test]
		[TestCase("20.10.2014", LocaleEnum.DE)]
		[TestCase("10/20/2014", LocaleEnum.EN)]
		[TestCase("20/10/2014", LocaleEnum.ES)]
		public void TestDateTranslation (String answer, LocaleEnum locale)
		{
			var date = new DateTime(2014, 10, 20);
			Assert.AreEqual(answer, _translator.TranslateDate(date, locale));
		}

		[Test]
		[TestCase("loggedin.user.inbox", "1 new message", "1", LocaleEnum.EN)]
		[TestCase("loggedin.user.inbox", "2 new messages", "2", LocaleEnum.EN)]
		[TestCase("loggedin.user.inbox", "55 new messagisms", "other", LocaleEnum.EN, "55")]
		[TestCase("loggedin.user.inbox", "1 neue Nachricht", "1", LocaleEnum.DE)]
		[TestCase("loggedin.user.inbox", "2 neue Nachrichten", "2", LocaleEnum.DE)]
		[TestCase("loggedin.user.inbox", "66 neue Nachrichtenismo", "other", LocaleEnum.DE, "66")]
		[TestCase("loggedin.user.inbox", "1 nuevo mensaje", "1", LocaleEnum.ES)]
		[TestCase("loggedin.user.inbox", "99 nuevo mensajismos", "other", LocaleEnum.ES, "99")]
		public void TestPluralTranslation(String key, String answer, String pluralDegree, LocaleEnum locale, params string[] args)
		{
			Assert.AreEqual(answer, _translator.TranslatePlural(key, pluralDegree, locale, args));
		}

		[TestCase("login.hello")]

		public void TestHelloKeyExcpectedToPassTranslation(string key)
		{
			Assert.AreEqual("Hello!", _translator.Translate(key), "The English translation failed for this key: " + key);
			Assert.AreEqual("¡Hola!", _translator.Translate(key, LocaleEnum.ES), "The Spanish translation failed for this key: " + key);
			Assert.AreEqual("Hallo!", _translator.Translate(key, LocaleEnum.DE), "The German translation failed for this key: " + key);
		}

		[TestCase("")]
		[TestCase("account.invalid.message")]
		public void TestHelloKeyExpectedToFailTranslation(string key)
		{
			Assert.AreNotEqual("Hello!", _translator.Translate(key), "English: these should not be equal!");
			Assert.AreNotEqual("¡Hola!", _translator.Translate(key, LocaleEnum.ES), "Spanish: these should not be equal!");
			Assert.AreNotEqual("Hallo!", _translator.Translate(key, LocaleEnum.DE), "German: these should not be equal!");
		}


		[TestCase("loggedin.user.day", "Billy Bob", "Tuesday")]
		public void TestAcceptedInterpolatedStrings(string key, string userName, string day)
		{
			Assert.AreEqual("Hello Billy Bob! Today is Tuesday", _translator.Translate(key, LocaleEnum.EN, userName, day), "English Translation Failed");
			Assert.AreEqual("Hallo, Billy Bob. Heute ist Tuesday", _translator.Translate(key, LocaleEnum.DE, userName, day), "German Translation Failed");
		}

		[TestCase("login.hello")]
		[TestCase("")]
		public void TestExcpectedToFailInterpolatedStrings(string key)
		{
			Assert.AreNotEqual("Hello Billy Bob! Today is Tuesday", _translator.Translate(key));
			Assert.AreNotEqual("Hallo, Billy Bob. Heute ist Tuesday", _translator.Translate(key));
		}

		[TestCase(0.0, "$0.00")]
		[TestCase(10.0, "$10.00")]
		[TestCase(100.0, "$100.00")]
		[TestCase(10000000000.0, "$10,000,000,000.00")]
		public void TestCurrencyValuesEn(double value, string answer)
		{
			Assert.AreEqual(answer, _translator.TranslateCurrency(value));
		}


		[TestCase(0.0, "0,00 €")]
		[TestCase(10.0, "10,00 €")]
		[TestCase(100.0, "100,00 €")]
		[TestCase(10000000000.0, "10.000.000.000,00 €")]
		public void TestCurrencyValuesEs(double value, string answer)
		{
			Assert.AreEqual(answer, _translator.TranslateCurrency(value, LocaleEnum.ES));
		}


		[TestCase(0.0, "0,00 €")]
		[TestCase(10.0, "10,00 €")]
		[TestCase(100.0, "100,00 €")]
		[TestCase(10000000000.0, "10.000.000.000,00 €")]
		public void TestCurrencyValuesDe(double value, string answer)
		{
			Assert.AreEqual(answer, _translator.TranslateCurrency(value, LocaleEnum.DE));
		}
	}
}
