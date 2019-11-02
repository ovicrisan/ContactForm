namespace ContactForm.Models
{
    public class ContactResult
    {
        public bool Success
        {
            get
            {
                if (RecaptchaResult != null && RecaptchaResult.ServiceResultType != ServiceResultType.Success)
                    return false;

                if (EmailResult == null)
                    if (PostResult == null)
                        return true;
                    else
                        return PostResult.ServiceResultType == ServiceResultType.Success;
                else
                        if (PostResult == null)
                            return EmailResult.ServiceResultType == ServiceResultType.Success;
                        else
                            return EmailResult.ServiceResultType == ServiceResultType.Success && PostResult.ServiceResultType == ServiceResultType.Success;
            }
        }

        public ServiceResult EmailResult { get; set; }
        public ServiceResult PostResult { get; set; }
        public ServiceResult RecaptchaResult { get; set; } 
    }
}
