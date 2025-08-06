using System.Text.Json.Serialization;

namespace DigitekShop.Application.Responses.Analytics
{
    public class AnalyticsResponse : BaseResponse
    {
        [JsonPropertyName("analytics")]
        public object? Analytics { get; set; }

        [JsonPropertyName("period")]
        public string? Period { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("metrics")]
        public Dictionary<string, object>? Metrics { get; set; }

        [JsonPropertyName("trends")]
        public object? Trends { get; set; }

        public AnalyticsResponse(bool success, string message = "") : base(success, message)
        {
        }

        public static AnalyticsResponse CreateSuccess(object analytics, string period, DateTime startDate, DateTime endDate, string message = "Analytics retrieved successfully")
        {
            return new AnalyticsResponse(true, message)
            {
                Analytics = analytics,
                Period = period,
                StartDate = startDate,
                EndDate = endDate
            };
        }

        public static AnalyticsResponse CreateWithMetrics(object analytics, Dictionary<string, object> metrics, string message = "Analytics with metrics retrieved successfully")
        {
            return new AnalyticsResponse(true, message)
            {
                Analytics = analytics,
                Metrics = metrics
            };
        }

        public static AnalyticsResponse CreateWithTrends(object analytics, object trends, string message = "Analytics with trends retrieved successfully")
        {
            return new AnalyticsResponse(true, message)
            {
                Analytics = analytics,
                Trends = trends
            };
        }

        public static AnalyticsResponse CreateError(string message, string? errorCode = null)
        {
            return new AnalyticsResponse(false, message);
        }
    }
} 