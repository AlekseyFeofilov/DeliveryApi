using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models;

public class PageInfoModel
{
    [JsonPropertyName("size")]
    public int Size;
    [JsonPropertyName("count")]
    public int Count;
    [JsonPropertyName("current")]
    public int Current;

    public PageInfoModel(int size, int count, int current)
    {
        Size = size;
        Count = count;
        Current = current;
    }
}