using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Items {

    public class CollectionFieldType : Entity {

        public string Name { get; set; } = null!;

        public ICollection<CollectionField> Fields { get; set; } = new List<CollectionField>();
    }
}
