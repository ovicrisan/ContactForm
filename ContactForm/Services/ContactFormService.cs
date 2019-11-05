using ContactForm.Models;
using ContactForm.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace ContactForm
{
    public class ContactFormService
    {
        public readonly ILogger logger;

        public ContactFormService()
        {
        }

        public ContactFormService(ILogger logger)
        {
            this.logger = logger;
        }

        public ContactResult Submit(ContactModel contact, ContactSettings contactSettings)
        {
            var result = new ContactResult();
            if (contact == null) throw new ArgumentNullException("Missing contact form data");
            if (contactSettings == null) throw new ArgumentNullException("Missing settings");

            if (logger != null) logger.LogInformation("Contact: {0} / {1} / {2} [Email: {3}, Post: {4}, Recaptcha: {5}]",
                contact.ContactName, contact.Email, contact.Phone, contactSettings.EmailSettings?.Enabled,
                contactSettings.PostSettings?.Enabled, contactSettings.RecaptchaSettings?.Enabled);

            if (contactSettings.RecaptchaSettings != null
                && contactSettings.RecaptchaSettings.Enabled
                && !string.IsNullOrEmpty(contactSettings.RecaptchaSettings.RecaptchaKey))
            {
                // Check Recaptcha
                var recaptchaService = new RecaptchaService();
                result.RecaptchaResult = recaptchaService.Validate(contactSettings.RecaptchaSettings, logger);
                if (result.RecaptchaResult.ServiceResultType != ServiceResultType.Success) return result;  // Stop processing immediately
            }

            if (contactSettings.EmailSettings != null && contactSettings.EmailSettings.Enabled)
            {
                var emailService = new EmailService();
                result.EmailResult = emailService.SendEmail(contact, contactSettings.EmailSettings, logger);
            }

            if (contactSettings.PostSettings != null && contactSettings.PostSettings.Enabled && !string.IsNullOrEmpty(contactSettings.PostSettings.PostURL))
            {
                var postService = new PostService();
                result.PostResult = postService.Post(contact, contactSettings.PostSettings, logger);
            }
            return result;
        }

        public string GetResultHTML(ContactResult result, ContactSettings contactSettings)
        {
            StringBuilder sbResult = new StringBuilder();
            sbResult.AppendLine(result.Success ? "Successfully executed: <br />" : "Execution result: <br />");
            if (result.EmailResult != null && result.EmailResult.ServiceResultType != ServiceResultType.None)
                sbResult.AppendFormat("{0} <br/>", result.EmailResult.Message);
            if (result.PostResult != null && result.PostResult.ServiceResultType != ServiceResultType.None)
                sbResult.AppendFormat("{0} <br/>", result.PostResult.Message);
            if (result.RecaptchaResult != null && result.RecaptchaResult.ServiceResultType != ServiceResultType.Success)
                sbResult.AppendFormat("Invalid reCAPTCHA: {0} <br/>", result.RecaptchaResult.Message);

            if (!string.IsNullOrEmpty(contactSettings.PostSettings.RedirectURL) && contactSettings.PostSettings.RedirectSeconds >= 0)
            {
                var redirectURL = string.Format(contactSettings.PostSettings.RedirectURL, result.Success ? "1" : "0");
                // Show 'Click to continue'
                sbResult.AppendFormat("<a href='{0}'>{1}</a>", redirectURL, contactSettings.PostSettings.RedirectText);

                if (contactSettings.PostSettings.RedirectSeconds > 0)
                {
                    // JS redirect after RedirectSeconds
                    sbResult.AppendFormat("\r\n<script type='text/javascript'>setTimeout(function() {{document.location.href='{0}'}}, {1})</script>",
                        redirectURL, contactSettings.PostSettings.RedirectSeconds * 1000);
                }
            }
            return sbResult.ToString();
        }
    }
}
