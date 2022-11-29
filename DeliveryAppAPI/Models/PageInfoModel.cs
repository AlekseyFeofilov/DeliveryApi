using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models;

public class PageInfoModel
{
    [JsonPropertyName("size")]
    public int Size { get; }
    [JsonPropertyName("count")]
    public int Count { get; }
    [JsonPropertyName("current")]
    public int Current { get; }

    public PageInfoModel(int size, int count, int current)
    {
        Size = size;
        Count = count;
        Current = current;
    }
}