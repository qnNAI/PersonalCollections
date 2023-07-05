using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Item;

namespace Application.Models.Collection
{
    public class CollectionDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public DateTime CreationDate { get; set; }

        public CollectionThemeDto Theme { get; set; } = null!;
        public AuthorDto Author { get; set; } = null!;

        public ICollection<ItemDto> Items { get; set; } = new List<ItemDto>();
    }
}
