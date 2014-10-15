using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Lingual.Enums;

namespace Lingual.Handlers
{
    public class LocaleFileHandler
    {
        private static readonly String ProjPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        public static JObject GetLocaleFile(LocaleEnum localeCode)
        {
            var localePath = Path.Combine(ProjPath, "locale", localeCode + ".json");
            var jsonLocaleFile = JObject.Parse(File.ReadAllText(localePath));
            return jsonLocaleFile;
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
