namespace ContactForm.Models
{
    public class ServiceResult
    {
        public ServiceResultType ServiceResultType { get; set; }
        public string Message { get; set; }
    }

    public enum ServiceResultType
    {
        None,
        Success,
        Error
    }
}
