using ContactForm.Models;

namespace ContactForm.Tests.Builders
{
    static public class EmailSettingsBuilder
    {
        static public EmailSettings GetDisabled()
        {
            return new EmailSettings { Enabled = false };
        }
    }
}
