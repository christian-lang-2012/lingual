using System;
using NUnit.Framework;
using NUnit;
using Lingual.TranslationUtilities;
using Lingual.Enums;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;


namespace Lingual.Tests
{
    [TestFixture()]
    public class UTest
    {
        [Test()]
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
            TranslationUtility T = TranslationUtility.Instance;
            Assert.AreEqual(answer, T.Translate(key, locale));
        }

        [Test()]
        [TestCase("loggedin.user.day", "Hello Cid! Today is Tuesday", LocaleEnum.EN, "Cid", "Tuesday")]
        [TestCase("loggedin.user.day", "Hallo, Cid. Heute ist Dienstag", LocaleEnum.DE, "Cid", "Dienstag")]
        [TestCase("loggedin.user.day", "¡Hola Cid! Hoy es Martes", LocaleEnum.ES, "Cid", "Martes")]
        public void TestInterpolationTranslation(String key, String answer, LocaleEnum locale, params string[] args)
        {
            TranslationUtility T = TranslationUtility.Instance;
            Assert.AreEqual(answer, T.Translate(key, locale, args));
        }

        [Test()]
        [TestCase("20.10.2014", LocaleEnum.DE)]
        [TestCase("10/20/2014", LocaleEnum.EN)]
        [TestCase("20/10/2014", LocaleEnum.ES)]
        public void TestDateTranslation (String answer, LocaleEnum locale)
        {
            TranslationUtility T = TranslationUtility.Instance;
            var date = new DateTime(2014, 10, 20);
            Assert.AreEqual(answer, T.TranslateDate(date, locale));
        }

        [Test()]
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
            TranslationUtility T = TranslationUtility.Instance;
            Assert.AreEqual(answer, T.TranslatePlural(key, pluralDegree, locale, args));
        }
    }
}
