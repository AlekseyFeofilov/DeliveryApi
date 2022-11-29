using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models.Dto;

public class DishPagedListDto
{
    [JsonPropertyName("dishes")] public IEnumerable<DishDto> Dishes { get; }
    [JsonPropertyName("pagination")] public PageInfoModel Pagination { get; }

    public DishPagedListDto(IEnumerable<DishDto> dishes, PageInfoModel pagination)
    {
        Dishes = dishes;
        Pagination = pagination;
    }
}