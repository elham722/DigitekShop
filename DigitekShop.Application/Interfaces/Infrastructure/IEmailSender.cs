using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitekShop.Application.Interfaces.Infrastructure
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = false);
        Task<bool> SendEmailAsync(IEnumerable<string> recipients, string subject, string body, bool isHtml = false);
        Task<bool> SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachment, string attachmentName, bool isHtml = false);
    }
}