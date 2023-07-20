using Application.Models.Collection;
using Application.Models.Item;

namespace Application.Models.Common
{
    public class MainPageResponse
    {
        public IEnumerable<ItemDto> Items { get; set; } = new List<ItemDto>();
        public IEnumerable<CollectionDto> Collections { get; set; } = new List<CollectionDto>();
        public IEnumerable<TagDto> Tags { get; set; } = new List<TagDto>();
    }
}
