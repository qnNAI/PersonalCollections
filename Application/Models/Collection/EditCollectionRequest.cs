using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Item;
using Microsoft.AspNetCore.Http;

namespace Application.Models.Collection
{
    public class EditCollectionRequest
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }

        public string CollectionThemeId { get; set; } = null!;
        public IFormFile? Image { get; set; }

        public List<CollectionFieldDto> Fields { get; set; } = new List<CollectionFieldDto>();
    }
}
