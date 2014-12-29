using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lingual.Infrastructure
{
    public interface ICultureTranslator
    {
        string Get(string key);
    }

    public class CultureTranslator : ICultureTranslator
    {
        public Dictionary<string,string> TranslationDictionary { get; set; }
        public string Get(string key)
        {
            return key
                .If(this.TranslationDictionary.ContainsKey)
                .Let(i => this.TranslationDictionary[i]);
        }

        public static ICultureTranslator FromFile(string filePath)
        {
            return filePath
                .IfDo(i => !File.Exists(i),
                    i => { throw new ArgumentException("File does not exist", "filePath"); })
                .TryLet(i => File.OpenRead(i),
                    ex => { throw new ArgumentException("File cannot be opened", ex); })
                .TryLet(stream => 
                {
                    using (stream)
                    using (var streamReader = new StreamReader(stream, Encoding.UTF8))
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        return JObject.Load(jsonReader);
                    }
                },
                    ex => { throw new ArgumentException("File cannot be parsed into json", "filePath", ex); })
                .Let(FromJson);
        }
        public static ICultureTranslator FromJson(JObject rootJson)
        {
            return new CultureTranslator
            {
                TranslationDictionary = TraverseRecursive(rootJson)
                    .ToDictionary(i => i.Item1, i => i.Item2)
            };
        }
        private static IEnumerable<Tuple<string,string>> TraverseRecursive(JObject json, string prefix = null)
        {
            return json.Properties()
                .SelectMany(prop => 
                {
                    var name = prefix == null 
                        ? prop.Name 
                        : string.Join(".", prefix, prop.Name);
                    if (prop.Value.Type == JTokenType.String)
                    {
                        return new[] { Tuple.Create(name, prop.Value.ToString()) };
                    }
                    if (prop.Value.Type == JTokenType.Object)
                    {
                        return TraverseRecursive((JObject)prop.Value, name);
                    }
                    return new Tuple<string,string>[] { };
                });
        }
    }
}


