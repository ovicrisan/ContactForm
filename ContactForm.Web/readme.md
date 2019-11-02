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