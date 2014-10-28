using System;
using NUnit.Framework;
using Lingual.TranslationUtilities;


namespace Lingual.Test
{

    [TestFixture]
    public class TranslatorUtilityTests
    {
        private readonly TranslationUtility _translator = TranslationUtility.Instance;

        //TODO: add unit tests for fall back logic

        [Test]
        [TestCase("login.hello", "Hello!", Locale.en_US)]
        [TestCase("login.hello", "Hallo!", Locale.de_DE)]
        [TestCase("login.hello", "¡Hola!", Locale.es_MX)]
        [TestCase("account.invalid.message", "The user name or password provided is incorrect.", Locale.en_US)]
        [TestCase("account.invalid.message", "El nombre de usuario o la contraseña facilitada no es la correcta.", Locale.es_MX)]
        [TestCase("account.invalid.message", "The user name or password provided is incorrect.", Locale.de_DE)]
        [TestCase("account.invalid.username", "The username provided is incorrect.", Locale.en_US)]
        [TestCase("account.invalid.username", "Die zur Verfügung gestellten Benutzernamen ist falsch.", Locale.de_DE)]
        [TestCase("account.invalid.username", "El nombre de usuario proporcionado es incorrecta.", Locale.es_MX)]
        public void TestTranslation(string key, string answer, Locale locale)
        {
            Assert.AreEqual(answer, _translator.Translate(key, locale), locale + " translation failed for " + key + ". Expected: " + answer);
        }

        [Test]
        [TestCase("loggedin.user.day", "Hello Cid! Today is Tuesday", Locale.en_US, "Cid", "Tuesday")]
        [TestCase("loggedin.user.day", "Hallo, Cid. Heute ist Dienstag", Locale.de_DE, "Cid", "Dienstag")]
        [TestCase("loggedin.user.day", "¡Hola Cid! Hoy es Martes", Locale.es_MX, "Cid", "Martes")]
        public void TestInterpolationTranslation(string key, string answer, Locale locale, params string[] args)
        {
            Assert.AreEqual(answer, _translator.Translate(key, locale, args), "Interpolation failed for " + locale + ". Expected: " + answer);
        }

        [Test]
        [TestCase("loggedin.user.day", "Hello Cid! Today is Tuesday", Locale.en_US, "Tuesday", "Cid")]
        [TestCase("loggedin.user.day", "Hallo, Cid. Heute ist Dienstag", Locale.de_DE, "", "")]
        public void FalseTestInterpolationTranslation(String key, String answer, Locale locale, params string[] args)
        {
            Assert.AreNotEqual(answer, _translator.Translate(key, locale, args));
        }

        [Test]
        [TestCase("20.10.2014", Locale.de_DE)]
        [TestCase("10/20/2014", Locale.en_US)]
        [TestCase("20/10/2014", Locale.es_MX)]
        public void TestDateTranslation(string answer, Locale locale)
        {
            var date = new DateTime(2014, 10, 20);
            Assert.AreEqual(answer, _translator.Localize(date, locale), "Date failed for " + locale + ". Expected: " + answer);
        }

        [Test]
        [TestCase("loggedin.user.inbox", "1 new message", PluralDegree.ONE, Locale.en_US)]
        [TestCase("loggedin.user.inbox", "2 new messages", PluralDegree.TWO, Locale.en_US)]
        [TestCase("loggedin.user.inbox", "55 new messagisms", PluralDegree.OTHER, Locale.en_US, "55")]
        [TestCase("loggedin.user.inbox", "1 neue Nachricht", PluralDegree.ONE, Locale.de_DE)]
        [TestCase("loggedin.user.inbox", "2 neue Nachrichten", PluralDegree.TWO, Locale.de_DE)]
        [TestCase("loggedin.user.inbox", "66 neue Nachrichtenismo", PluralDegree.OTHER, Locale.de_DE, "66")]
        [TestCase("loggedin.user.inbox", "1 nuevo mensaje", PluralDegree.ONE, Locale.es_MX)]
        [TestCase("loggedin.user.inbox", "99 nuevo mensajismos", PluralDegree.OTHER, Locale.es_MX, "99")]
        public void TestPluralTranslation(string key, string answer, PluralDegree pluralDegree, Locale locale, params string[] args)
        {
            Assert.AreEqual(answer, _translator.TranslatePlural(key, pluralDegree, locale, args), "Pluralization failed for " + locale + " with " + key + ". Expected : " + answer);
        }

        [TestCase("", "Test Data", Locale.en_US)]
        [TestCase("", "Test Data", Locale.es_MX)]
        [TestCase("", "Test Data", Locale.de_DE)]
        [TestCase("account.invalid.message", "Test Data", Locale.en_US)]
        [TestCase("account.invalid.message", "Test Data", Locale.es_MX)]
        [TestCase("account.invalid.message", "Test Data", Locale.de_DE)]
        public void TestHelloKeyExpectedToFailTranslation(string key, string answer, Locale locale)
        {
            Assert.AreNotEqual(answer, _translator.Translate(key, locale), locale.ToString() + ": These should not be equal!");
        }


        [TestCase("loggedin.user.day", "Hello Billy Bob! Today is Tuesday", "Billy Bob", "Tuesday", Locale.en_US)]
        [TestCase("loggedin.user.day", "Hallo, Billy Bob. Heute ist Tuesday", "Billy Bob", "Tuesday", Locale.de_DE)]
        public void TestAcceptedInterpolatedStrings(string key, string answer, string userName, string day, Locale locale)
        {
            Assert.AreEqual(answer, _translator.Translate(key, locale, userName, day), "Interpolation failed for " + locale + ". Expected: " + answer);
        }

        [TestCase(0.0, "$0.00")]
        [TestCase(10.0, "$10.00")]
        [TestCase(100.0, "$100.00")]
        [TestCase(10000000000.0, "$10,000,000,000.00")]
        public void TestCurrencyValuesEn(double value, string answer)
        {
            Assert.AreEqual(answer, _translator.Localize(value), "English currency failed for: " + value);
        }


        [TestCase(0.0, "$0.00")]
        [TestCase(10.0, "$10.00")]
        [TestCase(100.0, "$100.00")]
        [TestCase(10000000000.0, "$10,000,000,000.00")]
        public void TestCurrencyValuesEs(double value, string answer)
        {
            Assert.AreEqual(answer, _translator.Localize(value, Locale.es_MX), "Spanish currency failed for: " + value);
        }

        [TestCase(0.0, "0,00 €")]
        [TestCase(10.0, "10,00 €")]
        [TestCase(100.0, "100,00 €")]
        [TestCase(10000000000.0, "10.000.000.000,00 €")]
        public void TestCurrencyValuesDe(double value, string answer)
        {
            Assert.AreEqual(answer, _translator.Localize(value, Locale.de_DE), "Germany currency failed for: " + value);
        }
    }
}
