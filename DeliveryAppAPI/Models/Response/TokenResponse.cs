using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models.Response;

public class TokenResponse
{
    [JsonPropertyName("token")]
    public string Token { get; }

    public TokenResponse(string token)
    {
        Token = token;
    }
}