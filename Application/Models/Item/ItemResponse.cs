using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Collection;

namespace Application.Models.Item
{
    public class ItemResponse
    {
        public ItemDto Item { get; set; } = null!;
        public CollectionDto Collection { get; set; } = null!;
        public int Likes { get; set; }
        public bool IsLiked { get; set; }
    }
}
