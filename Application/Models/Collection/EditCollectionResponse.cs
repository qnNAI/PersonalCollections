using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Collection
{
    public class EditCollectionResponse
    {
        public bool Succeeded { get; set; }
        public ICollection<string> Errors { get; set; } = new List<string>();
    }
}
