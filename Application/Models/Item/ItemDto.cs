using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Collection;

namespace Application.Models.Item {

    public class ItemDto {

        public string Name { get; set; } = null!;
        public string CollectionId { get; set; } = null!;

        public ICollection<ItemFieldDto> Fields { get; set; } = new List<ItemFieldDto>();
        public ICollection<TagDto> Tags { get; set; } = new List<TagDto>();
    }
}
