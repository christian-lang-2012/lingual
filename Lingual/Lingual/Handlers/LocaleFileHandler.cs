using System;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using Lingual.Exceptions;

namespace Lingual.Handlers
{
    public class LocaleFileHandler
    {
        private static readonly String ProjPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        /// <summary>
        /// Gets the translation file for the specified locale
        /// </summary>
        /// <param name="localeCode">Locale to get translation file for</param>
        /// <returns>JObject of KV pairs (JSON)</returns>
        public static JObject GetLocaleFile(Locale localeCode)
        {
            var localePath = Path.Combine(ProjPath, "locale", localeCode.ToString().Replace('_', '-') + ".json");
            string contents;
            try
            {
                contents = File.ReadAllText(localePath, Encoding.UTF8);
            }
            catch (DirectoryNotFoundException)

        /// <summary>
        /// Return bool indicating if the file exists for this locale.
        /// </summary>
        /// <param name="locale">Locale file to look for</param>
        /// <returns>Bool indicating if the file exists for this locale</returns>
        public static bool LocaleFileExists(Locale locale)
        {
            return File.Exists(FilePathForLocale(locale));
        }

        /// <summary>
        /// Return file path for locale file to load.
        /// </summary>
        /// <param name="locale">Locale file to compute a path for.</param>
        /// <returns>Path to file for locale</returns>
        public static string FilePathForLocale(Locale locale)
        {
            return Path.Combine(ProjPath, "locale", locale.ToString().Replace('_', '-') + ".json");
        }

        /// <summary>
        /// Convert a content string containing JSON into a JObject.
        /// </summary>
        /// <param name="localeFileContent"></param>
        /// <returns>A JObject parsed from the content string</returns>
        public static JObject ParseFileContents(string localeFileContent)
        {
            if (string.IsNullOrEmpty(localeFileContent))
            {
                localeFileContent = "{}";
            }

            JObject jsonLocaleTranslations = null;
            try
            {
                jsonLocaleTranslations = JObject.Parse(localeFileContent);
            }
            catch (Exception exception)
            {
                jsonLocaleTranslations = new JObject();
            }

            return jsonLocaleTranslations;
        }

    }
}
