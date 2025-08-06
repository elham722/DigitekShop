using System.Text.Json.Serialization;

namespace DigitekShop.Application.Responses.Analytics
{
    public class ActivityResponse : BaseResponse
    {
        [JsonPropertyName("activities")]
        public object? Activities { get; set; }

        [JsonPropertyName("totalCount")]
        public int? TotalCount { get; set; }

        [JsonPropertyName("pageNumber")]
        public int? PageNumber { get; set; }

        [JsonPropertyName("pageSize")]
        public int? PageSize { get; set; }

        [JsonPropertyName("activityType")]
        public string? ActivityType { get; set; }

        [JsonPropertyName("userId")]
        public string? UserId { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }

        public ActivityResponse(bool success, string message = "") : base(success, message)
        {
        }

        public static ActivityResponse CreateSuccess(object activities, string message = "Activities retrieved successfully")
        {
            return new ActivityResponse(true, message)
            {
                Activities = activities
            };
        }

        public static ActivityResponse CreatePagedSuccess(object activities, int totalCount, int pageNumber, int pageSize, string message = "Activities retrieved successfully")
        {
            return new ActivityResponse(true, message)
            {
                Activities = activities,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public static ActivityResponse CreateFiltered(object activities, string activityType, string userId, DateTime startDate, DateTime endDate, string message = "Filtered activities retrieved successfully")
        {
            return new ActivityResponse(true, message)
            {
                Activities = activities,
                ActivityType = activityType,
                UserId = userId,
                StartDate = startDate,
                EndDate = endDate
            };
        }

        public static ActivityResponse CreateError(string message, string? errorCode = null)
        {
            return new ActivityResponse(false, message);
        }
    }
} 