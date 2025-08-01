using System.Text.Json.Serialization;

namespace DigitekShop.Application.Responses
{
    public class SuccessResponse<T> : BaseResponse where T : class
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }

        [JsonPropertyName("totalCount")]
        public int? TotalCount { get; set; }

        [JsonPropertyName("pageNumber")]
        public int? PageNumber { get; set; }

        [JsonPropertyName("pageSize")]
        public int? PageSize { get; set; }

        public SuccessResponse(T data, string message = "Operation completed successfully") 
            : base(true, message)
        {
            Data = data;
        }

        public SuccessResponse(T data, int totalCount, int pageNumber, int pageSize, string message = "Operation completed successfully") 
            : base(true, message)
        {
            Data = data;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public static SuccessResponse<T> Create(T data, string message = "Operation completed successfully")
        {
            return new SuccessResponse<T>(data, message);
        }

        public static SuccessResponse<T> CreatePaged(T data, int totalCount, int pageNumber, int pageSize, string message = "Operation completed successfully")
        {
            return new SuccessResponse<T>(data, totalCount, pageNumber, pageSize, message);
        }
    }
} 