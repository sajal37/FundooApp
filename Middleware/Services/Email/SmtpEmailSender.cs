using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using ModelLayer.Model;

namespace Middleware.Services.Email
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;

        public SmtpEmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmailAsync(MessageModel message)
        {
            if (string.IsNullOrWhiteSpace(message.Email) ||
                string.IsNullOrWhiteSpace(message.Subject) ||
                string.IsNullOrWhiteSpace(message.Body))
            {
                throw new ArgumentException("Email, subject, and body are required.");
            }

            var emailSettings = configuration.GetSection("EmailSettings");
            string? senderEmail = emailSettings["Email"];
            string? password = emailSettings["Password"];

            if (string.IsNullOrWhiteSpace(senderEmail) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException("Email settings are not configured.");
            }

            using MailMessage mail = new MailMessage();
            mail.From = new MailAddress(senderEmail);
            mail.To.Add(message.Email);
            mail.Subject = message.Subject;
            mail.Body = message.Body;
            mail.IsBodyHtml = true;

            using SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(senderEmail, password);

            try
            {
                await smtp.SendMailAsync(mail);
            }
            catch (SmtpException ex)
            {
                throw new HttpRequestException("The email service could not send the message.", ex);
            }
        }
    }
}
