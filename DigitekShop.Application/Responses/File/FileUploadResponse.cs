using System.Text.Json.Serialization;

namespace DigitekShop.Application.Responses.File
{
    public class FileUploadResponse : BaseResponse
    {
        [JsonPropertyName("file")]
        public object? File { get; set; }

        [JsonPropertyName("fileName")]
        public string? FileName { get; set; }

        [JsonPropertyName("fileSize")]
        public long? FileSize { get; set; }

        [JsonPropertyName("fileType")]
        public string? FileType { get; set; }

        [JsonPropertyName("fileUrl")]
        public string? FileUrl { get; set; }

        [JsonPropertyName("uploadedAt")]
        public DateTime? UploadedAt { get; set; }

        [JsonPropertyName("uploadedBy")]
        public string? UploadedBy { get; set; }

        public FileUploadResponse(bool success, string message = "") : base(success, message)
        {
        }

        public static FileUploadResponse CreateSuccess(object file, string fileName, long fileSize, string fileType, string fileUrl, string uploadedBy, string message = "File uploaded successfully")
        {
            return new FileUploadResponse(true, message)
            {
                File = file,
                FileName = fileName,
                FileSize = fileSize,
                FileType = fileType,
                FileUrl = fileUrl,
                UploadedAt = DateTime.UtcNow,
                UploadedBy = uploadedBy
            };
        }

        public static FileUploadResponse CreateError(string message, string? errorCode = null)
        {
            return new FileUploadResponse(false, message);
        }
    }
} 