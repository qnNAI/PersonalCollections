using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Collection
{
    public class CollectionsManagementResponse
    {
        public IEnumerable<CollectionDto> Collections { get; set; } = new List<CollectionDto>();
        public AuthorDto Author { get; set; } = null!;
    }
}
