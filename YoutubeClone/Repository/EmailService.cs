using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;
using YoutubeClone.Interfaces;
using YoutubeClone.Models;

namespace YoutubeClone.Repository
{
    public class EmailService : IEmailService
    {
        private const string templatePath = @"EmailTemplate/{0}.html";
        private readonly ILogger<EmailService> _logger;

        public readonly SMTPConfigModel _smtpConfig;

        public EmailService(IOptions<SMTPConfigModel> smtpConfig, ILogger<EmailService> logger)
        {
            _smtpConfig = smtpConfig.Value;
            _logger = logger;
            LogSmtpConfig();
        }
        private void LogSmtpConfig()
        {
            _logger.LogInformation($"SMTP Configuration - Host: {_smtpConfig.Host}, Port: {_smtpConfig.Port}, Username: {_smtpConfig.Username}, enablessl:{_smtpConfig.EnableSSL}");
        }

        public async Task SendTestEmail(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Hello {{UserName}}, This is a test email from youtube", userEmailOptions.PlaceHolders);
            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("TestEmail"), userEmailOptions.PlaceHolders);

            await SendEmail(userEmailOptions);
        }

        private async Task SendEmail(UserEmailOptions userEmailOptions)
        {
            if (userEmailOptions.ToEmails == null || !userEmailOptions.ToEmails.Any())
            {
                
                throw new ArgumentNullException(nameof(userEmailOptions.ToEmails), "To email addresses cannot be null or empty.");
            }
            MailMessage mail = new MailMessage
            {
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
                IsBodyHtml = _smtpConfig.IsBodyHTML
            };

            foreach (var toEmail in userEmailOptions.ToEmails)
            {
                if (!string.IsNullOrEmpty(toEmail))
                {
                    mail.To.Add(toEmail);
                }
            }

            NetworkCredential networkCredential = new NetworkCredential(_smtpConfig.Username, _smtpConfig.Password);

            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpConfig.Host,
                Port = _smtpConfig.Port,
                EnableSsl = _smtpConfig.EnableSSL,
                UseDefaultCredentials = _smtpConfig.UseDefaultCredentials,
                Credentials = networkCredential,
            };

            mail.BodyEncoding = Encoding.Default;

            await smtpClient.SendMailAsync(mail);
        }

        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;
        }

        private string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs !=null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }

            return text;
        }
    }
}
