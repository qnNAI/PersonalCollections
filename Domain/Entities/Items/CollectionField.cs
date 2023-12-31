﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Items {

    public class CollectionField : Entity {

        public string CollectionId { get; set; } = null!;
        public string CollectionFieldTypeId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public long Order { get; set; }

        [ForeignKey(nameof(CollectionId))]
        public Collection Collection { get; set; } = null!;

        [ForeignKey(nameof(CollectionFieldTypeId))]
        public CollectionFieldType FieldType { get; set; } = null!;
        
        public ICollection<ItemField> ItemFields { get; set; } = new List<ItemField>();
    }
}
