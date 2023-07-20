using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Models.Collection
{
    public class AddCollectionRequest : CollectionRequest
    {
        public string UserId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        
        public string CollectionThemeId { get; set; } = null!;
        public IFormFile? Image { get; set; } 
    }
}
