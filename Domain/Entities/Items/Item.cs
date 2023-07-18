using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Items {

    public class Item : Entity {

        public string Name { get; set; } = null!;
        public string CollectionId { get; set; } = null!;
        public DateTime CreationDate { get; set; }

        [ForeignKey(nameof(CollectionId))]
        public Collection Collection { get; set; } = null!;

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public ICollection<ItemTag> ItemTags { get; set; } = new List<ItemTag>();

        public ICollection<ItemField> Fields { get; set; } = new List<ItemField>();

        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
