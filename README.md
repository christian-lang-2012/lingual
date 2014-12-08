Lingual
=======

Lingual is an i18n platform built by Nuvi.

Lingual is a message-key i18n platform.
The storage system is individual JSON files for each language-culture name.
Each file should be named as such for language-culture files `en-us.json` and as `en.us` for just language.

The main class in Lingual is the `Translator` class. The `Translator`'s constructor needs a file path to the `locale` folder.
The `Translator` class holds a Dictionary of `LocaleTranslations` keyed by their Locales.
LocaleTranslations are a dictionary of key-value pairs. The key is the literal key that you pass in and the value is the translation.

There is no need to supply locales because each one is located in the Locale enum.
The locales in the `Locale` are all based off of the language-culture codes found here: http://msdn.microsoft.com/en-us/library/ee825488%28v=cs.20%29.aspx

If there is no `locale.json` file found on start up, then a `NullLocaleTranslations` gets put in it's place.
Essentially what `NullLocaleTranslations` does is it mimics a the LocaleTranslations, but instead of sending back actual data it sends back `null`.

