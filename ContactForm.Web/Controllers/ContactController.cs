using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ContactForm.Web.Models;
using ContactForm;
using ContactForm.Models;
using Microsoft.Extensions.Options;
using System.Text;
using System.Net.Http.Headers;

namespace ContactForm.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly ContactFormService contactFormService;
        private readonly ContactSettings contactSettings;

        public ContactController(ContactFormService contactFormService, IOptions<ContactSettings> contactSettings)
        {
            this.contactFormService = contactFormService;
            this.contactSettings = contactSettings.Value;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            return Content("(Error: POST here, as JSON or form encoded, for details see https://github.com/OviCrisan/ContactForm )");
        }

        [HttpPost("/")]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult IndexForm(ContactModel contact)
        {
            if (contactSettings.RecaptchaSettings != null
                && contactSettings.RecaptchaSettings.Enabled
                && !string.IsNullOrEmpty(contactSettings.RecaptchaSettings.RecaptchaKey))
            {
                contactSettings.RecaptchaSettings.RecaptchaResponse = contact.RecaptchaResponse = HttpContext.Request.Form["g-recaptcha-response"].ToString();
            }

            var result = contactFormService.Submit(contact, contactSettings);
            StringBuilder sbResult = new StringBuilder();
            sbResult.AppendLine(result.Success ? "Successfully executed: <br />" : "Execution result: <br />");
            if (result.EmailResult != null && result.EmailResult.ServiceResultType != ServiceResultType.None)
                sbResult.AppendFormat("{0} <br/>", result.EmailResult.Message);
            if (result.PostResult != null && result.PostResult.ServiceResultType != ServiceResultType.None)
                sbResult.AppendFormat("{0} <br/>", result.PostResult.Message);
            if (result.RecaptchaResult != null && result.RecaptchaResult.ServiceResultType != ServiceResultType.Success)
                sbResult.AppendFormat("Invalid reCAPTCHA: {0} <br/>", result.RecaptchaResult.Message);

            if (!string.IsNullOrEmpty(contactSettings.PostSettings.RedirectURL) && contactSettings.PostSettings.RedirectSeconds >= -1)
            {
                var redirectURL = string.Format(contactSettings.PostSettings.RedirectURL, result.Success ? "1" : "0");
                if (contactSettings.PostSettings.RedirectSeconds == -1)
                {
                    // Redirect immediately
                    return new RedirectResult(redirectURL);
                }
                else
                if (contactSettings.PostSettings.RedirectSeconds >= 0)
                {
                    // No redirect but show 'Click to continue'
                    sbResult.AppendFormat("<a href='{0}'>{1}</a>", redirectURL, contactSettings.PostSettings.RedirectText);

                    if (contactSettings.PostSettings.RedirectSeconds > 0)
                    {
                        // JS redirect after RedirectSeconds
                        sbResult.AppendFormat("\r\n<script type='text/javascript'>setTimeout(function() {{document.location.href='{0}'}}, {1})</script>",
                            redirectURL, contactSettings.PostSettings.RedirectSeconds * 1000);
                    }
                }
            }

            return new ContentResult { Content = sbResult.ToString(), ContentType = "text/html" };
        }

        [HttpPost("/")]
        [Consumes("application/json")]
        public IActionResult IndexJson([FromBody]ContactModel contact)
        {
            if (!string.IsNullOrEmpty(contact.RecaptchaResponse))
                contactSettings.RecaptchaSettings.RecaptchaResponse = contact.RecaptchaResponse;

            var result = contactFormService.Submit(contact, contactSettings);
            return Ok(result);
        }

        [Route("/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
