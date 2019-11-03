using ContactForm.Models;

namespace ContactForm.Tests.Builders
{
    static public class RecaptchaSettingsBuilder
    {
        static public RecaptchaSettings GetDisabled()
        {
            return new RecaptchaSettings { Enabled = false };
        }

        static public RecaptchaSettings GetEnabled()
        {
            // https://developers.google.com/recaptcha/docs/faq#id-like-to-run-automated-tests-with-recaptcha.-what-should-i-do
            return new RecaptchaSettings { 
                Enabled = true,
                RecaptchaKey= "6LeIxAcTAAAAAGG-vFI1TnRWxMZNFuojJ4WifJWe"
            };
        }

        static public RecaptchaSettings GetEnabledBad()
        {
            // Wrong key
            return new RecaptchaSettings
            {
                Enabled = true,
                RecaptchaKey = "6Le"
            };
        }
    }
}
