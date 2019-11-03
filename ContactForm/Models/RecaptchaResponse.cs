using System.Collections.Generic;
//using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ContactForm.Models
{
    public class RecaptchaResponse
    {
        public bool success { get; set; }
        //[JsonPropertyName("error-codes")]
        [JsonProperty(PropertyName = "error-codes")]
        public IList<string> error_codes { get; set; }   
    }
}
