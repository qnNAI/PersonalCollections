using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Item {

    public class GetItemsRequest 
    {
        public string CollectionId { get; set; } = null!;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Order { get; set; } = "asc";
        public string Filter { get; set; } = string.Empty;
    }
}
