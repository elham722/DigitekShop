using System.ComponentModel.DataAnnotations;

namespace DigitekShop.Application.Models.Identity
{
    public class UpdatePreferencesDto
    {
        [StringLength(10)]
        public string? PreferredLanguage { get; set; }

        [StringLength(50)]
        public string? TimeZone { get; set; }

        public bool EmailNotifications { get; set; }

        public bool SmsNotifications { get; set; }

        public bool PushNotifications { get; set; }
    }
} 