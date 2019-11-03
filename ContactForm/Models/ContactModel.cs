//using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ContactForm
{
    public class ContactModel
    {
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Subject { get; set; }
        public string Category { get; set; }
        public string Message { get; set; }
        //[JsonPropertyName("g-recaptcha-response")]
        [JsonProperty(PropertyName = "g-recaptcha-response")]
        public string RecaptchaResponse { get; set; }
    }
}
