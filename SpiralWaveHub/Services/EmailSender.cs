using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SpiralWaveHub.Settings;
using System.Net;
using System.Net.Mail;

namespace SpiralWaveHub.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly MailSettings _mailSettings;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmailSender(IOptions<MailSettings> mailSettings, IWebHostEnvironment webHostEnvironment)
        {
            _mailSettings = mailSettings.Value;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailMessage message = new()
            {
                From = new MailAddress(_mailSettings.Email!, _mailSettings.DisplayName),
                Body = htmlMessage,
                Subject = subject,
                IsBodyHtml = true
            };

            message.To.Add(_webHostEnvironment.IsDevelopment() ? "asker0096@gmail.com" : email);

            SmtpClient smtpClient = new(_mailSettings.Host)
            {
                Port = _mailSettings.Port,
                Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password),
                EnableSsl = true
            };

           
            
           
                // Your code to send email
                await smtpClient.SendMailAsync(message);
      
          

            smtpClient.Dispose();
        }
    }
}
