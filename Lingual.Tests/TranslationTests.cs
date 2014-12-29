using System;
using NUnit.Framework;
using System.Globalization;
using System.IO;
namespace Lingual.Tests
{
    [TestFixture]
    public class TranslationTests
    {
    	Translator translator = new Translator(new CultureInfo("en"), "locale", new LocaleDirectoryLoader());
    	
        [Test]
        public void TestTranslation()
        {
            var cases = new[]
            {
                new { key = "login.hello", answer = "Hello!", culture = new CultureInfo("en-US") },
                new { key = "login.hello", answer = "Hallo!", culture = new CultureInfo("de-de") },
                new { key = "login.hello", answer = "¡Hola!", culture = new CultureInfo("es-mx") },
                new { key = "account.invalid.message", answer = "The user name or password provided is incorrect.", culture = new CultureInfo("en-us") },
                new { key = "account.invalid.message", answer = "El nombre de usuario o la contraseña facilitada no es la correcta.", culture = new CultureInfo("es-mx") },
                new { key = "account.invalid.message", answer = "The user name or password provided is incorrect.", culture = new CultureInfo("de-de") },
                new { key = "account.invalid.username", answer = "The username provided is incorrect.", culture = new CultureInfo("en-us") },
                new { key = "account.invalid.username", answer = "Die zur Verfügung gestellten Benutzernamen ist falsch.", culture = new CultureInfo("de-de") },
                new { key = "account.invalid.username", answer = "El nombre de usuario proporcionado es incorrecta.", culture = new CultureInfo("es-mx") }
            };
            foreach (var entry in cases)
            {
                Assert.AreEqual(entry.answer, translator.Localize(entry.key, entry.culture));
            }
        }

        [Test]
        public void TestFallbackLogic()
        {
            var cases = new[]
            {
                new { key = "hello.test.message", answer = "This is a testie test", culture = new CultureInfo("en-US") },
                new { key = "this.key.doesnt.exist", answer = "this.key.doesnt.exist", culture = new CultureInfo("en-US") },
            };
            foreach (var entry in cases)
            {
                Assert.AreEqual(entry.answer, translator.Localize(entry.key, entry.culture));
            }
        }

        [Test]
        public void ItTraversesLocaleNestedObjectsTransparently()
        {
            Assert.AreEqual("value", translator.Localize("nested.key", null));
        }

        [Test]
        public void ItDoesntBlowUpWithNullParameters()
        {
            var cases = new[]
            {
                new { key = "hello.test.message", answer = "This is a testie test" },
                new { key = "this.key.doesnt.exist", answer = "this.key.doesnt.exist" },
                new { key = (string)null, answer = "" },
            };
            foreach (var entry in cases)
            {
                Assert.AreEqual(entry.answer, translator.Localize(entry.key, null));
            }
        }

        [Test]
        public void ItThrowsWithInvalidCultureIdentifier()
        {
            Assert.Throws<CultureNotFoundException>(() => translator.Localize("this.key.doesnt.exist", new CultureInfo("INVALIDCULTURE")));
        }

        [Test]
        public void ItDoesntThrowWithTranslationsForCulture()
        {
            Assert.DoesNotThrow(() => translator.Localize("this.key.doesnt.exist", new CultureInfo("ja")));
            Assert.DoesNotThrow(() => translator.Localize("hello.test.message", new CultureInfo("ja")));
        }

        [Test]
        public void TestInterpolationTranslation()
        {
            var entry = new { key = "loggedin.user.day", answer = "¡Hola Cid! Hoy es Martes", culture = new CultureInfo("es-MX") };
            var tokens = new { name = "Cid", day = "Martes" };
            Assert.AreEqual(entry.answer, translator.Localize(entry.key, entry.culture, tokens));
        }

        [Test]
        public void TestInterpolationTranslation_DE()
        {
            var entry = new { key = "loggedin.user.day", answer = "Hallo, Cid. Heute ist Dienstag", culture = new CultureInfo("de-DE") };
            var tokens = new { name = "Cid", day = "Dienstag" };
            Assert.AreEqual(entry.answer, translator.Localize(entry.key, entry.culture, tokens));
        }

        [Test]
        public void TestInterpolationTranslation_EN()
        {
            var entry = new { key = "loggedin.user.day", answer = "Hello Cid! Today is Tuesday", culture = new CultureInfo("en-US") };
            var tokens = new { name = "Cid", day = "Tuesday" };
            Assert.AreEqual(entry.answer, translator.Localize(entry.key, entry.culture, tokens));
        }

        [Test]
        public void FalseTestInterpolationTranslation()
        {
            var entry = new { key = "loggedin.user.day", answer = "Hello Cid! Today is Tuesday", culture = new CultureInfo("en-US") };
            var tokens = new { num = "Cid", d = "Tuesday" };
            Assert.AreNotEqual(entry.answer, translator.Localize(entry.key, entry.culture, tokens));
        }

        [Test]
        public void FalseTestInterpolationTranslation_EmptyDictionary()
        {
            var entry = new { key = "loggedin.user.day", answer = "Hallo, Cid. Heute ist Dienstag", culture = new CultureInfo("de-DE") };
            var tokens = new {};
            Assert.AreNotEqual(entry.answer, translator.Localize(entry.key, entry.culture, tokens));
        }

        [Test]
        public void TestDateTranslation()
        {
            var cases = new[]
            {
                new { answer = "20.10.2014", culture = new CultureInfo("de-DE") },
                new { answer = "10/20/2014", culture = new CultureInfo("en-US") },
                new { answer = "20/10/2014", culture = new CultureInfo("es-MX") },
            };

            foreach (var entry in cases)
            {
                var date = new DateTime(2014, 10, 20);
                Assert.AreEqual(entry.answer, translator.Localize(date, entry.culture));
            }

        }

        [Test]
        public void TestPluralTranslation_All()
        {
            var cases = new[]
            {
                new { key = "loggedin.user.inbox", answer = "1 new message", plurality = 1, culture = new CultureInfo("en-US") },
                new { key = "loggedin.user.inbox", answer = "2 new messages", plurality = 2, culture = new CultureInfo("en-US") },
                new { key = "loggedin.user.inbox", answer = "1 neue Nachricht", plurality = 1, culture = new CultureInfo("de-DE") },
                new { key = "loggedin.user.inbox", answer = "2 neue Nachrichten", plurality = 2, culture = new CultureInfo("de-DE") },
                new { key = "loggedin.user.inbox", answer = "1 nuevo mensaje", plurality = 1, culture = new CultureInfo("es-MX") },
            };
            foreach (var entry in cases)
            {
                Assert.AreEqual(entry.answer, translator.Localize(entry.key, (Plurality)entry.plurality, entry.culture));
            }

        }


        [Test]
        public void TestPluralTranslation_EN()
        {
            var entry = new { key = "loggedin.user.inbox", answer = "55 new messages", plurality = Plurality.OTHER, culture = new CultureInfo("en-US") };
            var tokens = new { amount = "55" };
            Assert.AreEqual(entry.answer, translator.Localize(entry.key, entry.plurality, entry.culture, tokens));
        }

        [Test]
        public void TestPluralTranslation_DE()
        {
            var entry = new { key = "loggedin.user.inbox", answer = "66 neue Nachrichtenismo", plurality = Plurality.OTHER, culture = new CultureInfo("de-DE") };
            var tokens = new { amount = "66" };
            Assert.AreEqual(entry.answer, translator.Localize(entry.key, entry.plurality, entry.culture, tokens));
        }

        [Test]
        public void TestPluralTranslation_ES()
        {
            var entry = new { key = "loggedin.user.inbox", answer = "99 nuevo mensajismos", plurality = Plurality.OTHER, culture = new CultureInfo("es-MX") };
            var tokens = new { amount = "99" };
            Assert.AreEqual(entry.answer, translator.Localize(entry.key, entry.plurality, entry.culture, tokens));
        }

        [Test]
        public void TestHelloKeyExpectedToFailTranslation()
        {
            var cases = new[]
            {
                new { key = "", answer = "Test Data", culture = new CultureInfo("en-US") },
                new { key = "", answer = "Test Data", culture = new CultureInfo("es-MX") },
                new { key = "", answer = "Test Data", culture = new CultureInfo("de-DE") },
                new { key = "account.invalid.message", answer = "Test Data", culture = new CultureInfo("en-US") },
                new { key = "account.invalid.message", answer = "Test Data", culture = new CultureInfo("es-MX") },
                new { key = "account.invalid.message", answer = "Test Data", culture = new CultureInfo("de-DE") },
            };
            foreach (var entry in cases)
            {
                Assert.AreNotEqual(entry.answer, translator.Localize(entry.key, entry.culture));
            }
        }


        [Test]
        public void TestCurrencyValuesEn()
        {
            var cases = new[]
            {
                new { key = 0.0, answer = "$0.00" },
                new { key = 10.0, answer = "$10.00" },
                new { key = 100.0, answer = "$100.00" },
                new { key = 10000000000.0, answer = "$10,000,000,000.00" },
            };
            foreach (var entry in cases)
            {
                Assert.AreEqual(entry.answer, translator.Localize(entry.key, new CultureInfo("en-US")));
            }
        }


        [Test]
        public void TestCurrencyanswersEs()
        {
            var cases = new[]
            {
                new { key = 0.0, answer = "$0.00" },
                new { key = 10.0, answer = "$10.00" },
                new { key = 100.0, answer = "$100.00" },
                new { key = 10000000000.0, answer = "$10,000,000,000.00" },
            };
            foreach (var entry in cases)
            {
                Assert.AreEqual(entry.answer, translator.Localize(entry.key, new CultureInfo("es-MX")));
            }
        }
        [Test]
        public void TestCurrencyanswersDe()
        {
            var cases = new[]
            {
                new { key = 0.0, answer = "0,00 €" },
                new { key = 10.0, answer = "10,00 €" },
                new { key = 100.0, answer = "100,00 €" },
                new { key = 10000000000.0, answer = "10.000.000.000,00 €" },
            };
            foreach (var entry in cases)
            {
                Assert.AreEqual(entry.answer, translator.Localize(entry.key, new CultureInfo("de-DE")));
            }
        }
    }
}

