namespace ContactForm.Models
{
    public class ContactSettings
    {
        public EmailSettings EmailSettings { get; set; } = new EmailSettings();
        public PostSettings PostSettings { get; set; } = new PostSettings();
        public RecaptchaSettings RecaptchaSettings { get; set; } = new RecaptchaSettings();
    }
}
