using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitekShop.Application.Interfaces.Infrastructure;
using Microsoft.Extensions.Logging;

namespace DigitekShop.Infrastructure.Email
{
    /// <summary>
    /// Mock implementation of IEmailSender for testing purposes.
    /// This implementation logs email information instead of actually sending emails.
    /// </summary>
    public class MockEmailSender : IEmailSender
    {
        private readonly ILogger<MockEmailSender> _logger;
        
        // Store sent emails for verification in tests
        public List<EmailMessage> SentEmails { get; } = new List<EmailMessage>();

        public MockEmailSender(ILogger<MockEmailSender> logger = null)
        {
            _logger = logger;
        }

        public Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = false)
        {
            var emailMessage = new EmailMessage
            {
                To = to,
                Subject = subject,
                Body = body,
                IsHtml = isHtml,
                SentAt = DateTime.UtcNow
            };

            SentEmails.Add(emailMessage);

            _logger?.LogInformation("Mock email sent: To: {To}, Subject: {Subject}", to, subject);

            return Task.FromResult(true);
        }

        public Task<bool> SendEmailWithAttachmentsAsync(string to, string subject, string body, List<string> attachmentPaths, bool isHtml = false)
        {
            var emailMessage = new EmailMessage
            {
                To = to,
                Subject = subject,
                Body = body,
                IsHtml = isHtml,
                AttachmentPaths = attachmentPaths,
                SentAt = DateTime.UtcNow
            };

            SentEmails.Add(emailMessage);

            _logger?.LogInformation("Mock email with attachments sent: To: {To}, Subject: {Subject}, Attachments: {AttachmentCount}", 
                to, subject, attachmentPaths?.Count ?? 0);

            return Task.FromResult(true);
        }

        /// <summary>
        /// Clear the list of sent emails (useful for test setup)
        /// </summary>
        public void ClearSentEmails()
        {
            SentEmails.Clear();
        }
    }

    /// <summary>
    /// Represents an email message for tracking in the mock sender
    /// </summary>
    public class EmailMessage
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsHtml { get; set; }
        public List<string> AttachmentPaths { get; set; } = new List<string>();
        public DateTime SentAt { get; set; }
    }
}