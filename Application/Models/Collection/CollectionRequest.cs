using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Collection
{
    public abstract class CollectionRequest
    {
        public List<Field> Fields { get; set; } = new List<Field>();

        public class Field
        {
            public string Name { get; set; } = null!;
            public string TypeId { get; set; } = null!;
        }
    }
}
