using System.Text.Json.Serialization;

namespace DigitekShop.Application.Responses
{
    public class CommandResponse : BaseResponse
    {
        [JsonPropertyName("operation")]
        public string Operation { get; set; } = string.Empty;

        [JsonPropertyName("affectedRows")]
        public int? AffectedRows { get; set; }

        public CommandResponse(string operation, string message = "Command executed successfully") 
            : base(true, message)
        {
            Operation = operation;
        }

        public static CommandResponse Create(string operation, string message = "Command executed successfully")
        {
            return new CommandResponse(operation, message);
        }
    }

    public class CommandResponse<T> : BaseResponse where T : class
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("operation")]
        public string Operation { get; set; } = string.Empty;

        [JsonPropertyName("affectedRows")]
        public int? AffectedRows { get; set; }

        public CommandResponse(string operation, string message = "Command executed successfully") 
            : base(true, message)
        {
            Operation = operation;
        }

        public CommandResponse(T data, string operation, string message = "Command executed successfully") 
            : base(true, message)
        {
            Data = data;
            Operation = operation;
        }

        public CommandResponse(int id, string operation, string message = "Command executed successfully") 
            : base(true, message)
        {
            Id = id;
            Operation = operation;
        }

        public CommandResponse(T data, int id, string operation, string message = "Command executed successfully") 
            : base(true, message)
        {
            Data = data;
            Id = id;
            Operation = operation;
        }

        public static CommandResponse<T> Create(string operation, string message = "Command executed successfully")
        {
            return new CommandResponse<T>(operation, message);
        }

        public static CommandResponse<T> CreateWithData(T data, string operation, string message = "Command executed successfully")
        {
            return new CommandResponse<T>(data, operation, message);
        }

        public static CommandResponse<T> CreateWithId(int id, string operation, string message = "Command executed successfully")
        {
            return new CommandResponse<T>(id, operation, message);
        }

        public static CommandResponse<T> CreateWithDataAndId(T data, int id, string operation, string message = "Command executed successfully")
        {
            return new CommandResponse<T>(data, id, operation, message);
        }
    }
} 