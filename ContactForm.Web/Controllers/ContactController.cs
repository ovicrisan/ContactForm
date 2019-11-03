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
            if (!string.IsNullOrEmpty(contactSettings.PostSettings.RedirectURL) && contactSettings.PostSettings.RedirectSeconds == -1)
                return new RedirectResult(string.Format(contactSettings.PostSettings.RedirectURL, result.Success ? "1" : "0"));
            else
                return new ContentResult { Content = contactFormService.GetResultHTML(result, contactSettings), ContentType = "text/html", StatusCode = 200 };
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
