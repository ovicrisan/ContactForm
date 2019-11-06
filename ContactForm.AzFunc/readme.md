# ContactForm.AzFunc - Azure Function

This is a sinple HTTP triggered Azure Function which calls the .NET standard library. It's restricted to OPTIONS and POST HTTP verbs and provide settings and logger access to the library.

In Azure you have to provide enviroment variables values, used for settings.

For local development and testing (using emulator) [read the docs](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs).