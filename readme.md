# ContactForm

This is a demo of a simple [contact form](https://github.com/ovicrisan/ContactForm/tree/master/ContactForm) processing library (.NET Standard), deployed to [NuGet.org](https://www.nuget.org/packages/OviCrisan.ContactForm/), 
using Azure DevOps pipeline for CI/CD, then used in a [web app + API](https://github.com/ovicrisan/ContactForm/tree/master/ContactForm.Web), Azure Function and AWS Lambda.

The core functionality of sending notification emails and/or calling a webhook or REST API using HTTP POST is provided by 
[ContactForm](https://github.com/ovicrisan/ContactForm/tree/master/ContactForm) .NET Standard library, then used by all other projects.

For details on each separate project see their *readme.md* files.

See a working example using an [Azure App Service deployment](https://contactformweb.azurewebsites.net/) at [https://OviCrisan.github.io/ContactForm/](https://ovicrisan.github.io/ContactForm/).