using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models.Response;

public class Response
{
    [JsonPropertyName("status")]
    public string Status;
    [JsonPropertyName("message")]
    public string Message;

    public Response(string status, string message)
    {
        Status = status;
        Message = message;
    }
}