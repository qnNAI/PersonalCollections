using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Item
{
    public class ItemFieldDto
    {
        public string Value { get; set; } = null!;
        public string ItemId { get; set; } = null!;
        public string CollectionFieldId { get; set; } = null!;

        public CollectionFieldDto CollectionField { get; set; } = null!;
    }
}
