# ContactForm.Web

**ASP.NET Core App and API to use ContactForm library**\

Manipulated via *appsettings.json* or environment variables:

```
"ContactSettings": {
  "EmailSettings": {
    "MailServer": "smtp.gmail.com",
    "Username": "***@gmail.com",
    "Password": "***",
    "MailSender": "***@gmail.com",
    "MailSenderName": "***",
    "MailReciever": "receiver@email.com",
    "MailPort": 587,
    "MailSecurity": 1,
    "SubjectPrefix": "Contact form notification: ",
    "Enabled": false
  },
  "PostSettings": {
    "PostURL": "https://postman-echo.com/post",
    "EncType": 1,
    "RedirectURL": "https://site.com/?success={0}",
    "RedirectSeconds": 10,
    "RedirectText":  "Click here to continue",
    "Enabled": false
  }
}
```

Some settings are self-explanatory, but others:

* Library differentiate between `Username` and `MailSender`, the former used to authenticate and the latter to send emails;
* `MailReciver` is the email address getting the notification when a new contact form is submitted;
* `MailSecurity` - 0 = no SSL/TLS, 1 = SSL / TLS enabled;
* `PostURL` - webhook or REST API URL getting the contact data via HTTP POST;
* `EncType` - 0 = form encoded, 1 = JSON
* `RedirectURL`, `RedirectSeconds` and `RedirectText` are used only for `EncType` = 0 (form posting), and have the URL where redirected after certain number of seconds, showing the link text specified;
* `RedirectSeconds` may have -1 for immediate redirect (HTTP status code 302), 0 for no automatic redirect but the user needs to click link to continue, or a positive value to be redirected automatically after that number of seconds;

For environment variable just follow MS rules to replace each level of indentation with double underscore ('__'), 
like:

`"ContactSettings__EmailSettings__Enabled" = true`

## Endpoint

The URL of deployed web application is actually the endpoint of both HTTP form POST (using `application/x-www-form-urlencoded` content type) and
REST API (using `application\json` content type). For the latter, just post the JSON formatted contact form data to the root of the application ('/').

When using HTTP GET you will see this simple message:

`(Error: POST here, as JSON or form encoded, for details see https://github.com/OviCrisan/ContactForm )`

## Azure deployment

The application is already deployed as Azure App Service (on a free plan) as [ContactFormWeb.AzureWebsites.net](https://contactformweb.azurewebsites.net/)  (captcha enabled).
Sample contact form at [OviCrisan.github.io/ContactForm](https://ovicrisan.github.io/ContactForm/) is using Azure App Service, both as HTTP form post and REST API with AJAX.

## Docker image deployment

`Dockerfile` used for building the Docker image is available in source code, with some default values for environment variables (disabling all options). 
The image is publicly available at [hub.docker.com/r/OviCrisan/ContactFormWeb](https://hub.docker.com/r/ovicrisan/contactformweb).

To use it locally just try this:

```
docker pull ovicrisan/contactformweb
docker run -p 8080:80 -e ContactSettings__PostSettings__Enabled=true  -e ContactSettings__PostSettings__PostURL=https://postman-echo.com/post -e ContactSettings__RecaptchaSettings__Enabled=true -e ContactSettings__RecaptchaSettings__RecaptchaKey=6LeIxAcTAAAAAG1G-vFI1TnRWxMZNFuojJ4WifJWe --rm --name contactform -d ovicrisan/contactformweb
```

Test it with your own form posting to http://localhost:8080 and then stop it with `docker stop contactform`

The application can be easily be deployed on Azure Container Instances or other cloud hosting.