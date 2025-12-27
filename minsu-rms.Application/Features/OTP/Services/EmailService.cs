using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordManagementSystem.Application.Features.OTP.DTO;
using RecordManagementSystem.Application.Features.OTP.Interfaces;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace RecordManagementSystem.Application.Features.OTP.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettingsDTO _settings;
        public EmailService(IOptions<EmailSettingsDTO> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, int message)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("MinSUTrails", _settings.SmtpUser));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.ToString()};

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.SmtpUser, _settings.SmtpPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

    }
}
