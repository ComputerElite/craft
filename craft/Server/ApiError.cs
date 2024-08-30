using System.Text.Json;
using ComputerUtils.Webserver;

namespace craft.Server;

public class ApiError
{
    public string Message { get; set; }
    static readonly ApiError Unauthorized = new ApiError { Message = "Unauthorized" };
    
    public static void SendUnauthorized(ServerRequest request)
    {
        request.SendString(JsonSerializer.Serialize(ApiError.Unauthorized), "application/json", 403);
    }
    public static void SendMissingKey(string key, ServerRequest request)
    {
        request.SendString(JsonSerializer.Serialize(new ApiError { Message = $"Missing key: {key}" }), "application/json", 400);
    }

    public static void SendNotFound(ServerRequest request)
    {
        request.SendString(JsonSerializer.Serialize(new ApiError { Message = "Not found" }), "application/json", 404);
    }
}