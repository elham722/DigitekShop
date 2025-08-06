using System.Text.Json.Serialization;

namespace DigitekShop.Application.Responses.Email
{
    public class EmailResponse : BaseResponse
    {
        [JsonPropertyName("email")]
        public object? Email { get; set; }

        [JsonPropertyName("messageId")]
        public string? MessageId { get; set; }

        [JsonPropertyName("recipient")]
        public string? Recipient { get; set; }

        [JsonPropertyName("subject")]
        public string? Subject { get; set; }

        [JsonPropertyName("sentAt")]
        public DateTime? SentAt { get; set; }

        [JsonPropertyName("deliveryStatus")]
        public string? DeliveryStatus { get; set; }

        [JsonPropertyName("templateName")]
        public string? TemplateName { get; set; }

        public EmailResponse(bool success, string message = "") : base(success, message)
        {
        }

        public static EmailResponse CreateSuccess(object email, string messageId, string recipient, string subject, string templateName, string message = "Email sent successfully")
        {
            return new EmailResponse(true, message)
            {
                Email = email,
                MessageId = messageId,
                Recipient = recipient,
                Subject = subject,
                SentAt = DateTime.UtcNow,
                DeliveryStatus = "Sent",
                TemplateName = templateName
            };
        }

        public static EmailResponse CreateQueued(object email, string messageId, string recipient, string subject, string templateName, string message = "Email queued successfully")
        {
            return new EmailResponse(true, message)
            {
                Email = email,
                MessageId = messageId,
                Recipient = recipient,
                Subject = subject,
                SentAt = DateTime.UtcNow,
                DeliveryStatus = "Queued",
                TemplateName = templateName
            };
        }

        public static EmailResponse CreateError(string message, string? errorCode = null)
        {
            return new EmailResponse(false, message);
        }
    }
} 