# ContactForm

This is a demo of a simple [contact form](https://github.com/ovicrisan/ContactForm/tree/master/ContactForm) processing library (.NET Standard) ([readme.md](ContactForm/readme.md)), deployed to [NuGet.org/packages/OviCrisan.ContactForm](https://www.nuget.org/packages/OviCrisan.ContactForm/), 
using Azure DevOps pipeline for CI/CD, then used in a [web app + API](https://github.com/ovicrisan/ContactForm/tree/master/ContactForm.Web) ([readme.md](ContactForm.Web/readme.md)) (also available as Docker image at [hub.docker.com/r/ovicrisan/contactformweb](https://hub.docker.com/r/ovicrisan/contactformweb/tags)), 
[Azure Function](https://github.com/ovicrisan/ContactForm/tree/master/ContactForm.AzFunc) ([readme.md](ContactForm.AzFunc/readme.md)) and [AWS Lambda function](https://github.com/ovicrisan/ContactForm/tree/master/ContactForm.AWSLambda) ([readme.md](ContactForm.AWSLambda/readme.md)).

The core functionality of sending notification emails and/or calling a webhook or REST API using HTTP POST is provided by 
[ContactForm](https://github.com/ovicrisan/ContactForm/tree/master/ContactForm) .NET Standard library, then used by all other projects.

![ContactForm diagram](https://ovicrisan.github.io/ContactForm/images/contactform1.png)

See a working example at [OviCrisan.github.io/ContactForm](https://ovicrisan.github.io/ContactForm/) using an Azure App Service deployment at [ContactFormWeb.AzureWebsites.net](https://contactformweb.azurewebsites.net/). 
Other deployments as Azure Function, Azure Container Instance and AWS Lambda function are not currently available, but can be deployed easily, as described in the docs of those projects.

**Challenges**

There were a few *challenges* with this project, like:

* Use .NET Standard 2.0 with older libraries for `Newtonsoft.Json` and `Microsoft.Extensions.Logging.Abstractions` so the library can be used with AWS Lambda, which is not yet using .NET Core 3.
* Web app and API use the same endpoint ('/'), depending on the `Content-Type` HTTP header. `Accept-Type` is not actually used so the rule is very simple: if passing content type `application/json` you will get back a JSON object response, if passing `application/x-www-form-urlencoded` you wil get back `text/html` possibly with a redirect status code or OK status code (200).
* Google reCAPTCHA is optional, but when used it checks version 2 with visible checkbox.
