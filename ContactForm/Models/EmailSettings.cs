namespace ContactForm.Models
{
    public class EmailSettings
    {
        public string MailServer { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string MailSender { get; set; }
        public string MailSenderName { get; set; }
        public string MailReciever { get; set; }
        public int MailPort { get; set; }
        public MailSecurity MailSecurity { get; set; }
        public string SubjectPrefix { get; set; }
        public bool Enabled { get; set; }
    }

    public enum MailSecurity
    {
        None,
        SSL,
        TLS
    }
}
