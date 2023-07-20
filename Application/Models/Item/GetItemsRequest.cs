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
        public List<DateEntry> DateEntries { get; set; } = new List<DateEntry>();

        public class DateEntry
        {
            public string Id { get; set; } = null!;
            public string Value { get; set; } = null!;
        }
    }
}
 