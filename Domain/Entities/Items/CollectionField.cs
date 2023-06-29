using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Items {

    public class CollectionField : Entity {

        public string CollectionId { get; set; } = null!;
        public string ItemFieldTypeId { get; set; } = null!;
        public string Name { get; set; } = null!;

        [ForeignKey(nameof(CollectionId))]
        public Collection Collection { get; set; } = null!;

        [ForeignKey(nameof(ItemFieldTypeId))]
        public ItemFieldType ItemFieldType { get; set; } = null!;
    }
}
