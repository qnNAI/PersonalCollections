using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Collection;

namespace Application.Models.Item
{
    public class CommentDto
    {
        public string Id { get; set; }
        public string Text { get; set; } = null!;
        public DateTime SentTime { get; set; }

        public string UserId { get; set; } = null!;
        public string ItemId { get; set; } = null!;

        public AuthorDto Author { get; set; } = null!;
    }
}
