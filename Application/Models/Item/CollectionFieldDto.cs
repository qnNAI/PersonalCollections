using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Item
{
    public class CollectionFieldDto
    {
        public string Id { get; set; } = null!;
        public string CollectionId { get; set; } = null!;
        public string Name { get; set; } = null!;

        public FieldTypeDto FieldType { get; set; } = null!;
    }
}
