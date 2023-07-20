using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Items
{
    public class ItemTag
    {
        public string ItemId { get; set; } = null!;
        public string TagId { get; set; } = null!;

        public Item Item { get; set; } = null!;
        public Tag Tag { get; set; } = null!;
    }
}
