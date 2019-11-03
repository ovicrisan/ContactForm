using ContactForm.Models;
using ContactForm.Tests.Builders;
using Xunit;

namespace ContactForm.Tests
{
    public class NoCaptchaTests
    {
        ContactModel contact = ContactModelBuilder.GetContactNoCaptcha();
        ContactFormService contactService = new ContactFormService();

        [Fact]
        public void Email_Disabled_Post_Disabled_No_Captcha()
        {
            var contact = ContactModelBuilder.GetContactNoCaptcha();
            var contactSettings = new ContactSettings { 
                EmailSettings = EmailSettingsBuilder.GetDisabled(),
                PostSettings = PostSettingsBuilder.GetDisabled(),
                RecaptchaSettings = RecaptchaSettingsBuilder.GetDisabled()
            };

            var result = contactService.Submit(contact, contactSettings);

            Assert.True(result.Success);
        }

        [Fact]
        public void Email_Disabled_Post_Enabled_Form_No_Captcha()
        {
            var contactSettings = new ContactSettings
            {
                EmailSettings = EmailSettingsBuilder.GetDisabled(),
                PostSettings = PostSettingsBuilder.GetEnabled(PostEncType.Form, -1),
                RecaptchaSettings = RecaptchaSettingsBuilder.GetDisabled()
            };

            var result = contactService.Submit(contact, contactSettings);

            Assert.Equal(ServiceResultType.Success, result.PostResult.ServiceResultType);
        }

        [Fact]
        public void Email_Disabled_Post_Enabled_Json_No_Captcha()
        {
            var contactSettings = new ContactSettings
            {
                EmailSettings = EmailSettingsBuilder.GetDisabled(),
                PostSettings = PostSettingsBuilder.GetEnabled(PostEncType.JSON, 0),
                RecaptchaSettings = RecaptchaSettingsBuilder.GetDisabled()
            };

            var result = contactService.Submit(contact, contactSettings);

            Assert.Equal(ServiceResultType.Success, result.PostResult.ServiceResultType);
        }
    }
}
