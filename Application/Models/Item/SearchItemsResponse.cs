using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Collection;

namespace Application.Models.Item
{
    public class SearchItemsResponse
    {
        public string Term { get; set; } = string.Empty;

        public IEnumerable<ItemDto> Items { get; set; } = new List<ItemDto>();
        public IEnumerable<CollectionDto> Collections { get; set; } = new List<CollectionDto>();
    }
}
