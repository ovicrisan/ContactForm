using ContactForm.Models;
using System;
using System.Net.Http;
//using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace ContactForm.Services
{
    public class RecaptchaService
    {
        public ServiceResult Validate(RecaptchaSettings recaptchaSettings, ILogger logger = null)
        {
            var result = new ServiceResult { ServiceResultType = ServiceResultType.None };
            var client = new HttpClient();

            try
            {
                if (string.IsNullOrEmpty(recaptchaSettings.RecaptchaResponse))
                {
                    result.ServiceResultType = ServiceResultType.Error;
                    result.Message = "Missing recaptcha response";
                }
                else
                {
                    var responseString = client.GetStringAsync($"https://www.recaptcha.net/recaptcha/api/siteverify?secret={recaptchaSettings.RecaptchaKey}&response={recaptchaSettings.RecaptchaResponse}").GetAwaiter().GetResult();
                    //var response = JsonSerializer.Deserialize<RecaptchaResponse>(responseString);
                    var response = JsonConvert.DeserializeObject<RecaptchaResponse>(responseString);
                    if (response.success)
                        result.ServiceResultType = ServiceResultType.Success;
                    else
                    {
                        result.ServiceResultType = ServiceResultType.Error;
                        result.Message = response.error_codes[0];
                        if(result.Message.EndsWith("secret"))
                            logger.LogInformation("Recaptcha failed: {0} / {1}", recaptchaSettings.RecaptchaKey?.Substring(0,10), recaptchaSettings.RecaptchaResponse?.Substring(0, 15));
                    }
                }
            }
            catch (Exception ex)
            {
                result.ServiceResultType = ServiceResultType.Error;
                result.Message = ex.Message;
                if (logger != null) logger.LogInformation(ex, "RecaptchaService error");
            }

            return result;
        }
    }
}
