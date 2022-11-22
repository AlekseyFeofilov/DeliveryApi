using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models.Response;

public class TokenResponse
{
    [JsonPropertyName("token")]
    public string Token { get; set; }

    public TokenResponse(string token)
    {
        Token = token;
    }
}