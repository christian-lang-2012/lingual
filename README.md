lingual
======

Lingual is an i18n platform built by Nuvi.

Lingual is a message-key i18n platform. The storage system is individual JSON files for each language-culture name. Each file should be named as such for language-culture files en-us.json and as en.us for just language.

The main interface in Lingual is `ITranslator`, which `Translator` implements. The Translator's constructor either needs the culture -> cultureTranslator data structure, or a directory and an `ILocaleDirectoryLoader` instance.

Lingual implements basic fallbacks from culture-specific, to culture-general language, then to a "root" fallback language, specified at the Translator language..

How to Use
==========

Given that you have the directory structure
```
./locales/en.json
./locales/en-US.json
./locales/en-GB.json
./locales/es.json
./locales/es-MX.json
```

Then you instantiate like this
```
ITranslator translator = new Translator(new CultureInfo("en"), "locales", new LocaleDirectoryLoader());
```

And now you can

`translator.Localize("login.submit", Thread.CurrentThread.CurrentCulture);`