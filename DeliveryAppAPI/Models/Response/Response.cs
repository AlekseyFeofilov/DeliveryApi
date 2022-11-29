using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models.Response;

public class Response
{
    [JsonPropertyName("status")]
    public string Status { get; }
    [JsonPropertyName("message")]
    public string Message { get; }

    public Response(string status, string message)
    {
        Status = status;
        Message = message;
    }
}