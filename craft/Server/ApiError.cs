using System.Text.Json;
using ComputerUtils.Webserver;

namespace craft.Server;

public class ApiError
{
    public string Message { get; set; }
    static readonly ApiError Unauthorized = new ApiError { Message = "Unauthorized" };
    static readonly ApiError Forbidden = new ApiError { Message = "Forbidden" };
    
    public static void SendUnauthorized(ServerRequest request)
    {
        request.SendString(JsonSerializer.Serialize(ApiError.Unauthorized), "application/json", 401);
    }
    public static void SendForbidden(ServerRequest request)
    {
        request.SendString(JsonSerializer.Serialize(ApiError.Forbidden), "application/json", 403);
    }
    public static void SendMissingKey(string key, ServerRequest request)
    {
        request.SendString(JsonSerializer.Serialize(new ApiError { Message = $"Missing key: {key}" }), "application/json", 400);
    }

    public static void SendNotFound(ServerRequest request)
    {
        request.SendString(JsonSerializer.Serialize(new ApiError { Message = "Not found" }), "application/json", 404);
    }

    public static void MalformedRequest(ServerRequest request, string msg = "Malformed request")
    {
        request.SendString(JsonSerializer.Serialize(new ApiError { Message = msg}), "application/json", 400);
    }
}