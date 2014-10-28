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
            {
                throw new LocaleFolderNotFoundException();
            }
            catch (FileNotFoundException ex)
            {
                contents = "{ \"null\":\"null\" }";
            }

            if (contents == "")
            {
                contents = "{ \"null\":\"null\" }";                
            }

            var jsonLocaleFile = JObject.Parse(contents);
            return jsonLocaleFile;
        }
    }
}
