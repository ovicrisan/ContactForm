namespace ContactForm.Web.Tests.Builders
{
    static public class ContactModelBuilder
    {
        static public ContactModel GetContactCaptcha()
        {
            return new ContactModel
            {
                ContactName = "test name",
                Category = "testing",
                Email = "x@x.x",
                Message = "test message",
                Phone = "+01 234 456",
                Subject = "test subject",
                RecaptchaResponse = "x"
            };
        }
    }
}
