using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Items {

    public class ItemField {

        public string Value { get; set; } = null!;

        public string CollectionFieldId { get; set; } = null!;
        public string ItemId { get; set; } = null!;

        [ForeignKey(nameof(CollectionFieldId))]
        public CollectionField CollectionField { get; set; } = null!;

        [ForeignKey(nameof(ItemId))]
        public Item Item { get; set; } = null!;
    }
}
