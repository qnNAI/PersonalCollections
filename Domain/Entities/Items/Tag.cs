using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Items {

    public class Tag : Entity {

        public string Name { get; set; } = null!;

        public ICollection<Item> Items { get; set; } = new List<Item>();
        public ICollection<ItemTag> ItemTags { get; set; } = new List<ItemTag>();
    }
}
