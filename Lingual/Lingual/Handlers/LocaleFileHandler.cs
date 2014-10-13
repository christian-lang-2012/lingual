using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lingual.Handlers
{
    public class LocaleFileHandler
    {
        private static String projPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        public static JObject GetLocaleFile(LocaleEnum localeCode)
        {
            var localePath = Path.Combine(projPath, "locale", localeCode + ".json");
            var JSONLocaleFile = JObject.Parse(File.ReadAllText(localePath));
            return JSONLocaleFile;
        }

        /// <summary>
        /// Checks to see if the locale folder exists.
        /// Creates the directory if it does not.
        /// </summary>
        /// <param name="localeFolderPath"></param>
        private void LocaleFolderCheck(String localeFolderPath)
        {
            if (!Directory.Exists(localeFolderPath))
            {
                Directory.CreateDirectory(localeFolderPath);
            }
        }
    }
}
