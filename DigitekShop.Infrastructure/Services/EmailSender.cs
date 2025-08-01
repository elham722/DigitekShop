using DigitekShop.Application.Interfaces.Infrastructure;
using DigitekShop.Infrastructure.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DigitekShop.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
        {
            _emailSettings = configuration.GetSection(EmailSettings.SectionName)
                .Get<EmailSettings>() ?? new EmailSettings();
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = false)
        {
            return await SendEmailAsync(new[] { to }, subject, body, isHtml);
        }

        public async Task<bool> SendEmailAsync(IEnumerable<string> recipients, string subject, string body, bool isHtml = false)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
                
                foreach (var recipient in recipients)
                {
                    if (!string.IsNullOrWhiteSpace(recipient))
                    {
                        message.To.Add(MailboxAddress.Parse(recipient));
                    }
                }

                if (!message.To.Any())
                {
                    _logger.LogWarning("No valid recipients found for email with subject: {Subject}", subject);
                    return false;
                }

                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                if (isHtml)
                {
                    bodyBuilder.HtmlBody = body;
                }
                else
                {
                    bodyBuilder.TextBody = body;
                }

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                
                // Connect to SMTP server
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, 
                    _emailSettings.EnableSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls);

                // Authenticate if credentials provided
                if (!string.IsNullOrEmpty(_emailSettings.SmtpUsername) && !string.IsNullOrEmpty(_emailSettings.SmtpPassword))
                {
                    await client.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
                }

                // Send the message
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Email sent successfully to {RecipientCount} recipients with subject: {Subject}", 
                    message.To.Count, subject);
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to send email with subject: {Subject}", subject);
                return false;
            }
        }

        public async Task<bool> SendEmailWithAttachmentAsync(string to, string subject, string body, 
            byte[] attachment, string attachmentName, bool isHtml = false)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
                message.To.Add(MailboxAddress.Parse(to));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                if (isHtml)
                {
                    bodyBuilder.HtmlBody = body;
                }
                else
                {
                    bodyBuilder.TextBody = body;
                }

                // Add attachment
                using var stream = new MemoryStream(attachment);
                bodyBuilder.Attachments.Add(attachmentName, stream.ToArray());

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, 
                    _emailSettings.EnableSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls);

                if (!string.IsNullOrEmpty(_emailSettings.SmtpUsername) && !string.IsNullOrEmpty(_emailSettings.SmtpPassword))
                {
                    await client.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
                }

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Email with attachment sent successfully to {Recipient} with subject: {Subject}", 
                    to, subject);
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to send email with attachment to {Recipient} with subject: {Subject}", 
                    to, subject);
                return false;
            }
        }
    }
}