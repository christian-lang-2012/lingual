using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Lingual.Enums;
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
        public static JObject GetLocaleFile(LocaleEnum localeCode)
        {
            var localePath = Path.Combine(ProjPath, "locale", localeCode + ".json");
            String contents;
            try
            {
                contents = File.ReadAllText(localePath, Encoding.UTF8);
            }
            catch (DirectoryNotFoundException)
            {
                throw new LocaleFolderNotFoundException();
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            var jsonLocaleFile = JObject.Parse(contents);
            return jsonLocaleFile;
        }
    }
}
