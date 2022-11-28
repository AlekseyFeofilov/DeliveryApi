using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models.Dto;

public class DishPagedListDto
{
    [JsonPropertyName("dishes")]
    public IEnumerable<DishDto> Dishes { get; set; }
    [JsonPropertyName("pagination")]
    public PageInfoModel Pagination { get; set; }

    public DishPagedListDto(IEnumerable<DishDto> dishes, PageInfoModel pagination)
    {
        Dishes = dishes;
        Pagination = pagination;
    }
}