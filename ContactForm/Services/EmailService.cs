using ContactForm.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ContactForm.Services
{
    public class EmailService
    {
        public ServiceResult SendEmail(ContactModel contact, EmailSettings emailSettings)
        {
            var result = new ServiceResult { ServiceResultType = ServiceResultType.None };
            try
            {
                MailMessage mail = new MailMessage();
                mail.BodyEncoding = Encoding.UTF8;
                mail.From = new MailAddress(emailSettings.MailSender, emailSettings.MailSenderName);
                SmtpClient smtp = new SmtpClient();
                smtp.Port = emailSettings.MailPort;
                smtp.Host = emailSettings.MailServer;
                if (emailSettings.MailSecurity != MailSecurity.None)
                {
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);
                }
                mail.To.Add(new MailAddress(emailSettings.MailReciever));
                //mail.IsBodyHtml = true;
                mail.Body = $@"Contact name: {contact.ContactName}
Email: {contact.Email}
Phone: {contact.Phone}
Category: {contact.Category}

{contact.Message}
";
                mail.Subject = emailSettings.SubjectPrefix + contact.Subject;
                smtp.Send(mail);
                result.ServiceResultType = ServiceResultType.Success;
                result.Message = "Email sent successfully from " + contact.Email;
            }
            catch (Exception ex)
            {
                result.ServiceResultType = ServiceResultType.Error;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
