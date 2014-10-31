using System;
using NUnit.Framework;
using Lingual;
using System.Collections.Generic;


namespace Lingual.Test
{

    [TestFixture]
    public class TranslatorUtilityTests
    {
        private readonly Translator _translator = Translator.Instance;

        //TODO: add unit tests for fall back logic

        [Test]
        [TestCase("hello.test.message", "This is a testie test", Locale.en_US)]
        [TestCase("this.key.doesnt.exist", "this.key.doesnt.exist", Locale.en_US)]
        public void TestFallbackLogic(string key, string answer, Locale locale)
        {
            Assert.AreEqual(answer, _translator.Translate(key, locale), "Fallback failed.");
        }

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
        [TestCase("loggedin.user.day", "¡Hola Cid! Hoy es Martes", Locale.es_MX)]
        public void TestInterpolationTranslation(string key, string answer, Locale locale)
        {
            var testDictionary = new Dictionary<string, string>() { { "__NAME__", "Cid" }, { "__DAY__", "Martes" } };
            Assert.AreEqual(answer, _translator.Translate(key, testDictionary, locale), "Interpolation failed for " + locale + ". Expected: " + answer);
        }

        [Test]
        [TestCase("loggedin.user.day", "Hallo, Cid. Heute ist Dienstag", Locale.de_DE)]
        public void TestInterpolationTranslation_DE(string key, string answer, Locale locale)
        {
            var testDictionary = new  Dictionary<string, string>() { {"__NAME__", "Cid"}, {"__DAY__", "Dienstag"} };
            Assert.AreEqual(answer, _translator.Translate(key, testDictionary, locale), "Interpolation failed for " + locale + ". Expected: " + answer);
        }

        [Test]
        [TestCase("loggedin.user.day", "Hello Cid! Today is Tuesday", Locale.en_US)]
        public void TestInterpolationTranslation_EN(string key, string answer, Locale locale)
        {
            var testDictionary = new Dictionary<string, string>() { { "__NAME__", "Cid" }, { "__DAY__", "Tuesday" } };
            Assert.AreEqual(answer, _translator.Translate(key, testDictionary, locale), "Interpolation failed for " + locale + ". Expected: " + answer);
        }

        [Test]
        [TestCase("loggedin.user.day", "Hello Cid! Today is Tuesday", Locale.en_US)]
        public void FalseTestInterpolationTranslation(String key, String answer, Locale locale)
        {
            var testDictionary = new Dictionary<string, string>() { { "_NuM_", "Cid" }, { "__D__", "Tuesday" } };
            Assert.AreNotEqual(answer, _translator.Translate(key, testDictionary, locale));
        }

        [Test]
        [TestCase("loggedin.user.day", "Hallo, Cid. Heute ist Dienstag", Locale.de_DE)]
        public void FalseTestInterpolationTranslation_EmptyDictionary(String key, String answer, Locale locale)
        {
            var testDictionary = new Dictionary<string, string>();
            Assert.AreNotEqual(answer, _translator.Translate(key, testDictionary, locale));
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
        [TestCase("loggedin.user.inbox", "1 new message", Plurality.ONE, Locale.en_US)]
        [TestCase("loggedin.user.inbox", "2 new messages", Plurality.TWO, Locale.en_US)]
        [TestCase("loggedin.user.inbox", "1 neue Nachricht", Plurality.ONE, Locale.de_DE)]
        [TestCase("loggedin.user.inbox", "2 neue Nachrichten", Plurality.TWO, Locale.de_DE)]
        [TestCase("loggedin.user.inbox", "1 nuevo mensaje", Plurality.ONE, Locale.es_MX)]
        public void TestPluralTranslation_All(string key, string answer, Plurality plurality, Locale locale)
        {
            Assert.AreEqual(answer, _translator.TranslatePlural(key, plurality, locale), "Pluralization failed for " + locale + " with " + key + ". Expected : " + answer);
        }


        [TestCase("loggedin.user.inbox", "55 new messages", Plurality.OTHER, Locale.en_US)]
        public void TestPluralTranslation_EN(string key, string answer, Plurality plurality, Locale locale)
        {
            var testDictionary = new Dictionary<string, string>() { {"__AMOUNT__", "55" } };
            Assert.AreEqual(answer, _translator.TranslatePlural(key, plurality, testDictionary, locale), "Pluralization failed for " + locale + " with " + key + ". Expected : " + answer);
        }

        [TestCase("loggedin.user.inbox", "66 neue Nachrichtenismo", Plurality.OTHER, Locale.de_DE)]
        public void TestPluralTranslation_DE(string key, string answer, Plurality plurality, Locale locale)
        {
            var testDictionary =  new Dictionary<string, string>() { { "__AMOUNT__", "66" } };
            Assert.AreEqual(answer, _translator.TranslatePlural(key, plurality, testDictionary, locale), "Pluralization failed for " + locale + " with " + key + ". Expected : " + answer);
        }

        [TestCase("loggedin.user.inbox", "99 nuevo mensajismos", Plurality.OTHER, Locale.es_MX)]
        public void TestPluralTranslation_ES(string key, string answer, Plurality plurality, Locale locale)
        {
            var testDictionary = new Dictionary<string, string>() { { "__AMOUNT__", "99" } };
            Assert.AreEqual(answer, _translator.TranslatePlural(key, plurality, testDictionary, locale), "Pluralization failed for " + locale + " with " + key + ". Expected : " + answer);
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
