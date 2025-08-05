using System;
using System.Collections.Generic;

namespace DigitekShop.Application.DTOs.Identity
{
    public class RegistrationResponseDto
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public string? UserId { get; set; }
        public string? EmailConfirmationToken { get; set; }
        public bool RequiresEmailConfirmation { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();
    }
} 