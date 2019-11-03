using ContactForm.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace ContactForm.AzFunc
{
    public static class Submit
    {
        [FunctionName("Submit")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "options", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            ContactModel contact;
            var isJson = req.ContentType.StartsWith("application/json");
            if (isJson)
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                contact = JsonConvert.DeserializeObject<ContactModel>(requestBody);
            }
            else
            {
                contact = new ContactModel
                {
                    ContactName = req.Form["ContactName"],
                    Email = req.Form["Email"],
                    Phone = req.Form["Phone"],
                    Subject = req.Form["Subject"],
                    Category = req.Form["Category"],
                    Message = req.Form["Message"],
                    RecaptchaResponse = req.Form["g-recaptcha-response"]
                };

            }

            if (contact == null)
                return new BadRequestObjectResult("Please pass a contact form data");
            else
            {
                // Get settings
                var config = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();
                var contactSettings = new ContactSettings();
                config.GetSection("ContactSettings").Bind(contactSettings);

                var contactService = new ContactFormService();
                contactSettings.RecaptchaSettings.RecaptchaResponse = contact.RecaptchaResponse;
                var result = contactService.Submit(contact, contactSettings);

                if(isJson)
                    return (ActionResult)new OkObjectResult(result);
                else
                {
                    // Return 'text/html'
                    if (!string.IsNullOrEmpty(contactSettings.PostSettings.RedirectURL) && contactSettings.PostSettings.RedirectSeconds == -1)
                        return new RedirectResult(string.Format(contactSettings.PostSettings.RedirectURL, result.Success ? "1" : "0"));
                    else
                        return new ContentResult { Content = contactService.GetResultHTML(result, contactSettings), ContentType = "text/html", StatusCode = 200 };
                }
            }
        }
    }
}
