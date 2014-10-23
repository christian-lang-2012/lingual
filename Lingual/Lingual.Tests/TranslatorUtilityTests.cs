using System;
using NUnit.Framework;
using Lingual.TranslationUtilities;


namespace Lingual.Tests
{

	[TestFixture]
	public class TranslatorUtilityTests
	{
		private readonly TranslationUtility _translator = TranslationUtility.Instance;

		[Test]
		[TestCase("login.hello", "Hello!", Locales.EN)]
		[TestCase("login.hello", "Hallo!", Locales.DE)]
		[TestCase("login.hello", "¡Hola!", Locales.ES)]
		[TestCase("account.invalid.message", "The user name or password provided is incorrect.", Locales.EN)]
		[TestCase("account.invalid.message", "El nombre de usuario o la contraseña facilitada no es la correcta.", Locales.ES)]
		[TestCase("account.invalid.message", "The user name or password provided is incorrect.", Locales.DE)]
		[TestCase("account.invalid.username", "The username provided is incorrect.", Locales.EN)]
		[TestCase("account.invalid.username", "Die zur Verfügung gestellten Benutzernamen ist falsch.", Locales.DE)]
		[TestCase("account.invalid.username", "El nombre de usuario proporcionado es incorrecta.", Locales.ES)]
		public void TestTranslation(string key, string answer, Locales locale)
		{
			Assert.AreEqual(answer, _translator.Translate(key, locale), locale + " translation failed for " + key + ". Expected: " + answer);
		}

		[Test]
		[TestCase("loggedin.user.day", "Hello Cid! Today is Tuesday", Locales.EN, "Cid", "Tuesday")]
		[TestCase("loggedin.user.day", "Hallo, Cid. Heute ist Dienstag", Locales.DE, "Cid", "Dienstag")]
		[TestCase("loggedin.user.day", "¡Hola Cid! Hoy es Martes", Locales.ES, "Cid", "Martes")]
		public void TestInterpolationTranslation(string key, string answer, Locales locale, params string[] args)
		{
			Assert.AreEqual(answer, _translator.Translate(key, locale, args), "Interpolation failed for " + locale + ". Expected: " + answer);
		}

		[Test]
		[TestCase("loggedin.user.day", "Hello Cid! Today is Tuesday", Locales.EN, "Tuesday", "Cid")]
		[TestCase("loggedin.user.day", "Hallo, Cid. Heute ist Dienstag", Locales.DE, "", "")]
		public void FalseTestInterpolationTranslation(String key, String answer, Locales locale, params string[] args)
		{
			Assert.AreNotEqual(answer, _translator.Translate(key, locale, args));
		}

		[Test]
		[TestCase("20.10.2014", Locales.DE)]
		[TestCase("10/20/2014", Locales.EN)]
		[TestCase("20/10/2014", Locales.ES)]
		public void TestDateTranslation (string answer, Locales locale)
		{
			var date = new DateTime(2014, 10, 20);
			Assert.AreEqual(answer, _translator.TranslateDate(date, locale), "Date failed for " + locale + ". Expected: " + answer);
		}

		[Test]
		[TestCase("loggedin.user.inbox", "1 new message", PluralDegree.ONE, Locales.EN)]
		[TestCase("loggedin.user.inbox", "2 new messages", PluralDegree.TWO, Locales.EN)]
		[TestCase("loggedin.user.inbox", "55 new messagisms", PluralDegree.OTHER, Locales.EN, "55")]
		[TestCase("loggedin.user.inbox", "1 neue Nachricht", PluralDegree.ONE, Locales.DE)]
		[TestCase("loggedin.user.inbox", "2 neue Nachrichten", PluralDegree.TWO, Locales.DE)]
		[TestCase("loggedin.user.inbox", "66 neue Nachrichtenismo", PluralDegree.OTHER, Locales.DE, "66")]
		[TestCase("loggedin.user.inbox", "1 nuevo mensaje", PluralDegree.ONE, Locales.ES)]
		[TestCase("loggedin.user.inbox", "99 nuevo mensajismos", PluralDegree.OTHER, Locales.ES, "99")]
		public void TestPluralTranslation(string key, string answer, PluralDegree pluralDegree, Locales locale, params string[] args)
		{
			Assert.AreEqual(answer, _translator.TranslatePlural(key, pluralDegree, locale, args), "Pluralization failed for " + locale + " with " + key + ". Expected : " + answer);
		}

		[TestCase("", "Test Data", Locales.EN)]
		[TestCase("", "Test Data", Locales.ES)]
		[TestCase("", "Test Data", Locales.DE)]
		[TestCase("account.invalid.message", "Test Data", Locales.EN)]
		[TestCase("account.invalid.message", "Test Data", Locales.ES)]
		[TestCase("account.invalid.message", "Test Data", Locales.DE)]
		public void TestHelloKeyExpectedToFailTranslation(string key, string answer, Locales locale)
		{
			Assert.AreNotEqual(answer , _translator.Translate(key, locale), locale.ToString() + ": These should not be equal!");
		}


		[TestCase("loggedin.user.day","Hello Billy Bob! Today is Tuesday", "Billy Bob", "Tuesday", Locales.EN)]
		[TestCase("loggedin.user.day", "Hallo, Billy Bob. Heute ist Tuesday", "Billy Bob", "Tuesday", Locales.DE)]
		public void TestAcceptedInterpolatedStrings(string key, string answer, string userName, string day, Locales locale)
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
			Assert.AreEqual(answer, _translator.TranslateCurrency(value, Locales.ES), "Spanish currency failed for: " + value);
		}

		[TestCase(0.0, "0,00 €")]
		[TestCase(10.0, "10,00 €")]
		[TestCase(100.0, "100,00 €")]
		[TestCase(10000000000.0, "10.000.000.000,00 €")]
		public void TestCurrencyValuesDe(double value, string answer)
		{
			Assert.AreEqual(answer, _translator.TranslateCurrency(value, Locales.DE), "Germany currency failed for: " + value);
		}
	}
}
