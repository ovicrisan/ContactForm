using ContactForm.Models;
using ContactForm.Services;
using System;

namespace ContactForm
{
    public class ContactFormService
    {
        public ContactResult Submit(ContactModel contact, ContactSettings contactSettings)
        {
            var result = new ContactResult();
            if (contact == null) throw new ArgumentNullException("Missing contact form data");

            if(contactSettings.EmailSettings != null && contactSettings.EmailSettings.Enabled)
            {
                var emailService = new EmailService();
                result.EmailResult = emailService.SendEmail(contact, contactSettings.EmailSettings);
            }

            if (contactSettings.PostSettings != null && contactSettings.PostSettings.Enabled && !string.IsNullOrEmpty(contactSettings.PostSettings.PostURL))
            {
                var postService = new PostService();
                result.PostResult = postService.Post(contact, contactSettings.PostSettings);
            }
            return result;
        }
    }
}
