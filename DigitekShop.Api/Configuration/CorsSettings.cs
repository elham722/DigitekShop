namespace DigitekShop.Api.Configuration
{
    public class CorsSettings
    {
        public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
        public string[] AllowedMethods { get; set; } = Array.Empty<string>();
        public string[] AllowedHeaders { get; set; } = Array.Empty<string>();
        public bool AllowCredentials { get; set; } = true;
        public int MaxAge { get; set; } = 86400;
    }
} 