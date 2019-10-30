namespace ContactForm.Models
{
    public class PostSettings
    {
        public string PostURL { get; set; }
        public PostEncType EncType { get; set; }

        public string RedirectURL { get; set; }
        public int RedirectSeconds { get; set; }
        public string RedirectText { get; set; }
        public bool Enabled { get; set; }
    }

    public enum PostEncType
    {
        Form,
        JSON
    }
}
