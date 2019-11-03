using ContactForm.Models;

namespace ContactForm.Web.Tests.Builders
{
    static public class PostSettingsBuilder
    {
        static public PostSettings GetDisabled()
        {
            return new PostSettings { Enabled = false };
        }

        static public PostSettings GetEnabled(PostEncType postEncType, int redirSeconds)
        {
            return new PostSettings { 
                Enabled = true, 
                EncType = postEncType,
                PostURL = "https://postman-echo.com/post",
                RedirectSeconds = redirSeconds,
                RedirectURL = "https://ovicrisan.github.io/ContactForm/?success={0}",
                RedirectText = "Click here to continue"
            };
        }
    }
}
