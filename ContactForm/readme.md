# ContactForm

**Contact form processing library which may send emails and/or POST data to webhook or REST API.**

[![Build status](https://ovicrisan.visualstudio.com/ContactForm/_apis/build/status/ContactForm-NuGet-CI)](https://ovicrisan.visualstudio.com/ContactForm/_build/latest?definitionId=2)

Declaration of the main function that does the work:

`public ContactResult Submit(ContactModel contact, ContactSettings contactSettings)`

Where
* `ContactResult` has a `Success` property calculated from `EmailResult` and `PostResult` properties;
* `ContactModel` has the actual data posted to email notification or webhook / REST API;
* `ContactSettings` has all configuration settings for email notification and HTTP posting, from the calling code (see `ContactForm.Web` project in this solution).

Then it's called simply, from Web App / API, Azure Function or even AWS Lambda.



