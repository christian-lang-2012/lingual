using System;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Lingual.Infrastructure
{
    public class LocaleFileLoader
    {
        private static string ProjectPath;

        public LocaleFileLoader(string projectPath)
        {
            ProjectPath = projectPath;
        }

        /// <summary>
        /// Gets the translation file for the specified locale
        /// </summary>
        /// <param name="locale">Locale to get translation file for</param>
        /// <returns>JObject of KV pairs (JSON)</returns>
        public JObject GetLocaleFile(Locale locale)
        {
            JObject jsonLocaleTranslations = null;
            string localeFileContent = string.Empty;

            if (LocaleFileExists(locale))
            {
                var localePath = FilePathForLocale(locale);
                localeFileContent = File.ReadAllText(localePath, Encoding.UTF8);
            }
            jsonLocaleTranslations = ParseFileContents(localeFileContent);

            return jsonLocaleTranslations;
        }

        /// <summary>
        /// Checks for the locale folder in the designated path 
        /// </summary>
        public void CheckLocaleFolderExists()
        {
            var localeDirectories = Directory.GetDirectories(ProjectPath, "Locale", SearchOption.TopDirectoryOnly);
            if (localeDirectories.Length != 0)
            {
                ProjectPath = localeDirectories[0];
            }
            else
            {
                throw new Exception("Locale directory not found in project");
            }
        }

        /// <summary>
        /// Return bool indicating if the file exists for this locale.
        /// </summary>
        /// <param name="locale">Locale file to look for</param>
        /// <returns>Bool indicating if the file exists for this locale</returns>
        public bool LocaleFileExists(Locale locale)
        {
            return File.Exists(FilePathForLocale(locale));
        }

        /// <summary>
        /// Return file path for locale file to load.
        /// </summary>
        /// <param name="locale">Locale file to compute a path for.</param>
        /// <returns>Path to file for locale</returns>
        public string FilePathForLocale(Locale locale)
        {
            return Path.Combine(ProjectPath, locale.ToString().Replace('_', '-') + ".json");
        }

        /// <summary>
        /// Convert a content string containing JSON into a JObject.
        /// </summary>
        /// <param name="localeFileContent"></param>
        /// <returns>A JObject parsed from the content string</returns>
        public JObject ParseFileContents(string localeFileContent)
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
            catch (Exception)
            {
                jsonLocaleTranslations = new JObject();
            }

            return jsonLocaleTranslations;
        }

    }
}
