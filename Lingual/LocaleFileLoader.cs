using System;
using System.Collections.Generic;
using System.Globalization;
using Lingual.Infrastructure;
using System.IO;
using System.Linq;

namespace Lingual
{
	public interface ILocaleFileLoader
	{
		Dictionary<CultureInfo, ICultureTranslator> ParseCultureTranslators(string directoryPath);
	}
	
	public class LocaleFileLoader : ILocaleFileLoader
	{
		public Dictionary<CultureInfo, ICultureTranslator> ParseCultureTranslators(string directoryPath)
		{
			return directoryPath
                .Let(i =>  Path.Combine(Directory.GetCurrentDirectory(), i))
                .If(Directory.Exists)
                .TryLet(directory => Directory.GetFiles(directory)
                    .Where(fileName => Path.GetExtension(fileName) == ".json")
                    .Select(fileName => new 
                    {
                        translator = CultureTranslator.FromFile(fileName),
                        culture = CultureFromLocaleFile(fileName)
                    })
                    .Where(i => i.culture != null)
                    .ToDictionary(i => i.culture, i => i.translator),
                ex => Console.WriteLine("Error reading locale file in directory '{0}': {1}", directoryPath, ex.ToString()));
        }
        public CultureInfo CultureFromLocaleFile(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName)
                .Let(shortFileName => new CultureInfo(shortFileName));
        }
	}
}

