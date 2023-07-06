using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Item
{
    public class GetItemsResponse
    {
        public string UserId { get; set; } = null!;
        public IEnumerable<ItemDto> Items { get; set; } = new List<ItemDto>();
    }
}
