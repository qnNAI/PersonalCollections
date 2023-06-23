using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Contracts.Services;
using Application.Models.Email;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Infrastructure.Services.Email {

    public class EmailService : IEmailService {
        private readonly EmailConfiguration _emailConfig;
        private readonly IConfiguration _configuration;

        public EmailService(EmailConfiguration emailConfig, IConfiguration configuration) {
            _emailConfig = emailConfig;
            _configuration = configuration;
        }

        public async Task SendEmailAsync(Message message) {
            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message) {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_emailConfig.FromUsername, _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage) {
            using var client = new SmtpClient();

            try {
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfig.UserName, _configuration["EmailPassword"]);
                await client.SendAsync(mailMessage);
            } catch {
                //log an error message or throw an exception or both.
                throw;
            } finally {
                client.Disconnect(true);
                client.Dispose();
            }
            
        }
    }
}
