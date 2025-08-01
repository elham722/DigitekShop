using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitekShop.Infrastructure.Email;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DigitekShop.Tests.Infrastructure.Email
{
    public class MockEmailSenderTests
    {
        [Fact]
        public async Task SendEmailAsync_ShouldAddEmailToSentEmails()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<MockEmailSender>>();
            var emailSender = new MockEmailSender(loggerMock.Object);
            string to = "test@example.com";
            string subject = "Test Subject";
            string body = "Test Body";

            // Act
            var result = await emailSender.SendEmailAsync(to, subject, body, false);

            // Assert
            Assert.True(result);
            Assert.Single(emailSender.SentEmails);
            Assert.Equal(to, emailSender.SentEmails[0].To);
            Assert.Equal(subject, emailSender.SentEmails[0].Subject);
            Assert.Equal(body, emailSender.SentEmails[0].Body);
            Assert.False(emailSender.SentEmails[0].IsHtml);
        }

        [Fact]
        public async Task SendEmailWithAttachmentsAsync_ShouldAddEmailWithAttachmentsToSentEmails()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<MockEmailSender>>();
            var emailSender = new MockEmailSender(loggerMock.Object);
            string to = "test@example.com";
            string subject = "Test Subject with Attachments";
            string body = "Test Body with Attachments";
            var attachments = new List<string> { "file1.pdf", "file2.jpg" };

            // Act
            var result = await emailSender.SendEmailWithAttachmentsAsync(to, subject, body, attachments, true);

            // Assert
            Assert.True(result);
            Assert.Single(emailSender.SentEmails);
            Assert.Equal(to, emailSender.SentEmails[0].To);
            Assert.Equal(subject, emailSender.SentEmails[0].Subject);
            Assert.Equal(body, emailSender.SentEmails[0].Body);
            Assert.True(emailSender.SentEmails[0].IsHtml);
            Assert.Equal(2, emailSender.SentEmails[0].AttachmentPaths.Count);
            Assert.Equal("file1.pdf", emailSender.SentEmails[0].AttachmentPaths[0]);
            Assert.Equal("file2.jpg", emailSender.SentEmails[0].AttachmentPaths[1]);
        }

        [Fact]
        public void ClearSentEmails_ShouldRemoveAllSentEmails()
        {
            // Arrange
            var emailSender = new MockEmailSender();
            emailSender.SentEmails.Add(new EmailMessage { To = "test1@example.com" });
            emailSender.SentEmails.Add(new EmailMessage { To = "test2@example.com" });
            Assert.Equal(2, emailSender.SentEmails.Count);

            // Act
            emailSender.ClearSentEmails();

            // Assert
            Assert.Empty(emailSender.SentEmails);
        }
    }
}