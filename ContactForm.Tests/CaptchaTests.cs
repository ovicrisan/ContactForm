using ContactForm.Models;
using ContactForm.Tests.Builders;
using Xunit;

namespace ContactForm.Tests
{
    public class CaptchaTests
    {
        ContactModel contact = ContactModelBuilder.GetContactCaptcha();
        ContactFormService contactService = new ContactFormService();

        [Fact]
        public void Email_Disabled_Post_Disabled_Captcha()
        {
            var contact = ContactModelBuilder.GetContactNoCaptcha();
            var contactSettings = new ContactSettings { 
                EmailSettings = EmailSettingsBuilder.GetDisabled(),
                PostSettings = PostSettingsBuilder.GetDisabled(),
                RecaptchaSettings = RecaptchaSettingsBuilder.GetEnabled()
            };

            var result = contactService.Submit(contact, contactSettings);

            Assert.True(result.Success);
        }

        [Fact]
        public void Email_Disabled_Post_Disabled_Captcha_Bad()
        {
            var contact = ContactModelBuilder.GetContactNoCaptcha();
            var contactSettings = new ContactSettings
            {
                EmailSettings = EmailSettingsBuilder.GetDisabled(),
                PostSettings = PostSettingsBuilder.GetDisabled(),
                RecaptchaSettings = RecaptchaSettingsBuilder.GetEnabledBad()
            };

            var result = contactService.Submit(contact, contactSettings);

            Assert.False(result.Success);
            Assert.Equal(ServiceResultType.Error, result.RecaptchaResult.ServiceResultType);
        }

        [Fact]
        public void Email_Disabled_Post_Enabled_Form_Captcha()
        {
            var contactSettings = new ContactSettings
            {
                EmailSettings = EmailSettingsBuilder.GetDisabled(),
                PostSettings = PostSettingsBuilder.GetEnabled(PostEncType.Form, -1),
                RecaptchaSettings = RecaptchaSettingsBuilder.GetEnabled()
            };

            var result = contactService.Submit(contact, contactSettings);

            Assert.Equal(ServiceResultType.Success, result.PostResult.ServiceResultType);
            Assert.Equal(ServiceResultType.Success, result.RecaptchaResult.ServiceResultType);
        }

        [Fact]
        public void Email_Disabled_Post_Enabled_Json_Captcha()
        {
            var contactSettings = new ContactSettings
            {
                EmailSettings = EmailSettingsBuilder.GetDisabled(),
                PostSettings = PostSettingsBuilder.GetEnabled(PostEncType.JSON, 0),
                RecaptchaSettings = RecaptchaSettingsBuilder.GetEnabled()
            };

            var result = contactService.Submit(contact, contactSettings);

            Assert.Equal(ServiceResultType.Success, result.PostResult.ServiceResultType);
            Assert.Equal(ServiceResultType.Success, result.RecaptchaResult.ServiceResultType);
        }
    }
}
