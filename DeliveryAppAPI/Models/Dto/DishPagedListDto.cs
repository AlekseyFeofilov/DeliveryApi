using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models.Dto;

public class DishPagedListDto
{
    [JsonPropertyName("dishes")]
    public DishDto[]? Dishes;
    [JsonPropertyName("pagination")]
    public PageInfoModel Pagination;

    public DishPagedListDto(DishDto[]? dishes, PageInfoModel pagination)
    {
        Dishes = dishes;
        Pagination = pagination;
    }
}