using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models;

public class PageInfoModel
{
    [JsonPropertyName("size")]
    public int Size { get; set; }
    [JsonPropertyName("count")]
    public int Count { get; set; }
    [JsonPropertyName("current")]
    public int Current { get; set; }

    public PageInfoModel(int size, int count, int current)
    {
        Size = size;
        Count = count;
        Current = current;
    }
}