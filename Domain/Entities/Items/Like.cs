using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Identity;

namespace Domain.Entities.Items
{
    public class Like
    {
        public string UserId { get; set; } = null!;
        public string ItemId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;
        public Item Item { get; set; } = null!;
    }
}
