using ContactForm.Models;
using Microsoft.Extensions.Logging;
//using System.Text.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ContactForm.Services
{
    public class PostService
    {
        public ServiceResult Post(ContactModel contact, PostSettings postSettings, ILogger logger = null)
        {
            var result = new ServiceResult { ServiceResultType = ServiceResultType.None };
            var client = new HttpClient();
            HttpContent content;
            try
            {
                if (postSettings.EncType == PostEncType.Form)
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                    content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("ContactName", contact.ContactName),
                        new KeyValuePair<string, string>("Email", contact.Email),
                        new KeyValuePair<string, string>("Phone", contact.Phone),
                        new KeyValuePair<string, string>("Category", contact.Category),
                        new KeyValuePair<string, string>("Subject", contact.Subject),
                        new KeyValuePair<string, string>("Message", contact.Message)
                    });
                }
                else
                {
                    //string tmpContent = JsonSerializer.Serialize(contact);
                    string tmpContent = JsonConvert.SerializeObject(contact);
                    content = new StringContent(tmpContent, Encoding.UTF8, "application/json");
                    if (logger != null) logger.LogInformation(tmpContent);
                }
                var response = client.PostAsync(postSettings.PostURL, content).GetAwaiter().GetResult();
                //var contents = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                result.ServiceResultType = ServiceResultType.Success;
                result.Message = $"Posted to {postSettings.PostURL} - Status code: {response.StatusCode}";
            }
            catch(Exception ex)
            {
                result.ServiceResultType = ServiceResultType.Error;
                result.Message = ex.Message;
                if (logger != null) logger.LogInformation(ex, "PostService error");
            }
            return result;
        }
    }
}