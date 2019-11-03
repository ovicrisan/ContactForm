using ContactForm.Models;
using System;
using System.Net.Http;
//using System.Text.Json;
using Newtonsoft.Json;

namespace ContactForm.Services
{
    public class RecaptchaService
    {
        public ServiceResult Validate(RecaptchaSettings recaptchaSettings)
        {
            var result = new ServiceResult { ServiceResultType = ServiceResultType.None };
            var client = new HttpClient();

            try
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
                }
            }
            catch (Exception ex)
            {
                result.ServiceResultType = ServiceResultType.Error;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
