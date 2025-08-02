using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TechZone.DAL.Models;

namespace TechZone.BLL.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {

            using var mailMessage = new MailMessage()
            {
                From = new MailAddress(_emailSettings.From, _emailSettings.DisplayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);

            using var smtp = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
            {
                Credentials = new NetworkCredential(_emailSettings.From, _emailSettings.Password),
                EnableSsl = _emailSettings.EnableSSL
            };

            await smtp.SendMailAsync(mailMessage);
        }
    }
}
