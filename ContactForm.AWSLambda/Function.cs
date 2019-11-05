using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using ContactForm.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ContactForm.AWSLambda
{
    public class Function
    {
        public ContactResult FunctionHandler(ContactModel contact, ILambdaContext context)
        {
            var settings = GetSettings();
            settings.RecaptchaSettings.RecaptchaResponse = contact.RecaptchaResponse;

            var contactService = new ContactFormService(new LoggerAdapter(context.Logger));
            return contactService.Submit(contact, settings);
        }

        private ContactSettings GetSettings()
        {
            var settings = new ContactSettings();

            settings.RecaptchaSettings.RecaptchaKey = Environment.GetEnvironmentVariable("RecaptchaKey");
            settings.RecaptchaSettings.Enabled = Environment.GetEnvironmentVariable("RecaptchaEnabled") == "true";

            settings.PostSettings.PostURL = Environment.GetEnvironmentVariable("PostURL");
            settings.PostSettings.EncType = PostEncType.JSON;
            settings.PostSettings.Enabled = Environment.GetEnvironmentVariable("PostEnabled") == "true";

            settings.EmailSettings.MailServer = Environment.GetEnvironmentVariable("MailServer");
            settings.EmailSettings.Username = Environment.GetEnvironmentVariable("Username");
            settings.EmailSettings.Password = Environment.GetEnvironmentVariable("Password");
            settings.EmailSettings.MailSender = Environment.GetEnvironmentVariable("MailSender");
            settings.EmailSettings.MailSenderName = Environment.GetEnvironmentVariable("MailSenderName");
            settings.EmailSettings.MailReciever = Environment.GetEnvironmentVariable("MailReciever");
            settings.EmailSettings.MailPort = Convert.ToInt32(Environment.GetEnvironmentVariable("MailPort"));
            var sec = Environment.GetEnvironmentVariable("MailSecurity");
            settings.EmailSettings.MailSecurity = sec == "2" ? MailSecurity.TLS : (sec == "1" ? MailSecurity.SSL : MailSecurity.None);
            settings.EmailSettings.SubjectPrefix = Environment.GetEnvironmentVariable("SubjectPrefix");
            settings.EmailSettings.Enabled = Environment.GetEnvironmentVariable("MailEnabled") == "true";

            return settings;
        }
    }
}
