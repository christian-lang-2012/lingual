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
		[TestCase("login.hello", "Hallo!", LocaleEnum.DE)]
		[TestCase("login.hello", "¡Hola!", LocaleEnum.ES)]
		[TestCase("account.invalid.message", "The user name or password provided is incorrect.", LocaleEnum.EN)]
		[TestCase("account.invalid.message", "El nombre de usuario o la contraseña facilitada no es la correcta.", LocaleEnum.ES)]
		[TestCase("account.invalid.message", "The user name or password provided is incorrect.", LocaleEnum.DE)]
		[TestCase("account.invalid.username", "The username provided is incorrect.", LocaleEnum.EN)]
		[TestCase("account.invalid.username", "Die zur Verfügung gestellten Benutzernamen ist falsch.", LocaleEnum.DE)]
		[TestCase("account.invalid.username", "El nombre de usuario proporcionado es incorrecta.", LocaleEnum.ES)]
		public void TestTranslation(string key, string answer, LocaleEnum locale)
		{
			Assert.AreEqual(answer, _translator.Translate(key, locale), locale + " translation failed for " + key + ". Expected: " + answer);
		}

		[Test]
		[TestCase("loggedin.user.day", "Hello Cid! Today is Tuesday", LocaleEnum.EN, "Cid", "Tuesday")]
		[TestCase("loggedin.user.day", "Hallo, Cid. Heute ist Dienstag", LocaleEnum.DE, "Cid", "Dienstag")]
		[TestCase("loggedin.user.day", "¡Hola Cid! Hoy es Martes", LocaleEnum.ES, "Cid", "Martes")]
		public void TestInterpolationTranslation(string key, string answer, LocaleEnum locale, params string[] args)
		{
			Assert.AreEqual(answer, _translator.Translate(key, locale, args), "Interpolation failed for " + locale + ". Expected: " + answer);
		}

		[Test]
		[TestCase("20.10.2014", LocaleEnum.DE)]
		[TestCase("10/20/2014", LocaleEnum.EN)]
		[TestCase("20/10/2014", LocaleEnum.ES)]
		public void TestDateTranslation (string answer, LocaleEnum locale)
		{
			var date = new DateTime(2014, 10, 20);
			Assert.AreEqual(answer, _translator.TranslateDate(date, locale), "Date failed for " + locale + ". Expected: " + answer);
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
		public void TestPluralTranslation(string key, string answer, string pluralDegree, LocaleEnum locale, params string[] args)
		{
			Assert.AreEqual(answer, _translator.TranslatePlural(key, pluralDegree, locale, args), "Pluralization failed for " + locale + " with " + key + ". Expected : " + answer);
		}

		[TestCase("", "Test Data", LocaleEnum.EN)]
		[TestCase("", "Test Data", LocaleEnum.ES)]
		[TestCase("", "Test Data", LocaleEnum.DE)]
		[TestCase("account.invalid.message", "Test Data", LocaleEnum.EN)]
		[TestCase("account.invalid.message", "Test Data", LocaleEnum.ES)]
		[TestCase("account.invalid.message", "Test Data", LocaleEnum.DE)]
		public void TestHelloKeyExpectedToFailTranslation(string key, string answer, LocaleEnum locale)
		{
			Assert.AreNotEqual(answer , _translator.Translate(key, locale), locale.ToString() + ": These should not be equal!");
		}


		[TestCase("loggedin.user.day","Hello Billy Bob! Today is Tuesday", "Billy Bob", "Tuesday", LocaleEnum.EN)]
		[TestCase("loggedin.user.day", "Hallo, Billy Bob. Heute ist Tuesday", "Billy Bob", "Tuesday", LocaleEnum.DE)]
		public void TestAcceptedInterpolatedStrings(string key, string answer, string userName, string day, LocaleEnum locale)
		{
			Assert.AreEqual(answer, _translator.Translate(key, locale, userName, day), "Interpolation failed for " + locale + ". Expected: " + answer);
		}

		[TestCase(0.0, "$0.00")]
		[TestCase(10.0, "$10.00")]
		[TestCase(100.0, "$100.00")]
		[TestCase(10000000000.0, "$10,000,000,000.00")]
		public void TestCurrencyValuesEn(double value, string answer)
		{
			Assert.AreEqual(answer, _translator.TranslateCurrency(value), "English currency failed for: " + value);
		}


		[TestCase(0.0, "0,00 €")]
		[TestCase(10.0, "10,00 €")]
		[TestCase(100.0, "100,00 €")]
		[TestCase(10000000000.0, "10.000.000.000,00 €")]
		public void TestCurrencyValuesEs(double value, string answer)
		{
			Assert.AreEqual(answer, _translator.TranslateCurrency(value, LocaleEnum.ES), "Spanish currency failed for: " + value);
		}


		[TestCase(0.0, "0,00 €")]
		[TestCase(10.0, "10,00 €")]
		[TestCase(100.0, "100,00 €")]
		[TestCase(10000000000.0, "10.000.000.000,00 €")]
		public void TestCurrencyValuesDe(double value, string answer)
		{
			Assert.AreEqual(answer, _translator.TranslateCurrency(value, LocaleEnum.DE), "Germany currency failed for: " + value);
		}
	}
}
