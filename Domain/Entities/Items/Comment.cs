using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Identity;

namespace Domain.Entities.Items
{
    public class Comment : Entity
    {
        public string Text { get; set; } = null!;
        public DateTime SentTime { get; set; }

        public string UserId { get; set; } = null!;
        public string ItemId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;
        public Item Item { get; set; } = null!;
    }
}
