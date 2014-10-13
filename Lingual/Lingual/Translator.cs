using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Lingual
{
	interface Translator
	{
		string translate(string key);
		
		string translate(string key, string locale);

		string translate(string key, params string[] arguments);

		string translate(string key, string locale, params string[] arguments);


	}
}
