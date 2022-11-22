using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models.Dto;

public class DishPagedListDto
{
    [JsonPropertyName("dishes")]
    public DishDto[]? Dishes { get; set; }
    [JsonPropertyName("pagination")]
    public PageInfoModel Pagination { get; set; }

    public DishPagedListDto(DishDto[]? dishes, PageInfoModel pagination)
    {
        Dishes = dishes;
        Pagination = pagination;
    }
}