using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Identity;

namespace Domain.Entities.Items {

    public class Collection : Entity {

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }

        public string CollectionThemeId { get; set; } = null!;
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(CollectionThemeId))]
        public CollectionTheme Theme { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        public ICollection<Item>? Items { get; set; }
    }
}
