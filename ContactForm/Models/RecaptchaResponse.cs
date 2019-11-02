using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ContactForm.Models
{
    public class RecaptchaResponse
    {
        public bool success { get; set; }
        [JsonPropertyName("error-codes")]
        public IList<string> error_codes { get; set; }   
    }
}
