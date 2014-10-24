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
        [TestCase("login.hello", "Hello!", Locale.EN)]
        [TestCase("login.hello", "Hallo!", Locale.DE)]
        [TestCase("login.hello", "¡Hola!", Locale.ES)]
        [TestCase("account.invalid.message", "The user name or password provided is incorrect.", Locale.EN)]
        [TestCase("account.invalid.message", "El nombre de usuario o la contraseña facilitada no es la correcta.", Locale.ES)]
        [TestCase("account.invalid.message", "The user name or password provided is incorrect.", Locale.DE)]
        [TestCase("account.invalid.username", "The username provided is incorrect.", Locale.EN)]
        [TestCase("account.invalid.username", "Die zur Verfügung gestellten Benutzernamen ist falsch.", Locale.DE)]
        [TestCase("account.invalid.username", "El nombre de usuario proporcionado es incorrecta.", Locale.ES)]
        public void TestTranslation(string key, string answer, Locale locale)
        {
            Assert.AreEqual(answer, _translator.Translate(key, locale), locale + " translation failed for " + key + ". Expected: " + answer);
        }

        [Test]
        [TestCase("loggedin.user.day", "Hello Cid! Today is Tuesday", Locale.EN, "Cid", "Tuesday")]
        [TestCase("loggedin.user.day", "Hallo, Cid. Heute ist Dienstag", Locale.DE, "Cid", "Dienstag")]
        [TestCase("loggedin.user.day", "¡Hola Cid! Hoy es Martes", Locale.ES, "Cid", "Martes")]
        public void TestInterpolationTranslation(string key, string answer, Locale locale, params string[] args)
        {
            Assert.AreEqual(answer, _translator.Translate(key, locale, args), "Interpolation failed for " + locale + ". Expected: " + answer);
        }

        [Test]
        [TestCase("loggedin.user.day", "Hello Cid! Today is Tuesday", Locale.EN, "Tuesday", "Cid")]
        [TestCase("loggedin.user.day", "Hallo, Cid. Heute ist Dienstag", Locale.DE, "", "")]
        public void FalseTestInterpolationTranslation(String key, String answer, Locale locale, params string[] args)
        {
            Assert.AreNotEqual(answer, _translator.Translate(key, locale, args));
        }

        [Test]
        [TestCase("20.10.2014", Locale.DE)]
        [TestCase("10/20/2014", Locale.EN)]
        [TestCase("20/10/2014", Locale.ES)]
        public void TestDateTranslation(string answer, Locale locale)
        {
            var date = new DateTime(2014, 10, 20);
            Assert.AreEqual(answer, _translator.Localize(date, locale), "Date failed for " + locale + ". Expected: " + answer);
        }

        [Test]
        [TestCase("loggedin.user.inbox", "1 new message", PluralDegree.ONE, Locale.EN)]
        [TestCase("loggedin.user.inbox", "2 new messages", PluralDegree.TWO, Locale.EN)]
        [TestCase("loggedin.user.inbox", "55 new messagisms", PluralDegree.OTHER, Locale.EN, "55")]
        [TestCase("loggedin.user.inbox", "1 neue Nachricht", PluralDegree.ONE, Locale.DE)]
        [TestCase("loggedin.user.inbox", "2 neue Nachrichten", PluralDegree.TWO, Locale.DE)]
        [TestCase("loggedin.user.inbox", "66 neue Nachrichtenismo", PluralDegree.OTHER, Locale.DE, "66")]
        [TestCase("loggedin.user.inbox", "1 nuevo mensaje", PluralDegree.ONE, Locale.ES)]
        [TestCase("loggedin.user.inbox", "99 nuevo mensajismos", PluralDegree.OTHER, Locale.ES, "99")]
        public void TestPluralTranslation(string key, string answer, PluralDegree pluralDegree, Locale locale, params string[] args)
        {
            Assert.AreEqual(answer, _translator.TranslatePlural(key, pluralDegree, locale, args), "Pluralization failed for " + locale + " with " + key + ". Expected : " + answer);
        }

        [TestCase("", "Test Data", Locale.EN)]
        [TestCase("", "Test Data", Locale.ES)]
        [TestCase("", "Test Data", Locale.DE)]
        [TestCase("account.invalid.message", "Test Data", Locale.EN)]
        [TestCase("account.invalid.message", "Test Data", Locale.ES)]
        [TestCase("account.invalid.message", "Test Data", Locale.DE)]
        public void TestHelloKeyExpectedToFailTranslation(string key, string answer, Locale locale)
        {
            Assert.AreNotEqual(answer, _translator.Translate(key, locale), locale.ToString() + ": These should not be equal!");
        }


        [TestCase("loggedin.user.day", "Hello Billy Bob! Today is Tuesday", "Billy Bob", "Tuesday", Locale.EN)]
        [TestCase("loggedin.user.day", "Hallo, Billy Bob. Heute ist Tuesday", "Billy Bob", "Tuesday", Locale.DE)]
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


        [TestCase(0.0, "0,00 €")]
        [TestCase(10.0, "10,00 €")]
        [TestCase(100.0, "100,00 €")]
        [TestCase(10000000000.0, "10.000.000.000,00 €")]
        public void TestCurrencyValuesEs(double value, string answer)
        {
            Assert.AreEqual(answer, _translator.Localize(value, Locale.ES), "Spanish currency failed for: " + value);
        }

        [TestCase(0.0, "0,00 €")]
        [TestCase(10.0, "10,00 €")]
        [TestCase(100.0, "100,00 €")]
        [TestCase(10000000000.0, "10.000.000.000,00 €")]
        public void TestCurrencyValuesDe(double value, string answer)
        {
            Assert.AreEqual(answer, _translator.Localize(value, Locale.DE), "Germany currency failed for: " + value);
        }
    }
}
