using ContactForm.Models;

namespace ContactForm.Web.Tests.Builders
{
    static public class EmailSettingsBuilder
    {
        static public EmailSettings GetDisabled()
        {
            return new EmailSettings { Enabled = false };
        }
    }
}
