using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Item
{
    public class AddItemRequest
    {
        public string Name { get; set; } = null!;
        public string CollectionId { get; set; } = null!;

        public List<TagDto> Tags { get; set; } = new List<TagDto>();
        public List<ItemFieldDto> Fields { get; set; } = new List<ItemFieldDto>();
    }
}
