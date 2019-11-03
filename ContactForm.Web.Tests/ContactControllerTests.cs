using ContactForm.Models;
using ContactForm.Web.Controllers;
using ContactForm.Web.Tests.Builders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using Xunit;

namespace ContactForm.Web.Tests
{
    public class ContactControllerTests
    {
        ContactModel contact = ContactModelBuilder.GetContactCaptcha();
        ContactFormService contactService = new ContactFormService();

        [Fact]
        public void Email_Disabled_Post_Disabled_Json_Captcha()
        {
            var contactSettings = new ContactSettings
            {
                EmailSettings = EmailSettingsBuilder.GetDisabled(),
                PostSettings = PostSettingsBuilder.GetDisabled(),
                RecaptchaSettings = RecaptchaSettingsBuilder.GetEnabled()
            };
            var options = Options.Create(contactSettings);
            var contactController = new ContactController(contactService, options);

            var result = contactController.IndexJson(contact) as OkObjectResult;
            var contactResult = result.Value as ContactResult;

            Assert.True(contactResult.Success);
        }

        [Fact]
        public void Email_Disabled_Post_Disabled_Json_Captcha_Bad()
        {
            var contactSettings = new ContactSettings
            {
                EmailSettings = EmailSettingsBuilder.GetDisabled(),
                PostSettings = PostSettingsBuilder.GetDisabled(),
                RecaptchaSettings = RecaptchaSettingsBuilder.GetEnabledBad()
            };
            var options = Options.Create(contactSettings);
            var contactController = new ContactController(contactService, options);

            var result = contactController.IndexJson(contact) as OkObjectResult;
            var contactResult = result.Value as ContactResult;

            Assert.False(contactResult.Success);
            Assert.Equal(ServiceResultType.Error, contactResult.RecaptchaResult.ServiceResultType);
        }

        [Fact]
        public void Email_Disabled_Post_Enabled_Json_Captcha()
        {
            var contactSettings = new ContactSettings
            {
                EmailSettings = EmailSettingsBuilder.GetDisabled(),
                PostSettings = PostSettingsBuilder.GetEnabled(PostEncType.JSON, -1),
                RecaptchaSettings = RecaptchaSettingsBuilder.GetEnabled()
            };
            var options = Options.Create(contactSettings);
            var contactController = new ContactController(contactService, options);

            var result = contactController.IndexJson(contact) as OkObjectResult;
            var contactResult = result.Value as ContactResult;

            Assert.Equal(200, result.StatusCode);
            Assert.True(contactResult.Success);
            Assert.Equal(ServiceResultType.Success, contactResult.PostResult.ServiceResultType);
        }

        [Fact]
        public void Email_Disabled_Post_Enabled_Post_Captcha()
        {
            var contactSettings = new ContactSettings
            {
                EmailSettings = EmailSettingsBuilder.GetDisabled(),
                PostSettings = PostSettingsBuilder.GetEnabled(PostEncType.JSON, 0),
                RecaptchaSettings = RecaptchaSettingsBuilder.GetEnabled()
            };
            var options = Options.Create(contactSettings);
            var contactController = new ContactController(contactService, options);

            var form = new Dictionary<string, StringValues>() { { "g-recaptcha-response", "x" } };
            contactController.ControllerContext.HttpContext = new DefaultHttpContext();
            contactController.Request.Form = new FormCollection(form);

            var result = contactController.IndexForm(contact) as ContentResult;

            Assert.Equal(200, result.StatusCode);
            Assert.StartsWith("Successfully", result.Content);
        }
    }
}
