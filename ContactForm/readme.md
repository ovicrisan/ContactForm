# ContactForm

**Contact form processing library which may send emails and/or POST data to webhook or REST API.**

[![Build status](https://OviCrisan.visualstudio.com/ContactForm/_apis/build/status/ContactForm-NuGet-CI)](https://ovicrisan.visualstudio.com/ContactForm/_build/latest?definitionId=2)

Declaration of the main function that does the work:

`public ContactResult Submit(ContactModel contact, ContactSettings contactSettings)`

Where
* `ContactResult` has a `Success` property calculated from `RecaptchaResult`, `EmailResult` and `PostResult` properties;
* `ContactModel` has the actual data posted to email notification or webhook / REST API;
* `ContactSettings` has all configuration settings for email notification and HTTP posting, from the calling code (see `ContactForm.Web` project in this solution).

Then it's called simply, from [Web App / API](../ContactForm.Web/readme.md), [Azure Function](../ContactForm.AzFunc/readme.md) and [AWS Lambda function](../ContactForm.AWSLambda/readme.md), see each separate project in the solution.
There is also a Docker public image for the web app / API project at [hub.docker.com/r/OviCrisan/contactformweb](https://hub.docker.com/r/ovicrisan/contactformweb/tags) (more about this in web app / API project readme.md)

Library uses older versions of `Newtonsoft.Json` (11.0.2) and `Microsoft.Extensions.Logging.Abstractions` (2.1.1) 
instead of latest ones or even newly introduced `System.Text.Json` because of the intended compatibility with AWS Lambda which uses version 2.1 of .NET Core.

Function returns a `ContactResult` object which comprises of 3 properties, for email, URL post and recaptcha services, each with separate results.

## Recaptcha

In order to use recaptcha (we use version 2 with visible checkbox) you need to register an account with Google and follow the steps from [Google.com/recaptcha/](https://www.google.com/recaptcha/) to create your public and private key. 
Public key is used in HTML page of the contact form, while secret key needs to be added to the settings in order to be verified from on the server side with the response from the contact form (generated automatically).

You can even use publickly available test keys ( [see this](https://developers.google.com/recaptcha/docs/faq#id-like-to-run-automated-tests-with-recaptcha.-what-should-i-do) )

* Site key: 6LeIxAcTAAAAAJcZVRqyHh71UMIEGNQ_MXjiZKhI
* Secret key: 6LeIxAcTAAAAAGG-vFI1TnRWxMZNFuojJ4WifJWe

I recommend to use your own keys for a full experience.

## Deployment as NuGet package

`ContactForm` library is available at [NuGet.org/packages/OviCrisan.ContactForm](https://www.nuget.org/packages/OviCrisan.ContactForm/)

You can add it to your projects with Packet Manager:

`Install-Package OviCrisan.ContactForm`

or with dotnet CLI:

`dotnet add package OviCrisan.ContactForm`

Deployment to NuGet.org is done automatically with [Azure DevOps pipeline CI/CD](azure-pipelines.yml) (see the badge above).

##Unit tests

The library project come with *some* unit tests in `ContactForm.Tests` project, with and without recaptcha settings.